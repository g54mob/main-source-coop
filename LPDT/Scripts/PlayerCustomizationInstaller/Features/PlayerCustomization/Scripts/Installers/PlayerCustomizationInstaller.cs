using Zenject;

namespace Features.PlayerCustomization.Scripts.Installers
{
	public class PlayerCustomizationInstaller : Installer<PlayerCustomizationInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<PlayerCustomizationService>().AsSingle();
		}
	}
}
