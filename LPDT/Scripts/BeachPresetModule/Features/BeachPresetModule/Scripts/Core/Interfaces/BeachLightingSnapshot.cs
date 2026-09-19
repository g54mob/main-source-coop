using System.Collections.Generic;
using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Core.Interfaces
{
	public class BeachLightingSnapshot
	{
		internal BeachRenderSettingsSnapshot RenderSettingsSnapshot { get; set; }

		internal Light PreviousSun { get; set; }

		internal Dictionary<Light, BeachLightSnapshot> LightSnapshots { get; } = new Dictionary<Light, BeachLightSnapshot>();
	}
}
