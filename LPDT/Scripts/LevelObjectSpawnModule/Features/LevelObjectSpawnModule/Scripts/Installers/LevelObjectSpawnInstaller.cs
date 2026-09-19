using Zenject;

namespace Features.LevelObjectSpawnModule.Scripts.Installers
{
	public class LevelObjectSpawnInstaller : Installer<LevelObjectSpawnInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<LevelObjectsSpawnService>().AsSingle();
			base.Container.BindInterfacesTo<LevelObjectsSpawnSystem>().AsSingle();
		}
	}
}
