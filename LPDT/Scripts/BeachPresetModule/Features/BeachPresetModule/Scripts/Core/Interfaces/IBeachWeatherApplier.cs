namespace Features.BeachPresetModule.Scripts.Core.Interfaces
{
	public interface IBeachWeatherApplier
	{
		void Apply(BeachWeatherState state);

		void Clear(BeachWeatherState state);
	}
}
