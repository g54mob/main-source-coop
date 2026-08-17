using System;
using UnityEngine;

namespace Febucci.UI.Core
{
	[Serializable]
	[CreateAssetMenu(fileName = "TextAnimator GlobalData", menuName = "TextAnimator/Create Global Text Animator Data")]
	public class TAnimGlobalDataScriptable : ScriptableObject
	{
		public const string resourcesPath = "TextAnimator GlobalData";

		[SerializeField]
		internal PresetBehaviorValues[] globalBehaviorPresets = new PresetBehaviorValues[0];

		[SerializeField]
		internal PresetAppearanceValues[] globalAppearancePresets = new PresetAppearanceValues[0];

		[SerializeField]
		internal string[] customActions = new string[0];

		[SerializeField]
		internal bool customTagsFormatting;

		[SerializeField]
		internal TAnimBuilder.TagFormatting tagInfo_behaviors = new TAnimBuilder.TagFormatting('<', '>');

		[SerializeField]
		internal TAnimBuilder.TagFormatting tagInfo_appearances = new TAnimBuilder.TagFormatting('{', '}');
	}
}
