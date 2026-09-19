using System.Collections.Generic;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.OverlayUI
{
	public class UIOverlayCanvasModel
	{
		private readonly Dictionary<OverlayCanvas, List<Canvas>> _overlays = new Dictionary<OverlayCanvas, List<Canvas>>();

		public void RegisterOverlay(OverlayCanvas overlayCanvas, Canvas canvas)
		{
			if (!_overlays.TryGetValue(overlayCanvas, out var value))
			{
				_overlays[overlayCanvas] = new List<Canvas> { canvas };
			}
			else
			{
				value.Add(canvas);
			}
		}

		public void UnRegisterOverlay(OverlayCanvas overlayCanvas, Canvas canvas)
		{
			if (_overlays.TryGetValue(overlayCanvas, out var value))
			{
				value.Remove(canvas);
			}
		}

		public List<Canvas> GetOverlay(OverlayCanvas overlayCanvas)
		{
			return _overlays[overlayCanvas];
		}
	}
}
