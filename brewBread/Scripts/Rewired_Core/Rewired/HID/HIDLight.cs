using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Rewired.Utils;

namespace Rewired.HID
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class HIDLight
	{
		private byte YcsrlJGBOhyhcFlRCbdISiBiFNSu;

		private byte mFxONZgLRwKKCRcrHxWcAMvKbTUP;

		private byte ZvrPPdyFZNXFZsKeLYnExKqycAkx;

		[CompilerGenerated]
		private Action yTiFSKKlGRdjfcEtgTrPZLpZcukr;

		public float ColorR
		{
			get
			{
				return (float)(int)YcsrlJGBOhyhcFlRCbdISiBiFNSu / 255f;
			}
			set
			{
				ColorRRaw = (byte)MathTools.Clamp((int)(value * 255f), 0, 255);
			}
		}

		public float ColorG
		{
			get
			{
				return (float)(int)mFxONZgLRwKKCRcrHxWcAMvKbTUP / 255f;
			}
			set
			{
				ColorGRaw = (byte)MathTools.Clamp((int)(value * 255f), 0, 255);
			}
		}

		public float ColorB
		{
			get
			{
				return (float)(int)ZvrPPdyFZNXFZsKeLYnExKqycAkx / 255f;
			}
			set
			{
				ColorBRaw = (byte)MathTools.Clamp((int)(value * 255f), 0, 255);
			}
		}

		public byte ColorRRaw
		{
			get
			{
				return YcsrlJGBOhyhcFlRCbdISiBiFNSu;
			}
			set
			{
				YcsrlJGBOhyhcFlRCbdISiBiFNSu = value;
				if (yTiFSKKlGRdjfcEtgTrPZLpZcukr != null)
				{
					yTiFSKKlGRdjfcEtgTrPZLpZcukr();
				}
			}
		}

		public byte ColorGRaw
		{
			get
			{
				return mFxONZgLRwKKCRcrHxWcAMvKbTUP;
			}
			set
			{
				mFxONZgLRwKKCRcrHxWcAMvKbTUP = value;
				if (yTiFSKKlGRdjfcEtgTrPZLpZcukr != null)
				{
					yTiFSKKlGRdjfcEtgTrPZLpZcukr();
				}
			}
		}

		public byte ColorBRaw
		{
			get
			{
				return ZvrPPdyFZNXFZsKeLYnExKqycAkx;
			}
			set
			{
				ZvrPPdyFZNXFZsKeLYnExKqycAkx = value;
				if (yTiFSKKlGRdjfcEtgTrPZLpZcukr != null)
				{
					yTiFSKKlGRdjfcEtgTrPZLpZcukr();
				}
			}
		}

		public event Action ValueChangedEvent
		{
			[CompilerGenerated]
			add
			{
				Action action = yTiFSKKlGRdjfcEtgTrPZLpZcukr;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref yTiFSKKlGRdjfcEtgTrPZLpZcukr, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = yTiFSKKlGRdjfcEtgTrPZLpZcukr;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref yTiFSKKlGRdjfcEtgTrPZLpZcukr, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public HIDLight()
		{
		}

		public HIDLight(byte P_0, byte P_1, byte P_2)
		{
			YcsrlJGBOhyhcFlRCbdISiBiFNSu = P_0;
			mFxONZgLRwKKCRcrHxWcAMvKbTUP = P_1;
			ZvrPPdyFZNXFZsKeLYnExKqycAkx = P_2;
		}
	}
}
