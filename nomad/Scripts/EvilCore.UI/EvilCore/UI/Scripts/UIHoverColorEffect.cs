using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace EvilCore.UI.Scripts
{
	public class UIHoverColorEffect : UIPointerEffect
	{
		[Header("Mode")]
		[SerializeField]
		private HoverColorMode mode;

		[Header("Swap (background <-> foreground)")]
		[SerializeField]
		private Graphic background;

		[SerializeField]
		private Graphic foreground;

		[Header("Tint")]
		[SerializeField]
		private Graphic[] tintTargets;

		[SerializeField]
		private Color hoverColor = Color.white;

		private Color _backgroundDefault;

		private Color _foregroundDefault;

		private Color[] _tintDefaults;

		protected override void CacheState()
		{
			if (background != null)
			{
				_backgroundDefault = background.color;
			}
			if (foreground != null)
			{
				_foregroundDefault = foreground.color;
			}
			if (tintTargets == null)
			{
				return;
			}
			_tintDefaults = new Color[tintTargets.Length];
			for (int i = 0; i < tintTargets.Length; i++)
			{
				if (tintTargets[i] != null)
				{
					_tintDefaults[i] = tintTargets[i].color;
				}
			}
		}

		protected override void OnHoverBegin()
		{
			if (mode == HoverColorMode.Swap)
			{
				if (background != null)
				{
					Tween.Color(background, _foregroundDefault, duration, enterEase, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
				}
				if (foreground != null)
				{
					Tween.Color(foreground, _backgroundDefault, duration, enterEase, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
				}
			}
			else
			{
				if (tintTargets == null)
				{
					return;
				}
				Graphic[] array = tintTargets;
				foreach (Graphic graphic in array)
				{
					if (graphic != null)
					{
						Tween.Color(graphic, hoverColor, duration, enterEase, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
					}
				}
			}
		}

		protected override void OnHoverEnd()
		{
			if (background != null)
			{
				Tween.Color(background, _backgroundDefault, duration, exitEase, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
			if (foreground != null)
			{
				Tween.Color(foreground, _foregroundDefault, duration, exitEase, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
			if (tintTargets == null || _tintDefaults == null)
			{
				return;
			}
			for (int i = 0; i < tintTargets.Length; i++)
			{
				if (tintTargets[i] != null)
				{
					Tween.Color(tintTargets[i], _tintDefaults[i], duration, exitEase, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
				}
			}
		}

		protected override void ResetImmediate()
		{
			if (background != null)
			{
				Tween.CompleteAll(background);
				background.color = _backgroundDefault;
			}
			if (foreground != null)
			{
				Tween.CompleteAll(foreground);
				foreground.color = _foregroundDefault;
			}
			if (tintTargets == null || _tintDefaults == null)
			{
				return;
			}
			for (int i = 0; i < tintTargets.Length; i++)
			{
				if (!(tintTargets[i] == null))
				{
					Tween.CompleteAll(tintTargets[i]);
					tintTargets[i].color = _tintDefaults[i];
				}
			}
		}
	}
}
