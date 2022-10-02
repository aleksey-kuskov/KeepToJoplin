using System;

namespace KeepToJoplin
{
	class ConvertError : Exception
	{
		public ConvertError(string message) : base(message)
		{
			
		}
	}
}