using UnityEngine;

namespace Obi
{
	public class ComputeSort
	{
		private ComputeShader sortShader;

		private int sortKernel;

		public ComputeSort()
		{
			sortShader = Resources.Load<ComputeShader>("Compute/BitonicSort");
			sortKernel = sortShader.FindKernel("BitonicSort");
		}

		public void Sort(GraphicsBuffer keys, GraphicsBuffer values)
		{
			if (keys.count != values.count)
			{
				return;
			}
			sortShader.SetInt("numEntries", keys.count);
			sortShader.SetBuffer(sortKernel, "Keys", keys);
			sortShader.SetBuffer(sortKernel, "Values", values);
			int num = keys.count.CeilToPowerOfTwo() / 2;
			int num2 = (int)Mathf.Log(num * 2, 2f);
			int threadGroupsX = ComputeMath.ThreadGroupCount(num, 128);
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < i + 1; j++)
				{
					int num3 = 1 << i - j;
					int val = 2 * num3 - 1;
					sortShader.SetInt("groupWidth", num3);
					sortShader.SetInt("groupHeight", val);
					sortShader.SetInt("stepIndex", j);
					sortShader.Dispatch(sortKernel, threadGroupsX, 1, 1);
				}
			}
		}
	}
}
