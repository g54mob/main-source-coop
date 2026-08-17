using System.IO;
using UnityEngine;

namespace EvilCore.Recording
{
	public static class DirectorPresetService
	{
		private const string FolderName = "DirectorMode";

		private const string FilePrefix = "DirectorPreset_";

		private const string FileExtension = ".json";

		public static string GetPresetsDirectory()
		{
			string text = Path.Combine(Application.persistentDataPath, "DirectorMode");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			return text;
		}

		public static void SavePreset(DirectorPresetData data, string fileName)
		{
			string presetsDirectory = GetPresetsDirectory();
			string text = SanitizeFileName(fileName);
			string path = Path.Combine(presetsDirectory, "DirectorPreset_" + text + ".json");
			string contents = JsonUtility.ToJson(data, prettyPrint: true);
			File.WriteAllText(path, contents);
		}

		public static DirectorPresetData LoadPreset(string fileName)
		{
			string presetsDirectory = GetPresetsDirectory();
			string text = SanitizeFileName(fileName);
			string path = Path.Combine(presetsDirectory, "DirectorPreset_" + text + ".json");
			if (!File.Exists(path))
			{
				return null;
			}
			return JsonUtility.FromJson<DirectorPresetData>(File.ReadAllText(path));
		}

		public static string[] GetAvailablePresets()
		{
			string[] files = Directory.GetFiles(GetPresetsDirectory(), "DirectorPreset_*.json");
			string[] array = new string[files.Length];
			for (int i = 0; i < files.Length; i++)
			{
				string text = Path.GetFileNameWithoutExtension(files[i]);
				if (text.StartsWith("DirectorPreset_"))
				{
					text = text.Substring("DirectorPreset_".Length);
				}
				array[i] = text;
			}
			return array;
		}

		public static void DeletePreset(string fileName)
		{
			string presetsDirectory = GetPresetsDirectory();
			string text = SanitizeFileName(fileName);
			string path = Path.Combine(presetsDirectory, "DirectorPreset_" + text + ".json");
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}

		private static string SanitizeFileName(string name)
		{
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			foreach (char oldChar in invalidFileNameChars)
			{
				name = name.Replace(oldChar, '_');
			}
			return name;
		}
	}
}
