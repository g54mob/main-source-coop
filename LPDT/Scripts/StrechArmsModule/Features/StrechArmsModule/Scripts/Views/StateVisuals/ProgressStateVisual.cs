using UnityEngine;
using UnityEngine.UI;

namespace Features.StrechArmsModule.Scripts.Views.StateVisuals
{
	public class ProgressStateVisual : ArmStateVisual
	{
		[SerializeField]
		private Image _fillProgressImage;

		public override void Enable()
		{
			base.gameObject.SetActive(value: true);
			_fillProgressImage.fillAmount = 0f;
		}

		public override void Disable()
		{
			base.gameObject.SetActive(value: false);
			_fillProgressImage.fillAmount = 0f;
		}

		public override void SetProgress(float progress)
		{
			_fillProgressImage.fillAmount = 1f - progress;
		}
	}
}
