using System;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	[HelpURL("https://kb.heathen.group/assets/steamworks/guides/stats-object")]
	public class AvgRateStatObject : StatObject
	{
		public float Value
		{
			get
			{
				return data.FloatValue();
			}
			set
			{
				data.Set(value);
			}
		}

		public override DataType Type => DataType.AvgRate;

		public void UpdateAvgRateStat(float value, double length)
		{
			data.Set(value, length);
		}
	}
}
