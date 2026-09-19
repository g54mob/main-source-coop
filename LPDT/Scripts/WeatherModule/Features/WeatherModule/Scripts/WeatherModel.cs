using System;
using Features.NetworkRandomModule.Scripts;
using JetBrains.Annotations;

namespace Features.WeatherModule.Scripts
{
	[Serializable]
	public class WeatherModel : INetworkRandomConsumer
	{
		private static readonly DeterministicHash NameHash = new DeterministicHash(typeof(WeatherModel).FullName);

		private WeatherPreset _debugUsedWeather;

		[CanBeNull]
		public WeatherPreset LastUsedWeather { get; set; }

		[CanBeNull]
		public WeatherPreset DebugUsedWeather
		{
			get
			{
				return _debugUsedWeather;
			}
			set
			{
				_debugUsedWeather = value;
				this.OnDebugUsedWeatherChanged?.Invoke(_debugUsedWeather);
			}
		}

		public Random Random { get; private set; }

		public event Action<WeatherPreset> OnDebugUsedWeatherChanged;

		public int GetConsumerIdentifier()
		{
			return NameHash.GetRaw();
		}

		public void InjectNetworkRandom(Random random)
		{
			Random = random;
		}
	}
}
