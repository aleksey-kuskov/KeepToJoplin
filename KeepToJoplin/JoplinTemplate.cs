namespace KeepToJoplin
{
	static class JoplinTemplate
	{
		public static string GetNotebook(string notebookId, string notebookName, string jsonDate)
		{
			const string template = "{0}\n\nid: {1}\n" +
			                        "created_time: {2}\nupdated_time: {2}\nuser_created_time: {2}\nuser_updated_time: {2}\n" +
			                        "encryption_cipher_text: \nencryption_applied: 0\nparent_id: \nis_shared: 0\nshare_id: \nmaster_key_id: \nicon: \ntype_: 2";

			return string.Format(template, notebookName, notebookId, jsonDate);
		}

		public static string GetLabel(string labelId, string labelName, string jsonDate)
		{
			const string template = "{0}\n\nid: {1}\n" +
			                        "created_time: {2}\nupdated_time: {2}\nuser_created_time: {2}\nuser_updated_time: {2}\n" +
			                        "encryption_cipher_text: \nencryption_applied: 0\nis_shared: 0\nparent_id: \ntype_: 5";

			return string.Format(template, labelName, labelId, jsonDate);
		}

		public static string GetAttachment(string attachmentId, string fileName, string fileExtension, long fileSize, string mimeType, string jsonDate)
		{
			const string template = "{0}\n\nid: {1}\nmime: {2}\nfilename: \n" +
			                        "created_time: {3}\nupdated_time: {3}\nuser_created_time: {3}\nuser_updated_time: {3}\n" +
			                        "file_extension: {4}\n" +
			                        "encryption_cipher_text: \nencryption_applied: 0\nencryption_blob_encrypted: 0\n" +
			                        "size: {5}\nis_shared: 0\nshare_id: \nmaster_key_id: \ntype_: 4";

			return string.Format(template, fileName, attachmentId, mimeType, jsonDate, fileExtension, fileSize);
		}

		public static string GetNote(string noteId, string notebookId, string noteTitle, string noteText, string jsonDateCreated, string jsonDateUpdated)
		{
			const string template = "{0}\n\n{1}\n\n" +
			                        "id: {2}\nparent_id: {3}\n" +
			                        "created_time: {4}\n" +
			                        "updated_time: {5}\n" +
			                        "is_conflict: 0\n" +
			                        "latitude: 0\nlongitude: 0\naltitude: 0.0000\n" +
			                        "author: \nsource_url: \nis_todo: 0\ntodo_due: 0\ntodo_completed: 0\nsource: joplin-desktop\n" +
			                        "source_application: net.cozic.joplin-desktop\napplication_data: \norder: 0\n" +
			                        "user_created_time: {4}\nuser_updated_time: {5}\n" +
			                        "encryption_cipher_text: \nencryption_applied: 0\nmarkup_language: 1\nis_shared: 0\n" +
			                        "share_id: \nconflict_original_id: \nmaster_key_id: \ntype_: 1";

			return string.Format(template, noteTitle, noteText, noteId, notebookId, jsonDateCreated, jsonDateUpdated);
		}

		public static string GetTag(string tagId, string noteId, string labelId, string jsonDate)
		{
			const string template = "id: {0}\n" +
			                        "note_id: {1}\n" +
			                        "tag_id: {2}\n" +
			                        "created_time: {3}\nupdated_time: {3}\nuser_created_time: {3}\nuser_updated_time: {3}\n" +
			                        "encryption_cipher_text: \nencryption_applied: 0\nis_shared: 0\ntype_: 6";
			
			return string.Format(template, tagId, noteId, labelId, jsonDate);
		}
	}
}