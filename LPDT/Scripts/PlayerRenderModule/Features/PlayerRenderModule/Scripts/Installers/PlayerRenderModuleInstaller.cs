using Zenject;

namespace Features.PlayerRenderModule.Scripts.Installers
{
	public class PlayerRenderModuleInstaller : Installer<PlayerRenderModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<PlayerRenderService>().AsSingle();
		}
	}
}
