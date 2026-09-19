using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public class ScreensView : ScreensViewBase
	{
		[SerializeField]
		private RectTransform _screenContainer;

		private GameObject _trackedScreenObject;

		public override RectTransform ScreenContainer => _screenContainer;

		public override void SetRootActive(bool active)
		{
			base.gameObject.SetActive(active);
		}

		public override void SetActiveScreen(GameObject screenObject)
		{
			_trackedScreenObject = screenObject;
		}

		public override void DestroyActiveScreen()
		{
			if (!(_trackedScreenObject == null))
			{
				Object.Destroy(_trackedScreenObject);
				_trackedScreenObject = null;
			}
		}

		private void OnDestroy()
		{
			DestroyActiveScreen();
		}
	}
}
