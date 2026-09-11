using System;
using POpusCodec;
using POpusCodec.Enums;

namespace Photon.Voice
{
	public class OpusCodec
	{
		public enum FrameDuration
		{
			Frame2dot5ms = 2500,
			Frame5ms = 5000,
			Frame10ms = 10000,
			Frame20ms = 20000,
			Frame40ms = 40000,
			Frame60ms = 60000
		}

		public static class Factory
		{
			public static IEncoder CreateEncoder<B>(VoiceInfo i, ILogger logger)
			{
				if (typeof(B) == typeof(float[]))
				{
					return new EncoderFloat(i, logger);
				}
				if (typeof(B) == typeof(short[]))
				{
					return new EncoderShort(i, logger);
				}
				throw new UnsupportedCodecException("Factory.CreateEncoder<" + typeof(B)?.ToString() + ">", i.Codec);
			}
		}

		public abstract class Encoder<T> : IEncoderDirect<T[]>, IEncoder, IDisposable
		{
			protected OpusEncoder encoder;

			protected bool disposed;

			private Action<ArraySegment<byte>, FrameFlags> output;

			private static readonly ArraySegment<byte> EmptyBuffer = new ArraySegment<byte>(new byte[0]);

			public string Error { get; private set; }

			public Action<ArraySegment<byte>, FrameFlags> Output
			{
				get
				{
					return output;
				}
				set
				{
					output = value;
					encoder.Output = value;
				}
			}

			protected Encoder(VoiceInfo i, ILogger logger)
			{
				try
				{
					encoder = new OpusEncoder((SamplingRate)i.SamplingRate, (Channels)i.Channels, i.Bitrate, OpusApplicationType.Voip, (Delay)(i.FrameDurationUs * 2 / 1000));
					string version = Version;
					VoiceInfo voiceInfo = i;
					logger.LogInfo("[PV] OpusCodec.Encoder created. Opus version " + version + ", " + voiceInfo.ToString());
				}
				catch (Exception ex)
				{
					Error = ex.ToString();
					if (Error == null)
					{
						Error = "Exception in OpusCodec.Encoder constructor";
					}
					logger.LogError("[PV] OpusCodec.Encoder: " + Error);
				}
			}

			public void Input(T[] buf)
			{
				if (Error != null)
				{
					return;
				}
				if (Output == null)
				{
					Error = "OpusCodec.Encoder: Output action is not set";
					return;
				}
				lock (this)
				{
					if (!disposed && Error == null)
					{
						encodeTyped(buf);
					}
				}
			}

			public void EndOfStream()
			{
				lock (this)
				{
					if (!disposed && Error == null)
					{
						Output(EmptyBuffer, FrameFlags.EndOfStream);
					}
				}
			}

			public ArraySegment<byte> DequeueOutput(out FrameFlags flags)
			{
				flags = (FrameFlags)0;
				return EmptyBuffer;
			}

			protected abstract void encodeTyped(T[] buf);

			public I GetPlatformAPI<I>() where I : class
			{
				return null;
			}

			public void Dispose()
			{
				lock (this)
				{
					if (encoder != null)
					{
						encoder.Dispose();
					}
					disposed = true;
				}
			}
		}

		public class EncoderFloat : Encoder<float>
		{
			internal EncoderFloat(VoiceInfo i, ILogger logger)
				: base(i, logger)
			{
			}

			protected override void encodeTyped(float[] buf)
			{
				encoder.Encode(buf);
			}
		}

		public class EncoderShort : Encoder<short>
		{
			internal EncoderShort(VoiceInfo i, ILogger logger)
				: base(i, logger)
			{
			}

			protected override void encodeTyped(short[] buf)
			{
				encoder.Encode(buf);
			}
		}

		public class Decoder<T> : IDecoder, IDisposable
		{
			protected OpusDecoder<T> decoder;

			private ILogger logger;

			private Action<FrameOut<T>> output;

			public string Error { get; private set; }

			public Decoder(Action<FrameOut<T>> output, ILogger logger)
			{
				this.output = output;
				this.logger = logger;
			}

			public void Open(VoiceInfo i)
			{
				try
				{
					decoder = new OpusDecoder<T>(output, (SamplingRate)i.SamplingRate, (Channels)i.Channels, i.FrameDurationSamples);
					ILogger obj = logger;
					string version = Version;
					VoiceInfo voiceInfo = i;
					obj.LogInfo("[PV] OpusCodec.Decoder created. Opus version " + version + ", " + voiceInfo.ToString());
				}
				catch (Exception ex)
				{
					Error = ex.ToString();
					if (Error == null)
					{
						Error = "Exception in OpusCodec.Decoder constructor";
					}
					logger.LogError("[PV] OpusCodec.Decoder: " + Error);
				}
			}

			public void Dispose()
			{
				if (decoder != null)
				{
					decoder.Dispose();
				}
			}

			public void Input(ref FrameBuffer buf)
			{
				if (Error == null)
				{
					bool endOfStream = (buf.Flags & FrameFlags.EndOfStream) != 0;
					decoder.DecodePacket(ref buf, endOfStream);
				}
			}
		}

		public class Util
		{
			internal static int bestEncoderSampleRate(int f)
			{
				int num = int.MaxValue;
				int result = 48000;
				foreach (object value in Enum.GetValues(typeof(SamplingRate)))
				{
					int num2 = Math.Abs((int)value - f);
					if (num2 < num)
					{
						num = num2;
						result = (int)value;
					}
				}
				return result;
			}
		}

		public static string Version => OpusLib.Version;
	}
}
