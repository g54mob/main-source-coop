using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenuModule.Scripts.Views.Chapters
{
	public abstract class ChapterOptionViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public Button ClickButton { get; private set; }

		public abstract void SetChapterNumber(int chapterNumber);

		public abstract void SetLevelsCount(int levelsCount);

		public abstract void SetSelected(bool isSelected);

		public abstract void SetLocked(bool isLocked);

		public abstract void SetChapterData(ChapterPreviewData chapterPreviewData);

		public abstract void SetProgress(int levelsCount, int reachedSubLevelCount, bool isChapterPassed);
	}
}
