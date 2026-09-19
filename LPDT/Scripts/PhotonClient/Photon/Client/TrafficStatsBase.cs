namespace Photon.Client
{
	public class TrafficStatsBase
	{
		public long BytesIn { get; internal set; }

		public long BytesOut { get; internal set; }

		public int PackagesIn { get; internal set; }

		public int PackagesOut { get; internal set; }

		public int UdpFragmentsIn { get; internal set; }

		public int UdpFragmentsOut { get; internal set; }

		public int UdpUnreliableCommandsSent { get; internal set; }

		public int UdpReliableCommandsSent { get; internal set; }

		public int UdpReliableCommandsResent { get; internal set; }

		public int UdpReliableCommandsInFlight { get; internal set; }

		public int DispatchIncomingCommandsCalls { get; internal set; }

		public int SendOutgoingCommandsCalls { get; internal set; }

		public virtual long RoundtripTime { get; internal set; }

		public virtual long RoundtripTimeVariance { get; internal set; }

		public virtual long LastRoundtripTime { get; internal set; }

		public TrafficStatsBase()
		{
		}

		public TrafficStatsBase(TrafficStatsBase origin = null)
		{
			BytesIn = origin.BytesIn;
			BytesOut = origin.BytesOut;
			PackagesIn = origin.PackagesIn;
			PackagesOut = origin.PackagesOut;
			UdpFragmentsIn = origin.UdpFragmentsIn;
			UdpFragmentsOut = origin.UdpFragmentsOut;
			UdpUnreliableCommandsSent = origin.UdpUnreliableCommandsSent;
			UdpReliableCommandsSent = origin.UdpReliableCommandsSent;
			UdpReliableCommandsResent = origin.UdpReliableCommandsResent;
			UdpReliableCommandsInFlight = origin.UdpReliableCommandsInFlight;
			DispatchIncomingCommandsCalls = origin.DispatchIncomingCommandsCalls;
			SendOutgoingCommandsCalls = origin.SendOutgoingCommandsCalls;
			RoundtripTime = origin.RoundtripTime;
			RoundtripTimeVariance = origin.RoundtripTimeVariance;
			LastRoundtripTime = origin.LastRoundtripTime;
		}
	}
}
