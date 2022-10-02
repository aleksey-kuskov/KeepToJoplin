using System;

namespace KeepToJoplin
{
	class IdProvider : IIdProvider
	{
		public string GetNewId()
		{
			return Guid.NewGuid().ToString("N");
		}
	}
}