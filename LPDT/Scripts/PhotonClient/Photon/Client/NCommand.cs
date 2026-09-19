using System;

namespace Photon.Client
{
	internal class NCommand : IComparable<NCommand>
	{
		internal const byte Ack2FeatureFlag = 0;

		internal const byte ReliableSendWindowFeatureFlag = 2;

		internal const byte FeatureFlagsLow = 2;

		internal const byte FV_UNRELIABLE = 0;

		internal const byte FV_RELIABLE = 1;

		internal const byte FV_UNRELIABLE_UNSEQUENCED = 2;

		internal const byte FV_RELIBALE_UNSEQUENCED = 3;

		internal const byte CT_NONE = 0;

		internal const byte CT_ACK = 1;

		internal const byte CT_CONNECT = 2;

		internal const byte CT_VERIFYCONNECT = 3;

		internal const byte CT_DISCONNECT = 4;

		internal const byte CT_PING = 5;

		internal const byte CT_SENDRELIABLE = 6;

		internal const byte CT_SENDUNRELIABLE = 7;

		internal const byte CT_SENDFRAGMENT = 8;

		internal const byte CT_SENDUNSEQUENCED = 11;

		internal const byte CT_EG_SERVERTIME = 12;

		internal const byte CT_EG_SEND_UNRELIABLE_PROCESSED = 13;

		internal const byte CT_EG_SEND_RELIABLE_UNSEQUENCED = 14;

		internal const byte CT_EG_SEND_FRAGMENT_UNSEQUENCED = 15;

		internal const byte CT_EG_ACK_UNSEQUENCED = 16;

		internal const byte CT_EG_ACK_2 = 17;

		internal const byte CT_EG_ACK_2_UNSEQUENCED = 18;

		internal const byte CT_EG_ACK_2_NULL = 19;

		internal const int HEADER_UDP_PACK_LENGTH = 12;

		internal const int CmdSizeMinimum = 12;

		internal const int CmdSizeAck = 20;

		internal const int CmdSizeConnect = 44;

		internal const int CmdSizeVerifyConnect = 44;

		internal const int CmdSizeDisconnect = 12;

		internal const int CmdSizePing = 12;

		internal const int CmdSizeReliableHeader = 12;

		internal const int CmdSizeUnreliableHeader = 16;

		internal const int CmdSizeUnsequensedHeader = 16;

		internal const int CmdSizeFragmentHeader = 32;

		internal const int CmdSizeMaxHeader = 36;

		internal byte commandFlags;

		internal byte commandType;

		internal byte commandChannelID;

		internal int reliableSequenceNumber;

		internal int unreliableSequenceNumber;

		internal int unsequencedGroupNumber;

		internal byte reservedByte = 4;

		internal int startSequenceNumber;

		internal int fragmentCount;

		internal int fragmentNumber;

		internal int totalLength;

		internal int fragmentOffset;

		internal int fragmentsRemaining;

		internal int commandSentTime;

		internal byte commandSentCount;

		internal int roundTripTimeout;

		internal int timeoutTime;

		internal int ackReceivedReliableSequenceNumber;

		internal int ackReceivedSentTime;

		internal int TimeOfReceive;

		internal int Size;

		internal StreamBuffer Payload;

		protected internal int SizeOfPayload => (Payload != null) ? Payload.Length : 0;

		protected internal bool IsFlaggedUnsequenced => (commandFlags & 2) > 0;

		protected internal bool IsFlaggedReliable => (commandFlags & 1) > 0 && commandType < 17;

		internal static void CreateAck(byte[] buffer, int offset, NCommand commandToAck, int sentTime)
		{
			buffer[offset++] = (byte)((!commandToAck.IsFlaggedUnsequenced) ? 1 : 16);
			buffer[offset++] = commandToAck.commandChannelID;
			buffer[offset++] = 0;
			buffer[offset++] = 4;
			MessageProtocol.Serialize(20, buffer, ref offset);
			MessageProtocol.Serialize(0, buffer, ref offset);
			MessageProtocol.Serialize(commandToAck.reliableSequenceNumber, buffer, ref offset);
			MessageProtocol.Serialize(sentTime, buffer, ref offset);
		}

		internal static void CreateAck2(byte[] buffer, int offset, byte channelId, int completeSequence, int gapBlock, byte gapBlockOffset, int sentTime, bool isSequenced)
		{
			buffer[offset++] = (byte)(isSequenced ? 17 : 18);
			buffer[offset++] = channelId;
			buffer[offset++] = gapBlockOffset;
			buffer[offset++] = 4;
			MessageProtocol.Serialize(20, buffer, ref offset);
			MessageProtocol.Serialize(gapBlock, buffer, ref offset);
			MessageProtocol.Serialize(completeSequence, buffer, ref offset);
			MessageProtocol.Serialize(sentTime, buffer, ref offset);
		}

		internal void Initialize(EnetPeer peer, byte commandType, StreamBuffer payload, byte channel, ConnectionStateValue connectionStateForDisconnect = ConnectionStateValue.Connected)
		{
			this.commandType = commandType;
			commandFlags = 1;
			commandChannelID = channel;
			Payload = payload;
			Size = 12;
			switch (this.commandType)
			{
			case 2:
			{
				Size = 44;
				byte[] array = new byte[32];
				array[0] = 0;
				array[1] = 0;
				int targetOffset = 2;
				MessageProtocol.Serialize((short)peer.mtu, array, ref targetOffset);
				array[4] = 0;
				array[5] = 2;
				array[6] = 128;
				array[7] = 0;
				array[8] = 0;
				array[9] = 0;
				array[10] = 0;
				array[11] = peer.ChannelCount;
				array[12] = byte.MaxValue;
				array[13] = byte.MaxValue;
				array[22] = 19;
				array[23] = 136;
				array[27] = 2;
				array[31] = 2;
				Payload = new StreamBuffer(array);
				break;
			}
			case 4:
				Size = 12;
				if (connectionStateForDisconnect != ConnectionStateValue.Connected)
				{
					commandFlags = 2;
					reservedByte = (byte)((connectionStateForDisconnect == ConnectionStateValue.Zombie) ? 2 : 4);
				}
				break;
			case 6:
				Size = 12 + payload.Length;
				break;
			case 14:
				Size = 12 + payload.Length;
				commandFlags = 3;
				break;
			case 7:
				Size = 16 + payload.Length;
				commandFlags = 0;
				break;
			case 11:
				Size = 16 + payload.Length;
				commandFlags = 2;
				break;
			case 8:
				Size = 32 + payload.Length;
				break;
			case 15:
				Size = 32 + payload.Length;
				commandFlags = 3;
				break;
			case 3:
			case 5:
			case 9:
			case 10:
			case 12:
			case 13:
				break;
			}
		}

		internal void Initialize(EnetPeer peer, byte[] inBuff, ref int readingOffset, int timeOfReceive)
		{
			commandType = inBuff[readingOffset++];
			commandChannelID = inBuff[readingOffset++];
			commandFlags = inBuff[readingOffset++];
			reservedByte = inBuff[readingOffset++];
			MessageProtocol.Deserialize(out Size, inBuff, ref readingOffset);
			MessageProtocol.Deserialize(out reliableSequenceNumber, inBuff, ref readingOffset);
			int num = 0;
			TimeOfReceive = timeOfReceive;
			switch (commandType)
			{
			case 1:
			case 16:
			case 17:
			case 18:
				MessageProtocol.Deserialize(out ackReceivedReliableSequenceNumber, inBuff, ref readingOffset);
				MessageProtocol.Deserialize(out ackReceivedSentTime, inBuff, ref readingOffset);
				break;
			case 6:
			case 14:
				num = Size - 12;
				break;
			case 7:
				MessageProtocol.Deserialize(out unreliableSequenceNumber, inBuff, ref readingOffset);
				num = Size - 16;
				break;
			case 11:
				MessageProtocol.Deserialize(out unsequencedGroupNumber, inBuff, ref readingOffset);
				num = Size - 16;
				break;
			case 8:
			case 15:
				MessageProtocol.Deserialize(out startSequenceNumber, inBuff, ref readingOffset);
				MessageProtocol.Deserialize(out fragmentCount, inBuff, ref readingOffset);
				MessageProtocol.Deserialize(out fragmentNumber, inBuff, ref readingOffset);
				MessageProtocol.Deserialize(out totalLength, inBuff, ref readingOffset);
				MessageProtocol.Deserialize(out fragmentOffset, inBuff, ref readingOffset);
				num = Size - 32;
				fragmentsRemaining = fragmentCount;
				break;
			case 3:
			{
				MessageProtocol.Deserialize(out short value, inBuff, ref readingOffset);
				MessageProtocol.Deserialize(out short _, inBuff, ref readingOffset);
				MessageProtocol.Deserialize(out short value3, inBuff, ref readingOffset);
				MessageProtocol.Deserialize(out short _, inBuff, ref readingOffset);
				readingOffset += 3;
				byte b = inBuff[readingOffset++];
				MessageProtocol.Deserialize(out short value5, inBuff, ref readingOffset);
				readingOffset += 20;
				if (peer.peerID == -1 || peer.peerID == -2)
				{
					peer.peerID = value;
				}
				peer.ServerFeatureFlags = (ushort)value3;
				if (peer.serverFeatureFlagsAvailable && peer.serverFeatureSyncReliableQueue)
				{
					peer.ServerMaxQueueableReliableCommands = (ushort)value5;
				}
				break;
			}
			default:
				readingOffset += Size - 12;
				break;
			}
			if (num != 0 && num < peer.mtu)
			{
				StreamBuffer streamBuffer = PeerBase.MessageBufferPool.Acquire();
				streamBuffer.Write(inBuff, readingOffset, num);
				Payload = streamBuffer;
				Payload.Position = 0;
				readingOffset += num;
			}
		}

		public void Reset()
		{
			commandFlags = 0;
			commandType = 0;
			commandChannelID = 0;
			reliableSequenceNumber = 0;
			unreliableSequenceNumber = 0;
			unsequencedGroupNumber = 0;
			reservedByte = 4;
			startSequenceNumber = 0;
			fragmentCount = 0;
			fragmentNumber = 0;
			totalLength = 0;
			fragmentOffset = 0;
			fragmentsRemaining = 0;
			commandSentTime = 0;
			commandSentCount = 0;
			roundTripTimeout = 0;
			timeoutTime = 0;
			ackReceivedReliableSequenceNumber = 0;
			ackReceivedSentTime = 0;
			Size = 0;
		}

		internal void SerializeHeader(byte[] buffer, ref int bufferIndex)
		{
			buffer[bufferIndex++] = commandType;
			buffer[bufferIndex++] = commandChannelID;
			buffer[bufferIndex++] = commandFlags;
			buffer[bufferIndex++] = reservedByte;
			MessageProtocol.Serialize(Size, buffer, ref bufferIndex);
			MessageProtocol.Serialize(reliableSequenceNumber, buffer, ref bufferIndex);
			if (commandType == 7)
			{
				MessageProtocol.Serialize(unreliableSequenceNumber, buffer, ref bufferIndex);
			}
			else if (commandType == 11)
			{
				MessageProtocol.Serialize(unsequencedGroupNumber, buffer, ref bufferIndex);
			}
			else if (commandType == 8 || commandType == 15)
			{
				MessageProtocol.Serialize(startSequenceNumber, buffer, ref bufferIndex);
				MessageProtocol.Serialize(fragmentCount, buffer, ref bufferIndex);
				MessageProtocol.Serialize(fragmentNumber, buffer, ref bufferIndex);
				MessageProtocol.Serialize(totalLength, buffer, ref bufferIndex);
				MessageProtocol.Serialize(fragmentOffset, buffer, ref bufferIndex);
			}
		}

		internal byte[] Serialize()
		{
			return Payload.GetBuffer();
		}

		public void FreePayload()
		{
			if (Payload != null)
			{
				PeerBase.MessageBufferPool.Release(Payload);
			}
			Payload = null;
		}

		public int CompareTo(NCommand other)
		{
			if (other == null)
			{
				return 1;
			}
			int num = reliableSequenceNumber - other.reliableSequenceNumber;
			if (IsFlaggedReliable || num != 0)
			{
				return num;
			}
			return unreliableSequenceNumber - other.unreliableSequenceNumber;
		}

		public override string ToString()
		{
			return ToString();
		}

		public string ToString(bool full = false)
		{
			string text = (IsFlaggedUnsequenced ? "u" : "");
			if (unreliableSequenceNumber == 0)
			{
				if (full)
				{
					return $"{text}{reliableSequenceNumber}/{commandChannelID}x{commandSentCount} (CMD {commandType} sent {commandSentTime} timeout {timeoutTime})";
				}
				return $"{text}{reliableSequenceNumber}/{commandChannelID}";
			}
			if (full)
			{
				return $"{text}{reliableSequenceNumber}.{unreliableSequenceNumber}/{commandChannelID}x{commandSentCount} (CMD {commandType} sent {commandSentTime} timeout {timeoutTime})";
			}
			return $"{text}{reliableSequenceNumber}.{unreliableSequenceNumber}/{commandChannelID}";
		}
	}
}
