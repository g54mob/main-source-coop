using Cysharp.Threading.Tasks;
using Features.DeadPartsModule.Scripts;
using Features.KrakenModule.Scripts.Core;
using Features.KrakenModule.Scripts.Data;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.KrakenModule.Scripts.Systems
{
	public class KrakenHelpThrowEvaluationSystem : ITickable
	{
		private readonly PlayerDeadPartModel _playerDeadPartModel;

		private readonly KrakenHelpThrowModel _krakenHelpThrowModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly KrakenRuntimeModel _runtimeModel;

		private readonly IDeadPartThrowTargetService _deadPartThrowTargetService;

		private float _checkTimer;

		private bool _isHelpThrowInProgress;

		public KrakenHelpThrowEvaluationSystem(PlayerDeadPartModel playerDeadPartModel, KrakenHelpThrowModel krakenHelpThrowModel, MultiplayerModel multiplayerModel, KrakenRuntimeModel runtimeModel, IDeadPartThrowTargetService deadPartThrowTargetService)
		{
			_playerDeadPartModel = playerDeadPartModel;
			_krakenHelpThrowModel = krakenHelpThrowModel;
			_multiplayerModel = multiplayerModel;
			_runtimeModel = runtimeModel;
			_deadPartThrowTargetService = deadPartThrowTargetService;
		}

		public void Tick()
		{
			_checkTimer -= Time.deltaTime;
			if (!(_checkTimer > 0f))
			{
				_checkTimer = 0.25f;
				TryRequestDeadPlayersAppear();
				if (!_isHelpThrowInProgress && TryEvaluateHelpThrow(out var controller, out var request))
				{
					TryStartHelpThrowAsync(controller, request).Forget();
				}
			}
		}

		private void TryRequestDeadPlayersAppear()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (!(networkRunner == null) && networkRunner.IsRunning && networkRunner.IsSharedModeMasterClient && _runtimeModel.TryGetController(out var controller) && controller.HasStateAuthority && !controller.IsVisible && ShouldAppearForDeadPlayersWithoutDeadParts(networkRunner))
			{
				controller.RequestAppear(KrakenAppearReason.DeadPlayersWithoutDeadParts);
			}
		}

		private bool ShouldAppearForDeadPlayersWithoutDeadParts(NetworkRunner runner)
		{
			if (!_playerDeadPartModel.HasAnyFreeSpawnedDeadPart())
			{
				return _deadPartThrowTargetService.HasDeadPlayers(runner);
			}
			return false;
		}

		private async UniTaskVoid TryStartHelpThrowAsync(KrakenController controller, KrakenHelpThrowRequest request)
		{
			_isHelpThrowInProgress = true;
			try
			{
				if (await controller.RequestHelpThrowAsync(request))
				{
					_krakenHelpThrowModel.TryConsumeHelpThrow();
				}
			}
			finally
			{
				_isHelpThrowInProgress = false;
			}
		}

		private bool TryEvaluateHelpThrow(out KrakenController controller, out KrakenHelpThrowRequest request)
		{
			controller = null;
			request = default(KrakenHelpThrowRequest);
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning || !networkRunner.IsSharedModeMasterClient)
			{
				return false;
			}
			if (!_runtimeModel.TryGetController(out controller) || !controller.HasStateAuthority || !controller.CanAcceptInteraction)
			{
				return false;
			}
			if (_krakenHelpThrowModel.RemainingHelpThrows <= 0)
			{
				return false;
			}
			if (!ShouldAppearForDeadPlayersWithoutDeadParts(networkRunner))
			{
				return false;
			}
			if (!_deadPartThrowTargetService.TryFindNearestBeachAliveTarget(controller.transform.position, out var target))
			{
				return false;
			}
			Vector3 helpDeadPartSpawnPosition = controller.HelpDeadPartSpawnPosition;
			request = new KrakenHelpThrowRequest(target.PlayerRef, target.TargetPosition, helpDeadPartSpawnPosition);
			return true;
		}
	}
}
