using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using POpusCodec.Enums;
using Photon.Voice;

namespace POpusCodec
{
	public class OpusDecoderAsync<T> : OpusDecoder<T>
	{
		protected static Dictionary<IntPtr, OpusDecoderAsync<T>> handles = new Dictionary<IntPtr, OpusDecoderAsync<T>>();

		private float[] bufOutFloat;

		private short[] bufOutShort;

		[MonoPInvokeCallback(typeof(Action<IntPtr, IntPtr, int, bool>))]
		public static void DataCallbackStatic(IntPtr handle, IntPtr p, int count, bool endOfStream)
		{
			if (handles.TryGetValue(handle, out var value))
			{
				value.dataCallback(p, count, endOfStream);
			}
		}

		public OpusDecoderAsync(Action<FrameOut<T>> output, SamplingRate outputSamplingRateHz, Channels numChannels, int frameDurationSamples)
			: base(output, outputSamplingRateHz, numChannels, frameDurationSamples)
		{
			handles[handle] = this;
		}

		protected void dataCallback(IntPtr p, int count, bool endOfStream)
		{
			if (output == null)
			{
				return;
			}
			if (TisFloat)
			{
				if (bufOutFloat == null || bufOutFloat.Length < count)
				{
					bufOutFloat = new float[count];
				}
				Marshal.Copy(p, bufOutFloat, 0, count);
				procOutput(bufOutFloat as T[], endOfStream);
			}
			else
			{
				if (bufOutShort == null || bufOutShort.Length < count)
				{
					bufOutShort = new short[count];
				}
				Marshal.Copy(p, bufOutShort, 0, count);
				procOutput(bufOutShort as T[], endOfStream);
			}
		}

		public override void Dispose()
		{
			if (handle != IntPtr.Zero)
			{
				handles.Remove(handle);
			}
			base.Dispose();
		}
	}
}
