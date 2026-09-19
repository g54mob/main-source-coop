using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.OverlayUI
{
	[RequireComponent(typeof(Canvas))]
	public class WriteToOverlay : MonoBehaviour
	{
		[SerializeField]
		private OverlayCanvas _overlayCanvas;

		private UIOverlayCanvasModel _uiOverlayCanvasModel;

		[Inject]
		private void InjectDependencies(UIOverlayCanvasModel uiOverlayCanvasModel)
		{
			_uiOverlayCanvasModel = uiOverlayCanvasModel;
		}

		private void Awake()
		{
			_uiOverlayCanvasModel?.RegisterOverlay(_overlayCanvas, GetComponent<Canvas>());
		}

		public void OverrideOverlayCanvas(OverlayCanvas overlayCanvas)
		{
			_uiOverlayCanvasModel.UnRegisterOverlay(_overlayCanvas, GetComponent<Canvas>());
			_uiOverlayCanvasModel.RegisterOverlay(overlayCanvas, GetComponent<Canvas>());
		}
	}
}
