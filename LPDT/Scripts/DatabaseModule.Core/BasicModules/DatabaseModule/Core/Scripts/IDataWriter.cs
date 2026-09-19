namespace BasicModules.DatabaseModule.Core.Scripts
{
	public interface IDataWriter
	{
		void WriteAllDataFromDatabaseToCsv(IDatabaseDataReader databaseDataReader, bool isWithNumberId);

		void WriteDataFromDatabaseToCsv(IDatabaseDataReader databaseDataReader, string tableName, bool isWithNumberId);
	}
}
