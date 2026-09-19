using System.Collections.Generic;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.OverlayUI
{
	public class UIOverlayService : IOverlayService
	{
		private readonly UIOverlayCanvasModel _uiOverlayCanvasModel;

		private readonly UIOverlayConfiguration _uiOverlayConfiguration;

		public UIOverlayService(UIOverlayCanvasModel uiOverlayCanvasModel, UIOverlayConfiguration uiOverlayConfiguration)
		{
			_uiOverlayCanvasModel = uiOverlayCanvasModel;
			_uiOverlayConfiguration = uiOverlayConfiguration;
		}

		public void SetOverlay(OverlayCanvas overlayCanvas)
		{
			List<Canvas> overlay = _uiOverlayCanvasModel.GetOverlay(overlayCanvas);
			for (int i = 0; i < overlay.Count; i++)
			{
				if (!(overlay[i] == null))
				{
					overlay[i].overrideSorting = true;
					overlay[i].sortingOrder = _uiOverlayConfiguration.OverlaySortingOrder;
				}
			}
		}

		public void SetDefault(OverlayCanvas overlayCanvas)
		{
			List<Canvas> overlay = _uiOverlayCanvasModel.GetOverlay(overlayCanvas);
			for (int i = 0; i < overlay.Count; i++)
			{
				if (!(overlay[i] == null))
				{
					overlay[i].sortingOrder = _uiOverlayConfiguration.DefaultSortingOrder;
					overlay[i].overrideSorting = false;
				}
			}
		}
	}
}
