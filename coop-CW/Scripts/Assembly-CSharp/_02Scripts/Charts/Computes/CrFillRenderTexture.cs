using UnityEngine;
using pworld.Scripts.Extensions;

namespace _02Scripts.Charts.Computes
{
	public static class CrFillRenderTexture
	{
		public static ComputeShader csFillRenderTexture;

		private const string KERNEL_NAME = "FillRenderTexture";

		private const string RT_NAME = "renderTexture";

		private const string FILL_COLOR_NAME = "fillColor";

		private const string RESOLUTION_NAME = "resolution";

		private const string THREAD_GROUP_NAME = "threadGroups";

		public static void FillRenderTexture(RenderTexture chunkRt, Color color)
		{
			int num = csFillRenderTexture.FindKernel("FillRenderTexture");
			csFillRenderTexture.SetTexture(num, "renderTexture", chunkRt);
			csFillRenderTexture.SetVector("fillColor", color);
			Vector2 vector = chunkRt.PGetSize();
			csFillRenderTexture.SetVector("resolution", vector);
			csFillRenderTexture.PDispatch(num, "threadGroups", vector.x, vector.y);
		}
	}
}
