using UnityEngine;

namespace Febucci.UI.Examples
{
	[AddComponentMenu("")]
	public class DefaultEffectsExample : MonoBehaviour
	{
		public TextAnimatorPlayer textAnimatorPlayer;

		private void Awake()
		{
		}

		private void Start()
		{
			string text = "<b>You can add effects by using <color=red>rich text tags</color>.</b>" + $"\nExample: writing {'"'}<noparse><shake>I'm cold</shake></noparse>{'"'} will result in {'"'}<shake>I'm cold</shake>{'"'}." + $"\n\n Effects that animate through time are called {'"'}<color=red>Behaviors</color>{'"'}, and the default tags are: ";
			for (int i = 0; i < TAnimTags.defaultBehaviors.Length; i++)
			{
				text += EffectsTesting.AddEffect(TAnimTags.defaultBehaviors[i]);
			}
			text += $"\n\n<b>Effects that animate letters while they appear on screen are called {'"'}<color=red>Appearances</color>{'"'} and the default tags are</b>: ";
			for (int j = 0; j < TAnimTags.defaultAppearances.Length; j++)
			{
				text += EffectsTesting.AddAppearanceEffect(TAnimTags.defaultAppearances[j]);
			}
			textAnimatorPlayer.ShowText(text);
		}
	}
}
