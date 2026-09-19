using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine;

namespace Features.StoreModule.Scripts.Views
{
	public abstract class StoreStateQuotaViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public TMP_Text QuotaText { get; private set; }

		[field: SerializeField]
		public GameObject VisibilityRoot { get; private set; }

		[field: SerializeField]
		public float LerpSpeed { get; private set; } = 5f;

		[field: SerializeField]
		public AnimationCurve LerpCurve { get; private set; } = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[field: SerializeField]
		public RectTransform Container { get; private set; }

		public void SetVisibilityRootActive(bool active)
		{
			if (VisibilityRoot != null)
			{
				VisibilityRoot.SetActive(active);
			}
		}

		public void SetQuotaLabel(string text)
		{
			if (QuotaText != null)
			{
				QuotaText.SetText(text);
			}
		}
	}
}
