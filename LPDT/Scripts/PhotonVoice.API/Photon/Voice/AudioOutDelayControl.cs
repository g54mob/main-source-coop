using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace Photon.Voice
{
	public class AudioOutDelayControl
	{
		[Serializable]
		public struct PlayDelayConfig
		{
			public static PlayDelayConfig Default = new PlayDelayConfig
			{
				Low = 200,
				High = 200,
				Max = 1000,
				SpeedUpPerc = 5
			};

			public int Low;

			public int High;

			public int Max;

			public int SpeedUpPerc;

			public int Delay
			{
				get
				{
					return Low;
				}
				set
				{
					Low = value;
					High = value;
					Max = Default.Max;
				}
			}
		}
	}
	public abstract class AudioOutDelayControl<T> : AudioOutDelayControl, IAudioOut<T>
	{
		protected readonly int sizeofT = Marshal.SizeOf(default(T));

		private const int TEMPO_UP_SKIP_GROUP = 6;

		private int frameSamples;

		private int frameSize;

		private int bufferSamples;

		private int bufferSamplesHalf;

		private int frequency;

		private int writeSamplePos;

		private int clearSamplePos;

		private PlayDelayConfig playDelayConfig;

		private int channels;

		private bool started;

		private bool flushed = true;

		private int targetDelaySamples;

		private int upperTargetDelaySamples;

		private int maxDelaySamples;

		private const int NO_PUSH_TIMEOUT_MS = 120;

		private int lastPushTime = Environment.TickCount - 120;

		protected readonly ILogger logger;

		protected readonly string logPrefix;

		private readonly bool debugInfo;

		private readonly bool processInService;

		private T[] zeroFrame;

		private T[] resampledFrame;

		private AudioUtil.TempoUp<T> tempoUp;

		private bool tempoChangeHQ;

		private ConcurrentQueue<T[]> frameQueue = new ConcurrentQueue<T[]>();

		public const int FRAME_POOL_CAPACITY = 50;

		private ArrayPool<T> framePool;

		private bool catchingUp;

		public abstract long OutPos { get; }

		public int Lag
		{
			get
			{
				if (started)
				{
					int num = writeSamplePos - (int)(OutPos % bufferSamples);
					return ((num > bufferSamplesHalf) ? (num - bufferSamples) : ((num < -bufferSamplesHalf) ? (num + bufferSamples) : num)) * 1000 / frequency;
				}
				return 0;
			}
		}

		public bool IsFlushed
		{
			get
			{
				if (started)
				{
					return flushed;
				}
				return true;
			}
		}

		public bool IsPlaying
		{
			get
			{
				if (!IsFlushed)
				{
					return Environment.TickCount - lastPushTime < 120;
				}
				return false;
			}
		}

		public abstract void OutCreate(int frequency, int channels, int bufferSamples);

		public abstract void OutStart();

		public abstract void OutWrite(T[] data, int offsetSamples);

		public bool IsZeroFrame(T[] f)
		{
			return zeroFrame == f;
		}

		public AudioOutDelayControl(bool processInService, PlayDelayConfig playDelayConfig, ILogger logger, string logPrefix, bool debugInfo)
		{
			this.processInService = processInService;
			this.playDelayConfig = playDelayConfig;
			this.logger = logger;
			this.logPrefix = logPrefix;
			this.debugInfo = debugInfo;
		}

		public void Start(int frequency, int channels, int frameSamples)
		{
			this.frequency = frequency;
			this.channels = channels;
			int num = frequency / 20;
			targetDelaySamples = playDelayConfig.Low * frequency / 1000;
			if (targetDelaySamples < num)
			{
				targetDelaySamples = num;
			}
			upperTargetDelaySamples = targetDelaySamples + (playDelayConfig.High - playDelayConfig.Low) * frequency / 1000;
			if (upperTargetDelaySamples < targetDelaySamples + num)
			{
				upperTargetDelaySamples = targetDelaySamples + num;
			}
			maxDelaySamples = upperTargetDelaySamples + (playDelayConfig.Max - playDelayConfig.High) * frequency / 1000;
			bufferSamples = 4 * maxDelaySamples;
			if (bufferSamples < 2 * frequency)
			{
				bufferSamples = 2 * frequency;
			}
			bufferSamplesHalf = bufferSamples / 2;
			this.frameSamples = frameSamples;
			frameSize = frameSamples * channels;
			writeSamplePos = targetDelaySamples;
			if (framePool == null || framePool.Info != frameSize)
			{
				framePool = new ArrayPool<T>(50, "AudioOutDelayControl", frameSize);
			}
			zeroFrame = new T[frameSize];
			resampledFrame = new T[frameSize];
			tempoChangeHQ = false;
			if (!tempoChangeHQ)
			{
				tempoUp = new AudioUtil.TempoUp<T>();
			}
			OutCreate(frequency, channels, bufferSamples);
			OutStart();
			started = true;
			logger.Log(LogLevel.Info, "{0} Start: {1} bs={2} ch={3} f={4} tds={5} utds={6} mds={7} speed={8} tempo={9}", logPrefix, (sizeofT == 2) ? "short" : "float", bufferSamples, channels, frequency, targetDelaySamples, upperTargetDelaySamples, maxDelaySamples, playDelayConfig.SpeedUpPerc, tempoChangeHQ ? "HQ" : "LQ");
		}

		private void processFrame(T[] frame)
		{
			int num = (int)(OutPos % bufferSamples);
			int num2 = writeSamplePos - num;
			int num3 = ((num2 > bufferSamplesHalf) ? (num2 - bufferSamples) : ((num2 < -bufferSamplesHalf) ? (num2 + bufferSamples) : num2));
			if (!flushed)
			{
				if (num3 > maxDelaySamples)
				{
					if (debugInfo)
					{
						logger.Log(LogLevel.Debug, "{0} overrun {1} {2} {3} {4} {5}", logPrefix, upperTargetDelaySamples, num3, num, writeSamplePos, num + targetDelaySamples);
					}
					writeSamplePos = (num + maxDelaySamples) % bufferSamples;
					num3 = maxDelaySamples;
				}
				else if (num3 < 0)
				{
					if (debugInfo)
					{
						logger.Log(LogLevel.Debug, "{0} underrun {1} {2} {3} {4} {5}", logPrefix, upperTargetDelaySamples, num3, num, writeSamplePos, num + targetDelaySamples);
					}
					writeSamplePos = (num + targetDelaySamples) % bufferSamples;
					num3 = targetDelaySamples;
				}
			}
			if (frame == null)
			{
				flushed = true;
				if (debugInfo)
				{
					logger.Log(LogLevel.Debug, "{0} stream flush pause {1} {2} {3} {4} {5}", logPrefix, upperTargetDelaySamples, num3, num, writeSamplePos, num + targetDelaySamples);
				}
				if (catchingUp)
				{
					catchingUp = false;
					if (debugInfo)
					{
						logger.Log(LogLevel.Debug, "{0} stream sync reset {1} {2} {3} {4} {5}", logPrefix, upperTargetDelaySamples, num3, num, writeSamplePos, num + targetDelaySamples);
					}
				}
				return;
			}
			if (flushed)
			{
				writeSamplePos = (num + targetDelaySamples) % bufferSamples;
				num3 = targetDelaySamples;
				flushed = false;
				if (debugInfo)
				{
					logger.Log(LogLevel.Debug, "{0} stream unpause {1} {2} {3} {4} {5}", logPrefix, upperTargetDelaySamples, num3, num, writeSamplePos, num + targetDelaySamples);
				}
			}
			if (num3 > upperTargetDelaySamples && !catchingUp)
			{
				if (!tempoChangeHQ)
				{
					tempoUp.Begin(channels, playDelayConfig.SpeedUpPerc, 6);
				}
				catchingUp = true;
				if (debugInfo)
				{
					logger.Log(LogLevel.Debug, "{0} stream sync started {1} {2} {3} {4} {5}", logPrefix, upperTargetDelaySamples, num3, num, writeSamplePos, num + targetDelaySamples);
				}
			}
			bool flag = false;
			if (num3 <= targetDelaySamples && catchingUp)
			{
				if (!tempoChangeHQ)
				{
					int num4 = tempoUp.End(frame);
					int num5 = frame.Length / channels - num4;
					Buffer.BlockCopy(frame, num4 * channels * sizeofT, resampledFrame, 0, num5 * channels * sizeofT);
					writeResampled(resampledFrame, num5);
					flag = true;
				}
				catchingUp = false;
				if (debugInfo)
				{
					logger.Log(LogLevel.Debug, "{0} stream sync finished {1} {2} {3} {4} {5}", logPrefix, upperTargetDelaySamples, num3, num, writeSamplePos, num + targetDelaySamples);
				}
			}
			if (flag)
			{
				return;
			}
			if (catchingUp)
			{
				if (!tempoChangeHQ)
				{
					int resampledLenSamples = tempoUp.Process(frame, resampledFrame);
					writeResampled(resampledFrame, resampledLenSamples);
				}
			}
			else
			{
				OutWrite(frame, writeSamplePos);
				writeSamplePos = (writeSamplePos + frame.Length / channels) % bufferSamples;
			}
		}

		public void Service()
		{
			if (!started)
			{
				return;
			}
			if (processInService)
			{
				T[] result;
				while (frameQueue.TryDequeue(out result))
				{
					processFrame(result);
					if (result == null)
					{
						break;
					}
					framePool.Free(result, result.Length);
				}
			}
			int num = (int)(OutPos % bufferSamples);
			if (clearSamplePos > num)
			{
				clearSamplePos -= bufferSamples;
			}
			while (clearSamplePos + frameSamples < num)
			{
				int num2 = clearSamplePos % bufferSamples;
				if (num2 < 0)
				{
					num2 += bufferSamples;
				}
				OutWrite(zeroFrame, num2);
				clearSamplePos += frameSamples;
			}
		}

		private int writeResampled(T[] f, int resampledLenSamples)
		{
			int num = (f.Length - resampledLenSamples * channels) * sizeofT;
			if (num > 0)
			{
				Buffer.BlockCopy(zeroFrame, 0, f, resampledLenSamples * channels * sizeofT, num);
			}
			OutWrite(f, writeSamplePos);
			writeSamplePos = (writeSamplePos + resampledLenSamples) % bufferSamples;
			return resampledLenSamples;
		}

		public void Push(T[] frame)
		{
			if (!started || frame.Length == 0)
			{
				return;
			}
			if (frame.Length != frameSize)
			{
				logger.Log(LogLevel.Error, "{0} audio frames are not of size: {1} != {2}", logPrefix, frame.Length, frameSize);
				return;
			}
			if (processInService)
			{
				T[] array = framePool.New();
				Buffer.BlockCopy(frame, 0, array, 0, frame.Length * sizeofT);
				frameQueue.Enqueue(array);
			}
			else
			{
				processFrame(frame);
			}
			lastPushTime = Environment.TickCount;
		}

		public void Flush()
		{
			if (processInService)
			{
				frameQueue.Enqueue(null);
			}
			else
			{
				processFrame(null);
			}
		}

		public virtual void Stop()
		{
			started = false;
		}
	}
}
