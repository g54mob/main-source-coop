using System.Collections.Generic;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.Data
{
	[CreateAssetMenu(fileName = "RegionsPingConfiguration_Default", menuName = "Configurations/LobbyModule/RegionsPingConfiguration")]
	public class RegionsPingConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public float RegionsPingUpdateDelay { get; private set; }

		[field: SerializeField]
		public List<PingVisual> PingVisuals { get; private set; } = new List<PingVisual>();

		public Color EvaluateColor(float pingMs)
		{
			if (PingVisuals == null || PingVisuals.Count == 0)
			{
				return Color.gray;
			}
			PingVisuals.Sort((PingVisual pingVisual, PingVisual b) => pingVisual.TargetValue.CompareTo(b.TargetValue));
			for (int num = 0; num < PingVisuals.Count; num++)
			{
				if (pingMs <= PingVisuals[num].TargetValue)
				{
					return PingVisuals[num].Color;
				}
			}
			List<PingVisual> pingVisuals = PingVisuals;
			return pingVisuals[pingVisuals.Count - 1].Color;
		}

		public string EvaluateHex(float pingMs)
		{
			return ColorUtility.ToHtmlStringRGB(EvaluateColor(pingMs));
		}
	}
}
