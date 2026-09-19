using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenuModule.Scripts.Credits
{
	public abstract class CreditsViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public CreditsSectionViewBase SectionPrefab { get; private set; }

		[field: SerializeField]
		public Transform SectionsContainer { get; private set; }

		[field: SerializeField]
		public Button BackButton { get; private set; }

		[field: SerializeField]
		public Selectable FirstSelectable { get; private set; }

		[field: SerializeField]
		public ScrollRect ScrollRect { get; private set; }

		[field: SerializeField]
		public float AutoScrollSpeed { get; private set; } = 40f;

		[field: SerializeField]
		public CanvasGroup BannerCanvasGroup { get; private set; }

		[field: SerializeField]
		public CanvasGroup WholeCanvasGroup { get; private set; }

		[field: SerializeField]
		public float WindowFadeInDuration { get; private set; } = 0.3f;

		[field: SerializeField]
		public float BannerHoldDuration { get; private set; } = 4f;

		[field: SerializeField]
		public float BannerFadeDuration { get; private set; } = 1f;
	}
}
