using Zenject;

namespace Features.TutorialModule.Scripts.UITipsModule
{
	public class UITipsModuleInstaller : Installer<UITipsModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<UITipService>().AsSingle();
		}
	}
}
