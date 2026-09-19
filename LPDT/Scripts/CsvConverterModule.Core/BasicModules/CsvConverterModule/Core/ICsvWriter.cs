using System.Collections.Generic;

namespace BasicModules.CsvConverterModule.Core
{
	public interface ICsvWriter
	{
		void CreateCsvFile(IList<IList<string>> table);
	}
}
