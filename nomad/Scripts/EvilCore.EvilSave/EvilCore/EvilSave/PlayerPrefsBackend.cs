using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.EvilSave
{
	public static class PlayerPrefsBackend
	{
		private const string KeyIndexKey = "__EvilSave_KeyIndex__";

		private static string Prefix => EvilSaveSettings.Instance.PlayerPrefsPrefix;

		private static string PrefixedKey(string key)
		{
			return Prefix + key;
		}

		public static void SetString(string key, string value)
		{
			PlayerPrefs.SetString(PrefixedKey(key), value);
			TrackKey(key);
		}

		public static string GetString(string key, string defaultValue = "")
		{
			return PlayerPrefs.GetString(PrefixedKey(key), defaultValue);
		}

		public static void SetInt(string key, int value)
		{
			PlayerPrefs.SetInt(PrefixedKey(key), value);
			TrackKey(key);
		}

		public static int GetInt(string key, int defaultValue = 0)
		{
			return PlayerPrefs.GetInt(PrefixedKey(key), defaultValue);
		}

		public static void SetFloat(string key, float value)
		{
			PlayerPrefs.SetFloat(PrefixedKey(key), value);
			TrackKey(key);
		}

		public static float GetFloat(string key, float defaultValue = 0f)
		{
			return PlayerPrefs.GetFloat(PrefixedKey(key), defaultValue);
		}

		public static void SetBool(string key, bool value)
		{
			PlayerPrefs.SetInt(PrefixedKey(key), value ? 1 : 0);
			TrackKey(key);
		}

		public static bool GetBool(string key, bool defaultValue = false)
		{
			return PlayerPrefs.GetInt(PrefixedKey(key), defaultValue ? 1 : 0) == 1;
		}

		public static void SetObject<T>(string key, T value)
		{
			string value2 = JsonUtility.ToJson(value);
			PlayerPrefs.SetString(PrefixedKey(key), value2);
			TrackKey(key);
		}

		public static T GetObject<T>(string key, T defaultValue = default(T))
		{
			string key2 = PrefixedKey(key);
			if (!PlayerPrefs.HasKey(key2))
			{
				return defaultValue;
			}
			string text = PlayerPrefs.GetString(key2);
			if (string.IsNullOrEmpty(text))
			{
				return defaultValue;
			}
			return JsonUtility.FromJson<T>(text);
		}

		public static bool HasKey(string key)
		{
			return PlayerPrefs.HasKey(PrefixedKey(key));
		}

		public static void DeleteKey(string key)
		{
			PlayerPrefs.DeleteKey(PrefixedKey(key));
			UntrackKey(key);
		}

		public static void DeleteAll()
		{
			string[] allKeys = GetAllKeys();
			for (int i = 0; i < allKeys.Length; i++)
			{
				PlayerPrefs.DeleteKey(PrefixedKey(allKeys[i]));
			}
			PlayerPrefs.DeleteKey("__EvilSave_KeyIndex__");
		}

		public static string[] GetAllKeys()
		{
			string text = PlayerPrefs.GetString("__EvilSave_KeyIndex__", "");
			if (string.IsNullOrEmpty(text))
			{
				return Array.Empty<string>();
			}
			string[] array = text.Split('\n');
			List<string> list = new List<string>();
			string[] array2 = array;
			foreach (string text2 in array2)
			{
				if (!string.IsNullOrEmpty(text2) && PlayerPrefs.HasKey(PrefixedKey(text2)))
				{
					list.Add(text2);
				}
			}
			return list.ToArray();
		}

		public static void Save()
		{
			PlayerPrefs.Save();
		}

		private static void TrackKey(string key)
		{
			string text = PlayerPrefs.GetString("__EvilSave_KeyIndex__", "");
			HashSet<string> hashSet = new HashSet<string>(string.IsNullOrEmpty(text) ? Array.Empty<string>() : text.Split('\n'));
			if (hashSet.Add(key))
			{
				hashSet.Remove("");
				PlayerPrefs.SetString("__EvilSave_KeyIndex__", string.Join("\n", hashSet));
			}
		}

		private static void UntrackKey(string key)
		{
			string text = PlayerPrefs.GetString("__EvilSave_KeyIndex__", "");
			if (!string.IsNullOrEmpty(text))
			{
				HashSet<string> hashSet = new HashSet<string>(text.Split('\n'));
				if (hashSet.Remove(key))
				{
					hashSet.Remove("");
					PlayerPrefs.SetString("__EvilSave_KeyIndex__", string.Join("\n", hashSet));
				}
			}
		}
	}
}
