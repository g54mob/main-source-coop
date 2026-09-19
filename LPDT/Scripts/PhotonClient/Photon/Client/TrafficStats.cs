using System.Diagnostics;

namespace Photon.Client
{
	public class TrafficStats : TrafficStatsBase
	{
		internal readonly Stopwatch connectionStopwatch;

		public int LastSendOutgoingTimestamp;

		public int LastSendAckTimestamp;

		public int LastSendOutgoingDeltaTime => (int)(connectionStopwatch.ElapsedMilliseconds - LastSendOutgoingTimestamp);

		public int LastSendAckDeltaTime => (int)(connectionStopwatch.ElapsedMilliseconds - LastSendAckTimestamp);

		public int LastReceiveTimestamp { get; internal set; }

		public int LastReceiveDeltaTime => (int)(connectionStopwatch.ElapsedMilliseconds - LastReceiveTimestamp);

		public int LastDispatchTimestamp { get; internal set; }

		public int LastDispatchDeltaTime => (int)(connectionStopwatch.ElapsedMilliseconds - LastDispatchTimestamp);

		public int LongestDeltaBetweenDispatchCalls { get; internal set; }

		public int LastDispatchDuration { get; internal set; }

		public int LongestDeltaBetweenSendOutgoingCalls { get; internal set; }

		public TrafficStats(Stopwatch connectionTimeSw)
		{
			connectionStopwatch = connectionTimeSw;
		}

		public TrafficStatsSnapshot ToSnapshot()
		{
			long timestamp = ((connectionStopwatch == null) ? 0 : connectionStopwatch.ElapsedMilliseconds);
			return new TrafficStatsSnapshot(this, timestamp);
		}

		public TrafficStatsDelta ToDelta(TrafficStatsSnapshot reference)
		{
			return new TrafficStatsDelta(reference, this);
		}

		internal void DispatchIncomingCommandsCalled(int timestamp)
		{
			if (LastDispatchTimestamp != 0)
			{
				int num = timestamp - LastDispatchTimestamp;
				if (num > LongestDeltaBetweenDispatchCalls)
				{
					LongestDeltaBetweenDispatchCalls = num;
				}
			}
			base.DispatchIncomingCommandsCalls++;
			LastDispatchTimestamp = timestamp;
		}

		internal void SendOutgoingCommandsCalled(int timestamp)
		{
			if (LastSendOutgoingTimestamp != 0)
			{
				int num = timestamp - LastSendOutgoingTimestamp;
				if (num > LongestDeltaBetweenSendOutgoingCalls)
				{
					LongestDeltaBetweenSendOutgoingCalls = num;
				}
			}
			base.SendOutgoingCommandsCalls++;
			LastSendOutgoingTimestamp = timestamp;
		}

		public void ResetMaximumCounters()
		{
			LongestDeltaBetweenDispatchCalls = 0;
			LongestDeltaBetweenSendOutgoingCalls = 0;
			LastDispatchTimestamp = 0;
			LastSendOutgoingTimestamp = 0;
		}

		public override string ToString()
		{
			return ToString(extended: true);
		}

		public string ToString(bool extended)
		{
			string text = ((base.UdpFragmentsIn <= 0 && base.UdpFragmentsOut <= 0) ? $"In: {base.BytesIn} bytes, {base.PackagesIn} packages.\nOut: {base.BytesOut} bytes, {base.PackagesOut} packages." : $"In: {base.BytesIn} bytes, {base.PackagesIn} packages, {base.UdpFragmentsIn} fragments.\nOut: {base.BytesOut} bytes, {base.PackagesOut} packages, {base.UdpFragmentsOut} fragments.");
			if (!extended)
			{
				return text;
			}
			return text + "\n" + $"Max time between Send: {LongestDeltaBetweenSendOutgoingCalls}ms, " + $"Dispatch: {LongestDeltaBetweenDispatchCalls}ms.  " + $"Send calls: {base.SendOutgoingCommandsCalls}.  " + $"Dispatch calls: {base.DispatchIncomingCommandsCalls}.";
		}
	}
}
