using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace KeepToJoplin
{
	class KeepToJoplinConverter
	{
		readonly IIdProvider _idProvider;
		readonly IDateTimeProvider _dateTimeProvider;
		string _notebookId = "";
		string _inputDirectory = "";
		string _outputDirectory = "";
		string _resourcesDirectory = "";
		List<Label> _labels = new(0);

		public Action<string> OnLog = x => { };

		public KeepToJoplinConverter(IIdProvider idProvider, IDateTimeProvider dateTimeProvider)
		{
			_idProvider = idProvider;
			_dateTimeProvider = dateTimeProvider;
		}

		public void Convert(string inputDirectory, string outputDirectory)
		{
			if (_inputDirectory == null)
				throw new ArgumentNullException(nameof(inputDirectory));

			if (_outputDirectory == null)
				throw new ArgumentNullException(nameof(outputDirectory));

			_inputDirectory = inputDirectory;
			_outputDirectory = outputDirectory;
			_resourcesDirectory = Path.Combine(_outputDirectory, "resources");
			Directory.CreateDirectory(_resourcesDirectory);

			CreateJoplinNotebook();
			LoadLabels();
			CreateJoplinLabels();
			ConvertNotes();
		}

		void CreateJoplinNotebook()
		{
			_notebookId = _idProvider.GetNewId();
			var content = JoplinTemplate.GetNotebook(_notebookId, "Keep Notes", _dateTimeProvider.UtcNow.ToJson());
			WriteJoplinFile(_notebookId, content);
		}

		void WriteJoplinFile(string id, string content)
		{
			var fileName = Path.Combine(_outputDirectory, id + ".md");
			File.WriteAllText(fileName, content);
		}

		void LoadLabels()
		{
			var labelsFile = Path.Combine(_inputDirectory, "Labels.txt");

			if (!File.Exists(labelsFile))
				return;

			var labelNames = File.ReadAllLines(labelsFile);
			_labels = labelNames.Select(x => new Label(_idProvider.GetNewId(), x)).ToList();
		}

		void CreateJoplinLabels()
		{
			foreach (var label in _labels)
			{
				var content = JoplinTemplate.GetLabel(label.Id, label.Name, _dateTimeProvider.UtcNow.ToJson());
				WriteJoplinFile(label.Id, content);
			}
		}

		void ConvertNotes()
		{
			var noteFiles = Directory.GetFiles(_inputDirectory, "*.json");
			foreach (var noteFile in noteFiles)
			{
				ConvertNoteFile(noteFile);
			}
		}

		void ConvertNoteFile(string noteFile)
		{
			var noteFileName = Path.GetFileName(noteFile);
			
			OnLog($"File: {noteFile}");
			
			var keepNoteJson = File.ReadAllText(noteFile);

			var keepNote = JsonSerializer.Deserialize<KeepNote>(keepNoteJson);

			if (keepNote == null)
			{
				throw new FormatException($"Parsing json file \"{noteFileName}\"");
			}

			keepNote.SetId(_idProvider.GetNewId());

			CreateJoplinAttachments(keepNote, noteFileName);
			CreateJoplinNote(keepNote);
			CreateJoplinTags(keepNote);
		}

		void CreateJoplinAttachments(KeepNote keepNote, string noteFileName)
		{
			foreach (var attachment in keepNote.attachments)
			{
				var fileName = attachment.filePath;
				var fullFileName = Path.Combine(_inputDirectory, fileName);
				var fileInfo = new FileInfo(fullFileName);
				if (!fileInfo.Exists)
				{
					if (fileName.EndsWith(".jpeg"))
						fileName = fileName.Substring(0, fileName.Length - 5) + ".jpg";
					else if (fileName.EndsWith(".jpg"))
						fileName = fileName.Substring(0, fileName.Length - 4) + ".jpeg";
					fullFileName = Path.Combine(_inputDirectory, fileName);
					fileInfo = new FileInfo(fullFileName);
					if (fileInfo.Exists)
					{
						OnLog($"Info: attachment \"{attachment.filePath} changed to found file \"{fileName}\"");
						attachment.filePath = fileName;
					}
					else
					{
						OnLog($"Warning: attachment \"{attachment.filePath}\" in \"{noteFileName}\" does not exist. Skipping.");
						continue;
					}
				}
				var attachmentId = _idProvider.GetNewId();
				attachment.SetId(attachmentId);
				var fileExtension = fileInfo.Extension.Length <= 0 ? "" : fileInfo.Extension.Substring(1);
				var fileSize = fileInfo.Length;
				var createdDate = keepNote.GetCreatedDateTime().ToJson();
				var content = JoplinTemplate.GetAttachment(attachmentId, fileName, fileExtension, fileSize, attachment.mimetype, createdDate);
				WriteJoplinFile(attachmentId, content);
				File.Copy(fullFileName, Path.Combine(_resourcesDirectory, attachmentId + fileInfo.Extension));
			}
		}

		void CreateJoplinNote(KeepNote keepNote)
		{
			var noteId = keepNote.GetId();
			var createdDate = keepNote.GetCreatedDateTime().ToJson();
			var updatedDate = keepNote.GetEditedDateTime().ToJson();
			var text = keepNote.textContent;
			
			foreach (var attachment in keepNote.attachments)
			{
				text += $"\n![{attachment.filePath}](:/{attachment.GetId()})";
			}

			var content = JoplinTemplate.GetNote(noteId, _notebookId, keepNote.title, text, createdDate, updatedDate);
			
			WriteJoplinFile(noteId, content);
		}

		void CreateJoplinTags(KeepNote keepNote)
		{
			foreach (var label in keepNote.labels)
			{
				var labelId = _labels.SingleOrDefault(x=>x.Name == label.name)?.Id;
				if (labelId == null)
				{
					OnLog($"Warning: label \"{label.name}\" not found in list. Skipping.");
					continue;
				}
				var tagId = _idProvider.GetNewId();
				var noteId = keepNote.GetId();
				
				var date = keepNote.GetCreatedDateTime().ToJson();
				var content = JoplinTemplate.GetTag(tagId, noteId, labelId, date);
				
				WriteJoplinFile(tagId, content);
			}
		}
	}
}