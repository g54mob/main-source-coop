using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace EvilCore.Localization
{
	public class LocalizationService : MonoBehaviour, ILocalizationService
	{
		[SerializeField]
		private LocalizationConfig config;

		private StringResolver _resolver;

		private LocaleFormatter _formatter;

		private PluralResolver _pluralResolver;

		private LocaleFontProvider _fontProvider;

		private PseudoLocalizer _pseudoLocalizer;

		private MissingTranslationTracker _missingTracker;

		private TextLengthValidator _lengthValidator;

		private List<LocaleInfo> _availableLocales = new List<LocaleInfo>();

		private PseudoLocalizationMode _pseudoMode;

		public static LocalizationService Instance { get; private set; }

		public bool IsInitialized { get; private set; }

		public string CurrentLocaleCode { get; private set; }

		public IReadOnlyList<LocaleInfo> AvailableLocales => _availableLocales;

		public PseudoLocalizationMode PseudoMode
		{
			get
			{
				return _pseudoMode;
			}
			set
			{
				_pseudoMode = value;
			}
		}

		public event Action OnLocaleChanged;

		public event Action<TMP_FontAsset> OnFontChanged;

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			Instance = this;
			Initialize();
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
			LocalizationSettings.SelectedLocaleChanged -= OnUnityLocaleChanged;
		}

		private void Initialize()
		{
			if (config == null)
			{
				Debug.LogError("[LocalizationService] LocalizationConfig is not assigned!");
				return;
			}
			_resolver = new StringResolver(config);
			_formatter = new LocaleFormatter();
			_pluralResolver = new PluralResolver();
			_fontProvider = new LocaleFontProvider(config.fontConfig);
			_pseudoLocalizer = new PseudoLocalizer();
			_missingTracker = new MissingTranslationTracker(config.logMissingKeys);
			_lengthValidator = new TextLengthValidator(config.defaultMaxCharacters, config.expansionWarningThreshold);
			_pseudoMode = config.defaultPseudoMode;
			if (LocalizationSettings.Instance != null)
			{
				Locale selectedLocale = LocalizationSettings.SelectedLocale;
				CurrentLocaleCode = ((selectedLocale != null) ? selectedLocale.Identifier.Code : config.fallbackLocaleCode);
				_formatter.SetLocale(CurrentLocaleCode);
				RefreshAvailableLocales();
				LocalizationSettings.SelectedLocaleChanged += OnUnityLocaleChanged;
				AsyncOperationHandle<LocalizationSettings> initializationOperation = LocalizationSettings.InitializationOperation;
				if (initializationOperation.IsDone)
				{
					_resolver.PreloadAllTables(NotifyLocaleChanged);
				}
				else
				{
					initializationOperation.Completed += delegate
					{
						_resolver.PreloadAllTables(NotifyLocaleChanged);
					};
				}
			}
			else
			{
				CurrentLocaleCode = config.fallbackLocaleCode;
				_formatter.SetLocale(CurrentLocaleCode);
			}
			IsInitialized = true;
		}

		public string Localize(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return key;
			}
			if (string.IsNullOrEmpty(config.keyPrefix) || !key.StartsWith(config.keyPrefix))
			{
				return key;
			}
			string text = _resolver.Resolve(key);
			if (text == null)
			{
				_missingTracker.Track(key, CurrentLocaleCode);
				if (!config.showMissingKeyVisualIndicator)
				{
					return key;
				}
				return string.Format(config.missingKeyFormat, key);
			}
			if (_pseudoMode != PseudoLocalizationMode.None)
			{
				text = _pseudoLocalizer.Apply(text, _pseudoMode);
			}
			if (config.enableLengthValidation)
			{
				_lengthValidator.Validate(key, text);
			}
			return text;
		}

		public string Localize(string key, params object[] args)
		{
			string text = Localize(key);
			if (text == key || args == null || args.Length == 0)
			{
				return text;
			}
			try
			{
				return string.Format(text, args);
			}
			catch (FormatException)
			{
				return text;
			}
		}

		public string LocalizeName(string englishName)
		{
			if (string.IsNullOrEmpty(englishName))
			{
				return englishName;
			}
			string text = NormalizeToKey(englishName);
			string text2 = config.keyPrefix + "name." + text;
			if (!_resolver.HasEntry(text2))
			{
				if (config.logMissingKeys)
				{
					Debug.LogWarning("[Localization] LocalizeName missing: \"" + englishName + "\" → key=\"" + text2 + "\" → table=\"" + (config.GetTableName("name") ?? config.defaultTableName) + "\" entry=\"" + text + "\"");
				}
				_missingTracker.Track(text2, CurrentLocaleCode);
				if (!config.hideMissingNames)
				{
					return englishName;
				}
				return string.Empty;
			}
			string text3 = _resolver.Resolve(text2);
			if (text3 == null)
			{
				if (!config.hideMissingNames)
				{
					return englishName;
				}
				text3 = string.Empty;
			}
			return text3;
		}

		public string LocalizeWithParams(string key, params (string name, object value)[] namedArgs)
		{
			string text = Localize(key);
			if (text == key || namedArgs == null || namedArgs.Length == 0)
			{
				return text;
			}
			for (int i = 0; i < namedArgs.Length; i++)
			{
				(string name, object value) tuple = namedArgs[i];
				string item = tuple.name;
				object item2 = tuple.value;
				text = text.Replace("{" + item + "}", item2?.ToString() ?? string.Empty);
			}
			return text;
		}

		public bool HasKey(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return false;
			}
			if (string.IsNullOrEmpty(config.keyPrefix) || !key.StartsWith(config.keyPrefix))
			{
				return false;
			}
			return _resolver.HasEntry(key);
		}

		public void SetLocale(string localeCode)
		{
			if (LocalizationSettings.Instance == null)
			{
				return;
			}
			foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
			{
				if (string.Equals(locale.Identifier.Code, localeCode, StringComparison.OrdinalIgnoreCase))
				{
					LocalizationSettings.SelectedLocale = locale;
					return;
				}
			}
			Debug.LogWarning("[LocalizationService] Locale \"" + localeCode + "\" not found in available locales.");
		}

		public string Pluralize(string key, int count)
		{
			if (string.IsNullOrEmpty(key))
			{
				return key;
			}
			string text = _pluralResolver.Resolve(_resolver, key, count, CurrentLocaleCode);
			if (text == null)
			{
				_missingTracker.Track(key, CurrentLocaleCode);
				if (!config.showMissingKeyVisualIndicator)
				{
					return key;
				}
				return string.Format(config.missingKeyFormat, key);
			}
			if (_pseudoMode != PseudoLocalizationMode.None)
			{
				text = _pseudoLocalizer.Apply(text, _pseudoMode);
			}
			return text;
		}

		public string Pluralize(string key, int count, params (string name, object value)[] namedArgs)
		{
			string text = Pluralize(key, count);
			if (namedArgs == null || namedArgs.Length == 0)
			{
				return text;
			}
			for (int i = 0; i < namedArgs.Length; i++)
			{
				(string name, object value) tuple = namedArgs[i];
				string item = tuple.name;
				object item2 = tuple.value;
				text = text.Replace("{" + item + "}", item2?.ToString() ?? string.Empty);
			}
			return text;
		}

		public string FormatNumber(double value, int decimals = 0)
		{
			return _formatter.FormatNumber(value, decimals);
		}

		public string FormatPercent(float ratio, int decimals = 0)
		{
			return _formatter.FormatPercent(ratio, decimals);
		}

		public string FormatDate(DateTime date)
		{
			return _formatter.FormatDate(date);
		}

		public string FormatTime(int hours, int minutes)
		{
			return _formatter.FormatTime(hours, minutes);
		}

		public string FormatTemperature(float celsius)
		{
			return _formatter.FormatTemperature(celsius, config.defaultTemperatureUnit);
		}

		public TMP_FontAsset GetLocalizedFont()
		{
			return _fontProvider.GetFont(CurrentLocaleCode);
		}

		public TMP_FontAsset GetLocalizedFont(string style)
		{
			return _fontProvider.GetFont(CurrentLocaleCode, style);
		}

		public IReadOnlyList<MissingTranslation> GetMissingTranslations()
		{
			return _missingTracker.GetAll();
		}

		public TranslationCoverage GetCoverage(string localeCode)
		{
			return new TranslationCoverage
			{
				LocaleCode = localeCode,
				TotalKeys = 0,
				TranslatedKeys = 0,
				MissingKeys = 0,
				Percentage = 0f,
				MissingKeyList = new List<string>()
			};
		}

		public void ClearMissingTranslations()
		{
			_missingTracker.Clear();
		}

		private void OnUnityLocaleChanged(Locale newLocale)
		{
			string text = ((newLocale != null) ? newLocale.Identifier.Code : config.fallbackLocaleCode);
			if (!(text == CurrentLocaleCode))
			{
				CurrentLocaleCode = text;
				_formatter.SetLocale(CurrentLocaleCode);
				RefreshAvailableLocales();
				_resolver.InvalidateCache();
				_resolver.PreloadAllTables(NotifyLocaleChanged);
			}
		}

		private void NotifyLocaleChanged()
		{
			this.OnLocaleChanged?.Invoke();
			TMP_FontAsset localizedFont = GetLocalizedFont();
			if (localizedFont != null)
			{
				this.OnFontChanged?.Invoke(localizedFont);
			}
		}

		private void RefreshAvailableLocales()
		{
			_availableLocales.Clear();
			if (LocalizationSettings.Instance == null)
			{
				return;
			}
			foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
			{
				CultureInfo cultureInfo = locale.Identifier.CultureInfo;
				_availableLocales.Add(new LocaleInfo
				{
					Code = locale.Identifier.Code,
					DisplayName = locale.LocaleName,
					NativeName = (cultureInfo?.NativeName ?? locale.LocaleName),
					IsRTL = (cultureInfo?.TextInfo?.IsRightToLeft == true)
				});
			}
		}

		private static string NormalizeToKey(string name)
		{
			StringBuilder stringBuilder = new StringBuilder(name.Length);
			bool flag = false;
			foreach (char c in name)
			{
				if (c == ' ' || c == '-' || c == '_')
				{
					if (!flag && stringBuilder.Length > 0)
					{
						stringBuilder.Append('_');
						flag = true;
					}
				}
				else if (char.IsLetterOrDigit(c))
				{
					stringBuilder.Append(char.ToLowerInvariant(c));
					flag = false;
				}
			}
			if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == '_')
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			return stringBuilder.ToString();
		}
	}
}
