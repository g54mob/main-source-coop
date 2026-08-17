using System;
using UnityEngine;

namespace EvilCore.EvilSave
{
	[Serializable]
	public class SaveSlotMetadata
	{
		public string slotId;

		public string displayName;

		public int saveVersion;

		public string createdAt;

		public string lastSavedAt;

		public float playtimeSeconds;

		public static SaveSlotMetadata Create(string slotId, string displayName = null, int version = 1)
		{
			string text = DateTime.UtcNow.ToString("o");
			return new SaveSlotMetadata
			{
				slotId = slotId,
				displayName = (displayName ?? slotId),
				saveVersion = version,
				createdAt = text,
				lastSavedAt = text,
				playtimeSeconds = 0f
			};
		}

		public string ToJson()
		{
			return JsonUtility.ToJson(this, prettyPrint: true);
		}

		public static SaveSlotMetadata FromJson(string json)
		{
			return JsonUtility.FromJson<SaveSlotMetadata>(json);
		}
	}
}
