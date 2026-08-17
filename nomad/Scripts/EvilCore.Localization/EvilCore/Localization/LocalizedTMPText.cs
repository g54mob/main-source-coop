using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EvilCore.Localization
{
	[RequireComponent(typeof(TMP_Text))]
	public class LocalizedTMPText : MonoBehaviour
	{
		[LocalizationKey(null)]
		[Tooltip("Localization key, e.g. \"@settings.fullscreen\". The prefab's authored text is used as a fallback until the service resolves.")]
		[SerializeField]
		private string key;

		[Tooltip("Optional font style passed to GetLocalizedFont (e.g. \"bold\").")]
		[SerializeField]
		private string fontStyle;

		[Tooltip("Swap the TMP font to the per-locale font on locale/font change.")]
		[SerializeField]
		private bool applyFont = true;

		private TMP_Text _text;

		private ILocalizationService _service;

		private bool _subscribed;

		public string Key => key;

		private void Awake()
		{
			_text = GetComponent<TMP_Text>();
		}

		private void OnEnable()
		{
			TryBind();
		}

		private void Start()
		{
			TryBind();
		}

		private void OnDisable()
		{
			if (_service != null && _subscribed)
			{
				_service.OnLocaleChanged -= Apply;
				_service.OnFontChanged -= OnFontChanged;
			}
			_subscribed = false;
		}

		public void SetKey(string newKey)
		{
			key = newKey;
			Apply();
		}

		private void TryBind()
		{
			if (_subscribed)
			{
				Apply();
				return;
			}
			if (_service == null)
			{
				_service = LocalizationService.Instance;
			}
			if (_service != null)
			{
				_service.OnLocaleChanged += Apply;
				_service.OnFontChanged += OnFontChanged;
				_subscribed = true;
				Apply();
			}
		}

		private void Apply()
		{
			if (_text == null)
			{
				_text = GetComponent<TMP_Text>();
			}
			if (!(_text == null) && _service != null && !string.IsNullOrEmpty(key))
			{
				_text.text = _service.Localize(key);
				_text.isRightToLeftText = IsCurrentLocaleRtl();
				if (applyFont)
				{
					ApplyFont();
				}
			}
		}

		private void OnFontChanged(TMP_FontAsset _)
		{
			ApplyFont();
		}

		private void ApplyFont()
		{
			if (!(_text == null) && _service != null && applyFont)
			{
				TMP_FontAsset localizedFont = _service.GetLocalizedFont(fontStyle);
				if (localizedFont != null)
				{
					_text.font = localizedFont;
				}
			}
		}

		private bool IsCurrentLocaleRtl()
		{
			IReadOnlyList<LocaleInfo> availableLocales = _service.AvailableLocales;
			if (availableLocales == null)
			{
				return false;
			}
			string currentLocaleCode = _service.CurrentLocaleCode;
			foreach (LocaleInfo item in availableLocales)
			{
				if (item.Code == currentLocaleCode)
				{
					return item.IsRTL;
				}
			}
			return false;
		}
	}
}
