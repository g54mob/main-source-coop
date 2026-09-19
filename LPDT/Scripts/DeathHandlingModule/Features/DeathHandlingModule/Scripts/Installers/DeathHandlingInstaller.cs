using Zenject;

namespace Features.DeathHandlingModule.Scripts.Installers
{
	public class DeathHandlingInstaller : Installer<DeathHandlingInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<DeathHandlingSystem>().AsSingle();
		}
	}
}
