using Zenject;

namespace Features.BeachInteractableCommonModule.Scripts.Installers
{
	public class BeachInteractableCommonInstaller : Installer<BeachInteractableCommonInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<LocalBeachInteractableSpawnService>().AsSingle();
			base.Container.Bind<NetworkBeachInteractableSpawnService>().AsSingle();
			base.Container.BindInterfacesTo<BeachInteractableSpawnServiceFacade>().AsSingle();
			base.Container.BindInterfacesTo<BeachInteractableSpawnAcquireService>().AsSingle();
		}
	}
}
