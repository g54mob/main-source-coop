using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
	private static readonly string _saveFolder = Application.persistentDataPath + "/Saves/";

	private static readonly string _localFileName = "local";

	private static readonly string _extension = ".txt";

	private static readonly string _fullLocalSavePath = _saveFolder + _localFileName + _extension;

	public static void Init()
	{
		if (!Directory.Exists(_saveFolder))
		{
			Directory.CreateDirectory(_saveFolder);
		}
	}

	public static void SaveServer(string name, string saveString)
	{
		File.WriteAllText(_saveFolder + name + _extension, saveString);
	}

	public static void DeleteServer(string name)
	{
		string path = _saveFolder + name + _extension;
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}

	public static List<string> LoadAllServers()
	{
		FileInfo[] files = new DirectoryInfo(_saveFolder).GetFiles("*" + _extension);
		List<string> list = new List<string>();
		FileInfo[] array = files;
		foreach (FileInfo fileInfo in array)
		{
			if (!(fileInfo.Name == _localFileName + _extension))
			{
				string text = File.ReadAllText(fileInfo.FullName);
				if (string.IsNullOrWhiteSpace(text))
				{
					File.Delete(fileInfo.FullName);
				}
				else
				{
					list.Add(text);
				}
			}
		}
		return list;
	}

	public static void SaveLocal(string saveString)
	{
		File.WriteAllText(_saveFolder + _localFileName + _extension, saveString);
	}

	public static string LoadLocal()
	{
		if (File.Exists(_fullLocalSavePath))
		{
			return File.ReadAllText(_fullLocalSavePath);
		}
		return null;
	}
}
