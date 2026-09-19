using Zenject;

namespace Features.EnemiesAppearSoundModule.Scripts.Installers
{
	public class EnemiesAppearSoundModuleInstaller : Installer<EnemiesAppearSoundModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<EnemiesAppearSoundTransformsModel>().AsSingle();
			base.Container.BindInterfacesTo<EnemiesAppearSoundSystem>().AsSingle();
		}
	}
}
