using Features.TutorialModule.Scripts.UIGlow.GlowingObjectsHolder;
using Features.TutorialModule.Scripts.UIGlow.GlowingService;
using Zenject;

namespace Features.TutorialModule.Scripts.UIGlow
{
	public class GlowingServiceInstaller : Installer<GlowingServiceInstaller>
	{
		public override void InstallBindings()
		{
			BindGlowingObjectsHolder();
			BindGlowingService();
		}

		private void BindGlowingObjectsHolder()
		{
			base.Container.Bind<Features.TutorialModule.Scripts.UIGlow.GlowingObjectsHolder.GlowingObjectsHolder>().AsSingle();
		}

		private void BindGlowingService()
		{
			base.Container.Bind<IGlowingService>().To<Features.TutorialModule.Scripts.UIGlow.GlowingService.GlowingService>().AsSingle();
		}
	}
}
