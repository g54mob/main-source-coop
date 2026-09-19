using Zenject;

namespace Features.TutorialModule.Scripts.SpecialTagsProcessor
{
	public class SpecialTagsProcessorInstaller : Installer<SpecialTagsProcessorInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<SpecialTagsInTextProcessor>().AsSingle();
		}
	}
}
