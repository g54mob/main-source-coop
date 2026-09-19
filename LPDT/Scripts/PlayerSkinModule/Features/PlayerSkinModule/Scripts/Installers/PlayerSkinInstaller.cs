using Zenject;

namespace Features.PlayerSkinModule.Scripts.Installers
{
	public class PlayerSkinInstaller : Installer<PlayerSkinInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<SkinModel>().AsSingle();
			base.Container.Bind<SkinChangedEvent>().AsSingle();
			base.Container.BindInterfacesTo<PlayerStateSkinSwitchService>().AsSingle();
			base.Container.BindInterfacesTo<LocalPlayerSkinSystem>().AsSingle();
		}
	}
}
