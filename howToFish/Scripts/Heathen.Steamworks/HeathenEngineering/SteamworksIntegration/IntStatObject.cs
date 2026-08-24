using System;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	[HelpURL("https://kb.heathen.group/assets/steamworks/guides/stats-object")]
	public class IntStatObject : StatObject
	{
		public int Value
		{
			get
			{
				return data.IntValue();
			}
			set
			{
				data.Set(value);
			}
		}

		public override DataType Type => DataType.Int;
	}
}
