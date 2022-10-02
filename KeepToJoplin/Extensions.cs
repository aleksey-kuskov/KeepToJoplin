using System;
using System.Text.Json;

namespace KeepToJoplin
{
	static class Extensions
	{
		public static string ToJson(this DateTime dateTime)
		{
			var date = JsonSerializer.Serialize(dateTime);
			return date.Substring(1, date.Length - 2);
		}
	}
}