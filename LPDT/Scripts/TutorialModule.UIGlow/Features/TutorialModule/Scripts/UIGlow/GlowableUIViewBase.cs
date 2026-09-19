using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.TutorialModule.Scripts.UIGlow
{
	public abstract class GlowableUIViewBase : ViewBehaviour
	{
		public abstract IGlowableObject GlowableObject { get; }
	}
}
