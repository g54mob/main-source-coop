using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mimicraft.Customization
{
	public static class CharacterSelection
	{
		private const string Key = "Mimicraft.SelectedCharacter";

		public static string SelectedPath
		{
			get
			{
				return PlayerPrefs.GetString("Mimicraft.SelectedCharacter", "");
			}
			set
			{
				PlayerPrefs.SetString("Mimicraft.SelectedCharacter", value ?? "");
				PlayerPrefs.Save();
			}
		}

		public static string Resolve()
		{
			string selectedPath = SelectedPath;
			if (!string.IsNullOrEmpty(selectedPath) && File.Exists(selectedPath))
			{
				return selectedPath;
			}
			List<CharacterListEntry> list = CharacterStorage.List();
			if (list.Count == 0)
			{
				return "";
			}
			SelectedPath = list[0].FilePath;
			return list[0].FilePath;
		}

		public static void ForgetIfSelected(string filePath)
		{
			if (!string.IsNullOrEmpty(filePath) && SelectedPath == filePath)
			{
				SelectedPath = "";
			}
		}
	}
}
