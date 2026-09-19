using Zenject;

namespace Features.ItemCollisionModule.Scripts.Installers
{
	public class ItemCollisionInstaller : Installer<ItemCollisionInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<ItemCollisionService>().AsSingle();
		}
	}
}
