using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct SpatialQueryJob : IJobParallelFor
	{
		[ReadOnly]
		public NativeMultilevelGrid<int> grid;

		[ReadOnly]
		public NativeArray<float4> positions;

		[ReadOnly]
		public NativeArray<quaternion> orientations;

		[ReadOnly]
		public NativeArray<float4> radii;

		[ReadOnly]
		public NativeArray<int> filters;

		[ReadOnly]
		public NativeArray<int> simplices;

		[ReadOnly]
		public SimplexCounts simplexCounts;

		[ReadOnly]
		public NativeArray<BurstQueryShape> shapes;

		[ReadOnly]
		public NativeArray<BurstAffineTransform> transforms;

		[WriteOnly]
		[NativeDisableParallelForRestriction]
		public NativeQueue<BurstQueryResult>.ParallelWriter results;

		[ReadOnly]
		public BurstAffineTransform worldToSolver;

		[ReadOnly]
		public Oni.SolverParameters parameters;

		public void Execute(int i)
		{
			BurstAffineTransform transform = worldToSolver * transforms[i];
			BurstAabb bounds = CalculateShapeAABB(shapes[i]).Transformed(in transform);
			int num = shapes[i].filter & 0xFFFF;
			int num2 = (shapes[i].filter & -65536) >> 16;
			bool in2D = parameters.mode == Oni.SolverParameters.Mode.Mode2D;
			for (int j = 0; j < grid.usedCells.Length; j++)
			{
				NativeMultilevelGrid<int>.Cell<int> cell = grid.usedCells[j];
				float num3 = NativeMultilevelGrid<int>.CellSizeOfLevel(cell.Coords.w);
				float4 float5 = (float4)cell.Coords * num3;
				if (!new BurstAabb(float5 - new float4(num3), float5 + new float4(2f * num3)).IntersectsAabb(in bounds, in2D))
				{
					continue;
				}
				for (int k = 0; k < cell.Length; k++)
				{
					int size;
					int simplexStartAndSize = simplexCounts.GetSimplexStartAndSize(cell[k], out size);
					bool flag = false;
					for (int l = 0; l < size; l++)
					{
						int num4 = filters[simplices[simplexStartAndSize + l]] & 0xFFFF;
						int num5 = (filters[simplices[simplexStartAndSize + l]] & -65536) >> 16;
						flag = flag || ((num4 & num2) != 0 && (num5 & num) != 0);
					}
					if (flag)
					{
						Query(shapes[i], in transform, i, cell[k], simplexStartAndSize, size);
					}
				}
			}
		}

		private BurstAabb CalculateShapeAABB(in BurstQueryShape shape)
		{
			float num = shape.contactOffset + shape.maxDistance;
			return shape.type switch
			{
				QueryShape.QueryType.Sphere => new BurstAabb(shape.center, shape.center, shape.size.x + num), 
				QueryShape.QueryType.Box => new BurstAabb(shape.center - shape.size * 0.5f - num, shape.center + shape.size * 0.5f + num), 
				QueryShape.QueryType.Ray => new BurstAabb(shape.center, shape.center + shape.size, num), 
				_ => default(BurstAabb), 
			};
		}

		private void Query(in BurstQueryShape shape, in BurstAffineTransform shapeToSolver, int shapeIndex, int simplexIndex, int simplexStart, int simplexSize)
		{
			switch (shape.type)
			{
			case QueryShape.QueryType.Sphere:
			{
				BurstSphereQuery burstSphereQuery = new BurstSphereQuery
				{
					colliderToSolver = shapeToSolver,
					shape = shape
				};
				burstSphereQuery.Query(shapeIndex, positions, orientations, radii, simplices, simplexIndex, simplexStart, simplexSize, results, parameters.surfaceCollisionIterations, parameters.surfaceCollisionTolerance);
				break;
			}
			case QueryShape.QueryType.Box:
			{
				BurstBoxQuery burstBoxQuery = new BurstBoxQuery
				{
					colliderToSolver = shapeToSolver,
					shape = shape
				};
				burstBoxQuery.Query(shapeIndex, positions, orientations, radii, simplices, simplexIndex, simplexStart, simplexSize, results, parameters.surfaceCollisionIterations, parameters.surfaceCollisionTolerance);
				break;
			}
			case QueryShape.QueryType.Ray:
			{
				BurstRay burstRay = new BurstRay
				{
					colliderToSolver = shapeToSolver,
					shape = shape
				};
				burstRay.Query(shapeIndex, positions, orientations, radii, simplices, simplexIndex, simplexStart, simplexSize, results, parameters.surfaceCollisionIterations, parameters.surfaceCollisionTolerance);
				break;
			}
			}
		}
	}
}
