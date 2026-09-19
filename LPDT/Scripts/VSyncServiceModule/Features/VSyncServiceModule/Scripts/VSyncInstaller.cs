using Zenject;

namespace Features.VSyncServiceModule.Scripts
{
	public class VSyncInstaller : Installer<VSyncInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<VsyncService>().AsSingle();
		}
	}
}
