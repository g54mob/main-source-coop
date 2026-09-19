using Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy
{
	public class HeadmanEnemyInstaller : MonoInstaller
	{
		[Header("Settings")]
		[SerializeField]
		private HeadmanWanderingSettings _wanderingSettings;

		[SerializeField]
		private HeadmanChasingSettings _chasingSettings;

		[SerializeField]
		private HeadmanSlowedSettings _slowedSettings;

		[SerializeField]
		private HeadmanRageSettings _rageSettings;

		[SerializeField]
		private HeadmanFearSettings _fearSettings;

		[SerializeField]
		private HeadmanNavmeshPositionSettings _navmeshPositionSettings;

		[Header("Scene refs")]
		[SerializeField]
		private HeadmanEnemyContext _context;

		[SerializeField]
		private HeadmanEnemy _enemy;

		public override void InstallBindings()
		{
			base.Container.Bind<HeadmanWanderingSettings>().FromInstance(_wanderingSettings).AsSingle();
			base.Container.Bind<HeadmanChasingSettings>().FromInstance(_chasingSettings).AsSingle();
			base.Container.Bind<HeadmanSlowedSettings>().FromInstance(_slowedSettings).AsSingle();
			base.Container.Bind<HeadmanRageSettings>().FromInstance(_rageSettings).AsSingle();
			base.Container.Bind<HeadmanFearSettings>().FromInstance(_fearSettings).AsSingle();
			base.Container.Bind<HeadmanNavmeshPositionSettings>().FromInstance(_navmeshPositionSettings).AsSingle();
			base.Container.Bind<HeadmanEnemyContext>().FromInstance(_context).AsSingle();
			base.Container.Bind<HeadmanEnemy>().FromInstance(_enemy).AsSingle();
		}
	}
}
