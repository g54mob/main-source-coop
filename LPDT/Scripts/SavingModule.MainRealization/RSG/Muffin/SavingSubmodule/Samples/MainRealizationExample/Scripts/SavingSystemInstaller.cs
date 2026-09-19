using RSG.Muffin.Plugins.Zenject.Addons.AddressablesConfigurationsLoader;
using RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts;
using RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts.Implementation;
using RSG.Muffin.ZenjectMockableRealizationSubmodule.ZenjectMockableRealizationModule;
using Zenject;

namespace RSG.Muffin.SavingSubmodule.Samples.MainRealizationExample.Scripts
{
	public class SavingSystemInstaller : Installer<SavingSystemInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<SavingInitializer>().AsSingle();
			base.Container.BindInterfacesTo<SavingManager>().AsSingle();
			base.Container.BindInterfacesTo<SaveDataContainer>().AsSingle();
			base.Container.BindMockableRealizationAsSingle<ISaveFilesManager>();
			base.Container.BindInterfacesTo<SaveDataSaver>().AsSingle();
			base.Container.BindInterfacesTo<SaveDataDeleter>().AsSingle();
			base.Container.BindInterfacesTo<SaveDataLoader>().AsSingle();
			base.Container.BindMockableRealizationAsSingle<ISavesCloudManager>();
			base.Container.BindMockableRealizationAsSingle<ISaveDataContainerInitializer>();
			base.Container.BindMockableRealizationAsSingle<ISavesCloudInitializer>();
			base.Container.BindConfigurationFromAddressables<SavingSystemConfiguration>("SavingSystemConfiguration_Default").AsSingle();
		}
	}
}
