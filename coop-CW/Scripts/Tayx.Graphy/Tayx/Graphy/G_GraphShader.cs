using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy
{
	public class G_GraphShader
	{
		public const int ArrayMaxSizeFull = 512;

		public const int ArrayMaxSizeLight = 128;

		public int ArrayMaxSize = 128;

		public float[] ShaderArrayValues;

		public Image Image;

		public float Average;

		public float GoodThreshold;

		public float CautionThreshold;

		public Color GoodColor = Color.white;

		public Color CautionColor = Color.white;

		public Color CriticalColor = Color.white;

		private static readonly int AveragePropertyId = Shader.PropertyToID("Average");

		private static readonly int GoodThresholdPropertyId = Shader.PropertyToID("_GoodThreshold");

		private static readonly int CautionThresholdPropertyId = Shader.PropertyToID("_CautionThreshold");

		private static readonly int GoodColorPropertyId = Shader.PropertyToID("_GoodColor");

		private static readonly int CautionColorPropertyId = Shader.PropertyToID("_CautionColor");

		private static readonly int CriticalColorPropertyId = Shader.PropertyToID("_CriticalColor");

		private static readonly int GraphValues = Shader.PropertyToID("GraphValues");

		private static readonly int GraphValuesLength = Shader.PropertyToID("GraphValues_Length");

		public void InitializeShader()
		{
			Image.material.SetFloatArray(GraphValues, new float[ArrayMaxSize]);
		}

		public void UpdateArrayValuesLength()
		{
			Image.material.SetInt(GraphValuesLength, ShaderArrayValues.Length);
		}

		public void UpdateAverage()
		{
			Image.material.SetFloat(AveragePropertyId, Average);
		}

		public void UpdateThresholds()
		{
			Image.material.SetFloat(GoodThresholdPropertyId, GoodThreshold);
			Image.material.SetFloat(CautionThresholdPropertyId, CautionThreshold);
		}

		public void UpdateColors()
		{
			Image.material.SetColor(GoodColorPropertyId, GoodColor);
			Image.material.SetColor(CautionColorPropertyId, CautionColor);
			Image.material.SetColor(CriticalColorPropertyId, CriticalColor);
		}

		public void UpdatePoints()
		{
			Image.material.SetFloatArray(GraphValues, ShaderArrayValues);
		}
	}
}
