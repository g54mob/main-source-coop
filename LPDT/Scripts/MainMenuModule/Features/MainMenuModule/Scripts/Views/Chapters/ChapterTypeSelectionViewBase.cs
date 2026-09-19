using Features.UINavigationModuleRealization.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenuModule.Scripts.Views.Chapters
{
	public abstract class ChapterTypeSelectionViewBase : ViewBehaviour
	{
		[SerializeField]
		private Transform _optionsContainer;

		[field: SerializeField]
		public ChapterTypeOptionViewBase ChapterTypeOptionViewBase { get; private set; }

		[field: SerializeField]
		public ScrollNavigation ScrollNavigation { get; private set; }

		[field: SerializeField]
		public Button LeftNavigationButton { get; private set; }

		[field: SerializeField]
		public Button RightNavigationButton { get; private set; }

		[field: SerializeField]
		public RectTransform RightNavigationContainer { get; private set; }

		[field: SerializeField]
		public RectTransform LeftNavigationContainer { get; private set; }

		public Transform GetOptionsContainer()
		{
			return _optionsContainer;
		}
	}
}
