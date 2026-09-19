using Cysharp.Threading.Tasks;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public abstract class LoadingScreenViewBase : ViewBehaviour
	{
		public abstract ScreensViewBase ScreensView { get; }

		public abstract void SetVisible(bool visible);

		public abstract void SetAlpha(float alpha);

		public abstract void SetInteractable(bool interactable);

		public abstract UniTask FadeAsync(float targetAlpha, float duration, bool useUnscaledTime);

		public abstract void SetBackgroundActive(bool active);

		public abstract void SetScreensRootVisible(bool visible);
	}
}
