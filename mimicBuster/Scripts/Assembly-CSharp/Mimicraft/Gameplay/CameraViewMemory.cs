using UnityEngine;

namespace Mimicraft.Gameplay
{
	public static class CameraViewMemory
	{
		private const string Key = "Mimicraft.CameraDistance";

		private static float? remembered;

		public static void Remember(float distance)
		{
			remembered = distance;
			PlayerPrefs.SetFloat("Mimicraft.CameraDistance", distance);
		}

		public static float Recall(float fallback)
		{
			if (remembered.HasValue)
			{
				return remembered.Value;
			}
			if (!PlayerPrefs.HasKey("Mimicraft.CameraDistance"))
			{
				return fallback;
			}
			return PlayerPrefs.GetFloat("Mimicraft.CameraDistance", fallback);
		}
	}
}
