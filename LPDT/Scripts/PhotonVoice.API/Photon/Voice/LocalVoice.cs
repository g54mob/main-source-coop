using System;
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

		private int EV_BUF_SIZE = 256;

		internal ushort evNumber;

		protected VoiceClient voiceClient;

		protected bool threadingEnabled;

		protected ArraySegment<byte> configFrame;

		protected volatile bool disposed;

		protected object disposeLock = new object();

		private const int NO_TRANSMIT_TIMEOUT_MS = 100;

		private int lastTransmitTime = Environment.TickCount - 100;

		private const int FRAME_PART_SIZE = int.MaxValue;

		private byte[] fecBuffer = new byte[0];

		private FrameFlags fecFlags;

		private byte fecFrameNumber;

		private int fecTotSize;

		private int fecMaxSize;

		private ushort fecCnt;

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

		public int EventBufferSize => EV_BUF_SIZE;

		protected string shortName { get; }

		public string Name { get; }

		public string LogPrefix { get; }

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
			EV_BUF_SIZE = ((opt.EventBufSize == 0) ? 256 : opt.EventBufSize);
			this.voiceClient = voiceClient;
			threadingEnabled = voiceClient.ThreadingEnabled;
			this.id = id;
			shortName = "v#" + id + "ch#" + voiceClient.channelStr(channelId);
			Name = "Local " + info.Codec.ToString() + " v#" + id + " ch#" + voiceClient.channelStr(channelId);
			LogPrefix = "[PV] " + Name;
			if (opt.Encoder == null)
			{
				string fmt = LogPrefix + ": encoder is null";
				voiceClient.logger.Log(LogLevel.Error, fmt);
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

		internal void onLeaveChannel()
		{
			sendVoiceRemove(DebugEchoMode, targetPlayers_);
		}

		internal void onPlayerJoin(int playerId)
		{
			if (targetPlayers_ == null || targetPlayers_.Contains(playerId))
			{
				sendVoiceInfoAndConfigFrame(targetMe: false, new int[1] { playerId });
			}
			else
			{
				voiceClient.logger.Log(LogLevel.Info, LogPrefix + " player " + playerId + " join is ignored becuase it's not in target players");
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
				voiceClient.logger.Log(LogLevel.Info, LogPrefix + " Sending voice info to " + targetStr + ": " + info.ToString() + " ev=" + evNumber);
				voiceClient.transport.SendVoiceInfo(this, channelId, targetMe, targetPlayers);
				if (configFrame.Count != 0)
				{
					voiceClient.logger.Log(LogLevel.Info, LogPrefix + " Sending config frame to " + targetStr);
					sendFrameParts(configFrame, FrameFlags.Config, targetMe, targetPlayers, 0, reliable: true);
				}
			}
		}

		protected void sendVoiceRemove(bool targetMe, int[] targetPlayers)
		{
			if (targetExits(targetMe, targetPlayers))
			{
				voiceClient.logger.Log(LogLevel.Info, LogPrefix + " Sending voice remove to " + getTargetStr(targetMe, targetPlayers));
				voiceClient.transport.SendVoiceRemove(this, channelId, targetMe, targetPlayers);
			}
		}

		internal void sendFrame(ArraySegment<byte> compressed, FrameFlags flags)
		{
			if ((flags & FrameFlags.Config) != 0)
			{
				if (configFrame != null)
				{
					if (configFrame.SequenceEqual(compressed))
					{
						if (voiceClient.logger.Level >= LogLevel.Trace)
						{
							voiceClient.logger.Log(LogLevel.Trace, LogPrefix + " Got config frame from encoder, " + configFrame.Count + " bytes: repeated, not sending");
						}
						return;
					}
					byte[] array = ((configFrame.Array != null && configFrame.Array.Length >= compressed.Count) ? configFrame.Array : new byte[compressed.Count]);
					configFrame = new ArraySegment<byte>(array, 0, compressed.Count);
					voiceClient.logger.Log(LogLevel.Info, LogPrefix + " Got config frame from encoder, " + configFrame.Count + " bytes: updated, sending");
				}
				else
				{
					configFrame = new ArraySegment<byte>(new byte[compressed.Count]);
					voiceClient.logger.Log(LogLevel.Info, LogPrefix + " Got config frame from encoder, " + configFrame.Count + " bytes: initial, sending");
				}
				Buffer.BlockCopy(compressed.Array, compressed.Offset, configFrame.Array, 0, compressed.Count);
			}
			if (voiceClient.transport.IsChannelJoined(channelId) && TransmitEnabled)
			{
				sendFrameParts(compressed, flags, DebugEchoMode, targetPlayers_, InterestGroup, Reliable);
			}
		}

		internal void sendFrameParts(ArraySegment<byte> compressed, FrameFlags flags, bool targetMe, int[] targetPlayers, byte interestGroup, bool reliable)
		{
			if (compressed.Count <= int.MaxValue)
			{
				sendFrame0(compressed, flags, targetMe, targetPlayers, interestGroup, reliable);
				return;
			}
			if (voiceClient.logger.Level >= LogLevel.Trace)
			{
				voiceClient.logger.Log(LogLevel.Trace, LogPrefix + " ev#" + evNumber + " fr#" + FramesSent + " parts: " + (compressed.Count + int.MaxValue - 1) / int.MaxValue);
			}
			for (int i = 0; i < compressed.Count; i += int.MaxValue)
			{
				FrameFlags frameFlags = flags;
				if (i != 0)
				{
					frameFlags |= FrameFlags.PartNotBeg;
				}
				if (i + int.MaxValue < compressed.Count)
				{
					frameFlags |= FrameFlags.PartNotEnd;
				}
				ArraySegment<byte> compressed2 = new ArraySegment<byte>(compressed.Array, compressed.Offset + i, Math.Min(int.MaxValue, compressed.Count - i));
				sendFrame0(compressed2, frameFlags, targetMe, targetPlayers, interestGroup, reliable);
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
				int num3 = ((EV_BUF_SIZE <= 256) ? 1 : 2);
				num2 -= num3;
				ushort num4 = (ushort)((compressed.Count + num2 - 1) / num2);
				for (ushort num5 = 0; num5 < num4; num5++)
				{
					bool flag = num5 == num4 - 1;
					FrameFlags frameFlags = flags;
					if (num5 > 0)
					{
						frameFlags |= FrameFlags.FragNotBeg;
					}
					if (!flag)
					{
						frameFlags |= FrameFlags.FragNotEnd;
					}
					byte b = 0;
					byte b2 = 0;
					int count;
					if (num5 != 0)
					{
						count = ((!flag) ? num2 : (compressed.Count - num2 * (num4 - 1)));
					}
					else
					{
						b = compressed.Array[compressed.Offset + num2];
						compressed.Array[compressed.Offset + num2] = (byte)num4;
						count = num2 + 1;
						if (EV_BUF_SIZE > 256)
						{
							b2 = compressed.Array[compressed.Offset + num2 + 1];
							compressed.Array[compressed.Offset + num2 + 1] = (byte)(num4 >> 8);
							count = num2 + 2;
						}
					}
					sendFrameEvent(new ArraySegment<byte>(compressed.Array, compressed.Offset + num5 * num2, count), frameFlags, sendFrameParams);
					if (num5 == 0)
					{
						compressed.Array[compressed.Offset + num2] = b;
						if (EV_BUF_SIZE > 256)
						{
							compressed.Array[compressed.Offset + num2 + 1] = b2;
						}
					}
					FramesSentFragments++;
				}
				if (voiceClient.logger.Level >= LogLevel.Trace)
				{
					voiceClient.logger.Log(LogLevel.Trace, LogPrefix + " ev#" + evNumber + " fr#" + FramesSent + " c#" + num4 + " Fragmented sent from events " + (ushort)((evNumber + EV_BUF_SIZE - num4) % EV_BUF_SIZE) + "-" + (ushort)((evNumber + EV_BUF_SIZE - 1) % EV_BUF_SIZE) + ", size: " + compressed.Count + ", flags: " + flags);
				}
				FramesSentFragmented++;
			}
			FramesSent++;
			FramesSentBytes += compressed.Count;
			if (compressed.Count > 0 && (flags & FrameFlags.Config) == 0)
			{
				lastTransmitTime = Environment.TickCount;
			}
		}

		private int FEC_INFO_SIZE()
		{
			if (EV_BUF_SIZE <= 256)
			{
				return 5;
			}
			return 6;
		}

		private void resetFEC()
		{
			Array.Clear(fecBuffer, 0, fecMaxSize + FEC_INFO_SIZE());
			fecFlags = (FrameFlags)0;
			fecFrameNumber = 0;
			fecTotSize = 0;
			fecMaxSize = 0;
			fecCnt = 0;
		}

		private void sendFrameEvent(ArraySegment<byte> data, FrameFlags flags, SendFrameParams sendFramePar)
		{
			int fEC = FEC;
			voiceClient.transport.SendFrame(data, flags, evNumber, (byte)FramesSent, id, channelId, sendFramePar);
			sendSpacingProfile.Update(lost: false, flush: false);
			evNumber = (ushort)((evNumber + 1) % EV_BUF_SIZE);
			if (fEC <= 0)
			{
				return;
			}
			if (fecBuffer.Length < data.Count + FEC_INFO_SIZE())
			{
				byte[] sourceArray = fecBuffer;
				fecBuffer = new byte[data.Count + FEC_INFO_SIZE()];
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
				ushort num = (ushort)((evNumber + EV_BUF_SIZE - fecCnt) % EV_BUF_SIZE);
				fecBuffer[fecMaxSize + 4] = (byte)num;
				if (EV_BUF_SIZE > 256)
				{
					fecBuffer[fecMaxSize + 5] = (byte)(num >> 8);
				}
				voiceClient.transport.SendFrame(new ArraySegment<byte>(fecBuffer, 0, fecMaxSize + FEC_INFO_SIZE()), FrameFlags.FEC, evNumber, (byte)evNumber, id, channelId, sendFramePar);
				resetFEC();
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
