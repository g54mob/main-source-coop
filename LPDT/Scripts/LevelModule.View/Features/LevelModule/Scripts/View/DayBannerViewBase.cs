using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.LevelModule.Scripts.View
{
	public abstract class DayBannerViewBase : ViewBehaviour
	{
		public abstract void SetDay(int chapterNumber, int dayInChapter);

		public abstract void Show();
	}
}
