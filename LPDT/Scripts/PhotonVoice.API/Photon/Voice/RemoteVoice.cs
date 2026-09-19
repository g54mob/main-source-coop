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

		private const int MAX_EV_BUF_SIZE = 2048;

		private int EV_BUF_SIZE;

		internal RemoteVoiceOptions options;

		internal int channelId;

		protected bool threadingEnabled;

		private volatile bool disposed;

		private object disposeLock = new object();

		private volatile int receiving;

		private volatile bool decoding;

		private SpacingProfile receiveSpacingProfile = new SpacingProfile(1000);

		private VoiceClient voiceClient;

		private FrameBuffer[] eventQueue;

		private int[] eventQueueLock;

		private byte frameWritePos;

		private byte frameReadPos;

		private ushort eventReadPos;

		private AutoResetEvent frameQueueReady;

		private int flushingFrameNum = -1;

		private static FrameBuffer nullFrame;

		private ConcurrentQueue<FrameBuffer> configFrameQueue = new ConcurrentQueue<FrameBuffer>();

		private bool started;

		private FragmentedPoolSlot[] fragmentedPool = new FragmentedPoolSlot[10];

		private FrameBuffer[] fecQueue;

		private int[] fecQueueLock;

		private ushort[] fecXoredEvents;

		private const int FEC_EVENT_TIMEOUT_INF = 127;

		private byte fecEventTimeout = 127;

		private int QUEUE_CLEAR_LAG = 64;

		private bool fragmentDetected;

		private int partAssmblPos;

		private byte[] partAssmblBuffer;

		private IDisposable partAssmblDisposer;

		private FragmentedPoolSlot[] partPool = new FragmentedPoolSlot[10];

		internal VoiceInfo Info { get; private set; }

		internal int DelayFrames { get; set; }

		private string shortName { get; }

		public string LogPrefix { get; }

		public string ReceiveSpacingProfileDump => receiveSpacingProfile.Dump;

		public int ReceiveSpacingProfileMax => receiveSpacingProfile.Max;

		internal RemoteVoice(VoiceClient client, RemoteVoiceOptions options, int channelId, int playerId, byte voiceId, VoiceInfo info, int eventBufferSize)
		{
			this.options = options;
			LogPrefix = options.logPrefix;
			voiceClient = client;
			threadingEnabled = voiceClient.ThreadingEnabled;
			this.channelId = channelId;
			Info = info;
			shortName = "v#" + voiceId + "ch#" + voiceClient.channelStr(channelId) + "p#" + playerId;
			if (eventBufferSize > 2048)
			{
				string fmt = LogPrefix + ": eventBufferSize " + eventBufferSize + " exceeds the maximum allowed value " + 2048;
				voiceClient.logger.Log(LogLevel.Error, fmt);
				throw new ArgumentException("encoder");
			}
			if (eventBufferSize < 0)
			{
				string fmt2 = LogPrefix + ": eventBufferSize " + eventBufferSize + " < 0";
				voiceClient.logger.Log(LogLevel.Error, fmt2);
				throw new ArgumentException("encoder");
			}
			EV_BUF_SIZE = ((eventBufferSize == 0) ? 256 : eventBufferSize);
			QUEUE_CLEAR_LAG = Math.Min(QUEUE_CLEAR_LAG, EV_BUF_SIZE);
			eventQueue = new FrameBuffer[EV_BUF_SIZE];
			eventQueueLock = new int[EV_BUF_SIZE];
			fecQueue = new FrameBuffer[EV_BUF_SIZE];
			fecQueueLock = new int[EV_BUF_SIZE];
			fecXoredEvents = new ushort[EV_BUF_SIZE];
			if (this.options.Decoder == null)
			{
				string fmt3 = LogPrefix + ": decoder is null (set it with options Decoder property or SetOutput method in OnRemoteVoiceInfoAction)";
				voiceClient.logger.Log(LogLevel.Error, fmt3);
				disposed = true;
			}
			else if (!threadingEnabled)
			{
				voiceClient.logger.Log(LogLevel.Info, LogPrefix + ": Starting decode singlethreaded");
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

		internal void receiveBytes(ref FrameBuffer receivedBytes, ushort evNumber)
		{
			if (receivedBytes.IsConfig)
			{
				if ((receivedBytes.Flags & FrameFlags.MaskFrag) != 0)
				{
					voiceClient.logger.Log(LogLevel.Error, LogPrefix + " ev#" + evNumber + " fr#" + receivedBytes.FrameNum + " wr#" + frameWritePos + ", flags: " + receivedBytes.Flags.ToString() + ": config frame can't be fragmented");
				}
				else
				{
					while (!configFrameQueue.IsEmpty && configFrameQueue.Count > 10)
					{
						if (configFrameQueue.TryDequeue(out var result))
						{
							result.Release();
						}
					}
					configFrameQueue.Enqueue(receivedBytes);
					receivedBytes.Retain();
				}
			}
			if (!started && !receivedBytes.IsFEC)
			{
				started = true;
				frameReadPos = receivedBytes.FrameNum;
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
				ushort num = receivedBytes.Array[receivedBytes.Offset + receivedBytes.Length - 1];
				if (EV_BUF_SIZE > 256)
				{
					num = (ushort)(receivedBytes.Array[receivedBytes.Offset + receivedBytes.Length - 2] + (num << 8));
				}
				for (ushort num2 = num; num2 != evNumber; num2 = (ushort)((num2 + 1) % EV_BUF_SIZE))
				{
					fecXoredEvents[num2] = evNumber;
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
				byte b = (byte)(receivedBytes.FrameNum - frameWritePos);
				int num3 = ((b > 127) ? (b - 256) : b);
				switch (num3)
				{
				default:
					if (voiceClient.logger.Level >= LogLevel.Trace)
					{
						voiceClient.logger.Log(LogLevel.Trace, LogPrefix + " ev#" + evNumber + " fr#" + receivedBytes.FrameNum + " wr#" + frameWritePos + " frame number discontinuity: " + num3);
					}
					frameReadPos = receivedBytes.FrameNum;
					frameWritePos = receivedBytes.FrameNum;
					eventReadPos = evNumber;
					if (frameQueueReady != null)
					{
						frameQueueReady.Set();
					}
					break;
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
					frameWritePos = receivedBytes.FrameNum;
					if (frameQueueReady != null)
					{
						frameQueueReady.Set();
					}
					break;
				case -10:
				case -9:
				case -8:
				case -7:
				case -6:
				case -5:
				case -4:
				case -3:
				case -2:
				case -1:
					voiceClient.FramesLate++;
					if (voiceClient.logger.Level >= LogLevel.Trace)
					{
						voiceClient.logger.Log(LogLevel.Trace, LogPrefix + " ev#" + evNumber + " fr#" + receivedBytes.FrameNum + " wr#" + frameWritePos + " late: " + -num3 + " r/b " + receivedBytes.Length + ", flags: " + receivedBytes.Flags);
					}
					break;
				case 0:
					break;
				}
				if (!threadingEnabled)
				{
					try
					{
						decodeQueue();
					}
					catch (Exception ex)
					{
						voiceClient.logger.Log(LogLevel.Error, LogPrefix + ": Exception in receiveBytes: " + ex);
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
			byte b = (byte)(frameWritePos - num);
			int num2 = 0;
			while (!disposed && num2++ < 100 && (byte)(b - frameReadPos) < 127)
			{
				FrameBuffer result;
				while (configFrameQueue.TryDequeue(out result))
				{
					decoderInputPartial(ref result);
					result.Release();
				}
				if (flushingFrameNum == frameReadPos)
				{
					flushingFrameNum = -1;
				}
				ushort num3 = eventReadPos;
				byte num4 = frameReadPos;
				eventReadPos = (ushort)((eventReadPos + processFrame(eventReadPos, b)) % EV_BUF_SIZE);
				if (num4 != frameReadPos)
				{
					num2 = 0;
				}
				for (ushort num5 = num3; num5 != eventReadPos; num5 = (ushort)((num5 + 1) % EV_BUF_SIZE))
				{
					ushort num6 = (ushort)((num5 + EV_BUF_SIZE - QUEUE_CLEAR_LAG) % EV_BUF_SIZE);
					while (Interlocked.Exchange(ref eventQueueLock[num6], 1) == 1)
					{
					}
					eventQueue[num6].Release();
					eventQueue[num6] = nullFrame;
					Interlocked.Exchange(ref eventQueueLock[num6], 0);
					while (Interlocked.Exchange(ref fecQueueLock[num6], 1) == 1)
					{
					}
					fecQueue[num6].Release();
					fecQueue[num6] = nullFrame;
					Interlocked.Exchange(ref fecQueueLock[num6], 0);
				}
			}
		}

		private void processLostEvent(ushort lostEvNum, ref FrameBuffer lostEv)
		{
			ushort num = fecXoredEvents[lostEvNum];
			while (Interlocked.Exchange(ref fecQueueLock[num], 1) == 1)
			{
			}
			ref FrameBuffer reference = ref fecQueue[num];
			if (reference.IsFEC)
			{
				if (recoverLostEvent(lostEvNum, ref lostEv, num, ref reference))
				{
					voiceClient.FramesRecovered++;
				}
			}
			else if (voiceClient.logger.Level >= LogLevel.Debug)
			{
				voiceClient.logger.Log(LogLevel.Debug, LogPrefix + " ev#" + lostEvNum + " FEC failed to recover because of non-FEC event in FEC events lookup array at index " + num + " (" + ((reference.Array == null) ? "empty" : ("flags: " + reference.Flags)) + ")");
			}
			Interlocked.Exchange(ref fecQueueLock[num], 0);
		}

		private bool recoverLostEvent(ushort lostEvNum, ref FrameBuffer lostEv, ushort fecEvNum, ref FrameBuffer fecEv)
		{
			voiceClient.FramesTryFEC++;
			int num = fecEv.Offset + fecEv.Length - 1;
			ushort num2 = fecEv.Array[num--];
			if (EV_BUF_SIZE > 256)
			{
				num2 = (ushort)(fecEv.Array[num--] + (num2 << 8));
			}
			int num3 = fecEv.Array[num--] << 8;
			num3 += fecEv.Array[num--];
			FrameFlags frameFlags = (FrameFlags)fecEv.Array[num--];
			byte b = fecEv.Array[num--];
			for (ushort num4 = num2; num4 != fecEvNum; num4 = (ushort)((num4 + 1) % EV_BUF_SIZE))
			{
				if (num4 != lostEvNum)
				{
					while (Interlocked.Exchange(ref eventQueueLock[num4], 1) == 1)
					{
					}
					if (eventQueue[num4].Array == null)
					{
						for (ushort num5 = num2; num5 != (ushort)((num4 + 1) % EV_BUF_SIZE); num5 = (ushort)((num5 + 1) % EV_BUF_SIZE))
						{
							if (num5 != lostEvNum)
							{
								Interlocked.Exchange(ref eventQueueLock[num5], 0);
							}
						}
						if (voiceClient.logger.Level >= LogLevel.Debug)
						{
							voiceClient.logger.Log(LogLevel.Debug, LogPrefix + " ev#" + lostEvNum + " FEC failed to recover from events " + num2 + "-" + fecEvNum + " because at least 2 events are lost");
						}
						return false;
					}
				}
			}
			for (ushort num6 = num2; num6 != fecEvNum; num6 = (ushort)((num6 + 1) % EV_BUF_SIZE))
			{
				if (num6 != lostEvNum)
				{
					FrameBuffer frameBuffer = eventQueue[num6];
					for (int i = 0; i < frameBuffer.Length; i++)
					{
						fecEv.Array[fecEv.Offset + i] ^= frameBuffer.Array[frameBuffer.Offset + i];
					}
					frameFlags ^= frameBuffer.Flags;
					b ^= frameBuffer.FrameNum;
					num3 -= frameBuffer.Length;
					Interlocked.Exchange(ref eventQueueLock[num6], 0);
				}
			}
			if (num3 >= 0 && num3 <= fecEv.Length)
			{
				lostEv = new FrameBuffer(fecEv, fecEv.Offset, num3, frameFlags, b);
				fecEv = nullFrame;
				if (voiceClient.logger.Level >= LogLevel.Trace)
				{
					ILogger logger = voiceClient.logger;
					string[] obj = new string[11]
					{
						LogPrefix,
						" ev#",
						lostEvNum.ToString(),
						" fr#",
						lostEv.FrameNum.ToString(),
						" FEC recovered from events ",
						num2.ToString(),
						"-",
						fecEvNum.ToString(),
						", size: ",
						null
					};
					int num7 = num3;
					obj[10] = num7.ToString();
					logger.Log(LogLevel.Trace, string.Concat(obj));
				}
				return true;
			}
			if (voiceClient.logger.Level >= LogLevel.Debug)
			{
				voiceClient.logger.Log(LogLevel.Debug, LogPrefix + " ev#" + lostEvNum + " FEC failed to recover from FEC event of size " + fecEv.Length + " because of wrong size " + num3);
			}
			return false;
		}

		private ushort processFrame(ushort begEvNum, byte maxFrameReadPos)
		{
			while (Interlocked.Exchange(ref eventQueueLock[begEvNum], 1) == 1)
			{
			}
			ref FrameBuffer reference = ref eventQueue[begEvNum];
			if (reference.Array == null && fecEventTimeout < 127)
			{
				processLostEvent(begEvNum, ref reference);
			}
			if (reference.IsConfig)
			{
				Interlocked.Exchange(ref eventQueueLock[begEvNum], 0);
				frameReadPos++;
				return 1;
			}
			if (reference.Array == null)
			{
				if (voiceClient.logger.Level >= LogLevel.Trace)
				{
					voiceClient.logger.Log(LogLevel.Trace, LogPrefix + " ev#" + begEvNum + " fr#" + reference.FrameNum + " wr#" + frameWritePos + " rd#" + frameReadPos + " lost event");
				}
				Interlocked.Exchange(ref eventQueueLock[begEvNum], 0);
				voiceClient.EventsLost++;
				return 1;
			}
			while (frameReadPos != reference.FrameNum)
			{
				if (voiceClient.logger.Level >= LogLevel.Trace)
				{
					voiceClient.logger.Log(LogLevel.Trace, LogPrefix + " ev#" + begEvNum + " fr#" + reference.FrameNum + " wr#" + frameWritePos + " rd#" + frameReadPos + " missing frame");
				}
				decoderInputPartial(ref nullFrame);
				voiceClient.FramesLost++;
				frameReadPos++;
				if ((byte)(maxFrameReadPos - frameReadPos) >= 127)
				{
					Interlocked.Exchange(ref eventQueueLock[begEvNum], 0);
					return 0;
				}
			}
			switch (reference.Flags & FrameFlags.MaskFrag)
			{
			case FrameFlags.FragNotEnd:
			{
				frameReadPos++;
				fragmentDetected = true;
				bool flag = false;
				ushort num = reference.Array[reference.Offset + reference.Length - 1];
				if (EV_BUF_SIZE > 256)
				{
					num = (ushort)(reference.Array[reference.Offset + reference.Length - 2] + (num << 8));
				}
				if (num == 0)
				{
					voiceClient.logger.Log(LogLevel.Warning, LogPrefix + " ev#" + begEvNum + " fr#" + reference.FrameNum + " c#" + num + " 1st event corrupted: 0 fragments count");
					Interlocked.Exchange(ref eventQueueLock[begEvNum], 0);
					return 1;
				}
				int num2 = reference.Length - 1;
				if (EV_BUF_SIZE > 256)
				{
					num2--;
				}
				int size = num2 * num;
				IDisposable disposer;
				byte[] array = fragmentedPoolGetSlot(fragmentedPool, size, out disposer);
				Array.Copy(reference.Array, reference.Offset, array, 0, num2);
				Interlocked.Exchange(ref eventQueueLock[begEvNum], 0);
				int num3 = num2;
				ushort num4 = (ushort)((begEvNum + 1) % EV_BUF_SIZE);
				for (ushort num5 = 1; num5 != num; num5++)
				{
					voiceClient.FramesReceivedFragments++;
					while (Interlocked.Exchange(ref eventQueueLock[num4], 1) == 1)
					{
					}
					ref FrameBuffer reference2 = ref eventQueue[num4];
					if (reference2.Array == null && fecEventTimeout < 127)
					{
						processLostEvent(num4, ref reference2);
					}
					if (reference2.FrameNum == reference.FrameNum && (reference2.Flags & FrameFlags.FragNotBeg) != 0)
					{
						int num6 = ((reference2.Length < num2) ? reference2.Length : num2);
						Array.Copy(reference2.Array, reference2.Offset, array, num3, num6);
						num3 += num6;
					}
					else
					{
						flag = true;
						Array.Clear(array, num3, num2);
						num3 += num2;
						if (voiceClient.logger.Level >= LogLevel.Trace)
						{
							voiceClient.logger.Log(LogLevel.Trace, LogPrefix + " ev#" + begEvNum + " fr#" + reference.FrameNum + " c#" + num + " Fragmented segment zeroed due to invalid fragment ev#" + num4 + " fr#" + reference2.FrameNum + ", flags:" + reference2.Flags.ToString() + ((reference2.Array == null) ? " NULL" : ""));
						}
					}
					Interlocked.Exchange(ref eventQueueLock[num4], 0);
					num4 = (ushort)((num4 + 1) % EV_BUF_SIZE);
				}
				FrameBuffer buf = new FrameBuffer(array, 0, num3, reference.Flags, reference.FrameNum, disposer);
				voiceClient.FramesReceivedFragmented++;
				if (flag)
				{
					voiceClient.FramesFragPart++;
				}
				if (voiceClient.logger.Level >= LogLevel.Trace)
				{
					voiceClient.logger.Log(LogLevel.Trace, LogPrefix + " DEC ev#" + begEvNum + " fr#" + buf.FrameNum + " c#" + num + " Fragmented assembled from events " + begEvNum + "-" + (ushort)((begEvNum + num - 1) % EV_BUF_SIZE) + ", size: " + num3 + ", flags: " + reference.Flags);
				}
				decoderInputPartial(ref buf);
				buf.Release();
				return num;
			}
			case (FrameFlags)0:
				frameReadPos++;
				decoderInputPartial(ref reference);
				break;
			default:
				voiceClient.EventsLost++;
				break;
			}
			Interlocked.Exchange(ref eventQueueLock[begEvNum], 0);
			return 1;
		}

		private void decoderInputPartial(ref FrameBuffer buf)
		{
			FrameFlags frameFlags = buf.Flags & FrameFlags.MaskPart;
			switch (frameFlags)
			{
			case (FrameFlags)0:
				decoderInput(ref buf);
				return;
			case FrameFlags.PartNotEnd:
				if (partAssmblPos != 0)
				{
					voiceClient.logger.Log(LogLevel.Error, LogPrefix + " fr#" + buf.FrameNum + " first part before previous frame last part");
				}
				partAssmblPos = 0;
				if (partAssmblDisposer != null)
				{
					partAssmblDisposer.Dispose();
				}
				partAssmblBuffer = fragmentedPoolGetSlot(partPool, buf.Length, out partAssmblDisposer);
				break;
			}
			if (partAssmblBuffer == null)
			{
				voiceClient.logger.Log(LogLevel.Error, LogPrefix + " fr#" + buf.FrameNum + " missing first part");
				return;
			}
			if (partAssmblBuffer.Length < partAssmblPos + buf.Length)
			{
				if (partAssmblDisposer != null)
				{
					partAssmblDisposer.Dispose();
				}
				byte[] destinationArray = fragmentedPoolGetSlot(partPool, partAssmblPos + buf.Length, out partAssmblDisposer);
				Array.Copy(partAssmblBuffer, destinationArray, partAssmblPos);
				partAssmblBuffer = destinationArray;
			}
			Array.Copy(buf.Array, buf.Offset, partAssmblBuffer, partAssmblPos, buf.Length);
			partAssmblPos += buf.Length;
			if (frameFlags == FrameFlags.PartNotBeg)
			{
				if (voiceClient.logger.Level >= LogLevel.Trace)
				{
					voiceClient.logger.Log(LogLevel.Trace, LogPrefix + " DEC fr#" + buf.FrameNum + " Partial assembled from parts, size: " + partAssmblPos + ", flags: " + buf.Flags);
				}
				FrameBuffer buf2 = new FrameBuffer(partAssmblBuffer, 0, partAssmblPos, buf.Flags, buf.FrameNum, partAssmblDisposer);
				decoderInput(ref buf2);
				buf2.Release();
				partAssmblPos = 0;
				partAssmblDisposer = null;
				partAssmblBuffer = null;
			}
		}

		private void decoderInput(ref FrameBuffer buf)
		{
			try
			{
				options.Decoder.Input(ref buf);
			}
			catch (Exception ex)
			{
				voiceClient.logger.Log(LogLevel.Error, LogPrefix + " Decoder Exception: " + ex.ToString());
			}
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
			voiceClient.logger.Log(LogLevel.Info, LogPrefix + ": Starting decode thread");
			frameQueueReady = new AutoResetEvent(initialState: false);
			try
			{
				options.Decoder.Open(Info);
				while (!disposed)
				{
					frameQueueReady.WaitOne();
					decodeQueue();
				}
			}
			catch (Exception ex)
			{
				voiceClient.logger.Log(LogLevel.Error, LogPrefix + ": Exception in decode thread: " + ex);
				decoding = false;
				Dispose();
			}
			finally
			{
				voiceClient.logger.Log(LogLevel.Info, LogPrefix + ": Exiting decode thread");
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

		private byte[] fragmentedPoolGetSlot(FragmentedPoolSlot[] pool, int size, out IDisposable disposer)
		{
			int i;
			for (i = 0; i < pool.Length && pool[i] != null && !pool[i].IsFree; i++)
			{
			}
			if (i == pool.Length)
			{
				voiceClient.logger.Log(LogLevel.Error, LogPrefix + " Fragmented/parted pool is full, allocating " + size + " bytes directly");
				disposer = null;
				return new byte[size];
			}
			byte[] array = ((pool[i] != null && pool[i].Buf.Length >= size) ? pool[i].Buf : new byte[size]);
			if (pool[i] == null)
			{
				pool[i] = new FragmentedPoolSlot();
			}
			pool[i].Buf = array;
			disposer = pool[i];
			return array;
		}
	}
}
