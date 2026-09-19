using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.EmotesModule.Scripts.Views
{
	public abstract class EmoteListItemViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public Image EmoteImage { get; private set; }

		[field: SerializeField]
		public TMP_Text KeyText { get; private set; }

		[field: SerializeField]
		public Animator Animator { get; private set; }

		public int EmoteIndex { get; internal set; }
	}
}
