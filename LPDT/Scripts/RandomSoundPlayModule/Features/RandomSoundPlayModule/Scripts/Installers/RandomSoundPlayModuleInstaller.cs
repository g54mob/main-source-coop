using Zenject;

namespace Features.RandomSoundPlayModule.Scripts.Installers
{
	public class RandomSoundPlayModuleInstaller : Installer<RandomSoundPlayModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<EnemiesNearPlayersUpdateSystem>().AsSingle();
		}
	}
}
