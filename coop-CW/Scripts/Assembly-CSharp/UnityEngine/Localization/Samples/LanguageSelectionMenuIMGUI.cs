using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Localization.Settings;

namespace UnityEngine.Localization.Samples
{
	public class LanguageSelectionMenuIMGUI : MonoBehaviour
	{
		public Rect windowRect = new Rect(0f, 0f, 300f, 300f);

		public Color selectColor = Color.yellow;

		public Color defaultColor = Color.gray;

		private Vector2 m_ScrollPos;

		private Dictionary<Locale, string> m_Labels = new Dictionary<Locale, string>();

		[Tooltip("Use the current active settings if possible or create a new one for the example")]
		public bool useActiveLocalizationSettings;

		private void Start()
		{
			LocalizationSettings instanceDontCreateDefault = LocalizationSettings.GetInstanceDontCreateDefault();
			if (useActiveLocalizationSettings && instanceDontCreateDefault != null)
			{
				Debug.Log("Using included localization data");
				return;
			}
			Debug.Log("Creating default localization data");
			instanceDontCreateDefault = ScriptableObject.CreateInstance<LocalizationSettings>();
			SimpleLocalesProvider simpleLocalesProvider = new SimpleLocalesProvider();
			instanceDontCreateDefault.SetAvailableLocales(simpleLocalesProvider);
			simpleLocalesProvider.AddLocale(Locale.CreateLocale(new LocaleIdentifier(SystemLanguage.Arabic)));
			simpleLocalesProvider.AddLocale(Locale.CreateLocale(new LocaleIdentifier(SystemLanguage.English)));
			simpleLocalesProvider.AddLocale(Locale.CreateLocale(new LocaleIdentifier(SystemLanguage.French)));
			simpleLocalesProvider.AddLocale(Locale.CreateLocale(new LocaleIdentifier(SystemLanguage.German)));
			simpleLocalesProvider.AddLocale(Locale.CreateLocale(new LocaleIdentifier(SystemLanguage.Japanese)));
			List<IStartupLocaleSelector> startupLocaleSelectors = instanceDontCreateDefault.GetStartupLocaleSelectors();
			startupLocaleSelectors.Clear();
			startupLocaleSelectors.Add(new SpecificLocaleSelector
			{
				LocaleId = SystemLanguage.English
			});
			instanceDontCreateDefault.OnSelectedLocaleChanged += OnSelectedLocaleChanged;
			LocalizationSettings.Instance = instanceDontCreateDefault;
		}

		private static void OnSelectedLocaleChanged(Locale newLocale)
		{
			Debug.Log("OnSelectedLocaleChanged: The locale just changed to " + newLocale);
		}

		private void OnGUI()
		{
			windowRect = GUI.Window(GetHashCode(), windowRect, DrawWindowContents, "Select Language");
		}

		private string GetLocaleLabel(Locale locale)
		{
			if (m_Labels.TryGetValue(locale, out var value))
			{
				return value;
			}
			CultureInfo cultureInfo = locale.Identifier.CultureInfo;
			value = ((cultureInfo == null) ? locale.ToString() : ((!(cultureInfo.EnglishName != cultureInfo.NativeName)) ? cultureInfo.EnglishName : (cultureInfo.EnglishName + "(" + cultureInfo.NativeName + ")")));
			m_Labels[locale] = value;
			return value;
		}

		private void DrawWindowContents(int id)
		{
			if (!LocalizationSettings.SelectedLocaleAsync.IsDone)
			{
				GUILayout.Label("Initializing Locales: " + LocalizationSettings.SelectedLocaleAsync.PercentComplete);
				GUI.DragWindow();
				return;
			}
			ILocalesProvider availableLocales = LocalizationSettings.AvailableLocales;
			if (availableLocales.Locales.Count == 0)
			{
				GUILayout.Label("No Locales included in the active Localization Settings.");
			}
			else
			{
				Color contentColor = GUI.contentColor;
				m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos);
				for (int i = 0; i < availableLocales.Locales.Count; i++)
				{
					Locale locale = availableLocales.Locales[i];
					GUI.contentColor = ((LocalizationSettings.SelectedLocale == locale) ? selectColor : defaultColor);
					if (GUILayout.Button(GetLocaleLabel(locale)))
					{
						LocalizationSettings.SelectedLocale = locale;
					}
				}
				GUILayout.EndScrollView();
				GUI.contentColor = contentColor;
			}
			GUI.DragWindow();
		}
	}
}
