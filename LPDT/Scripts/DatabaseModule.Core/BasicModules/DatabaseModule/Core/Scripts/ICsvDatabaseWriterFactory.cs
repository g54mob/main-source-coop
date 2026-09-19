using System.IO;
using BasicModules.CsvConverterModule.Core;

namespace BasicModules.DatabaseModule.Core.Scripts
{
	public interface ICsvDatabaseWriterFactory
	{
		StreamWriter CreateStreamWriter(string path);

		ICsvWriter CreateCsvWriter(StreamWriter streamWriter, CsvSettings csvSettings);
	}
}
