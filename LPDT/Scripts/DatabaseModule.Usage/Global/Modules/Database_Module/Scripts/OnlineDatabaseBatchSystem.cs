using BasicModules.DatabaseModule.Core.Scripts;
using Zenject;

namespace Global.Modules.Database_Module.Scripts
{
	public class OnlineDatabaseBatchSystem : IInitializable
	{
		private readonly DatabaseBatchModel _databaseBatchModel;

		private readonly IDatabaseDataReader _databaseDataReader;

		public OnlineDatabaseBatchSystem(DatabaseBatchModel databaseBatchModel, IDatabaseDataReader databaseDataReader)
		{
			_databaseBatchModel = databaseBatchModel;
			_databaseDataReader = databaseDataReader;
		}

		public void Initialize()
		{
			_databaseBatchModel.BatchedDatabaseDataByType = _databaseDataReader.GetAllTablesDataFromDatabase();
		}
	}
}
