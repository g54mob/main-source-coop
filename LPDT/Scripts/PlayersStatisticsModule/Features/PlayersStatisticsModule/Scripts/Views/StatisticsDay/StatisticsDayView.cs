using TMPro;
using UnityEngine;

namespace Features.PlayersStatisticsModule.Scripts.Views.StatisticsDay
{
	public class StatisticsDayView : StatisticsDayViewBase
	{
		[SerializeField]
		private TMP_Text _dayText;

		[SerializeField]
		private TMP_Text _chapterCompletedText;

		public override void SetDayText(string dayText)
		{
			if (_dayText != null)
			{
				_dayText.SetText(dayText);
			}
		}

		public override void SetChapterCompletedText(string chapterCompletedText)
		{
			if (_chapterCompletedText != null)
			{
				_chapterCompletedText.SetText(chapterCompletedText);
			}
		}

		public override void SetChapterCompletedVisible(bool visible)
		{
			if (_chapterCompletedText != null)
			{
				_chapterCompletedText.gameObject.SetActive(visible);
			}
		}
	}
}
