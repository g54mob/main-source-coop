using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicModules.DatabaseModule.Core.Scripts
{
	public class DataWriter : IDataWriter
	{
		private readonly ICsvDatabaseConverter _csvDatabaseConverter;

		private readonly ITableNamer _tableNamer;

		public DataWriter(ICsvDatabaseConverter csvDatabaseConverter, ITableNamer tableNamer)
		{
			_csvDatabaseConverter = csvDatabaseConverter;
			_tableNamer = tableNamer;
		}

		public void WriteDataFromDatabaseToCsv(IDatabaseDataReader databaseDataReader, string tableName, bool isWithNumberId)
		{
			IList<IList<object>> tableMatrix = databaseDataReader.GetTableMatrix(tableName);
			if (!isWithNumberId)
			{
				for (int i = 0; i < tableMatrix.Count; i++)
				{
					tableMatrix[i].RemoveAt((i == 0) ? 1 : 0);
				}
			}
			IList<IList<string>> list = ((IEnumerable<IList<object>>)tableMatrix).Select((Func<IList<object>, IList<string>>)((IList<object> innerList) => innerList.Select((object item) => item?.ToString() ?? string.Empty).ToList())).ToList();
			for (int num = 0; num < list[0].Count; num++)
			{
				list[0][num] = ReplaceAdditionalIndex(list[0][num]);
			}
			_csvDatabaseConverter.FromDataToCsvFile(_tableNamer.ConvertFromTableNameToClassName(tableName), list);
		}

		private string ReplaceAdditionalIndex(string cell)
		{
			int num = cell.IndexOf('[');
			if (num == -1)
			{
				return cell;
			}
			int num2 = cell.IndexOf(']', num);
			if (num2 != -1)
			{
				string text = cell.Substring(0, num);
				string text2 = cell;
				int num3 = num2 + 1;
				cell = text + text2.Substring(num3, text2.Length - num3);
			}
			return cell;
		}

		public void WriteAllDataFromDatabaseToCsv(IDatabaseDataReader databaseDataReader, bool isWithNumberId)
		{
			foreach (string allTableName in databaseDataReader.GetAllTableNames())
			{
				WriteDataFromDatabaseToCsv(databaseDataReader, allTableName, isWithNumberId);
			}
		}
	}
}
