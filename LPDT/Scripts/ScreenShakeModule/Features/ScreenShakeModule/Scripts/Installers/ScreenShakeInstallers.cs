using Zenject;

namespace Features.ScreenShakeModule.Scripts.Installers
{
	public class ScreenShakeInstallers : Installer<ScreenShakeInstallers>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<ScreenShakeService>().AsSingle();
		}
	}
}
