using UnityEngine;

namespace SCPE
{
	public static class ShaderParameters
	{
		public static int _BlitScaleBiasRt = Shader.PropertyToID("_BlitScaleBiasRt");

		public static int _BlitScaleBias = Shader.PropertyToID("_BlitScaleBias");

		public static int _DeferredRendering = Shader.PropertyToID("_DeferredRendering");

		public static int unity_WorldToLight = Shader.PropertyToID("unity_WorldToLight");

		public static int Params = Shader.PropertyToID("_Params");

		public static int FadeParams = Shader.PropertyToID("_FadeParams");

		public static int BlurOffsets = Shader.PropertyToID("_BlurOffsets");

		public static int BlurRadius = Shader.PropertyToID("_BlurRadius");
	}
}
