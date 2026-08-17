using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NomadDrive.Features.Interaction.UI.InteractionUIObjects
{
	public class InteractionUIObject : MonoBehaviour
	{
		[SerializeField]
		private Image interactionKeyImage;

		[SerializeField]
		private TextMeshProUGUI interactionText;

		public void Set(Sprite interactionKeySprite, string interactionString)
		{
			interactionKeyImage.sprite = interactionKeySprite;
			interactionText.text = interactionString;
		}

		public void UpdateUI(string interactionString, Sprite interactionKeySprite)
		{
			interactionText.text = interactionString;
			interactionKeyImage.sprite = interactionKeySprite;
		}
	}
}
