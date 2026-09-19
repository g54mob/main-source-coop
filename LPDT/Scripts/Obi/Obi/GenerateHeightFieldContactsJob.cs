using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct GenerateHeightFieldContactsJob : IJobParallelFor
	{
		[ReadOnly]
		public NativeList<Oni.ContactPair> contactPairs;

		[ReadOnly]
		public NativeArray<float4> velocities;

		[ReadOnly]
		public NativeArray<float4> positions;

		[ReadOnly]
		public NativeArray<quaternion> orientations;

		[ReadOnly]
		public NativeArray<float> invMasses;

		[ReadOnly]
		public NativeArray<float4> radii;

		[ReadOnly]
		public NativeArray<int> simplices;

		[ReadOnly]
		public SimplexCounts simplexCounts;

		[ReadOnly]
		public NativeArray<BurstAabb> simplexBounds;

		[ReadOnly]
		public NativeArray<BurstAffineTransform> transforms;

		[ReadOnly]
		public NativeArray<BurstColliderShape> shapes;

		[ReadOnly]
		public NativeArray<BurstRigidbody> rigidbodies;

		[ReadOnly]
		public NativeArray<HeightFieldHeader> heightFieldHeaders;

		[ReadOnly]
		public NativeArray<float> heightFieldSamples;

		[WriteOnly]
		[NativeDisableParallelForRestriction]
		public NativeQueue<BurstContact>.ParallelWriter contactsQueue;

		[ReadOnly]
		public int firstPair;

		[ReadOnly]
		public BurstInertialFrame solverToWorld;

		[ReadOnly]
		public BurstAffineTransform worldToSolver;

		[ReadOnly]
		public float deltaTime;

		[ReadOnly]
		public Oni.SolverParameters parameters;

		public void Execute(int i)
		{
			int bodyA = contactPairs[firstPair + i].bodyA;
			int bodyB = contactPairs[firstPair + i].bodyB;
			BurstColliderShape burstColliderShape = shapes[bodyB];
			if (burstColliderShape.dataIndex < 0)
			{
				return;
			}
			HeightFieldHeader heightFieldHeader = heightFieldHeaders[burstColliderShape.dataIndex];
			int rigidbodyIndex = shapes[bodyB].rigidbodyIndex;
			int size;
			int simplexStartAndSize = simplexCounts.GetSimplexStartAndSize(bodyA, out size);
			BurstAabb burstAabb = simplexBounds[bodyA];
			BurstAffineTransform colliderToSolver = worldToSolver * transforms[bodyB];
			BurstAabb burstAabb2 = burstAabb.Transformed(math.inverse(float4x4.TRS(colliderToSolver.translation.xyz, colliderToSolver.rotation, colliderToSolver.scale.xyz)));
			BurstHeightField function = new BurstHeightField
			{
				colliderToSolver = colliderToSolver,
				shape = shapes[bodyB],
				header = heightFieldHeaders[shapes[bodyB].dataIndex],
				heightFieldSamples = heightFieldSamples
			};
			float4 zero = float4.zero;
			BurstContact value = new BurstContact
			{
				bodyA = bodyA,
				bodyB = bodyB
			};
			int num = (int)burstColliderShape.center.x;
			int num2 = (int)burstColliderShape.center.y;
			float num3 = burstColliderShape.size.x / (float)(num - 1);
			float num4 = burstColliderShape.size.z / (float)(num2 - 1);
			int2 int5 = new int2((int)math.floor(burstAabb2.min[0] / num3), (int)math.floor(burstAabb2.min[2] / num4));
			int2 int6 = new int2((int)math.floor(burstAabb2.max[0] / num3), (int)math.floor(burstAabb2.max[2] / num4));
			for (int j = int5[0]; j <= int6[0]; j++)
			{
				if (j < 0 || j >= num - 1)
				{
					continue;
				}
				for (int k = int5[1]; k <= int6[1]; k++)
				{
					if (k < 0 || k >= num2 - 1)
					{
						continue;
					}
					int num5 = math.clamp(j + 1, 0, num - 1);
					int num6 = math.clamp(k + 1, 0, num2 - 1);
					float num7 = heightFieldSamples[heightFieldHeader.firstSample + k * num + j] * burstColliderShape.size.y;
					float x = heightFieldSamples[heightFieldHeader.firstSample + k * num + num5] * burstColliderShape.size.y;
					float x2 = heightFieldSamples[heightFieldHeader.firstSample + num6 * num + j] * burstColliderShape.size.y;
					float x3 = heightFieldSamples[heightFieldHeader.firstSample + num6 * num + num5] * burstColliderShape.size.y;
					if (!(num7 < 0f))
					{
						num7 = math.abs(num7);
						x = math.abs(x);
						x2 = math.abs(x2);
						x3 = math.abs(x3);
						float x4 = (float)j * burstColliderShape.size.x / (float)(num - 1);
						float x5 = (float)num5 * burstColliderShape.size.x / (float)(num - 1);
						float z = (float)k * burstColliderShape.size.z / (float)(num2 - 1);
						float z2 = (float)num6 * burstColliderShape.size.z / (float)(num2 - 1);
						float4 convexBary = BurstMath.BarycenterForSimplexOfSize(size);
						float4 float5 = new float4(x4, x2, z2, 0f);
						float4 float6 = new float4(x5, x3, z2, 0f);
						float4 float7 = new float4(x4, num7, z, 0f);
						function.tri.Cache(float5, float6, float7);
						zero.xyz = math.normalizesafe(math.cross((float6 - float5).xyz, (float7 - float5).xyz));
						BurstLocalOptimization.SurfacePoint surfacePoint = BurstLocalOptimization.Optimize(ref function, positions, orientations, radii, simplices, simplexStartAndSize, size, ref convexBary, out var convexPoint, parameters.surfaceCollisionIterations, parameters.surfaceCollisionTolerance);
						float4 zero2 = float4.zero;
						float num8 = 0f;
						for (int l = 0; l < size; l++)
						{
							int index = simplices[simplexStartAndSize + l];
							num8 += radii[index].x * convexBary[l];
							zero2 += velocities[index] * convexBary[l];
						}
						float4 float8 = float4.zero;
						if (rigidbodyIndex >= 0)
						{
							float8 = BurstMath.GetRigidbodyVelocityAtPoint(rigidbodyIndex, surfacePoint.point, rigidbodies, solverToWorld);
						}
						float num9 = math.dot(convexPoint - surfacePoint.point, surfacePoint.normal);
						if (math.dot(zero2 - float8, surfacePoint.normal) * deltaTime + num9 <= num8 + burstColliderShape.contactOffset + parameters.collisionMargin)
						{
							value.pointB = surfacePoint.point;
							value.normal = surfacePoint.normal * function.shape.sign;
							value.pointA = convexBary;
							contactsQueue.Enqueue(value);
						}
						float5 = new float4(x4, num7, z, 0f);
						float6 = new float4(x5, x3, z2, 0f);
						float7 = new float4(x5, x, z, 0f);
						function.tri.Cache(float5, float6, float7);
						zero.xyz = math.normalizesafe(math.cross((float6 - float5).xyz, (float7 - float5).xyz));
						surfacePoint = BurstLocalOptimization.Optimize(ref function, positions, orientations, radii, simplices, simplexStartAndSize, size, ref convexBary, out convexPoint, parameters.surfaceCollisionIterations, parameters.surfaceCollisionTolerance);
						zero2 = float4.zero;
						num8 = 0f;
						for (int m = 0; m < size; m++)
						{
							int index2 = simplices[simplexStartAndSize + m];
							num8 += radii[index2].x * convexBary[m];
							zero2 += velocities[index2] * convexBary[m];
						}
						float8 = float4.zero;
						if (rigidbodyIndex >= 0)
						{
							float8 = BurstMath.GetRigidbodyVelocityAtPoint(rigidbodyIndex, surfacePoint.point, rigidbodies, solverToWorld);
						}
						num9 = math.dot(convexPoint - surfacePoint.point, surfacePoint.normal);
						if (math.dot(zero2 - float8, surfacePoint.normal) * deltaTime + num9 <= num8 + burstColliderShape.contactOffset + parameters.collisionMargin)
						{
							value.pointB = surfacePoint.point;
							value.normal = surfacePoint.normal * function.shape.sign;
							value.pointA = convexBary;
							contactsQueue.Enqueue(value);
						}
					}
				}
			}
		}
	}
}
