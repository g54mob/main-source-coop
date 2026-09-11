using UnityEngine;
using pworld.Scripts.Extensions;

namespace _02Scripts.Charts.Computes
{
	public class CrSetupChartRT
	{
		public ComputeShader csSetupRT;

		public void SetupChartRT(RenderTexture _rt)
		{
			if (!(_rt == null))
			{
				int kernelIndex = csSetupRT.FindKernel("SetupRT");
				csSetupRT.GetKernelThreadGroupSizes(kernelIndex, out var x, out var y, out var _);
				int threadGroupsX = Mathf.CeilToInt((float)_rt.width / (float)x);
				int threadGroupsY = Mathf.CeilToInt((float)_rt.height / (float)y);
				csSetupRT.SetTexture(kernelIndex, "renderTexture", _rt);
				csSetupRT.SetVector("resolution", _rt.PGetSize());
				csSetupRT.Dispatch(kernelIndex, threadGroupsX, threadGroupsY, 1);
			}
		}
	}
}
