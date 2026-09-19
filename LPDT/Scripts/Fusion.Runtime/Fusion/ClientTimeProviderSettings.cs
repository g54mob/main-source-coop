using System;

namespace Fusion
{
	internal struct ClientTimeProviderSettings
	{
		public Topologies Topology;

		public double TimeScaleOffsetMax;

		public double SampleWindowSeconds;

		public double OutgoingQuantile;

		public double IncomingQuantile;

		public double OutgoingRedundancy;

		public double IncomingRedundancy;

		public double OutgoingAddedRaw;

		public TimeSyncConfiguration.Unit OutgoingAddedUnit;

		public double IncomingAddedRaw;

		public TimeSyncConfiguration.Unit IncomingAddedUnit;

		public int OutgoingSendRate;

		public int IncomingSendRate;

		public double OutgoingSendDelta;

		public double IncomingSendDelta;

		public int MaxResimulations;

		public double MinimumInputDelayRaw;

		public TimeSyncConfiguration.Unit MinimumInputDelayUnit;

		public int ClientTickRate;

		public double ClientSimDeltaTime;

		public int ServerTickRate;

		public double ServerSimDeltaTime;

		public readonly double OutgoingAdded
		{
			get
			{
				TimeSyncConfiguration.Unit outgoingAddedUnit = OutgoingAddedUnit;
				if (1 == 0)
				{
				}
				double result = outgoingAddedUnit switch
				{
					TimeSyncConfiguration.Unit.Milliseconds => OutgoingAddedRaw / 1000.0, 
					TimeSyncConfiguration.Unit.Ticks => OutgoingAddedRaw * ClientSimDeltaTime, 
					_ => throw new ArgumentOutOfRangeException("unit is invalid"), 
				};
				if (1 == 0)
				{
				}
				return result;
			}
		}

		public readonly double IncomingAdded
		{
			get
			{
				TimeSyncConfiguration.Unit incomingAddedUnit = IncomingAddedUnit;
				if (1 == 0)
				{
				}
				double result = incomingAddedUnit switch
				{
					TimeSyncConfiguration.Unit.Milliseconds => IncomingAddedRaw / 1000.0, 
					TimeSyncConfiguration.Unit.Ticks => IncomingAddedRaw * ClientSimDeltaTime, 
					_ => throw new ArgumentOutOfRangeException("unit is invalid"), 
				};
				if (1 == 0)
				{
				}
				return result;
			}
		}

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
					TimeSyncConfiguration.Unit.Ticks => MinimumInputDelayRaw * ClientSimDeltaTime, 
					_ => throw new ArgumentOutOfRangeException("unit is invalid"), 
				};
				if (1 == 0)
				{
				}
				return result;
			}
		}

		public static ClientTimeProviderSettings Default()
		{
			TickRate.Resolved resolved = TickRate.Resolve(TickRate.Default);
			return new ClientTimeProviderSettings
			{
				Topology = Topologies.ClientServer,
				TimeScaleOffsetMax = 0.05,
				SampleWindowSeconds = 1.0,
				OutgoingQuantile = 0.949999988079071,
				IncomingQuantile = 0.949999988079071,
				OutgoingRedundancy = 1.0,
				IncomingRedundancy = 1.0,
				OutgoingAddedRaw = 0.0,
				OutgoingAddedUnit = TimeSyncConfiguration.Unit.Milliseconds,
				IncomingAddedRaw = 0.0,
				IncomingAddedUnit = TimeSyncConfiguration.Unit.Milliseconds,
				OutgoingSendRate = resolved.ClientSend,
				IncomingSendRate = resolved.ServerSend,
				OutgoingSendDelta = resolved.ClientSendDelta,
				IncomingSendDelta = resolved.ServerSendDelta,
				MaxResimulations = -1,
				MinimumInputDelayRaw = 0.0,
				MinimumInputDelayUnit = TimeSyncConfiguration.Unit.Milliseconds,
				ClientTickRate = resolved.Client,
				ClientSimDeltaTime = resolved.ClientTickDelta,
				ServerTickRate = resolved.Server,
				ServerSimDeltaTime = resolved.ServerTickDelta
			};
		}
	}
}
