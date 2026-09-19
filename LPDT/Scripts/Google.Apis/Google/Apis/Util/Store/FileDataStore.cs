using System;
using System.IO;
using System.Threading.Tasks;
using Google.Apis.Json;

namespace Google.Apis.Util.Store
{
	public class FileDataStore : IDataStore
	{
		private const string XdgDataHomeSubdirectory = "google-filedatastore";

		private static readonly Task CompletedTask = Task.FromResult(0);

		private readonly string folderPath;

		public string FolderPath => folderPath;

		public FileDataStore(string folder, bool fullPath = false)
		{
			folderPath = (fullPath ? folder : Path.Combine(GetHomeDirectory(), folder));
			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}
		}

		private string GetHomeDirectory()
		{
			string environmentVariable = Environment.GetEnvironmentVariable("APPDATA");
			if (!string.IsNullOrEmpty(environmentVariable))
			{
				return environmentVariable;
			}
			string environmentVariable2 = Environment.GetEnvironmentVariable("HOME");
			if (!string.IsNullOrEmpty(environmentVariable2))
			{
				string text = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
				if (string.IsNullOrEmpty(text))
				{
					text = Path.Combine(environmentVariable2, ".local", "share");
				}
				return Path.Combine(text, "google-filedatastore");
			}
			throw new PlatformNotSupportedException("Relative FileDataStore paths not supported on this platform.");
		}

		public Task StoreAsync<T>(string key, T value)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("Key MUST have a value");
			}
			string contents = NewtonsoftJsonSerializer.Instance.Serialize(value);
			File.WriteAllText(Path.Combine(folderPath, GenerateStoredKey(key, typeof(T))), contents);
			return CompletedTask;
		}

		public Task DeleteAsync<T>(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("Key MUST have a value");
			}
			string path = Path.Combine(folderPath, GenerateStoredKey(key, typeof(T)));
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return CompletedTask;
		}

		public Task<T> GetAsync<T>(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("Key MUST have a value");
			}
			TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
			string path = Path.Combine(folderPath, GenerateStoredKey(key, typeof(T)));
			if (File.Exists(path))
			{
				try
				{
					string input = File.ReadAllText(path);
					taskCompletionSource.SetResult(NewtonsoftJsonSerializer.Instance.Deserialize<T>(input));
				}
				catch (Exception exception)
				{
					taskCompletionSource.SetException(exception);
				}
			}
			else
			{
				taskCompletionSource.SetResult(default(T));
			}
			return taskCompletionSource.Task;
		}

		public Task ClearAsync()
		{
			if (Directory.Exists(folderPath))
			{
				Directory.Delete(folderPath, recursive: true);
				Directory.CreateDirectory(folderPath);
			}
			return CompletedTask;
		}

		public static string GenerateStoredKey(string key, Type t)
		{
			return $"{t.FullName}-{key}";
		}
	}
}
