using Features.LevelModule.Scripts;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.MainMenuModule.Scripts.Views.Chapters
{
	[CreateAssetMenu(fileName = "ChapterTypeLocalizationConfiguration_Default", menuName = "Configurations/Chapters/ChapterTypeLocalizationConfiguration")]
	public class ChapterTypeLocalizationConfiguration : ScriptableObject
	{
		[SerializeField]
		private LocalizationKey _defaultValue;

		[SerializeField]
		private SerializableDictionary<ChapterType, LocalizationKey> _chapterTypeLocalizationKeys;

		public LocalizationKey GetLocalizationKeyForChapterType(ChapterType chapterType)
		{
			if (_chapterTypeLocalizationKeys.TryGetValue(chapterType, out var value))
			{
				return value;
			}
			return _defaultValue;
		}
	}
}
