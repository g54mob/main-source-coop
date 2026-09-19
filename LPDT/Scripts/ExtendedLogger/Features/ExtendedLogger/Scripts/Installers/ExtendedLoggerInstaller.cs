using Zenject;

namespace Features.ExtendedLogger.Scripts.Installers
{
	public class ExtendedLoggerInstaller : Installer<ExtendedLoggerInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<OverlayService>().AsSingle();
			base.Container.BindInterfacesTo<OverlayInputSystem>().AsSingle();
			base.Container.Bind<OverlayModel>().AsSingle();
		}
	}
}
