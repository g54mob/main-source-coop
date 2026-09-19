using Features.PlatformStatusRealizationModule.Scripts.Systems;
using Zenject;

namespace Features.PlatformStatusRealizationModule.Scripts.Installers
{
	public class PlatformStatusRealizationInstaller : Installer<PlatformStatusRealizationInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<IGameStatusService>().To<GameStatusService>().AsSingle();
			base.Container.BindInterfacesTo<MainMenuStatusSyncSystem>().AsSingle();
		}
	}
}
