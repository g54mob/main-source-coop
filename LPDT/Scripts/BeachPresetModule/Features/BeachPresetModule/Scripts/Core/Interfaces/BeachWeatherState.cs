namespace Features.BeachPresetModule.Scripts.Core.Interfaces
{
	public readonly struct BeachWeatherState
	{
		public readonly string WeatherId;

		public readonly float Intensity;

		public readonly int Seed;

		public BeachWeatherState(string weatherId, float intensity, int seed)
		{
			WeatherId = weatherId;
			Intensity = intensity;
			Seed = seed;
		}
	}
}
