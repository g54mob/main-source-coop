using System.Collections.Generic;
using UnityEngine;

namespace Features.MainMenuModule.Scripts.Views.Chapters.SupView
{
	public class ChapterProgressListSupView : MonoBehaviour
	{
		[SerializeField]
		private List<ChapterProgressItemSupView> _chapterProgressItemSupViews;

		public void SetTotalCount(int totalCount)
		{
			for (int i = 0; i < _chapterProgressItemSupViews.Count; i++)
			{
				_chapterProgressItemSupViews[i].gameObject.SetActive(i < totalCount);
			}
		}

		public void SetProgress(int totalCount, int reachedSubLevelCount, bool isChapterPassed)
		{
			SetTotalCount(totalCount);
			int lastEnteredIndex = reachedSubLevelCount - 1;
			for (int i = 0; i < _chapterProgressItemSupViews.Count; i++)
			{
				if (i < totalCount)
				{
					_chapterProgressItemSupViews[i].SetState(ResolveState(i, lastEnteredIndex, isChapterPassed));
				}
			}
		}

		public void SetState(int index, ChapterProgressItemState state)
		{
			if (index >= 0 && index < _chapterProgressItemSupViews.Count)
			{
				_chapterProgressItemSupViews[index].SetState(state);
			}
		}

		private ChapterProgressItemState ResolveState(int index, int lastEnteredIndex, bool isChapterPassed)
		{
			if (isChapterPassed || index < lastEnteredIndex)
			{
				return ChapterProgressItemState.Completed;
			}
			if (index == lastEnteredIndex)
			{
				return ChapterProgressItemState.Died;
			}
			return ChapterProgressItemState.Idle;
		}
	}
}
