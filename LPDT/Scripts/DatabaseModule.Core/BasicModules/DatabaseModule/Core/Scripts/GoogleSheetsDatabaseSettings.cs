namespace BasicModules.DatabaseModule.Core.Scripts
{
	public class GoogleSheetsDatabaseSettings : IDataBaseSettings
	{
		private readonly string _connectionString;

		private readonly string _jsonKeyFullPath;

		public GoogleSheetsDatabaseSettings(string connectionString, string jsonKeyFullPath)
		{
			_connectionString = connectionString;
			_jsonKeyFullPath = jsonKeyFullPath;
		}

		public string GetConnectionString()
		{
			return _connectionString;
		}

		public string GetJsonFullPath()
		{
			return _jsonKeyFullPath;
		}
	}
}
