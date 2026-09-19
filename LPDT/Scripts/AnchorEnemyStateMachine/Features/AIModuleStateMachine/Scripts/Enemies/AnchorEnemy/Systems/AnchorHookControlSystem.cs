using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.GrabModule.Scripts.PhysGrab;
using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class AnchorHookControlSystem : MonoSystem
	{
		[SerializeField]
		private AnchorHookController _hookController;

		[SerializeField]
		private AnchorItemView _anchorView;

		private AnchorEnemyContext _context;

		private PlayersRagdollModel _playersRagdollModel;

		private bool _isEnabled;

		private bool _hitPlayer;

		private bool _missed;

		private int _hookedPlayerId = -1;

		public bool HitPlayer => _hitPlayer;

		public bool Missed => _missed;

		public bool HasGrabbedPlayer => _hookController.HasGrabbedPlayer;

		public int HookedPlayerId => _hookedPlayerId;

		public float CarrierDistanceToRoot => _hookController.CarrierDistanceToRoot;

		public float ArriveDistance => _hookController.ArriveDistance;

		public float LaunchAimHeight
		{
			get
			{
				if (_anchorView != null && _anchorView.HasRestReference)
				{
					return _anchorView.RestWorldY;
				}
				return _hookController.LaunchHeight;
			}
		}

		public bool IsAnchorSettled
		{
			get
			{
				if (_anchorView != null)
				{
					return _anchorView.IsAnchorSettled;
				}
				return false;
			}
		}

		public float MinFlightTimeBeforeSettle
		{
			get
			{
				if (!(_anchorView != null))
				{
					return 0.15f;
				}
				return _anchorView.MinFlightTimeBeforeSettle;
			}
		}

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(AnchorEnemyContext context, PlayersRagdollModel playersRagdollModel)
		{
			_context = context;
			_playersRagdollModel = playersRagdollModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			_hookController.OnPlayerHit += HandlePlayerHit;
			_hookController.OnMissed += HandleMissed;
			_hookController.OnGrabReleased += HandleGrabReleased;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_hookController.OnPlayerHit -= HandlePlayerHit;
			_hookController.OnMissed -= HandleMissed;
			_hookController.OnGrabReleased -= HandleGrabReleased;
			ClearHookRagdoll();
			base.Despawned(runner, hasState);
		}

		public override void Enable()
		{
			_isEnabled = true;
		}

		public override void Disable()
		{
			_isEnabled = false;
		}

		public override void Clear()
		{
			ClearLatches();
		}

		public void Launch(Vector3 targetPosition)
		{
			ClearLatches();
			_anchorView?.PrepareForLaunch();
			_hookController.Launch(targetPosition);
			_anchorView?.LaunchWithImpulse(targetPosition, _context.ThrowFlightTime, 0f, _context.ThrowSpeedMultiplier, _context.MaxHookDistance);
			_context.SetAnchorThrown(value: true);
		}

		public void Reel()
		{
			if (_anchorView == null || !_anchorView.IsLatchedToPlayer)
			{
				TeleportCarrierToAnchor();
			}
			_anchorView?.BeginSpringReel();
			int num;
			float num2;
			if (!HasGrabbedPlayer)
			{
				if (_anchorView != null)
				{
					num = (_anchorView.IsLatchedToPlayer ? 1 : 0);
					if (num != 0)
					{
						goto IL_0069;
					}
				}
				else
				{
					num = 0;
				}
				num2 = _context.ReelNearSpeed;
				goto IL_0074;
			}
			num = 1;
			goto IL_0069;
			IL_0069:
			num2 = _context.ReelPlayerNearSpeed;
			goto IL_0074;
			IL_0074:
			float nearSpeed = num2;
			float farSpeed = ((num != 0) ? _context.ReelPlayerFarSpeed : _context.ReelFarSpeed);
			_hookController.Reel(nearSpeed, farSpeed, _context.MaxHookDistance);
		}

		public void ResetHook()
		{
			ClearLatches();
			_hookController.ResetHook();
			ClearHookRagdoll();
			_anchorView?.ReturnToRest();
			_context.SetAnchorThrown(value: false);
		}

		public void NotifyEmptyFlightEnded()
		{
			_missed = true;
		}

		private void TeleportCarrierToAnchor()
		{
			if (!(_anchorView == null))
			{
				Vector3 anchorWorldPosition = _anchorView.AnchorWorldPosition;
				_hookController.TeleportCarrierTo(anchorWorldPosition);
				_anchorView.TeleportSphereTo(anchorWorldPosition);
			}
		}

		private void ClearLatches()
		{
			_hitPlayer = false;
			_missed = false;
		}

		private void HandlePlayerHit()
		{
			_hitPlayer = true;
			_hookedPlayerId = _hookController.GrabbedPlayerId;
			ApplyHookRagdoll(_hookedPlayerId);
		}

		private void HandleMissed(AnchorMissReason _)
		{
			_missed = true;
		}

		private void HandleGrabReleased(GrabReleaseReason reason)
		{
			_anchorView?.ClearPlayerLatch();
			ClearHookRagdoll();
			if (reason == GrabReleaseReason.VictimLost)
			{
				_context.SetPriorityPlayer(null);
			}
		}

		private void ApplyHookRagdoll(int playerId)
		{
			if (playerId >= 0 && _playersRagdollModel.TryGetPlayerRagdoll(playerId, out var ragdoll) && ragdoll.IsSpawnedAndValid())
			{
				ragdoll.AddSimulationReason(RagdollSimulationReasonEnum.EnemyGrab);
				ragdoll.SetSimulateHandsRagdoll(simulate: false);
			}
		}

		private void ClearHookRagdoll()
		{
			if (_hookedPlayerId >= 0)
			{
				int hookedPlayerId = _hookedPlayerId;
				_hookedPlayerId = -1;
				if (_playersRagdollModel.TryGetPlayerRagdoll(hookedPlayerId, out var ragdoll) && ragdoll.IsSpawnedAndValid())
				{
					ragdoll.RemoveSimulationReason(RagdollSimulationReasonEnum.EnemyGrab);
					ragdoll.SetSimulateHandsRagdoll(simulate: true);
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
