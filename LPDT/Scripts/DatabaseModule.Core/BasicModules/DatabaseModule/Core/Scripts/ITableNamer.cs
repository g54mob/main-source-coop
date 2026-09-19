using System;

namespace BasicModules.DatabaseModule.Core.Scripts
{
	public interface ITableNamer
	{
		Type GetTypeByTableName(string tableName);

		string GetTableNameByType(Type type);

		string ConvertFromClassNameToTableName(string className);

		string ConvertFromTableNameToClassName(string className);
	}
}
