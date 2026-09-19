using System.Collections.Generic;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.EmotesModule.Scripts.Views
{
	public abstract class EmotesDropdownViewBase : ViewBehaviour
	{
		public GameObject EmotesHotBarContainer;

		public GridLayoutGroup EmotesGridLayoutGroup;

		public List<RectTransform> RectTransformToRebuilds;

		public CanvasGroup CanvasGroup;

		public float FadeDuration = 0.25f;

		public float AutoHideDelay = 5f;

		public List<EmoteListItemViewBase> EmotesList;

		public List<EmoteHotbarItemViewBase> EmotesHotbarList;

		public GameObject GamepadTip;

		public Vector2 EmotesGridLayoutGroupAnimationSpacing = new Vector2(-20f, -20f);

		public float EmotesGridLayoutGroupAnimationDuration = 0.25f;
	}
}
