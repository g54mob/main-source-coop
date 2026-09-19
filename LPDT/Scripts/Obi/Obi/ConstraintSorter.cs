using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class ConstraintSorter<T> where T : unmanaged, IConstraint
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct ConstraintComparer<K> : IComparer<K> where K : IConstraint
		{
			public int Compare(K x, K y)
			{
				return x.GetParticle(1).CompareTo(y.GetParticle(1));
			}
		}

		[BurstCompile]
		public struct CountSortPerFirstParticleJob : IJob
		{
			[ReadOnly]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<T> input;

			public NativeArray<T> output;

			[NativeDisableContainerSafetyRestriction]
			public NativeArray<int> digitCount;

			public int maxDigits;

			public int maxIndex;

			public void Execute()
			{
				int num = (1 << maxDigits) - 1;
				for (int i = 0; i < input.Length; i++)
				{
					digitCount[input[i].GetParticle(0) & num]++;
				}
				int num2 = digitCount[0];
				digitCount[0] = 0;
				for (int j = 1; j <= maxIndex; j++)
				{
					int num3 = digitCount[j];
					digitCount[j] = digitCount[j - 1] + num2;
					num2 = num3;
				}
				for (int k = 0; k < input.Length; k++)
				{
					int num4 = digitCount[input[k].GetParticle(0) & num]++;
					if (num4 == 1 && input.Length == 1)
					{
						output[0] = input[0];
					}
					output[num4] = input[k];
				}
			}
		}

		[BurstCompile]
		public struct SortSubArraysJob : IJobParallelFor
		{
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<T> InOutArray;

			[NativeDisableContainerSafetyRestriction]
			[DeallocateOnJobCompletion]
			public NativeArray<int> NextElementIndex;

			[ReadOnly]
			public ConstraintComparer<T> comparer;

			public void Execute(int workItemIndex)
			{
				int num = 0;
				if (workItemIndex > 0)
				{
					num = NextElementIndex[workItemIndex - 1];
				}
				if (num < InOutArray.Length)
				{
					int length = NextElementIndex[workItemIndex] - num;
					DefaultSortOfSubArrays(InOutArray, num, length, comparer);
				}
			}

			public static void DefaultSortOfSubArrays(NativeArray<T> inOutArray, int startIndex, int length, ConstraintComparer<T> comparer)
			{
				if (length > 2)
				{
					inOutArray.Slice(startIndex, length).Sort(comparer);
				}
				else if (length == 2 && inOutArray[startIndex].GetParticle(1) > inOutArray[startIndex + 1].GetParticle(1))
				{
					T value = inOutArray[startIndex + 1];
					inOutArray[startIndex + 1] = inOutArray[startIndex];
					inOutArray[startIndex] = value;
				}
			}
		}

		public JobHandle SortConstraints(int particleCount, NativeArray<T> constraints, ref NativeArray<T> sortedConstraints, JobHandle handle)
		{
			NativeArray<int> nativeArray = new NativeArray<int>(particleCount + 1, Allocator.TempJob);
			int num = 0;
			int num2 = particleCount - 1;
			int num3 = num2;
			while (num3 > 0)
			{
				num3 >>= 1;
				num++;
			}
			handle = new CountSortPerFirstParticleJob
			{
				input = constraints,
				output = sortedConstraints,
				maxDigits = num,
				maxIndex = num2,
				digitCount = nativeArray
			}.Schedule(handle);
			int innerloopBatchCount = math.max(1, num2 / 32);
			handle = IJobParallelForExtensions.Schedule(new SortSubArraysJob
			{
				InOutArray = sortedConstraints,
				NextElementIndex = nativeArray,
				comparer = default(ConstraintComparer<T>)
			}, nativeArray.Length, innerloopBatchCount, handle);
			return handle;
		}
	}
}
