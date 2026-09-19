using Fusion.Statistics;

namespace Fusion
{
	internal class ServerTimeProvider : ITimeProvider
	{
		private ServerTimeProviderSettings _settings;

		private double _time;

		internal ServerTimeProvider()
		{
			_settings = ServerTimeProviderSettings.Default();
		}

		internal ServerTimeProvider(ServerTimeProviderSettings settings)
		{
			_settings = settings;
		}

		private void Reset(Tick snapshot)
		{
			_time = (double)(int)snapshot * _settings.SimDeltaTime;
		}

		private void Update(double unscaledDeltaTime)
		{
			_time += unscaledDeltaTime;
		}

		bool ITimeProvider.IsRunning()
		{
			return true;
		}

		void ITimeProvider.Configure(SimulationRuntimeConfig src)
		{
			_settings.SimDeltaTime = src.TickRate.ClientTickDelta;
		}

		void ITimeProvider.Configure(TimeSyncConfiguration tsc)
		{
			tsc.SanityCheck();
			_settings.MinimumInputDelayRaw = tsc.MinimumInputDelay;
			_settings.MinimumInputDelayUnit = tsc.MinimumInputDelayUnit;
		}

		void ITimeProvider.Reset(double roundTripTime, Tick snapshot)
		{
			Reset(snapshot);
		}

		void ITimeProvider.Snap()
		{
		}

		void ITimeProvider.Update(double unscaledDeltaTime)
		{
			Update(unscaledDeltaTime);
		}

		void ITimeProvider.OnSnapshotReceived(double roundTripTime, Tick snapshot, double? snapshotDeltaTime)
		{
		}

		void ITimeProvider.OnFeedbackReceived(Simulation.TimeFeedback feedback)
		{
		}

		void ITimeProvider.ResetFeedback()
		{
		}

		Instant ITimeProvider.Now()
		{
			return new Instant
			{
				Input = _time + _settings.MinimumInputDelay,
				Local = _time,
				Remote = _time
			};
		}

		DebugInstant ITimeProvider.DebugNow()
		{
			return new DebugInstant
			{
				Input = _time + _settings.MinimumInputDelay,
				InputDelay = _settings.MinimumInputDelay,
				TargetInputDelay = _settings.MinimumInputDelay,
				Local = _time,
				Remote = _time,
				InputOffset = 0.0,
				TargetInputOffset = 0.0,
				InterpDelay = 0.0,
				TargetInterpDelay = 0.0
			};
		}

		void ITimeProvider.Log(FusionStatisticsManager stats)
		{
		}

		void ITimeProvider.SetPlayerIndex(int index)
		{
		}

		void ITimeProvider.StartTrace()
		{
		}

		void ITimeProvider.StopTrace()
		{
		}
	}
}
