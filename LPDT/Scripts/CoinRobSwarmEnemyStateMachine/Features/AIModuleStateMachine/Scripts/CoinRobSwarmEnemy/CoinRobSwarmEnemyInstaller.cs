using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.Core.Settings;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy
{
	public class CoinRobSwarmEnemyInstaller : MonoInstaller
	{
		[SerializeField]
		private CoinRobSwarmEnemySettings _swarmSettings;

		[SerializeField]
		private CoinRobChaseSettings _chaseSettings;

		[SerializeField]
		private CoinRobCombatSettings _combatSettings;

		[SerializeField]
		private CoinRobStealSettings _stealSettings;

		[SerializeField]
		private FearHoleAbsorbAnimationSettings _fearHoleAbsorbAnimationSettings;

		[SerializeField]
		private CoinRobCauldronSettings _cauldronSettings;

		[SerializeField]
		private CoinRobSwarmEnemyContext _context;

		[SerializeField]
		private CoinRobSwarmEnemy _enemy;

		[SerializeField]
		private CoinRobSwarmAnimatorPresenter _presenter;

		public override void InstallBindings()
		{
			base.Container.Bind<CoinRobSwarmEnemySettings>().FromInstance(_swarmSettings).AsSingle();
			base.Container.Bind<CoinRobChaseSettings>().FromInstance(_chaseSettings).AsSingle();
			base.Container.Bind<CoinRobCombatSettings>().FromInstance(_combatSettings).AsSingle();
			base.Container.Bind<CoinRobStealSettings>().FromInstance(_stealSettings).AsSingle();
			base.Container.Bind<FearHoleAbsorbAnimationSettings>().FromInstance(_fearHoleAbsorbAnimationSettings).AsSingle();
			base.Container.Bind<CoinRobCauldronSettings>().FromInstance(_cauldronSettings).AsSingle();
			base.Container.Bind<CoinRobSwarmEnemyContext>().FromInstance(_context).AsSingle();
			base.Container.Bind<CoinRobSwarmEnemy>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<CoinRobSwarmAnimatorPresenter>().FromInstance(_presenter).AsSingle();
			base.Container.Bind<CoinRobTargetSensor>().AsSingle();
			base.Container.Bind<CoinRobPlayerProximityService>().AsSingle();
			base.Container.Bind<CoinRobPlayerAttackValidator>().AsSingle();
			base.Container.Bind<CoinRobSwarmFormationService>().AsSingle();
			base.Container.Bind<CoinRobSwarmSpawnPositionService>().AsSingle();
		}
	}
}
