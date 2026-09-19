using System;
using System.Collections.Generic;
using System.Threading;

namespace Photon.Voice
{
	public class LocalVoiceFramed<T> : LocalVoice
	{
		private Framer<T> framer;

		private int preProcessorsCnt;

		private List<IProcessor<T>> processors = new List<IProcessor<T>>();

		private bool dataEncodeThreadStarted;

		private Queue<T[]> pushDataQueue = new Queue<T[]>();

		private AutoResetEvent pushDataQueueReady = new AutoResetEvent(initialState: false);

		private ObjectFactory<T[], int> bufferFactory;

		private int framesSkippedNextLog;

		private int framesSkipped;

		private bool exitThread;

		private int processNullFramesCnt;

		public int OptimalSourceFrameSize { get; private set; }

		public ObjectFactory<T[], int> BufferFactory => bufferFactory;

		public bool PushDataAsyncReady
		{
			get
			{
				lock (pushDataQueue)
				{
					return pushDataQueue.Count < 49;
				}
			}
		}

		protected T[] processFrame(T[] buf, int p0, int p1)
		{
			for (int i = p0; i < p1; i++)
			{
				buf = processors[i].Process(buf);
				if (buf == null)
				{
					break;
				}
			}
			return buf;
		}

		public void AddPostProcessor(params IProcessor<T>[] processors)
		{
			lock (disposeLock)
			{
				foreach (IProcessor<T> item in processors)
				{
					this.processors.Add(item);
				}
			}
		}

		public void AddPreProcessor(params IProcessor<T>[] processors)
		{
			lock (disposeLock)
			{
				foreach (IProcessor<T> item in processors)
				{
					this.processors.Insert(preProcessorsCnt++, item);
				}
			}
		}

		public void RemoveProcessor(params IProcessor<T>[] processors)
		{
			lock (disposeLock)
			{
				foreach (IProcessor<T> item in processors)
				{
					int num = this.processors.IndexOf(item);
					if (num >= 0)
					{
						if (num < preProcessorsCnt)
						{
							preProcessorsCnt--;
						}
						this.processors.Remove(item);
					}
				}
			}
		}

		public void ClearProcessors()
		{
			lock (disposeLock)
			{
				processors.Clear();
				preProcessorsCnt = 0;
			}
		}

		internal LocalVoiceFramed(VoiceClient voiceClient, byte id, VoiceInfo voiceInfo, int inSampleRate, int channelId, VoiceCreateOptions opt)
			: base(voiceClient, id, voiceInfo, channelId, opt)
		{
			if (voiceInfo.FrameSize == 0)
			{
				throw new Exception(base.LogPrefix + ": non 0 frame size required for framed stream");
			}
			if (inSampleRate <= 0)
			{
				throw new Exception(base.LogPrefix + ": invalid input sample rate " + inSampleRate);
			}
			OptimalSourceFrameSize = voiceInfo.FrameSize;
			if (voiceInfo.SamplingRate != 0 && inSampleRate != voiceInfo.SamplingRate)
			{
				if (voiceInfo.SamplingRate <= 0 || inSampleRate / voiceInfo.SamplingRate > 10 || voiceInfo.SamplingRate / inSampleRate > 10)
				{
					throw new Exception(base.LogPrefix + ": unsupported values for resamling ratio: " + voiceInfo.SamplingRate + "/" + inSampleRate);
				}
				framer = new FramerResampler<T>(voiceInfo.FrameSize, voiceInfo.Channels, voiceInfo.SamplingRate, inSampleRate, interpolate: true);
				OptimalSourceFrameSize = voiceInfo.FrameSize * inSampleRate / voiceInfo.SamplingRate;
				base.voiceClient.logger.Log(LogLevel.Warning, "[PV] Local voice #" + base.id + " audio source frequency " + inSampleRate + " and encoder sampling rate " + voiceInfo.SamplingRate + " do not match. Resampling will occur before encoding (FramerResampler, interp).");
			}
			else
			{
				framer = new Framer<T>(voiceInfo.FrameSize);
				base.voiceClient.logger.Log(LogLevel.Info, "[PV] Local voice #" + base.id + " audio source frequency and encoder sampling rate are the same " + voiceInfo.SamplingRate + ". No resampling required (Framer).");
			}
			bufferFactory = new ArrayPoolSet<T>(50, base.Name, OptimalSourceFrameSize, 5);
		}

		public void PushDataAsync(T[] buf)
		{
			if (disposed)
			{
				return;
			}
			if (!threadingEnabled)
			{
				PushData(buf);
				bufferFactory.Free(buf, buf.Length);
				return;
			}
			if (!dataEncodeThreadStarted)
			{
				voiceClient.logger.Log(LogLevel.Info, base.LogPrefix + ": Starting data encode thread");
				Thread thread = new Thread(PushDataAsyncThread);
				thread.Start();
				Util.SetThreadName(thread, "[PV] Enc" + base.shortName);
				dataEncodeThreadStarted = true;
			}
			if (PushDataAsyncReady)
			{
				lock (pushDataQueue)
				{
					pushDataQueue.Enqueue(buf);
				}
				pushDataQueueReady.Set();
				return;
			}
			bufferFactory.Free(buf, buf.Length);
			if (framesSkipped == framesSkippedNextLog)
			{
				voiceClient.logger.Log(LogLevel.Warning, base.LogPrefix + ": PushData queue overflow. Frames skipped: " + (framesSkipped + 1));
				framesSkippedNextLog = framesSkipped + 10;
			}
			framesSkipped++;
		}

		private void PushDataAsyncThread()
		{
			try
			{
				while (!exitThread)
				{
					pushDataQueueReady.WaitOne();
					while (!exitThread)
					{
						T[] array = null;
						lock (pushDataQueue)
						{
							if (pushDataQueue.Count > 0)
							{
								array = pushDataQueue.Dequeue();
							}
						}
						if (array == null)
						{
							break;
						}
						PushData(array);
						bufferFactory.Free(array, array.Length);
					}
				}
			}
			catch (Exception ex)
			{
				voiceClient.logger.Log(LogLevel.Error, base.LogPrefix + ": Exception in encode thread: " + ex);
				throw ex;
			}
			finally
			{
				Dispose();
				bufferFactory.Dispose();
				pushDataQueueReady.Close();
				voiceClient.logger.Log(LogLevel.Info, base.LogPrefix + ": Exiting data encode thread");
			}
		}

		public void PushData(T[] buf)
		{
			if (!base.TransmitEnabled)
			{
				return;
			}
			if (encoder is IEncoderDirect<T[]>)
			{
				lock (disposeLock)
				{
					if (!disposed)
					{
						T[] array = processFrame(buf, 0, preProcessorsCnt);
						if (array != null)
						{
							foreach (T[] item in framer.Frame(array))
							{
								T[] array2 = processFrame(item, preProcessorsCnt, processors.Count);
								if (array2 != null)
								{
									processNullFramesCnt = 0;
									((IEncoderDirect<T[]>)encoder).Input(array2);
								}
								else
								{
									processNullFramesCnt++;
									if (processNullFramesCnt == 1)
									{
										encoder.EndOfStream();
									}
								}
							}
							return;
						}
						processNullFramesCnt++;
						if (processNullFramesCnt == 1)
						{
							encoder.EndOfStream();
						}
					}
					return;
				}
			}
			throw new Exception(base.LogPrefix + ": PushData(T[]) called on encoder of unsupported type " + ((encoder == null) ? "null" : encoder.GetType().ToString()));
		}

		public override void Dispose()
		{
			exitThread = true;
			lock (disposeLock)
			{
				if (disposed)
				{
					return;
				}
				foreach (IProcessor<T> processor in processors)
				{
					processor.Dispose();
				}
				base.Dispose();
				pushDataQueueReady.Set();
			}
		}
	}
}
