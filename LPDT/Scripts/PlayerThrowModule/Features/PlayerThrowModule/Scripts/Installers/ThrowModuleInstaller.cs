using Features.ThrowModule.Scripts;
using Zenject;

namespace Features.PlayerThrowModule.Scripts.Installers
{
	public class ThrowModuleInstaller : Installer<ThrowModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<LocalPlayerThrowService>().AsSingle();
		}
	}
}
