using Features.AIModuleStateMachine.Scripts.Core;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.MimicEnemy.Settings;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy
{
	public class BartenderEnemyInstaller : MonoInstaller
	{
		[SerializeField]
		private BartenderEnemyContext _context;

		[SerializeField]
		private BartenderEnemy _enemy;

		[SerializeField]
		private EnemyStatsConfiguration _enemyStatsConfiguration;

		[SerializeField]
		private MimicFacialAnimationSettings _facialAnimationSettings;

		[SerializeField]
		private BartenderReactSettings _reactSettings;

		[SerializeField]
		private BartenderAppearanceSettings _appearanceSettings;

		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<BartenderEnemyContext>().FromInstance(_context).AsSingle();
			base.Container.Bind<BartenderEnemy>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<ICurrentStateProvider<BartenderStateId>>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<EnemyStatsConfiguration>().FromInstance(_enemyStatsConfiguration).AsSingle();
			if (_facialAnimationSettings != null)
			{
				base.Container.Bind<MimicFacialAnimationSettings>().FromInstance(_facialAnimationSettings).AsSingle();
			}
			if (_reactSettings != null)
			{
				base.Container.Bind<BartenderReactSettings>().FromInstance(_reactSettings).AsSingle();
			}
			BartenderAppearanceSettings bartenderAppearanceSettings = _appearanceSettings;
			if (bartenderAppearanceSettings == null)
			{
				bartenderAppearanceSettings = ScriptableObject.CreateInstance<BartenderAppearanceSettings>();
				Debug.LogWarning("[Bartender] BartenderAppearanceSettings missing on installer — using runtime default (Skin10).");
			}
			base.Container.Bind<BartenderAppearanceSettings>().FromInstance(bartenderAppearanceSettings).AsSingle();
		}
	}
}
