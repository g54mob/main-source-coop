using System;

namespace Photon.Voice.Unity
{
	public class RemoteVoiceLink
	{
		public readonly VoiceInfo VoiceInfo;

		public readonly int PlayerId;

		public readonly byte VoiceId;

		public readonly int ChannelId;

		private string cached;

		public event Action<FrameOut<float>> FloatFrameDecoded;

		public event Action RemoteVoiceRemoved;

		public RemoteVoiceLink(VoiceInfo info, int playerId, byte voiceId, int channelId, ref RemoteVoiceOptions options)
		{
			VoiceInfo = info;
			PlayerId = playerId;
			VoiceId = voiceId;
			ChannelId = channelId;
			options.SetOutput(OnDecodedFrameFloatAction);
			options.OnRemoteVoiceRemoveAction = OnRemoteVoiceRemoveAction;
		}

		private void OnRemoteVoiceRemoveAction()
		{
			if (this.RemoteVoiceRemoved != null)
			{
				this.RemoteVoiceRemoved();
			}
		}

		private void OnDecodedFrameFloatAction(FrameOut<float> floats)
		{
			if (this.FloatFrameDecoded != null)
			{
				this.FloatFrameDecoded(floats);
			}
		}

		public override string ToString()
		{
			if (string.IsNullOrEmpty(cached))
			{
				cached = $"[p#{PlayerId} v#{VoiceId} c#{ChannelId} i:{{{VoiceInfo}}}]";
			}
			return cached;
		}
	}
}
