using Features.TutorialModule.Scripts.UIGlow.GlowingObjectsHolder;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.TutorialModule.Scripts.UIGlow
{
	[PublicAPI]
	public class GlowableUIPresenter : PresenterBehaviour<GlowableUIViewBase>
	{
		private readonly Features.TutorialModule.Scripts.UIGlow.GlowingObjectsHolder.GlowingObjectsHolder _glowingObjectsHolder;

		public GlowableUIPresenter(Features.TutorialModule.Scripts.UIGlow.GlowingObjectsHolder.GlowingObjectsHolder glowingObjectsHolder)
		{
			_glowingObjectsHolder = glowingObjectsHolder;
		}

		protected override void OnViewSet()
		{
			_glowingObjectsHolder.Register(base.View.GlowableObject);
		}

		protected override void OnDisposed()
		{
			_glowingObjectsHolder.Unregister(base.View.GlowableObject);
		}
	}
}
