using Features.AIModuleStateMachine.Scripts.Core;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.MimicEnemy.Settings;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy
{
	public class MimicEnemyInstaller : MonoInstaller
	{
		[SerializeField]
		private MimicEnemyContext _context;

		[SerializeField]
		private MimicEnemy _enemy;

		[SerializeField]
		private EnemyStatsConfiguration _enemyStatsConfiguration;

		[SerializeField]
		private MimicFacialAnimationSettings _facialAnimationSettings;

		[SerializeField]
		private MimicCauldronSettings _cauldronSettings;

		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<MimicEnemyContext>().FromInstance(_context).AsSingle();
			base.Container.Bind<MimicEnemy>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<ICurrentStateProvider<MimicStateId>>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<EnemyStatsConfiguration>().FromInstance(_enemyStatsConfiguration).AsSingle();
			base.Container.Bind<MimicFacialAnimationSettings>().FromInstance(_facialAnimationSettings).AsSingle();
			base.Container.Bind<MimicCauldronSettings>().FromInstance((_cauldronSettings != null) ? _cauldronSettings : ScriptableObject.CreateInstance<MimicCauldronSettings>()).AsSingle();
		}
	}
}
