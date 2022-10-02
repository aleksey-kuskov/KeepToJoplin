using System;
using System.Collections.Generic;

namespace KeepToJoplin
{
	class KeepNote : WithPrivateId
	{
		static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public string title { get; set; } = "";
		public string textContent { get; set; } = "";
		public long userEditedTimestampUsec { get; set; }
		public long createdTimestampUsec { get; set; }
		public List<Label> labels { get; set; } = new();
		public List<Attachment> attachments { get; set; } = new();

		public class Label
		{
			public string name { get; set; } = "";
		}

		public class Attachment : WithPrivateId
		{
			public string filePath { get; set; } = "";
			public string mimetype { get; set; } = "";
		}
		
		public DateTime GetCreatedDateTime()
		{
			var milliseconds = createdTimestampUsec / 1000;
			return Epoch.AddMilliseconds(milliseconds);
		}

		public DateTime GetEditedDateTime()
		{
			var milliseconds = userEditedTimestampUsec / 1000;
			return Epoch.AddMilliseconds(milliseconds);
		}
	}
}