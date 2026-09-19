using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class ParticleGrid : IDisposable
	{
		[BurstCompile]
		private struct UpdateGrid : IJob
		{
			public NativeMultilevelGrid<int> grid;

			[ReadOnly]
			public NativeArray<BurstAabb> simplexBounds;

			public NativeArray<int4> cellCoords;

			[ReadOnly]
			public Oni.SolverParameters parameters;

			[ReadOnly]
			public int simplexCount;

			public void Execute()
			{
				grid.Clear();
				for (int i = 0; i < simplexCount; i++)
				{
					int num = NativeMultilevelGrid<int>.GridLevelForSize(simplexBounds[i].MaxAxisLength());
					float cellSize = NativeMultilevelGrid<int>.CellSizeOfLevel(num);
					int4 value = new int4(GridHash.Quantize(simplexBounds[i].center.xyz, cellSize), num);
					if (parameters.mode == Oni.SolverParameters.Mode.Mode2D)
					{
						value[2] = 0;
					}
					cellCoords[i] = value;
					int orCreateCell = grid.GetOrCreateCell(cellCoords[i]);
					NativeMultilevelGrid<int>.Cell<int> value2 = grid.usedCells[orCreateCell];
					value2.Add(i);
					grid.usedCells[orCreateCell] = value2;
				}
			}
		}

		[BurstCompile]
		public struct GenerateParticleParticleContactsJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeMultilevelGrid<int> grid;

			[DeallocateOnJobCompletion]
			[ReadOnly]
			public NativeArray<int> gridLevels;

			[ReadOnly]
			public NativeArray<float4> positions;

			[ReadOnly]
			public NativeArray<quaternion> orientations;

			[ReadOnly]
			public NativeArray<float4> restPositions;

			[ReadOnly]
			public NativeArray<quaternion> restOrientations;

			[ReadOnly]
			public NativeArray<float4> velocities;

			[ReadOnly]
			public NativeArray<float> invMasses;

			[ReadOnly]
			public NativeArray<float4> radii;

			[ReadOnly]
			public NativeArray<float4> normals;

			[ReadOnly]
			public NativeArray<float4> fluidMaterials;

			[ReadOnly]
			public NativeArray<int> phases;

			[ReadOnly]
			public NativeArray<int> filters;

			[ReadOnly]
			public NativeArray<int> simplices;

			[ReadOnly]
			public SimplexCounts simplexCounts;

			[ReadOnly]
			public NativeArray<int> particleMaterialIndices;

			[ReadOnly]
			public NativeArray<BurstCollisionMaterial> collisionMaterials;

			[WriteOnly]
			[NativeDisableParallelForRestriction]
			public NativeQueue<BurstContact>.ParallelWriter contactsQueue;

			[WriteOnly]
			[NativeDisableParallelForRestriction]
			public NativeQueue<FluidInteraction>.ParallelWriter fluidInteractionsQueue;

			[ReadOnly]
			public float dt;

			[ReadOnly]
			public float collisionMargin;

			[ReadOnly]
			public int optimizationIterations;

			[ReadOnly]
			public float optimizationTolerance;

			public void Execute(int i)
			{
				BurstSimplex simplexShape = new BurstSimplex
				{
					positions = restPositions,
					radii = radii,
					simplices = simplices
				};
				IntraCellSearch(i, ref simplexShape);
				IntraLevelSearch(i, ref simplexShape);
			}

			private void IntraCellSearch(int cellIndex, ref BurstSimplex simplexShape)
			{
				int length = grid.usedCells[cellIndex].Length;
				for (int i = 0; i < length; i++)
				{
					for (int j = i + 1; j < length; j++)
					{
						InteractionTest(grid.usedCells[cellIndex][i], grid.usedCells[cellIndex][j], ref simplexShape);
					}
				}
			}

			private void InterCellSearch(int cellIndex, int neighborCellIndex, ref BurstSimplex simplexShape)
			{
				int length = grid.usedCells[cellIndex].Length;
				int length2 = grid.usedCells[neighborCellIndex].Length;
				for (int i = 0; i < length; i++)
				{
					for (int j = 0; j < length2; j++)
					{
						InteractionTest(grid.usedCells[cellIndex][i], grid.usedCells[neighborCellIndex][j], ref simplexShape);
					}
				}
			}

			private void IntraLevelSearch(int cellIndex, ref BurstSimplex simplexShape)
			{
				int4 coords = grid.usedCells[cellIndex].Coords;
				for (int i = 0; i < 13; i++)
				{
					int4 cellCoords = new int4(coords.xyz + GridHash.cellOffsets3D[i], coords.w);
					if (grid.TryGetCellIndex(cellCoords, out var cellIndex2))
					{
						InterCellSearch(cellIndex, cellIndex2, ref simplexShape);
					}
				}
				int num = gridLevels.IndexOf(coords.w);
				if (num < 0)
				{
					return;
				}
				for (num++; num < gridLevels.Length; num++)
				{
					int level = gridLevels[num];
					int4 parentCellCoords = NativeMultilevelGrid<int>.GetParentCellCoords(coords, level);
					for (int j = -1; j <= 1; j++)
					{
						for (int k = -1; k <= 1; k++)
						{
							for (int l = -1; l <= 1; l++)
							{
								int4 cellCoords2 = parentCellCoords + new int4(j, k, l, 0);
								if (grid.TryGetCellIndex(cellCoords2, out var cellIndex3))
								{
									InterCellSearch(cellIndex, cellIndex3, ref simplexShape);
								}
							}
						}
					}
				}
			}

			private int GetSimplexGroup(int simplexStart, int simplexSize, out ObiUtils.ParticleFlags flags, out int category, out int mask, ref bool restPositionsEnabled)
			{
				flags = (ObiUtils.ParticleFlags)0;
				int num = 0;
				category = 0;
				mask = 0;
				for (int i = 0; i < simplexSize; i++)
				{
					int index = simplices[simplexStart + i];
					num = math.max(num, ObiUtils.GetGroupFromPhase(phases[index]));
					flags |= ObiUtils.GetFlagsFromPhase(phases[index]);
					category |= filters[index] & 0xFFFF;
					mask |= (filters[index] & -65536) >> 16;
					restPositionsEnabled |= restPositions[index].w > 0.5f;
				}
				return num;
			}

			private void InteractionTest(int A, int B, ref BurstSimplex simplexShape)
			{
				int size;
				int lhs = simplexCounts.GetSimplexStartAndSize(A, out size);
				int size2;
				int rhs = simplexCounts.GetSimplexStartAndSize(B, out size2);
				for (int i = 0; i < size; i++)
				{
					for (int j = 0; j < size2; j++)
					{
						if (simplices[lhs + i] == simplices[rhs + j])
						{
							return;
						}
					}
				}
				bool restPositionsEnabled = false;
				ObiUtils.ParticleFlags flags;
				int category;
				int mask;
				int lhs2 = GetSimplexGroup(lhs, size, out flags, out category, out mask, ref restPositionsEnabled);
				ObiUtils.ParticleFlags flags2;
				int category2;
				int mask2;
				int rhs2 = GetSimplexGroup(rhs, size2, out flags2, out category2, out mask2, ref restPositionsEnabled);
				if (lhs2 == rhs2)
				{
					if ((flags & flags2 & ObiUtils.ParticleFlags.SelfCollide) == 0)
					{
						return;
					}
				}
				else if ((mask & category2) == 0 || (mask2 & category) == 0)
				{
					return;
				}
				if ((flags & ObiUtils.ParticleFlags.Fluid) != 0 && (flags2 & ObiUtils.ParticleFlags.Fluid) != 0)
				{
					int num = simplices[lhs];
					int num2 = simplices[rhs];
					float num3 = math.lengthsq(positions[num].xyz - positions[num2].xyz);
					float num4 = math.max(fluidMaterials[num].x, fluidMaterials[num2].x) + collisionMargin;
					if (num3 <= num4 * num4)
					{
						fluidInteractionsQueue.Enqueue(new FluidInteraction
						{
							particleA = num,
							particleB = num2
						});
					}
					return;
				}
				if ((flags & ObiUtils.ParticleFlags.OneSided) != 0 && category < category2)
				{
					ObiUtils.Swap(ref A, ref B);
					ObiUtils.Swap(ref lhs, ref rhs);
					ObiUtils.Swap(ref size, ref size2);
					ObiUtils.Swap(ref flags, ref flags2);
					ObiUtils.Swap(ref lhs2, ref rhs2);
				}
				float4 convexBary = BurstMath.BarycenterForSimplexOfSize(size);
				simplexShape.simplexStart = rhs;
				simplexShape.simplexSize = size2;
				simplexShape.positions = restPositions;
				simplexShape.CacheData();
				float num5 = 0f;
				float num6 = 0f;
				float4 convexPoint;
				if (lhs2 == rhs2 && restPositionsEnabled)
				{
					BurstLocalOptimization.SurfacePoint surfacePoint = BurstLocalOptimization.Optimize(ref simplexShape, restPositions, restOrientations, radii, simplices, lhs, size, ref convexBary, out convexPoint, 4, 0f);
					for (int k = 0; k < size; k++)
					{
						num5 += radii[simplices[lhs + k]].x * convexBary[k];
					}
					for (int l = 0; l < size2; l++)
					{
						num6 += radii[simplices[rhs + l]].x * surfacePoint.bary[l];
					}
					if (math.dot(convexPoint - surfacePoint.point, surfacePoint.normal) < num5 + num6)
					{
						return;
					}
				}
				convexBary = BurstMath.BarycenterForSimplexOfSize(size);
				simplexShape.positions = positions;
				simplexShape.CacheData();
				BurstLocalOptimization.SurfacePoint surfacePoint2 = BurstLocalOptimization.Optimize(ref simplexShape, positions, orientations, radii, simplices, lhs, size, ref convexBary, out convexPoint, optimizationIterations, optimizationTolerance);
				num5 = 0f;
				num6 = 0f;
				float4 zero = float4.zero;
				float4 zero2 = float4.zero;
				float4 zero3 = float4.zero;
				float4 zero4 = float4.zero;
				float num7 = 0f;
				float num8 = 0f;
				for (int m = 0; m < size; m++)
				{
					int index = simplices[lhs + m];
					num5 += radii[index].x * convexBary[m];
					zero += velocities[index] * convexBary[m];
					zero3 += ((normals[index].w < 0f) ? new float4(math.rotate(orientations[index], normals[index].xyz), normals[index].w) : normals[index]) * convexBary[m];
					num7 += invMasses[index] * convexBary[m];
				}
				for (int n = 0; n < size2; n++)
				{
					int index2 = simplices[rhs + n];
					num6 += radii[index2].x * surfacePoint2.bary[n];
					zero2 += velocities[index2] * surfacePoint2.bary[n];
					zero4 += ((normals[index2].w < 0f) ? new float4(math.rotate(orientations[index2], normals[index2].xyz), normals[index2].w) : normals[index2]) * surfacePoint2.bary[n];
					num8 += invMasses[index2] * convexBary[n];
				}
				float num9 = math.dot(convexPoint - surfacePoint2.point, surfacePoint2.normal);
				if (!(math.dot(zero - zero2, surfacePoint2.normal) * dt + num9 <= num5 + num6 + collisionMargin))
				{
					return;
				}
				if ((flags2 & ObiUtils.ParticleFlags.OneSided) != 0 && category < category2)
				{
					BurstMath.OneSidedNormal(zero4, ref surfacePoint2.normal);
				}
				if (lhs2 != rhs2 && (zero4.w < 0f || zero3.w < 0f) && num9 * 1.05f <= num5 + num6)
				{
					float4 float5 = zero4;
					if (zero4.w >= 0f || (zero3.w < 0f && zero4.w < zero3.w))
					{
						float5 = new float4(-zero3.xyz, zero3.w);
					}
					if (math.abs(float5.w) <= math.max(num5, num6) * 1.5f)
					{
						BurstMath.OneSidedNormal(float5, ref surfacePoint2.normal);
					}
					else
					{
						surfacePoint2.normal = float5;
					}
				}
				surfacePoint2.normal.w = 0f;
				contactsQueue.Enqueue(new BurstContact
				{
					bodyA = A,
					bodyB = B,
					pointA = convexBary,
					pointB = surfacePoint2.bary,
					normal = surfacePoint2.normal
				});
			}
		}

		public NativeMultilevelGrid<int> grid;

		public NativeQueue<BurstContact> particleContactQueue;

		public NativeQueue<FluidInteraction> fluidInteractionQueue;

		public ParticleGrid()
		{
			grid = new NativeMultilevelGrid<int>(1000, Allocator.Persistent);
			particleContactQueue = new NativeQueue<BurstContact>(Allocator.Persistent);
			fluidInteractionQueue = new NativeQueue<FluidInteraction>(Allocator.Persistent);
		}

		public void Update(BurstSolverImpl solver, JobHandle inputDeps)
		{
			new UpdateGrid
			{
				grid = grid,
				simplexBounds = solver.simplexBounds,
				simplexCount = solver.simplexCounts.simplexCount,
				cellCoords = solver.cellCoords,
				parameters = solver.abstraction.parameters
			}.Schedule(inputDeps).Complete();
		}

		public JobHandle GenerateContacts(BurstSolverImpl solver, float deltaTime)
		{
			return IJobParallelForExtensions.Schedule(new GenerateParticleParticleContactsJob
			{
				grid = grid,
				gridLevels = grid.populatedLevels.GetKeyArray(Allocator.TempJob),
				positions = solver.positions,
				orientations = solver.orientations,
				restPositions = solver.restPositions,
				restOrientations = solver.restOrientations,
				velocities = solver.velocities,
				invMasses = solver.invMasses,
				radii = solver.principalRadii,
				normals = solver.normals,
				fluidMaterials = solver.fluidMaterials,
				phases = solver.phases,
				filters = solver.filters,
				simplices = solver.simplices,
				simplexCounts = solver.simplexCounts,
				particleMaterialIndices = solver.abstraction.collisionMaterials.AsNativeArray<int>(),
				collisionMaterials = ObiColliderWorld.GetInstance().collisionMaterials.AsNativeArray<BurstCollisionMaterial>(),
				contactsQueue = particleContactQueue.AsParallelWriter(),
				fluidInteractionsQueue = fluidInteractionQueue.AsParallelWriter(),
				dt = deltaTime,
				collisionMargin = solver.abstraction.parameters.collisionMargin,
				optimizationIterations = solver.abstraction.parameters.surfaceCollisionIterations,
				optimizationTolerance = solver.abstraction.parameters.surfaceCollisionTolerance
			}, grid.CellCount, 1);
		}

		public JobHandle SpatialQuery(BurstSolverImpl solver, NativeArray<BurstQueryShape> shapes, NativeArray<BurstAffineTransform> transforms, NativeQueue<BurstQueryResult> results)
		{
			return IJobParallelForExtensions.Schedule(new SpatialQueryJob
			{
				grid = grid,
				positions = solver.abstraction.prevPositions.AsNativeArray<float4>(),
				orientations = solver.abstraction.prevOrientations.AsNativeArray<quaternion>(),
				radii = solver.abstraction.principalRadii.AsNativeArray<float4>(),
				filters = solver.abstraction.filters.AsNativeArray<int>(),
				simplices = solver.simplices,
				simplexCounts = solver.simplexCounts,
				shapes = shapes,
				transforms = transforms,
				results = results.AsParallelWriter(),
				worldToSolver = solver.worldToSolver,
				parameters = solver.abstraction.parameters
			}, shapes.Length, 4);
		}

		public void GetCells(ObiNativeAabbList cells)
		{
			if (cells.count == grid.usedCells.Length)
			{
				for (int i = 0; i < grid.usedCells.Length; i++)
				{
					NativeMultilevelGrid<int>.Cell<int> cell = grid.usedCells[i];
					float num = NativeMultilevelGrid<int>.CellSizeOfLevel(cell.Coords.w);
					float4 float5 = (float4)cell.Coords * num;
					float5[3] = 0f;
					cells[i] = new Aabb(float5, float5 + new float4(num, num, num, 0f));
				}
			}
		}

		public void Dispose()
		{
			grid.Dispose();
			particleContactQueue.Dispose();
			fluidInteractionQueue.Dispose();
		}
	}
}
