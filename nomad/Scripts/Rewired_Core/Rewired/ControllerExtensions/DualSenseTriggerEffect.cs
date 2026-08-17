using System;
using Rewired.Utils;

namespace Rewired.ControllerExtensions
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false)]
	internal static class DualSenseTriggerEffect
	{
		public const byte strengthMin = 0;

		public const byte strengthMax = 8;

		public const byte amplitudeMin = 0;

		public const byte amplitudeMax = 8;

		public const byte frequencyMin = 0;

		public const byte frequencyMax = 255;

		public const byte positionCount = 10;

		public const byte positionMin = 0;

		public const byte positionMax = 9;

		internal static bool IsInRange(byte value, byte min, byte max)
		{
			if (value >= min)
			{
				return value <= max;
			}
			return false;
		}

		internal static byte Clamp(byte value, byte min, byte max)
		{
			if (value < min)
			{
				LogValueClamped(value, min);
				return min;
			}
			if (value > max)
			{
				LogValueClamped(value, max);
				return max;
			}
			return value;
		}

		internal static float NormalizeStrength(byte value)
		{
			return MathTools.Clamp01((float)(int)value / 8f);
		}

		internal static float NormalizePosition(byte value)
		{
			return MathTools.Clamp01((float)(int)value / 9f);
		}

		internal static float NormalizeAmplitude(byte value)
		{
			return MathTools.Clamp01((float)(int)value / 8f);
		}

		internal static float NormalizeFrequency(byte value)
		{
			return MathTools.Clamp01((float)(int)value / 255f);
		}

		internal static void ThrowArgumentOutOfRange(string name, byte min, byte max)
		{
			throw new ArgumentOutOfRangeException(name + " is outside the allowed range of " + min + " - " + max);
		}

		internal static void LogValueClamped(byte origValue, byte clampedValue)
		{
			Logger.LogWarning("Trigger effect parameter value " + origValue + " was outside the allowed range and was clamped to " + clampedValue, requiredThreadSafety: true);
		}
	}
}
