using System.Collections.Generic;
using System.Linq;
using BasicModules.DatabaseModule.Core.Scripts;

namespace Global.Modules.Database_Module.Scripts
{
	public class OnlineDatabaseService : IDatabaseService
	{
		private readonly DatabaseBatchModel _databaseBatchModel;

		private readonly ITableNamer _dataHoldersNamer;

		private readonly IDatabaseDataReader _databaseDataReader;

		private readonly IDatabaseDataWriter _databaseDataWriter;

		public OnlineDatabaseService(DatabaseBatchModel databaseBatchModel, ITableNamer dataHoldersNamer, IDatabaseDataReader databaseDataReader, IDatabaseDataWriter databaseDataWriter)
		{
			_databaseBatchModel = databaseBatchModel;
			_dataHoldersNamer = dataHoldersNamer;
			_databaseDataReader = databaseDataReader;
			_databaseDataWriter = databaseDataWriter;
		}

		public List<TDataHolder> GetTableData<TDataHolder>() where TDataHolder : new()
		{
			return _databaseBatchModel.BatchedDatabaseDataByType[typeof(TDataHolder)].Select((object d) => (TDataHolder)d).ToList();
		}

		public void SetDataToTable<TDataHolder>(List<DatabaseSetDataHolder> databaseSetDataHolder)
		{
			List<List<string>> list = new List<List<string>>();
			foreach (IList<object> item in _databaseDataReader.GetRawTableMatrix(_dataHoldersNamer.GetTableNameByType(typeof(TDataHolder))))
			{
				list.Add(item.Select((object s) => s.ToString()).ToList());
			}
			CreateCsvForSetting(databaseSetDataHolder, list);
			_databaseDataWriter.SetDataToDatabaseTable<TDataHolder>(list);
		}

		public void SetDataToTable(string tableName, List<DatabaseSetDataHolder> databaseSetDataHolder)
		{
			List<List<string>> list = new List<List<string>>();
			foreach (IList<object> item in _databaseDataReader.GetRawTableMatrix(tableName))
			{
				list.Add(item.Select((object s) => s.ToString()).ToList());
			}
			CreateCsvForSetting(databaseSetDataHolder, list);
			_databaseDataWriter.SetDataToDatabaseTable(tableName, list);
		}

		private static void CreateCsvForSetting(List<DatabaseSetDataHolder> databaseSetDataHolder, List<List<string>> csvData)
		{
			foreach (DatabaseSetDataHolder item in databaseSetDataHolder)
			{
				int num = item.Row;
				if (item.DatabaseSettingDataType == DatabaseSettingDataType.SetInLastRow)
				{
					num = csvData.Count;
				}
				else if (item.DatabaseSettingDataType == DatabaseSettingDataType.SetInPreLastRow)
				{
					num = csvData.Count - 1;
				}
				AddBracersIfNeeded(num, csvData);
				csvData[num][item.Column] = item.Data;
			}
		}

		private static void AddBracersIfNeeded(int rowIndex, List<List<string>> csvData)
		{
			int num = rowIndex - (csvData.Count - 1);
			if (num <= 0)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				List<string> list = new List<string>();
				for (int j = 0; j < csvData[0].Count; j++)
				{
					list.Add(string.Empty);
				}
				csvData.Add(list);
			}
		}
	}
}
