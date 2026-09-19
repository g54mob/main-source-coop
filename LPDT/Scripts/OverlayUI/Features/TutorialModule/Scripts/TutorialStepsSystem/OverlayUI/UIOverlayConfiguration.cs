using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.OverlayUI
{
	[CreateAssetMenu(fileName = "UIOverlayConfiguration_Default", menuName = "Configurations/OverlayUI/UIOverlayConfiguration")]
	public class UIOverlayConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public int OverlaySortingOrder { get; private set; }

		[field: SerializeField]
		public int DefaultSortingOrder { get; private set; }
	}
}
