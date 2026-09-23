using System;
using System.Collections.Generic;
using Mimicraft.Settings;
using Steamworks;
using UnityEngine;

namespace Mimicraft.Localization
{
	public static class StartupLanguage
	{
		private static readonly Dictionary<string, string> SteamNames = new Dictionary<string, string>
		{
			{ "turkish", "tr" },
			{ "english", "en" },
			{ "russian", "ru" },
			{ "schinese", "zh-Hans" },
			{ "tchinese", "zh-Hant" },
			{ "spanish", "es" },
			{ "latam", "es" },
			{ "brazilian", "pt-BR" },
			{ "portuguese", "pt-BR" },
			{ "german", "de" },
			{ "french", "fr" },
			{ "japanese", "ja" },
			{ "koreana", "ko" },
			{ "ukrainian", "uk" },
			{ "arabic", "ar" },
			{ "greek", "el" },
			{ "italian", "it" }
		};

		public const string Fallback = "en";

		public static string Detect(IReadOnlyList<string> available)
		{
			if (available == null || available.Count == 0)
			{
				return null;
			}
			return Match(FromSteam(), available) ?? Match(FromSystem(Application.systemLanguage), available) ?? Match("en", available);
		}

		public static string DetectExact(IReadOnlyList<string> available)
		{
			if (available == null || available.Count == 0)
			{
				return null;
			}
			return Match(FromSteam(), available) ?? Match(FromSystem(Application.systemLanguage), available);
		}

		private static string FromSteam()
		{
			try
			{
				if (!SteamClient.IsValid)
				{
					return null;
				}
				string gameLanguage = SteamApps.GameLanguage;
				if (string.IsNullOrWhiteSpace(gameLanguage))
				{
					return null;
				}
				string value;
				return SteamNames.TryGetValue(gameLanguage.Trim().ToLowerInvariant(), out value) ? value : null;
			}
			catch (Exception)
			{
				return null;
			}
		}

		private static string FromSystem(SystemLanguage language)
		{
			return language switch
			{
				SystemLanguage.Turkish => "tr", 
				SystemLanguage.English => "en", 
				SystemLanguage.Russian => "ru", 
				SystemLanguage.Chinese => "zh-Hans", 
				SystemLanguage.ChineseSimplified => "zh-Hans", 
				SystemLanguage.ChineseTraditional => "zh-Hant", 
				SystemLanguage.Spanish => "es", 
				SystemLanguage.Portuguese => "pt-BR", 
				SystemLanguage.German => "de", 
				SystemLanguage.French => "fr", 
				SystemLanguage.Japanese => "ja", 
				SystemLanguage.Korean => "ko", 
				SystemLanguage.Ukrainian => "uk", 
				SystemLanguage.Arabic => "ar", 
				SystemLanguage.Greek => "el", 
				SystemLanguage.Italian => "it", 
				_ => null, 
			};
		}

		public static void RepairMisdetected(IReadOnlyList<string> available)
		{
			if (available == null || available.Count == 0 || PlayerPrefs.GetInt("Mimicraft.Settings.LanguageRepaired", 0) == 1)
			{
				return;
			}
			PlayerPrefs.SetInt("Mimicraft.Settings.LanguageRepaired", 1);
			PlayerPrefs.Save();
			if (GameSettings.HasSavedLanguage && string.Equals(GameSettings.LanguageId, "tr", StringComparison.Ordinal))
			{
				string text = DetectExact(available);
				if (!string.IsNullOrEmpty(text) && !(text == "tr"))
				{
					Debug.Log("[Dil] Kayitli dil 'tr' ama bu makine '" + text + "' diyor - eski bir hatanin yazdigi deger temizlendi, dil yeniden tespit edilecek.");
					GameSettings.ForgetLanguage();
				}
			}
		}

		private static string Match(string wanted, IReadOnlyList<string> available)
		{
			if (string.IsNullOrEmpty(wanted))
			{
				return null;
			}
			foreach (string item in available)
			{
				if (string.Equals(item, wanted, StringComparison.OrdinalIgnoreCase))
				{
					return item;
				}
			}
			string b = Family(wanted);
			foreach (string item2 in available)
			{
				if (string.Equals(Family(item2), b, StringComparison.OrdinalIgnoreCase))
				{
					return item2;
				}
			}
			return null;
		}

		private static string Family(string code)
		{
			int num = code.IndexOf('-');
			if (num <= 0)
			{
				return code;
			}
			return code.Substring(0, num);
		}
	}
}
