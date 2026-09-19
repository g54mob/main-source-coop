using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	public class ComputePrefixSum
	{
		private ComputeShader scanShader;

		private int scanInBucketKernel;

		private int scanAddBucketResult;

		private List<GraphicsBuffer> blockSums = new List<GraphicsBuffer>();

		private List<GraphicsBuffer> prefixBlockSums = new List<GraphicsBuffer>();

		private int inputSize;

		private const int threadsPerGroup = 512;

		public ComputePrefixSum(int inputSize)
		{
			scanShader = Resources.Load<ComputeShader>("Compute/Scan");
			scanInBucketKernel = scanShader.FindKernel("ScanInBucketExclusive");
			scanAddBucketResult = scanShader.FindKernel("ScanAddBucketResult");
			this.inputSize = inputSize;
			int num = inputSize;
			while (num > 1)
			{
				num = (num + 512 - 1) / 512;
				blockSums.Add(new GraphicsBuffer(GraphicsBuffer.Target.Structured, num, 4));
				prefixBlockSums.Add(new GraphicsBuffer(GraphicsBuffer.Target.Structured, num, 4));
			}
		}

		public void Dispose()
		{
			foreach (GraphicsBuffer blockSum in blockSums)
			{
				if (blockSum != null && blockSum.IsValid())
				{
					blockSum.Dispose();
				}
			}
			blockSums.Clear();
			foreach (GraphicsBuffer prefixBlockSum in prefixBlockSums)
			{
				if (prefixBlockSum != null && prefixBlockSum.IsValid())
				{
					prefixBlockSum.Dispose();
				}
			}
			prefixBlockSums.Clear();
		}

		public void Sum(GraphicsBuffer input, GraphicsBuffer result)
		{
			if (input.count == inputSize)
			{
				Sum(input, result, input.count, 0);
			}
		}

		private void Sum(GraphicsBuffer input, GraphicsBuffer result, int count, int level)
		{
			int num = (count + 512 - 1) / 512;
			scanShader.SetInt("count", count);
			scanShader.SetBuffer(scanInBucketKernel, "_Input", input);
			scanShader.SetBuffer(scanInBucketKernel, "_Result", result);
			scanShader.SetBuffer(scanInBucketKernel, "_BlockSum", blockSums[level]);
			scanShader.Dispatch(scanInBucketKernel, num, 1, 1);
			if (num > 1)
			{
				Sum(blockSums[level], prefixBlockSums[level], num, level + 1);
				scanShader.SetInt("count", count);
				scanShader.SetBuffer(scanAddBucketResult, "_Input", prefixBlockSums[level]);
				scanShader.SetBuffer(scanAddBucketResult, "_Result", result);
				scanShader.Dispatch(scanAddBucketResult, num, 1, 1);
			}
		}
	}
}
