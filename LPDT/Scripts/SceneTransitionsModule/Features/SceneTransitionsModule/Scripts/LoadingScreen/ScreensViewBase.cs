using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public abstract class ScreensViewBase : ViewBehaviour
	{
		public abstract RectTransform ScreenContainer { get; }

		public abstract void SetRootActive(bool active);

		public abstract void SetActiveScreen(GameObject screenObject);

		public abstract void DestroyActiveScreen();
	}
}
