using System;
using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Photon.Voice
{
	public class LoadBalancingTransport : LoadBalancingClient, IVoiceTransport, ILogger, IDisposable
	{
		internal const int REMOTE_VOICE_CHANNEL = 0;

		protected VoiceClient voiceClient;

		private PhotonTransportProtocol protocol;

		protected readonly bool cppCompatibilityMode;

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

		public void LogError(string fmt, params object[] args)
		{
			DebugReturn(DebugLevel.ERROR, string.Format(fmt, args));
		}

		public void LogWarning(string fmt, params object[] args)
		{
			DebugReturn(DebugLevel.WARNING, string.Format(fmt, args));
		}

		public void LogInfo(string fmt, params object[] args)
		{
			DebugReturn(DebugLevel.INFO, string.Format(fmt, args));
		}

		public void LogDebug(string fmt, params object[] args)
		{
			DebugReturn(DebugLevel.ALL, string.Format(fmt, args));
		}

		public bool IsChannelJoined(int channelId)
		{
			return base.State == ClientState.Joined;
		}

		public LoadBalancingTransport(ILogger logger = null, ConnectionProtocol connectionProtocol = ConnectionProtocol.Udp, bool cppCompatibilityMode = false)
			: base(connectionProtocol)
		{
			if (logger == null)
			{
				logger = this;
			}
			this.cppCompatibilityMode = cppCompatibilityMode;
			base.EventReceived += onEventActionVoiceClient;
			base.StateChanged += onStateChangeVoiceClient;
			voiceClient = new VoiceClient(this, logger);
			int num = Enum.GetValues(typeof(Codec)).Length + 1;
			if (base.LoadBalancingPeer.ChannelCount < num)
			{
				base.LoadBalancingPeer.ChannelCount = (byte)num;
			}
			protocol = new PhotonTransportProtocol(voiceClient, logger);
		}

		public new void Service()
		{
			base.Service();
			voiceClient.Service();
		}

		[Obsolete("Use LoadBalancingPeer::OpChangeGroups().")]
		public virtual bool ChangeAudioGroups(byte[] groupsToRemove, byte[] groupsToAdd)
		{
			return base.LoadBalancingPeer.OpChangeGroups(groupsToRemove, groupsToAdd);
		}

		private RaiseEventOptions buildEvOptFromTargets(bool targetMe, int[] targetPlayers)
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
			if (targetMe)
			{
				if (targetPlayers == null)
				{
					raiseEventOptions.Receivers = ReceiverGroup.All;
				}
				else if (targetPlayers.Length == 0)
				{
					raiseEventOptions.TargetActors = new int[1] { base.LocalPlayer.ActorNumber };
				}
				else
				{
					raiseEventOptions.TargetActors = new int[targetPlayers.Length + 1];
					Array.Copy(targetPlayers, raiseEventOptions.TargetActors, targetPlayers.Length);
					raiseEventOptions.TargetActors[targetPlayers.Length] = base.LocalPlayer.ActorNumber;
				}
			}
			else
			{
				raiseEventOptions.TargetActors = targetPlayers;
			}
			return raiseEventOptions;
		}

		public void SendVoiceInfo(LocalVoice voice, int channelId, bool targetMe, int[] targetPlayers)
		{
			object customEventContent = protocol.buildVoicesInfo(voice);
			SendOptions sendOptions = new SendOptions
			{
				DeliveryMode = DeliveryMode.Reliable,
				Channel = (byte)channelId
			};
			RaiseEventOptions raiseEventOptions = buildEvOptFromTargets(targetMe, targetPlayers);
			OpRaiseEvent(202, customEventContent, raiseEventOptions, sendOptions);
		}

		public void SendVoiceRemove(LocalVoice voice, int channelId, bool targetMe, int[] targetPlayers)
		{
			object customEventContent = protocol.buildVoiceRemoveMessage(voice);
			SendOptions sendOptions = new SendOptions
			{
				DeliveryMode = DeliveryMode.Reliable,
				Channel = (byte)channelId
			};
			RaiseEventOptions raiseEventOptions = buildEvOptFromTargets(targetMe, targetPlayers);
			OpRaiseEvent(202, customEventContent, raiseEventOptions, sendOptions);
		}

		protected virtual object buildFrameMessage(byte voiceId, byte evNumber, byte frNumber, ArraySegment<byte> data, FrameFlags flags)
		{
			return protocol.buildFrameMessage(voiceId, evNumber, frNumber, data, flags);
		}

		public void SendFrame(ArraySegment<byte> data, FrameFlags flags, byte evNumber, byte frNumber, byte voiceId, int channelId, SendFrameParams par)
		{
			object customEventContent = buildFrameMessage(voiceId, evNumber, frNumber, data, flags);
			SendOptions sendOptions = new SendOptions
			{
				DeliveryMode = (((flags & FrameFlags.Config) != 0) ? DeliveryMode.Reliable : ((!cppCompatibilityMode) ? (par.Reliable ? DeliveryMode.ReliableUnsequenced : DeliveryMode.UnreliableUnsequenced) : (par.Reliable ? DeliveryMode.Reliable : DeliveryMode.Unreliable))),
				Channel = (byte)channelId,
				Encrypt = par.Encrypt
			};
			RaiseEventOptions raiseEventOptions = buildEvOptFromTargets(par.TargetMe, par.TargetPlayers);
			raiseEventOptions.InterestGroup = par.InterestGroup;
			OpRaiseEvent(FrameCode, customEventContent, raiseEventOptions, sendOptions);
			while (base.LoadBalancingPeer.SendOutgoingCommands())
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
				protocol.onVoiceEvent(ev[245], 0, ev.Sender, ev.Sender == base.LocalPlayer.ActorNumber);
				return;
			}
			switch (ev.Code)
			{
			case byte.MaxValue:
			{
				int sender = ev.Sender;
				if (sender != base.LocalPlayer.ActorNumber)
				{
					voiceClient.onPlayerJoin(sender);
				}
				break;
			}
			case 254:
			{
				int sender = ev.Sender;
				if (sender == base.LocalPlayer.ActorNumber)
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
