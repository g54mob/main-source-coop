using System;
using UnityEngine;

namespace Rewired.Utils.Attributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class FieldRangeAttribute : PropertyAttribute
	{
		private float YRHrcKrBLPvmwYXdAQyyTlwlmumT;

		private float RzgrGCYyrhCVDzCkvrSjoYoVqjrx;

		private int aTbcQLCPgnzuFjfhjhRweGDvYLxcA;

		private int KEqmKuqqmMYgHOmbGEbSptqMGQZeA;

		public float minFloat => YRHrcKrBLPvmwYXdAQyyTlwlmumT;

		public float maxFloat => RzgrGCYyrhCVDzCkvrSjoYoVqjrx;

		public int minInt => aTbcQLCPgnzuFjfhjhRweGDvYLxcA;

		public int maxInt => KEqmKuqqmMYgHOmbGEbSptqMGQZeA;

		public FieldRangeAttribute(float P_0, float P_1)
		{
			YRHrcKrBLPvmwYXdAQyyTlwlmumT = P_0;
			RzgrGCYyrhCVDzCkvrSjoYoVqjrx = P_1;
			aTbcQLCPgnzuFjfhjhRweGDvYLxcA = (int)P_0;
			KEqmKuqqmMYgHOmbGEbSptqMGQZeA = (int)P_1;
		}

		public FieldRangeAttribute(int P_0, int P_1)
		{
			aTbcQLCPgnzuFjfhjhRweGDvYLxcA = P_0;
			KEqmKuqqmMYgHOmbGEbSptqMGQZeA = P_1;
			YRHrcKrBLPvmwYXdAQyyTlwlmumT = P_0;
			RzgrGCYyrhCVDzCkvrSjoYoVqjrx = P_1;
		}
	}
}
