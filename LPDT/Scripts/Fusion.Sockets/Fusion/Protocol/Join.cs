#define DEBUG
using System;

namespace Fusion.Protocol
{
	internal class Join : Message
	{
		public JoinMessageType Type;

		public PluginGameMode GameMode;

		public PeerMode PeerMode;

		public JoinRequests JoinRequests;

		public byte[] UniqueId;

		public int PlayerRef;

		public byte[] EncryptionKey;

		public byte[] EncryptionKeySecret;

		public int PlayerCounterRef;

		public Join()
		{
		}

		public Join(JoinMessageType type, PluginGameMode mode, PeerMode peerMode, int playerRef = 0, JoinRequests joinRequests = JoinRequests.None, byte[] uniqueID = null, byte[] encryptionKey = null, byte[] encryptionKeySecret = null, int playerCounterRef = 0, ProtocolMessageVersion protocolVersion = ProtocolMessageVersion.V1_7_0, Version serializationVersion = null)
			: base(protocolVersion, serializationVersion)
		{
			if (type == JoinMessageType.Request)
			{
				Assert.Check(playerRef == 0, "playerRef == 0");
			}
			else
			{
				Assert.Check(playerRef > 0, "playerRef > 0");
			}
			Type = type;
			GameMode = mode;
			JoinRequests = joinRequests;
			PeerMode = peerMode;
			UniqueId = uniqueID;
			PlayerRef = playerRef;
			EncryptionKey = encryptionKey;
			EncryptionKeySecret = encryptionKeySecret;
			PlayerCounterRef = playerCounterRef;
		}

		protected override void SerializeProtected(BitStream stream)
		{
			byte value = (byte)Type;
			byte value2 = (byte)GameMode;
			byte value3 = (byte)PeerMode;
			uint value4 = (uint)JoinRequests;
			stream.Serialize(ref value);
			stream.Serialize(ref value2);
			stream.Serialize(ref value3);
			stream.Serialize(ref value4);
			if ((int)ProtocolVersion >= 6)
			{
				stream.Serialize(ref UniqueId);
			}
			if ((int)ProtocolVersion >= 7)
			{
				stream.Serialize(ref PlayerRef);
			}
			if ((int)ProtocolVersion >= 9)
			{
				stream.Serialize(ref EncryptionKey);
				stream.Serialize(ref EncryptionKeySecret);
			}
			if ((int)ProtocolVersion >= 12)
			{
				stream.Serialize(ref PlayerCounterRef);
			}
			Type = (JoinMessageType)value;
			GameMode = (PluginGameMode)value2;
			PeerMode = (PeerMode)value3;
			JoinRequests = (JoinRequests)value4;
		}

		public override string ToString()
		{
			byte[] uniqueId = UniqueId;
			long num = ((uniqueId != null && uniqueId.Length == 8) ? BitConverter.ToInt64(UniqueId) : 0);
			return "[Join: " + string.Format("{0}={1}, ", "Type", Type) + string.Format("{0}={1}, ", "GameMode", GameMode) + string.Format("{0}={1}, ", "PeerMode", PeerMode) + string.Format("{0}={1}, ", "JoinRequests", JoinRequests) + string.Format("{0}={1}, ", "UniqueId", num) + string.Format("{0}={1}, ", "PlayerRef", PlayerRef) + string.Format("{0}={1}, ", "PlayerCounterRef", PlayerCounterRef) + base.ToString() + "]";
		}
	}
}
