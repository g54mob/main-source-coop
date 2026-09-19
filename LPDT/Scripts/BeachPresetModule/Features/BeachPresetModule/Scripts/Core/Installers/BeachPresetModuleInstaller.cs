using Features.BeachPresetModule.Scripts.Core.Interfaces;
using Features.BeachPresetModule.Scripts.Services;
using Zenject;

namespace Features.BeachPresetModule.Scripts.Core.Installers
{
	public class BeachPresetModuleInstaller : Installer<BeachPresetModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<BeachPresetApplyByLevelSystem>().AsSingle();
			base.Container.Bind<IBeachLightingService>().To<BeachLightingService>().AsSingle();
			base.Container.Bind<IBeachMaterialSwapService>().To<BeachMaterialSwapService>().AsSingle();
			base.Container.Bind<IBeachPrefabSpawnService>().To<BeachPrefabSpawnService>().AsSingle();
			base.Container.Bind<IBeachGateCorridorLightService>().To<BeachGateCorridorLightService>().AsSingle();
			base.Container.Bind<IBeachItemSpawnService>().To<BeachItemSpawnService>().AsSingle();
		}
	}
}
