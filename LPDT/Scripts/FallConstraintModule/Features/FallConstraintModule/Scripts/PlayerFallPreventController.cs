using Cysharp.Threading.Tasks;
using Features.LevelModule.Scripts;
using Features.Movement.Scripts;
using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Features.SessionManagementModule.Models;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.FallConstraintModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerFallPreventController : NetworkBehaviour
	{
		[SerializeField]
		private PlayerCharacterMovableBase _playerCharacterMovableBase;

		[SerializeField]
		private NetworkObject _networkObject;

		private FallConstraintModel _fallConstraintModel;

		private TeleportationPointsEventClass _teleportationPointsEventClass;

		private ILoadingScreenService _loadingScreenService;

		private SessionStateMachine _sessionStateMachine;

		private bool _isTeleportPending;

		private bool _unsanctionedLogged;

		[Inject]
		public void InjectDependencies(FallConstraintModel fallConstraintModel, TeleportationPointsEventClass teleportationPointsEventClass, ILoadingScreenService loadingScreenService, SessionStateMachine sessionStateMachine)
		{
			_fallConstraintModel = fallConstraintModel;
			_teleportationPointsEventClass = teleportationPointsEventClass;
			_loadingScreenService = loadingScreenService;
			_sessionStateMachine = sessionStateMachine;
		}

		public override void Spawned()
		{
			if (_networkObject.HasInputAuthority)
			{
				_teleportationPointsEventClass.OnPlayerTeleported += OnPlayerTeleported;
				_teleportationPointsEventClass.OnCancelPendingTeleportsRequested += OnCancelPendingTeleports;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_teleportationPointsEventClass.OnPlayerTeleported -= OnPlayerTeleported;
			_teleportationPointsEventClass.OnCancelPendingTeleportsRequested -= OnCancelPendingTeleports;
		}

		private void OnPlayerTeleported()
		{
			_isTeleportPending = false;
		}

		private void OnCancelPendingTeleports()
		{
			_isTeleportPending = false;
		}

		private void Update()
		{
			if (!_networkObject.HasInputAuthority || _teleportationPointsEventClass.BeachTeleportationPoints.Count == 0)
			{
				return;
			}
			Vector3 position = _playerCharacterMovableBase.GetPosition();
			if (!_fallConstraintModel.FallConstraints.TryGetValue(FallConstraintType.Lower, out var value) || !_fallConstraintModel.FallConstraints.TryGetValue(FallConstraintType.Upper, out var value2) || value == null || value2 == null)
			{
				return;
			}
			if (!(position.y <= value.position.y) && !(position.y >= value2.position.y))
			{
				_isTeleportPending = false;
				_unsanctionedLogged = false;
			}
			else
			{
				if (_isTeleportPending)
				{
					return;
				}
				if (_sessionStateMachine.Target.Value != SessionState.Level || !_sessionStateMachine.IsTargetStateActive)
				{
					if (!_unsanctionedLogged)
					{
						Debug.LogWarning($"[FallPrevent] Suppressed unsanctioned beach teleport for p{_networkObject.InputAuthority.PlayerId} — body out of bounds at {position} but session Target={_sessionStateMachine.Target.Value} active={_sessionStateMachine.IsTargetStateActive} (beach recovery only fires in an active Level).");
						_unsanctionedLogged = true;
					}
				}
				else
				{
					PreventFall();
				}
			}
		}

		private void PreventFall()
		{
			_isTeleportPending = true;
			Debug.Log($"[FallPrevent] Out-of-bounds beach recovery for p{_networkObject.InputAuthority.PlayerId} — teleporting to beach from {_playerCharacterMovableBase.GetPosition()} (Target={_sessionStateMachine.Target.Value}).");
			_loadingScreenService.Show(LoadingScreenShowType.ShowFadeInstant);
			_teleportationPointsEventClass.InvokeTeleportationToBeach();
			_playerCharacterMovableBase.SetLinearVelocity(Vector3.zero);
			_loadingScreenService.HideAsync(LoadingScreenShowType.ShowFade).Forget();
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
