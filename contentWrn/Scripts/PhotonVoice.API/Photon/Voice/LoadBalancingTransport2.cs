using System;
using ExitGames.Client.Photon;

namespace Photon.Voice
{
	public class LoadBalancingTransport2 : LoadBalancingTransport
	{
		private const int MAX_DATA_OFFSET = 5;

		protected override byte FrameCode => 203;

		public LoadBalancingTransport2(ILogger logger = null, ConnectionProtocol connectionProtocol = ConnectionProtocol.Udp, bool cppCompatibilityMode = false)
			: base(logger, connectionProtocol, cppCompatibilityMode)
		{
			base.LoadBalancingPeer.UseByteArraySlicePoolForEvents = true;
			base.LoadBalancingPeer.ReuseEventInstance = true;
		}

		public override int GetPayloadFragmentSize(SendFrameParams par)
		{
			int num = 6;
			if (par.TargetPlayers != null)
			{
				num += 3 + par.TargetPlayers.Length;
			}
			return 1113 - num;
		}

		protected override object buildFrameMessage(byte voiceId, byte evNumber, byte frNumber, ArraySegment<byte> data, FrameFlags flags)
		{
			ByteArraySlice byteArraySlice = base.LoadBalancingPeer.ByteArraySlicePool.Acquire(data.Count + 5);
			int num = 1;
			byteArraySlice.Buffer[num++] = voiceId;
			byteArraySlice.Buffer[num++] = evNumber;
			byteArraySlice.Buffer[num++] = (byte)flags;
			if (evNumber != frNumber)
			{
				byteArraySlice.Buffer[num++] = frNumber;
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
				onVoiceFrameEvent(ev[245], 0, ev.Sender, base.LocalPlayer.ActorNumber);
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
				LogError("[PV] onVoiceFrameEvent did not receive data (readable as byte[]) " + content0);
				return;
			}
			byte b = array[num];
			byte voiceId = array[num + 1];
			byte b2 = array[num + 2];
			FrameFlags flags = (FrameFlags)0;
			if (b > 3)
			{
				flags = (FrameFlags)array[3];
			}
			byte frameNum = b2;
			if (b > 4)
			{
				frameNum = array[4];
			}
			FrameBuffer receivedBytes = ((byteArraySlice == null) ? new FrameBuffer(array, b, num2 - b, flags, frameNum, null) : new FrameBuffer(byteArraySlice.Buffer, byteArraySlice.Offset + b, num2 - b, flags, frameNum, byteArraySlice));
			voiceClient.onFrame(playerId, voiceId, b2, ref receivedBytes, playerId == localPlayerId);
			receivedBytes.Release();
		}
	}
}
