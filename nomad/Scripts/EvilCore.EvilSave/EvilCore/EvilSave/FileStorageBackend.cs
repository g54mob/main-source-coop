using System;
using System.IO;
using Cysharp.Threading.Tasks;

namespace EvilCore.EvilSave
{
	public class FileStorageBackend : IStorageBackend
	{
		public byte[] Read(string path)
		{
			if (!File.Exists(path))
			{
				return null;
			}
			return File.ReadAllBytes(path);
		}

		public void Write(string path, byte[] data)
		{
			string directoryName = Path.GetDirectoryName(path);
			if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			File.WriteAllBytes(path, data);
		}

		public void Delete(string path)
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}

		public bool Exists(string path)
		{
			return File.Exists(path);
		}

		public string[] ListFiles(string directory, string pattern = "*")
		{
			if (!Directory.Exists(directory))
			{
				return Array.Empty<string>();
			}
			return Directory.GetFiles(directory, pattern);
		}

		public string[] ListDirectories(string directory)
		{
			if (!Directory.Exists(directory))
			{
				return Array.Empty<string>();
			}
			return Directory.GetDirectories(directory);
		}

		public async UniTask<byte[]> ReadAsync(string path)
		{
			if (!File.Exists(path))
			{
				return null;
			}
			string localPath = path;
			return await UniTask.RunOnThreadPool(() => File.ReadAllBytes(localPath));
		}

		public async UniTask WriteAsync(string path, byte[] data)
		{
			string directoryName = Path.GetDirectoryName(path);
			if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			string localPath = path;
			byte[] localData = data;
			await UniTask.RunOnThreadPool(delegate
			{
				File.WriteAllBytes(localPath, localData);
			});
		}
	}
}
