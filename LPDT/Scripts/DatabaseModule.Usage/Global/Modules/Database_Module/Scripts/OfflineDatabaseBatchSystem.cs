using BasicModules.DatabaseModule.Core.Scripts;
using Global.Modules.DatabaseModule.Scripts.Generated;
using Zenject;

namespace Global.Modules.Database_Module.Scripts
{
	public class OfflineDatabaseBatchSystem : IInitializable
	{
		private readonly DatabaseBatchModel _databaseBatchModel;

		private readonly DataHolderTypesMapper _dataHolderTypesModel;

		private readonly ICsvDatabaseConverter _csvDatabaseConverter;

		public OfflineDatabaseBatchSystem(DatabaseBatchModel databaseBatchModel, DataHolderTypesMapper dataHolderTypesModel, ICsvDatabaseConverter csvDatabaseConverter)
		{
			_databaseBatchModel = databaseBatchModel;
			_dataHolderTypesModel = dataHolderTypesModel;
			_csvDatabaseConverter = csvDatabaseConverter;
		}

		public void Initialize()
		{
			_databaseBatchModel.BatchedDatabaseDataByType = _csvDatabaseConverter.GetAllTablesData(_dataHolderTypesModel.GetAllTypes());
		}
	}
}
