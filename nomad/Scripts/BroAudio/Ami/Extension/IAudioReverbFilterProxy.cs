using UnityEngine;

namespace Ami.Extension
{
	public interface IAudioReverbFilterProxy
	{
		AudioReverbPreset reverbPreset { get; set; }

		float dryLevel { get; set; }

		float room { get; set; }

		float roomHF { get; set; }

		float decayTime { get; set; }

		float decayHFRatio { get; set; }

		float reflectionsLevel { get; set; }

		float reflectionsDelay { get; set; }

		float reverbLevel { get; set; }

		float reverbDelay { get; set; }

		float diffusion { get; set; }

		float density { get; set; }

		float hfReference { get; set; }

		float roomLF { get; set; }

		float lfReference { get; set; }

		bool enabled { get; set; }
	}
}
