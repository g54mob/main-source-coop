using BasicModules.DatabaseModule.Core.Scripts;
using Google.Apis.Sheets.v4;

namespace Global.Modules.Database_Module.Scripts.Configurations
{
	public class DatabaseDataManipulatorsFactory : IDatabaseDataManipulatorsFactory
	{
		private readonly ITableNamer _dataHoldersNamer;

		public DatabaseDataManipulatorsFactory(ITableNamer dataHoldersNamer)
		{
			_dataHoldersNamer = dataHoldersNamer;
		}

		public IDatabaseDataReader CreateGoogleSheetsDatabaseDataReader(ICsvDatabaseConverter csvDatabaseConverter, DatabasePickingDataConfiguration databasePickingDataConfiguration)
		{
			GoogleSheetsDatabaseSettings dataBaseSettings = new GoogleSheetsDatabaseSettings(databasePickingDataConfiguration.ConnectionString, databasePickingDataConfiguration.JsonKeyFullPath);
			SheetsService sheetsService = new GoogleSheetsConnection().ConnectToGoogleSheetsService(dataBaseSettings);
			GoogleSheetsDatabaseDataReader googleSheetsDatabaseDataReader = new GoogleSheetsDatabaseDataReader(dataBaseSettings, _dataHoldersNamer, csvDatabaseConverter, sheetsService);
			googleSheetsDatabaseDataReader.Initialize();
			return googleSheetsDatabaseDataReader;
		}

		public IDatabaseDataWriter CreateGoogleSheetsDatabaseDataWriter(DatabasePickingDataConfiguration databasePickingDataConfiguration)
		{
			GoogleSheetsDatabaseSettings dataBaseSettings = new GoogleSheetsDatabaseSettings(databasePickingDataConfiguration.ConnectionString, databasePickingDataConfiguration.JsonKeyFullPath);
			SheetsService sheetsService = new GoogleSheetsConnection().ConnectToGoogleSheetsService(dataBaseSettings);
			return new GoogleSheetsDatabaseDataWriter(dataBaseSettings, sheetsService, _dataHoldersNamer);
		}
	}
}
