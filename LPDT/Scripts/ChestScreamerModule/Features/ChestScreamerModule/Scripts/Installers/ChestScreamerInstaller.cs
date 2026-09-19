using Features.ChestScreamerModule.Scripts.Presets;
using Features.ChestScreamerModule.Scripts.Systems;
using Zenject;

namespace Features.ChestScreamerModule.Scripts.Installers
{
	public class ChestScreamerInstaller : Installer<ChestScreamerInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<ChestScreamerGameplaySystem>().AsSingle();
			base.Container.BindInterfacesTo<ChestScreamerPresetResolver>().AsSingle();
		}
	}
}
