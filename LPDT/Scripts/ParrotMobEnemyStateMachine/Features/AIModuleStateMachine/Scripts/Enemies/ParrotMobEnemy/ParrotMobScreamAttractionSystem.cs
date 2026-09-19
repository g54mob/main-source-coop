using Features.AIModule.Scripts.AttractionZone;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.Settings;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class ParrotMobScreamAttractionSystem : MonoSystem
	{
		private ParrotMobEnemy _enemy;

		private ParrotMobEnemyContext _context;

		private IEnemyAttractionZoneService _attractionZoneService;

		private ParrotMobScreamSettings _screamSettings;

		private bool _isScreamCallActive;

		private bool _wasScreaming;

		public override bool IsEnabled => true;

		[Inject]
		public void InjectDependencies(ParrotMobEnemy enemy, ParrotMobEnemyContext context, IEnemyAttractionZoneService attractionZoneService, ParrotMobScreamSettings screamSettings)
		{
			_enemy = enemy;
			_context = context;
			_attractionZoneService = attractionZoneService;
			_screamSettings = screamSettings;
		}

		public override void Enable()
		{
		}

		public override void Disable()
		{
		}

		public override void Clear()
		{
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			CancelScreamCall();
			base.Despawned(runner, hasState);
		}

		public override void FixedUpdateNetwork()
		{
			if (!base.Initialized)
			{
				return;
			}
			if (!base.HasStateAuthority)
			{
				if (_isScreamCallActive)
				{
					CancelScreamCall();
				}
			}
			else if (!_context.IsDead)
			{
				UpdateScreamAttraction();
			}
		}

		private void UpdateScreamAttraction()
		{
			bool flag = _enemy.CurrentStateId == ParrotMobStateId.Scream;
			if (flag && !_wasScreaming)
			{
				BroadcastScreamCall();
			}
			if (!flag && _wasScreaming && _isScreamCallActive)
			{
				CancelScreamCall();
			}
			_wasScreaming = flag;
		}

		private void BroadcastScreamCall()
		{
			if (_attractionZoneService != null && !(_screamSettings == null))
			{
				_isScreamCallActive = true;
				_attractionZoneService.RaiseZone(new AttractionZoneData(_enemy.gameObject.GetInstanceID(), _enemy.EnemyType, _enemy.transform.position, _screamSettings.AttractRadius, _screamSettings.ApproachRadius));
			}
		}

		private void CancelScreamCall()
		{
			_isScreamCallActive = false;
			if (_attractionZoneService != null && !(_enemy == null))
			{
				_attractionZoneService.EndZone(_enemy.gameObject.GetInstanceID());
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
