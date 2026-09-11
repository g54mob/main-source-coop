using System;
using UnityEngine;

namespace Zorro.Settings
{
	public class DefaultSettingsSaveLoad : ISettingsSaveLoad
	{
		public bool TryLoadInt(Type type, out int value)
		{
			string fullName = type.FullName;
			if (PlayerPrefs.HasKey(fullName))
			{
				value = PlayerPrefs.GetInt(fullName);
				return true;
			}
			value = 0;
			return false;
		}

		public void SaveInt(Type type, int value)
		{
			PlayerPrefs.SetInt(type.FullName, value);
		}

		public void WriteToDisk()
		{
			PlayerPrefs.Save();
		}

		public bool TryLoadFloat(Type type, out float value)
		{
			string fullName = type.FullName;
			if (PlayerPrefs.HasKey(fullName))
			{
				value = PlayerPrefs.GetFloat(fullName);
				return true;
			}
			value = 0f;
			return false;
		}

		public void SaveFloat(Type type, float value)
		{
			PlayerPrefs.SetFloat(type.FullName, value);
		}

		public void SaveString(Type type, string value)
		{
			PlayerPrefs.SetString(type.FullName, value);
		}

		public bool TryGetString(Type type, out string value)
		{
			string fullName = type.FullName;
			if (PlayerPrefs.HasKey(fullName))
			{
				value = PlayerPrefs.GetString(fullName);
				return true;
			}
			value = "";
			return false;
		}

		public bool TryLoadBool(Type type, out bool value)
		{
			string fullName = type.FullName;
			if (PlayerPrefs.HasKey(fullName))
			{
				value = PlayerPrefs.GetInt(fullName) != 0;
				return true;
			}
			value = false;
			return false;
		}

		public void SaveBool(Type type, bool value)
		{
			PlayerPrefs.SetInt(type.FullName, value ? 1 : 0);
		}

		public bool TryLoadEnum<T>(Type type, out T value) where T : unmanaged, Enum
		{
			string fullName = type.FullName;
			if (PlayerPrefs.HasKey(fullName) && Enum.TryParse<T>(PlayerPrefs.GetString(fullName), out var result))
			{
				value = result;
				return true;
			}
			value = default(T);
			return false;
		}

		public void SaveEnum<T>(Type type, T value) where T : unmanaged, Enum
		{
		}

		public bool TryLoadEnum(Type type, out int[] values)
		{
			string fullName = type.FullName;
			if (PlayerPrefs.HasKey(fullName))
			{
				string text = PlayerPrefs.GetString(fullName);
				Debug.Log("Loading " + type?.ToString() + ": " + text);
				if (text.Contains('|'))
				{
					string[] array = text.Split('|');
					int[] array2 = new int[array.Length];
					for (int i = 0; i < array.Length; i++)
					{
						if (int.TryParse(array[i], out var result))
						{
							array2[i] = result;
							continue;
						}
						values = null;
						return false;
					}
					values = array2;
					return true;
				}
				if (int.TryParse(text, out var result2))
				{
					values = new int[1] { result2 };
					return true;
				}
			}
			else
			{
				Debug.LogWarning("No key found for " + fullName);
			}
			values = null;
			return false;
		}

		public void SaveEnum(Type type, int[] values)
		{
			if (values == null)
			{
				Debug.LogError("Tried to save null array.");
				return;
			}
			string text = string.Join("|", values);
			PlayerPrefs.SetString(type.FullName, text);
			Debug.Log($"Saving {type}: {text}");
		}
	}
}
