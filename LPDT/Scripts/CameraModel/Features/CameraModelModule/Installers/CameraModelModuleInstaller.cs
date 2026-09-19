using Zenject;

namespace Features.CameraModelModule.Installers
{
	public class CameraModelModuleInstaller : Installer<CameraModelModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<CameraTransitionControlSystem>().AsSingle();
			base.Container.BindInterfacesTo<CameraPerlinTransitionSystem>().AsSingle();
			base.Container.Bind<CameraTransitionSkinUpdateRequest>().AsSingle();
			base.Container.BindInterfacesTo<CameraGrabFocusService>().AsSingle();
			base.Container.BindInterfacesTo<CameraSpectatorFollowService>().AsSingle();
			base.Container.BindInterfacesTo<CameraLateResolveService>().AsSingle();
		}
	}
}
