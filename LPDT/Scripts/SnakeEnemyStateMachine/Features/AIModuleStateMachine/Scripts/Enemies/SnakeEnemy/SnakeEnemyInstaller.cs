using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.States;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy
{
	public class SnakeEnemyInstaller : MonoInstaller
	{
		[SerializeField]
		private SnakeEnemyContext _context;

		[SerializeField]
		private SnakeEnemy _enemy;

		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<SnakeEnemyContext>().FromInstance(_context).AsSingle();
			base.Container.Bind<SnakeEnemy>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<ICurrentStateProvider<SnakeStateId>>().FromInstance(_enemy).AsSingle();
		}
	}
}
