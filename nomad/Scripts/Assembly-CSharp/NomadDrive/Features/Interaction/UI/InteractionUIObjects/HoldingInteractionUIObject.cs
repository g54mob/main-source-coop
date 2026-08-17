using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace NomadDrive.Features.Interaction.UI.InteractionUIObjects
{
	public class HoldingInteractionUIObject : InteractionUIObject
	{
		[SerializeField]
		private Image holdingInteractionProgressImage;

		private float _interactionDuration;

		private Sequence _interactionProgressSequence;

		public void Set(Sprite keySprite, string interactionString, float interactionDuration)
		{
			Set(keySprite, interactionString);
			_interactionDuration = interactionDuration;
			ResetInteractionProgressImageFillAmount();
		}

		public void FillInteractionProgressImage()
		{
			_interactionProgressSequence = Sequence.Create().Chain(Tween.UIFillAmount(holdingInteractionProgressImage, 1f, _interactionDuration, Ease.Linear));
		}

		public void CancelFillInteractionProgressImage()
		{
			_interactionProgressSequence.Stop();
			holdingInteractionProgressImage.fillAmount = 0f;
		}

		public void ResetInteractionProgressImageFillAmount()
		{
			_interactionProgressSequence.Stop();
			holdingInteractionProgressImage.fillAmount = 0f;
		}
	}
}
