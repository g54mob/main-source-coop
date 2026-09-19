using Global.SerializableDictionary;
using UnityEngine;

namespace Features.MainMenuModule.Scripts.Views.Chapters
{
	[CreateAssetMenu(fileName = "ChapterPreviewConfiguration_Default", menuName = "Configurations/Chapters/ChapterPreviewConfiguration")]
	public class ChapterPreviewConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<int, ChapterPreviewData> ChapterPreviews { get; private set; }

		[field: SerializeField]
		public SerializableDictionary<ChapterFrameType, ChapterFrameData> Frames { get; private set; }

		[field: SerializeField]
		public ChapterPreviewData FallbackPreview { get; private set; }

		public ChapterPreviewData GetPreviewOrFallback(int chapterIndex)
		{
			if (ChapterPreviews == null || !ChapterPreviews.TryGetValue(chapterIndex, out var value))
			{
				return FallbackPreview;
			}
			return value;
		}

		public void OnValidate()
		{
			foreach (ChapterPreviewData value2 in ChapterPreviews.Values)
			{
				value2.FrameData = ((Frames != null && Frames.TryGetValue(value2.FrameType, out var value)) ? value : null);
			}
		}
	}
}
