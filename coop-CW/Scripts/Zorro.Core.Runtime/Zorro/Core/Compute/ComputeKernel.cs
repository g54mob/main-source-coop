using Unity.Mathematics;
using UnityEngine;

namespace Zorro.Core.Compute
{
	public class ComputeKernel
	{
		public int kernelID;

		private ComputeShader m_computeShader;

		public ComputeKernel(ComputeShader computeShader, string kernelName)
		{
			kernelID = computeShader.FindKernel(kernelName);
			m_computeShader = computeShader;
		}

		public void Dispatch(int3 dispatchSize)
		{
			m_computeShader.Dispatch(kernelID, dispatchSize.x, dispatchSize.y, dispatchSize.z);
		}
	}
}
