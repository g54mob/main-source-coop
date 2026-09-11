using UnityEngine;
using UnityEngine.Rendering;
using pworld.Scripts.Extensions;

namespace DefaultNamespace.Petter.TitleCard
{
	public class BoolTransformTest : MonoBehaviour
	{
		public ComputeShader csRtToBool;

		public ComputeShader csBoolToRt;

		private bool[] resultData;

		public void RtToBool()
		{
			RenderTexture titleCardRt = Object.FindObjectOfType<TitleCardCanvas>().titleCardRt;
			int width = titleCardRt.width;
			int height = titleCardRt.height;
			ComputeBuffer computeBuffer = new ComputeBuffer(width * height, 1);
			int num = csRtToBool.FindKernel("CsRtToBoolArray");
			csBoolToRt.SetTexture(num, "sourceTexture", titleCardRt);
			csBoolToRt.SetBuffer(num, "resultBuffer", computeBuffer);
			csBoolToRt.SetVector("resolution", new Vector2(width, height));
			csBoolToRt.PDispatch(num, "threadGroups", width, height);
			AsyncGPUReadback.Request(computeBuffer, delegate(AsyncGPUReadbackRequest data)
			{
				if (data.hasError)
				{
					Debug.LogError("GPU readback error detected.");
				}
				else
				{
					resultData = data.GetData<bool>().ToArray();
				}
			});
			computeBuffer.Dispose();
		}

		public void BoolToRt()
		{
		}
	}
}
