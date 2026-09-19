using System;
using System.Collections.Generic;
using System.Linq;
using BasicModules.DatabaseModule.Core.Scripts;

namespace Global.Modules.Database_Module.Scripts
{
	public class OfflineDatabaseService : IDatabaseService
	{
		private readonly DatabaseBatchModel _databaseBatchModel;

		public OfflineDatabaseService(DatabaseBatchModel databaseBatchModel)
		{
			_databaseBatchModel = databaseBatchModel;
		}

		public List<TDataHolder> GetTableData<TDataHolder>() where TDataHolder : new()
		{
			return _databaseBatchModel.BatchedDatabaseDataByType[typeof(TDataHolder)].Select((object d) => (TDataHolder)d).ToList();
		}

		public void SetDataToTable<TDataHolder>(List<DatabaseSetDataHolder> databaseSetDataHolder)
		{
			throw new NotImplementedException();
		}

		public void SetDataToTable(string tableName, List<DatabaseSetDataHolder> databaseSetDataHolder)
		{
			throw new NotImplementedException();
		}
	}
}
