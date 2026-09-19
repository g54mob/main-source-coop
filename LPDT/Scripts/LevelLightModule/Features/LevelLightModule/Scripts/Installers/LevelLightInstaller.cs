using Zenject;

namespace Features.LevelLightModule.Scripts.Installers
{
	public class LevelLightInstaller : Installer<LevelLightInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<LightObjectsGroupService>().AsSingle();
			base.Container.BindInterfacesTo<FlashlightSpawnService>().AsSingle();
		}
	}
}
