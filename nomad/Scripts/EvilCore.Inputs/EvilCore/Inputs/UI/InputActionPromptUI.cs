using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EvilCore.Inputs.UI
{
	public class InputActionPromptUI : MonoBehaviour
	{
		[SerializeField]
		private Image actionIcon;

		[SerializeField]
		private TextMeshProUGUI actionText;

		public void Initialize(Sprite actionSprite, string actionString)
		{
			SetActionIcon(actionSprite);
			SetActionText(actionString);
		}

		private void SetActionIcon(Sprite sprite)
		{
			actionIcon.sprite = sprite;
		}

		private void SetActionText(string text)
		{
			actionText.text = text;
		}
	}
}
