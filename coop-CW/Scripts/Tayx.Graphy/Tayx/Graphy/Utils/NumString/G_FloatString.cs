using UnityEngine;

namespace Tayx.Graphy.Utils.NumString
{
	public static class G_FloatString
	{
		private const string m_floatFormat = "0.0";

		private static float m_decimalMultiplier = 10f;

		private static string[] m_negativeBuffer = new string[0];

		private static string[] m_positiveBuffer = new string[0];

		public static float MinValue => 0f - (m_negativeBuffer.Length - 1).FromIndex();

		public static float MaxValue => (m_positiveBuffer.Length - 1).FromIndex();

		public static void Init(float minNegativeValue, float maxPositiveValue)
		{
			int num = minNegativeValue.ToIndex();
			int num2 = maxPositiveValue.ToIndex();
			if (MinValue > minNegativeValue && num >= 0)
			{
				m_negativeBuffer = new string[num];
				for (int i = 0; i < num; i++)
				{
					m_negativeBuffer[i] = (-i - 1).FromIndex().ToString("0.0");
				}
			}
			if (MaxValue < maxPositiveValue && num2 >= 0)
			{
				m_positiveBuffer = new string[num2 + 1];
				for (int j = 0; j < num2 + 1; j++)
				{
					m_positiveBuffer[j] = j.FromIndex().ToString("0.0");
				}
			}
		}

		public static void Dispose()
		{
			m_negativeBuffer = new string[0];
			m_positiveBuffer = new string[0];
		}

		public static string ToStringNonAlloc(this float value)
		{
			int num = value.ToIndex();
			if (value < 0f && num < m_negativeBuffer.Length)
			{
				return m_negativeBuffer[num];
			}
			if (value >= 0f && num < m_positiveBuffer.Length)
			{
				return m_positiveBuffer[num];
			}
			return value.ToString();
		}

		public static string ToStringNonAlloc(this float value, string format)
		{
			int num = value.ToIndex();
			if (value < 0f && num < m_negativeBuffer.Length)
			{
				return m_negativeBuffer[num];
			}
			if (value >= 0f && num < m_positiveBuffer.Length)
			{
				return m_positiveBuffer[num];
			}
			return value.ToString(format);
		}

		public static int ToInt(this float f)
		{
			return (int)f;
		}

		public static float ToFloat(this int i)
		{
			return i;
		}

		private static int ToIndex(this float f)
		{
			return Mathf.Abs((f * m_decimalMultiplier).ToInt());
		}

		private static float FromIndex(this int i)
		{
			return i.ToFloat() / m_decimalMultiplier;
		}
	}
}
