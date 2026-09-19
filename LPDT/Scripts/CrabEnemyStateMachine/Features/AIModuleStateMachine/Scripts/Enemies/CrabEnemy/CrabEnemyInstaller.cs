using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.States;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy
{
	public class CrabEnemyInstaller : MonoInstaller
	{
		[SerializeField]
		private CrabEnemyContext _context;

		[SerializeField]
		private CrabEnemy _enemy;

		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<CrabEnemyContext>().FromInstance(_context).AsSingle();
			base.Container.Bind<CrabEnemy>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<ICurrentStateProvider<CrabStateId>>().FromInstance(_enemy).AsSingle();
		}
	}
}
