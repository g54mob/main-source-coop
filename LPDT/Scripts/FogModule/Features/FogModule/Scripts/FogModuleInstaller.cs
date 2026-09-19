using Zenject;

namespace Features.FogModule.Scripts
{
	public class FogModuleInstaller : Installer<FogModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<FogRegionResolverService>().AsSingle();
			base.Container.BindInterfacesTo<FogRegionBlendSystem>().AsSingle();
		}
	}
}
