using System;
using System.Globalization;

namespace EvilCore
{
	public readonly struct SaveSlotInfo
	{
		public readonly string SlotId;

		public readonly string DisplayName;

		public readonly string LastSavedAtIso;

		public readonly float PlaytimeSeconds;

		public readonly string ThumbnailPath;

		public bool HasThumbnail => !string.IsNullOrEmpty(ThumbnailPath);

		public DateTime LastSavedLocal
		{
			get
			{
				if (!DateTime.TryParse(LastSavedAtIso, null, DateTimeStyles.RoundtripKind, out var result))
				{
					return DateTime.MinValue;
				}
				return result.ToLocalTime();
			}
		}

		public SaveSlotInfo(string slotId, string displayName, string lastSavedAtIso, float playtimeSeconds, string thumbnailPath)
		{
			SlotId = slotId;
			DisplayName = displayName;
			LastSavedAtIso = lastSavedAtIso;
			PlaytimeSeconds = playtimeSeconds;
			ThumbnailPath = thumbnailPath;
		}
	}
}
