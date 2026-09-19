using System.Collections.Generic;
using BasicModules.DatabaseModule.Core.Scripts;

namespace Global.Modules.Database_Module.Scripts
{
	public interface IDatabaseService
	{
		List<TDataHolder> GetTableData<TDataHolder>() where TDataHolder : new();

		void SetDataToTable<TDataHolder>(List<DatabaseSetDataHolder> databaseSetDataHolder);

		void SetDataToTable(string tableName, List<DatabaseSetDataHolder> databaseSetDataHolder);
	}
}
