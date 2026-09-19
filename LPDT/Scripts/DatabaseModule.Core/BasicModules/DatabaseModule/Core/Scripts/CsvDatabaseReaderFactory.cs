using System.IO;
using BasicModules.CsvConverterModule.Core;

namespace BasicModules.DatabaseModule.Core.Scripts
{
	public class CsvDatabaseReaderFactory : ICsvDatabaseReaderFactory
	{
		public StringReader CreateStringReader(string csvString)
		{
			return new StringReader(csvString);
		}

		public ICsvReader CreateCsvReader(StringReader stringReader, CsvSettings csvSettings)
		{
			CsvReader csvReader = new CsvReader(stringReader, csvSettings);
			((ICsvReader)csvReader).Initialize();
			return csvReader;
		}
	}
}
