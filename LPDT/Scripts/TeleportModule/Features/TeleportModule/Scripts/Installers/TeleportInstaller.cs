using Zenject;

namespace Features.TeleportModule.Scripts.Installers
{
	public class TeleportInstaller : Installer<TeleportInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<TeleportService>().AsSingle();
		}
	}
}
