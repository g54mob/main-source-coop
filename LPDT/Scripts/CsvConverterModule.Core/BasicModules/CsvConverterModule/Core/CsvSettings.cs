using UnityEngine;

namespace BasicModules.CsvConverterModule.Core
{
	public class CsvSettings
	{
		private readonly string _delimiter;

		private readonly string _tablesExportingPath;

		private readonly string _folderName;

		public CsvSettings(CsvConfiguration csvConfiguration, string folderName)
		{
			_delimiter = csvConfiguration.Delimiter;
			_tablesExportingPath = csvConfiguration.TablesExportingPath + folderName + "/";
			_folderName = folderName;
		}

		public string GetDelimiter()
		{
			return _delimiter;
		}

		public string GetCsvExportPath()
		{
			return Application.dataPath + _tablesExportingPath;
		}

		public string GetFolderName()
		{
			return _folderName;
		}
	}
}
