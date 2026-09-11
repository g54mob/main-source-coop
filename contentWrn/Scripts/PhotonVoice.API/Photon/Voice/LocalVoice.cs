using System;
using System.Collections.Generic;
using System.Linq;

namespace Photon.Voice
{
	public class LocalVoice : IDisposable
	{
		public const int DATA_POOL_CAPACITY = 50;

		private bool transmitEnabled = true;

		private bool debugEchoMode;

		protected int[] targetPlayers_;

		protected VoiceInfo info;

		protected IEncoder encoder;

		internal byte id;

		internal int channelId;

		internal byte evNumber;

		protected VoiceClient voiceClient;

		protected bool threadingEnabled;

		protected ArraySegment<byte> configFrame;

		protected volatile bool disposed;

		protected object disposeLock = new object();

		private const int NO_TRANSMIT_TIMEOUT_MS = 100;

		private int lastTransmitTime = Environment.TickCount - 100;

		private const int FEC_INFO_SIZE = 5;

		private byte[] fecBuffer = new byte[0];

		private FrameFlags fecFlags;

		private byte fecFrameNumber;

		private int fecTotSize;

		private int fecMaxSize;

		private byte fecCnt;

		internal Dictionary<byte, int> eventTimestamps = new Dictionary<byte, int>();

		private SpacingProfile sendSpacingProfile = new SpacingProfile(1000);

		public VoiceInfo Info => info;

		public bool TransmitEnabled
		{
			get
			{
				return transmitEnabled;
			}
			set
			{
				if (transmitEnabled != value)
				{
					if (transmitEnabled && encoder != null && voiceClient.transport.IsChannelJoined(channelId))
					{
						encoder.EndOfStream();
					}
					transmitEnabled = value;
				}
			}
		}

		public bool IsCurrentlyTransmitting => Environment.TickCount - lastTransmitTime < 100;

		public int FramesSent { get; private set; }

		public int FramesSentFragmented { get; private set; }

		public int FramesSentFragments { get; private set; }

		public int FramesSentBytes { get; private set; }

		public bool Reliable { get; set; }

		public bool Encrypt { get; set; }

		public bool Fragment { get; set; }

		public int FEC { get; set; }

		public IServiceable LocalUserServiceable { get; set; }

		[Obsolete("Use InterestGroup.")]
		public byte Group
		{
			get
			{
				return InterestGroup;
			}
			set
			{
				InterestGroup = value;
			}
		}

		public byte InterestGroup { get; set; }

		public bool DebugEchoMode
		{
			get
			{
				return debugEchoMode;
			}
			set
			{
				if (debugEchoMode == value)
				{
					return;
				}
				debugEchoMode = value;
				if (isJoined)
				{
					if (debugEchoMode)
					{
						sendVoiceInfoAndConfigFrame(targetMe: true, new int[0]);
					}
					else
					{
						sendVoiceRemove(targetMe: true, new int[0]);
					}
				}
			}
		}

		public int[] TargetPlayers
		{
			get
			{
				if (targetPlayers_ != null)
				{
					return (int[])targetPlayers_.Clone();
				}
				return null;
			}
			set
			{
				int[] array = ((value == null) ? null : ((int[])value.Clone()));
				if (isJoined)
				{
					if (targetPlayers_ != null && array != null)
					{
						sendVoiceRemove(targetMe: false, targetPlayers_.Except(array).ToArray());
						sendVoiceInfoAndConfigFrame(targetMe: false, array.Except(targetPlayers_).ToArray());
					}
					else if (targetPlayers_ != null || array != null)
					{
						sendVoiceRemove(targetMe: false, targetPlayers_);
						sendVoiceInfoAndConfigFrame(targetMe: false, array);
					}
				}
				targetPlayers_ = array;
			}
		}

		public string SendSpacingProfileDump => sendSpacingProfile.Dump;

		public int SendSpacingProfileMax => sendSpacingProfile.Max;

		public byte ID => id;

		public byte EvNumber => evNumber;

		protected string shortName => "v#" + id + "ch#" + voiceClient.channelStr(channelId);

		public string Name => "Local " + info.Codec.ToString() + " v#" + id + " ch#" + voiceClient.channelStr(channelId);

		public string LogPrefix => "[PV] " + Name;

		protected bool isJoined
		{
			get
			{
				if (voiceClient != null)
				{
					return voiceClient.transport.IsChannelJoined(channelId);
				}
				return false;
			}
		}

		public void SendSpacingProfileStart()
		{
			sendSpacingProfile.Start();
		}

		internal LocalVoice()
		{
		}

		internal LocalVoice(VoiceClient voiceClient, byte id, VoiceInfo voiceInfo, int channelId, VoiceCreateOptions opt)
		{
			info = voiceInfo;
			this.channelId = channelId;
			InterestGroup = opt.InterestGroup;
			TargetPlayers = opt.TargetPlayers;
			DebugEchoMode = opt.DebugEchoMode;
			Reliable = opt.Reliable;
			Encrypt = opt.Encrypt;
			Fragment = opt.Fragment;
			FEC = opt.FEC;
			this.voiceClient = voiceClient;
			threadingEnabled = voiceClient.ThreadingEnabled;
			this.id = id;
			if (opt.Encoder == null)
			{
				string fmt = LogPrefix + ": encoder is null";
				voiceClient.logger.LogError(fmt);
				throw new ArgumentNullException("encoder");
			}
			encoder = opt.Encoder;
			encoder.Output = sendFrame;
		}

		internal virtual void service()
		{
			while (true)
			{
				FrameFlags flags;
				ArraySegment<byte> compressed = encoder.DequeueOutput(out flags);
				if (compressed.Count == 0)
				{
					break;
				}
				sendFrame(compressed, flags);
			}
			if (LocalUserServiceable != null)
			{
				LocalUserServiceable.Service(this);
			}
		}

		protected bool targetExits(bool targetMe, int[] targetPlayers)
		{
			if (!targetMe && targetPlayers != null)
			{
				return targetPlayers.Length != 0;
			}
			return true;
		}

		internal void onJoinChannel()
		{
			sendVoiceInfoAndConfigFrame(DebugEchoMode, targetPlayers_);
		}

		internal void onPlayerJoin(int playerId)
		{
			if (targetPlayers_ == null || targetPlayers_.Contains(playerId))
			{
				sendVoiceInfoAndConfigFrame(targetMe: false, new int[1] { playerId });
			}
			else
			{
				voiceClient.logger.LogInfo(LogPrefix + " player " + playerId + " join is ignored becuase it's not in target players");
			}
		}

		internal void sendVoiceInfoAndConfigFrame()
		{
			sendVoiceInfoAndConfigFrame(DebugEchoMode, targetPlayers_);
		}

		private string getTargetStr(bool targetMe, int[] targetPlayers)
		{
			string text = ((targetPlayers == null) ? "others" : string.Join(", ", targetPlayers));
			if (targetMe)
			{
				text = text + ((text.Length > 0) ? " and " : "") + "me";
			}
			return text;
		}

		protected void sendVoiceInfoAndConfigFrame(bool targetMe, int[] targetPlayers)
		{
			if (targetExits(targetMe, targetPlayers))
			{
				string targetStr = getTargetStr(targetMe, targetPlayers);
				voiceClient.logger.LogInfo(LogPrefix + " Sending voice info to " + targetStr + ": " + info.ToString() + " ev=" + evNumber);
				voiceClient.transport.SendVoiceInfo(this, channelId, targetMe, targetPlayers);
				if (configFrame.Count != 0)
				{
					voiceClient.logger.LogInfo(LogPrefix + " Sending config frame to " + targetStr);
					sendFrame0(configFrame, FrameFlags.Config, targetMe, targetPlayers, 0, reliable: true);
				}
			}
		}

		internal void sendVoiceRemove()
		{
			sendVoiceRemove(DebugEchoMode, targetPlayers_);
		}

		protected void sendVoiceRemove(bool targetMe, int[] targetPlayers)
		{
			if (targetExits(targetMe, targetPlayers))
			{
				voiceClient.logger.LogInfo(LogPrefix + " Sending voice remove to " + getTargetStr(targetMe, targetPlayers));
				voiceClient.transport.SendVoiceRemove(this, channelId, targetMe, targetPlayers);
			}
		}

		internal void sendFrame(ArraySegment<byte> compressed, FrameFlags flags)
		{
			if ((flags & FrameFlags.Config) != 0)
			{
				byte[] array = ((configFrame.Array != null && configFrame.Array.Length >= compressed.Count) ? configFrame.Array : new byte[compressed.Count]);
				Buffer.BlockCopy(compressed.Array, compressed.Offset, array, 0, compressed.Count);
				configFrame = new ArraySegment<byte>(array, 0, compressed.Count);
				voiceClient.logger.LogInfo(LogPrefix + " Got config frame from encoder, " + configFrame.Count + " bytes");
			}
			if (voiceClient.transport.IsChannelJoined(channelId) && TransmitEnabled)
			{
				sendFrame0(compressed, flags, DebugEchoMode, targetPlayers_, InterestGroup, Reliable);
			}
		}

		internal void sendFrame0(ArraySegment<byte> compressed, FrameFlags flags, bool targetMe, int[] targetPlayers, byte interestGroup, bool reliable)
		{
			if (!targetExits(targetMe, targetPlayers))
			{
				return;
			}
			bool num = Fragment && (flags & FrameFlags.Config) == 0;
			_ = flags & FrameFlags.EndOfStream;
			SendFrameParams sendFrameParams = new SendFrameParams(targetMe, targetPlayers, interestGroup, reliable, Encrypt);
			int num2 = (num ? voiceClient.transport.GetPayloadFragmentSize(sendFrameParams) : 0);
			if (num2 <= 0 || compressed.Count <= num2)
			{
				sendFrameEvent(compressed, flags, sendFrameParams);
			}
			else
			{
				byte b = (byte)((compressed.Count + 1 + num2 - 1) / num2);
				for (byte b2 = 0; b2 < b; b2++)
				{
					bool flag = b2 == b - 1;
					FrameFlags frameFlags = flags;
					if (b2 > 0)
					{
						frameFlags |= FrameFlags.FragNotBeg;
					}
					if (!flag)
					{
						frameFlags |= FrameFlags.FragNotEnd;
					}
					byte b3 = 0;
					int count;
					if (b2 != 0)
					{
						count = ((!flag) ? num2 : (compressed.Count % num2));
					}
					else
					{
						b3 = compressed.Array[compressed.Offset + num2];
						compressed.Array[compressed.Offset + num2] = b;
						count = num2 + 1;
					}
					sendFrameEvent(new ArraySegment<byte>(compressed.Array, compressed.Offset + b2 * num2, count), frameFlags, sendFrameParams);
					if (b2 == 0)
					{
						compressed.Array[compressed.Offset + num2] = b3;
					}
					FramesSentFragments++;
				}
				voiceClient.logger.LogDebug(LogPrefix + " ev#" + evNumber + " fr#" + FramesSent + " c#" + b + " Fragmented sent from events " + (byte)(evNumber - b) + "-" + evNumber + ", size: " + compressed.Count + ", flags: " + flags);
				FramesSentFragmented++;
			}
			FramesSent++;
			FramesSentBytes += compressed.Count;
			if (compressed.Count > 0 && (flags & FrameFlags.Config) == 0)
			{
				lastTransmitTime = Environment.TickCount;
			}
		}

		private void resetFEC()
		{
			Array.Clear(fecBuffer, 0, fecMaxSize + 5);
			fecFlags = (FrameFlags)0;
			fecFrameNumber = 0;
			fecTotSize = 0;
			fecMaxSize = 0;
			fecCnt = 0;
		}

		private void sendFrameEvent(ArraySegment<byte> data, FrameFlags flags, SendFrameParams sendFramePar)
		{
			int fEC = FEC;
			byte b = 0;
			int num = -1;
			voiceClient.transport.SendFrame(data, flags, evNumber, (byte)FramesSent, id, channelId, sendFramePar);
			if (num >= 0)
			{
				data.Array[num] = b;
			}
			sendSpacingProfile.Update(lost: false, flush: false);
			if (DebugEchoMode)
			{
				eventTimestamps[evNumber] = Environment.TickCount;
			}
			evNumber++;
			if (fEC > 0)
			{
				if (fecBuffer.Length < data.Count + 5)
				{
					byte[] sourceArray = fecBuffer;
					fecBuffer = new byte[data.Count + 5];
					Array.Copy(sourceArray, fecBuffer, fecMaxSize);
				}
				for (int i = 0; i < data.Count; i++)
				{
					fecBuffer[i] ^= data.Array[data.Offset + i];
				}
				fecMaxSize = ((fecMaxSize < data.Count) ? data.Count : fecMaxSize);
				fecFlags ^= flags;
				fecFrameNumber ^= (byte)FramesSent;
				fecTotSize += data.Count;
				fecCnt++;
				if (fecCnt >= fEC)
				{
					fecBuffer[fecMaxSize] = fecFrameNumber;
					fecBuffer[fecMaxSize + 1] = (byte)fecFlags;
					fecBuffer[fecMaxSize + 2] = (byte)fecTotSize;
					fecBuffer[fecMaxSize + 3] = (byte)(fecTotSize >> 8);
					fecBuffer[fecMaxSize + 4] = (byte)(evNumber - fecCnt);
					voiceClient.transport.SendFrame(new ArraySegment<byte>(fecBuffer, 0, fecMaxSize + 5), FrameFlags.FEC, evNumber, evNumber, id, channelId, sendFramePar);
					resetFEC();
				}
			}
		}

		public void RemoveSelf()
		{
			if (voiceClient != null)
			{
				voiceClient.RemoveLocalVoice(this);
			}
		}

		public virtual void Dispose()
		{
			if (!disposed)
			{
				if (encoder != null)
				{
					encoder.Dispose();
				}
				disposed = true;
			}
		}
	}
}
