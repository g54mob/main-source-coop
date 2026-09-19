using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct CalculateSimplexBoundsJob : IJobParallelFor
	{
		[ReadOnly]
		public NativeArray<float4> radii;

		[ReadOnly]
		public NativeArray<float4> fluidMaterials;

		[ReadOnly]
		public NativeArray<float4> positions;

		[ReadOnly]
		public NativeArray<float4> velocities;

		[ReadOnly]
		public NativeArray<int> simplices;

		[ReadOnly]
		public SimplexCounts simplexCounts;

		[ReadOnly]
		public NativeArray<int> particleMaterialIndices;

		[ReadOnly]
		public NativeArray<BurstCollisionMaterial> collisionMaterials;

		public NativeArray<BurstAabb> simplexBounds;

		public NativeArray<BurstAabb> reducedBounds;

		[ReadOnly]
		public Oni.SolverParameters parameters;

		[ReadOnly]
		public float dt;

		public void Execute(int i)
		{
			int size;
			int simplexStartAndSize = simplexCounts.GetSimplexStartAndSize(i, out size);
			BurstAabb value = new BurstAabb(float.MaxValue, float.MinValue);
			BurstAabb value2 = new BurstAabb(float.MaxValue, float.MinValue);
			for (int j = 0; j < size; j++)
			{
				int index = simplices[simplexStartAndSize + j];
				int num = particleMaterialIndices[index];
				float num2 = radii[index].x + ((num >= 0) ? collisionMaterials[num].stickDistance : 0f);
				value.EncapsulateParticle(positions[index], BurstIntegration.IntegrateLinear(positions[index], velocities[index], dt * parameters.particleCCD), math.max(num2, fluidMaterials[index].x * 0.5f) + parameters.collisionMargin);
				value2.EncapsulateParticle(positions[index], BurstIntegration.IntegrateLinear(positions[index], velocities[index], dt), num2);
			}
			simplexBounds[i] = value;
			reducedBounds[i] = value2;
		}
	}
}
