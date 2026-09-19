using System;
using System.Collections.Generic;

namespace BasicModules.DatabaseModule.Core.Scripts
{
	public interface ICsvDatabaseConverter
	{
		List<TDataType> FromMatrixToActualData<TDataType>(IList<IList<object>> matrix) where TDataType : new();

		List<object> FromMatrixToActualData(IList<IList<object>> matrix, Type type);

		List<TDataType> GetTableData<TDataType>() where TDataType : new();

		List<TDataType> GetTableData<TDataType>(string tableName) where TDataType : new();

		Dictionary<Type, List<object>> GetAllTablesData(List<Type> types);

		void FromDataToCsvFile(string tableName, IList<IList<string>> table);
	}
}
