using Zenject;

namespace Features.EnvironmentSpawnModule.Scripts.Installers
{
	public class EnvironmentSpawnInstaller : Installer<EnvironmentSpawnInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<EnvironmentSpawnPointsModel>().AsSingle();
			base.Container.BindInterfacesTo<EnvironmentSpawnSystem>().AsSingle();
		}
	}
}
