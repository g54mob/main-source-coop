using Cysharp.Threading.Tasks;

namespace EvilCore.EvilSave
{
	public interface IStorageBackend
	{
		byte[] Read(string path);

		void Write(string path, byte[] data);

		void Delete(string path);

		bool Exists(string path);

		string[] ListFiles(string directory, string pattern = "*");

		string[] ListDirectories(string directory);

		UniTask<byte[]> ReadAsync(string path);

		UniTask WriteAsync(string path, byte[] data);
	}
}
