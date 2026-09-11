using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Photon.Voice
{
	internal class RemoteVoice : IDisposable
	{
		private class FragmentedPoolSlot : IDisposable
		{
			private byte[] buf;

			public bool IsFree { get; private set; }

			public byte[] Buf
			{
				get
				{
					return buf;
				}
				set
				{
					buf = value;
					IsFree = false;
				}
			}

			public void Dispose()
			{
				IsFree = true;
			}
		}

		internal RemoteVoiceOptions options;

		internal int channelId;

		private int playerId;

		private byte voiceId;

		protected bool threadingEnabled;

		private volatile bool disposed;

		private object disposeLock = new object();

		private volatile int receiving;

		private volatile bool decoding;

		private SpacingProfile receiveSpacingProfile = new SpacingProfile(1000);

		private VoiceClient voiceClient;

		private FrameBuffer[] eventQueue = new FrameBuffer[256];

		private int[] eventQueueLock = new int[256];

		private byte frameWritePos;

		private byte frameReadPos;

		private byte eventReadPos;

		private AutoResetEvent frameQueueReady;

		private int flushingFrameNum = -1;

		private FrameBuffer nullFrame;

		private ConcurrentQueue<FrameBuffer> configFrameQueue = new ConcurrentQueue<FrameBuffer>();

		private bool started;

		private FragmentedPoolSlot[] fragmentedPool = new FragmentedPoolSlot[10];

		private FrameBuffer[] fecQueue = new FrameBuffer[256];

		private int[] fecQueueLock = new int[256];

		private byte[] fecXoredEvents = new byte[256];

		private const int FEC_EVENT_TIMEOUT_INF = 127;

		private byte fecEventTimeout = 127;

		private const int QUEUE_CLEAR_LAG = 64;

		private bool fragmentDetected;

		internal VoiceInfo Info { get; private set; }

		internal int DelayFrames { get; set; }

		private string shortName => "v#" + voiceId + "ch#" + voiceClient.channelStr(channelId) + "p#" + playerId;

		public string LogPrefix { get; private set; }

		public string ReceiveSpacingProfileDump => receiveSpacingProfile.Dump;

		public int ReceiveSpacingProfileMax => receiveSpacingProfile.Max;

		internal RemoteVoice(VoiceClient client, RemoteVoiceOptions options, int channelId, int playerId, byte voiceId, VoiceInfo info, byte lastEventNumber)
		{
			this.options = options;
			LogPrefix = options.logPrefix;
			voiceClient = client;
			threadingEnabled = voiceClient.ThreadingEnabled;
			this.channelId = channelId;
			this.playerId = playerId;
			this.voiceId = voiceId;
			Info = info;
			if (this.options.Decoder == null)
			{
				string fmt = LogPrefix + ": decoder is null (set it with options Decoder property or SetOutput method in OnRemoteVoiceInfoAction)";
				voiceClient.logger.LogError(fmt);
				disposed = true;
			}
			else if (!threadingEnabled)
			{
				voiceClient.logger.LogInfo(LogPrefix + ": Starting decode singlethreaded");
				options.Decoder.Open(Info);
			}
			else
			{
				Thread thread = new Thread(decodeThread);
				Util.SetThreadName(thread, "[PV] Dec" + shortName);
				thread.Start();
			}
		}

		public void ReceiveSpacingProfileStart()
		{
			receiveSpacingProfile.Start();
		}

		internal void receiveBytes(ref FrameBuffer receivedBytes, byte evNumber)
		{
			if (receivedBytes.IsConfig)
			{
				if ((receivedBytes.Flags & FrameFlags.MaskFrag) != 0)
				{
					voiceClient.logger.LogError(LogPrefix + " ev#" + evNumber + " fr#" + receivedBytes.FrameNum + " wr#" + frameWritePos + ", flags: " + receivedBytes.Flags.ToString() + ": config frame can't be fragmented");
					return;
				}
				while (!configFrameQueue.IsEmpty && configFrameQueue.Count > 10)
				{
					if (configFrameQueue.TryDequeue(out var result))
					{
						result.Release();
					}
				}
				configFrameQueue.Enqueue(receivedBytes);
				receivedBytes.Retain();
				return;
			}
			if (!started && !receivedBytes.IsFEC)
			{
				started = true;
				frameReadPos = (byte)(receivedBytes.FrameNum - 1);
				frameWritePos = receivedBytes.FrameNum;
				eventReadPos = evNumber;
			}
			if (receivedBytes.IsFEC)
			{
				while (Interlocked.Exchange(ref fecQueueLock[evNumber], 1) == 1)
				{
				}
				fecQueue[evNumber].Release();
				fecQueue[evNumber] = receivedBytes;
				Interlocked.Exchange(ref fecQueueLock[evNumber], 0);
				receivedBytes.Retain();
				for (byte b = receivedBytes.Array[receivedBytes.Offset + receivedBytes.Length - 1]; b != evNumber; b++)
				{
					fecXoredEvents[b] = evNumber;
				}
				fecEventTimeout = 0;
			}
			else
			{
				while (Interlocked.Exchange(ref eventQueueLock[evNumber], 1) == 1)
				{
				}
				eventQueue[evNumber].Release();
				eventQueue[evNumber] = receivedBytes;
				Interlocked.Exchange(ref eventQueueLock[evNumber], 0);
				receivedBytes.Retain();
				if ((receivedBytes.Flags & FrameFlags.EndOfStream) != 0)
				{
					flushingFrameNum = evNumber;
				}
				if (fecEventTimeout < 127)
				{
					fecEventTimeout++;
				}
				int num = (byte)(receivedBytes.FrameNum - (frameWritePos + 1));
				if (num > 127)
				{
					voiceClient.FramesLate++;
					voiceClient.logger.LogDebug(LogPrefix + " ev#" + evNumber + " fr#" + receivedBytes.FrameNum + " wr#" + frameWritePos + " late: " + (255 - num) + " r/b " + receivedBytes.Length + ", flags: " + receivedBytes.Flags);
				}
				else
				{
					frameWritePos = receivedBytes.FrameNum;
					if (frameQueueReady != null)
					{
						frameQueueReady.Set();
					}
					if (num != 0)
					{
						voiceClient.FramesMiss += num;
						voiceClient.logger.LogDebug(LogPrefix + " ev#" + evNumber + " fr#" + receivedBytes.FrameNum + " wr#" + frameWritePos + " miss: " + num + " r/b " + receivedBytes.Length + ", flags: " + receivedBytes.Flags);
					}
				}
				if (!threadingEnabled)
				{
					try
					{
						decodeQueue();
					}
					catch (Exception ex)
					{
						voiceClient.logger.LogError(LogPrefix + ": Exception in receiveBytes: " + ex);
						Interlocked.Decrement(ref receiving);
						Dispose();
					}
				}
				receiveSpacingProfile.Update(lost: false, (receivedBytes.Flags & FrameFlags.EndOfStream) != 0);
			}
			Interlocked.Decrement(ref receiving);
		}

		private void decodeQueue()
		{
			int num = 0;
			if (flushingFrameNum < 0)
			{
				num = ((DelayFrames <= 0) ? (fragmentDetected ? 1 : 0) : ((DelayFrames > 127) ? 127 : DelayFrames));
			}
			int num2 = 0;
			while (!disposed && num2++ < 10 && (byte)(frameWritePos - frameReadPos) > num)
			{
				FrameBuffer result;
				while (configFrameQueue.TryDequeue(out result))
				{
					options.Decoder.Input(ref result);
					result.Release();
				}
				if (flushingFrameNum == frameReadPos)
				{
					flushingFrameNum = -1;
				}
				byte num3 = eventReadPos;
				byte num4 = frameReadPos;
				eventReadPos += processFrame(eventReadPos);
				if (num4 != frameReadPos)
				{
					num2 = 0;
				}
				for (byte b = num3; b != eventReadPos; b++)
				{
					byte b2 = (byte)(b - 64);
					while (Interlocked.Exchange(ref eventQueueLock[b2], 1) == 1)
					{
					}
					eventQueue[b2].Release();
					eventQueue[b2] = nullFrame;
					Interlocked.Exchange(ref eventQueueLock[b2], 0);
					while (Interlocked.Exchange(ref fecQueueLock[b2], 1) == 1)
					{
					}
					fecQueue[b2].Release();
					fecQueue[b2] = nullFrame;
					Interlocked.Exchange(ref fecQueueLock[b2], 0);
				}
			}
		}

		private void processLostEvent(byte lostEvNum, ref FrameBuffer lostEv)
		{
			byte b = fecXoredEvents[lostEvNum];
			while (Interlocked.Exchange(ref fecQueueLock[b], 1) == 1)
			{
			}
			ref FrameBuffer reference = ref fecQueue[b];
			if (reference.IsFEC)
			{
				if (recoverLostEvent(lostEvNum, ref lostEv, b, ref reference))
				{
					voiceClient.FramesRecovered++;
				}
			}
			else
			{
				voiceClient.logger.LogDebug(LogPrefix + " ev#" + lostEvNum + " FEC failed to recover because of non-FEC event in FEC events lookup array at index " + b + " (" + ((reference.Array == null) ? "empty" : ("flags: " + reference.Flags)) + ")");
			}
			Interlocked.Exchange(ref fecQueueLock[b], 0);
		}

		private bool recoverLostEvent(byte lostEvNum, ref FrameBuffer lostEv, byte fecEvNum, ref FrameBuffer fecEv)
		{
			voiceClient.FramesTryFEC++;
			int num = fecEv.Offset + fecEv.Length;
			byte b = fecEv.Array[num - 5];
			FrameFlags frameFlags = (FrameFlags)fecEv.Array[num - 4];
			int num2 = fecEv.Array[num - 3] + (fecEv.Array[num - 2] << 8);
			byte b2 = fecEv.Array[num - 1];
			for (byte b3 = b2; b3 != fecEvNum; b3++)
			{
				if (b3 != lostEvNum)
				{
					while (Interlocked.Exchange(ref eventQueueLock[b3], 1) == 1)
					{
					}
					if (eventQueue[b3].Array == null)
					{
						for (byte b4 = b2; b4 != (byte)(b3 + 1); b4++)
						{
							Interlocked.Exchange(ref eventQueueLock[b4], 0);
						}
						voiceClient.logger.LogDebug(LogPrefix + " ev#" + lostEvNum + " FEC failed to recover from events " + b2 + "-" + fecEvNum + " because at least 2 events are lost");
						return false;
					}
				}
			}
			for (byte b5 = b2; b5 != fecEvNum; b5++)
			{
				FrameBuffer frameBuffer = eventQueue[b5];
				for (int i = 0; i < frameBuffer.Length; i++)
				{
					fecEv.Array[fecEv.Offset + i] ^= frameBuffer.Array[frameBuffer.Offset + i];
				}
				frameFlags ^= frameBuffer.Flags;
				b ^= frameBuffer.FrameNum;
				num2 -= frameBuffer.Length;
				Interlocked.Exchange(ref eventQueueLock[b5], 0);
			}
			if (num2 >= 0 && num2 <= fecEv.Length)
			{
				lostEv = new FrameBuffer(fecEv, fecEv.Offset, num2, frameFlags, b);
				fecEv = nullFrame;
				ILogger logger = voiceClient.logger;
				string[] obj = new string[11]
				{
					LogPrefix,
					" ev#",
					lostEvNum.ToString(),
					" fr#",
					lostEv.FrameNum.ToString(),
					" FEC recovered from events ",
					b2.ToString(),
					"-",
					fecEvNum.ToString(),
					", size: ",
					null
				};
				int num3 = num2;
				obj[10] = num3.ToString();
				logger.LogDebug(string.Concat(obj));
				return true;
			}
			voiceClient.logger.LogDebug(LogPrefix + " ev#" + lostEvNum + " FEC failed to recover from FEC event of size " + fecEv.Length + " because of wrong resulting size " + num2);
			return false;
		}

		private byte processFrame(byte begEvNum)
		{
			while (Interlocked.Exchange(ref eventQueueLock[begEvNum], 1) == 1)
			{
			}
			ref FrameBuffer reference = ref eventQueue[begEvNum];
			if (reference.Array == null && fecEventTimeout < 127)
			{
				processLostEvent(begEvNum, ref reference);
			}
			if (reference.Array == null)
			{
				voiceClient.logger.LogDebug(LogPrefix + " ev#" + begEvNum + " fr#" + reference.FrameNum + " wr#" + frameWritePos + " rd#" + frameReadPos + " lost event");
				Interlocked.Exchange(ref eventQueueLock[begEvNum], 0);
				voiceClient.EventsLost++;
				return 1;
			}
			if (frameReadPos != reference.FrameNum)
			{
				frameReadPos++;
				while (frameReadPos != reference.FrameNum)
				{
					voiceClient.logger.LogDebug(LogPrefix + " ev#" + begEvNum + " fr#" + reference.FrameNum + " wr#" + frameWritePos + " rd#" + frameReadPos + " missing frame");
					options.Decoder.Input(ref nullFrame);
					voiceClient.FramesLost++;
					frameReadPos++;
				}
			}
			switch (reference.Flags & FrameFlags.MaskFrag)
			{
			case FrameFlags.FragNotEnd:
			{
				fragmentDetected = true;
				bool flag = false;
				byte b = reference.Array[reference.Offset + reference.Length - 1];
				if (b == 0)
				{
					voiceClient.logger.LogWarning(LogPrefix + " ev#" + begEvNum + " fr#" + reference.FrameNum + " c#" + b + " 1st event corrupted: 0 fragments count");
					Interlocked.Exchange(ref eventQueueLock[begEvNum], 0);
					return 1;
				}
				int num = reference.Length - 1;
				int num2 = num * b;
				int i;
				for (i = 0; i < fragmentedPool.Length && fragmentedPool[i] != null && !fragmentedPool[i].IsFree; i++)
				{
				}
				byte[] array;
				if (i != fragmentedPool.Length)
				{
					array = ((fragmentedPool[i] != null && fragmentedPool[i].Buf.Length >= num2) ? fragmentedPool[i].Buf : new byte[num2]);
				}
				else
				{
					voiceClient.logger.LogError(LogPrefix + " Fragmented pool is full, allocating " + num2 + " bytes directly");
					array = new byte[num2];
				}
				Array.Copy(reference.Array, reference.Offset, array, 0, num);
				Interlocked.Exchange(ref eventQueueLock[begEvNum], 0);
				int num3 = num;
				byte b2 = (byte)(begEvNum + 1);
				for (byte b3 = 1; b3 != b; b3++)
				{
					voiceClient.FramesReceivedFragments++;
					while (Interlocked.Exchange(ref eventQueueLock[b2], 1) == 1)
					{
					}
					ref FrameBuffer reference2 = ref eventQueue[b2];
					if (reference2.Array == null && fecEventTimeout < 127)
					{
						processLostEvent(b2, ref reference2);
					}
					if (reference2.FrameNum == reference.FrameNum && (reference2.Flags & FrameFlags.FragNotBeg) != 0)
					{
						int num4 = ((reference2.Length < num) ? reference2.Length : num);
						Array.Copy(reference2.Array, reference2.Offset, array, num3, num4);
						num3 += num4;
					}
					else
					{
						flag = true;
						Array.Clear(array, num3, num);
						num3 += num;
						voiceClient.logger.LogDebug(LogPrefix + " ev#" + begEvNum + " fr#" + reference.FrameNum + " c#" + b + " Fragmented segment zeroed due to invalid fragment ev#" + b2 + " fr#" + reference2.FrameNum + ", flags:" + reference2.Flags.ToString() + ((reference2.Array == null) ? " NULL" : ""));
					}
					Interlocked.Exchange(ref eventQueueLock[b2], 0);
					b2++;
				}
				IDisposable disposer = null;
				if (i != fragmentedPool.Length)
				{
					if (fragmentedPool[i] == null)
					{
						fragmentedPool[i] = new FragmentedPoolSlot();
					}
					fragmentedPool[i].Buf = array;
					disposer = fragmentedPool[i];
				}
				FrameBuffer buf = new FrameBuffer(array, 0, num3, reference.Flags, reference.FrameNum, disposer);
				voiceClient.FramesReceivedFragmented++;
				if (flag)
				{
					voiceClient.FramesFragPart++;
				}
				voiceClient.logger.LogDebug(LogPrefix + " DEC ev#" + begEvNum + " fr#" + buf.FrameNum + " c#" + b + " Fragmented assembled from events " + begEvNum + "-" + (byte)(begEvNum + b - 1) + ", size: " + num3 + ", flags: " + reference.Flags);
				options.Decoder.Input(ref buf);
				buf.Release();
				return b;
			}
			case (FrameFlags)0:
				options.Decoder.Input(ref reference);
				break;
			default:
				voiceClient.EventsLost++;
				break;
			}
			Interlocked.Exchange(ref eventQueueLock[begEvNum], 0);
			return 1;
		}

		private void decodeThread()
		{
			lock (disposeLock)
			{
				if (disposed)
				{
					return;
				}
				decoding = true;
			}
			voiceClient.logger.LogInfo(LogPrefix + ": Starting decode thread");
			frameQueueReady = new AutoResetEvent(initialState: false);
			try
			{
				options.Decoder.Open(Info);
				while (!disposed)
				{
					decodeQueue();
					frameQueueReady.WaitOne();
				}
			}
			catch (Exception ex)
			{
				voiceClient.logger.LogError(LogPrefix + ": Exception in decode thread: " + ex);
				decoding = false;
				Dispose();
			}
			finally
			{
				voiceClient.logger.LogInfo(LogPrefix + ": Exiting decode thread");
			}
			decoding = false;
		}

		internal void removeAndDispose()
		{
			if (options.OnRemoteVoiceRemoveAction != null)
			{
				options.OnRemoteVoiceRemoveAction();
			}
			Dispose();
		}

		public void Dispose()
		{
			lock (disposeLock)
			{
				if (disposed)
				{
					return;
				}
				disposed = true;
			}
			if (frameQueueReady != null)
			{
				frameQueueReady.Set();
			}
			while (receiving > 0 || decoding)
			{
				Array.Clear(eventQueueLock, 0, eventQueueLock.Length);
				Array.Clear(fecQueueLock, 0, fecQueueLock.Length);
			}
			if (frameQueueReady != null)
			{
				frameQueueReady.Close();
			}
			for (int i = 0; i < eventQueue.Length; i++)
			{
				eventQueue[i].Release();
				eventQueue[i] = nullFrame;
			}
			for (int j = 0; j < fecQueue.Length; j++)
			{
				fecQueue[j].Release();
				fecQueue[j] = nullFrame;
			}
			options.Decoder.Dispose();
		}
	}
}
