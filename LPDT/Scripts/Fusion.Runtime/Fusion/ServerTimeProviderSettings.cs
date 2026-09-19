using System;

namespace Fusion
{
	internal struct ServerTimeProviderSettings
	{
		public double SimDeltaTime;

		public double MinimumInputDelayRaw;

		public TimeSyncConfiguration.Unit MinimumInputDelayUnit;

		public readonly double MinimumInputDelay
		{
			get
			{
				TimeSyncConfiguration.Unit minimumInputDelayUnit = MinimumInputDelayUnit;
				if (1 == 0)
				{
				}
				double result = minimumInputDelayUnit switch
				{
					TimeSyncConfiguration.Unit.Milliseconds => MinimumInputDelayRaw / 1000.0, 
					TimeSyncConfiguration.Unit.Ticks => MinimumInputDelayRaw * SimDeltaTime, 
					_ => throw new ArgumentOutOfRangeException("unit is invalid"), 
				};
				if (1 == 0)
				{
				}
				return result;
			}
		}

		public static ServerTimeProviderSettings Default()
		{
			TickRate.Resolved resolved = TickRate.Resolve(TickRate.Default);
			return new ServerTimeProviderSettings
			{
				SimDeltaTime = resolved.ClientTickDelta,
				MinimumInputDelayRaw = 0.0,
				MinimumInputDelayUnit = TimeSyncConfiguration.Unit.Milliseconds
			};
		}
	}
}
