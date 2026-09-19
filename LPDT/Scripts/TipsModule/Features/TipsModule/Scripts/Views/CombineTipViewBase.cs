using Features.TutorialModule.Scripts.UIGlow;
using TMPro;
using UnityEngine.UI;

namespace Features.TipsModule.Scripts.Views
{
	public abstract class CombineTipViewBase : TipSlotViewBase
	{
		public Image PrimaryTipIcon;

		public TMP_Text PrimaryTipIconText;

		public Image SecondaryTipIcon;

		public TMP_Text SecondaryTipIconText;

		public TMP_Text TipText;

		public GlowableTipObject GlowableTipObject;
	}
}
