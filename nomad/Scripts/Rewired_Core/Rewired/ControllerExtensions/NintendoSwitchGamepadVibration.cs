using System;
using Rewired.Utils.Attributes;

namespace Rewired.ControllerExtensions
{
	[Serializable]
	public struct NintendoSwitchGamepadVibration : IEquatable<NintendoSwitchGamepadVibration>
	{
		internal const int frequencyLowDefault = 160;

		internal const int frequencyHighDefault = 320;

		public const float frequencyLowMin = 40.875885f;

		public const float frequencyLowMax = 626.28613f;

		public const float frequencyHighMin = 81.75177f;

		public const float frequencyHighMax = 1252.5723f;

		[FieldRange(0f, 1f)]
		public float amplitudeLow;

		[FieldRange(40.875885f, 626.28613f)]
		public float frequencyLow;

		[FieldRange(0f, 1f)]
		public float amplitudeHigh;

		[FieldRange(81.75177f, 1252.5723f)]
		public float frequencyHigh;

		internal static NintendoSwitchGamepadVibration MuGeSBARWXVWUKoGXbDgeSZGhIQJB => default(NintendoSwitchGamepadVibration);

		internal NintendoSwitchGamepadVibration(float P_0, float P_1, float P_2, float P_3)
		{
			if (P_0 < 0f)
			{
				P_0 = 0f;
			}
			if (P_0 > 1f)
			{
				P_0 = 1f;
			}
			if (P_1 < 0f)
			{
				P_1 = 0f;
			}
			if (P_2 < 0f)
			{
				P_2 = 0f;
			}
			if (P_2 > 1f)
			{
				P_2 = 1f;
			}
			if (P_3 < 0f)
			{
				P_3 = 0f;
			}
			amplitudeLow = P_0;
			frequencyLow = P_1;
			amplitudeHigh = P_2;
			frequencyHigh = P_3;
		}

		public bool Equals(NintendoSwitchGamepadVibration other)
		{
			if (amplitudeLow == other.amplitudeLow && frequencyLow == other.frequencyLow && amplitudeHigh == other.amplitudeHigh)
			{
				return frequencyHigh == other.frequencyHigh;
			}
			return false;
		}

		bool IEquatable<NintendoSwitchGamepadVibration>.Equals(NintendoSwitchGamepadVibration other)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Equals
			return this.Equals(other);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is NintendoSwitchGamepadVibration))
			{
				return false;
			}
			return Equals((NintendoSwitchGamepadVibration)obj);
		}

		public override int GetHashCode()
		{
			return (((17 * 29 + amplitudeLow.GetHashCode()) * 29 + frequencyLow.GetHashCode()) * 29 + amplitudeHigh.GetHashCode()) * 29 + frequencyHigh.GetHashCode();
		}

		public static bool operator ==(NintendoSwitchGamepadVibration a, NintendoSwitchGamepadVibration b)
		{
			if (a.amplitudeLow == b.amplitudeLow && a.frequencyLow == b.frequencyLow && a.amplitudeHigh == b.amplitudeHigh)
			{
				return a.frequencyHigh == b.frequencyHigh;
			}
			return false;
		}

		public static bool operator !=(NintendoSwitchGamepadVibration a, NintendoSwitchGamepadVibration b)
		{
			return !(a == b);
		}

		public override string ToString()
		{
			return string.Format("Low:{0}, {1}Hz, High:{2}, {3}Hz", amplitudeLow.ToString("f3"), frequencyLow.ToString("f3"), amplitudeHigh.ToString("f3"), frequencyHigh.ToString("f3"));
		}

		public static NintendoSwitchGamepadVibration Create()
		{
			return new NintendoSwitchGamepadVibration(0f, 160f, 0f, 320f);
		}

		public static NintendoSwitchGamepadVibration Create(float amplitudeLow, float frequencyLow, float amplitudeHigh, float frequencyHigh)
		{
			return new NintendoSwitchGamepadVibration(amplitudeLow, frequencyLow, amplitudeHigh, frequencyHigh);
		}

		public static NintendoSwitchGamepadVibration Create(float amplitudeLow, float amplitudeHigh)
		{
			return new NintendoSwitchGamepadVibration(amplitudeLow, 160f, amplitudeHigh, 320f);
		}
	}
}
