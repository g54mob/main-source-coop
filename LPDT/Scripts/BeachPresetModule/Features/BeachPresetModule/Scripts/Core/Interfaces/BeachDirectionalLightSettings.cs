using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Core.Interfaces
{
	public class BeachDirectionalLightSettings
	{
		public bool Enabled { get; set; }

		public Color Color { get; set; }

		public float Intensity { get; set; }

		public Quaternion Rotation { get; set; }
	}
}
