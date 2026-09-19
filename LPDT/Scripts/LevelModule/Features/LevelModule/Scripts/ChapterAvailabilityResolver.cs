using System;
using System.Collections.Generic;

namespace Features.LevelModule.Scripts
{
	public static class ChapterAvailabilityResolver
	{
		public static bool IsChapterAvailable(IReadOnlyList<Chapter> chapters, Func<int, bool> isChapterPassed, int chapterIndex)
		{
			if (chapters == null || chapterIndex < 0 || chapterIndex >= chapters.Count)
			{
				return false;
			}
			if (chapterIndex == 0)
			{
				return true;
			}
			if (isChapterPassed(chapterIndex) || isChapterPassed(chapterIndex - 1))
			{
				return true;
			}
			if (chapters[chapterIndex].IsOptional)
			{
				return false;
			}
			for (int num = chapterIndex - 1; num >= 0; num--)
			{
				if (!chapters[num].IsOptional)
				{
					return isChapterPassed(num);
				}
			}
			return true;
		}
	}
}
