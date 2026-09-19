using System;
using Photon.Client;
using Photon.Realtime;

namespace Photon.Voice
{
	public class Realtime5Transport : RealtimeClient, IVoiceTransport, IDisposable
	{
		private class LBCLogger : ILogger
		{
			private Realtime5Transport lbt;

			public LogLevel Level
			{
				get
				{
					if (lbt.LogLevel == Photon.Client.LogLevel.Info)
					{
						return Photon.Voice.LogLevel.Info;
					}
					if (lbt.LogLevel == Photon.Client.LogLevel.Warning)
					{
						return Photon.Voice.LogLevel.Warning;
					}
					if (lbt.LogLevel == Photon.Client.LogLevel.Error)
					{
						return Photon.Voice.LogLevel.Error;
					}
					return Photon.Voice.LogLevel.Trace;
				}
			}

			public LBCLogger(Realtime5Transport lbt)
			{
				this.lbt = lbt;
			}

			public void Log(LogLevel level, string fmt, params object[] args)
			{
				if (Level >= level)
				{
					Photon.Client.LogLevel level2 = Photon.Client.LogLevel.Debug;
					switch (level)
					{
					case Photon.Voice.LogLevel.Info:
						level2 = Photon.Client.LogLevel.Info;
						break;
					case Photon.Voice.LogLevel.Warning:
						level2 = Photon.Client.LogLevel.Warning;
						break;
					case Photon.Voice.LogLevel.Error:
						level2 = Photon.Client.LogLevel.Error;
						break;
					}
					lbt.DebugReturn(level2, string.Format(fmt, args));
				}
			}
		}

		internal const int REMOTE_VOICE_CHANNEL = 0;

		protected VoiceClient voiceClient;

		private PhotonTransportProtocol protocol;

		protected readonly bool cppCompatibilityMode;

		protected readonly ILogger logger;

		public VoiceClient VoiceClient => voiceClient;

		protected virtual byte FrameCode => 202;

		public virtual int GetPayloadFragmentSize(SendFrameParams par)
		{
			int num = 6;
			if (par.TargetPlayers != null)
			{
				num += 3 + par.TargetPlayers.Length;
			}
			return 1114 - num;
		}

		public bool IsChannelJoined(int channelId)
		{
			return base.State == ClientState.Joined;
		}

		public Realtime5Transport(ILogger logger = null, ConnectionProtocol connectionProtocol = ConnectionProtocol.Udp, bool cppCompatibilityMode = false)
			: base(connectionProtocol)
		{
			if (logger == null)
			{
				logger = new LBCLogger(this);
			}
			base.ClientType = ClientAppType.Voice;
			this.cppCompatibilityMode = cppCompatibilityMode;
			base.EventReceived += onEventActionVoiceClient;
			base.StateChanged += onStateChangeVoiceClient;
			voiceClient = new VoiceClient(this, logger);
			if (RealtimePeer.ChannelCount < 4)
			{
				RealtimePeer.ChannelCount = 4;
			}
			protocol = new PhotonTransportProtocol(voiceClient, logger);
			this.logger = logger;
		}

		public new void Service()
		{
			base.Service();
			voiceClient.Service();
		}

		[Obsolete("Use RealtimePeer::OpChangeGroups().")]
		public virtual bool ChangeAudioGroups(byte[] groupsToRemove, byte[] groupsToAdd)
		{
			return OpChangeGroups(groupsToRemove, groupsToAdd);
		}

		private RaiseEventArgs buildEvOptFromTargets(bool targetMe, int[] targetPlayers)
		{
			RaiseEventArgs result = default(RaiseEventArgs);
			if (targetMe)
			{
				if (targetPlayers == null)
				{
					result.Receivers = ReceiverGroup.All;
				}
				else if (targetPlayers.Length == 0)
				{
					result.TargetActors = new int[1] { LocalPlayer.ActorNumber };
				}
				else
				{
					result.TargetActors = new int[targetPlayers.Length + 1];
					Array.Copy(targetPlayers, result.TargetActors, targetPlayers.Length);
					result.TargetActors[targetPlayers.Length] = LocalPlayer.ActorNumber;
				}
			}
			else
			{
				if (result.TargetActors != null && result.TargetActors.Length == 0)
				{
					throw new ArgumentException("Realtime5Transport: no targets specified in Send* method call");
				}
				result.TargetActors = targetPlayers;
			}
			return result;
		}

		public void SendVoiceInfo(LocalVoice voice, int channelId, bool targetMe, int[] targetPlayers)
		{
			object customEventContent = protocol.buildVoicesInfo(voice);
			SendOptions sendOptions = new SendOptions
			{
				DeliveryMode = DeliveryMode.Reliable,
				Channel = (byte)channelId
			};
			RaiseEventArgs raiseEventArgs = buildEvOptFromTargets(targetMe, targetPlayers);
			OpRaiseEvent(202, customEventContent, raiseEventArgs, sendOptions);
		}

		public void SendVoiceRemove(LocalVoice voice, int channelId, bool targetMe, int[] targetPlayers)
		{
			object customEventContent = protocol.buildVoiceRemoveMessage(voice);
			SendOptions sendOptions = new SendOptions
			{
				DeliveryMode = DeliveryMode.Reliable,
				Channel = (byte)channelId
			};
			RaiseEventArgs raiseEventArgs = buildEvOptFromTargets(targetMe, targetPlayers);
			OpRaiseEvent(202, customEventContent, raiseEventArgs, sendOptions);
		}

		protected virtual object buildFrameMessage(byte voiceId, ushort evNumber, byte frNumber, ArraySegment<byte> data, FrameFlags flags)
		{
			return protocol.buildFrameMessage(voiceId, evNumber, frNumber, data, flags);
		}

		public void SendFrame(ArraySegment<byte> data, FrameFlags flags, ushort evNumber, byte frNumber, byte voiceId, int channelId, SendFrameParams par)
		{
			object customEventContent = buildFrameMessage(voiceId, evNumber, frNumber, data, flags);
			SendOptions sendOptions = new SendOptions
			{
				DeliveryMode = (((flags & FrameFlags.Config) != 0) ? DeliveryMode.Reliable : ((!cppCompatibilityMode) ? (par.Reliable ? DeliveryMode.ReliableUnsequenced : DeliveryMode.UnreliableUnsequenced) : (par.Reliable ? DeliveryMode.Reliable : DeliveryMode.Unreliable))),
				Channel = (byte)channelId,
				Encrypt = par.Encrypt
			};
			RaiseEventArgs raiseEventArgs = buildEvOptFromTargets(par.TargetMe, par.TargetPlayers);
			raiseEventArgs.InterestGroup = par.InterestGroup;
			OpRaiseEvent(FrameCode, customEventContent, raiseEventArgs, sendOptions);
			while (RealtimePeer.SendOutgoingCommands())
			{
			}
		}

		public string ChannelIdStr(int channelId)
		{
			return null;
		}

		public string PlayerIdStr(int playerId)
		{
			return null;
		}

		protected virtual void onEventActionVoiceClient(EventData ev)
		{
			if (ev.Code == 202)
			{
				protocol.onVoiceEvent(ev[245], 0, ev.Sender, ev.Sender == LocalPlayer.ActorNumber);
				return;
			}
			switch (ev.Code)
			{
			case byte.MaxValue:
			{
				int sender = ev.Sender;
				if (sender != LocalPlayer.ActorNumber)
				{
					voiceClient.onPlayerJoin(sender);
				}
				break;
			}
			case 254:
			{
				int sender = ev.Sender;
				if (sender == LocalPlayer.ActorNumber)
				{
					voiceClient.onLeaveAllChannels();
				}
				else
				{
					voiceClient.onPlayerLeave(sender);
				}
				break;
			}
			}
		}

		private void onStateChangeVoiceClient(ClientState fromState, ClientState state)
		{
			if (fromState == ClientState.Joined)
			{
				voiceClient.onLeaveAllChannels();
			}
			if (state == ClientState.Joined)
			{
				voiceClient.onJoinAllChannels();
			}
		}

		public void Dispose()
		{
			voiceClient.Dispose();
		}
	}
}
