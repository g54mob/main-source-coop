using Features.ItemDamageModule.Scripts;
using Zenject;

namespace Features.ItemDamageModule.Installers
{
	public class ItemDamageInstaller : Installer<ItemDamageInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<ItemCostReduceSystem>().AsSingle();
			base.Container.BindInterfacesTo<ItemCollisionSoundSystem>().AsSingle();
			base.Container.BindInterfacesTo<ItemCostReduceService>().AsSingle();
			base.Container.BindInterfacesTo<ItemCostReduceSpawnCoinsService>().AsSingle();
		}
	}
}
