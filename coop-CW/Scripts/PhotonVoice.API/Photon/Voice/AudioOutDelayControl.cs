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

		private int playSamplePos;

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

		private PrimitiveArrayPool<T> framePool = new PrimitiveArrayPool<T>(50, "AudioOutDelayControl");

		private bool catchingUp;

		public abstract long OutPos { get; }

		public int Lag
		{
			get
			{
				if (started)
				{
					int num = writeSamplePos - playSamplePos;
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
			if (bufferSamples < frameSamples)
			{
				bufferSamples = frameSamples;
			}
			bufferSamplesHalf = bufferSamples / 2;
			this.frameSamples = frameSamples;
			frameSize = frameSamples * channels;
			writeSamplePos = targetDelaySamples;
			if (framePool.Info != frameSize)
			{
				framePool.Init(frameSize);
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
			logger.LogInfo("{0} Start: {1} bs={2} ch={3} f={4} tds={5} utds={6} mds={7} speed={8} tempo={9}", logPrefix, (sizeofT == 2) ? "short" : "float", bufferSamples, channels, frequency, targetDelaySamples, upperTargetDelaySamples, maxDelaySamples, playDelayConfig.SpeedUpPerc, tempoChangeHQ ? "HQ" : "LQ");
		}

		private void processFrame(T[] frame, int playSamplePos)
		{
			int num = writeSamplePos - playSamplePos;
			int num2 = ((num > bufferSamplesHalf) ? (num - bufferSamples) : ((num < -bufferSamplesHalf) ? (num + bufferSamples) : num));
			if (!flushed)
			{
				if (num2 > maxDelaySamples)
				{
					if (debugInfo)
					{
						logger.LogDebug("{0} overrun {1} {2} {3} {4} {5}", logPrefix, upperTargetDelaySamples, num2, playSamplePos, writeSamplePos, playSamplePos + targetDelaySamples);
					}
					writeSamplePos = (playSamplePos + maxDelaySamples) % bufferSamples;
					num2 = maxDelaySamples;
				}
				else if (num2 < 0)
				{
					if (debugInfo)
					{
						logger.LogDebug("{0} underrun {1} {2} {3} {4} {5}", logPrefix, upperTargetDelaySamples, num2, playSamplePos, writeSamplePos, playSamplePos + targetDelaySamples);
					}
					writeSamplePos = (playSamplePos + targetDelaySamples) % bufferSamples;
					num2 = targetDelaySamples;
				}
			}
			if (frame == null)
			{
				flushed = true;
				if (debugInfo)
				{
					logger.LogDebug("{0} stream flush pause {1} {2} {3} {4} {5}", logPrefix, upperTargetDelaySamples, num2, playSamplePos, writeSamplePos, playSamplePos + targetDelaySamples);
				}
				if (catchingUp)
				{
					catchingUp = false;
					if (debugInfo)
					{
						logger.LogDebug("{0} stream sync reset {1} {2} {3} {4} {5}", logPrefix, upperTargetDelaySamples, num2, playSamplePos, writeSamplePos, playSamplePos + targetDelaySamples);
					}
				}
				return;
			}
			if (flushed)
			{
				writeSamplePos = (playSamplePos + targetDelaySamples) % bufferSamples;
				num2 = targetDelaySamples;
				flushed = false;
				if (debugInfo)
				{
					logger.LogDebug("{0} stream unpause {1} {2} {3} {4} {5}", logPrefix, upperTargetDelaySamples, num2, playSamplePos, writeSamplePos, playSamplePos + targetDelaySamples);
				}
			}
			if (num2 > upperTargetDelaySamples && !catchingUp)
			{
				if (!tempoChangeHQ)
				{
					tempoUp.Begin(channels, playDelayConfig.SpeedUpPerc, 6);
				}
				catchingUp = true;
				if (debugInfo)
				{
					logger.LogDebug("{0} stream sync started {1} {2} {3} {4} {5}", logPrefix, upperTargetDelaySamples, num2, playSamplePos, writeSamplePos, playSamplePos + targetDelaySamples);
				}
			}
			bool flag = false;
			if (num2 <= targetDelaySamples && catchingUp)
			{
				if (!tempoChangeHQ)
				{
					int num3 = tempoUp.End(frame);
					int num4 = frame.Length / channels - num3;
					Buffer.BlockCopy(frame, num3 * channels * sizeofT, resampledFrame, 0, num4 * channels * sizeofT);
					writeResampled(resampledFrame, num4);
					flag = true;
				}
				catchingUp = false;
				if (debugInfo)
				{
					logger.LogDebug("{0} stream sync finished {1} {2} {3} {4} {5}", logPrefix, upperTargetDelaySamples, num2, playSamplePos, writeSamplePos, playSamplePos + targetDelaySamples);
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
			playSamplePos = (int)(OutPos % bufferSamples);
			if (processInService)
			{
				T[] result;
				while (frameQueue.TryDequeue(out result))
				{
					processFrame(result, playSamplePos);
					if (result == null)
					{
						break;
					}
					framePool.Release(result, result.Length);
				}
			}
			if (clearSamplePos > playSamplePos)
			{
				clearSamplePos -= bufferSamples;
			}
			while (clearSamplePos + frameSamples < playSamplePos)
			{
				int num = clearSamplePos % bufferSamples;
				if (num < 0)
				{
					num += bufferSamples;
				}
				OutWrite(zeroFrame, num);
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
				logger.LogError("{0} audio frames are not of size: {1} != {2}", logPrefix, frame.Length, frameSize);
				return;
			}
			if (processInService)
			{
				T[] array = framePool.AcquireOrCreate();
				Buffer.BlockCopy(frame, 0, array, 0, frame.Length * sizeofT);
				frameQueue.Enqueue(array);
			}
			else
			{
				processFrame(frame, playSamplePos);
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
				processFrame(null, playSamplePos);
			}
		}

		public virtual void Stop()
		{
			started = false;
		}
	}
}
