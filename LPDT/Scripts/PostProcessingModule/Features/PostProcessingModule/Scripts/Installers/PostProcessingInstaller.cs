using Zenject;

namespace Features.PostProcessingModule.Scripts.Installers
{
	public class PostProcessingInstaller : Installer<PostProcessingInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<StorePostProcessingSystem>().AsSingle();
			base.Container.BindInterfacesTo<VignetteAdjustingSystem>().AsSingle();
			base.Container.BindInterfacesTo<StaminaVignetteAdjustingSystem>().AsSingle();
			base.Container.BindInterfacesTo<FlickerAdjustingSystem>().AsSingle();
			base.Container.BindInterfacesTo<CameraWaterLensAdjustingSystem>().AsSingle();
		}
	}
}
