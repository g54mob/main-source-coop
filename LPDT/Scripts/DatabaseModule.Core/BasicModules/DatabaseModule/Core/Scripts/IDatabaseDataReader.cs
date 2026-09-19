using System;
using System.Collections.Generic;

namespace BasicModules.DatabaseModule.Core.Scripts
{
	public interface IDatabaseDataReader
	{
		void Initialize();

		Dictionary<Type, List<object>> GetAllTablesDataFromDatabase();

		List<TDataType> GetTableData<TDataType>() where TDataType : new();

		List<object> GetTableData(Type type);

		IList<IList<object>> GetTableMatrix(string tableName);

		IList<IList<object>> GetRawTableMatrix(string tableName);

		List<string> GetAllTableNames();

		List<string> GetAllEnumNames();
	}
}
