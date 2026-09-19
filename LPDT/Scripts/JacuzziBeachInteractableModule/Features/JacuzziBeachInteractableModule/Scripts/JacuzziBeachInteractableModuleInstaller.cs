using Features.JacuzziBeachInteractableModule.Scripts.Rendering;
using Zenject;

namespace Features.JacuzziBeachInteractableModule.Scripts
{
	public class JacuzziBeachInteractableModuleInstaller : Installer<JacuzziBeachInteractableModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<JacuzziHazeBlendAggregator>().AsSingle();
		}
	}
}
