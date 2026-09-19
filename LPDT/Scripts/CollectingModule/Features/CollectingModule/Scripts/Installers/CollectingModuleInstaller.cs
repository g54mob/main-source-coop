using Zenject;

namespace Features.CollectingModule.Scripts.Installers
{
	public class CollectingModuleInstaller : Installer<CollectingModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<IItemCollectService>().To<ItemCollectService>().AsSingle();
			base.Container.Bind<CollectItemFellEvent>().AsSingle();
		}
	}
}
