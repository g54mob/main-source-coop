using System.IO;
using System.Threading.Tasks;

namespace QFSW.QC.Extras
{
	public static class FileCommands
	{
		[Command("write-file", ~Platform.WebGLPlayer, MonoTargetType.Single)]
		[CommandDescription("Writes the provided data to a file at the provided path")]
		private static async Task WriteFile(string path, string data)
		{
			new FileInfo(path).Directory?.Create();
			using StreamWriter writer = new StreamWriter(path);
			await writer.WriteAsync(data);
		}

		[Command("read-file", ~Platform.WebGLPlayer, MonoTargetType.Single)]
		[CommandDescription("Reads the contents of the file at the provided path")]
		private static string ReadFile(string path)
		{
			using StreamReader streamReader = new StreamReader(path);
			return streamReader.ReadToEnd();
		}
	}
}
