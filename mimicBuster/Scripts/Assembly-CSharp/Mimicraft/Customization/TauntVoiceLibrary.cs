using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Mimicraft.Voice;
using UnityEngine;

namespace Mimicraft.Customization
{
	public static class TauntVoiceLibrary
	{
		public readonly struct Entry
		{
			public readonly string Id;

			public readonly string Path;

			public readonly DateTime RecordedAt;

			public Entry(string id, string path, DateTime recordedAt)
			{
				Id = id;
				Path = path;
				RecordedAt = recordedAt;
			}
		}

		private const string FolderName = "TauntVoices";

		private const string Extension = ".voice";

		private const string SelectedKey = "Mimicraft.Taunt.Selected";

		public const int MaxEntries = 20;

		private const string ForcedSelectedKey = "Mimicraft.TauntVoice.ForcedSelected";

		public static string Folder => Path.Combine(Application.persistentDataPath, "TauntVoices");

		public static string SelectedId
		{
			get
			{
				return PlayerPrefs.GetString("Mimicraft.Taunt.Selected", "");
			}
			set
			{
				PlayerPrefs.SetString("Mimicraft.Taunt.Selected", value ?? "");
				PlayerPrefs.Save();
			}
		}

		public static string ForcedSelectedId
		{
			get
			{
				return PlayerPrefs.GetString("Mimicraft.TauntVoice.ForcedSelected", "");
			}
			set
			{
				PlayerPrefs.SetString("Mimicraft.TauntVoice.ForcedSelected", value ?? "");
				PlayerPrefs.Save();
			}
		}

		public static List<Entry> List()
		{
			List<Entry> list = new List<Entry>();
			try
			{
				Migrate();
				if (!Directory.Exists(Folder))
				{
					return list;
				}
				string[] files = Directory.GetFiles(Folder, "*.voice");
				foreach (string path in files)
				{
					list.Add(new Entry(Path.GetFileNameWithoutExtension(path), path, File.GetLastWriteTimeUtc(path)));
				}
				list.Sort((Entry a, Entry b) => b.RecordedAt.CompareTo(a.RecordedAt));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[TauntVoiceLibrary] Kutuphane okunamadi: " + ex.Message);
			}
			return list;
		}

		public static string Add(byte[] data)
		{
			if (data == null || data.Length == 0)
			{
				return "";
			}
			try
			{
				Directory.CreateDirectory(Folder);
				if (List().Count >= 20)
				{
					return "";
				}
				string text = NextId();
				File.WriteAllBytes(PathOf(text), data);
				SelectedId = text;
				return text;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[TauntVoiceLibrary] Kayit yazilamadi: " + ex.Message);
				return "";
			}
		}

		public static byte[] Read(string id)
		{
			try
			{
				string path = PathOf(id);
				if (string.IsNullOrEmpty(id) || !File.Exists(path))
				{
					return Array.Empty<byte>();
				}
				byte[] array = File.ReadAllBytes(path);
				if (TauntVoiceClip.Accepts(array, out var rejection))
				{
					return array;
				}
				Debug.LogWarning("[TauntVoiceLibrary] '" + id + "' kabul edilmedi (" + rejection + ") - yok sayiliyor.");
				return Array.Empty<byte>();
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[TauntVoiceLibrary] '" + id + "' okunamadi: " + ex.Message);
				return Array.Empty<byte>();
			}
		}

		public static byte[] SelectedBytes()
		{
			return Read(SelectedId);
		}

		public static byte[] ForcedSelectedBytes()
		{
			return Read(ForcedSelectedId);
		}

		public static void Delete(string id)
		{
			try
			{
				string path = PathOf(id);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				if (SelectedId == id)
				{
					SelectedId = "";
				}
				if (ForcedSelectedId == id)
				{
					ForcedSelectedId = "";
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[TauntVoiceLibrary] '" + id + "' silinemedi: " + ex.Message);
			}
		}

		private static string PathOf(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				return Path.Combine(Folder, id + ".voice");
			}
			return "";
		}

		private static string NextId()
		{
			for (int i = 1; i <= 21; i++)
			{
				string text = "Taunt " + i.ToString(CultureInfo.InvariantCulture);
				if (!File.Exists(PathOf(text)))
				{
					return text;
				}
			}
			return "Taunt " + DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture);
		}

		private static void Migrate()
		{
			string text = Path.Combine(Application.persistentDataPath, "taunt.voice");
			if (!File.Exists(text))
			{
				return;
			}
			try
			{
				Directory.CreateDirectory(Folder);
				string text2 = NextId();
				File.Move(text, PathOf(text2));
				if (string.IsNullOrEmpty(SelectedId))
				{
					SelectedId = text2;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[TauntVoiceLibrary] Eski kayit tasinamadi: " + ex.Message);
			}
		}
	}
}
