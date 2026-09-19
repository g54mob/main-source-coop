using System;
using RSG.Muffin.ApplicationFilesModule.Core.Scripts;

namespace BasicModules.DatabaseModule.Core.Scripts
{
	public class DataHoldersNamer : ITableNamer
	{
		private const string DATA_HOLDER = "DataHolder";

		private readonly IApplicationFilesHolder _applicationFilesHolder;

		public DataHoldersNamer(IApplicationFilesHolder applicationFilesHolder)
		{
			_applicationFilesHolder = applicationFilesHolder;
		}

		public Type GetTypeByTableName(string tableName)
		{
			return _applicationFilesHolder.GetTypeByName(tableName + "DataHolder");
		}

		public string GetTableNameByType(Type type)
		{
			return type.Name.Replace("DataHolder", "");
		}

		public string ConvertFromClassNameToTableName(string className)
		{
			return className.Replace("DataHolder", "");
		}

		public string ConvertFromTableNameToClassName(string className)
		{
			return className + "DataHolder";
		}
	}
}
