using System;
using System.Collections.Generic;
using System.Threading;
using ArabicSupport;
using Cysharp.Threading.Tasks;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.Modules.Localization_Module.Scripts;
using RSG.Muffin.AssetLoaderModule.Core;
using TMPro;
using UnityEngine;

namespace Features.CustomFontAssetCreatorModule.Scripts
{
	public class CustomFontAssetCreatorService : ICustomFontAssetCreatorService
	{
		private const string DEFAULT_CHARACTERS = "1234567890_";

		private readonly FontsData _fontsData;

		private readonly ILanguageService _languageService;

		private readonly IAssetLoaderFacadeService _loaderService;

		private readonly LocalizationBatchModel _localizationBatchModel;

		private readonly CustomFontAssetModel _customFontAssetModel;

		private readonly LanguagesLocalizationConfiguration _languagesLocalizationConfiguration;

		private CancellationTokenSource _generationCts;

		public CustomFontAssetCreatorService(FontsData fontsData, ILanguageService languageService, IAssetLoaderFacadeService loaderService, LocalizationBatchModel localizationBatchModel, CustomFontAssetModel customFontAssetModel, LanguagesLocalizationConfiguration languagesLocalizationConfiguration)
		{
			_fontsData = fontsData;
			_languageService = languageService;
			_loaderService = loaderService;
			_localizationBatchModel = localizationBatchModel;
			_customFontAssetModel = customFontAssetModel;
			_languagesLocalizationConfiguration = languagesLocalizationConfiguration;
		}

		public async UniTask GenerateFonts()
		{
			CancelOngoingGeneration();
			CancellationTokenSource cts = (_generationCts = new CancellationTokenSource());
			CancellationToken token = cts.Token;
			try
			{
				token.ThrowIfCancellationRequested();
				ClearGeneratedFonts();
				foreach (FontData data in _fontsData.Datas)
				{
					token.ThrowIfCancellationRequested();
					if (data.Languages.Contains(_languageService.GetCurrentLanguage()))
					{
						await GenerateFont(data, new LanguageMapper(), token);
					}
				}
				await AddFallbacksToPrimaryFonts(token);
				token.ThrowIfCancellationRequested();
				_languageService.OnLanguageChanged?.Invoke();
			}
			catch (OperationCanceledException)
			{
			}
			finally
			{
				if (_generationCts == cts)
				{
					_generationCts.Dispose();
					_generationCts = null;
				}
			}
		}

		private void CancelOngoingGeneration()
		{
			if (_generationCts != null)
			{
				_generationCts.Cancel();
				_generationCts.Dispose();
				_generationCts = null;
			}
		}

		public async UniTask GenerateFontsWithWindow()
		{
			await GenerateFonts();
		}

		private void ClearGeneratedFonts()
		{
			foreach (TMP_FontAsset value in _customFontAssetModel.GeneratedFonts.Values)
			{
				if (value != null)
				{
					UnityEngine.Object.Destroy(value);
				}
			}
			_customFontAssetModel.GeneratedFonts.Clear();
		}

		private async UniTask AddFallbacksToPrimaryFonts(CancellationToken token)
		{
			foreach (PrimaryFontBindingData bindingData in _fontsData.PrimaryFontBindings)
			{
				token.ThrowIfCancellationRequested();
				List<TMP_FontAsset> fontsToAdd = new List<TMP_FontAsset>();
				foreach (FallbackFontType item in bindingData.FontsToSet)
				{
					if (_customFontAssetModel.GeneratedFonts.TryGetValue(item, out var value))
					{
						fontsToAdd.Add(value);
					}
				}
				foreach (AdditionalSpecialFontData additionalSpecialFont in _fontsData.AdditionalSpecialFonts)
				{
					if (additionalSpecialFont.PrimaryFontType == bindingData.PrimaryFontType)
					{
						fontsToAdd.Add(additionalSpecialFont.Font);
					}
				}
				TMP_FontAsset primaryFont = await _loaderService.LoadAssetAsync<TMP_FontAsset>(_fontsData.PrimaryFontsPath[bindingData.PrimaryFontType], AssetLoadSource.Addressables);
				token.ThrowIfCancellationRequested();
				RemovePreviouslyAppliedFallbacks(bindingData.PrimaryFontType, primaryFont);
				DeduplicateFallbackTable(primaryFont);
				ApplyFallbacks(bindingData.PrimaryFontType, primaryFont, fontsToAdd);
			}
		}

		private void RemovePreviouslyAppliedFallbacks(PrimaryFontType primaryFontType, TMP_FontAsset primaryFont)
		{
			if (!_customFontAssetModel.AppliedPrimaryFallbacks.TryGetValue(primaryFontType, out var value))
			{
				return;
			}
			foreach (TMP_FontAsset item in value)
			{
				if (item != null)
				{
					primaryFont.fallbackFontAssetTable.Remove(item);
				}
			}
			value.Clear();
		}

		private void ApplyFallbacks(PrimaryFontType primaryFontType, TMP_FontAsset primaryFont, List<TMP_FontAsset> fontsToAdd)
		{
			if (!_customFontAssetModel.AppliedPrimaryFallbacks.TryGetValue(primaryFontType, out var value))
			{
				List<TMP_FontAsset> list = (_customFontAssetModel.AppliedPrimaryFallbacks[primaryFontType] = new List<TMP_FontAsset>());
				value = list;
			}
			foreach (TMP_FontAsset item in fontsToAdd)
			{
				if (!(item == null) && !(item == primaryFont) && !primaryFont.fallbackFontAssetTable.Contains(item))
				{
					primaryFont.fallbackFontAssetTable.Add(item);
					value.Add(item);
				}
			}
		}

		private static void DeduplicateFallbackTable(TMP_FontAsset primaryFont)
		{
			List<TMP_FontAsset> list = new List<TMP_FontAsset>();
			foreach (TMP_FontAsset item in primaryFont.fallbackFontAssetTable)
			{
				if (item != null && !list.Contains(item))
				{
					list.Add(item);
				}
			}
			primaryFont.fallbackFontAssetTable.Clear();
			primaryFont.fallbackFontAssetTable.AddRange(list);
		}

		private async UniTask GenerateFont(FontData fontData, LanguageMapper languageMapper, CancellationToken token)
		{
			Font font = await _loaderService.LoadAssetAsync<Font>(_fontsData.FontsPath[fontData.FontType], AssetLoadSource.Addressables);
			token.ThrowIfCancellationRequested();
			TMP_FontAsset tMP_FontAsset = TMP_FontAsset.CreateFontAsset(font);
			tMP_FontAsset.ClearFontAssetData();
			tMP_FontAsset.atlasPopulationMode = AtlasPopulationMode.Dynamic;
			tMP_FontAsset.isMultiAtlasTexturesEnabled = true;
			string text = "1234567890_";
			string text2 = "";
			foreach (ILocalizationDataHolder localizationDataHolder in _localizationBatchModel.LocalizationDataHolders)
			{
				foreach (Language language in fontData.Languages)
				{
					string stringByLanguage = languageMapper.GetStringByLanguage(localizationDataHolder, language);
					for (int i = 0; i < stringByLanguage.Length; i++)
					{
						char value = stringByLanguage[i];
						if (_languagesLocalizationConfiguration.ArabicLanguages.Contains(language))
						{
							text2 += value;
						}
						else if (!text.Contains(value))
						{
							text += value;
						}
					}
				}
			}
			text += text2;
			text2 = ArabicFixer.Fix(text2, showTashkeel: false, useHinduNumbers: false);
			text += text2;
			tMP_FontAsset.TryAddCharacters(text, includeFontFeatures: true);
			tMP_FontAsset.atlasPopulationMode = AtlasPopulationMode.Static;
			_customFontAssetModel.GeneratedFonts.Add(fontData.FontType, tMP_FontAsset);
		}
	}
}
