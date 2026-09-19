using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public abstract class LoadingScreenTipsViewBase : ViewBehaviour
	{
		public abstract float AnimatedTipDotsInterval { get; }

		public abstract int AnimatedTipMaxDots { get; }

		public abstract string DefaultText { get; }

		public abstract void SetVisible(bool visible);

		public abstract void SetText(string tip);

		public abstract void Reset();
	}
}
