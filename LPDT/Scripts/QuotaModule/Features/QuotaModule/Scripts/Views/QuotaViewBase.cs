using System.Collections.Generic;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.QuotaModule.Scripts.Views
{
	public abstract class QuotaViewBase : ViewBehaviour
	{
		[SerializeField]
		private List<Image> _inHandImages = new List<Image>();

		[field: SerializeField]
		public CircularArcSlider QuotaHandleSlider { get; private set; }

		[field: SerializeField]
		public CircularArcSlider InHandHandleSlider { get; private set; }

		[field: SerializeField]
		public TMP_Text QuotaText { get; private set; }

		[field: SerializeField]
		public float LerpDuration { get; private set; } = 0.5f;

		[field: SerializeField]
		public AnimationCurve LerpCurve { get; private set; } = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[field: SerializeField]
		public CanvasGroup CanvasGroup { get; private set; }

		[field: SerializeField]
		public CanvasGroup GlobalCanvasGroup { get; private set; }

		[field: SerializeField]
		public float FadeDuration { get; private set; }

		[field: SerializeField]
		public float AllFadeDuration { get; private set; }

		[field: SerializeField]
		public float AllFadeDurationExit { get; private set; } = 0.5f;

		[field: SerializeField]
		public float DelayBeforeFade { get; private set; }

		[field: SerializeField]
		public RectTransform Container { get; private set; }

		public void SetInHandSliderColor(Color primaryColor)
		{
			foreach (Image inHandImage in _inHandImages)
			{
				inHandImage.color = primaryColor;
			}
		}
	}
}
