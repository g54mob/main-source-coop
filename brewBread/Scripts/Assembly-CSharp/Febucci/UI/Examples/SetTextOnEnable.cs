using UnityEngine;

namespace Febucci.UI.Examples
{
	[AddComponentMenu("Febucci/TextAnimator/Utilities/SetTextOnEnable")]
	public class SetTextOnEnable : MonoBehaviour
	{
		public TextAnimatorPlayer tanimPlayer;

		private string textToSet;

		private void Awake()
		{
			textToSet = tanimPlayer.textAnimator.tmproText.text;
			tanimPlayer.ShowText("");
		}

		private void OnEnable()
		{
			tanimPlayer.ShowText(textToSet);
		}

		private void OnDisable()
		{
			if (tanimPlayer != null)
			{
				tanimPlayer.StopShowingText();
				tanimPlayer.ShowText("");
			}
		}
	}
}
