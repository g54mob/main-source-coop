using BasicModules.CsvConverterModule.Core;
using BasicModules.DatabaseModule.Core.Scripts;

namespace Global.Modules.Database_Module.Scripts.Configurations
{
	public class CsvDatabaseConverterFactory : ICsvDatabaseConverterFactory
	{
		private readonly ICsvDatabaseReaderFactory _csvDatabaseReaderFactory;

		private readonly ICsvDatabaseWriterFactory _csvDatabaseWriterFactory;

		public CsvDatabaseConverterFactory(ICsvDatabaseReaderFactory csvDatabaseReaderFactory, ICsvDatabaseWriterFactory csvDatabaseWriterFactory)
		{
			_csvDatabaseReaderFactory = csvDatabaseReaderFactory;
			_csvDatabaseWriterFactory = csvDatabaseWriterFactory;
		}

		public ICsvDatabaseConverter CreateCsvDatabaseConverter(CsvConfiguration csvConfiguration, string folderName)
		{
			return new CsvDatabaseConverter(new CsvSettings(csvConfiguration, folderName), _csvDatabaseReaderFactory, _csvDatabaseWriterFactory);
		}
	}
}
