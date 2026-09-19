using Zenject;

namespace Features.BoosterModule.BoosterModule.Scripts.Installers
{
	public class BoosterModuleInstaller : Installer<BoosterModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<BoosterEntitiesFactory>().AsSingle();
			base.Container.Bind<BoosterModel>().AsSingle();
			base.Container.BindInterfacesTo<BoostersService>().AsSingle();
			base.Container.BindInterfacesTo<BoosterSystem>().AsSingle();
			base.Container.BindInterfacesTo<BoosterTimeSystem>().AsSingle();
		}
	}
}
