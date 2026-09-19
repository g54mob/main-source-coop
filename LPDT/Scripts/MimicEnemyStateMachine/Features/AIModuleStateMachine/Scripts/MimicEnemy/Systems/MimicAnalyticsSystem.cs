using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.PlayerSpawner.Scripts;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class MimicAnalyticsSystem : MonoSystem
	{
		private MimicEnemyContext _context;

		private SessionAnalyticsModel _sessionAnalyticsModel;

		private bool _enabled;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(MimicEnemyContext context, SessionAnalyticsModel sessionAnalyticsModel)
		{
			_context = context;
			_sessionAnalyticsModel = sessionAnalyticsModel;
		}

		public override void Enable()
		{
			_enabled = true;
			_context.OnDetectedPlayersChanged += ProcessPriorityPlayerChanged;
		}

		public override void Disable()
		{
			_enabled = false;
			_context.OnDetectedPlayersChanged -= ProcessPriorityPlayerChanged;
			Clear();
		}

		public override void Clear()
		{
		}

		private void ProcessPriorityPlayerChanged()
		{
			foreach (PlayerDataHolder detectedPlayer in _context.DetectedPlayers)
			{
				_sessionAnalyticsModel.RegisterEnemyTarget(detectedPlayer.NetworkObject.InputAuthority.PlayerId);
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
