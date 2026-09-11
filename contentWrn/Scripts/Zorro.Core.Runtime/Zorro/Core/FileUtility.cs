using System.IO;
using UnityEngine;

namespace Zorro.Core
{
	public static class FileUtility
	{
		public static void CopyFilesRecursively(string sourcePath, string targetPath)
		{
			string[] directories = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories);
			for (int i = 0; i < directories.Length; i++)
			{
				Directory.CreateDirectory(directories[i].Replace(sourcePath, targetPath));
			}
			directories = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories);
			foreach (string obj in directories)
			{
				File.Copy(obj, obj.Replace(sourcePath, targetPath), overwrite: true);
			}
		}

		public static void RemoveFilesContainingString(string directory, string stringToRemove)
		{
			string[] files = Directory.GetFiles(directory);
			foreach (string text in files)
			{
				if (text.Contains(stringToRemove))
				{
					File.Delete(text);
				}
			}
		}

		public static void RemoveDirectoriesContainingString(string directory, string stringToRemove)
		{
			string[] directories = Directory.GetDirectories(directory);
			foreach (string text in directories)
			{
				if (text.Contains(stringToRemove))
				{
					Directory.Delete(text, recursive: true);
				}
			}
		}

		public static void DeleteAllFilesWithExtension(string submodulePath, string extension, bool recursive = true)
		{
			string[] files = Directory.GetFiles(submodulePath, "*." + extension, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
			foreach (string text in files)
			{
				File.Delete(text);
				Debug.Log("Deleted file: " + text);
			}
		}
	}
}
