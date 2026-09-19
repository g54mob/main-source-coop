using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.EmotesModule.Scripts.Views
{
	public abstract class EmoteHotbarItemViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public Image EmoteImage { get; private set; }

		[field: SerializeField]
		public TMP_Text KeyText { get; private set; }

		[field: SerializeField]
		public Animator Animator { get; private set; }

		[field: SerializeField]
		public EmoteGroup EmoteGroup { get; private set; }
	}
}
