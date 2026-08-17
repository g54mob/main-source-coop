using UnityEngine;

namespace QFSW.QC.Extras
{
	public static class TimeCommands
	{
		[Command("time-scale", "the scale at which time is passing by.", Platform.AllPlatforms, MonoTargetType.Single)]
		private static float TimeScale
		{
			get
			{
				return Time.timeScale;
			}
			set
			{
				Time.timeScale = value;
			}
		}
	}
}
