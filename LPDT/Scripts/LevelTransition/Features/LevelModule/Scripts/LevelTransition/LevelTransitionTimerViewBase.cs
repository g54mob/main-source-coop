using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.LevelModule.Scripts.LevelTransition
{
	public abstract class LevelTransitionTimerViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public CanvasGroup CanvasGroup { get; private set; }

		[field: SerializeField]
		public AnimationCurve LerpCurve { get; private set; } = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[field: SerializeField]
		public float FadeDuration { get; private set; }

		[field: SerializeField]
		public float DelayBeforeFade { get; private set; }

		[field: SerializeField]
		public Image Fill { get; private set; }

		[field: SerializeField]
		public RectTransform Arrow { get; private set; }
	}
}
