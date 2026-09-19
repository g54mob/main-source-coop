using System;

namespace Features.EnumHelpersModule.Scripts
{
	public class EnumParser : IEnumParser
	{
		public T Parse<T>(string value) where T : struct
		{
			return (T)Enum.Parse(typeof(T), value);
		}

		public bool TryParse<T>(string value, out T parsedEnum) where T : struct
		{
			return Enum.TryParse<T>(value, out parsedEnum);
		}
	}
}
