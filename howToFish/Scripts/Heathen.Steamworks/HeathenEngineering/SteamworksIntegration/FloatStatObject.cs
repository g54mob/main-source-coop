using System;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	[HelpURL("https://kb.heathen.group/assets/steamworks/guides/stats-object")]
	public class FloatStatObject : StatObject
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

		public override DataType Type => DataType.Float;
	}
}
