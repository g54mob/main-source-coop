using System;
using UnityEngine;

namespace Rewired.Utils.Attributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class FieldRangeAttribute : PropertyAttribute
	{
		private float bYmOxJJyJaFKspywVsKasCOcNpsh;

		private float uoDCgViMcSlRjuDEmHIQcvAUXOOM;

		private int uLkTrcFrBuGSQtlMJiSFhMeHDJEHA;

		private int rMyyJscCHWYlPzDUVWvqeyfjgqNN;

		public float minFloat => bYmOxJJyJaFKspywVsKasCOcNpsh;

		public float maxFloat => uoDCgViMcSlRjuDEmHIQcvAUXOOM;

		public int minInt => uLkTrcFrBuGSQtlMJiSFhMeHDJEHA;

		public int maxInt => rMyyJscCHWYlPzDUVWvqeyfjgqNN;

		public FieldRangeAttribute(float P_0, float P_1)
		{
			bYmOxJJyJaFKspywVsKasCOcNpsh = P_0;
			uoDCgViMcSlRjuDEmHIQcvAUXOOM = P_1;
			uLkTrcFrBuGSQtlMJiSFhMeHDJEHA = (int)P_0;
			rMyyJscCHWYlPzDUVWvqeyfjgqNN = (int)P_1;
		}

		public FieldRangeAttribute(int P_0, int P_1)
		{
			uLkTrcFrBuGSQtlMJiSFhMeHDJEHA = P_0;
			rMyyJscCHWYlPzDUVWvqeyfjgqNN = P_1;
			bYmOxJJyJaFKspywVsKasCOcNpsh = P_0;
			uoDCgViMcSlRjuDEmHIQcvAUXOOM = P_1;
		}
	}
}
