using System;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.ComponentControls
{
	[Serializable]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	public abstract class TouchControl : CustomControllerControl
	{
		private Canvas _canvas;

		private RectTransform __rectTransform;

		internal TouchController gGeqnLRNKcjgzKaWVxWuqSNJvSze => kVndxYkOyKrGmxUDdlmyfMvjGUQDA() as TouchController;

		internal Canvas vQpmvzIqZzQRgZjNdTCOIzNihLiH => _canvas;

		internal RectTransform whnJFhhvlVIfvAzDjDUVAxyZymsH
		{
			get
			{
				Canvas canvas = vQpmvzIqZzQRgZjNdTCOIzNihLiH;
				if (canvas == null)
				{
					return null;
				}
				return canvas.transform as RectTransform;
			}
		}

		internal RectTransform dHBtGVwmKSUQYlNEBqhwMLJxhsUgA => __rectTransform ?? (__rectTransform = GetComponent<RectTransform>());

		bool CustomControllerControl.osKcqUcyYlVGlGygpMaOnUYNqJDBA => kVndxYkOyKrGmxUDdlmyfMvjGUQDA() as TouchController != null;

		[CustomObfuscation(rename = false)]
		internal TouchControl()
		{
		}

		[CustomObfuscation(rename = false)]
		internal override void OnValidate()
		{
			base.OnValidate();
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				nGHzehsIbiveZqWLVMehuidueiKkA(true, false);
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				nGHzehsIbiveZqWLVMehuidueiKkA(false, true);
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				nGHzehsIbiveZqWLVMehuidueiKkA(false, true);
			}
		}

		internal override bool lpOPYPkfRAdylCMSLphTlIgUWIWy()
		{
			if (!base.lpOPYPkfRAdylCMSLphTlIgUWIWy())
			{
				return false;
			}
			if (!nGHzehsIbiveZqWLVMehuidueiKkA(true, true))
			{
				return false;
			}
			return true;
		}

		internal override void FyKKzlnIjsJaSimYwuqKSwNJHIHx()
		{
			base.FyKKzlnIjsJaSimYwuqKSwNJHIHx();
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				nGHzehsIbiveZqWLVMehuidueiKkA(true, true);
			}
		}

		[CustomObfuscation(rename = false)]
		internal override IComponentController FindController()
		{
			return UnityTools.GetComponentInSelfOrParents<CustomController>(base.transform);
		}

		[CustomObfuscation(rename = false)]
		internal override Type GetRequiredControllerType()
		{
			return typeof(TouchController);
		}

		private bool nGHzehsIbiveZqWLVMehuidueiKkA(bool P_0, bool P_1)
		{
			_canvas = UnityTools.GetComponentInSelfOrParents<Canvas>(base.gameObject);
			if (_canvas == null)
			{
				if (P_0)
				{
					Logger.LogError("No Canvas was found. Touch controls must be a child of a Canvas.");
				}
				return false;
			}
			if (_canvas.renderMode == RenderMode.WorldSpace)
			{
				if (P_1)
				{
					Logger.LogError("Touch controls cannot be used with a world space Canvas. Change the canvas render mode to screen space.");
				}
				return false;
			}
			return true;
		}
	}
}
