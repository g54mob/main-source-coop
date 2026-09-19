using System.Collections.Generic;

namespace BasicModules.DatabaseModule.Core.Scripts
{
	public interface IDatabaseDataWriter
	{
		void SetDataToDatabaseTable<TTable>(List<List<string>> csvData);

		void SetDataToDatabaseTable(string tableName, List<List<string>> csvData);
	}
}
