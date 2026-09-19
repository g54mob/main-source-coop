using System;

namespace Fusion.Protocol
{
	internal class DummyTrafficSync : Message
	{
		internal const int DummySendIntervalMax = 300000;

		internal const int DummySendIntervalMin = 100;

		internal const int DummySizeMax = 128;

		internal const int DummySizeMin = 2;

		public int SendInterval { get; private set; } = 300000;

		public int Size { get; private set; } = 2;

		public override bool IsValid
		{
			get
			{
				int result;
				if (base.IsValid)
				{
					int sendInterval = SendInterval;
					if (sendInterval >= 100 && sendInterval <= 300000)
					{
						sendInterval = Size;
						result = ((sendInterval >= 2 && sendInterval <= 128) ? 1 : 0);
						goto IL_0038;
					}
				}
				result = 0;
				goto IL_0038;
				IL_0038:
				return (byte)result != 0;
			}
		}

		public DummyTrafficSync()
		{
		}

		public DummyTrafficSync(int sendInterval, int size, ProtocolMessageVersion protocolVersion = ProtocolMessageVersion.V1_7_0, Version serializationVersion = null)
			: base(protocolVersion, serializationVersion)
		{
			SendInterval = Math.Clamp(sendInterval, 100, 300000);
			Size = Math.Clamp(size, 2, 128);
		}

		protected override void SerializeProtected(BitStream stream)
		{
			int value = SendInterval;
			int value2 = Size;
			stream.Serialize(ref value);
			stream.Serialize(ref value2);
			SendInterval = Math.Max(Math.Min(value, 300000), 100);
			Size = Math.Max(Math.Min(value2, 128), 2);
		}

		public override string ToString()
		{
			return string.Format("[{0}: {1}={2}, {3}={4}, {5}]", "DummyTrafficSync", "SendInterval", SendInterval, "Size", Size, base.ToString());
		}
	}
}
