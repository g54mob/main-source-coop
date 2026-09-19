using Features.CartUpgradesModule.Scripts.Core;
using Features.CartUpgradesModule.Scripts.Systems;
using Zenject;

namespace Features.CartUpgradesModule.Scripts.Installers
{
	public class CartUpgradesInstaller : Installer<CartUpgradesInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<CartUpgradeService>().AsSingle();
			base.Container.BindInterfacesTo<CartTierRegistry>().AsSingle();
			base.Container.BindInterfacesTo<CartUpgradeSwapSystem>().AsSingle();
			base.Container.BindInterfacesTo<CartUpgradeLevelResetSystem>().AsSingle();
		}
	}
}
