using System;
using System.Collections;
using System.Collections.Generic;
using Features.CoroutineUtils.Scripts;
using Features.GrabModule.Scripts;
using Features.LineArmModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.DeadPartsModule.Scripts
{
	public class DeadPartConnectSystem : IInitializable, IDisposable, ITickable
	{
		private readonly IPlayerResurrectionService _playerResurrectionService;

		private readonly PlayerDeadPartsConfiguration _configuration;

		private readonly PlayerDeadPartModel _playerDeadPartModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ICoroutineRunner _coroutineRunner;

		private readonly LineArmsModel _lineArmsModel;

		private readonly IPlayerStateService _playerStateService;

		private readonly PlayerDeadPartTypes _playerDeadPartTypes;

		private Coroutine _activeConnectionCoroutine;

		private bool _isResurrectionInProgress;

		private List<PlayerDeadPart> _inactiveDeadParts = new List<PlayerDeadPart>();

		public DeadPartConnectSystem(PlayerDeadPartModel playerDeadPartModel, MultiplayerModel multiplayerModel, PlayerDeadPartsConfiguration configuration, ICoroutineRunner coroutineRunner, IPlayerResurrectionService playerResurrectionService, LineArmsModel lineArmsModel, IPlayerStateService playerStateService, PlayerDeadPartTypes playerDeadPartTypes)
		{
			_playerDeadPartModel = playerDeadPartModel;
			_multiplayerModel = multiplayerModel;
			_configuration = configuration;
			_coroutineRunner = coroutineRunner;
			_playerResurrectionService = playerResurrectionService;
			_lineArmsModel = lineArmsModel;
			_playerStateService = playerStateService;
			_playerDeadPartTypes = playerDeadPartTypes;
		}

		public void Initialize()
		{
		}

		public void Dispose()
		{
			StopCurrentConnectionCoroutine();
		}

		public void Tick()
		{
			if (_playerStateService.IsPlayerAlive(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				_isResurrectionInProgress = false;
			}
			else
			{
				if (_isResurrectionInProgress)
				{
					return;
				}
				PlayerAlivePart playerAlivePart = _playerDeadPartModel.PlayerAlivePart;
				if (!(playerAlivePart == null))
				{
					_inactiveDeadParts.Clear();
					foreach (PlayerDeadPart item in _playerDeadPartModel.DeadPartsInRange)
					{
						if (item == null)
						{
							_inactiveDeadParts.Add(item);
							continue;
						}
						if (!item.gameObject.activeSelf)
						{
							return;
						}
						if (!(item.SpawnTimer < _configuration.ResurrectionMinTime) && !(item.UpperPos == null) && !(playerAlivePart.DownPos == null) && !(Vector3.Distance(item.UpperPos.position, playerAlivePart.DownPos.position) > _configuration.TargetResurrectionDistance))
						{
							Vector3 vector = playerAlivePart.DownPos.position - playerAlivePart.UpperPos.position;
							Vector3 to = item.UpperPos.position - playerAlivePart.UpperPos.position;
							if (Vector3.Angle(vector, to) <= _configuration.ResurrectionMaxAngle)
							{
								_isResurrectionInProgress = true;
								_activeConnectionCoroutine = _coroutineRunner.StartCoroutine(ConnectDeadPartCoroutine(item));
								break;
							}
						}
					}
					{
						foreach (PlayerDeadPart inactiveDeadPart in _inactiveDeadParts)
						{
							_playerDeadPartModel.RemoveDeadPart(inactiveDeadPart);
						}
						return;
					}
				}
				StopCurrentConnectionCoroutine();
			}
		}

		private void StopCurrentConnectionCoroutine()
		{
			if (_activeConnectionCoroutine != null)
			{
				_coroutineRunner.StopCoroutine(_activeConnectionCoroutine);
				_activeConnectionCoroutine = null;
			}
			_isResurrectionInProgress = false;
		}

		private IEnumerator ConnectDeadPartCoroutine(PlayerDeadPart playerDeadPart)
		{
			if (playerDeadPart.GetComponent<NetworkObject>() == null)
			{
				StopCurrentConnectionCoroutine();
				yield break;
			}
			if (playerDeadPart.BodyRigidbody == null)
			{
				StopCurrentConnectionCoroutine();
				yield break;
			}
			foreach (SimplePointGrabable grabbable in playerDeadPart.Grabbables)
			{
				grabbable.BlockGrabRPC();
			}
			playerDeadPart.Use();
			_playerDeadPartModel.RemoveDeadPart(playerDeadPart);
			List<int> list = new List<int>();
			foreach (SimplePointGrabable item in new List<SimplePointGrabable>(playerDeadPart.Grabbables))
			{
				foreach (int item2 in new List<int>(item.GrabbedByPlayers))
				{
					list.Add(item2);
				}
			}
			foreach (int item3 in list)
			{
				if (_lineArmsModel.TryGetLineArmForPlayer(item3, out var lineArm))
				{
					lineArm.UnJoinAll(throwItem: false);
				}
			}
			OnRotationComplete(playerDeadPart);
		}

		private void OnRotationComplete(PlayerDeadPart playerDeadPart)
		{
			_activeConnectionCoroutine = null;
			if (!(playerDeadPart == null))
			{
				_playerDeadPartTypes.SetLocalBottomPart(playerDeadPart.DeadPartType, playerDeadPart.CustomizationData, playerDeadPart.UsageCount);
				ResurrectPlayer(playerDeadPart);
			}
		}

		private void ResurrectPlayer(PlayerDeadPart playerDeadPart)
		{
			int playerId = _playerDeadPartModel.PlayerAlivePart.Object.InputAuthority.PlayerId;
			Debug.Log($"[HealthTrace] Resurrect via DEAD-PART CONNECT p{playerId} partOwner=p{playerDeadPart.Object.StateAuthority.PlayerId} usage={playerDeadPart.UsageCount}");
			_playerDeadPartModel.PlayerAlivePart.SetDeadPartUsageCount(playerDeadPart.UsageCount);
			_playerDeadPartModel.PlayerAlivePart.DeadPartJoinBoosterBehaviour.ApplyDeadPartEffect(playerDeadPart.BoosterSetting);
			_playerResurrectionService.ResurrectPlayer(playerId, restoreHp: true, playerDeadPart.Object.StateAuthority.PlayerId);
			playerDeadPart.DespawnPart();
		}
	}
}
