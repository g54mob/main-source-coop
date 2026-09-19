using BasicModules.CsvConverterModule.Core;
using BasicModules.DatabaseModule.Core.Scripts;

namespace Global.Modules.Database_Module.Scripts.Configurations
{
	public interface ICsvDatabaseConverterFactory
	{
		ICsvDatabaseConverter CreateCsvDatabaseConverter(CsvConfiguration csvConfiguration, string folderName);
	}
}
