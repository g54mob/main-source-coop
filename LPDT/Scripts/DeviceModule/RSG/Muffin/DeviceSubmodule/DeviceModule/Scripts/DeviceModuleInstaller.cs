using Features.DeviceModule.Scripts;
using RSG.Muffin.ZenjectMockableRealizationSubmodule.ZenjectMockableRealizationModule;
using Zenject;

namespace RSG.Muffin.DeviceSubmodule.DeviceModule.Scripts
{
	public class DeviceModuleInstaller : Installer<DeviceModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<DeviceTypeModel>().AsSingle();
			base.Container.BindInterfacesTo<DeviceService>().AsSingle();
			BindDefaultDeviceTypeAccessor();
			BindOverridenDeviceTypeAccessor();
			BindDeviceTypeInitializeService();
			base.Container.BindInterfacesTo<DeviceSystem>().AsSingle();
		}

		private void BindDeviceTypeInitializeService()
		{
			base.Container.Bind<IDeviceTypeInitializeService>().To<DeviceTypeInitializeService>().AsSingle();
		}

		private void BindOverridenDeviceTypeAccessor()
		{
			base.Container.BindMockableRealizationAsSingle<IRealDeviceTypeAccessor>();
		}

		private void BindDefaultDeviceTypeAccessor()
		{
			base.Container.Bind<IUnityDeviceTypeAccessor>().To<UnityDeviceTypeAccessor>().AsSingle();
		}
	}
}
