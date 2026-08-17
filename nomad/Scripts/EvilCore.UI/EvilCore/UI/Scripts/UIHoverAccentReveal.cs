using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace EvilCore.UI.Scripts
{
	public class UIHoverAccentReveal : UIPointerEffect
	{
		[Header("Accent")]
		[SerializeField]
		private AccentRevealMode mode;

		[SerializeField]
		private RectTransform accent;

		[SerializeField]
		private Graphic accentGraphic;

		[SerializeField]
		private float fullWidth = 100f;

		private float _accentHeight;

		protected override void CacheState()
		{
			if (accent != null)
			{
				_accentHeight = accent.sizeDelta.y;
			}
		}

		protected override void OnHoverBegin()
		{
			if (mode == AccentRevealMode.Width)
			{
				if (accent != null)
				{
					Tween.UISizeDelta(accent, new Vector2(fullWidth, _accentHeight), duration, enterEase, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
				}
			}
			else if (accentGraphic != null)
			{
				Tween.Alpha(accentGraphic, 1f, duration, enterEase, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
		}

		protected override void OnHoverEnd()
		{
			if (mode == AccentRevealMode.Width)
			{
				if (accent != null)
				{
					Tween.UISizeDelta(accent, new Vector2(0f, _accentHeight), duration, exitEase, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
				}
			}
			else if (accentGraphic != null)
			{
				Tween.Alpha(accentGraphic, 0f, duration, exitEase, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
		}

		protected override void ResetImmediate()
		{
			if (mode == AccentRevealMode.Width)
			{
				if (!(accent == null))
				{
					Tween.CompleteAll(accent);
					accent.sizeDelta = new Vector2(0f, _accentHeight);
				}
			}
			else if (!(accentGraphic == null))
			{
				Tween.CompleteAll(accentGraphic);
				Color color = accentGraphic.color;
				color.a = 0f;
				accentGraphic.color = color;
			}
		}
	}
}
