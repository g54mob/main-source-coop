using Zenject;

namespace Features.CoroutineUtils.Scripts.Installers
{
	public class CoroutineRunnerInstaller : Installer<CoroutineRunnerInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<ICoroutineRunner>().To<CoroutineRunner>().FromComponentsInHierarchy()
				.AsSingle();
		}
	}
}
