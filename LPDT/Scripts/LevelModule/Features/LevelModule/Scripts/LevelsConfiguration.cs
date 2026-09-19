using System.Collections.Generic;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.LevelModule.Scripts
{
	[CreateAssetMenu(fileName = "LevelsConfiguration_Default", menuName = "Configurations/Levels/LevelsConfiguration")]
	public class LevelsConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<LevelType, string> LevelsPool { get; private set; }

		[field: SerializeField]
		public SerializableDictionary<LevelType, List<LevelType>> LevelsSequence { get; private set; }

		[field: SerializeField]
		public SerializableDictionary<LevelSequenceSet, List<Chapter>> ChapterSequences { get; private set; }

		public List<ChapterType> GetChapterTypes(LevelSequenceSet sequenceSet)
		{
			List<ChapterType> list = new List<ChapterType>();
			if (!ChapterSequences.TryGetValue(sequenceSet, out var value))
			{
				return list;
			}
			foreach (Chapter item in value)
			{
				if (item.ChapterType != ChapterType.None && !list.Contains(item.ChapterType))
				{
					list.Add(item.ChapterType);
				}
			}
			return list;
		}

		public List<int> GetChapterIndexesOfType(LevelSequenceSet sequenceSet, ChapterType chapterType)
		{
			List<int> list = new List<int>();
			if (!ChapterSequences.TryGetValue(sequenceSet, out var value))
			{
				return list;
			}
			for (int i = 0; i < value.Count; i++)
			{
				if (value[i].ChapterType == chapterType)
				{
					list.Add(i);
				}
			}
			return list;
		}

		public int GetSequenceLevelNumber(LevelSequenceSet sequenceSet, int chapterIndex, int levelInChapterIndex)
		{
			if (sequenceSet == LevelSequenceSet.None)
			{
				sequenceSet = LevelSequenceSet.Default;
			}
			int num = ((levelInChapterIndex < 1) ? 1 : levelInChapterIndex);
			if (!ChapterSequences.TryGetValue(sequenceSet, out var value) || value.Count == 0)
			{
				return num;
			}
			int num2 = 0;
			int num3 = ((chapterIndex >= 0) ? chapterIndex : 0);
			for (int i = 0; i < num3 && i < value.Count; i++)
			{
				num2 += value[i].Levels.Count;
			}
			return num2 + num;
		}
	}
}
