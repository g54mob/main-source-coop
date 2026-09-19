using RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts;
using Zenject;

namespace Features.InputDeviceModuleRealization.Scripts
{
	public class InputRealizationInstaller : Installer<InputRealizationInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<InputModel>().AsSingle();
			base.Container.BindInterfacesTo<DeviceInputService>().AsSingle();
			base.Container.BindInterfacesTo<InputDeviceActions>().AsSingle();
			base.Container.BindInterfacesTo<InputDeviceService>().AsSingle();
			base.Container.BindInterfacesTo<InputInitializeSystem>().AsSingle();
		}
	}
}
