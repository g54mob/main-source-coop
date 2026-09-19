using RSG.Muffin.AssetLoaderModule.Core;
using Zenject;

namespace Features.AssetLoaderRealization.Scripts.Installers
{
	public class AssetLoaderInstaller : Installer<AssetLoaderInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<ResourceAssetLoaderService>().AsSingle();
			base.Container.BindInterfacesTo<AddressableAssetLoaderService>().AsSingle();
			base.Container.BindInterfacesTo<AssetLoaderFacadeService>().AsSingle();
		}
	}
}
