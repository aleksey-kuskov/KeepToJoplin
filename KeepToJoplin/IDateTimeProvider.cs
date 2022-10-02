using System;

namespace KeepToJoplin
{
	interface IDateTimeProvider
	{
		public DateTime UtcNow { get; }
	}
}