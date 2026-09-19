using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.PlayerSpawner.Scripts;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class ParrotMobPlayerDetectionSyncSystem : MonoSystem
	{
		private ParrotMobEnemyContext _context;

		private bool _isEnabled;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(ParrotMobEnemyContext context)
		{
			_context = context;
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
			_context.SetDetectedPlayers(new List<PlayerDataHolder>());
			_context.SetPriorityPlayer(null);
		}

		private void LateUpdate()
		{
			if (!base.Initialized || !_isEnabled || !base.HasStateAuthority)
			{
				return;
			}
			PlayerDataHolder playerDataHolder = (_context.PlayerTriggerSensor.HasPlayerInside ? _context.PlayerTriggerSensor.PlayerInsideTrigger : null);
			if (!HasSameDetectedPlayer(playerDataHolder))
			{
				PlayerDataHolder priorityPlayer = _context.PriorityPlayer;
				if (playerDataHolder == null || priorityPlayer == null || priorityPlayer != playerDataHolder)
				{
					_context.IsZoneEngagementActive = false;
				}
				List<PlayerDataHolder> detectedPlayers = ((playerDataHolder != null) ? new List<PlayerDataHolder> { playerDataHolder } : new List<PlayerDataHolder>());
				_context.SetDetectedPlayers(detectedPlayers);
				_context.SetPriorityPlayer(playerDataHolder);
			}
		}

		private bool HasSameDetectedPlayer(PlayerDataHolder visiblePlayer)
		{
			if (visiblePlayer == null)
			{
				return _context.DetectedPlayers.Count == 0;
			}
			if (_context.DetectedPlayers.Count == 1)
			{
				return _context.DetectedPlayers[0] == visiblePlayer;
			}
			return false;
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
