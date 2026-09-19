using Zenject;

namespace Features.RumModule.Scripts
{
	public class RumInstaller : Installer<RumInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<TemporalStatsRewardSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<RumEffectsCleanupSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<StatDrivenHazeVolumeSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<DrunkennessDecaySystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<DrunkLevelEffectsSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<DrunkVoiceEffectsSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<DrunkFaintSnoreSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<DrunkSoberSplashService>().AsSingle();
		}
	}
}
