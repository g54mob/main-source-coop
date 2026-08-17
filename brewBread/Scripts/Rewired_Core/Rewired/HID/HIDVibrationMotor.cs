using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Rewired.Utils;

namespace Rewired.HID
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class HIDVibrationMotor
	{
		private int VYLwSmqqPJbqHegXduvgUzJZPVdo;

		private int GvrxGKAAUCfbrSaWQiwGtfzeQdHu;

		private int YNspTHPVtMHjZAqqMDrpTxyGCBio;

		[CompilerGenerated]
		private Action yTiFSKKlGRdjfcEtgTrPZLpZcukr;

		public float Speed
		{
			get
			{
				return rmeXjnRGCesaYQiHIswitoihOnGf(VYLwSmqqPJbqHegXduvgUzJZPVdo);
			}
			set
			{
				VYLwSmqqPJbqHegXduvgUzJZPVdo = GZIbnCEBhplFIyqgrrdKIWOqBFghb(value);
				if (yTiFSKKlGRdjfcEtgTrPZLpZcukr != null)
				{
					yTiFSKKlGRdjfcEtgTrPZLpZcukr();
				}
			}
		}

		public int SpeedRaw
		{
			get
			{
				return VYLwSmqqPJbqHegXduvgUzJZPVdo;
			}
			set
			{
				VYLwSmqqPJbqHegXduvgUzJZPVdo = value;
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

		public HIDVibrationMotor(int P_0, int P_1)
		{
			GvrxGKAAUCfbrSaWQiwGtfzeQdHu = P_0;
			YNspTHPVtMHjZAqqMDrpTxyGCBio = P_1;
		}

		private float rmeXjnRGCesaYQiHIswitoihOnGf(int P_0)
		{
			return MathTools.Clamp((float)P_0 / (float)YNspTHPVtMHjZAqqMDrpTxyGCBio, 0f, 1f);
		}

		private int GZIbnCEBhplFIyqgrrdKIWOqBFghb(float P_0)
		{
			return MathTools.Clamp((int)(P_0 * (float)YNspTHPVtMHjZAqqMDrpTxyGCBio), GvrxGKAAUCfbrSaWQiwGtfzeQdHu, YNspTHPVtMHjZAqqMDrpTxyGCBio);
		}
	}
}
