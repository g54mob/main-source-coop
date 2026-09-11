using System;
using System.Collections.Generic;

namespace Photon.Voice
{
	internal class PhotonTransportProtocol
	{
		private enum EventSubcode : byte
		{
			VoiceInfo = 1,
			VoiceRemove = 2,
			Frame = 3
		}

		private enum EventParam : byte
		{
			VoiceId = 1,
			SamplingRate = 2,
			Channels = 3,
			FrameDurationUs = 4,
			Bitrate = 5,
			Width = 6,
			Height = 7,
			FPS = 8,
			KeyFrameInt = 9,
			UserData = 10,
			EventNumber = 11,
			Codec = 12
		}

		private VoiceClient voiceClient;

		private ILogger logger;

		public PhotonTransportProtocol(VoiceClient voiceClient, ILogger logger)
		{
			this.voiceClient = voiceClient;
			this.logger = logger;
		}

		internal object[] buildVoicesInfo(LocalVoice v)
		{
			object[] array = new object[1];
			object[] result = new object[3]
			{
				(byte)0,
				EventSubcode.VoiceInfo,
				array
			};
			array[0] = new Dictionary<byte, object>
			{
				{ 1, v.ID },
				{
					12,
					v.Info.Codec
				},
				{
					2,
					v.Info.SamplingRate
				},
				{
					3,
					v.Info.Channels
				},
				{
					4,
					v.Info.FrameDurationUs
				},
				{
					5,
					v.Info.Bitrate
				},
				{
					6,
					v.Info.Width
				},
				{
					7,
					v.Info.Height
				},
				{
					8,
					v.Info.FPS
				},
				{
					9,
					v.Info.KeyFrameInt
				},
				{
					10,
					v.Info.UserData
				},
				{ 11, v.EvNumber }
			};
			return result;
		}

		internal object[] buildVoiceRemoveMessage(LocalVoice v)
		{
			byte[] array = new byte[1] { v.ID };
			return new object[3]
			{
				(byte)0,
				EventSubcode.VoiceRemove,
				array
			};
		}

		internal object[] buildFrameMessage(byte voiceId, byte evNumber, byte frNumber, ArraySegment<byte> data, FrameFlags flags)
		{
			if (evNumber == frNumber)
			{
				return new object[4]
				{
					voiceId,
					evNumber,
					data,
					(byte)flags
				};
			}
			return new object[5]
			{
				voiceId,
				evNumber,
				data,
				(byte)flags,
				frNumber
			};
		}

		internal void onVoiceEvent(object content0, int channelId, int playerId, bool isLocalPlayer)
		{
			object[] array = (object[])content0;
			if ((byte)array[0] == 0)
			{
				switch ((byte)array[1])
				{
				case 1:
					onVoiceInfo(channelId, playerId, array[2]);
					break;
				case 2:
					onVoiceRemove(channelId, playerId, array[2]);
					break;
				default:
					logger.LogError("[PV] Unknown sevent subcode " + array[1]);
					break;
				}
				return;
			}
			byte voiceId = (byte)array[0];
			byte b = (byte)array[1];
			byte[] array2 = (byte[])array[2];
			FrameFlags flags = (FrameFlags)0;
			if (array.Length > 3)
			{
				flags = (FrameFlags)array[3];
			}
			byte frameNum = b;
			if (array.Length > 4)
			{
				frameNum = (byte)array[4];
			}
			FrameBuffer receivedBytes = new FrameBuffer(array2, flags, frameNum);
			voiceClient.onFrame(playerId, voiceId, b, ref receivedBytes, isLocalPlayer);
			receivedBytes.Release();
		}

		private void onVoiceInfo(int channelId, int playerId, object payload)
		{
			object[] array = (object[])payload;
			for (int i = 0; i < array.Length; i++)
			{
				Dictionary<byte, object> dictionary = (Dictionary<byte, object>)array[i];
				byte voiceId = (byte)dictionary[1];
				byte eventNumber = (byte)dictionary[11];
				VoiceInfo info = createVoiceInfoFromEventPayload(dictionary);
				voiceClient.onVoiceInfo(channelId, playerId, voiceId, eventNumber, info);
			}
		}

		private void onVoiceRemove(int channelId, int playerId, object payload)
		{
			byte[] voiceIds = (byte[])payload;
			voiceClient.onVoiceRemove(playerId, voiceIds);
		}

		private VoiceInfo createVoiceInfoFromEventPayload(Dictionary<byte, object> h)
		{
			VoiceInfo result = new VoiceInfo
			{
				Codec = (Codec)h[12],
				SamplingRate = (int)h[2],
				Channels = (int)h[3],
				FrameDurationUs = (int)h[4],
				Bitrate = (int)h[5]
			};
			if (h.ContainsKey(6))
			{
				result.Width = (int)h[6];
			}
			if (h.ContainsKey(7))
			{
				result.Height = (int)h[7];
			}
			if (h.ContainsKey(8))
			{
				result.FPS = (int)h[8];
			}
			if (h.ContainsKey(9))
			{
				result.KeyFrameInt = (int)h[9];
			}
			result.UserData = h[10];
			return result;
		}
	}
}
