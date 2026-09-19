using System;
using Features.ChineseDetectionModule.Scripts.Core;
using Features.ChineseDetectionModule.Scripts.Data;
using Features.SettingsMenuModule.Scripts.Data;
using Global.Modules.Localization_Module.Scripts;
using Zenject;

namespace Features.ChineseDetectionModule.Scripts.Systems
{
	public class ChineseDetectionSystem : IInitializable, IDisposable
	{
		private readonly IChineseDetectionService _chineseDetectionService;

		private readonly ChineseDetectionModel _chineseDetectionModel;

		private readonly RegionsPingModel _regionsPingModel;

		private readonly ILanguageService _languageService;

		public ChineseDetectionSystem(IChineseDetectionService chineseDetectionService, ChineseDetectionModel chineseDetectionModel, RegionsPingModel regionsPingModel, ILanguageService languageService)
		{
			_chineseDetectionService = chineseDetectionService;
			_chineseDetectionModel = chineseDetectionModel;
			_regionsPingModel = regionsPingModel;
			_languageService = languageService;
		}

		public void Initialize()
		{
			ILanguageService languageService = _languageService;
			languageService.OnPreLanguageChanged = (Action)Delegate.Combine(languageService.OnPreLanguageChanged, new Action(ReevaluateChineseAudience));
			_regionsPingModel.OnCurrentRegionUpdated += ReevaluateChineseAudience;
			_regionsPingModel.OnRegionsInfoUpdated += ReevaluateChineseAudience;
			ReevaluateChineseAudience();
		}

		public void Dispose()
		{
			ILanguageService languageService = _languageService;
			languageService.OnPreLanguageChanged = (Action)Delegate.Remove(languageService.OnPreLanguageChanged, new Action(ReevaluateChineseAudience));
			_regionsPingModel.OnCurrentRegionUpdated -= ReevaluateChineseAudience;
			_regionsPingModel.OnRegionsInfoUpdated -= ReevaluateChineseAudience;
		}

		private void ReevaluateChineseAudience()
		{
			_chineseDetectionModel.SetIsChineseAudience(_chineseDetectionService.IsChineseAudience());
		}
	}
}
