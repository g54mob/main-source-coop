using UnityEngine;

namespace Tayx.Graphy.Utils.NumString
{
	public static class G_IntString
	{
		private static string[] m_negativeBuffer = new string[0];

		private static string[] m_positiveBuffer = new string[0];

		public static int MinValue => -(m_negativeBuffer.Length - 1);

		public static int MaxValue => m_positiveBuffer.Length;

		public static void Init(int minNegativeValue, int maxPositiveValue)
		{
			if (MinValue > minNegativeValue && minNegativeValue <= 0)
			{
				int num = Mathf.Abs(minNegativeValue);
				m_negativeBuffer = new string[num];
				for (int i = 0; i < num; i++)
				{
					m_negativeBuffer[i] = (-i - 1).ToString();
				}
			}
			if (MaxValue < maxPositiveValue && maxPositiveValue >= 0)
			{
				m_positiveBuffer = new string[maxPositiveValue + 1];
				for (int j = 0; j < maxPositiveValue + 1; j++)
				{
					m_positiveBuffer[j] = j.ToString();
				}
			}
		}

		public static void Dispose()
		{
			m_negativeBuffer = new string[0];
			m_positiveBuffer = new string[0];
		}

		public static string ToStringNonAlloc(this int value)
		{
			if (value < 0 && -value <= m_negativeBuffer.Length)
			{
				return m_negativeBuffer[-value - 1];
			}
			if (value >= 0 && value < m_positiveBuffer.Length)
			{
				return m_positiveBuffer[value];
			}
			return value.ToString();
		}
	}
}
