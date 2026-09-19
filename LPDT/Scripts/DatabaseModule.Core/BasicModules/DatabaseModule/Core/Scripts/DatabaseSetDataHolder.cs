namespace BasicModules.DatabaseModule.Core.Scripts
{
	public class DatabaseSetDataHolder
	{
		public DatabaseSettingDataType DatabaseSettingDataType { get; }

		public int Row { get; }

		public int Column { get; }

		public string Data { get; }

		public DatabaseSetDataHolder(int row, int column, string data)
		{
			Row = row;
			Column = column;
			Data = data;
		}

		public DatabaseSetDataHolder(DatabaseSettingDataType databaseSettingDataType, int column, string data)
		{
			DatabaseSettingDataType = databaseSettingDataType;
			Column = column;
			Data = data;
		}
	}
}
