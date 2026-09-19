using Zenject;

namespace Features.PlayerStatesModule.Scripts.Installers
{
	public class PlayerStateModuleInstaller : Installer<PlayerStateModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<SpectatorModel>().AsSingle();
			base.Container.BindInterfacesTo<PlayerStunHandleSystem>().AsSingle();
		}
	}
}
