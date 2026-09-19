using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Obi
{
	public class BurstColliderWorld : MonoBehaviour, IColliderWorldImpl
	{
		private struct MovingCollider
		{
			public BurstCellSpan oldSpan;

			public BurstCellSpan newSpan;

			public int entity;
		}

		[BurstCompile]
		private struct IdentifyMovingColliders : IJobParallelFor
		{
			[WriteOnly]
			[NativeDisableParallelForRestriction]
			public NativeQueue<MovingCollider>.ParallelWriter movingColliders;

			[ReadOnly]
			public NativeArray<BurstColliderShape> shapes;

			[ReadOnly]
			public NativeArray<BurstRigidbody> rigidbodies;

			[ReadOnly]
			public NativeArray<BurstCollisionMaterial> collisionMaterials;

			public NativeArray<BurstAabb> bounds;

			public NativeArray<BurstCellSpan> cellIndices;

			[ReadOnly]
			public int colliderCount;

			[ReadOnly]
			public float dt;

			public void Execute(int i)
			{
				BurstAabb burstAabb = bounds[i];
				int rigidbodyIndex = shapes[i].rigidbodyIndex;
				if (rigidbodyIndex >= 0 && rigidbodyIndex < rigidbodies.Length)
				{
					burstAabb.Sweep(rigidbodies[rigidbodyIndex].velocity * dt);
				}
				if (shapes[i].materialIndex >= 0)
				{
					burstAabb.Expand(collisionMaterials[shapes[i].materialIndex].stickDistance);
				}
				int num = NativeMultilevelGrid<int>.GridLevelForSize(burstAabb.AverageAxisLength());
				float cellSize = NativeMultilevelGrid<int>.CellSizeOfLevel(num);
				BurstCellSpan burstCellSpan = new BurstCellSpan(new int4(GridHash.Quantize(burstAabb.min.xyz, cellSize), num), new int4(GridHash.Quantize(burstAabb.max.xyz, cellSize), num));
				if (shapes[i].is2D)
				{
					burstCellSpan.min[2] = 0;
					burstCellSpan.max[2] = 0;
				}
				if (i >= colliderCount || cellIndices[i] != burstCellSpan)
				{
					movingColliders.Enqueue(new MovingCollider
					{
						oldSpan = cellIndices[i],
						newSpan = burstCellSpan,
						entity = i
					});
					cellIndices[i] = burstCellSpan;
				}
			}
		}

		[BurstCompile]
		private struct UpdateMovingColliders : IJob
		{
			public NativeQueue<MovingCollider> movingColliders;

			public NativeMultilevelGrid<int> grid;

			[ReadOnly]
			public int colliderCount;

			public void Execute()
			{
				while (movingColliders.Count > 0)
				{
					MovingCollider movingCollider = movingColliders.Dequeue();
					grid.RemoveFromCells(movingCollider.oldSpan, movingCollider.entity);
					if (movingCollider.entity < colliderCount)
					{
						grid.AddToCells(movingCollider.newSpan, movingCollider.entity);
					}
				}
				grid.RemoveEmpty();
			}
		}

		[BurstCompile]
		private struct GenerateContactsJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeMultilevelGrid<int> colliderGrid;

			[DeallocateOnJobCompletion]
			[ReadOnly]
			public NativeArray<int> gridLevels;

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
			public NativeArray<int> filters;

			[ReadOnly]
			public NativeArray<int> particleMaterialIndices;

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
			public NativeArray<BurstCollisionMaterial> collisionMaterials;

			[ReadOnly]
			public NativeArray<BurstRigidbody> rigidbodies;

			[ReadOnly]
			public NativeArray<BurstAabb> bounds;

			[WriteOnly]
			[NativeDisableParallelForRestriction]
			public NativeQueue<Oni.ContactPair>.ParallelWriter contactPairQueue;

			[NativeDisableParallelForRestriction]
			public NativeArray<int> colliderTypeCounts;

			[ReadOnly]
			public BurstAffineTransform solverToWorld;

			[ReadOnly]
			public float deltaTime;

			[ReadOnly]
			public Oni.SolverParameters parameters;

			public unsafe void Execute(int i)
			{
				int size;
				int simplexStartAndSize = simplexCounts.GetSimplexStartAndSize(i, out size);
				BurstAabb burstAabb = simplexBounds[i].Transformed(in solverToWorld);
				NativeList<int> nativeList = new NativeList<int>(16, Allocator.Temp);
				int3 y = new int3(10);
				bool flag = parameters.mode == Oni.SolverParameters.Mode.Mode2D;
				for (int j = 0; j < gridLevels.Length; j++)
				{
					float cellSize = NativeMultilevelGrid<int>.CellSizeOfLevel(gridLevels[j]);
					int3 int5 = GridHash.Quantize(burstAabb.min.xyz, cellSize);
					int3 int6 = GridHash.Quantize(burstAabb.max.xyz, cellSize);
					int6 = int5 + math.min(int6 - int5, y);
					for (int k = int5[0]; k <= int6[0]; k++)
					{
						for (int l = int5[1]; l <= int6[1]; l++)
						{
							if (flag && colliderGrid.TryGetCellIndex(new int4(k, l, 0, gridLevels[j]), out var cellIndex))
							{
								NativeMultilevelGrid<int>.Cell<int> cell = colliderGrid.usedCells[cellIndex];
								nativeList.AddRange(cell.ContentsPointer, cell.Length);
							}
							for (int m = int5[2]; m <= int6[2]; m++)
							{
								if (colliderGrid.TryGetCellIndex(new int4(k, l, m, gridLevels[j]), out var cellIndex2))
								{
									NativeMultilevelGrid<int>.Cell<int> cell2 = colliderGrid.usedCells[cellIndex2];
									nativeList.AddRange(cell2.ContentsPointer, cell2.Length);
								}
							}
						}
					}
				}
				if (nativeList.Length <= 0)
				{
					return;
				}
				NativeArray<int> array = nativeList.AsArray();
				array.Sort();
				int num = array.Unique();
				for (int n = 0; n < num; n++)
				{
					int num2 = array[n];
					if (num2 < shapes.Length)
					{
						BurstColliderShape burstColliderShape = shapes[num2];
						BurstAabb burstAabb2 = bounds[num2];
						int rigidbodyIndex = burstColliderShape.rigidbodyIndex;
						if (rigidbodyIndex >= 0)
						{
							burstAabb2.Sweep(rigidbodies[rigidbodyIndex].velocity * deltaTime);
						}
						if (burstColliderShape.materialIndex >= 0)
						{
							burstAabb2.Expand(collisionMaterials[burstColliderShape.materialIndex].stickDistance);
						}
						bool flag2 = false;
						int num3 = burstColliderShape.filter & 0xFFFF;
						int num4 = (burstColliderShape.filter & -65536) >> 16;
						for (int num5 = 0; num5 < size; num5++)
						{
							int num6 = filters[simplices[simplexStartAndSize + num5]] & 0xFFFF;
							int num7 = (filters[simplices[simplexStartAndSize + num5]] & -65536) >> 16;
							flag2 = flag2 || ((num6 & num4) != 0 && (num7 & num3) != 0);
						}
						if (flag2 && burstAabb.IntersectsAabb(in burstAabb2, flag))
						{
							Interlocked.Increment(ref *(int*)((byte*)colliderTypeCounts.GetUnsafePtr() + (nint)burstColliderShape.type * (nint)4));
							contactPairQueue.Enqueue(new Oni.ContactPair
							{
								bodyA = i,
								bodyB = num2
							});
						}
					}
				}
			}
		}

		[BurstCompile]
		private struct PrefixSumJob : IJob
		{
			[ReadOnly]
			public NativeArray<int> array;

			public NativeArray<int> sum;

			public void Execute()
			{
				sum[0] = 0;
				for (int i = 1; i < sum.Length; i++)
				{
					sum[i] = sum[i - 1] + array[i - 1];
				}
			}
		}

		[BurstCompile]
		private struct SortContactPairsByShape : IJob
		{
			public NativeQueue<Oni.ContactPair> contactPairQueue;

			[ReadOnly]
			public NativeArray<BurstColliderShape> shapes;

			[ReadOnly]
			public NativeArray<int> start;

			public NativeArray<int> count;

			public NativeList<Oni.ContactPair> contactPairs;

			public void Execute()
			{
				contactPairs.ResizeUninitialized(contactPairQueue.Count);
				while (!contactPairQueue.IsEmpty())
				{
					Oni.ContactPair value = contactPairQueue.Dequeue();
					int type = (int)shapes[value.bodyB].type;
					contactPairs[start[type] + --count[type]] = value;
				}
			}
		}

		[BurstCompile]
		private struct ApplyForceZonesJob : IJobParallelFor
		{
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> externalForces;

			[NativeDisableParallelForRestriction]
			public NativeArray<float4> wind;

			[NativeDisableParallelForRestriction]
			public NativeArray<float4> velocities;

			[NativeDisableParallelForRestriction]
			public NativeArray<float4> colors;

			[NativeDisableParallelForRestriction]
			public NativeArray<float> life;

			[ReadOnly]
			public NativeArray<float4> positions;

			[ReadOnly]
			public NativeArray<float> invMasses;

			[ReadOnly]
			public NativeArray<int> simplices;

			[ReadOnly]
			public SimplexCounts simplexCounts;

			[ReadOnly]
			public NativeArray<BurstAffineTransform> transforms;

			[ReadOnly]
			public NativeArray<BurstColliderShape> shapes;

			[ReadOnly]
			public NativeArray<ForceZone> forceZones;

			[ReadOnly]
			public NativeArray<BurstContact> contacts;

			[ReadOnly]
			public BurstAffineTransform worldToSolver;

			[ReadOnly]
			public float deltaTime;

			public void Execute(int i)
			{
				BurstContact burstContact = contacts[i];
				int forceZoneIndex = shapes[burstContact.bodyB].forceZoneIndex;
				if (forceZoneIndex < 0)
				{
					return;
				}
				int size;
				int simplexStartAndSize = simplexCounts.GetSimplexStartAndSize(burstContact.bodyA, out size);
				for (int j = 0; j < size; j++)
				{
					int num = simplices[simplexStartAndSize + j];
					float num2 = 0f - math.dot(positions[num] - burstContact.pointB, burstContact.normal);
					if (num2 < 0f)
					{
						continue;
					}
					float4 float5 = (worldToSolver * transforms[burstContact.bodyB]).TransformDirection(new float4(0f, 0f, 1f, 0f));
					float num3 = 1f;
					float num4 = forceZones[forceZoneIndex].maxDistance - forceZones[forceZoneIndex].minDistance;
					if (math.abs(num4) > 1E-07f)
					{
						num3 = math.pow(math.saturate((num2 - forceZones[forceZoneIndex].minDistance) / num4), forceZones[forceZoneIndex].falloffPower);
					}
					float num5 = forceZones[forceZoneIndex].intensity * num3;
					float num6 = forceZones[forceZoneIndex].damping * num3;
					float t = math.pow(1f - math.saturate(forceZones[forceZoneIndex].color.a * num3), deltaTime);
					colors[num] = math.lerp((Vector4)forceZones[forceZoneIndex].color, colors[num], t);
					float4 float6 = float4.zero;
					switch (forceZones[forceZoneIndex].type)
					{
					case ForceZone.ZoneType.Radial:
						float6 = burstContact.normal * num5;
						break;
					case ForceZone.ZoneType.Vortex:
						float6 = new float4(math.cross(float5.xyz * num5, burstContact.normal.xyz).xyz, 0f);
						break;
					case ForceZone.ZoneType.Directional:
						float6 = float5 * num5;
						break;
					default:
						BurstMath.AtomicAdd(life, num, (0f - num5) * deltaTime);
						continue;
					}
					switch (forceZones[forceZoneIndex].dampingDir)
					{
					case ForceZone.DampingDirection.ForceDirection:
					{
						float4 float7 = math.normalizesafe(float6);
						float6 -= float7 * math.dot(velocities[num], float7) * num6;
						break;
					}
					case ForceZone.DampingDirection.SurfaceDirection:
						float6 -= burstContact.normal * math.dot(velocities[num], burstContact.normal) * num6;
						break;
					default:
						float6 -= velocities[num] * num6;
						break;
					}
					if (invMasses[num] > 0f)
					{
						switch (forceZones[forceZoneIndex].mode)
						{
						case ForceZone.ForceMode.Acceleration:
							BurstMath.AtomicAdd(externalForces, num, float6 / size / invMasses[num]);
							break;
						case ForceZone.ForceMode.Force:
							BurstMath.AtomicAdd(externalForces, num, float6 / size);
							break;
						case ForceZone.ForceMode.Wind:
							BurstMath.AtomicAdd(wind, num, float6 / size);
							break;
						}
					}
				}
			}
		}

		private NativeMultilevelGrid<int> grid;

		private NativeQueue<MovingCollider> movingColliders;

		private NativeArray<int> colliderTypeCounts;

		private NativeQueue<Oni.ContactPair> contactPairQueue;

		public NativeList<Oni.ContactPair> contactPairs;

		public NativeArray<int> contactOffsetsPerType;

		public NativeQueue<BurstContact> colliderContactQueue;

		public ObiNativeCellSpanList cellSpans;

		public int referenceCount { get; private set; }

		public int colliderCount { get; private set; }

		public void Awake()
		{
			grid = new NativeMultilevelGrid<int>(1000, Allocator.Persistent);
			movingColliders = new NativeQueue<MovingCollider>(Allocator.Persistent);
			colliderContactQueue = new NativeQueue<BurstContact>(Allocator.Persistent);
			contactPairQueue = new NativeQueue<Oni.ContactPair>(Allocator.Persistent);
			colliderTypeCounts = new NativeArray<int>(7, Allocator.Persistent);
			contactOffsetsPerType = new NativeArray<int>(8, Allocator.Persistent);
			contactPairs = new NativeList<Oni.ContactPair>(Allocator.Persistent);
			cellSpans = new ObiNativeCellSpanList();
			ObiColliderWorld.GetInstance().RegisterImplementation(this);
		}

		public void OnDestroy()
		{
			ObiColliderWorld.GetInstance().UnregisterImplementation(this);
			grid.Dispose();
			movingColliders.Dispose();
			colliderTypeCounts.Dispose();
			contactPairQueue.Dispose();
			contactPairs.Dispose();
			contactOffsetsPerType.Dispose();
			colliderContactQueue.Dispose();
			cellSpans.Dispose();
		}

		public void IncreaseReferenceCount()
		{
			referenceCount++;
		}

		public void DecreaseReferenceCount()
		{
			if (--referenceCount <= 0 && base.gameObject != null)
			{
				Object.DestroyImmediate(base.gameObject);
			}
		}

		public void SetColliders(ObiNativeColliderShapeList shapes, ObiNativeAabbList bounds, ObiNativeAffineTransformList transforms)
		{
			colliderCount = shapes.count;
			while (colliderCount > cellSpans.count)
			{
				cellSpans.Add(new CellSpan(new VInt4(10000), new VInt4(10000)));
			}
		}

		public void SetRigidbodies(ObiNativeRigidbodyList rigidbody)
		{
		}

		public void SetForceZones(ObiNativeForceZoneList rigidbody)
		{
		}

		public void SetCollisionMaterials(ObiNativeCollisionMaterialList materials)
		{
		}

		public void SetTriangleMeshData(ObiNativeTriangleMeshHeaderList headers, ObiNativeBIHNodeList nodes, ObiNativeTriangleList triangles, ObiNativeVector3List vertices)
		{
		}

		public void SetEdgeMeshData(ObiNativeEdgeMeshHeaderList headers, ObiNativeBIHNodeList nodes, ObiNativeEdgeList edges, ObiNativeVector2List vertices)
		{
		}

		public void SetDistanceFieldData(ObiNativeDistanceFieldHeaderList headers, ObiNativeDFNodeList nodes)
		{
		}

		public void SetHeightFieldData(ObiNativeHeightFieldHeaderList headers, ObiNativeFloatList samples)
		{
		}

		public void UpdateWorld(float deltaTime)
		{
			ObiColliderWorld instance = ObiColliderWorld.GetInstance();
			JobHandle dependsOn = IJobParallelForExtensions.Schedule(new IdentifyMovingColliders
			{
				movingColliders = movingColliders.AsParallelWriter(),
				shapes = instance.colliderShapes.AsNativeArray<BurstColliderShape>(cellSpans.count),
				rigidbodies = instance.rigidbodies.AsNativeArray<BurstRigidbody>(),
				collisionMaterials = instance.collisionMaterials.AsNativeArray<BurstCollisionMaterial>(),
				bounds = instance.colliderAabbs.AsNativeArray<BurstAabb>(cellSpans.count),
				cellIndices = cellSpans.AsNativeArray<BurstCellSpan>(),
				colliderCount = colliderCount,
				dt = deltaTime
			}, cellSpans.count, 128);
			new UpdateMovingColliders
			{
				movingColliders = movingColliders,
				grid = grid,
				colliderCount = colliderCount
			}.Schedule(dependsOn).Complete();
			if (colliderCount < cellSpans.count)
			{
				cellSpans.count -= cellSpans.count - colliderCount;
			}
		}

		public JobHandle ApplyForceZones(BurstSolverImpl solver, float deltaTime, JobHandle inputDeps)
		{
			ObiColliderWorld instance = ObiColliderWorld.GetInstance();
			return IJobParallelForExtensions.Schedule(new ApplyForceZonesJob
			{
				contacts = solver.abstraction.colliderContacts.AsNativeArray<BurstContact>(),
				positions = solver.positions,
				velocities = solver.velocities,
				externalForces = solver.externalForces,
				wind = solver.wind,
				invMasses = solver.invMasses,
				life = solver.life,
				colors = solver.colors,
				simplices = solver.simplices,
				simplexCounts = solver.simplexCounts,
				transforms = instance.colliderTransforms.AsNativeArray<BurstAffineTransform>(),
				shapes = instance.colliderShapes.AsNativeArray<BurstColliderShape>(),
				forceZones = instance.forceZones.AsNativeArray<ForceZone>(),
				worldToSolver = solver.worldToSolver,
				deltaTime = deltaTime
			}, solver.abstraction.colliderContacts.count, 64, inputDeps);
		}

		public JobHandle GenerateContacts(BurstSolverImpl solver, float deltaTime, JobHandle inputDeps)
		{
			ObiColliderWorld instance = ObiColliderWorld.GetInstance();
			inputDeps = IJobParallelForExtensions.Schedule(new GenerateContactsJob
			{
				colliderGrid = grid,
				gridLevels = grid.populatedLevels.GetKeyArray(Allocator.TempJob),
				positions = solver.positions,
				orientations = solver.orientations,
				velocities = solver.velocities,
				invMasses = solver.invMasses,
				radii = solver.principalRadii,
				filters = solver.filters,
				particleMaterialIndices = solver.collisionMaterials,
				simplices = solver.simplices,
				simplexCounts = solver.simplexCounts,
				simplexBounds = solver.simplexBounds,
				transforms = instance.colliderTransforms.AsNativeArray<BurstAffineTransform>(),
				shapes = instance.colliderShapes.AsNativeArray<BurstColliderShape>(),
				rigidbodies = instance.rigidbodies.AsNativeArray<BurstRigidbody>(),
				collisionMaterials = instance.collisionMaterials.AsNativeArray<BurstCollisionMaterial>(),
				bounds = instance.colliderAabbs.AsNativeArray<BurstAabb>(),
				contactPairQueue = contactPairQueue.AsParallelWriter(),
				colliderTypeCounts = colliderTypeCounts,
				solverToWorld = solver.solverToWorld,
				deltaTime = deltaTime,
				parameters = solver.abstraction.parameters
			}, solver.simplexCounts.simplexCount, 16, inputDeps);
			inputDeps = new PrefixSumJob
			{
				array = colliderTypeCounts,
				sum = contactOffsetsPerType
			}.Schedule(inputDeps);
			inputDeps = new SortContactPairsByShape
			{
				contactPairQueue = contactPairQueue,
				shapes = instance.colliderShapes.AsNativeArray<BurstColliderShape>(),
				start = contactOffsetsPerType,
				count = colliderTypeCounts,
				contactPairs = contactPairs
			}.Schedule(inputDeps);
			inputDeps.Complete();
			inputDeps = BurstSphere.GenerateContacts(instance, solver, contactPairs, colliderContactQueue, contactOffsetsPerType, deltaTime, inputDeps);
			inputDeps = BurstBox.GenerateContacts(instance, solver, contactPairs, colliderContactQueue, contactOffsetsPerType, deltaTime, inputDeps);
			inputDeps = BurstCapsule.GenerateContacts(instance, solver, contactPairs, colliderContactQueue, contactOffsetsPerType, deltaTime, inputDeps);
			inputDeps = BurstDistanceField.GenerateContacts(instance, solver, contactPairs, colliderContactQueue, contactOffsetsPerType, deltaTime, inputDeps);
			inputDeps = BurstTriangleMesh.GenerateContacts(instance, solver, contactPairs, colliderContactQueue, contactOffsetsPerType, deltaTime, inputDeps);
			inputDeps = BurstHeightField.GenerateContacts(instance, solver, contactPairs, colliderContactQueue, contactOffsetsPerType, deltaTime, inputDeps);
			inputDeps = BurstEdgeMesh.GenerateContacts(instance, solver, contactPairs, colliderContactQueue, contactOffsetsPerType, deltaTime, inputDeps);
			return inputDeps;
		}
	}
}
