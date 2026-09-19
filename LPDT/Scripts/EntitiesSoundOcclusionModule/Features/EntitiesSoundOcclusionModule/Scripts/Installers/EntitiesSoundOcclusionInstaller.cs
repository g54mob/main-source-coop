using Zenject;

namespace Features.EntitiesSoundOcclusionModule.Scripts.Installers
{
	public class EntitiesSoundOcclusionInstaller : Installer<EntitiesSoundOcclusionInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<EntitiesSoundOcclusionService>().AsSingle();
			base.Container.Bind<EntitiesSoundOcclusionModel>().AsSingle();
			base.Container.BindInterfacesTo<SoundOcclusionInstanceSignalSystem>().AsSingle();
			base.Container.BindInterfacesTo<AudioOcclusionRoutingSystem>().AsSingle();
		}
	}
}
