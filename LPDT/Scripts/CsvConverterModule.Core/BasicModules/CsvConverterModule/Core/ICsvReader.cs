using System;
using System.Collections.Generic;

namespace BasicModules.CsvConverterModule.Core
{
	public interface ICsvReader
	{
		void Initialize();

		List<TDataType> GetRecords<TDataType>() where TDataType : new();

		List<object> GetRecords(Type type);
	}
}
