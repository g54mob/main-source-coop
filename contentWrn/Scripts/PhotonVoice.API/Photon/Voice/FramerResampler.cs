using System.Collections.Generic;

namespace Photon.Voice
{
	public class FramerResampler<T> : Framer<T>
	{
		protected bool TisFloat;

		protected bool interpolate;

		protected int channels;

		protected int resampleNum;

		protected int resampleDen;

		protected float resampleRatioInv;

		protected int delta;

		private T inSampleCh1;

		private T inSampleCh2;

		private float inSampleCh1Interp;

		private float inSampleCh2Interp;

		private float inSampleCh1InterpChange;

		private float inSampleCh2InterpChange;

		public FramerResampler(int frameSize, int channels, int resampleNum, int resampleDen, bool interpolate)
			: base(frameSize / channels * channels)
		{
			TisFloat = default(T) is float;
			this.channels = channels;
			this.resampleNum = resampleNum;
			this.resampleDen = resampleDen;
			this.interpolate = interpolate;
			resampleRatioInv = (float)resampleDen / (float)resampleNum;
		}

		public override IEnumerable<T[]> Frame(T[] bufT)
		{
			int bufPos = 0;
			int bufLen = bufT.Length / channels * channels;
			int num;
			if (!interpolate)
			{
				num = channels;
				if (num != 1)
				{
					if (num != 2)
					{
						yield break;
					}
					while (bufPos < bufLen)
					{
						if (delta <= 0)
						{
							inSampleCh1 = bufT[bufPos++];
							inSampleCh2 = bufT[bufPos++];
							delta += resampleNum;
						}
						while (delta > 0)
						{
							base.frame[framePos++] = inSampleCh1;
							base.frame[framePos++] = inSampleCh2;
							if (framePos == base.frame.Length)
							{
								yield return base.frame;
								framePos = 0;
							}
							delta -= resampleDen;
						}
					}
					yield break;
				}
				while (bufPos < bufLen)
				{
					if (delta <= 0)
					{
						inSampleCh1 = bufT[bufPos++];
						delta += resampleNum;
					}
					while (delta > 0)
					{
						base.frame[framePos++] = inSampleCh1;
						if (framePos == base.frame.Length)
						{
							yield return base.frame;
							framePos = 0;
						}
						delta -= resampleDen;
					}
				}
				yield break;
			}
			float deltaK;
			if (TisFloat)
			{
				float[] buf = bufT as float[];
				float[] frame = base.frame as float[];
				deltaK = (float)delta / (float)resampleNum;
				num = channels;
				if (num != 1)
				{
					if (num != 2)
					{
						yield break;
					}
					while (bufPos < bufLen)
					{
						if (delta <= 0)
						{
							float num2 = buf[bufPos++];
							float num3 = buf[bufPos++];
							inSampleCh1InterpChange = inSampleCh1Interp - num2;
							inSampleCh2InterpChange = inSampleCh2Interp - num3;
							inSampleCh1Interp = num2;
							inSampleCh2Interp = num3;
							delta += resampleNum;
							deltaK += 1f;
						}
						while (delta > 0)
						{
							frame[framePos++] = inSampleCh1Interp + inSampleCh1InterpChange * deltaK;
							frame[framePos++] = inSampleCh2Interp + inSampleCh2InterpChange * deltaK;
							if (framePos == frame.Length)
							{
								yield return base.frame;
								framePos = 0;
							}
							delta -= resampleDen;
							deltaK -= resampleRatioInv;
						}
					}
					yield break;
				}
				while (bufPos < bufLen)
				{
					if (delta <= 0)
					{
						float num4 = buf[bufPos++];
						inSampleCh1InterpChange = inSampleCh1Interp - num4;
						inSampleCh1Interp = num4;
						delta += resampleNum;
						deltaK += 1f;
					}
					while (delta > 0)
					{
						frame[framePos++] = inSampleCh1Interp + inSampleCh1InterpChange * deltaK;
						if (framePos == frame.Length)
						{
							yield return base.frame;
							framePos = 0;
						}
						delta -= resampleDen;
						deltaK -= resampleRatioInv;
					}
				}
				yield break;
			}
			short[] buf2 = bufT as short[];
			short[] frame2 = base.frame as short[];
			deltaK = (float)delta / (float)resampleNum;
			num = channels;
			if (num != 1)
			{
				if (num != 2)
				{
					yield break;
				}
				while (bufPos < bufLen)
				{
					if (delta <= 0)
					{
						short num5 = buf2[bufPos++];
						short num6 = buf2[bufPos++];
						inSampleCh1InterpChange = inSampleCh1Interp - (float)num5;
						inSampleCh2InterpChange = inSampleCh2Interp - (float)num6;
						inSampleCh1Interp = num5;
						inSampleCh2Interp = num6;
						delta += resampleNum;
						deltaK += 1f;
					}
					while (delta > 0)
					{
						frame2[framePos++] = (short)(inSampleCh1Interp + inSampleCh1InterpChange * deltaK);
						frame2[framePos++] = (short)(inSampleCh2Interp + inSampleCh2InterpChange * deltaK);
						if (framePos == frame2.Length)
						{
							yield return base.frame;
							framePos = 0;
						}
						delta -= resampleDen;
						deltaK -= resampleRatioInv;
					}
				}
				yield break;
			}
			while (bufPos < bufLen)
			{
				if (delta <= 0)
				{
					short num7 = buf2[bufPos++];
					inSampleCh1InterpChange = inSampleCh1Interp - (float)num7;
					inSampleCh1Interp = num7;
					delta += resampleNum;
					deltaK += 1f;
				}
				while (delta > 0)
				{
					frame2[framePos++] = (short)(inSampleCh1Interp + inSampleCh1InterpChange * deltaK);
					if (framePos == frame2.Length)
					{
						yield return base.frame;
						framePos = 0;
					}
					delta -= resampleDen;
					deltaK -= resampleRatioInv;
				}
			}
		}
	}
}
