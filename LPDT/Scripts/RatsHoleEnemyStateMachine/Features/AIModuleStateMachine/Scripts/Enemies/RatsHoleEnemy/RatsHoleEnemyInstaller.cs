using Features.AIModuleStateMachine.Scripts.Core;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.Settings;
using Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.States;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy
{
	public class RatsHoleEnemyInstaller : MonoInstaller
	{
		[SerializeField]
		private RatsHoleEnemyContext _context;

		[SerializeField]
		private RatsHoleEnemy _enemy;

		[SerializeField]
		private RatsHoleEnemyAttackSettings _attackSettings;

		[SerializeField]
		private RatsHoleEnemyMovementSettings _movementSettings;

		[SerializeField]
		private FearHoleAbsorbAnimationSettings _fearHoleAbsorbAnimationSettings;

		[SerializeField]
		private FearHoleAbsorbVisualController _fearHoleAbsorbVisualController;

		public override void InstallBindings()
		{
			base.Container.Bind<RatsHoleEnemyAttackSettings>().FromInstance(_attackSettings).AsSingle();
			base.Container.Bind<RatsHoleEnemyMovementSettings>().FromInstance(_movementSettings).AsSingle();
			base.Container.Bind<FearHoleAbsorbAnimationSettings>().FromInstance(_fearHoleAbsorbAnimationSettings).AsSingle();
			base.Container.BindInterfacesAndSelfTo<RatsHoleEnemyContext>().FromInstance(_context).AsSingle();
			base.Container.Bind<RatsHoleEnemy>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<IFearHoleAbsorbVisualController>().FromInstance(_fearHoleAbsorbVisualController).AsSingle();
			base.Container.Bind<ICurrentStateProvider<RatsHoleEnemyStateId>>().FromInstance(_enemy).AsSingle();
		}
	}
}
