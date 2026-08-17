using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace EvilCore.UI.Scripts
{
	public class UIFillBarAnimator : MonoBehaviour
	{
		[Header("Target")]
		[SerializeField]
		private Image image;

		[Header("Settings")]
		[SerializeField]
		private float duration = 0.25f;

		[SerializeField]
		private Ease ease = Ease.OutCubic;

		private Tween _tween;

		private void Awake()
		{
			if (image == null)
			{
				image = GetComponent<Image>();
			}
		}

		public void SetFill(float target01)
		{
			if (!(image == null))
			{
				target01 = Mathf.Clamp01(target01);
				_tween.Stop();
				_tween = Tween.Custom(this, image.fillAmount, target01, duration, delegate(UIFillBarAnimator anim, float value)
				{
					anim.image.fillAmount = value;
				}, ease, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
		}

		public void SetFillInstant(float target01)
		{
			if (!(image == null))
			{
				_tween.Stop();
				image.fillAmount = Mathf.Clamp01(target01);
			}
		}

		private void OnDisable()
		{
			_tween.Stop();
		}
	}
}
