using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.PlayerSpawner.Scripts;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Detection
{
	[NetworkBehaviourWeaved(0)]
	public class EnemyDetectionAnalyticsSystem : MonoSystem
	{
		private IDetectionContext _detectionContext;

		private SessionAnalyticsModel _sessionAnalyticsModel;

		private bool _enabled;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(IDetectionContext detectionContext, SessionAnalyticsModel sessionAnalyticsModel)
		{
			_detectionContext = detectionContext;
			_sessionAnalyticsModel = sessionAnalyticsModel;
		}

		public override void Enable()
		{
			_enabled = true;
			_detectionContext.OnDetectedPlayersChanged += ProcessDetectedPlayersChanged;
		}

		public override void Disable()
		{
			_enabled = false;
			_detectionContext.OnDetectedPlayersChanged -= ProcessDetectedPlayersChanged;
			Clear();
		}

		public override void Clear()
		{
		}

		private void ProcessDetectedPlayersChanged()
		{
			foreach (PlayerDataHolder detectedPlayer in _detectionContext.DetectedPlayers)
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
