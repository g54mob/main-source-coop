using Features.UIAnimationsModule.Scripts;
using Zenject;

namespace Features.UIAnimationsModule.Installers
{
	public class UIAnimationsInstaller : Installer<UIAnimationsInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<IUIAnimationPresetResolver>().To<UIAnimationPresetResolver>().AsSingle();
			base.Container.Bind<IUIAnimationService>().To<UIAnimationService>().AsSingle();
		}
	}
}
