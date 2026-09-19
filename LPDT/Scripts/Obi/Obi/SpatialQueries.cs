using UnityEngine;

namespace Obi
{
	public class SpatialQueries
	{
		private ComputePrefixSum prefixSum;

		private ComputeShader gridShader;

		private int buildKernel;

		private int gridPopulationKernel;

		private int sortKernel;

		private int contactsKernel;

		private int clearKernel;

		private int prefixSumPairsKernel;

		private int sortPairsKernel;

		public GraphicsBuffer sortedShapeIndicesBuffer;

		public GraphicsBuffer cellIndicesBuffer;

		public GraphicsBuffer cellOffsetsBuffer;

		public GraphicsBuffer cellCountsBuffer;

		public GraphicsBuffer offsetInCells;

		public GraphicsBuffer levelPopulation;

		private GraphicsBuffer queryTypeCounts;

		public GraphicsBuffer unsortedContactPairs;

		public GraphicsBuffer contactPairs;

		public GraphicsBuffer contactOffsetsPerType;

		public GraphicsBuffer dispatchBuffer;

		private const int maxCells = 262144;

		private const int cellsPerShape = 8;

		private const int maxGridLevels = 24;

		private uint[] queryCountClear = new uint[3];

		private uint[] dispatchClear = new uint[20]
		{
			0u, 1u, 1u, 0u, 0u, 1u, 1u, 0u, 0u, 1u,
			1u, 0u, 0u, 1u, 1u, 0u, 0u, 1u, 1u, 0u
		};

		private ComputeSphereQuery spheres;

		private ComputeBoxQuery boxes;

		private ComputeRayQuery rays;

		public SpatialQueries(uint capacity)
		{
			gridShader = Resources.Load<ComputeShader>("Compute/SpatialQueries");
			buildKernel = gridShader.FindKernel("BuildUnsortedList");
			gridPopulationKernel = gridShader.FindKernel("FindPopulatedLevels");
			sortKernel = gridShader.FindKernel("SortList");
			contactsKernel = gridShader.FindKernel("BuildContactList");
			clearKernel = gridShader.FindKernel("Clear");
			prefixSumPairsKernel = gridShader.FindKernel("PrefixSumColliderCounts");
			sortPairsKernel = gridShader.FindKernel("SortContactPairs");
			gridShader.SetInt("shapeTypeCount", 3);
			gridShader.SetInt("cellsPerShape", 8);
			gridShader.SetInt("maxCells", 262144);
			cellOffsetsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 262144, 4);
			cellCountsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 262144, 4);
			levelPopulation = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 25, 4);
			dispatchBuffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, dispatchClear.Length, 4);
			queryTypeCounts = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 3, 4);
			contactOffsetsPerType = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 4, 4);
			prefixSum = new ComputePrefixSum(262144);
			spheres = new ComputeSphereQuery();
			boxes = new ComputeBoxQuery();
			rays = new ComputeRayQuery();
			SetCapacity(capacity);
		}

		public void Dispose()
		{
			prefixSum?.Dispose();
			cellOffsetsBuffer?.Dispose();
			cellCountsBuffer?.Dispose();
			levelPopulation?.Dispose();
			dispatchBuffer?.Dispose();
			queryTypeCounts?.Dispose();
			contactOffsetsPerType?.Dispose();
			DisposeOfResultsData();
			DisposeOfQueryData();
		}

		private void DisposeOfResultsData()
		{
			contactPairs?.Dispose();
			unsortedContactPairs?.Dispose();
		}

		private void DisposeOfQueryData()
		{
			cellIndicesBuffer?.Dispose();
			offsetInCells?.Dispose();
			sortedShapeIndicesBuffer?.Dispose();
		}

		private void SetCapacity(uint capacity)
		{
			DisposeOfResultsData();
			gridShader.SetInt("maxResults", (int)capacity);
			if (capacity != 0)
			{
				unsortedContactPairs = new GraphicsBuffer(GraphicsBuffer.Target.Structured, (int)capacity, 8);
				contactPairs = new GraphicsBuffer(GraphicsBuffer.Target.Structured, (int)capacity, 8);
			}
		}

		public void SpatialQuery(ComputeSolverImpl solver, GraphicsBuffer shapes, GraphicsBuffer transforms, GraphicsBuffer results)
		{
			results.SetCounterValue(0u);
			if (solver.activeParticlesBuffer == null || solver.simplices == null)
			{
				return;
			}
			if (contactPairs == null || !contactPairs.IsValid() || contactPairs.count != solver.abstraction.maxQueryResults)
			{
				SetCapacity(solver.abstraction.maxQueryResults);
			}
			if (contactPairs != null && contactPairs.IsValid())
			{
				if (cellIndicesBuffer == null || !cellIndicesBuffer.IsValid() || shapes.count * 8 >= cellIndicesBuffer.count)
				{
					DisposeOfQueryData();
					cellIndicesBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, shapes.count * 8 * 2, 4);
					offsetInCells = new GraphicsBuffer(GraphicsBuffer.Target.Structured, shapes.count * 8 * 2, 4);
					sortedShapeIndicesBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, shapes.count * 8 * 2, 4);
				}
				gridShader.SetInt("queryCount", shapes.count);
				int threadGroupsX = ComputeMath.ThreadGroupCount(solver.simplexCounts.simplexCount, 128);
				int threadGroupsX2 = ComputeMath.ThreadGroupCount(shapes.count, 128);
				int num = ComputeMath.ThreadGroupCount(shapes.count * 8, 128);
				int a = ComputeMath.ThreadGroupCount(262144, 128);
				queryTypeCounts.SetData(queryCountClear);
				dispatchBuffer.SetData(dispatchClear);
				gridShader.SetBuffer(clearKernel, "cellOffsets", cellOffsetsBuffer);
				gridShader.SetBuffer(clearKernel, "cellIndices", cellIndicesBuffer);
				gridShader.SetBuffer(clearKernel, "cellCounts", cellCountsBuffer);
				gridShader.SetBuffer(clearKernel, "levelPopulation", levelPopulation);
				gridShader.Dispatch(clearKernel, Mathf.Max(a, num), 1, 1);
				gridShader.SetBuffer(buildKernel, "shapes", shapes);
				gridShader.SetBuffer(buildKernel, "transforms", transforms);
				gridShader.SetBuffer(buildKernel, "cellIndices", cellIndicesBuffer);
				gridShader.SetBuffer(buildKernel, "cellCounts", cellCountsBuffer);
				gridShader.SetBuffer(buildKernel, "offsetInCells", offsetInCells);
				gridShader.SetBuffer(buildKernel, "levelPopulation", levelPopulation);
				gridShader.SetBuffer(buildKernel, "worldToSolver", solver.worldToSolverBuffer);
				gridShader.Dispatch(buildKernel, threadGroupsX2, 1, 1);
				gridShader.SetBuffer(gridPopulationKernel, "levelPopulation", levelPopulation);
				gridShader.Dispatch(gridPopulationKernel, 1, 1, 1);
				prefixSum.Sum(cellCountsBuffer, cellOffsetsBuffer);
				gridShader.SetBuffer(sortKernel, "sortedColliderIndices", sortedShapeIndicesBuffer);
				gridShader.SetBuffer(sortKernel, "offsetInCells", offsetInCells);
				gridShader.SetBuffer(sortKernel, "cellIndices", cellIndicesBuffer);
				gridShader.SetBuffer(sortKernel, "cellOffsets", cellOffsetsBuffer);
				gridShader.Dispatch(sortKernel, num, 1, 1);
				gridShader.SetInt("pointCount", solver.simplexCounts.pointCount);
				gridShader.SetInt("edgeCount", solver.simplexCounts.edgeCount);
				gridShader.SetInt("triangleCount", solver.simplexCounts.triangleCount);
				gridShader.SetInt("surfaceCollisionIterations", solver.abstraction.parameters.surfaceCollisionIterations);
				gridShader.SetFloat("surfaceCollisionTolerance", solver.abstraction.parameters.surfaceCollisionTolerance);
				gridShader.SetInt("mode", (int)solver.abstraction.parameters.mode);
				gridShader.SetBuffer(contactsKernel, "simplices", solver.simplices);
				gridShader.SetBuffer(contactsKernel, "simplexBounds", solver.simplexBounds);
				gridShader.SetBuffer(contactsKernel, "positions", solver.positionsBuffer);
				gridShader.SetBuffer(contactsKernel, "orientations", solver.orientationsBuffer);
				gridShader.SetBuffer(contactsKernel, "principalRadii", solver.principalRadiiBuffer);
				gridShader.SetBuffer(contactsKernel, "filters", solver.filtersBuffer);
				gridShader.SetBuffer(contactsKernel, "sortedColliderIndices", sortedShapeIndicesBuffer);
				gridShader.SetBuffer(contactsKernel, "transforms", transforms);
				gridShader.SetBuffer(contactsKernel, "shapes", shapes);
				gridShader.SetBuffer(contactsKernel, "collisionMaterialIndices", solver.collisionMaterialIndexBuffer);
				gridShader.SetBuffer(contactsKernel, "cellIndices", cellIndicesBuffer);
				gridShader.SetBuffer(contactsKernel, "cellOffsets", cellOffsetsBuffer);
				gridShader.SetBuffer(contactsKernel, "cellCounts", cellCountsBuffer);
				gridShader.SetBuffer(contactsKernel, "levelPopulation", levelPopulation);
				gridShader.SetBuffer(contactsKernel, "solverToWorld", solver.solverToWorldBuffer);
				gridShader.SetBuffer(contactsKernel, "worldToSolver", solver.worldToSolverBuffer);
				gridShader.SetBuffer(contactsKernel, "colliderTypeCounts", queryTypeCounts);
				gridShader.SetBuffer(contactsKernel, "unsortedContactPairs", unsortedContactPairs);
				gridShader.SetBuffer(contactsKernel, "dispatchBuffer", dispatchBuffer);
				gridShader.Dispatch(contactsKernel, threadGroupsX, 1, 1);
				gridShader.SetBuffer(prefixSumPairsKernel, "colliderTypeCounts", queryTypeCounts);
				gridShader.SetBuffer(prefixSumPairsKernel, "contactOffsetsPerType", contactOffsetsPerType);
				gridShader.SetBuffer(prefixSumPairsKernel, "dispatchBuffer", dispatchBuffer);
				gridShader.Dispatch(prefixSumPairsKernel, 1, 1, 1);
				gridShader.SetBuffer(sortPairsKernel, "shapes", shapes);
				gridShader.SetBuffer(sortPairsKernel, "unsortedContactPairs", unsortedContactPairs);
				gridShader.SetBuffer(sortPairsKernel, "contactPairs", contactPairs);
				gridShader.SetBuffer(sortPairsKernel, "colliderTypeCounts", queryTypeCounts);
				gridShader.SetBuffer(sortPairsKernel, "contactOffsetsPerType", contactOffsetsPerType);
				gridShader.SetBuffer(sortPairsKernel, "dispatchBuffer", dispatchBuffer);
				gridShader.DispatchIndirect(sortPairsKernel, dispatchBuffer, 16u);
				boxes.GetResults(solver, this, transforms, shapes, results);
				spheres.GetResults(solver, this, transforms, shapes, results);
				rays.GetResults(solver, this, transforms, shapes, results);
			}
		}
	}
}
