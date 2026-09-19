using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public class LoadingScreenAnimatedWheelView : LoadingScreenContentViewBase
	{
		[SerializeField]
		private RectTransform _wheelTransform;

		public override void OnHidden()
		{
			if (_wheelTransform != null)
			{
				_wheelTransform.localRotation = Quaternion.identity;
			}
		}
	}
}
