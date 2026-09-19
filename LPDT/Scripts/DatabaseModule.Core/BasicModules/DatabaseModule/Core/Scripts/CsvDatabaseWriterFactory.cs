using System.IO;
using BasicModules.CsvConverterModule.Core;

namespace BasicModules.DatabaseModule.Core.Scripts
{
	public class CsvDatabaseWriterFactory : ICsvDatabaseWriterFactory
	{
		public StreamWriter CreateStreamWriter(string path)
		{
			string directoryName = Path.GetDirectoryName(path);
			if (directoryName != null && !Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			return new StreamWriter(path);
		}

		public ICsvWriter CreateCsvWriter(StreamWriter streamWriter, CsvSettings csvSettings)
		{
			return new CsvWriter(streamWriter, csvSettings);
		}
	}
}
