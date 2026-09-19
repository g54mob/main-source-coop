using System.Collections.Generic;

namespace Features.EnumHelpersModule.Scripts
{
	public interface IEnumValuesProvider
	{
		List<string> GetAllValuesAsStrings<T>();

		List<T> GetAllValues<T>();
	}
}
