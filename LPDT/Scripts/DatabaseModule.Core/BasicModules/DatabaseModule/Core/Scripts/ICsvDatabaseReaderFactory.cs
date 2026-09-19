using System.IO;
using BasicModules.CsvConverterModule.Core;

namespace BasicModules.DatabaseModule.Core.Scripts
{
	public interface ICsvDatabaseReaderFactory
	{
		StringReader CreateStringReader(string csvString);

		ICsvReader CreateCsvReader(StringReader stringReader, CsvSettings csvSettings);
	}
}
