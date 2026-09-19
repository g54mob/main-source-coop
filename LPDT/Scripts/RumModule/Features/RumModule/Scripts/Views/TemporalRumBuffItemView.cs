using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.RumModule.Scripts.Views
{
	public class TemporalRumBuffItemView : TemporalRumBuffItemViewBase
	{
		[SerializeField]
		private Image _rumIconImage;

		[SerializeField]
		private TMP_Text _stackCountText;

		[SerializeField]
		private TMP_Text _remainingSecondsText;

		public override void SetRumIcon(Sprite icon)
		{
			if (!(_rumIconImage == null))
			{
				_rumIconImage.sprite = icon;
				_rumIconImage.enabled = icon != null;
			}
		}

		public override void SetStackCount(int count)
		{
			if (!(_stackCountText == null))
			{
				_stackCountText.SetText((count > 1) ? $"x{count}" : string.Empty);
			}
		}

		public override void SetRemainingSeconds(int seconds)
		{
			if (_remainingSecondsText != null)
			{
				_remainingSecondsText.SetText($"{seconds}");
			}
		}
	}
}
