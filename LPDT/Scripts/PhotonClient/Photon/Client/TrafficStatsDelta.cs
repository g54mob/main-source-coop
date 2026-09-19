using System.Text;

namespace Photon.Client
{
	public class TrafficStatsDelta : TrafficStatsBase
	{
		public long DeltaTime;

		public override long RoundtripTime { get; internal set; }

		public override long RoundtripTimeVariance { get; internal set; }

		public override long LastRoundtripTime { get; internal set; }

		public long ReferenceRoundtripTime { get; set; }

		public long ReferenceRoundtripTimeVariance { get; set; }

		public long LaterRoundtripTime { get; set; }

		public long LaterRoundtripTimeVariance { get; set; }

		internal TrafficStatsDelta(TrafficStatsBase reference, TrafficStatsBase now)
		{
			base.BytesIn = now.BytesIn - reference.BytesIn;
			base.BytesOut = now.BytesOut - reference.BytesOut;
			base.PackagesIn = now.PackagesIn - reference.PackagesIn;
			base.PackagesOut = now.PackagesOut - reference.PackagesOut;
			base.UdpFragmentsIn = now.UdpFragmentsIn - reference.UdpFragmentsIn;
			base.UdpFragmentsOut = now.UdpFragmentsOut - reference.UdpFragmentsOut;
			base.UdpUnreliableCommandsSent = now.UdpUnreliableCommandsSent - reference.UdpUnreliableCommandsSent;
			base.UdpReliableCommandsSent = now.UdpReliableCommandsSent - reference.UdpReliableCommandsSent;
			base.UdpReliableCommandsResent = now.UdpReliableCommandsResent - reference.UdpReliableCommandsResent;
			base.UdpReliableCommandsInFlight = now.UdpReliableCommandsInFlight - reference.UdpReliableCommandsInFlight;
			base.DispatchIncomingCommandsCalls = now.DispatchIncomingCommandsCalls - reference.DispatchIncomingCommandsCalls;
			base.SendOutgoingCommandsCalls = now.SendOutgoingCommandsCalls - reference.SendOutgoingCommandsCalls;
			ReferenceRoundtripTime = reference.RoundtripTime;
			ReferenceRoundtripTimeVariance = reference.RoundtripTimeVariance;
			LaterRoundtripTime = now.RoundtripTime;
			LaterRoundtripTimeVariance = now.RoundtripTimeVariance;
			RoundtripTime = now.RoundtripTime - reference.RoundtripTime;
			RoundtripTimeVariance = now.RoundtripTimeVariance - reference.RoundtripTimeVariance;
			LastRoundtripTime = now.LastRoundtripTime - reference.LastRoundtripTime;
		}

		public TrafficStatsDelta(TrafficStatsSnapshot reference, TrafficStatsSnapshot now)
			: this((TrafficStatsBase)reference, (TrafficStatsBase)now)
		{
			DeltaTime = now.SnapshotTimestamp - reference.SnapshotTimestamp;
		}

		public TrafficStatsDelta(TrafficStatsSnapshot reference, TrafficStats now)
			: this((TrafficStatsBase)reference, (TrafficStatsBase)now)
		{
			DeltaTime = now.connectionStopwatch.ElapsedMilliseconds - reference.SnapshotTimestamp;
		}

		public override string ToString()
		{
			return ToString(udpValues: true, rttValues: true, callValues: true);
		}

		public string ToString(bool udpValues = true, bool rttValues = false, bool callValues = false)
		{
			float num = (float)DeltaTime / 1000f;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"Delta elapsed: {num:F3} sec.  Out: {base.BytesOut:N0} bytes -> {base.BytesOut / DeltaTime:N0} kB/sec.  In: {base.BytesIn:N0} bytes -> {base.BytesIn / DeltaTime:N0} kB/sec.\nPackages Out: {base.PackagesOut:N0} In: {base.PackagesIn:N0}");
			if (udpValues)
			{
				if (base.UdpReliableCommandsSent > 0)
				{
					stringBuilder.AppendLine($"Reliable commands out: {base.UdpReliableCommandsSent:N0}  resent: {base.UdpReliableCommandsResent:N0}  in flight: {base.UdpReliableCommandsInFlight:N0}.");
				}
				if (base.UdpUnreliableCommandsSent > 0)
				{
					stringBuilder.AppendLine($"Unreliable commands out: {base.UdpUnreliableCommandsSent:N0}.");
				}
				if (base.UdpFragmentsIn > 0 || base.UdpFragmentsOut > 0)
				{
					stringBuilder.AppendLine($"Fragments out: {base.UdpFragmentsOut:N0}  in: {base.UdpFragmentsIn:N0}.");
				}
			}
			if (rttValues)
			{
				stringBuilder.AppendLine($"RTT/Variance from: {ReferenceRoundtripTime}/{LaterRoundtripTimeVariance}  to: {LaterRoundtripTime}/{LaterRoundtripTimeVariance}");
			}
			if (callValues && (base.DispatchIncomingCommandsCalls > 0 || base.SendOutgoingCommandsCalls > 0))
			{
				stringBuilder.AppendLine($"DispatchIncomingCommands calls: {base.DispatchIncomingCommandsCalls:N0}  SendOutgoingCommands calls: {base.SendOutgoingCommandsCalls:N0}.");
			}
			return stringBuilder.ToString();
		}
	}
}
