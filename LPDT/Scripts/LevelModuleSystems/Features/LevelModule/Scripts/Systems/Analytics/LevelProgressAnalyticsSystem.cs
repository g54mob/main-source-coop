using System;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.MultiplayerSessionServices.Scripts;
using Features.QuotaModule.Scripts;
using Zenject;

namespace Features.LevelModule.Scripts.Systems.Analytics
{
	public class LevelProgressAnalyticsSystem : IInitializable, IDisposable
	{
		private readonly CustomPlayerEventsSynchronizedModel _customPlayerEventsSynchronizedModel;

		private readonly QuotaCompletionModel _quotaCompletionModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly LevelModel _levelModel;

		public LevelProgressAnalyticsSystem(QuotaCompletionModel quotaCompletionModel, LevelModel levelModel, CustomPlayerEventsSynchronizedModel customPlayerEventsSynchronizedModel, MultiplayerModel multiplayerModel)
		{
			_quotaCompletionModel = quotaCompletionModel;
			_levelModel = levelModel;
			_customPlayerEventsSynchronizedModel = customPlayerEventsSynchronizedModel;
			_multiplayerModel = multiplayerModel;
		}

		public void Initialize()
		{
			_quotaCompletionModel.OnQuotaCompleted += OnQuotaCompleted;
		}

		public void Dispose()
		{
			_quotaCompletionModel.OnQuotaCompleted -= OnQuotaCompleted;
		}

		private void OnQuotaCompleted(bool isQuotaCompleted)
		{
			if (isQuotaCompleted && _multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				int currentSequenceLevelNumber = _levelModel.CurrentSequenceLevelNumber;
				if (currentSequenceLevelNumber >= 1)
				{
					_customPlayerEventsSynchronizedModel.SendPlayerEvent(-1, $"level_{currentSequenceLevelNumber}:completed");
				}
			}
		}
	}
}
