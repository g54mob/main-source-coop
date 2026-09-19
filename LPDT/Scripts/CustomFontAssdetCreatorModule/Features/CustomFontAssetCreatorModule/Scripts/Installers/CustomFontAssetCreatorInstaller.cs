using Zenject;

namespace Features.CustomFontAssetCreatorModule.Scripts.Installers
{
	public class CustomFontAssetCreatorInstaller : Installer<CustomFontAssetCreatorInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<CustomFontAssetModel>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<CustomFontAssetCreatorService>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<CustomFontAssetCreatorSystem>().AsSingle();
		}
	}
}
