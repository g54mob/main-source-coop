using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Features.PlayerSpawner.Scripts;
using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerGrabSystem : MonoSystem
	{
		[SerializeField]
		private PlayerGrabHolder _leftPlayerGrabHolder;

		[SerializeField]
		private PlayerGrabHolder _rightPlayerGrabHolder;

		private CrabEnemyContext _crabContext;

		private IDetectionContext _detectionContext;

		private PlayerGrabSimplePointGrabableModel _playerGrabModel;

		private PlayersRagdollModel _playersRagdollModel;

		private PlayerEnemyInteractionBlocksModel _interactionBlocksModel;

		private bool _isEnabled;

		private int _grabbedPlayerId = -1;

		private PlayerGrabHolder _activeHolder;

		private bool _heldPlayerDeparted;

		private bool _holdTakenByOther;

		public bool IsHolding => _grabbedPlayerId >= 0;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(CrabEnemyContext crabContext, IDetectionContext detectionContext, PlayerGrabSimplePointGrabableModel playerGrabModel, PlayersRagdollModel playersRagdollModel, PlayerEnemyInteractionBlocksModel interactionBlocksModel)
		{
			_crabContext = crabContext;
			_detectionContext = detectionContext;
			_playerGrabModel = playerGrabModel;
			_playersRagdollModel = playersRagdollModel;
			_interactionBlocksModel = interactionBlocksModel;
		}

		public override void Enable()
		{
			_isEnabled = true;
			_heldPlayerDeparted = false;
			_holdTakenByOther = false;
			_crabContext.SetActiveClaw(CrabClawSide.None);
			SubscribeActiveHolder();
			TryGrabPriorityPlayer();
		}

		public override void Disable()
		{
			_isEnabled = false;
			UnsubscribeActiveHolder();
			Clear();
		}

		public override void Clear()
		{
			if (_grabbedPlayerId < 0)
			{
				_activeHolder = null;
				return;
			}
			if (_activeHolder != null)
			{
				_activeHolder.ReleaseGrab();
			}
			CleanupGrabbedPlayer();
			_activeHolder = null;
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

		public bool ConsumeHoldTakenByOther()
		{
			if (!_holdTakenByOther)
			{
				return false;
			}
			_holdTakenByOther = false;
			return true;
		}

		public void ClearActiveClaw()
		{
			_crabContext.SetActiveClaw(CrabClawSide.None);
		}

		private void OnVictimLost()
		{
			_heldPlayerDeparted = true;
		}

		private void OnPlayerHolderReleased(GrabReleaseReason reason)
		{
			if (reason == GrabReleaseReason.TakenByOther)
			{
				_holdTakenByOther = true;
			}
			UnsubscribeActiveHolder();
			CleanupGrabbedPlayer();
			_activeHolder = null;
			if (reason != GrabReleaseReason.TakenByOther)
			{
				_crabContext.SetActiveClaw(CrabClawSide.None);
			}
		}

		private void TryGrabPriorityPlayer()
		{
			if (!base.HasStateAuthority || IsHolding)
			{
				return;
			}
			PlayerDataHolder priorityPlayer = _detectionContext.PriorityPlayer;
			if (priorityPlayer == null || priorityPlayer.NetworkObject == null)
			{
				return;
			}
			int playerId = priorityPlayer.NetworkObject.InputAuthority.PlayerId;
			if (!_playerGrabModel.PlayerGrabables.TryGetValue(playerId, out var value) || !(value is SimplePointGrabable simplePointGrabable) || _interactionBlocksModel.IsBlocked(playerId) || simplePointGrabable.GrabbedByExternalsCount > 0)
			{
				return;
			}
			PlayerGrabHolder playerGrabHolder = SelectNearestHolder(simplePointGrabable.transform.position);
			if (!(playerGrabHolder == null) && playerGrabHolder.TryGrab(simplePointGrabable))
			{
				_activeHolder = playerGrabHolder;
				_grabbedPlayerId = playerId;
				_heldPlayerDeparted = false;
				_holdTakenByOther = false;
				_crabContext.SetActiveClaw((playerGrabHolder == _leftPlayerGrabHolder) ? CrabClawSide.Left : CrabClawSide.Right);
				SubscribeActiveHolder();
				if (_playersRagdollModel.TryGetPlayerRagdoll(playerId, out var ragdoll) && ragdoll.IsSpawnedAndValid())
				{
					ragdoll.AddSimulationReason(RagdollSimulationReasonEnum.EnemyGrab);
					ragdoll.SetSimulateHandsRagdoll(simulate: false);
				}
				_interactionBlocksModel.SetBlock(playerId, PlayerEnemyInteractionBlockReason.OccupiedByEnemy);
			}
		}

		private PlayerGrabHolder SelectNearestHolder(Vector3 grabablePosition)
		{
			bool flag = _leftPlayerGrabHolder != null && _leftPlayerGrabHolder.CarryAnchor != null;
			bool flag2 = _rightPlayerGrabHolder != null && _rightPlayerGrabHolder.CarryAnchor != null;
			if (!flag && !flag2)
			{
				return null;
			}
			if (!flag)
			{
				return _rightPlayerGrabHolder;
			}
			if (!flag2)
			{
				return _leftPlayerGrabHolder;
			}
			float num = Vector3.Distance(_leftPlayerGrabHolder.CarryAnchor.position, grabablePosition);
			float num2 = Vector3.Distance(_rightPlayerGrabHolder.CarryAnchor.position, grabablePosition);
			if (!(num <= num2))
			{
				return _rightPlayerGrabHolder;
			}
			return _leftPlayerGrabHolder;
		}

		private void SubscribeActiveHolder()
		{
			if (!(_activeHolder == null))
			{
				_activeHolder.OnVictimLost -= OnVictimLost;
				_activeHolder.OnReleased -= OnPlayerHolderReleased;
				_activeHolder.OnVictimLost += OnVictimLost;
				_activeHolder.OnReleased += OnPlayerHolderReleased;
			}
		}

		private void UnsubscribeActiveHolder()
		{
			if (!(_activeHolder == null))
			{
				_activeHolder.OnVictimLost -= OnVictimLost;
				_activeHolder.OnReleased -= OnPlayerHolderReleased;
			}
		}

		private void CleanupGrabbedPlayer()
		{
			if (_grabbedPlayerId >= 0)
			{
				int grabbedPlayerId = _grabbedPlayerId;
				if (_playersRagdollModel.TryGetPlayerRagdoll(grabbedPlayerId, out var ragdoll) && ragdoll.IsSpawnedAndValid())
				{
					ragdoll.RemoveSimulationReason(RagdollSimulationReasonEnum.EnemyGrab);
					ragdoll.SetSimulateHandsRagdoll(simulate: true);
				}
				_interactionBlocksModel.ClearBlock(grabbedPlayerId, PlayerEnemyInteractionBlockReason.OccupiedByEnemy);
				_grabbedPlayerId = -1;
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
