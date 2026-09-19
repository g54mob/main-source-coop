using Features.PlayersStatisticsModule.Scripts.Views.Statistics;
using Features.ViewSystemModule.Scripts.Windows;
using Features.VignetteUIEffectModule.Scripts;
using Zenject;

namespace Features.ViewSystemModule.Scripts.Installers
{
	public class SessionUIInstaller : Installer<SessionUIInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<SessionWindow>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<VignetteEffectWindow>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<GameOverWindow>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<QuotaCompletedWindow>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<DeathWindow>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<SpectatorWindow>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<DebugWindow>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<DebugEnableWindow>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<WorldCanvasWindow>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<TutorialWindow>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<StatisticWindow>().AsSingle();
		}
	}
}
