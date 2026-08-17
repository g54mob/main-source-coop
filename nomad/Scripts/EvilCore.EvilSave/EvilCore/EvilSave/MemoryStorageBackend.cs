using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace EvilCore.EvilSave
{
	public class MemoryStorageBackend : IStorageBackend
	{
		private readonly Dictionary<string, byte[]> _store = new Dictionary<string, byte[]>();

		public byte[] Read(string path)
		{
			if (!_store.TryGetValue(NormalizePath(path), out var value))
			{
				return null;
			}
			return value;
		}

		public void Write(string path, byte[] data)
		{
			_store[NormalizePath(path)] = data;
		}

		public void Delete(string path)
		{
			_store.Remove(NormalizePath(path));
		}

		public bool Exists(string path)
		{
			return _store.ContainsKey(NormalizePath(path));
		}

		public string[] ListFiles(string directory, string pattern = "*")
		{
			string dir = NormalizePath(directory);
			return _store.Keys.Where((string k) => k.StartsWith(dir)).ToArray();
		}

		public string[] ListDirectories(string directory)
		{
			string text = NormalizePath(directory);
			HashSet<string> hashSet = new HashSet<string>();
			foreach (string key in _store.Keys)
			{
				if (key.StartsWith(text))
				{
					string text2 = key.Substring(text.Length).TrimStart('/');
					int num = text2.IndexOf('/');
					if (num > 0)
					{
						hashSet.Add(text + "/" + text2.Substring(0, num));
					}
				}
			}
			return hashSet.ToArray();
		}

		public UniTask<byte[]> ReadAsync(string path)
		{
			return UniTask.FromResult(Read(path));
		}

		public UniTask WriteAsync(string path, byte[] data)
		{
			Write(path, data);
			return UniTask.CompletedTask;
		}

		public void Clear()
		{
			_store.Clear();
		}

		private static string NormalizePath(string path)
		{
			return path.Replace('\\', '/');
		}
	}
}
