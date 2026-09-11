using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DunGen.Analysis
{
	public sealed class NumberSetData
	{
		public float? Min { get; private set; }

		public float? Max { get; private set; }

		public float? Average { get; private set; }

		public float? StandardDeviation { get; private set; }

		public NumberSetData(IEnumerable<float> values)
		{
			if (!values.Any())
			{
				Min = null;
				Max = null;
				Average = null;
				StandardDeviation = null;
				return;
			}
			float[] array = values.ToArray();
			Min = array.Min();
			Max = array.Max();
			Average = array.Average();
			float[] array2 = new float[array.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = Mathf.Pow(array[i] - Average.Value, 2f);
			}
			StandardDeviation = Mathf.Sqrt(array2.Sum() / (float)array2.Length);
		}

		public override string ToString()
		{
			if (!Min.HasValue)
			{
				return "[ No data available ]";
			}
			return $"[ Min: {Min:N1}, Max: {Max:N1}, Average: {Average:N1}, Standard Deviation: {StandardDeviation:N2} ]";
		}
	}
}
