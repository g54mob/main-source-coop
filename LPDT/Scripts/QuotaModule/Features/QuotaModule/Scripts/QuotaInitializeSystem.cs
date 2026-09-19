using System;
using System.Collections.Generic;
using Features.LevelModule.Scripts;
using Features.LevelObjectSpawnModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using UnityEngine;
using Zenject;

namespace Features.QuotaModule.Scripts
{
	public class QuotaInitializeSystem : IInitializable, IDisposable, ILevelQuotaInitializer
	{
		private readonly QuotaConfiguration _quotaConfiguration;

		private readonly QuotaSynchronizedModel _quotaSynchronizedModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly LevelsObjectCostModel _levelsObjectCostModel;

		private readonly ILevelObjectsSpawnService _levelObjectsSpawnService;

		private readonly LevelModel _levelModel;

		private readonly QuotaCompletionModel _quotaCompletionModel;

		private readonly CurrentWalletSynchronizedModel _currentWalletSynchronizedModel;

		private readonly LevelsConfiguration _levelsConfiguration;

		private readonly ChapterResumeRewardConfiguration _chapterResumeRewardConfiguration;

		public QuotaInitializeSystem(QuotaConfiguration quotaConfiguration, QuotaSynchronizedModel quotaSynchronizedModel, MultiplayerModel multiplayerModel, LevelsObjectCostModel levelsObjectCostModel, ILevelObjectsSpawnService levelObjectsSpawnService, LevelModel levelModel, QuotaCompletionModel quotaCompletionModel, CurrentWalletSynchronizedModel currentWalletSynchronizedModel, LevelsConfiguration levelsConfiguration, ChapterResumeRewardConfiguration chapterResumeRewardConfiguration)
		{
			_quotaConfiguration = quotaConfiguration;
			_quotaSynchronizedModel = quotaSynchronizedModel;
			_multiplayerModel = multiplayerModel;
			_levelsObjectCostModel = levelsObjectCostModel;
			_levelObjectsSpawnService = levelObjectsSpawnService;
			_levelModel = levelModel;
			_quotaCompletionModel = quotaCompletionModel;
			_currentWalletSynchronizedModel = currentWalletSynchronizedModel;
			_levelsConfiguration = levelsConfiguration;
			_chapterResumeRewardConfiguration = chapterResumeRewardConfiguration;
		}

		public void Initialize()
		{
			_quotaSynchronizedModel.OnQuotaChanged += OnQuotaChanged;
		}

		public void Dispose()
		{
			_quotaSynchronizedModel.OnQuotaChanged -= OnQuotaChanged;
		}

		public void HarvestCollectedGoldToWallet()
		{
			_currentWalletSynchronizedModel.SetQuota(_quotaSynchronizedModel.CurrentQuota.Value);
		}

		public void ResetRunWallet()
		{
			_currentWalletSynchronizedModel.SetQuota(0f);
		}

		public void ApplyChapterStartWallet()
		{
			int coins = ResolveChapterStartCoins(_levelModel.SelectedChapterIndex);
			coins = ClampToThemeBoundaryCap(_levelModel.SelectedChapterIndex, coins);
			_currentWalletSynchronizedModel.SetQuota(coins);
		}

		private int ClampToThemeBoundaryCap(int selectedChapterIndex, int coins)
		{
			if (!_levelsConfiguration.ChapterSequences.TryGetValue(_levelModel.SelectedSequenceSet, out var value))
			{
				return coins;
			}
			if (selectedChapterIndex < 0 || selectedChapterIndex >= value.Count)
			{
				return coins;
			}
			List<LevelType> levels = value[selectedChapterIndex].Levels;
			if (levels.Count == 0)
			{
				return coins;
			}
			if (_quotaConfiguration.ThemeBoundaryCarryCap.TryGetValue(levels[0], out var value2) && (float)coins > value2)
			{
				return Mathf.RoundToInt(value2);
			}
			return coins;
		}

		public void ApplyThemeBoundaryCarryCap()
		{
			if (_quotaConfiguration.ThemeBoundaryCarryCap.TryGetValue(_levelModel.CurrentLevel, out var value) && _currentWalletSynchronizedModel.CurrentSessionMoney > value)
			{
				_currentWalletSynchronizedModel.SetQuota(value);
			}
		}

		private int ResolveChapterStartCoins(int selectedChapterIndex)
		{
			if (selectedChapterIndex <= 0)
			{
				return 0;
			}
			if (!_levelsConfiguration.ChapterSequences.TryGetValue(_levelModel.SelectedSequenceSet, out var value))
			{
				return 0;
			}
			int num = selectedChapterIndex - 1;
			if (num < 0 || num >= value.Count)
			{
				return 0;
			}
			List<LevelType> levels = value[num].Levels;
			if (levels.Count == 0)
			{
				return 0;
			}
			LevelType levelType = levels[levels.Count - 1];
			if (!_chapterResumeRewardConfiguration.CoinsByLevel.TryGetValue(levelType, out var value2))
			{
				Debug.LogWarning($"[ChapterStartWallet] No resume coins configured for last level {levelType} of the previous chapter. Starting at 0.");
				return 0;
			}
			return value2;
		}

		public void InitializeForCurrentLevel()
		{
			if (_quotaSynchronizedModel.MaxQuota.Value > 0f)
			{
				return;
			}
			_quotaCompletionModel.IsQuotaCompleted.Value = false;
			_quotaCompletionModel.IsBellActivated.Value = false;
			_levelObjectsSpawnService.PrepareSpawnItems(_levelModel.CurrentLevel);
			float value2;
			if (_quotaConfiguration.ExplicitQuotaByLevelOverride.TryGetValue(_levelModel.CurrentLevel, out var value))
			{
				_quotaSynchronizedModel.CurrentQuota.Value = 0f;
				_quotaSynchronizedModel.MaxQuota.Value = value;
			}
			else if (_quotaConfiguration.QuotaPercentByLevel.TryGetValue(_levelModel.CurrentLevel, out value2))
			{
				float num = value2 / 100f * (float)_levelsObjectCostModel.TotalCost;
				if (num == 0f)
				{
					num = 1f;
				}
				num = Mathf.RoundToInt(num);
				_quotaSynchronizedModel.CurrentQuota.Value = 0f;
				_quotaSynchronizedModel.MaxQuota.Value = num;
			}
		}

		private void OnQuotaChanged(float current, float max)
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient && !(max <= 0f) && !(current < max))
			{
				_quotaCompletionModel.IsQuotaCompleted.Value = true;
			}
		}
	}
}
