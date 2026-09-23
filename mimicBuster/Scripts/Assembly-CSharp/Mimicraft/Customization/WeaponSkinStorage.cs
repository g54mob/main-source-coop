using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Mimicraft.Customization
{
	public static class WeaponSkinStorage
	{
		private const string FolderName = "WeaponSkins";

		public const string LegacyName = "Kaydedilmiş";

		private const string SelectionPrefix = "Mimicraft.WeaponSkin.";

		private static string Directory_ => Path.Combine(Application.persistentDataPath, "WeaponSkins");

		public static bool Exists(string weaponId)
		{
			if (!string.IsNullOrWhiteSpace(weaponId))
			{
				return File.Exists(PathFor(weaponId));
			}
			return false;
		}

		public static WeaponSkinData Load(string weaponId)
		{
			if (string.IsNullOrWhiteSpace(weaponId))
			{
				return null;
			}
			string text = PathFor(weaponId);
			if (!File.Exists(text))
			{
				return null;
			}
			byte[] bytes;
			try
			{
				bytes = File.ReadAllBytes(text);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[WeaponSkinStorage] '" + text + "' okunamadi: " + ex.Message);
				return null;
			}
			if (!WeaponSkinFile.TryDecode(bytes, out var skins))
			{
				Debug.LogWarning("[WeaponSkinStorage] '" + text + "' bozuk ya da bu build'in anlamadigi bir surumde - o silahin modeli yuklenmeyecek.");
				return null;
			}
			if (skins.TryGetValue(weaponId, out var value))
			{
				return value;
			}
			using (Dictionary<string, WeaponSkinData>.Enumerator enumerator = skins.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current.Value;
				}
			}
			return null;
		}

		public static Dictionary<string, WeaponSkinData> LoadAll()
		{
			Dictionary<string, WeaponSkinData> dictionary = new Dictionary<string, WeaponSkinData>();
			if (!Directory.Exists(Directory_))
			{
				return dictionary;
			}
			foreach (WeaponSkinListEntry item in List())
			{
				if (!dictionary.ContainsKey(item.WeaponId) && IsWorn(item, SelectedPath(item.WeaponId)))
				{
					string weaponId;
					string skinName;
					WeaponSkinData weaponSkinData = LoadFile(item.FilePath, out weaponId, out skinName);
					if (weaponSkinData != null)
					{
						dictionary[item.WeaponId] = weaponSkinData;
					}
				}
			}
			return dictionary;
		}

		private static bool IsWorn(WeaponSkinListEntry entry, string selected)
		{
			if (!string.IsNullOrEmpty(selected))
			{
				return entry.FilePath == selected;
			}
			return entry.IsLegacy;
		}

		public static string WornPath(string weaponId)
		{
			if (string.IsNullOrWhiteSpace(weaponId))
			{
				return "";
			}
			string selected = SelectedPath(weaponId);
			foreach (WeaponSkinListEntry item in List(weaponId))
			{
				if (IsWorn(item, selected))
				{
					return item.FilePath;
				}
			}
			return "";
		}

		public static void Save(string weaponId, WeaponSkinData skin)
		{
			if (string.IsNullOrWhiteSpace(weaponId) || skin == null)
			{
				return;
			}
			string text = PathFor(weaponId);
			try
			{
				if (!Directory.Exists(Directory_))
				{
					Directory.CreateDirectory(Directory_);
				}
				Dictionary<string, WeaponSkinData> skins = new Dictionary<string, WeaponSkinData> { [weaponId] = skin };
				File.WriteAllBytes(text, WeaponSkinFile.Encode(skins));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[WeaponSkinStorage] '" + text + "' yazilamadi: " + ex.Message);
			}
		}

		public static void Delete(string weaponId)
		{
			if (!string.IsNullOrWhiteSpace(weaponId))
			{
				string path = PathFor(weaponId);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
		}

		private static string PathFor(string weaponId)
		{
			StringBuilder stringBuilder = new StringBuilder(weaponId.Length);
			foreach (char c in weaponId)
			{
				stringBuilder.Append((char.IsLetterOrDigit(c) || c == '-' || c == '_') ? c : '_');
			}
			return Path.Combine(Directory_, stringBuilder?.ToString() + ".weapons");
		}

		public static List<WeaponSkinListEntry> List(string weaponId = null)
		{
			List<WeaponSkinListEntry> list = new List<WeaponSkinListEntry>();
			if (!Directory.Exists(Directory_))
			{
				return list;
			}
			string[] files = Directory.GetFiles(Directory_, "*.weaponskin");
			foreach (string text in files)
			{
				if (WeaponSkinEntryFile.TryReadHeader(text, out var weaponId2, out var skinName) && (weaponId == null || !(weaponId2 != weaponId)))
				{
					list.Add(new WeaponSkinListEntry(text, weaponId2, skinName, legacy: false));
				}
			}
			files = Directory.GetFiles(Directory_, "*.weapons");
			foreach (string text2 in files)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text2);
				if (weaponId == null || !(fileNameWithoutExtension != weaponId))
				{
					list.Add(new WeaponSkinListEntry(text2, fileNameWithoutExtension, "Kaydedilmiş", legacy: true));
				}
			}
			list.Sort((WeaponSkinListEntry a, WeaponSkinListEntry b) => string.Compare(a.SkinName, b.SkinName, StringComparison.OrdinalIgnoreCase));
			return list;
		}

		public static WeaponSkinData LoadFile(string filePath, out string weaponId, out string skinName)
		{
			weaponId = "";
			skinName = "";
			if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
			{
				return null;
			}
			try
			{
				WeaponSkinData data;
				if (filePath.EndsWith(".weaponskin", StringComparison.OrdinalIgnoreCase))
				{
					return WeaponSkinEntryFile.TryDecode(File.ReadAllBytes(filePath), out weaponId, out skinName, out data) ? data : null;
				}
				weaponId = Path.GetFileNameWithoutExtension(filePath);
				skinName = "Kaydedilmiş";
				return Load(weaponId);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[WeaponSkinStorage] '" + filePath + "' okunamadi: " + ex.Message);
				return null;
			}
		}

		public static string SaveFile(string filePath, string weaponId, string skinName, WeaponSkinData skin)
		{
			if (string.IsNullOrWhiteSpace(weaponId) || skin == null)
			{
				return filePath;
			}
			try
			{
				if (!Directory.Exists(Directory_))
				{
					Directory.CreateDirectory(Directory_);
				}
				if (string.IsNullOrEmpty(filePath) || !filePath.EndsWith(".weaponskin", StringComparison.OrdinalIgnoreCase))
				{
					filePath = Path.Combine(Directory_, string.Format("{0:N}{1}", Guid.NewGuid(), ".weaponskin"));
				}
				File.WriteAllBytes(filePath, WeaponSkinEntryFile.Encode(weaponId, skinName, skin));
				return filePath;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[WeaponSkinStorage] '" + filePath + "' yazilamadi: " + ex.Message);
				return filePath;
			}
		}

		public static void DeleteFile(string filePath)
		{
			if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
			{
				if (WeaponSkinEntryFile.TryReadHeader(filePath, out var weaponId, out var _) && SelectedPath(weaponId) == filePath)
				{
					Select(weaponId, null);
				}
				File.Delete(filePath);
			}
		}

		public static string SelectedPath(string weaponId)
		{
			if (!string.IsNullOrWhiteSpace(weaponId))
			{
				return PlayerPrefs.GetString("Mimicraft.WeaponSkin." + weaponId, "");
			}
			return "";
		}

		public static void Select(string weaponId, string filePath)
		{
			if (!string.IsNullOrWhiteSpace(weaponId))
			{
				PlayerPrefs.SetString("Mimicraft.WeaponSkin." + weaponId, filePath ?? "");
				PlayerPrefs.Save();
			}
		}
	}
}
