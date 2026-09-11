using UnityEngine;

namespace EasyTextEffects
{
	public static class TimeUtil
	{
		public static float GetTime()
		{
			if (Application.isPlaying)
			{
				return Time.time;
			}
			return 0f;
		}
	}
}
