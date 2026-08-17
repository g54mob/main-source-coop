using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices.Utility
{
	public static class FileSystemUtility
	{
		public static async Task<(bool Success, string Result)> TryReadAllTextAsync(string filePath)
		{
			if (!(await ExistsInternalAsync(filePath, isDirectory: false)))
			{
				return (Success: false, Result: null);
			}
			string text = await ReadAllTextAsync(filePath);
			return (text == null) ? (Success: false, Result: null) : (Success: true, Result: text);
		}

		public static async Task<string> ReadAllTextAsync(string path)
		{
			_ = 1;
			try
			{
				string result;
				await using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					using StreamReader reader = new StreamReader(fileStream);
					result = await reader.ReadToEndAsync();
				}
				return result;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				throw;
			}
		}

		public static string ReadAllText(string path)
		{
			try
			{
				using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
				using StreamReader streamReader = new StreamReader(stream);
				return streamReader.ReadToEnd();
			}
			catch (IOException exception)
			{
				Debug.LogException(exception);
				throw;
			}
		}

		public static IEnumerable<string> GetFileSystemEntries(string path, string pattern, bool recursive = true)
		{
			SearchOption searchOption = (recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
			return Directory.EnumerateFileSystemEntries(path, pattern, searchOption);
		}

		public static string CombinePaths(params string[] paths)
		{
			return Path.Combine(paths);
		}

		public static string GetFullPath(string path)
		{
			return Path.GetFullPath(path);
		}

		public static string GetFileName(string path)
		{
			return Path.GetFileName(path);
		}

		public static void WriteFile(string filePath, string content, bool createDirectory = true)
		{
			Task.Run(() => WriteFileAsync(filePath, content, createDirectory)).GetAwaiter().GetResult();
		}

		public static async Task WriteFileAsync(string filePath, string content, bool createDirectory = true)
		{
			FileInfo fileInfo = new FileInfo(filePath);
			if (createDirectory && fileInfo.Directory != null)
			{
				CreateDirectory(fileInfo.Directory);
			}
			await using StreamWriter writer = new StreamWriter(filePath);
			await writer.WriteAsync(content);
		}

		private static void CreateDirectory(DirectoryInfo dInfo)
		{
			try
			{
				dInfo.Create();
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		public static bool DirectoryExists(string path)
		{
			return ExistsInternal(path, isDirectory: true);
		}

		public static bool FileExists(string path)
		{
			return ExistsInternal(path);
		}

		private static bool ExistsInternal(string path, bool isDirectory = false)
		{
			bool flag = false;
			if (isDirectory)
			{
				return Directory.Exists(path);
			}
			return File.Exists(path);
		}

		public static async Task<bool> DirectoryExistsAsync(string path)
		{
			return await ExistsInternalAsync(path, isDirectory: true);
		}

		public static async Task<bool> FileExistsAsync(string path)
		{
			return await ExistsInternalAsync(path, isDirectory: false);
		}

		private static async Task<bool> ExistsInternalAsync(string path, bool isDirectory)
		{
			bool result = ((!isDirectory) ? File.Exists(path) : Directory.Exists(path));
			return await Task.FromResult(result);
		}

		public static string GetProjectPath()
		{
			string text = CombinePaths(Directory.GetCurrentDirectory(), "Assets");
			if (DirectoryExists(text))
			{
				return Path.GetFullPath(CombinePaths(text, ".."));
			}
			throw new DirectoryNotFoundException("Unable to locate the Assets folder from the current directory.");
		}

		public static void NormalizePath(ref string path)
		{
			char oldChar = ((Path.DirectorySeparatorChar == '\\') ? '/' : '\\');
			path = path.Replace(oldChar, Path.DirectorySeparatorChar);
		}
	}
}
