using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EvilCore.Localization
{
	public class GlobalLocaleFontApplier : MonoBehaviour
	{
		[Tooltip("The same LocaleFontConfig wired into LocalizationConfig. Used to distinguish a Latin locale (the default font) from a dedicated CJK font for the fallback decision.")]
		[SerializeField]
		private LocaleFontConfig fontConfig;

		private ILocalizationService _service;

		private bool _subscribed;

		private TMP_FontAsset _originalDefaultFont;

		private List<TMP_FontAsset> _originalFallbacks;

		private bool _snapshotTaken;

		private void Start()
		{
			_service = LocalizationService.Instance;
			if (_service == null)
			{
				Debug.LogWarning("[GlobalLocaleFontApplier] LocalizationService not available; the global font will not be applied.");
				return;
			}
			if (fontConfig == null)
			{
				Debug.LogWarning("[GlobalLocaleFontApplier] LocaleFontConfig is not assigned; the CJK fallback will not be managed.");
			}
			TakeSnapshot();
			ApplyGlobalFont(_service.GetLocalizedFont());
			_service.OnFontChanged += ApplyGlobalFont;
			SceneManager.sceneLoaded += OnSceneLoaded;
			_subscribed = true;
		}

		private void OnDestroy()
		{
			if (_subscribed && _service != null)
			{
				_service.OnFontChanged -= ApplyGlobalFont;
				SceneManager.sceneLoaded -= OnSceneLoaded;
			}
			_subscribed = false;
			RestoreSnapshot();
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (_service != null)
			{
				ApplyGlobalFont(_service.GetLocalizedFont());
			}
		}

		private void ApplyGlobalFont(TMP_FontAsset font)
		{
			if (font == null)
			{
				return;
			}
			TMP_Settings.defaultFontAsset = font;
			TMP_Text[] array = Object.FindObjectsByType<TMP_Text>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (TMP_Text tMP_Text in array)
			{
				if (!(tMP_Text == null) && !(tMP_Text.font == font) && !(tMP_Text.GetComponent<LocalizedTMPText>() != null) && !(tMP_Text.GetComponent<LocaleFontIgnore>() != null))
				{
					tMP_Text.font = font;
					tMP_Text.SetAllDirty();
				}
			}
			UpdateFallback(font);
		}

		private void UpdateFallback(TMP_FontAsset font)
		{
			List<TMP_FontAsset> fallbackFontAssets = TMP_Settings.fallbackFontAssets;
			if (fallbackFontAssets == null)
			{
				if (fontConfig != null && font != fontConfig.DefaultFont)
				{
					Debug.LogWarning("[GlobalLocaleFontApplier] TMP_Settings has no fallback list; CJK glyphs in runtime-spawned text may not resolve. Add a fallback list in TMP Settings.");
				}
				return;
			}
			fallbackFontAssets.Clear();
			if (fontConfig != null && font != fontConfig.DefaultFont)
			{
				fallbackFontAssets.Add(font);
			}
		}

		private void TakeSnapshot()
		{
			if (!_snapshotTaken)
			{
				_originalDefaultFont = TMP_Settings.defaultFontAsset;
				List<TMP_FontAsset> fallbackFontAssets = TMP_Settings.fallbackFontAssets;
				_originalFallbacks = ((fallbackFontAssets != null) ? new List<TMP_FontAsset>(fallbackFontAssets) : new List<TMP_FontAsset>());
				_snapshotTaken = true;
			}
		}

		private void RestoreSnapshot()
		{
			if (_snapshotTaken)
			{
				TMP_Settings.defaultFontAsset = _originalDefaultFont;
				List<TMP_FontAsset> fallbackFontAssets = TMP_Settings.fallbackFontAssets;
				if (fallbackFontAssets != null)
				{
					fallbackFontAssets.Clear();
					fallbackFontAssets.AddRange(_originalFallbacks);
				}
				_snapshotTaken = false;
			}
		}
	}
}
