using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct UpdateParticlesJob : IJobParallelForDefer
	{
		[ReadOnly]
		public NativeArray<float4> positions;

		[ReadOnly]
		public NativeArray<quaternion> orientations;

		[ReadOnly]
		public NativeArray<float4> velocities;

		[ReadOnly]
		public NativeArray<float4> angularVelocities;

		[ReadOnly]
		public NativeArray<float4> principalRadii;

		[ReadOnly]
		public NativeArray<float4> fluidData;

		[ReadOnly]
		public NativeArray<float4> fluidMaterial;

		[ReadOnly]
		public NativeArray<int> simplices;

		[ReadOnly]
		public SimplexCounts simplexCounts;

		[ReadOnly]
		public NativeMultilevelGrid<int> grid;

		[DeallocateOnJobCompletion]
		[ReadOnly]
		public NativeArray<int> gridLevels;

		[ReadOnly]
		public Poly6Kernel densityKernel;

		[ReadOnly]
		public NativeArray<float4> inputPositions;

		[ReadOnly]
		public NativeArray<float4> inputVelocities;

		[ReadOnly]
		public NativeArray<float4> inputColors;

		[ReadOnly]
		public NativeArray<float4> inputAttributes;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> outputPositions;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> outputVelocities;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> outputColors;

		[NativeDisableParallelForRestriction]
		public NativeArray<float4> outputAttributes;

		[NativeDisableParallelForRestriction]
		public NativeArray<int> dispatchBuffer;

		[ReadOnly]
		public Oni.SolverParameters parameters;

		public float3 agingOverPopulation;

		public int minFluidNeighbors;

		public float deltaTime;

		public int currentAliveParticles;

		private static readonly int4[] offsets = new int4[8]
		{
			new int4(0, 0, 0, 1),
			new int4(1, 0, 0, 1),
			new int4(0, 1, 0, 1),
			new int4(1, 1, 0, 1),
			new int4(0, 0, 1, 1),
			new int4(1, 0, 1, 1),
			new int4(0, 1, 1, 1),
			new int4(1, 1, 1, 1)
		};

		public unsafe void Execute(int i)
		{
			int* unsafePtr = (int*)dispatchBuffer.GetUnsafePtr();
			int num = Interlocked.Add(ref unsafePtr[3], -1);
			if (num >= inputPositions.Length || !(inputAttributes[num].x > 0f))
			{
				return;
			}
			int index = Interlocked.Add(ref unsafePtr[7], 1) - 1;
			float4 value = inputAttributes[num];
			float4 float5 = BurstMath.UnpackFloatRGBA(value.w);
			int num2 = ((parameters.mode == Oni.SolverParameters.Mode.Mode2D) ? 4 : 8);
			float4 zero = float4.zero;
			float4 zero2 = float4.zero;
			float num3 = 0f - float5.w;
			uint num4 = 0u;
			float4 float6 = inputPositions[num];
			for (int j = 0; j < gridLevels.Length; j++)
			{
				int num5 = gridLevels[j];
				float num6 = NativeMultilevelGrid<int>.CellSizeOfLevel(num5);
				float num7 = num6 * 0.5f;
				float4 float7 = math.floor(float6 / num6);
				float7[3] = 0f;
				if (parameters.mode == Oni.SolverParameters.Mode.Mode2D)
				{
					float7[2] = 0f;
				}
				int4 int5 = (int4)math.sign(float6 - (float7 * num6 + new float4(num7)));
				int5[3] = num5;
				for (int k = 0; k < num2; k++)
				{
					if (!grid.TryGetCellIndex((int4)float7 + offsets[k] * int5, out var cellIndex))
					{
						continue;
					}
					NativeMultilevelGrid<int>.Cell<int> cell = grid.usedCells[cellIndex];
					for (int l = 0; l < cell.Length; l++)
					{
						int size;
						int simplexStartAndSize = simplexCounts.GetSimplexStartAndSize(cell[l], out size);
						for (int m = 0; m < size; m++)
						{
							int index2 = simplices[simplexStartAndSize + m];
							float4 x = float6 - positions[index2];
							x[3] = 0f;
							if (parameters.mode == Oni.SolverParameters.Mode.Mode2D)
							{
								x[2] = 0f;
							}
							float num8 = math.length(x);
							if (num8 <= num7)
							{
								float3 float8 = fluidMaterial[index2].x * (principalRadii[index2].xyz / principalRadii[index2].x);
								float4 float9 = new float4(math.cross(angularVelocities[index2].xyz, x.xyz), 0f);
								zero2 += float9 * densityKernel.W(num8, float8.x) / densityKernel.W(0f, float8.x);
								x.xyz = math.mul(math.conjugate(orientations[index2]), x.xyz) / float8;
								num8 = math.length(x) * float8.x;
								float num9 = densityKernel.W(num8, float8.x) / fluidData[index2].x;
								num3 += num9;
								zero += velocities[index2] * num9;
								num4++;
							}
						}
					}
				}
			}
			float4 float10 = float4.zero;
			float num10 = 1f;
			float num11 = 1f + BurstMath.Remap01((float)currentAliveParticles / (float)inputPositions.Length, agingOverPopulation.x, agingOverPopulation.y) * (agingOverPopulation.z - 1f);
			if (num3 > 1E-07f && num4 >= minFluidNeighbors)
			{
				float10 = float5.z / deltaTime * (zero / (num3 + float5.w) + zero2 - inputVelocities[num]);
				float10 -= new float4(parameters.gravity * parameters.foamGravityScale * inputVelocities[num].w * math.saturate(num3), 0f);
			}
			else
			{
				float10 += new float4(parameters.gravity * parameters.foamGravityScale, 0f);
				num10 = float5.y;
				num11 *= float5.x * 50f;
			}
			float10[3] = 0f;
			value.x -= value.y * deltaTime * num11;
			outputAttributes[index] = value;
			outputColors[index] = inputColors[num];
			outputVelocities[index] = (inputVelocities[num] + float10 * deltaTime) * num10;
			outputPositions[index] = new float4((inputPositions[num] + outputVelocities[index] * deltaTime).xyz, num4);
		}
	}
}
