using Zenject;

namespace Features.ItemsModule.Scripts.Installers
{
	public class ItemsModuleInstaller : Installer<ItemsModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<ItemSpawnService>().AsSingle();
		}
	}
}
