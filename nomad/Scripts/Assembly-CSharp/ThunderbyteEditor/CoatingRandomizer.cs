using UnityEngine;

namespace ThunderbyteEditor
{
	public class CoatingRandomizer : MonoBehaviour
	{
		public ShaderType shaderType;

		public bool bRandomizeLayer2Intensity;

		public bool bRandomizeLayer2Contrast;

		public bool bRandomizeLayer3Intensity;

		public bool bRandomizeLayer3Contrast;

		[Range(1f, 10f)]
		public float test = 1f;

		public float layer2IntensityMin;

		public float layer2IntensityMax = 10f;

		public float layer2ContrastMin = 0.1f;

		public float layer2ContrastMax = 10f;

		public float layer3IntensityMin;

		public float layer3IntensityMax = 10f;

		public float layer3ContrastMin = 0.1f;

		public float layer3ContrastMax = 10f;

		private string layer2Intensity = "_L2MaskIntensityOffset";

		private string layer2Contrast = "_L2MaskContrastOffset";

		private string layer3Intensity = "_L3MaskIntensityOffset";

		private string layer3Contrast = "_L3MaskContrastOffset";

		private Renderer rendererComp;

		private MaterialPropertyBlock propertyBlock;

		private void Awake()
		{
			propertyBlock = new MaterialPropertyBlock();
			rendererComp = GetComponent<Renderer>();
			rendererComp.GetPropertyBlock(propertyBlock);
			if (shaderType == ShaderType.Coating2Layers)
			{
				layer2Intensity = "_MaskIntensityOffset";
				layer2Contrast = "_MaskContrastOffset";
				if (bRandomizeLayer2Intensity)
				{
					propertyBlock.SetFloat(layer2Intensity, Random.Range(layer2IntensityMin, layer2IntensityMax));
				}
				if (bRandomizeLayer2Contrast)
				{
					propertyBlock.SetFloat(layer2Contrast, Random.Range(layer2ContrastMin, layer2ContrastMax));
				}
			}
			else if (shaderType == ShaderType.Coating3Layers)
			{
				layer2Intensity = "_L2MaskIntensityOffset";
				layer2Contrast = "_L2MaskContrastOffset";
				layer3Intensity = "_L3MaskIntensityOffset";
				layer3Contrast = "_L3MaskContrastOffset";
				if (bRandomizeLayer2Intensity)
				{
					propertyBlock.SetFloat(layer2Intensity, Random.Range(layer2IntensityMin, layer2IntensityMax));
				}
				if (bRandomizeLayer2Contrast)
				{
					propertyBlock.SetFloat(layer2Contrast, Random.Range(layer2ContrastMin, layer2ContrastMax));
				}
				if (bRandomizeLayer3Intensity)
				{
					propertyBlock.SetFloat(layer3Intensity, Random.Range(layer3IntensityMin, layer3IntensityMax));
				}
				if (bRandomizeLayer3Contrast)
				{
					propertyBlock.SetFloat(layer3Contrast, Random.Range(layer3ContrastMin, layer3ContrastMax));
				}
			}
			rendererComp.SetPropertyBlock(propertyBlock);
		}
	}
}
