using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public struct ConstraintBatcher<T> : IDisposable where T : struct, IConstraintProvider
	{
		[BurstCompile]
		private struct BatchContactsJob : IJob
		{
			[DeallocateOnJobCompletion]
			public NativeArray<ushort> batchMasks;

			[DeallocateOnJobCompletion]
			public NativeArray<int> batchIndices;

			[ReadOnly]
			public BatchLUT lut;

			public T constraintDesc;

			public NativeArray<BatchData> batchData;

			public NativeArray<int> activeBatchCount;

			public int maxBatches;

			public unsafe void Execute()
			{
				for (int i = 0; i < batchData.Length; i++)
				{
					batchData[i] = new BatchData(i, maxBatches);
				}
				WorkItem* ptr = stackalloc WorkItem[maxBatches];
				for (int j = 0; j < maxBatches; j++)
				{
					ptr[j] = default(WorkItem);
				}
				int constraintCount = constraintDesc.GetConstraintCount();
				for (int k = 0; k < constraintCount; k++)
				{
					int num = 0;
					for (int l = 0; l < constraintDesc.GetParticleCount(k); l++)
					{
						num |= batchMasks[constraintDesc.GetParticle(k, l)];
					}
					int num2 = (batchIndices[k] = lut.batchIndex[num]);
					int num4 = num2;
					BatchData value = batchData[num4];
					value.constraintCount++;
					batchData[num4] = value;
					if (!ptr[num4].Add(k))
					{
						continue;
					}
					if (num4 != maxBatches - 1)
					{
						for (int m = 0; m < ptr[num4].constraintCount; m++)
						{
							int constraintIndex = ptr[num4].constraints[m];
							for (int n = 0; n < constraintDesc.GetParticleCount(constraintIndex); n++)
							{
								batchMasks[constraintDesc.GetParticle(constraintIndex, n)] |= value.batchID;
							}
						}
					}
					ptr[num4].constraintCount = 0;
				}
				activeBatchCount[0] = 0;
				int num5 = 0;
				for (int num6 = 0; num6 < batchData.Length; num6++)
				{
					BatchData value2 = batchData[num6];
					if (value2.constraintCount == 0)
					{
						break;
					}
					value2.workItemSize = math.min(64, value2.constraintCount);
					value2.workItemCount = (value2.constraintCount + value2.workItemSize - 1) / value2.workItemSize;
					value2.startIndex = num5;
					num5 += value2.constraintCount;
					activeBatchCount[0]++;
					batchData[num6] = value2;
				}
				for (int num7 = 0; num7 < constraintCount; num7++)
				{
					BatchData value3 = batchData[batchIndices[num7]];
					int sortedIndex = value3.startIndex + value3.activeConstraintCount++;
					constraintDesc.WriteSortedConstraint(num7, sortedIndex);
					batchData[batchIndices[num7]] = value3;
				}
			}
		}

		public int maxBatches;

		private BatchLUT batchLUT;

		public ConstraintBatcher(int maxBatches)
		{
			this.maxBatches = math.min(17, maxBatches);
			batchLUT = new BatchLUT(this.maxBatches);
		}

		public void Dispose()
		{
			batchLUT.Dispose();
		}

		public JobHandle BatchConstraints(ref T constraintDesc, int particleCount, ref NativeArray<BatchData> batchData, ref NativeArray<int> activeBatchCount, JobHandle inputDeps)
		{
			if (activeBatchCount.Length != 1)
			{
				return inputDeps;
			}
			return new BatchContactsJob
			{
				batchMasks = new NativeArray<ushort>(particleCount, Allocator.TempJob),
				batchIndices = new NativeArray<int>(constraintDesc.GetConstraintCount(), Allocator.TempJob),
				lut = batchLUT,
				constraintDesc = constraintDesc,
				batchData = batchData,
				activeBatchCount = activeBatchCount,
				maxBatches = maxBatches
			}.Schedule(inputDeps);
		}
	}
}
