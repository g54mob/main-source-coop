using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BasicModules.CsvConverterModule.Core
{
	public class CsvWriter : ICsvWriter
	{
		private readonly StreamWriter _streamWriter;

		private readonly CsvSettings _csvSettings;

		public CsvWriter(StreamWriter streamWriter, CsvSettings csvSettings)
		{
			_streamWriter = streamWriter ?? throw new ArgumentNullException("streamWriter");
			_csvSettings = csvSettings ?? throw new ArgumentNullException("csvSettings");
		}

		public void CreateCsvFile(IList<IList<string>> table)
		{
			foreach (IList<string> item in table)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string item2 in item)
				{
					stringBuilder.Append(item2 + _csvSettings.GetDelimiter());
				}
				_streamWriter.Write(stringBuilder);
				_streamWriter.WriteLine();
			}
		}
	}
}
