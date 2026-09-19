using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.OverlayUI.Installers
{
	public class OverlayUIInstaller : Installer<OverlayUIInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<UIOverlayCanvasModel>().AsSingle();
			base.Container.Bind<IOverlayService>().To<UIOverlayService>().AsSingle();
		}
	}
}
