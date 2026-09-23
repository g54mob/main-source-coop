using System.Text;
using UnityEngine;

namespace Mimicraft.Networking
{
	public static class PlayerNameStore
	{
		private const string PrefsKey = "Mimicraft.PlayerName";

		private const string Fallback = "Oyuncu";

		public const int MaxLength = 16;

		public const int MaxBytes = 48;

		private static string cached;

		public static bool HasRealName => !string.IsNullOrEmpty(cached);

		public static string Get()
		{
			if (!string.IsNullOrEmpty(cached))
			{
				return cached;
			}
			string text = PlayerPrefs.GetString("Mimicraft.PlayerName", "");
			if (!string.IsNullOrWhiteSpace(text) && text != "Oyuncu")
			{
				return cached = Sanitize(text);
			}
			if (SteamManager.IsInitialized && !string.IsNullOrWhiteSpace(SteamManager.LocalName))
			{
				return cached = Sanitize(SteamManager.LocalName);
			}
			return "Oyuncu";
		}

		public static void Set(string name)
		{
			string text = Sanitize(name);
			if (!(text == "Oyuncu"))
			{
				cached = text;
				PlayerPrefs.SetString("Mimicraft.PlayerName", cached);
				PlayerPrefs.Save();
			}
		}

		public static string Sanitize(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return "Oyuncu";
			}
			string text = name.Trim();
			if (text.Length > 16)
			{
				text = text.Substring(0, 16);
			}
			return TruncateToBytes(text);
		}

		private static string TruncateToBytes(string value)
		{
			while (value.Length > 0 && Encoding.UTF8.GetByteCount(value) > 48)
			{
				value = value.Substring(0, value.Length - 1);
			}
			if (value.Length != 0)
			{
				return value;
			}
			return "Oyuncu";
		}
	}
}
