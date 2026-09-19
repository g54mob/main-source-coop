using System;
using System.Collections.Generic;
using System.Linq;
using Features.BoosterModule.BoosterModule.Scripts.Entities;
using Features.GamePauseModule.Scripts;
using Features.GameUpdaterModule;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.BoosterModule.BoosterModule.Scripts
{
	public class BoosterTimeSystem : IInitializable, IDisposable
	{
		private BoosterModel _boosterModel;

		private GameGlobalNetworkingPause _gameGlobalNetworkingPause;

		private Coroutine _coroutine;

		private MultiplayerModel _multiplayerModel;

		private IGameUpdater _gameUpdater;

		[Inject]
		public void InjectDependencies(BoosterModel boosterModel, GameGlobalNetworkingPause gameGlobalNetworkingPause, MultiplayerModel multiplayerModel, IGameUpdater gameUpdater)
		{
			_boosterModel = boosterModel;
			_gameGlobalNetworkingPause = gameGlobalNetworkingPause;
			_multiplayerModel = multiplayerModel;
			_gameUpdater = gameUpdater;
		}

		public void Initialize()
		{
			_gameUpdater.OnUpdate += ChangeBoosterTime;
		}

		public void Dispose()
		{
			_gameUpdater.OnUpdate -= ChangeBoosterTime;
		}

		private void ChangeBoosterTime()
		{
			foreach (KeyValuePair<PlayerRef, Dictionary<string, IBoosterEntity>> activeBooster in _boosterModel.ActiveBoosters)
			{
				for (int i = 0; i < activeBooster.Value.Values.Count; i++)
				{
					KeyValuePair<string, IBoosterEntity> keyValuePair = activeBooster.Value.ElementAt(i);
					IBoosterEntity value = keyValuePair.Value;
					if (value.IsLifeTimeBlocked)
					{
						continue;
					}
					value.CurrentBoosterLifeTime -= Time.deltaTime * _gameGlobalNetworkingPause.NetworkTimeScale;
					if (!(value.CurrentBoosterLifeTime > 0f))
					{
						_boosterModel.RemoveActiveBooster(keyValuePair.Key, activeBooster.Key);
						i--;
						if (activeBooster.Key == _multiplayerModel.NetworkRunner.LocalPlayer)
						{
							value.Deactivate();
						}
					}
				}
			}
		}
	}
}
