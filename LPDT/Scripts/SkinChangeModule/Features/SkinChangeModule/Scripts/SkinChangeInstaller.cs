using Zenject;

namespace Features.SkinChangeModule.Scripts
{
	public class SkinChangeInstaller : Installer<SkinChangeInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<SkinChangeService>().AsSingle();
		}
	}
}
