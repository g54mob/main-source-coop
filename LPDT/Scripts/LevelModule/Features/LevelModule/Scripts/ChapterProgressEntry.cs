using System;
using System.Collections.Generic;

namespace Features.LevelModule.Scripts
{
	[Serializable]
	public class ChapterProgressEntry
	{
		public LevelSequenceSet Set;

		public List<int> PassedChapters = new List<int>();

		public int HighestUnlockedChapterIndex;

		public List<ChapterSubLevelProgress> SubLevelProgress = new List<ChapterSubLevelProgress>();
	}
}
