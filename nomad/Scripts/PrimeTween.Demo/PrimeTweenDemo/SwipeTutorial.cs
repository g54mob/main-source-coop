using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace PrimeTweenDemo
{
	public class SwipeTutorial : MonoBehaviour
	{
		private Tween tween;

		private void OnEnable()
		{
			tween = Tween.Alpha(GetComponent<Text>(), 1f, 0f, 1f, Ease.InOutSine, -1, CycleMode.Yoyo);
		}

		public void Hide()
		{
			if (tween.isAlive)
			{
				tween.SetRemainingCycles(stopAtEndValue: true);
			}
		}
	}
}
