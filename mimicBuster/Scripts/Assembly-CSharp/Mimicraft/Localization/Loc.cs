using System;
using System.Collections.Generic;
using Mimicraft.Settings;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Mimicraft.Localization
{
	public static class Loc
	{
		public static readonly string[] TableNames = new string[7] { "UI", "Settings", "Gameplay", "Editor", "Tutorial", "Gartic", "Content" };

		private static readonly Dictionary<string, string> tableOfKey = new Dictionary<string, string>(StringComparer.Ordinal);

		public const string SourceLanguage = "tr";

		private static bool syncing;

		private static bool hooked;

		private static readonly Dictionary<string, string> LanguageNames = new Dictionary<string, string>
		{
			{ "tr", "Türkçe" },
			{ "en", "English" },
			{ "ru", "Русский" },
			{ "zh-Hans", "简体中文" },
			{ "es", "Español" },
			{ "pt-BR", "Português (Brasil)" },
			{ "de", "Deutsch" },
			{ "fr", "Français" },
			{ "ja", "日本語" },
			{ "ko", "한국어" },
			{ "zh-Hant", "繁體中文" },
			{ "uk", "Українська" },
			{ "ar", "العربية" },
			{ "el", "Ελληνικά" },
			{ "it", "Italiano" }
		};

		public const string FallbackLanguage = "en";

		private static readonly HashSet<string> reportedMissing = new HashSet<string>(StringComparer.Ordinal);

		private static readonly HashSet<string> resolveWarnings = new HashSet<string>();

		private static bool chosen;

		public static string Language
		{
			get
			{
				EnsureHooked();
				Locale locale = (Ready ? LocalizationSettings.SelectedLocale : null);
				if (!(locale != null))
				{
					return "tr";
				}
				return locale.Identifier.Code;
			}
		}

		public static IReadOnlyList<string> Languages
		{
			get
			{
				EnsureHooked();
				List<string> list = new List<string>();
				if (!Ready || LocalizationSettings.AvailableLocales == null)
				{
					return list;
				}
				foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
				{
					if (locale != null)
					{
						list.Add(locale.Identifier.Code);
					}
				}
				return list;
			}
		}

		public static bool IsRightToLeft => IsRightToLeftLanguage(Language);

		private static Locale EnglishLocale
		{
			get
			{
				if (LocalizationSettings.AvailableLocales == null)
				{
					return null;
				}
				return LocalizationSettings.AvailableLocales.GetLocale("en");
			}
		}

		private static bool Ready => LocalizationSettings.HasSettings;

		public static event Action Changed;

		private static string Resolve(string key)
		{
			if (tableOfKey.TryGetValue(key, out var value))
			{
				return value;
			}
			string[] tableNames = TableNames;
			foreach (string text in tableNames)
			{
				StringTable table = LocalizationSettings.StringDatabase.GetTable(text);
				if (!(table == null) && !(table.SharedData == null) && table.SharedData.Contains(key))
				{
					tableOfKey[key] = text;
					return text;
				}
			}
			return null;
		}

		public static void ForgetTableIndex()
		{
			tableOfKey.Clear();
		}

		public static string LanguageName(string code)
		{
			if (string.IsNullOrEmpty(code))
			{
				return "";
			}
			if (!LanguageNames.TryGetValue(code, out var value))
			{
				return code;
			}
			return value;
		}

		public static bool IsRightToLeftLanguage(string code)
		{
			switch (code)
			{
			default:
				return code == "ur";
			case "ar":
			case "he":
			case "fa":
				return true;
			}
		}

		public static void SetLanguage(string code)
		{
			EnsureHooked();
			if (!Ready || string.IsNullOrEmpty(code) || code == Language)
			{
				return;
			}
			foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
			{
				if (!(locale == null) && !(locale.Identifier.Code != code))
				{
					chosen = !syncing;
					LocalizationSettings.SelectedLocale = locale;
					chosen = false;
					return;
				}
			}
			Debug.LogWarning("[Loc] '" + code + "' diye bir dil yok - projedeki Locale varliklarina bak.");
		}

		public static string Get(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return "";
			}
			if (TryGet(key, out var text))
			{
				return text;
			}
			WarnMissing(key);
			return "#" + key;
		}

		public static string Format(string key, params object[] args)
		{
			if (string.IsNullOrEmpty(key))
			{
				return "";
			}
			EnsureHooked();
			string text = (Ready ? Resolve(key) : null);
			if (text == null || !TryRead(text, key, args, out var text2))
			{
				WarnMissing(key);
				return "#" + key;
			}
			return text2;
		}

		private static bool TryRead(string table, string key, object[] args, out string text)
		{
			bool flag = TryReadFrom(table, key, args, null, out text);
			if (!flag)
			{
				Locale englishLocale = EnglishLocale;
				flag = englishLocale != null && TryReadFrom(table, key, args, englishLocale, out text);
			}
			if (flag)
			{
				text = Shortcuts.Fill(text);
			}
			return flag;
		}

		private static bool TryReadFrom(string table, string key, object[] args, Locale locale, out string text)
		{
			text = "";
			StringTable table2 = LocalizationSettings.StringDatabase.GetTable(table, locale);
			StringTableEntry stringTableEntry = ((table2 != null) ? table2.GetEntry(key) : null);
			if (stringTableEntry == null)
			{
				return false;
			}
			string text2 = ((args == null || args.Length == 0) ? stringTableEntry.GetLocalizedString() : stringTableEntry.GetLocalizedString(args));
			if (string.IsNullOrEmpty(text2))
			{
				return false;
			}
			text = text2;
			return true;
		}

		private static void WarnMissing(string key)
		{
			if (reportedMissing.Add(key))
			{
				Debug.LogWarning("[Loc] '" + key + "' hiçbir tabloda yok - İngilizcesi bile. Genellikle sebep: CSV'ye yeni anahtar eklendi ama tablolar güncellenmedi - Tools > Mimicraft > Yerelleştirme > Tabloları CSV'den güncelle.");
			}
		}

		public static bool TryGet(string key, out string text)
		{
			text = "";
			if (string.IsNullOrEmpty(key) || !Ready)
			{
				return false;
			}
			EnsureHooked();
			string text2 = Resolve(key);
			if (text2 != null)
			{
				return TryRead(text2, key, null, out text);
			}
			return false;
		}

		public static string Resolve(LocalizedString localized, string fallback)
		{
			if (localized == null || localized.IsEmpty || !Ready)
			{
				return fallback;
			}
			try
			{
				string localizedString = localized.GetLocalizedString();
				if (!string.IsNullOrEmpty(localizedString))
				{
					return localizedString;
				}
			}
			catch (Exception ex)
			{
				WarnOnce($"[Loc] '{localized}' cozulemedi ({ex.GetType().Name}) - yazili metne dusuldu.");
			}
			return fallback;
		}

		private static void WarnOnce(string message)
		{
			if (resolveWarnings.Add(message))
			{
				Debug.LogWarning(message);
			}
		}

		public static bool Has(string key)
		{
			if (string.IsNullOrEmpty(key) || !Ready)
			{
				return false;
			}
			return Resolve(key) != null;
		}

		public static void Reload()
		{
			ForgetTableIndex();
			Loc.Changed?.Invoke();
		}

		private static void OnBindingsChanged()
		{
			Loc.Changed?.Invoke();
		}

		private static void EnsureHooked()
		{
			if (!hooked && LocalizationSettings.HasSettings)
			{
				hooked = true;
				LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
				LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
				GameSettings.Changed -= OnSettingsChanged;
				GameSettings.Changed += OnSettingsChanged;
				GameInput.BindingsChanged -= OnBindingsChanged;
				GameInput.BindingsChanged += OnBindingsChanged;
				GameSettings.Load();
				ApplySavedLanguage();
			}
		}

		private static void OnSettingsChanged()
		{
			if (!syncing && !(GameSettings.LanguageId == Language) && GameSettings.HasSavedLanguage)
			{
				SetLanguage(GameSettings.LanguageId);
			}
		}

		private static void ApplySavedLanguage()
		{
			StartupLanguage.RepairMisdetected(Languages);
			string text = GameSettings.LanguageId;
			if (!GameSettings.HasSavedLanguage)
			{
				string text2 = StartupLanguage.Detect(Languages);
				if (!string.IsNullOrEmpty(text2))
				{
					text = text2;
					GameSettings.AdoptDetectedLanguage(text2);
				}
			}
			if (string.IsNullOrEmpty(text) || text == Language)
			{
				return;
			}
			syncing = true;
			try
			{
				SetLanguage(text);
			}
			finally
			{
				syncing = false;
			}
		}

		private static void OnLocaleChanged(Locale locale)
		{
			if (chosen && !syncing && locale != null)
			{
				GameSettings.SetLanguageId(locale.Identifier.Code);
			}
			Loc.Changed?.Invoke();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetOnPlay()
		{
			Loc.Changed = null;
			syncing = false;
			chosen = false;
			hooked = false;
			reportedMissing.Clear();
			LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
			GameSettings.Changed -= OnSettingsChanged;
		}
	}
}
