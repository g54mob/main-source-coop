using Febucci.UI.Core;
using UnityEngine;

namespace Febucci.UI.Examples
{
	[AddComponentMenu("")]
	public class EffectsTesting : MonoBehaviour
	{
		public TextAnimatorPlayer textAnimatorPlayer;

		private void Awake()
		{
			TAnimBuilder.InitializeGlobalDatabase();
		}

		private void Start()
		{
			ShowText();
		}

		public static string AddEffect(string tag)
		{
			return "<" + tag + "><noparse><" + tag + "></noparse></" + tag + ">, ";
		}

		public static string AddAppearanceEffect(string tag)
		{
			return "{" + tag + "}<noparse>{" + tag + "}</noparse>{/" + tag + "}, ";
		}

		public void ShowText()
		{
			string text = "Detected Behavior effects:\n";
			string[] allBehaviorsTags = TAnimBuilder.GetAllBehaviorsTags();
			string[] allApppearancesTags = TAnimBuilder.GetAllApppearancesTags();
			for (int i = 0; i < allBehaviorsTags.Length; i++)
			{
				text += AddEffect(allBehaviorsTags[i]);
			}
			text += "\n\nDetected Appearance effects:\n";
			for (int j = 0; j < allApppearancesTags.Length; j++)
			{
				text += AddAppearanceEffect(allApppearancesTags[j]);
			}
			textAnimatorPlayer.ShowText(text);
		}
	}
}
