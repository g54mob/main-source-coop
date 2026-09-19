using System;
using Cysharp.Threading.Tasks;
using Features.KrakenModule.Scripts.Data;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.KrakenModule.Scripts.Systems
{
	public class KrakenAggressiveThrowEvaluationSystem : IInitializable, IDisposable
	{
		private readonly KrakenAggressiveThrowConfiguration _configuration;

		private readonly KrakenPlayerThrowTrackerModel _throwTrackerModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly KrakenRuntimeModel _runtimeModel;

		private float _lastAggressiveThrowTime = float.NegativeInfinity;

		private bool _isAggressiveThrowInProgress;

		public KrakenAggressiveThrowEvaluationSystem(KrakenAggressiveThrowConfiguration configuration, KrakenPlayerThrowTrackerModel throwTrackerModel, MultiplayerModel multiplayerModel, KrakenRuntimeModel runtimeModel)
		{
			_configuration = configuration;
			_throwTrackerModel = throwTrackerModel;
			_multiplayerModel = multiplayerModel;
			_runtimeModel = runtimeModel;
		}

		public void Initialize()
		{
			_throwTrackerModel.OnThrowRecorded += TryTriggerAggressiveThrow;
		}

		public void Dispose()
		{
			_throwTrackerModel.OnThrowRecorded -= TryTriggerAggressiveThrow;
		}

		private void TryTriggerAggressiveThrow()
		{
			if (!_isAggressiveThrowInProgress && !(_configuration == null) && !(_configuration.KrakenRockPrefab == null))
			{
				NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
				if (!(networkRunner == null) && networkRunner.IsRunning && networkRunner.IsSharedModeMasterClient && _runtimeModel.TryGetController(out var controller) && controller.HasStateAuthority && controller.CanAcceptInteraction && !(Time.time - _lastAggressiveThrowTime < _configuration.AggressiveThrowCooldown) && _throwTrackerModel.TotalCountInWindow >= _configuration.MinTotalThrowsToTrigger && _throwTrackerModel.TryGetTopThrower(out var topThrower))
				{
					TryStartAggressiveThrowAsync(controller, topThrower).Forget();
				}
			}
		}

		private async UniTaskVoid TryStartAggressiveThrowAsync(KrakenController controller, PlayerRef target)
		{
			_isAggressiveThrowInProgress = true;
			try
			{
				if (await controller.RequestAggressiveRockThrowAsync(target))
				{
					_lastAggressiveThrowTime = Time.time;
					_throwTrackerModel.ClearWindow();
				}
			}
			finally
			{
				_isAggressiveThrowInProgress = false;
			}
		}
	}
}
