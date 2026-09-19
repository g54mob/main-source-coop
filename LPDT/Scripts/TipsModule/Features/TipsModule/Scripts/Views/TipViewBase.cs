using Features.TutorialModule.Scripts.UIGlow;
using TMPro;
using UnityEngine.UI;

namespace Features.TipsModule.Scripts.Views
{
	public abstract class TipViewBase : TipSlotViewBase
	{
		public Image TipIcon;

		public TMP_Text TipIconText;

		public TMP_Text TipText;

		public GlowableTipObject GlowableTipObject;
	}
}
