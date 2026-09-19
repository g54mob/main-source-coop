using Zenject;

namespace Features.LineArmModule.Scripts
{
	public class UnjoinItemsOnPlayerFallSystemInstaller : Installer<UnjoinItemsOnPlayerFallSystemInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<UnjoinItemsOnPlayerFallSystem>().AsSingle();
		}
	}
}
