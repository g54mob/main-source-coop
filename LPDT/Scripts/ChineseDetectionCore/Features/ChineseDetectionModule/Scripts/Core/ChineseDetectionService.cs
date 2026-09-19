using System;
using System.Collections.Generic;
using Features.SettingsMenuModule.Scripts.Data;
using Features.SteamImplementationModule.Scripts;
using Fusion.Photon.Realtime;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.Modules.Localization_Module.Scripts;

namespace Features.ChineseDetectionModule.Scripts.Core
{
	public class ChineseDetectionService : IChineseDetectionService
	{
		private const string BASE_REGION = "eu";

		private static readonly HashSet<string> _chineseRegionCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "hk" };

		private static readonly HashSet<Language> _chineseLanguages = new HashSet<Language>
		{
			Language.TraditionalChinese,
			Language.SimplifiedChinese
		};

		private readonly ISteamRegionProvider _steamRegionProvider;

		private readonly CurrentSettingsModel _currentSettingsModel;

		private readonly RegionsPingModel _regionsPingModel;

		private readonly ILanguageService _languageService;

		public ChineseDetectionService(ISteamRegionProvider steamRegionProvider, CurrentSettingsModel currentSettingsModel, RegionsPingModel regionsPingModel, ILanguageService languageService)
		{
			_steamRegionProvider = steamRegionProvider;
			_currentSettingsModel = currentSettingsModel;
			_regionsPingModel = regionsPingModel;
			_languageService = languageService;
		}

		public bool IsChineseAudience()
		{
			if (IsChineseSteamRegion())
			{
				return true;
			}
			if (_chineseRegionCodes.Contains(ResolveCurrentPhotonRegion()))
			{
				return _chineseLanguages.Contains(_languageService.GetCurrentLanguage());
			}
			return false;
		}

		private bool IsChineseSteamRegion()
		{
			if (!_steamRegionProvider.IsAvailable)
			{
				return false;
			}
			string ipCountry = _steamRegionProvider.GetIpCountry();
			if (!string.IsNullOrEmpty(ipCountry))
			{
				return _chineseRegionCodes.Contains(ipCountry);
			}
			return false;
		}

		private string ResolveCurrentPhotonRegion()
		{
			if (_currentSettingsModel.HasFixedRegion)
			{
				return _currentSettingsModel.FixedRegion;
			}
			return ResolveBestPingRegion();
		}

		private string ResolveBestPingRegion()
		{
			IReadOnlyList<RegionInfo> regions = _regionsPingModel.Regions;
			if (regions == null || regions.Count == 0)
			{
				return "eu";
			}
			RegionInfo regionInfo = regions[0];
			foreach (RegionInfo item in regions)
			{
				if (item.RegionPing < regionInfo.RegionPing)
				{
					regionInfo = item;
				}
			}
			return regionInfo.RegionCode;
		}
	}
}
