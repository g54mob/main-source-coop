using Google.Apis.Sheets.v4;

namespace BasicModules.DatabaseModule.Core.Scripts
{
	public interface IGoogleSheetsConnection
	{
		SheetsService ConnectToGoogleSheetsService(IDataBaseSettings dataBaseSettings);

		string GetSheetCurrentVersion(IDataBaseSettings dataBaseSettings);
	}
}
