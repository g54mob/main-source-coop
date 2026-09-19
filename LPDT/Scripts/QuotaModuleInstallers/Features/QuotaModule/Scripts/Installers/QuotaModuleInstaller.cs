using Features.QuotaModule.Scripts.PhysicsContainer;
using Features.QuotaModule.Scripts.Systems;
using Zenject;

namespace Features.QuotaModule.Scripts.Installers
{
	public class QuotaModuleInstaller : Installer<QuotaModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<QuotaContainerLocationModel>().AsSingle();
			base.Container.BindInterfacesTo<QuotaInitializeSystem>().AsSingle();
			base.Container.BindInterfacesTo<QuotaProgressSystem>().AsSingle();
			base.Container.BindInterfacesTo<FreeContainerItemsTrackSystem>().AsSingle();
			base.Container.BindInterfacesTo<QuotaAnalyticsSystem>().AsSingle();
		}
	}
}
