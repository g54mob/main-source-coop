using System;

namespace Features.TeethModule.Scripts.Tooth
{
	[Serializable]
	public class ToothData
	{
		public ToothPreset Preset;

		public bool IsKnocked;

		public ToothData(ToothPreset preset, bool isKnocked)
		{
			Preset = preset;
			IsKnocked = isKnocked;
		}
	}
}
