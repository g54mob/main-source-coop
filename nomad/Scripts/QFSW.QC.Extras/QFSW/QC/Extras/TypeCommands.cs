using System;
using System.Collections.Generic;

namespace QFSW.QC.Extras
{
	public static class TypeCommands
	{
		[Command("enum-info", "gets all of the numeric values and value names for the specified enum type.", Platform.AllPlatforms, MonoTargetType.Single)]
		private static IEnumerable<object> GetEnumInfo(Type enumType)
		{
			if (!enumType.IsEnum)
			{
				throw new ArgumentException($"Supplied type '{enumType}' must be an enum type");
			}
			Type enumInnerType = enumType.GetEnumUnderlyingType();
			Array vals = enumType.GetEnumValues();
			for (int i = 0; i < vals.Length; i++)
			{
				object value = vals.GetValue(i);
				object key = Convert.ChangeType(value, enumInnerType);
				KeyValuePair<object, object> keyValuePair = new KeyValuePair<object, object>(key, value);
				yield return keyValuePair;
			}
		}
	}
}
