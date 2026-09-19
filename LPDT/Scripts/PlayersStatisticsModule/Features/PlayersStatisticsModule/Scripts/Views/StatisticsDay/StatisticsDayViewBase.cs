using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.PlayersStatisticsModule.Scripts.Views.StatisticsDay
{
	public abstract class StatisticsDayViewBase : ViewBehaviour
	{
		public abstract void SetDayText(string dayText);

		public abstract void SetChapterCompletedText(string chapterCompletedText);

		public abstract void SetChapterCompletedVisible(bool visible);
	}
}
