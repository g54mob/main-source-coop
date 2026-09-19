using System.Collections;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.DamageableTrackModule.Scripts;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class AnchorPlayerGrabSystem : MonoSystem
	{
		private const float EnemyGrabHandoffTimeout = 0.75f;

		[SerializeField]
		private PlayerGrabHolder _handGrabHolder;

		private AnchorEnemyContext _context;

		private IDetectionContext _detectionContext;

		private PlayerGrabSimplePointGrabableModel _playerGrabModel;

		private PlayersRagdollModel _playersRagdollModel;

		private PlayerEnemyInteractionBlocksModel _interactionBlocksModel;

		private PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private PlayerStatesConfiguration _playerStatesConfiguration;

		private bool _isEnabled;

		private int _grabbedPlayerId = -1;

		private int _pendingEnemyGrabPlayerId = -1;

		private Coroutine _enemyGrabHandoffRoutine;

		private bool _heldPlayerDeparted;

		public int GrabbedPlayerId => _grabbedPlayerId;

		public bool IsHolding => _grabbedPlayerId >= 0;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(AnchorEnemyContext context, IDetectionContext detectionContext, PlayerGrabSimplePointGrabableModel playerGrabModel, PlayersRagdollModel playersRagdollModel, PlayerEnemyInteractionBlocksModel interactionBlocksModel, PlayerDamageablesTrackModel playerDamageablesTrackModel, PlayerStatesConfiguration playerStatesConfiguration)
		{
			_context = context;
			_detectionContext = detectionContext;
			_playerGrabModel = playerGrabModel;
			_playersRagdollModel = playersRagdollModel;
			_interactionBlocksModel = interactionBlocksModel;
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
			_playerStatesConfiguration = playerStatesConfiguration;
		}

		public override void Enable()
		{
			_isEnabled = true;
			_heldPlayerDeparted = false;
			SubscribeHolder();
			TryGrabPriorityPlayer();
		}

		public override void Disable()
		{
			_isEnabled = false;
			UnsubscribeHolder();
			Clear();
		}

		public override void Clear()
		{
			if (_grabbedPlayerId >= 0)
			{
				_handGrabHolder.ReleaseGrab();
				CleanupGrabbedPlayer(removeEnemyGrab: true);
				ForceRemovePendingEnemyGrab();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			UnsubscribeHolder();
			ForceRemovePendingEnemyGrab();
			base.Despawned(runner, hasState);
		}

		public bool ConsumeHeldPlayerDeparted()
		{
			if (!_heldPlayerDeparted)
			{
				return false;
			}
			_heldPlayerDeparted = false;
			return true;
		}

		public void ThrowHeldPlayer()
		{
			if (!base.HasStateAuthority || _grabbedPlayerId < 0)
			{
				return;
			}
			int grabbedPlayerId = _grabbedPlayerId;
			NetworkObject networkObject = ResolveVictimObject(grabbedPlayerId);
			if (networkObject == null || !networkObject.IsValid)
			{
				UnsubscribeHolder();
				if (_handGrabHolder != null && _handGrabHolder.IsGrabbing)
				{
					_handGrabHolder.ReleaseGrab();
				}
				CleanupGrabbedPlayer(removeEnemyGrab: true);
				ForceRemovePendingEnemyGrab();
				return;
			}
			UnsubscribeHolder();
			_handGrabHolder.ReleaseGrab();
			if (_playerDamageablesTrackModel.AllPlayerDamageables.TryGetValue(grabbedPlayerId, out var value))
			{
				Vector3 forward = base.transform.forward;
				forward.y = 0f;
				if (forward.sqrMagnitude < 1E-06f)
				{
					forward = Vector3.forward;
				}
				forward.Normalize();
				Vector3 normalized = (forward + Vector3.up * _context.ThrowUpBias).normalized;
				float force = _context.ThrowForce * _playerStatesConfiguration.StunThrowMultiplier;
				value.Damage(new DamageData
				{
					Damage = _context.ThrowDamage,
					Direction = normalized,
					Force = force,
					DamageDealerPlayerID = base.Object.StateAuthority.PlayerId,
					ForceMode = ForceMode.Impulse,
					IsStunning = true,
					StunDurationPreset = _context.ThrowStunDurationPreset,
					Source = DamageDataSourceExtensions.ForEnemyAttack(_context.transform, EnemyType.Anchor.ToString(), DamageType.Melee)
				});
			}
			CleanupGrabbedPlayer(removeEnemyGrab: false);
			BeginEnemyGrabHandoff(grabbedPlayerId);
		}

		private void OnVictimLost()
		{
			_heldPlayerDeparted = true;
			_detectionContext.SetPriorityPlayer(null);
		}

		private void OnHolderReleased(GrabReleaseReason _)
		{
			CleanupGrabbedPlayer(removeEnemyGrab: true);
			ForceRemovePendingEnemyGrab();
		}

		private void SubscribeHolder()
		{
			if (!(_handGrabHolder == null))
			{
				_handGrabHolder.OnVictimLost -= OnVictimLost;
				_handGrabHolder.OnReleased -= OnHolderReleased;
				_handGrabHolder.OnVictimLost += OnVictimLost;
				_handGrabHolder.OnReleased += OnHolderReleased;
			}
		}

		private void UnsubscribeHolder()
		{
			if (!(_handGrabHolder == null))
			{
				_handGrabHolder.OnVictimLost -= OnVictimLost;
				_handGrabHolder.OnReleased -= OnHolderReleased;
			}
		}

		private void TryGrabPriorityPlayer()
		{
			if (!base.HasStateAuthority)
			{
				return;
			}
			PlayerDataHolder priorityPlayer = _detectionContext.PriorityPlayer;
			if (priorityPlayer == null || priorityPlayer.NetworkObject == null)
			{
				return;
			}
			int playerId = priorityPlayer.NetworkObject.InputAuthority.PlayerId;
			if (_interactionBlocksModel.IsBlocked(playerId) || !_context.TryGetPriorityPlayerTrackingHorizontalDistance(out var distance))
			{
				return;
			}
			float handGrabRadius = _context.HandGrabRadius;
			if ((!(handGrabRadius > 0f) || !(distance > handGrabRadius)) && _playerGrabModel.PlayerGrabables.TryGetValue(playerId, out var value) && value is SimplePointGrabable grabable && _handGrabHolder.TryGrab(grabable))
			{
				_grabbedPlayerId = playerId;
				_heldPlayerDeparted = false;
				if (_playersRagdollModel.TryGetPlayerRagdoll(playerId, out var ragdoll) && ragdoll.IsSpawnedAndValid())
				{
					ragdoll.AddSimulationReason(RagdollSimulationReasonEnum.EnemyGrab);
					ragdoll.SetSimulateHandsRagdoll(simulate: false);
				}
				_interactionBlocksModel.SetBlock(playerId, PlayerEnemyInteractionBlockReason.OccupiedByEnemy);
			}
		}

		private void CleanupGrabbedPlayer(bool removeEnemyGrab)
		{
			if (_grabbedPlayerId >= 0)
			{
				int grabbedPlayerId = _grabbedPlayerId;
				if (removeEnemyGrab)
				{
					RemoveEnemyGrabReason(grabbedPlayerId);
				}
				_interactionBlocksModel.ClearBlock(grabbedPlayerId, PlayerEnemyInteractionBlockReason.OccupiedByEnemy);
				_grabbedPlayerId = -1;
			}
		}

		private void BeginEnemyGrabHandoff(int playerId)
		{
			ForceRemovePendingEnemyGrab();
			_pendingEnemyGrabPlayerId = playerId;
			_enemyGrabHandoffRoutine = StartCoroutine(EnemyGrabHandoffRoutine(playerId));
		}

		private IEnumerator EnemyGrabHandoffRoutine(int playerId)
		{
			float elapsed = 0f;
			while (elapsed < 0.75f && !HasThrowReason(playerId))
			{
				elapsed += Time.deltaTime;
				yield return null;
			}
			if (_pendingEnemyGrabPlayerId == playerId)
			{
				RemoveEnemyGrabReason(playerId);
				_pendingEnemyGrabPlayerId = -1;
			}
			_enemyGrabHandoffRoutine = null;
		}

		private void ForceRemovePendingEnemyGrab()
		{
			if (_enemyGrabHandoffRoutine != null)
			{
				StopCoroutine(_enemyGrabHandoffRoutine);
				_enemyGrabHandoffRoutine = null;
			}
			if (_pendingEnemyGrabPlayerId >= 0)
			{
				RemoveEnemyGrabReason(_pendingEnemyGrabPlayerId);
				_pendingEnemyGrabPlayerId = -1;
			}
		}

		private bool HasThrowReason(int playerId)
		{
			if (_playersRagdollModel.TryGetPlayerRagdoll(playerId, out var ragdoll) && ragdoll.IsSpawnedAndValid())
			{
				return ragdoll.HasSimulationReason(RagdollSimulationReasonEnum.Throw);
			}
			return false;
		}

		private void RemoveEnemyGrabReason(int playerId)
		{
			if (_playersRagdollModel.TryGetPlayerRagdoll(playerId, out var ragdoll) && ragdoll.IsSpawnedAndValid())
			{
				ragdoll.RemoveSimulationReason(RagdollSimulationReasonEnum.EnemyGrab);
				ragdoll.SetSimulateHandsRagdoll(simulate: true);
			}
		}

		private NetworkObject ResolveVictimObject(int playerId)
		{
			if (_playerDamageablesTrackModel.AllPlayerDamageables.TryGetValue(playerId, out var value) && value?.NetworkObject != null && value.NetworkObject.IsValid)
			{
				return value.NetworkObject;
			}
			if (_playerGrabModel.PlayerGrabables.TryGetValue(playerId, out var value2) && value2 is SimplePointGrabable simplePointGrabable && simplePointGrabable.NetworkObject != null && simplePointGrabable.NetworkObject.IsValid)
			{
				return simplePointGrabable.NetworkObject;
			}
			return null;
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
