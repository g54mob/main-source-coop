using Global.Modules.LocalizationModule.Scripts.Generated;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenuModule.Scripts.Views.Chapters
{
	public abstract class ChapterTypeOptionViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public Button ClickButton { get; private set; }

		public abstract void SetNameLocalizationKey(LocalizationKey localizationKey);

		public abstract void SetSelected(bool isSelected);
	}
}
