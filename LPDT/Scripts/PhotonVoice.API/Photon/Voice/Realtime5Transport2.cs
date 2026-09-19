using System;
using Photon.Client;

namespace Photon.Voice
{
	public class Realtime5Transport2 : Realtime5Transport
	{
		private const int MAX_DATA_OFFSET = 6;

		protected override byte FrameCode => 203;

		public Realtime5Transport2(ILogger logger = null, ConnectionProtocol connectionProtocol = ConnectionProtocol.Udp, bool cppCompatibilityMode = false)
			: base(logger, connectionProtocol, cppCompatibilityMode)
		{
			RealtimePeer.UseByteArraySlicePoolForEvents = true;
			RealtimePeer.ReuseEventInstance = true;
		}

		public override int GetPayloadFragmentSize(SendFrameParams par)
		{
			int num = 6;
			if (par.TargetPlayers != null)
			{
				num += 3 + par.TargetPlayers.Length;
			}
			return 1112 - num;
		}

		protected override object buildFrameMessage(byte voiceId, ushort evNumber, byte frNumber, ArraySegment<byte> data, FrameFlags flags)
		{
			ByteArraySlice byteArraySlice = RealtimePeer.ByteArraySlicePool.Acquire(data.Count + 6);
			int num = 1;
			byteArraySlice.Buffer[num++] = voiceId;
			byteArraySlice.Buffer[num++] = (byte)evNumber;
			byteArraySlice.Buffer[num++] = (byte)flags;
			if (evNumber != frNumber)
			{
				byteArraySlice.Buffer[num++] = frNumber;
				if (evNumber >> 8 != 0)
				{
					byteArraySlice.Buffer[num++] = (byte)(evNumber >> 8);
				}
			}
			byteArraySlice.Buffer[0] = (byte)num;
			Buffer.BlockCopy(data.Array, data.Offset, byteArraySlice.Buffer, num, data.Count);
			byteArraySlice.Count = data.Count + num;
			return byteArraySlice;
		}

		protected override void onEventActionVoiceClient(EventData ev)
		{
			if (ev.Code == 203)
			{
				onVoiceFrameEvent(ev[245], 0, ev.Sender, LocalPlayer.ActorNumber);
			}
			else
			{
				base.onEventActionVoiceClient(ev);
			}
		}

		internal void onVoiceFrameEvent(object content0, int channelId, int playerId, int localPlayerId)
		{
			int num = 0;
			ByteArraySlice byteArraySlice = content0 as ByteArraySlice;
			byte[] array;
			int num2;
			if (byteArraySlice != null)
			{
				array = byteArraySlice.Buffer;
				num2 = byteArraySlice.Count;
				num = byteArraySlice.Offset;
			}
			else
			{
				array = content0 as byte[];
				num2 = array.Length;
			}
			if (array == null || num2 < 3)
			{
				logger.Log(Photon.Voice.LogLevel.Error, "[PV] onVoiceFrameEvent did not receive data (readable as byte[]) " + content0);
				return;
			}
			byte b = array[num];
			byte voiceId = array[num + 1];
			ushort num3 = array[num + 2];
			FrameFlags flags = (FrameFlags)0;
			if (b > 3)
			{
				flags = (FrameFlags)array[3];
			}
			byte frameNum = (byte)num3;
			if (b > 4)
			{
				frameNum = array[4];
			}
			if (b > 5)
			{
				num3 += (ushort)(array[5] << 8);
			}
			FrameBuffer receivedBytes = ((byteArraySlice == null) ? new FrameBuffer(array, b, num2 - b, flags, frameNum, null) : new FrameBuffer(byteArraySlice.Buffer, byteArraySlice.Offset + b, num2 - b, flags, frameNum, byteArraySlice));
			voiceClient.onFrame(playerId, voiceId, num3, ref receivedBytes, playerId == localPlayerId);
			receivedBytes.Release();
		}
	}
}
