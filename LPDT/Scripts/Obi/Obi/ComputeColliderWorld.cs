using UnityEngine;

namespace Obi
{
	public class ComputeColliderWorld : MonoBehaviour, IColliderWorldImpl
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

		private int applyForceZonesKernel;

		private int writeForceZoneResultsKernel;

		public GraphicsBuffer materialsBuffer;

		public GraphicsBuffer aabbsBuffer;

		public GraphicsBuffer transformsBuffer;

		public GraphicsBuffer shapesBuffer;

		public GraphicsBuffer forceZonesBuffer;

		public GraphicsBuffer rigidbodiesBuffer;

		public GraphicsBuffer sortedColliderIndicesBuffer;

		public GraphicsBuffer cellIndicesBuffer;

		public GraphicsBuffer cellOffsetsBuffer;

		public GraphicsBuffer cellCountsBuffer;

		public GraphicsBuffer offsetInCells;

		public GraphicsBuffer levelPopulation;

		private GraphicsBuffer colliderTypeCounts;

		public GraphicsBuffer unsortedContactPairs;

		public GraphicsBuffer contactPairs;

		public GraphicsBuffer contactOffsetsPerType;

		public GraphicsBuffer dispatchBuffer;

		public GraphicsBuffer heightFieldHeaders;

		public GraphicsBuffer heightFieldSamples;

		public GraphicsBuffer distanceFieldHeaders;

		public GraphicsBuffer dfNodes;

		public GraphicsBuffer edgeMeshHeaders;

		public GraphicsBuffer edgeBihNodes;

		public GraphicsBuffer edges;

		public GraphicsBuffer edgeVertices;

		public GraphicsBuffer triangleMeshHeaders;

		public GraphicsBuffer bihNodes;

		public GraphicsBuffer triangles;

		public GraphicsBuffer vertices;

		public const int maxContacts = 262144;

		public const int maxCells = 262144;

		public const int cellsPerCollider = 8;

		private const int maxGridLevels = 24;

		private uint[] colliderCountClear = new uint[7];

		private uint[] dispatchClear = new uint[36]
		{
			0u, 1u, 1u, 0u, 0u, 1u, 1u, 0u, 0u, 1u,
			1u, 0u, 0u, 1u, 1u, 0u, 0u, 1u, 1u, 0u,
			0u, 1u, 1u, 0u, 0u, 1u, 1u, 0u, 0u, 1u,
			1u, 0u, 0u, 1u, 1u, 0u
		};

		private ComputeSphere spheres;

		private ComputeBox boxes;

		private ComputeCapsule capsules;

		private ComputeTriangleMesh triangleMeshes;

		private ComputeEdgeMesh edgeMeshes;

		private ComputeDistanceField distanceFields;

		private ComputeHeightField heightFields;

		public int referenceCount { get; private set; }

		public int colliderCount { get; private set; }

		public int rigidbodyCount { get; private set; } = -1;

		public int forceZoneCount { get; private set; } = -1;

		public int materialCount { get; private set; } = -1;

		public int triangleMeshCount { get; private set; } = -1;

		public int edgeMeshCount { get; private set; } = -1;

		public int distanceFieldCount { get; private set; } = -1;

		public int heightFieldCount { get; private set; } = -1;

		public void Awake()
		{
			ObiColliderWorld.GetInstance().RegisterImplementation(this);
			prefixSum = new ComputePrefixSum(262144);
			gridShader = Resources.Load<ComputeShader>("Compute/ColliderGrid");
			buildKernel = gridShader.FindKernel("BuildUnsortedList");
			gridPopulationKernel = gridShader.FindKernel("FindPopulatedLevels");
			sortKernel = gridShader.FindKernel("SortList");
			contactsKernel = gridShader.FindKernel("BuildContactList");
			clearKernel = gridShader.FindKernel("Clear");
			prefixSumPairsKernel = gridShader.FindKernel("PrefixSumColliderCounts");
			sortPairsKernel = gridShader.FindKernel("SortContactPairs");
			applyForceZonesKernel = gridShader.FindKernel("ApplyForceZones");
			writeForceZoneResultsKernel = gridShader.FindKernel("WriteForceZoneResults");
			gridShader.SetInt("shapeTypeCount", 7);
			gridShader.SetInt("maxContacts", 262144);
			gridShader.SetInt("colliderCount", colliderCount);
			gridShader.SetInt("cellsPerCollider", 8);
			gridShader.SetInt("maxCells", 262144);
			cellOffsetsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 262144, 4);
			cellCountsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 262144, 4);
			levelPopulation = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 25, 4);
			colliderTypeCounts = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 7, 4);
			contactOffsetsPerType = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 8, 4);
			unsortedContactPairs = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 262144, 8);
			contactPairs = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 262144, 8);
			dispatchBuffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, dispatchClear.Length, 4);
			spheres = new ComputeSphere();
			boxes = new ComputeBox();
			capsules = new ComputeCapsule();
			triangleMeshes = new ComputeTriangleMesh();
			edgeMeshes = new ComputeEdgeMesh();
			distanceFields = new ComputeDistanceField();
			heightFields = new ComputeHeightField();
		}

		public void OnDestroy()
		{
			ObiColliderWorld.GetInstance().UnregisterImplementation(this);
			prefixSum.Dispose();
			cellOffsetsBuffer.Dispose();
			cellCountsBuffer.Dispose();
			levelPopulation.Dispose();
			contactPairs.Dispose();
			dispatchBuffer.Dispose();
			colliderTypeCounts.Dispose();
			contactOffsetsPerType.Dispose();
			unsortedContactPairs.Dispose();
			if (cellIndicesBuffer != null)
			{
				cellIndicesBuffer.Dispose();
			}
			if (offsetInCells != null)
			{
				offsetInCells.Dispose();
			}
			if (sortedColliderIndicesBuffer != null)
			{
				sortedColliderIndicesBuffer.Dispose();
			}
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
			if (colliderCount != shapes.count || aabbsBuffer == null || !aabbsBuffer.IsValid())
			{
				aabbsBuffer = bounds.AsComputeBuffer<Aabb>();
			}
			else
			{
				bounds.Upload();
			}
			if (colliderCount != shapes.count || shapesBuffer == null || !shapesBuffer.IsValid())
			{
				shapesBuffer = shapes.AsComputeBuffer<ColliderShape>();
			}
			else
			{
				shapes.Upload();
			}
			if (colliderCount != shapes.count || transformsBuffer == null || !transformsBuffer.IsValid())
			{
				transformsBuffer = transforms.AsComputeBuffer<AffineTransform>();
			}
			else
			{
				transforms.Upload();
			}
			if (colliderCount != shapes.count)
			{
				colliderCount = shapes.count;
				gridShader.SetInt("colliderCount", colliderCount);
				if (cellIndicesBuffer != null)
				{
					cellIndicesBuffer.Release();
					cellIndicesBuffer = null;
				}
				if (offsetInCells != null)
				{
					offsetInCells.Release();
					offsetInCells = null;
				}
				if (sortedColliderIndicesBuffer != null)
				{
					sortedColliderIndicesBuffer.Release();
					sortedColliderIndicesBuffer = null;
				}
				if (colliderCount > 0)
				{
					cellIndicesBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, colliderCount * 8, 4);
					offsetInCells = new GraphicsBuffer(GraphicsBuffer.Target.Structured, colliderCount * 8, 4);
					sortedColliderIndicesBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, colliderCount * 8, 4);
				}
			}
		}

		public void SetForceZones(ObiNativeForceZoneList forceZones)
		{
			if (forceZoneCount != forceZones.count || forceZonesBuffer == null || !forceZonesBuffer.IsValid())
			{
				forceZoneCount = forceZones.count;
				forceZonesBuffer = forceZones.SafeAsComputeBuffer<ForceZone>();
			}
			else
			{
				forceZones.Upload();
			}
		}

		public void SetRigidbodies(ObiNativeRigidbodyList rigidbody)
		{
			if (rigidbodyCount != rigidbody.count || rigidbodiesBuffer == null || !rigidbodiesBuffer.IsValid())
			{
				rigidbodyCount = rigidbody.count;
				rigidbodiesBuffer = rigidbody.SafeAsComputeBuffer<ColliderRigidbody>();
			}
			else
			{
				rigidbody.Upload();
			}
		}

		public void SetCollisionMaterials(ObiNativeCollisionMaterialList materials)
		{
			if (materialCount != materials.count || materialsBuffer == null || !materialsBuffer.IsValid())
			{
				materialCount = materials.count;
				materialsBuffer = materials.SafeAsComputeBuffer<CollisionMaterial>();
			}
			else
			{
				materials.Upload();
			}
		}

		public void SetTriangleMeshData(ObiNativeTriangleMeshHeaderList headers, ObiNativeBIHNodeList nodes, ObiNativeTriangleList triangles, ObiNativeVector3List vertices)
		{
			if (triangleMeshCount != headers.count || triangleMeshHeaders == null || !triangleMeshHeaders.IsValid())
			{
				triangleMeshCount = headers.count;
				triangleMeshHeaders = headers.SafeAsComputeBuffer<TriangleMeshHeader>();
				bihNodes = nodes.SafeAsComputeBuffer<BIHNode>();
				this.triangles = triangles.SafeAsComputeBuffer<Triangle>();
				this.vertices = vertices.SafeAsComputeBuffer<Vector3>();
			}
		}

		public void SetEdgeMeshData(ObiNativeEdgeMeshHeaderList headers, ObiNativeBIHNodeList nodes, ObiNativeEdgeList edges, ObiNativeVector2List vertices)
		{
			if (edgeMeshCount != headers.count || edgeMeshHeaders == null || !edgeMeshHeaders.IsValid())
			{
				edgeMeshCount = headers.count;
				edgeMeshHeaders = headers.SafeAsComputeBuffer<EdgeMeshHeader>();
				edgeBihNodes = nodes.SafeAsComputeBuffer<BIHNode>();
				this.edges = edges.SafeAsComputeBuffer<Edge>();
				edgeVertices = vertices.SafeAsComputeBuffer<Vector2>();
			}
		}

		public void SetDistanceFieldData(ObiNativeDistanceFieldHeaderList headers, ObiNativeDFNodeList nodes)
		{
			if (distanceFieldCount != headers.count || distanceFieldHeaders == null || !distanceFieldHeaders.IsValid())
			{
				distanceFieldCount = headers.count;
				distanceFieldHeaders = headers.SafeAsComputeBuffer<DistanceFieldHeader>();
				dfNodes = nodes.SafeAsComputeBuffer<DFNode>();
			}
		}

		public void SetHeightFieldData(ObiNativeHeightFieldHeaderList headers, ObiNativeFloatList samples)
		{
			if (heightFieldCount != headers.count || heightFieldHeaders == null || !heightFieldHeaders.IsValid())
			{
				heightFieldCount = headers.count;
				heightFieldHeaders = headers.SafeAsComputeBuffer<HeightFieldHeader>();
				heightFieldSamples = samples.SafeAsComputeBuffer<float>();
			}
		}

		public void UpdateWorld(float deltaTime)
		{
			if (colliderCount > 0)
			{
				int threadGroupsX = ComputeMath.ThreadGroupCount(colliderCount, 128);
				int num = ComputeMath.ThreadGroupCount(colliderCount * 8, 128);
				int a = ComputeMath.ThreadGroupCount(262144, 128);
				gridShader.SetBuffer(clearKernel, "cellOffsets", cellOffsetsBuffer);
				gridShader.SetBuffer(clearKernel, "cellIndices", cellIndicesBuffer);
				gridShader.SetBuffer(clearKernel, "cellCounts", cellCountsBuffer);
				gridShader.SetBuffer(clearKernel, "levelPopulation", levelPopulation);
				gridShader.Dispatch(clearKernel, Mathf.Max(a, num), 1, 1);
				gridShader.SetBuffer(buildKernel, "aabbs", aabbsBuffer);
				gridShader.SetBuffer(buildKernel, "shapes", shapesBuffer);
				gridShader.SetBuffer(buildKernel, "rigidbodies", rigidbodiesBuffer);
				gridShader.SetBuffer(buildKernel, "collisionMaterials", materialsBuffer);
				gridShader.SetBuffer(buildKernel, "cellIndices", cellIndicesBuffer);
				gridShader.SetBuffer(buildKernel, "cellCounts", cellCountsBuffer);
				gridShader.SetBuffer(buildKernel, "offsetInCells", offsetInCells);
				gridShader.SetBuffer(buildKernel, "levelPopulation", levelPopulation);
				gridShader.Dispatch(buildKernel, threadGroupsX, 1, 1);
				gridShader.SetBuffer(gridPopulationKernel, "levelPopulation", levelPopulation);
				gridShader.Dispatch(gridPopulationKernel, 1, 1, 1);
				prefixSum.Sum(cellCountsBuffer, cellOffsetsBuffer);
				gridShader.SetBuffer(sortKernel, "sortedColliderIndices", sortedColliderIndicesBuffer);
				gridShader.SetBuffer(sortKernel, "offsetInCells", offsetInCells);
				gridShader.SetBuffer(sortKernel, "cellIndices", cellIndicesBuffer);
				gridShader.SetBuffer(sortKernel, "cellOffsets", cellOffsetsBuffer);
				gridShader.Dispatch(sortKernel, num, 1, 1);
			}
		}

		public void ApplyForceZones(ComputeSolverImpl solver, float deltaTime)
		{
			if (colliderCount > 0 && solver.activeParticlesBuffer != null && solver.simplices != null && forceZonesBuffer != null)
			{
				gridShader.SetInt("pointCount", solver.simplexCounts.pointCount);
				gridShader.SetInt("edgeCount", solver.simplexCounts.edgeCount);
				gridShader.SetInt("triangleCount", solver.simplexCounts.triangleCount);
				gridShader.SetFloat("deltaTime", deltaTime);
				gridShader.SetBuffer(applyForceZonesKernel, "contacts", solver.abstraction.colliderContacts.computeBuffer);
				gridShader.SetBuffer(applyForceZonesKernel, "dispatchBuffer", dispatchBuffer);
				gridShader.SetBuffer(applyForceZonesKernel, "simplices", solver.simplices);
				gridShader.SetBuffer(applyForceZonesKernel, "positions", solver.positionsBuffer);
				gridShader.SetBuffer(applyForceZonesKernel, "velocities", solver.velocitiesBuffer);
				gridShader.SetBuffer(applyForceZonesKernel, "colors", solver.colorsBuffer);
				gridShader.SetBuffer(applyForceZonesKernel, "invMasses", solver.invMassesBuffer);
				gridShader.SetBuffer(applyForceZonesKernel, "transforms", transformsBuffer);
				gridShader.SetBuffer(applyForceZonesKernel, "shapes", shapesBuffer);
				gridShader.SetBuffer(applyForceZonesKernel, "forceZones", forceZonesBuffer);
				gridShader.SetBuffer(applyForceZonesKernel, "deltasAsInt", solver.positionDeltasIntBuffer);
				gridShader.SetBuffer(applyForceZonesKernel, "orientationDeltasAsInt", solver.orientationDeltasIntBuffer);
				gridShader.SetBuffer(applyForceZonesKernel, "worldToSolver", solver.worldToSolverBuffer);
				gridShader.DispatchIndirect(applyForceZonesKernel, dispatchBuffer);
				int threadGroupsX = ComputeMath.ThreadGroupCount(solver.activeParticleCount, 128);
				gridShader.SetInt("particleCount", solver.activeParticleCount);
				gridShader.SetBuffer(writeForceZoneResultsKernel, "activeParticles", solver.activeParticlesBuffer);
				gridShader.SetBuffer(writeForceZoneResultsKernel, "externalForces", solver.externalForcesBuffer);
				gridShader.SetBuffer(writeForceZoneResultsKernel, "life", solver.lifeBuffer);
				gridShader.SetBuffer(writeForceZoneResultsKernel, "wind", solver.windBuffer);
				gridShader.SetBuffer(writeForceZoneResultsKernel, "deltasAsInt", solver.positionDeltasIntBuffer);
				gridShader.SetBuffer(writeForceZoneResultsKernel, "orientationDeltasAsInt", solver.orientationDeltasIntBuffer);
				gridShader.Dispatch(writeForceZoneResultsKernel, threadGroupsX, 1, 1);
			}
		}

		public void GenerateContacts(ComputeSolverImpl solver, float deltaTime)
		{
			if (colliderCount > 0)
			{
				int threadGroupsX = ComputeMath.ThreadGroupCount(solver.simplexCounts.simplexCount, 128);
				colliderTypeCounts.SetData(colliderCountClear);
				dispatchBuffer.SetData(dispatchClear);
				if (solver.activeParticlesBuffer != null && solver.simplices != null)
				{
					solver.abstraction.colliderContacts.computeBuffer.SetCounterValue(0u);
					gridShader.SetInt("pointCount", solver.simplexCounts.pointCount);
					gridShader.SetInt("edgeCount", solver.simplexCounts.edgeCount);
					gridShader.SetInt("triangleCount", solver.simplexCounts.triangleCount);
					gridShader.SetFloat("colliderCCD", solver.abstraction.parameters.colliderCCD);
					gridShader.SetInt("surfaceCollisionIterations", solver.abstraction.parameters.surfaceCollisionIterations);
					gridShader.SetFloat("surfaceCollisionTolerance", solver.abstraction.parameters.surfaceCollisionTolerance);
					gridShader.SetInt("mode", (int)solver.abstraction.parameters.mode);
					gridShader.SetFloat("deltaTime", deltaTime);
					gridShader.SetBuffer(contactsKernel, "simplices", solver.simplices);
					gridShader.SetBuffer(contactsKernel, "simplexBounds", solver.simplexBounds);
					gridShader.SetBuffer(contactsKernel, "positions", solver.positionsBuffer);
					gridShader.SetBuffer(contactsKernel, "orientations", solver.orientationsBuffer);
					gridShader.SetBuffer(contactsKernel, "principalRadii", solver.principalRadiiBuffer);
					gridShader.SetBuffer(contactsKernel, "filters", solver.filtersBuffer);
					gridShader.SetBuffer(contactsKernel, "sortedColliderIndices", sortedColliderIndicesBuffer);
					gridShader.SetBuffer(contactsKernel, "aabbs", aabbsBuffer);
					gridShader.SetBuffer(contactsKernel, "transforms", transformsBuffer);
					gridShader.SetBuffer(contactsKernel, "shapes", shapesBuffer);
					gridShader.SetBuffer(contactsKernel, "rigidbodies", rigidbodiesBuffer);
					gridShader.SetBuffer(contactsKernel, "collisionMaterials", materialsBuffer);
					gridShader.SetBuffer(contactsKernel, "collisionMaterialIndices", solver.collisionMaterialIndexBuffer);
					gridShader.SetBuffer(contactsKernel, "cellIndices", cellIndicesBuffer);
					gridShader.SetBuffer(contactsKernel, "cellOffsets", cellOffsetsBuffer);
					gridShader.SetBuffer(contactsKernel, "cellCounts", cellCountsBuffer);
					gridShader.SetBuffer(contactsKernel, "levelPopulation", levelPopulation);
					gridShader.SetBuffer(contactsKernel, "solverToWorld", solver.solverToWorldBuffer);
					gridShader.SetBuffer(contactsKernel, "colliderTypeCounts", colliderTypeCounts);
					gridShader.SetBuffer(contactsKernel, "unsortedContactPairs", unsortedContactPairs);
					gridShader.SetBuffer(contactsKernel, "dispatchBuffer", dispatchBuffer);
					gridShader.Dispatch(contactsKernel, threadGroupsX, 1, 1);
					gridShader.SetBuffer(prefixSumPairsKernel, "colliderTypeCounts", colliderTypeCounts);
					gridShader.SetBuffer(prefixSumPairsKernel, "contactOffsetsPerType", contactOffsetsPerType);
					gridShader.SetBuffer(prefixSumPairsKernel, "dispatchBuffer", dispatchBuffer);
					gridShader.Dispatch(prefixSumPairsKernel, 1, 1, 1);
					gridShader.SetBuffer(sortPairsKernel, "shapes", shapesBuffer);
					gridShader.SetBuffer(sortPairsKernel, "unsortedContactPairs", unsortedContactPairs);
					gridShader.SetBuffer(sortPairsKernel, "contactPairs", contactPairs);
					gridShader.SetBuffer(sortPairsKernel, "colliderTypeCounts", colliderTypeCounts);
					gridShader.SetBuffer(sortPairsKernel, "contactOffsetsPerType", contactOffsetsPerType);
					gridShader.SetBuffer(sortPairsKernel, "dispatchBuffer", dispatchBuffer);
					gridShader.DispatchIndirect(sortPairsKernel, dispatchBuffer, 16u);
					boxes.GenerateContacts(solver, this);
					spheres.GenerateContacts(solver, this);
					capsules.GenerateContacts(solver, this);
					triangleMeshes.GenerateContacts(solver, this, deltaTime);
					edgeMeshes.GenerateContacts(solver, this, deltaTime);
					distanceFields.GenerateContacts(solver, this, deltaTime);
					heightFields.GenerateContacts(solver, this, deltaTime);
				}
			}
		}
	}
}
