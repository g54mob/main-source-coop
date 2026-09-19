using System;
using System.Collections.Generic;
using System.Linq;

namespace Features.EnumHelpersModule.Scripts
{
	public class EnumValuesProvider : IEnumValuesProvider
	{
		public List<string> GetAllValuesAsStrings<T>()
		{
			return ((T[])Enum.GetValues(typeof(T))).Select((T x) => x.ToString()).ToList();
		}

		public List<T> GetAllValues<T>()
		{
			return ((T[])Enum.GetValues(typeof(T))).ToList();
		}
	}
}
