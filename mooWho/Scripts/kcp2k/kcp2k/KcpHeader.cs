using System;

namespace kcp2k
{
	public static class KcpHeader
	{
		public static bool ParseReliable(byte value, out KcpHeaderReliable header)
		{
			if (Enum.IsDefined(typeof(KcpHeaderReliable), value))
			{
				header = (KcpHeaderReliable)value;
				return true;
			}
			header = KcpHeaderReliable.Ping;
			return false;
		}

		public static bool ParseUnreliable(byte value, out KcpHeaderUnreliable header)
		{
			if (Enum.IsDefined(typeof(KcpHeaderUnreliable), value))
			{
				header = (KcpHeaderUnreliable)value;
				return true;
			}
			header = KcpHeaderUnreliable.Disconnect;
			return false;
		}
	}
}
