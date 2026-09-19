namespace Features.BeachPresetModule.Scripts.Core.Interfaces
{
	public interface IBeachLightingService
	{
		BeachLightingSnapshot Capture(BeachLightingSettings settings);

		void Apply(BeachLightingSettings settings);

		void Restore(BeachLightingSnapshot snapshot);
	}
}
