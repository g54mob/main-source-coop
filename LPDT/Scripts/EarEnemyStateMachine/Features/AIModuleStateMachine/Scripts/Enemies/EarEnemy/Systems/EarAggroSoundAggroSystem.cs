using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.Settings;
using Features.EntitiesSoundOcclusionModule.Scripts.EnemiesSoundOcclusion;
using Fusion;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class EarAggroSoundAggroSystem : MonoSystem
	{
		private EarEnemyContext _context;

		private EarEnemySettings _settings;

		private PlayerSoundSourceGateService _playerSoundSourceGateService;

		private EarEnemy _enemy;

		private bool _isEnabled;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		public void InjectDependencies(EarEnemyContext context, EarEnemy enemy, EarEnemySettings settings, PlayerSoundSourceGateService playerSoundSourceGateService)
		{
			_context = context;
			_enemy = enemy;
			_settings = settings;
			_playerSoundSourceGateService = playerSoundSourceGateService;
		}

		public override void Enable()
		{
			if (base.HasStateAuthority)
			{
				_isEnabled = true;
				_context.OnEnemyTriggeredBySound += OnSoundHeard;
			}
		}

		public override void Disable()
		{
			if (base.HasStateAuthority)
			{
				_isEnabled = false;
				_context.OnEnemyTriggeredBySound -= OnSoundHeard;
			}
		}

		public override void Clear()
		{
		}

		private void OnSoundHeard(HeardSound heardSound)
		{
			if (!_playerSoundSourceGateService.IsSoundFromOutsideGatePlayer(heardSound) && !_settings.IsSoundPathIgnored(heardSound.SoundPath) && ((heardSound.SourceId != 0 && heardSound.SourceId == _context.SoundTargetSourceId) || !(heardSound.Loudness <= _context.SoundTargetLoudness)))
			{
				_context.SetSoundTarget(heardSound);
				_enemy.TryRegisterEncounterFromHeardSound(heardSound);
				_context.SetDestinationToReachablePoint(heardSound.Position);
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
