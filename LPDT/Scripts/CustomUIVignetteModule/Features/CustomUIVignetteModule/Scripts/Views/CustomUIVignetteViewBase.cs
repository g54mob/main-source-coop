using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.CustomUIVignetteModule.Scripts.Views
{
	public abstract class CustomUIVignetteViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public AnimationCurve VignetteCurve { get; private set; }

		[field: SerializeField]
		public CanvasGroup VignetteCanvasGroup { get; private set; }
	}
}
