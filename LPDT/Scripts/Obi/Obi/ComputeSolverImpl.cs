using UnityEngine;
using UnityEngine.Rendering;

namespace Obi
{
	public class ComputeSolverImpl : ISolverImpl
	{
		private ObiSolver m_Solver;

		private IComputeConstraintsImpl[] constraints;

		private int[] padding = new int[18];

		private ComputeJobHandle jobHandle;

		public ComputeParticleGrid particleGrid;

		public ComputeColliderWorld colliderGrid;

		public SpatialQueries spatialQueries;

		private InertialFrame m_InertialFrame;

		public GraphicsBuffer deadIndicesBuffer;

		public GraphicsBuffer positionsBuffer;

		public GraphicsBuffer orientationsBuffer;

		public GraphicsBuffer startPositionsBuffer;

		public GraphicsBuffer endPositionsBuffer;

		public GraphicsBuffer startOrientationsBuffer;

		public GraphicsBuffer endOrientationsBuffer;

		public GraphicsBuffer restPositionsBuffer;

		public GraphicsBuffer prevPositionsBuffer;

		public GraphicsBuffer restOrientationsBuffer;

		public GraphicsBuffer prevOrientationsBuffer;

		public GraphicsBuffer renderablePositionsBuffer;

		public GraphicsBuffer renderableOrientationsBuffer;

		public GraphicsBuffer renderableRadiiBuffer;

		public GraphicsBuffer colorsBuffer;

		public GraphicsBuffer collisionMaterialIndexBuffer;

		public GraphicsBuffer principalRadiiBuffer;

		public GraphicsBuffer velocitiesBuffer;

		public GraphicsBuffer invMassesBuffer;

		public GraphicsBuffer phasesBuffer;

		public GraphicsBuffer filtersBuffer;

		public GraphicsBuffer angularVelocitiesBuffer;

		public GraphicsBuffer invRotationalMassesBuffer;

		public GraphicsBuffer externalForcesBuffer;

		public GraphicsBuffer externalTorquesBuffer;

		public GraphicsBuffer windBuffer;

		public GraphicsBuffer lifeBuffer;

		public GraphicsBuffer fluidDataBuffer;

		public GraphicsBuffer userDataBuffer;

		public GraphicsBuffer fluidMaterialsBuffer;

		public GraphicsBuffer fluidMaterials2Buffer;

		public GraphicsBuffer fluidInterfaceBuffer;

		public GraphicsBuffer anisotropiesBuffer;

		public GraphicsBuffer auxPositions;

		public GraphicsBuffer auxVelocities;

		public GraphicsBuffer auxColors;

		public GraphicsBuffer auxAttributes;

		public GraphicsBuffer auxOffsetInCell;

		public GraphicsBuffer auxSortedToOriginal;

		public GraphicsBuffer normalsBuffer;

		public GraphicsBuffer cellCoordsBuffer;

		public GraphicsBuffer positionDeltasIntBuffer;

		public GraphicsBuffer orientationDeltasIntBuffer;

		public GraphicsBuffer positionConstraintCountBuffer;

		public GraphicsBuffer orientationConstraintCountBuffer;

		public GraphicsBuffer activeParticlesBuffer;

		public GraphicsBuffer fluidDispatchBuffer;

		public GraphicsBuffer tangentsIntBuffer;

		public GraphicsBuffer deformableEdgesBuffer;

		public GraphicsBuffer deformableTrianglesBuffer;

		public GraphicsBuffer solverToWorldBuffer;

		public GraphicsBuffer worldToSolverBuffer;

		public GraphicsBuffer inertialFrameBuffer;

		private AffineTransform[] solverToWorldArray;

		private AffineTransform[] worldToSolverArray;

		private InertialFrame[] inertialFrameArray;

		public GraphicsBuffer rigidbodyLinearDeltasBuffer;

		public GraphicsBuffer rigidbodyAngularDeltasBuffer;

		public GraphicsBuffer rigidbodyLinearDeltasIntBuffer;

		public GraphicsBuffer rigidbodyAngularDeltasIntBuffer;

		public GraphicsBuffer reducedBounds;

		public SimplexCounts simplexCounts;

		public GraphicsBuffer simplices;

		public GraphicsBuffer simplexBounds;

		public Aabb solverBounds;

		private AsyncGPUReadbackRequest boundsRequest;

		private ComputeShader solverShader;

		private int applyInertialForcesKernel;

		private int applyRigidbodyDeltasKernel;

		private int predictPositionsKernel;

		private int updateVelocitiesKernel;

		private int updatePositionsKernel;

		private int updateLifetimesKernel;

		private int enforceLimitsKernel;

		private int interpolateKernel;

		private ComputeShader boundsShader;

		private int simplexBoundsKernel;

		private int editSimplexBoundsKernel;

		private int boundsReductionKernel;

		private ComputeShader deformableTrisShader;

		private int resetNormalsKernel;

		private int updateNormalsKernel;

		private int updateEdgeNormalsKernel;

		private int orientationFromNormalsKernel;

		private ComputeShader foamShader;

		private int sortDataKernel;

		private int emitShapeFoamKernel;

		private int emitFoamKernel;

		private int copyAliveKernel;

		private int updateFoamKernel;

		private int integrateFoamKernel;

		private int copyKernel;

		private ComputeShader foamDensityShader;

		private int clearGridKernel;

		private int insertGridKernel;

		private int sortByGridKernel;

		private int computeDensityKernel;

		private int applyDensityKernel;

		private ComputeShader foamCollisionShader;

		private int solveDiffuseContactsKernel;

		public ObiSolver abstraction => m_Solver;

		public int particleCount => m_Solver.positions.count;

		public int activeParticleCount => m_Solver.activeParticles.count;

		public int deformableTriangleCount => m_Solver.deformableTriangles.count / 3;

		public int deformableEdgeCount => m_Solver.deformableEdges.count / 2;

		public InertialFrame inertialFrame => m_InertialFrame;

		public uint activeFoamParticleCount { get; private set; }

		public ComputeSolverImpl(ObiSolver solver)
		{
			m_Solver = solver;
			jobHandle = new ComputeJobHandle();
			solverBounds = new Aabb(solver.transform.position - Vector3.one, solver.transform.position + Vector3.one);
			solver.queryResults.ResizeUninitialized((int)abstraction.maxQueryResults);
			solver.queryResults.SafeAsComputeBuffer<QueryResult>(GraphicsBuffer.Target.Counter);
			solver.foamCount.AsComputeBuffer<int>(GraphicsBuffer.Target.IndirectArguments);
			solver.foamPositions.AsComputeBuffer<Vector4>();
			solver.foamVelocities.AsComputeBuffer<Vector4>();
			solver.foamColors.AsComputeBuffer<Vector4>();
			solver.foamAttributes.AsComputeBuffer<Vector4>();
			solverShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/Solver"));
			applyInertialForcesKernel = solverShader.FindKernel("ApplyInertialForces");
			applyRigidbodyDeltasKernel = solverShader.FindKernel("ApplyRigidbodyDeltas");
			updateLifetimesKernel = solverShader.FindKernel("UpdateLifetimes");
			predictPositionsKernel = solverShader.FindKernel("PredictPositions");
			updateVelocitiesKernel = solverShader.FindKernel("UpdateVelocities");
			updatePositionsKernel = solverShader.FindKernel("UpdatePositions");
			enforceLimitsKernel = solverShader.FindKernel("EnforceLimits");
			interpolateKernel = solverShader.FindKernel("Interpolate");
			boundsShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/BoundsReduction"));
			simplexBoundsKernel = boundsShader.FindKernel("RuntimeSimplexBounds");
			editSimplexBoundsKernel = boundsShader.FindKernel("EditSimplexBounds");
			boundsReductionKernel = boundsShader.FindKernel("Reduce");
			deformableTrisShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/DeformableTriangles"));
			resetNormalsKernel = deformableTrisShader.FindKernel("ResetNormals");
			updateNormalsKernel = deformableTrisShader.FindKernel("UpdateNormals");
			updateEdgeNormalsKernel = deformableTrisShader.FindKernel("UpdateEdgeNormals");
			orientationFromNormalsKernel = deformableTrisShader.FindKernel("OrientationFromNormals");
			foamShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/FluidFoam"));
			sortDataKernel = foamShader.FindKernel("SortFluidData");
			emitShapeFoamKernel = foamShader.FindKernel("EmitShape");
			emitFoamKernel = foamShader.FindKernel("Emit");
			copyAliveKernel = foamShader.FindKernel("CopyAliveCount");
			updateFoamKernel = foamShader.FindKernel("Update");
			copyKernel = foamShader.FindKernel("Copy");
			integrateFoamKernel = foamShader.FindKernel("Integrate");
			foamDensityShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/FluidFoamDensity"));
			clearGridKernel = foamDensityShader.FindKernel("Clear");
			insertGridKernel = foamDensityShader.FindKernel("InsertInGrid");
			sortByGridKernel = foamDensityShader.FindKernel("SortByGrid");
			computeDensityKernel = foamDensityShader.FindKernel("ComputeDensity");
			applyDensityKernel = foamDensityShader.FindKernel("ApplyDensity");
			foamCollisionShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/FluidFoamCollisions"));
			solveDiffuseContactsKernel = foamCollisionShader.FindKernel("SolveDiffuseContacts");
			solverToWorldBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 1, 48);
			solverToWorldArray = new AffineTransform[1];
			worldToSolverBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 1, 48);
			worldToSolverArray = new AffineTransform[1];
			inertialFrameBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 1, 160);
			inertialFrameArray = new InertialFrame[1];
			fluidDispatchBuffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, 4, 4);
			GetOrCreateColliderWorld();
			if (colliderGrid != null)
			{
				colliderGrid.IncreaseReferenceCount();
			}
			particleGrid = new ComputeParticleGrid();
			spatialQueries = new SpatialQueries(solver.maxQueryResults);
			constraints = new IComputeConstraintsImpl[18];
			constraints[0] = new ComputeTetherConstraints(this);
			constraints[1] = new ComputeVolumeConstraints(this);
			constraints[2] = new ComputeChainConstraints(this);
			constraints[4] = new ComputeBendConstraints(this);
			constraints[5] = new ComputeDistanceConstraints(this);
			constraints[6] = new ComputeShapeMatchingConstraints(this);
			constraints[7] = new ComputeBendTwistConstraints(this);
			constraints[8] = new ComputeStretchShearConstraints(this);
			constraints[9] = new ComputePinConstraints(this);
			constraints[3] = new ComputePinholeConstraints(this);
			constraints[13] = new ComputeSkinConstraints(this);
			constraints[14] = new ComputeAerodynamicConstraints(this);
			constraints[15] = new ComputeStitchConstraints(this);
			constraints[10] = new ComputeParticleCollisionConstraints(this);
			constraints[10].CreateConstraintsBatch();
			constraints[12] = new ComputeColliderCollisionConstraints(this);
			constraints[12].CreateConstraintsBatch();
			constraints[16] = new ComputeParticleFrictionConstraints(this);
			constraints[16].CreateConstraintsBatch();
			constraints[17] = new ComputeColliderFrictionConstraints(this);
			constraints[17].CreateConstraintsBatch();
			constraints[11] = new ComputeDensityConstraints(this);
			constraints[11].CreateConstraintsBatch();
		}

		public void Destroy()
		{
			reducedBounds?.Dispose();
			solverToWorldBuffer?.Dispose();
			worldToSolverBuffer?.Dispose();
			inertialFrameBuffer?.Dispose();
			fluidDispatchBuffer?.Dispose();
			for (int i = 0; i < constraints.Length; i++)
			{
				if (constraints[i] != null)
				{
					constraints[i].Dispose();
				}
			}
			particleGrid?.Dispose();
			if (colliderGrid != null)
			{
				colliderGrid.DecreaseReferenceCount();
			}
			spatialQueries?.Dispose();
			positionDeltasIntBuffer?.Dispose();
			orientationDeltasIntBuffer?.Dispose();
			rigidbodyLinearDeltasIntBuffer?.Dispose();
			rigidbodyAngularDeltasIntBuffer?.Dispose();
			tangentsIntBuffer?.Dispose();
			simplexBounds?.Dispose();
			auxPositions?.Dispose();
			auxVelocities?.Dispose();
			auxColors?.Dispose();
			auxAttributes?.Dispose();
			auxOffsetInCell?.Dispose();
			auxSortedToOriginal?.Dispose();
		}

		private void GetOrCreateColliderWorld()
		{
			colliderGrid = Object.FindObjectOfType<ComputeColliderWorld>();
			if (colliderGrid == null)
			{
				GameObject gameObject = new GameObject("ComputeCollisionWorld", typeof(ComputeColliderWorld));
				colliderGrid = gameObject.GetComponent<ComputeColliderWorld>();
			}
		}

		public void PushData()
		{
			abstraction.positions.Upload();
			abstraction.orientations.Upload();
			abstraction.velocities.Upload();
			abstraction.angularVelocities.Upload();
			abstraction.colors.Upload();
			abstraction.startPositions.Upload();
			abstraction.startOrientations.Upload();
			abstraction.endPositions.Upload();
			abstraction.endOrientations.Upload();
			abstraction.restPositions.Upload();
			abstraction.restOrientations.Upload();
			abstraction.normals.Upload();
			abstraction.principalRadii.Upload();
			abstraction.invMasses.Upload();
			abstraction.invRotationalMasses.Upload();
			abstraction.phases.Upload();
			abstraction.filters.Upload();
			abstraction.externalForces.Upload();
			abstraction.externalTorques.Upload();
			abstraction.wind.Upload();
			abstraction.life.Upload();
			abstraction.fluidData.Upload();
			abstraction.userData.Upload();
			abstraction.fluidInterface.Upload();
			abstraction.fluidMaterials.Upload();
			abstraction.fluidMaterials2.Upload();
			rigidbodyLinearDeltasIntBuffer.SetData(abstraction.rigidbodyLinearDeltas.AsNativeArray<VInt4>());
			rigidbodyAngularDeltasIntBuffer.SetData(abstraction.rigidbodyAngularDeltas.AsNativeArray<VInt4>());
		}

		public void RequestReadback()
		{
			solverShader.SetBuffer(applyRigidbodyDeltasKernel, "linearDeltasAsInt", rigidbodyLinearDeltasIntBuffer);
			solverShader.SetBuffer(applyRigidbodyDeltasKernel, "angularDeltasAsInt", rigidbodyAngularDeltasIntBuffer);
			solverShader.SetBuffer(applyRigidbodyDeltasKernel, "linearDeltas", rigidbodyLinearDeltasBuffer);
			solverShader.SetBuffer(applyRigidbodyDeltasKernel, "angularDeltas", rigidbodyAngularDeltasBuffer);
			solverShader.SetInt("particleCount", abstraction.rigidbodyLinearDeltas.count);
			int threadGroupsX = ComputeMath.ThreadGroupCount(abstraction.rigidbodyLinearDeltas.count, 128);
			solverShader.Dispatch(applyRigidbodyDeltasKernel, threadGroupsX, 1, 1);
			abstraction.rigidbodyLinearDeltas.Readback();
			abstraction.rigidbodyAngularDeltas.Readback();
			abstraction.positions.Readback();
			abstraction.velocities.Readback();
			if (constraints[6] is ComputeShapeMatchingConstraints computeShapeMatchingConstraints)
			{
				computeShapeMatchingConstraints.RequestDataReadback();
			}
			if (constraints[5] is ComputeDistanceConstraints computeDistanceConstraints)
			{
				computeDistanceConstraints.RequestDataReadback();
			}
			if (constraints[9] is ComputePinConstraints computePinConstraints)
			{
				computePinConstraints.RequestDataReadback();
			}
			if (constraints[3] is ComputePinholeConstraints computePinholeConstraints)
			{
				computePinholeConstraints.RequestDataReadback();
			}
		}

		public void InitializeFrame(Vector4 translation, Vector4 scale, Quaternion rotation)
		{
			m_InertialFrame = new InertialFrame(translation, scale, rotation);
		}

		public void UpdateFrame(Vector4 translation, Vector4 scale, Quaternion rotation, float deltaTime)
		{
			m_InertialFrame.Update(translation, scale, rotation, deltaTime);
			solverToWorldArray[0] = m_InertialFrame.frame;
			solverToWorldBuffer.SetData(solverToWorldArray);
			worldToSolverArray[0] = m_InertialFrame.frame.Inverse();
			worldToSolverBuffer.SetData(worldToSolverArray);
			inertialFrameArray[0] = m_InertialFrame;
			inertialFrameBuffer.SetData(inertialFrameArray);
		}

		public IObiJobHandle ApplyFrame(float worldLinearInertiaScale, float worldAngularInertiaScale, float deltaTime)
		{
			if (activeParticleCount > 0)
			{
				Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.zero, inertialFrame.frame.rotation, new Vector3(1f / inertialFrame.frame.scale.x, 1f / inertialFrame.frame.scale.y, 1f / inertialFrame.frame.scale.z));
				Matrix4x4 matrix4x2 = Matrix4x4.Transpose(matrix4x);
				Vector4 val = (matrix4x2 * Matrix4x4.Scale(inertialFrame.angularVelocity) * matrix4x).Diagonal();
				Vector4 val2 = (matrix4x2 * Matrix4x4.Scale(inertialFrame.angularAcceleration) * matrix4x).Diagonal();
				Vector4 val3 = matrix4x2 * inertialFrame.acceleration;
				int threadGroupsX = ComputeMath.ThreadGroupCount(activeParticleCount, 128);
				solverShader.SetInt("particleCount", activeParticleCount);
				solverShader.SetFloat("deltaTime", deltaTime);
				solverShader.SetFloat("worldLinearInertiaScale", abstraction.worldLinearInertiaScale);
				solverShader.SetFloat("worldAngularInertiaScale", abstraction.worldAngularInertiaScale);
				solverShader.SetVector("angularVel", val);
				solverShader.SetVector("eulerAccel", val2);
				solverShader.SetVector("inertialAccel", val3);
				solverShader.SetVector("ambientWind", abstraction.parameters.ambientWind);
				solverShader.SetBool("inertialWind", abstraction.windSpace == Space.World);
				solverShader.SetBuffer(applyInertialForcesKernel, "activeParticles", activeParticlesBuffer);
				solverShader.SetBuffer(applyInertialForcesKernel, "positions", positionsBuffer);
				solverShader.SetBuffer(applyInertialForcesKernel, "velocities", velocitiesBuffer);
				solverShader.SetBuffer(applyInertialForcesKernel, "invMasses", invMassesBuffer);
				solverShader.SetBuffer(applyInertialForcesKernel, "wind", windBuffer);
				solverShader.SetBuffer(applyInertialForcesKernel, "inertialSolverFrame", inertialFrameBuffer);
				solverShader.Dispatch(applyInertialForcesKernel, threadGroupsX, 1, 1);
			}
			return jobHandle;
		}

		public void SetDeformableTriangles(ObiNativeIntList indices, ObiNativeVector2List uvs)
		{
			if (indices.count > 0)
			{
				deformableTrianglesBuffer = indices.AsComputeBuffer<int>();
				GraphicsBuffer buffer = uvs.AsComputeBuffer<Vector2>();
				deformableTrisShader.SetBuffer(updateNormalsKernel, "deformableTriangles", deformableTrianglesBuffer);
				deformableTrisShader.SetBuffer(updateNormalsKernel, "deformableTriangleUVs", buffer);
				deformableTrisShader.SetInt("triangleCount", deformableTriangleCount);
			}
		}

		public void SetDeformableEdges(ObiNativeIntList indices)
		{
			if (indices.count > 0)
			{
				deformableEdgesBuffer = indices.AsComputeBuffer<int>();
				deformableTrisShader.SetBuffer(updateEdgeNormalsKernel, "deformableEdges", deformableEdgesBuffer);
				deformableTrisShader.SetInt("edgeCount", deformableEdgeCount);
			}
		}

		public void SetSimplices(ObiNativeIntList simplices, SimplexCounts counts)
		{
			simplexCounts = counts;
			if (simplices.count > 0)
			{
				boundsShader.SetInt("pointCount", simplexCounts.pointCount);
				boundsShader.SetInt("edgeCount", simplexCounts.edgeCount);
				boundsShader.SetInt("triangleCount", simplexCounts.triangleCount);
				this.simplices = simplices.AsComputeBuffer<int>();
				cellCoordsBuffer = abstraction.cellCoords.AsComputeBuffer<VInt4>();
				if (simplexBounds == null || counts.simplexCount > simplexBounds.count)
				{
					simplexBounds?.Dispose();
					simplexBounds = new GraphicsBuffer(GraphicsBuffer.Target.Structured, counts.simplexCount * 2, 32);
					reducedBounds?.Dispose();
					reducedBounds = new GraphicsBuffer(GraphicsBuffer.Target.Structured, ComputeMath.NextMultiple(counts.simplexCount * 2, 256), 32);
				}
				if (particleGrid != null && particleGrid.SetCapacity(Mathf.Max(counts.simplexCount, particleCount), (uint)Mathf.Max(1f, abstraction.maxParticleContacts), (uint)Mathf.Max(1f, abstraction.maxParticleNeighbors)))
				{
					abstraction.colliderContacts.ResizeUninitialized(particleGrid.contactPairs.count);
					abstraction.colliderContacts.SafeAsComputeBuffer<Oni.Contact>(GraphicsBuffer.Target.Counter);
					abstraction.particleContacts.ResizeUninitialized(particleGrid.contactPairs.count);
					abstraction.particleContacts.SafeAsComputeBuffer<Oni.Contact>(GraphicsBuffer.Target.Counter);
					abstraction.contactEffectiveMasses.ResizeUninitialized(particleGrid.contactPairs.count);
					abstraction.contactEffectiveMasses.SafeAsComputeBuffer<ContactEffectiveMasses>();
					abstraction.particleContactEffectiveMasses.ResizeUninitialized(particleGrid.contactPairs.count);
					abstraction.particleContactEffectiveMasses.SafeAsComputeBuffer<ContactEffectiveMasses>();
				}
			}
			else
			{
				this.simplices = null;
			}
		}

		public void SetActiveParticles(ObiNativeIntList indices)
		{
			if (indices.computeBuffer == null || indices.computeBuffer.count != indices.capacity)
			{
				activeParticlesBuffer = indices.AsComputeBuffer<int>(indices.capacity);
			}
			else
			{
				indices.UploadFullCapacity();
			}
			if (activeParticlesBuffer != null)
			{
				solverShader.SetBuffer(predictPositionsKernel, "activeParticles", activeParticlesBuffer);
				solverShader.SetBuffer(updateVelocitiesKernel, "activeParticles", activeParticlesBuffer);
				solverShader.SetBuffer(updatePositionsKernel, "activeParticles", activeParticlesBuffer);
				solverShader.SetBuffer(enforceLimitsKernel, "activeParticles", activeParticlesBuffer);
			}
		}

		public IObiJobHandle UpdateBounds(IObiJobHandle inputDeps, float stepTime)
		{
			if (activeParticleCount > 0 && reducedBounds != null)
			{
				boundsShader.SetFloat("deltaTime", stepTime);
				boundsShader.SetFloat("collisionMargin", abstraction.parameters.collisionMargin);
				boundsShader.SetFloat("particleCCD", abstraction.parameters.particleCCD);
				int num = simplexCounts.simplexCount;
				int num2 = ComputeMath.ThreadGroupCount(num, 256);
				if (colliderGrid.materialsBuffer != null)
				{
					boundsShader.SetBuffer(simplexBoundsKernel, "simplexBounds", simplexBounds);
					boundsShader.SetBuffer(simplexBoundsKernel, "simplices", simplices);
					boundsShader.SetBuffer(simplexBoundsKernel, "reducedBounds", reducedBounds);
					boundsShader.SetBuffer(simplexBoundsKernel, "activeParticles", activeParticlesBuffer);
					boundsShader.SetBuffer(simplexBoundsKernel, "positions", positionsBuffer);
					boundsShader.SetBuffer(simplexBoundsKernel, "velocities", velocitiesBuffer);
					boundsShader.SetBuffer(simplexBoundsKernel, "principalRadii", principalRadiiBuffer);
					boundsShader.SetBuffer(simplexBoundsKernel, "fluidMaterials", fluidMaterialsBuffer);
					boundsShader.SetBuffer(simplexBoundsKernel, "collisionMaterials", colliderGrid.materialsBuffer);
					boundsShader.SetBuffer(simplexBoundsKernel, "collisionMaterialIndices", collisionMaterialIndexBuffer);
					boundsShader.Dispatch(simplexBoundsKernel, num2, 1, 1);
				}
				else
				{
					boundsShader.SetBuffer(editSimplexBoundsKernel, "simplexBounds", simplexBounds);
					boundsShader.SetBuffer(editSimplexBoundsKernel, "simplices", simplices);
					boundsShader.SetBuffer(editSimplexBoundsKernel, "reducedBounds", reducedBounds);
					boundsShader.SetBuffer(editSimplexBoundsKernel, "activeParticles", activeParticlesBuffer);
					boundsShader.SetBuffer(editSimplexBoundsKernel, "positions", positionsBuffer);
					boundsShader.SetBuffer(editSimplexBoundsKernel, "velocities", velocitiesBuffer);
					boundsShader.SetBuffer(editSimplexBoundsKernel, "principalRadii", principalRadiiBuffer);
					boundsShader.SetBuffer(editSimplexBoundsKernel, "fluidMaterials", fluidMaterialsBuffer);
					boundsShader.Dispatch(editSimplexBoundsKernel, num2, 1, 1);
				}
				boundsShader.SetBuffer(boundsReductionKernel, "reducedBounds", reducedBounds);
				do
				{
					boundsShader.Dispatch(boundsReductionKernel, num2, 1, 1);
					num2 = ComputeMath.ThreadGroupCount(num, 256);
					num /= 256;
				}
				while (num2 > 1);
				boundsRequest = AsyncGPUReadback.Request(reducedBounds, 32, 0);
				solverShader.SetFloat("deltaTime", stepTime);
				solverShader.SetBuffer(updateLifetimesKernel, "activeParticles", activeParticlesBuffer);
				solverShader.SetBuffer(updateLifetimesKernel, "life", lifeBuffer);
				solverShader.SetBuffer(updateLifetimesKernel, "deadParticles", deadIndicesBuffer);
				solverShader.Dispatch(updateLifetimesKernel, ComputeMath.ThreadGroupCount(activeParticleCount, 128), 1, 1);
			}
			return inputDeps;
		}

		public void GetBounds(ref Vector3 min, ref Vector3 max)
		{
			boundsRequest.WaitForCompletion();
			if (boundsRequest.done && !boundsRequest.hasError)
			{
				solverBounds = boundsRequest.GetData<Aabb>()[0];
			}
			min = solverBounds.min;
			max = solverBounds.max;
		}

		public int GetConstraintCount(Oni.ConstraintType type)
		{
			return 0;
		}

		public void SetParameters(Oni.SolverParameters parameters)
		{
			solverShader.SetInt("mode", (int)parameters.mode);
			solverShader.SetInt("interpolation", (int)parameters.interpolation);
			solverShader.SetVector("gravity", parameters.gravity);
			solverShader.SetFloat("damping", parameters.damping);
			solverShader.SetFloat("sleepThreshold", parameters.sleepThreshold);
			solverShader.SetFloat("collisionMargin", parameters.collisionMargin);
			solverShader.SetFloat("maxVelocity", parameters.maxVelocity);
			solverShader.SetFloat("maxAngularVelocity", parameters.maxAngularVelocity);
		}

		public void SetConstraintGroupParameters(Oni.ConstraintType type, ref Oni.ConstraintParameters parameters)
		{
		}

		public void ParticleCountChanged(ObiSolver solver)
		{
			deadIndicesBuffer = abstraction.deadParticles.AsComputeBuffer<int>(abstraction.deadParticles.capacity, GraphicsBuffer.Target.Counter);
			colorsBuffer = abstraction.colors.AsComputeBuffer<Vector4>();
			positionsBuffer = abstraction.positions.AsComputeBuffer<Vector4>();
			orientationsBuffer = abstraction.orientations.AsComputeBuffer<Quaternion>();
			startPositionsBuffer = abstraction.startPositions.AsComputeBuffer<Vector4>();
			endPositionsBuffer = abstraction.endPositions.AsComputeBuffer<Vector4>();
			startOrientationsBuffer = abstraction.startOrientations.AsComputeBuffer<Quaternion>();
			endOrientationsBuffer = abstraction.endOrientations.AsComputeBuffer<Quaternion>();
			restPositionsBuffer = abstraction.restPositions.AsComputeBuffer<Vector4>();
			restOrientationsBuffer = abstraction.restOrientations.AsComputeBuffer<Vector4>();
			prevPositionsBuffer = abstraction.prevPositions.AsComputeBuffer<Vector4>();
			prevOrientationsBuffer = abstraction.prevOrientations.AsComputeBuffer<Quaternion>();
			renderablePositionsBuffer = abstraction.renderablePositions.AsComputeBuffer<Vector4>();
			renderableOrientationsBuffer = abstraction.renderableOrientations.AsComputeBuffer<Quaternion>();
			renderableRadiiBuffer = abstraction.renderableRadii.AsComputeBuffer<Vector4>();
			collisionMaterialIndexBuffer = abstraction.collisionMaterials.AsComputeBuffer<int>();
			angularVelocitiesBuffer = abstraction.angularVelocities.AsComputeBuffer<Vector4>();
			invRotationalMassesBuffer = abstraction.invRotationalMasses.AsComputeBuffer<float>();
			externalForcesBuffer = abstraction.externalForces.AsComputeBuffer<Vector4>();
			externalTorquesBuffer = abstraction.externalTorques.AsComputeBuffer<Vector4>();
			windBuffer = abstraction.wind.AsComputeBuffer<Vector4>();
			velocitiesBuffer = abstraction.velocities.AsComputeBuffer<Vector4>();
			principalRadiiBuffer = abstraction.principalRadii.AsComputeBuffer<Vector4>();
			invMassesBuffer = abstraction.invMasses.AsComputeBuffer<float>();
			phasesBuffer = abstraction.phases.AsComputeBuffer<int>();
			filtersBuffer = abstraction.filters.AsComputeBuffer<int>();
			lifeBuffer = abstraction.life.AsComputeBuffer<float>();
			fluidDataBuffer = abstraction.fluidData.AsComputeBuffer<Vector4>();
			userDataBuffer = abstraction.userData.AsComputeBuffer<Vector4>();
			fluidInterfaceBuffer = abstraction.fluidInterface.AsComputeBuffer<Vector4>();
			fluidMaterialsBuffer = abstraction.fluidMaterials.AsComputeBuffer<Vector4>();
			fluidMaterials2Buffer = abstraction.fluidMaterials2.AsComputeBuffer<Vector4>();
			anisotropiesBuffer = abstraction.anisotropies.AsComputeBuffer<Matrix4x4>();
			normalsBuffer = abstraction.normals.AsComputeBuffer<Vector4>();
			positionConstraintCountBuffer = abstraction.positionConstraintCounts.AsComputeBuffer<int>();
			orientationConstraintCountBuffer = abstraction.orientationConstraintCounts.AsComputeBuffer<int>();
			if (positionDeltasIntBuffer != null)
			{
				positionDeltasIntBuffer.Dispose();
				positionDeltasIntBuffer = null;
			}
			if (abstraction.positionDeltas.count > 0)
			{
				positionDeltasIntBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, abstraction.positionDeltas.count, abstraction.positionDeltas.stride);
				positionDeltasIntBuffer.SetData(new Vector4[abstraction.positionDeltas.count]);
			}
			if (orientationDeltasIntBuffer != null)
			{
				orientationDeltasIntBuffer.Dispose();
				orientationDeltasIntBuffer = null;
			}
			if (abstraction.orientationDeltas.count > 0)
			{
				orientationDeltasIntBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, abstraction.orientationDeltas.count, abstraction.orientationDeltas.stride);
				orientationDeltasIntBuffer.SetData(new Vector4[abstraction.orientationDeltas.count]);
			}
			if (tangentsIntBuffer != null)
			{
				tangentsIntBuffer.Dispose();
				tangentsIntBuffer = null;
			}
			if (abstraction.normals.count > 0)
			{
				tangentsIntBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, abstraction.normals.count, abstraction.normals.stride);
				tangentsIntBuffer.SetData(new VInt4[abstraction.normals.count]);
			}
			if (positionsBuffer != null)
			{
				solverShader.SetBuffer(predictPositionsKernel, "positions", positionsBuffer);
			}
			if (prevPositionsBuffer != null)
			{
				solverShader.SetBuffer(predictPositionsKernel, "prevPositions", prevPositionsBuffer);
			}
			if (orientationsBuffer != null)
			{
				solverShader.SetBuffer(predictPositionsKernel, "orientations", orientationsBuffer);
			}
			if (prevOrientationsBuffer != null)
			{
				solverShader.SetBuffer(predictPositionsKernel, "prevOrientations", prevOrientationsBuffer);
			}
			if (velocitiesBuffer != null)
			{
				solverShader.SetBuffer(predictPositionsKernel, "velocities", velocitiesBuffer);
			}
			if (invMassesBuffer != null)
			{
				solverShader.SetBuffer(predictPositionsKernel, "invMasses", invMassesBuffer);
			}
			if (angularVelocitiesBuffer != null)
			{
				solverShader.SetBuffer(predictPositionsKernel, "angularVelocities", angularVelocitiesBuffer);
			}
			if (invRotationalMassesBuffer != null)
			{
				solverShader.SetBuffer(predictPositionsKernel, "invRotationalMasses", invRotationalMassesBuffer);
			}
			if (externalForcesBuffer != null)
			{
				solverShader.SetBuffer(predictPositionsKernel, "externalForces", externalForcesBuffer);
			}
			if (externalTorquesBuffer != null)
			{
				solverShader.SetBuffer(predictPositionsKernel, "externalTorques", externalTorquesBuffer);
			}
			if (phasesBuffer != null)
			{
				solverShader.SetBuffer(predictPositionsKernel, "phases", phasesBuffer);
			}
			if (fluidInterfaceBuffer != null)
			{
				solverShader.SetBuffer(predictPositionsKernel, "buoyancies", fluidInterfaceBuffer);
			}
			if (positionsBuffer != null)
			{
				solverShader.SetBuffer(updateVelocitiesKernel, "positions", positionsBuffer);
			}
			if (prevPositionsBuffer != null)
			{
				solverShader.SetBuffer(updateVelocitiesKernel, "prevPositions", prevPositionsBuffer);
			}
			if (orientationsBuffer != null)
			{
				solverShader.SetBuffer(updateVelocitiesKernel, "orientations", orientationsBuffer);
			}
			if (prevOrientationsBuffer != null)
			{
				solverShader.SetBuffer(updateVelocitiesKernel, "prevOrientations", prevOrientationsBuffer);
			}
			if (velocitiesBuffer != null)
			{
				solverShader.SetBuffer(updateVelocitiesKernel, "velocities", velocitiesBuffer);
			}
			if (angularVelocitiesBuffer != null)
			{
				solverShader.SetBuffer(updateVelocitiesKernel, "angularVelocities", angularVelocitiesBuffer);
			}
			if (invMassesBuffer != null)
			{
				solverShader.SetBuffer(updateVelocitiesKernel, "invMasses", invMassesBuffer);
			}
			if (invRotationalMassesBuffer != null)
			{
				solverShader.SetBuffer(updateVelocitiesKernel, "invRotationalMasses", invRotationalMassesBuffer);
			}
			if (positionsBuffer != null)
			{
				solverShader.SetBuffer(updatePositionsKernel, "positions", positionsBuffer);
			}
			if (prevPositionsBuffer != null)
			{
				solverShader.SetBuffer(updatePositionsKernel, "prevPositions", prevPositionsBuffer);
			}
			if (orientationsBuffer != null)
			{
				solverShader.SetBuffer(updatePositionsKernel, "orientations", orientationsBuffer);
			}
			if (prevOrientationsBuffer != null)
			{
				solverShader.SetBuffer(updatePositionsKernel, "prevOrientations", prevOrientationsBuffer);
			}
			if (velocitiesBuffer != null)
			{
				solverShader.SetBuffer(updatePositionsKernel, "velocities", velocitiesBuffer);
			}
			if (angularVelocitiesBuffer != null)
			{
				solverShader.SetBuffer(updatePositionsKernel, "angularVelocities", angularVelocitiesBuffer);
			}
			if (positionsBuffer != null)
			{
				solverShader.SetBuffer(enforceLimitsKernel, "positions", positionsBuffer);
			}
			if (prevPositionsBuffer != null)
			{
				solverShader.SetBuffer(enforceLimitsKernel, "prevPositions", prevPositionsBuffer);
			}
			if (lifeBuffer != null)
			{
				solverShader.SetBuffer(enforceLimitsKernel, "life", lifeBuffer);
			}
			if (phasesBuffer != null)
			{
				solverShader.SetBuffer(enforceLimitsKernel, "phases", phasesBuffer);
			}
			if (positionsBuffer != null)
			{
				solverShader.SetBuffer(interpolateKernel, "positions", positionsBuffer);
			}
			if (startPositionsBuffer != null)
			{
				solverShader.SetBuffer(interpolateKernel, "startPositions", startPositionsBuffer);
			}
			if (endPositionsBuffer != null)
			{
				solverShader.SetBuffer(interpolateKernel, "endPositions", endPositionsBuffer);
			}
			if (renderablePositionsBuffer != null)
			{
				solverShader.SetBuffer(interpolateKernel, "renderablePositions", renderablePositionsBuffer);
			}
			if (orientationsBuffer != null)
			{
				solverShader.SetBuffer(interpolateKernel, "orientations", orientationsBuffer);
			}
			if (startOrientationsBuffer != null)
			{
				solverShader.SetBuffer(interpolateKernel, "startOrientations", startOrientationsBuffer);
			}
			if (endOrientationsBuffer != null)
			{
				solverShader.SetBuffer(interpolateKernel, "endOrientations", endOrientationsBuffer);
			}
			if (renderableOrientationsBuffer != null)
			{
				solverShader.SetBuffer(interpolateKernel, "renderableOrientations", renderableOrientationsBuffer);
			}
			if (principalRadiiBuffer != null)
			{
				solverShader.SetBuffer(interpolateKernel, "principalRadii", principalRadiiBuffer);
			}
			if (renderableRadiiBuffer != null)
			{
				solverShader.SetBuffer(interpolateKernel, "renderableRadii", renderableRadiiBuffer);
			}
		}

		public void MaxFoamParticleCountChanged(ObiSolver solver)
		{
			auxPositions?.Dispose();
			auxVelocities?.Dispose();
			auxColors?.Dispose();
			auxAttributes?.Dispose();
			auxOffsetInCell?.Dispose();
			auxSortedToOriginal?.Dispose();
			if (m_Solver.maxFoamParticles != 0)
			{
				solver.foamPositions.AsComputeBuffer<Vector4>();
				solver.foamVelocities.AsComputeBuffer<Vector4>();
				solver.foamColors.AsComputeBuffer<Vector4>();
				solver.foamAttributes.AsComputeBuffer<Vector4>();
				auxPositions = new GraphicsBuffer(GraphicsBuffer.Target.Structured, (int)m_Solver.maxFoamParticles, 16);
				auxVelocities = new GraphicsBuffer(GraphicsBuffer.Target.Structured, (int)m_Solver.maxFoamParticles, 16);
				auxColors = new GraphicsBuffer(GraphicsBuffer.Target.Structured, (int)m_Solver.maxFoamParticles, 16);
				auxAttributes = new GraphicsBuffer(GraphicsBuffer.Target.Structured, (int)m_Solver.maxFoamParticles, 16);
				auxOffsetInCell = new GraphicsBuffer(GraphicsBuffer.Target.Structured, (int)m_Solver.maxFoamParticles, 4);
				auxSortedToOriginal = new GraphicsBuffer(GraphicsBuffer.Target.Structured, (int)m_Solver.maxFoamParticles, 4);
			}
		}

		public void SetRigidbodyArrays(ObiSolver solver)
		{
			rigidbodyLinearDeltasBuffer = solver.rigidbodyLinearDeltas.SafeAsComputeBuffer<Vector4>();
			rigidbodyAngularDeltasBuffer = solver.rigidbodyAngularDeltas.SafeAsComputeBuffer<Vector4>();
			if (rigidbodyLinearDeltasIntBuffer != null)
			{
				rigidbodyLinearDeltasIntBuffer.Dispose();
				rigidbodyLinearDeltasIntBuffer = null;
			}
			if (rigidbodyAngularDeltasIntBuffer != null)
			{
				rigidbodyAngularDeltasIntBuffer.Dispose();
				rigidbodyAngularDeltasIntBuffer = null;
			}
			rigidbodyLinearDeltasIntBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, rigidbodyLinearDeltasBuffer.count, solver.rigidbodyLinearDeltas.stride);
			rigidbodyLinearDeltasIntBuffer.SetData(new Vector4[rigidbodyLinearDeltasBuffer.count]);
			rigidbodyAngularDeltasIntBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, rigidbodyAngularDeltasBuffer.count, solver.rigidbodyAngularDeltas.stride);
			rigidbodyAngularDeltasIntBuffer.SetData(new Vector4[rigidbodyAngularDeltasBuffer.count]);
		}

		public IConstraintsBatchImpl CreateConstraintsBatch(Oni.ConstraintType type)
		{
			if (constraints[(int)type] != null)
			{
				return constraints[(int)type].CreateConstraintsBatch();
			}
			return null;
		}

		public void DestroyConstraintsBatch(IConstraintsBatchImpl batch)
		{
			if (batch != null && constraints[(int)batch.constraintType] != null)
			{
				constraints[(int)batch.constraintType].RemoveBatch(batch);
			}
		}

		public void FinishSimulation()
		{
			abstraction.positions.WaitForReadback();
			abstraction.velocities.WaitForReadback();
			abstraction.rigidbodyLinearDeltas.WaitForReadback();
			abstraction.rigidbodyAngularDeltas.WaitForReadback();
			if (constraints[6] is ComputeShapeMatchingConstraints computeShapeMatchingConstraints)
			{
				computeShapeMatchingConstraints.WaitForReadback();
			}
			if (constraints[5] is ComputeDistanceConstraints computeDistanceConstraints)
			{
				computeDistanceConstraints.WaitForReadback();
			}
			if (constraints[9] is ComputePinConstraints computePinConstraints)
			{
				computePinConstraints.WaitForReadback();
			}
			if (constraints[3] is ComputePinholeConstraints computePinholeConstraints)
			{
				computePinholeConstraints.WaitForReadback();
			}
			abstraction.externalForces.WipeToZero();
			abstraction.externalTorques.WipeToZero();
			abstraction.externalForces.Upload();
			abstraction.externalTorques.Upload();
			abstraction.startPositions.CopyFrom(abstraction.endPositions);
			abstraction.startOrientations.CopyFrom(abstraction.endOrientations);
			abstraction.endPositions.CopyFrom(abstraction.positions);
			abstraction.endOrientations.CopyFrom(abstraction.orientations);
			abstraction.startPositions.Upload(force: true);
			abstraction.startOrientations.Upload(force: true);
			abstraction.endPositions.Upload(force: true);
			abstraction.endOrientations.Upload(force: true);
		}

		public IObiJobHandle CollisionDetection(IObiJobHandle inputDeps, float stepTime)
		{
			Oni.ConstraintParameters constraintParameters = abstraction.GetConstraintParameters(Oni.ConstraintType.Collision);
			Oni.ConstraintParameters constraintParameters2 = abstraction.GetConstraintParameters(Oni.ConstraintType.ParticleCollision);
			Oni.ConstraintParameters constraintParameters3 = abstraction.GetConstraintParameters(Oni.ConstraintType.Density);
			if (constraintParameters2.enabled || constraintParameters3.enabled)
			{
				UpdateDiffuseDensity(stepTime);
				particleGrid.BuildGrid(this, stepTime);
				if (constraintParameters3.enabled)
				{
					particleGrid.GenerateFluidNeighborhoods(this);
				}
				if (constraintParameters2.enabled)
				{
					particleGrid.GenerateContacts(this);
				}
			}
			if (constraintParameters.enabled)
			{
				colliderGrid.GenerateContacts(this, stepTime);
				colliderGrid.ApplyForceZones(this, stepTime);
			}
			return inputDeps;
		}

		public IObiJobHandle Substep(IObiJobHandle handle, float stepTime, float substepTime, int steps, float timeLeft)
		{
			int num = ComputeMath.ThreadGroupCount(activeParticleCount, 128);
			solverShader.SetInt("particleCount", activeParticleCount);
			if (activeParticleCount > 0)
			{
				solverShader.SetFloat("deltaTime", substepTime);
				solverShader.SetFloat("velocityScale", Mathf.Pow(1f - Mathf.Clamp(m_Solver.parameters.damping, 0f, 1f), substepTime));
				solverShader.Dispatch(predictPositionsKernel, num, 1, 1);
				ApplyConstraints(stepTime, substepTime, steps, timeLeft);
				EnforceLimits(num);
				solverShader.Dispatch(updateVelocitiesKernel, num, 1, 1);
				CalculateVelocityCorrections(stepTime, substepTime, steps, timeLeft);
			}
			int num2 = Mathf.RoundToInt(timeLeft / substepTime);
			int num3 = Mathf.CeilToInt((float)abstraction.substeps / (float)abstraction.foamSubsteps);
			if (num2 % num3 == 0)
			{
				UpdateDiffuseParticles(substepTime * (float)num3);
				UpdateDiffuseCollisions(substepTime * (float)num3);
				IntegrateDiffuseParticles(substepTime * (float)num3);
			}
			if (activeParticleCount > 0)
			{
				UpdatePositions(substepTime, num);
			}
			return handle;
		}

		private void CalculateVelocityCorrections(float stepTime, float substepTime, int steps, float timeLeft)
		{
			constraints[14].Project(stepTime, substepTime, steps, timeLeft);
			if (m_Solver.GetConstraintParameters(Oni.ConstraintType.Density).enabled && constraints[11] is ComputeDensityConstraints computeDensityConstraints)
			{
				computeDensityConstraints.CalculateVelocityCorrections(substepTime);
			}
		}

		private void UpdatePositions(float deltaTime, int threadGroups)
		{
			if (m_Solver.GetConstraintParameters(Oni.ConstraintType.Density).enabled && constraints[11] is ComputeDensityConstraints computeDensityConstraints)
			{
				computeDensityConstraints.ApplyVelocityCorrections(deltaTime);
			}
			solverShader.Dispatch(updatePositionsKernel, threadGroups, 1, 1);
		}

		private void ApplyConstraints(float stepTime, float substepTime, int steps, float timeLeft)
		{
			int num = 0;
			for (int i = 0; i < 18; i++)
			{
				Oni.ConstraintParameters constraintParameters = m_Solver.GetConstraintParameters((Oni.ConstraintType)i);
				if (constraintParameters.enabled)
				{
					num = Mathf.Max(num, constraintParameters.iterations);
					constraints[i].Initialize(stepTime, substepTime, steps, timeLeft);
				}
			}
			for (int j = 0; j < 18; j++)
			{
				Oni.ConstraintParameters constraintParameters2 = m_Solver.GetConstraintParameters((Oni.ConstraintType)j);
				if (constraintParameters2.enabled && constraintParameters2.iterations > 0)
				{
					padding[j] = Mathf.CeilToInt((float)num / (float)constraintParameters2.iterations);
				}
				else
				{
					padding[j] = num;
				}
			}
			for (int k = 1; k < num; k++)
			{
				for (int l = 0; l < 18; l++)
				{
					if (l != 14 && m_Solver.GetConstraintParameters((Oni.ConstraintType)l).enabled && k % padding[l] == 0)
					{
						constraints[l].Project(stepTime, substepTime, steps, timeLeft);
					}
				}
			}
			for (int m = 0; m < 18; m++)
			{
				if (m != 14)
				{
					Oni.ConstraintParameters constraintParameters3 = m_Solver.GetConstraintParameters((Oni.ConstraintType)m);
					if (constraintParameters3.enabled && constraintParameters3.iterations > 0)
					{
						constraints[m].Project(stepTime, substepTime, steps, timeLeft);
					}
				}
			}
			Oni.ConstraintParameters constraintParameters4 = m_Solver.GetConstraintParameters(Oni.ConstraintType.ParticleCollision);
			if (constraintParameters4.enabled && constraintParameters4.iterations > 0)
			{
				constraints[10].Project(stepTime, substepTime, steps, timeLeft);
			}
			constraintParameters4 = m_Solver.GetConstraintParameters(Oni.ConstraintType.Collision);
			if (constraintParameters4.enabled && constraintParameters4.iterations > 0)
			{
				constraints[12].Project(stepTime, substepTime, steps, timeLeft);
			}
		}

		private void EnforceLimits(int threadGroups)
		{
			if (abstraction.useLimits)
			{
				solverShader.SetBool("killOffLimits", abstraction.killOffLimitsParticles);
				solverShader.SetVector("boundaryLimitsMin", abstraction.boundaryLimits.min);
				solverShader.SetVector("boundaryLimitsMax", abstraction.boundaryLimits.max);
				solverShader.Dispatch(enforceLimitsKernel, threadGroups, 1, 1);
			}
		}

		public IObiJobHandle ApplyInterpolation(IObiJobHandle inputDeps, ObiNativeVector4List startPositions, ObiNativeQuaternionList startOrientations, float stepTime, float unsimulatedTime)
		{
			if (particleCount <= 0)
			{
				return inputDeps;
			}
			int threadGroupsX = ComputeMath.ThreadGroupCount(particleCount, 128);
			solverShader.SetInt("particleCount", particleCount);
			solverShader.SetFloat("deltaTime", stepTime);
			solverShader.SetFloat("blendFactor", (stepTime > 0f) ? (unsimulatedTime / stepTime) : 0f);
			solverShader.SetInt("interpolate", (int)m_Solver.parameters.interpolation);
			solverShader.Dispatch(interpolateKernel, threadGroupsX, 1, 1);
			if ((deformableTriangleCount > 0 || deformableEdgeCount > 0) && normalsBuffer != null)
			{
				threadGroupsX = ComputeMath.ThreadGroupCount(normalsBuffer.count, 128);
				deformableTrisShader.SetInt("normalsCount", normalsBuffer.count);
				deformableTrisShader.SetBuffer(resetNormalsKernel, "phases", phasesBuffer);
				deformableTrisShader.SetBuffer(resetNormalsKernel, "normals", normalsBuffer);
				deformableTrisShader.SetBuffer(resetNormalsKernel, "tangentsInt", tangentsIntBuffer);
				deformableTrisShader.Dispatch(resetNormalsKernel, threadGroupsX, 1, 1);
				if (deformableTriangleCount > 0)
				{
					threadGroupsX = ComputeMath.ThreadGroupCount(deformableTriangleCount, 128);
					deformableTrisShader.SetBuffer(updateNormalsKernel, "renderablePositions", renderablePositionsBuffer);
					deformableTrisShader.SetBuffer(updateNormalsKernel, "normalsInt", normalsBuffer);
					deformableTrisShader.SetBuffer(updateNormalsKernel, "tangentsInt", tangentsIntBuffer);
					deformableTrisShader.Dispatch(updateNormalsKernel, threadGroupsX, 1, 1);
				}
				if (deformableEdgeCount > 0)
				{
					threadGroupsX = ComputeMath.ThreadGroupCount(deformableEdgeCount, 128);
					deformableTrisShader.SetBuffer(updateEdgeNormalsKernel, "renderablePositions", renderablePositionsBuffer);
					deformableTrisShader.SetBuffer(updateEdgeNormalsKernel, "velocities", velocitiesBuffer);
					deformableTrisShader.SetBuffer(updateEdgeNormalsKernel, "wind", windBuffer);
					deformableTrisShader.SetBuffer(updateEdgeNormalsKernel, "normalsInt", normalsBuffer);
					deformableTrisShader.Dispatch(updateEdgeNormalsKernel, threadGroupsX, 1, 1);
				}
				threadGroupsX = ComputeMath.ThreadGroupCount(normalsBuffer.count, 128);
				deformableTrisShader.SetBuffer(orientationFromNormalsKernel, "phases", phasesBuffer);
				deformableTrisShader.SetBuffer(orientationFromNormalsKernel, "renderableOrientations", renderableOrientationsBuffer);
				deformableTrisShader.SetBuffer(orientationFromNormalsKernel, "normals", normalsBuffer);
				deformableTrisShader.SetBuffer(orientationFromNormalsKernel, "tangentsInt", tangentsIntBuffer);
				deformableTrisShader.Dispatch(orientationFromNormalsKernel, threadGroupsX, 1, 1);
			}
			Oni.ConstraintParameters constraintParameters = abstraction.GetConstraintParameters(Oni.ConstraintType.Pin);
			if (constraintParameters.enabled && constraintParameters.iterations > 0)
			{
				ComputePinConstraints computePinConstraints = constraints[9] as ComputePinConstraints;
				if (Application.isPlaying)
				{
					computePinConstraints?.ProjectRenderablePositions();
				}
			}
			Oni.ConstraintParameters constraintParameters2 = m_Solver.GetConstraintParameters(Oni.ConstraintType.Density);
			if (constraintParameters2.enabled && constraintParameters2.iterations > 0)
			{
				ComputeDensityConstraints computeDensityConstraints = constraints[11] as ComputeDensityConstraints;
				if (Application.isPlaying)
				{
					computeDensityConstraints?.CalculateAnisotropyLaplacianSmoothing();
				}
			}
			return inputDeps;
		}

		private void UpdateDiffuseDensity(float deltaTime)
		{
			if (abstraction.GetRenderSystem<ObiFoamGenerator>() is ComputeFoamRenderSystem computeFoamRenderSystem && m_Solver.maxFoamParticles != 0 && particleGrid.cellCounts != null)
			{
				for (int i = 0; i < computeFoamRenderSystem.renderers.Count; i++)
				{
					if (computeFoamRenderSystem.renderers[i].pressure > 0f && computeFoamRenderSystem.renderers[i].actor.solverIndices?.computeBuffer != null)
					{
						float num = 0.01f + Mathf.Clamp01(1f - computeFoamRenderSystem.renderers[i].density);
						float num2 = computeFoamRenderSystem.renderers[i].size * num;
						int threadGroupsX = ComputeMath.ThreadGroupCount(particleGrid.cellCounts.count, 128);
						foamDensityShader.SetFloat("deltaTime", deltaTime);
						foamDensityShader.SetInt("maxCells", particleGrid.cellCounts.count);
						foamDensityShader.SetInt("maxFoamParticles", abstraction.foamPositions.computeBuffer.count);
						foamDensityShader.SetInt("mode", (int)abstraction.parameters.mode);
						foamDensityShader.SetFloat("pressure", computeFoamRenderSystem.renderers[i].pressure);
						foamDensityShader.SetFloat("particleRadius", num2);
						foamDensityShader.SetFloat("smoothingRadius", num2 * 2f * computeFoamRenderSystem.renderers[i].smoothingRadius);
						foamDensityShader.SetFloat("surfaceTension", computeFoamRenderSystem.renderers[i].surfaceTension);
						foamDensityShader.SetFloat("viscosity", computeFoamRenderSystem.renderers[i].viscosity);
						foamDensityShader.SetVector("volumeLightDirection", (computeFoamRenderSystem.renderers[i].volumeLight != null) ? abstraction.transform.InverseTransformDirection(computeFoamRenderSystem.renderers[i].volumeLight.transform.forward) : Vector3.down);
						foamDensityShader.SetBuffer(clearGridKernel, "cellStart", particleGrid.cellOffsets);
						foamDensityShader.SetBuffer(clearGridKernel, "cellCounts", particleGrid.cellCounts);
						foamDensityShader.Dispatch(clearGridKernel, threadGroupsX, 1, 1);
						foamDensityShader.SetBuffer(insertGridKernel, "inputPositions", abstraction.foamPositions.computeBuffer);
						foamDensityShader.SetBuffer(insertGridKernel, "offsetInCell", auxOffsetInCell);
						foamDensityShader.SetBuffer(insertGridKernel, "cellCounts", particleGrid.cellCounts);
						foamDensityShader.SetBuffer(insertGridKernel, "dispatch", abstraction.foamCount.computeBuffer);
						foamDensityShader.DispatchIndirect(insertGridKernel, abstraction.foamCount.computeBuffer);
						particleGrid.cellsPrefixSum.Sum(particleGrid.cellCounts, particleGrid.cellOffsets);
						foamDensityShader.SetBuffer(sortByGridKernel, "inputPositions", abstraction.foamPositions.computeBuffer);
						foamDensityShader.SetBuffer(sortByGridKernel, "inputVelocities", abstraction.foamVelocities.computeBuffer);
						foamDensityShader.SetBuffer(sortByGridKernel, "sortedPositions", auxPositions);
						foamDensityShader.SetBuffer(sortByGridKernel, "sortedVelocities", auxVelocities);
						foamDensityShader.SetBuffer(sortByGridKernel, "sortedToOriginal", auxSortedToOriginal);
						foamDensityShader.SetBuffer(sortByGridKernel, "offsetInCell", auxOffsetInCell);
						foamDensityShader.SetBuffer(sortByGridKernel, "cellStart", particleGrid.cellOffsets);
						foamDensityShader.SetBuffer(sortByGridKernel, "cellCounts", particleGrid.cellCounts);
						foamDensityShader.SetBuffer(sortByGridKernel, "dispatch", abstraction.foamCount.computeBuffer);
						foamDensityShader.DispatchIndirect(sortByGridKernel, abstraction.foamCount.computeBuffer);
						foamDensityShader.SetBuffer(computeDensityKernel, "inputPositions", abstraction.foamPositions.computeBuffer);
						foamDensityShader.SetBuffer(computeDensityKernel, "sortedPositions", auxPositions);
						foamDensityShader.SetBuffer(computeDensityKernel, "fluidData", auxColors);
						foamDensityShader.SetBuffer(computeDensityKernel, "cellStart", particleGrid.cellOffsets);
						foamDensityShader.SetBuffer(computeDensityKernel, "cellCounts", particleGrid.cellCounts);
						foamDensityShader.SetBuffer(computeDensityKernel, "dispatch", abstraction.foamCount.computeBuffer);
						foamDensityShader.DispatchIndirect(computeDensityKernel, abstraction.foamCount.computeBuffer);
						foamDensityShader.SetBuffer(applyDensityKernel, "inputPositions", abstraction.foamPositions.computeBuffer);
						foamDensityShader.SetBuffer(applyDensityKernel, "inputVelocities", abstraction.foamVelocities.computeBuffer);
						foamDensityShader.SetBuffer(applyDensityKernel, "sortedPositions", auxPositions);
						foamDensityShader.SetBuffer(applyDensityKernel, "sortedVelocities", auxVelocities);
						foamDensityShader.SetBuffer(applyDensityKernel, "sortedToOriginal", auxSortedToOriginal);
						foamDensityShader.SetBuffer(applyDensityKernel, "fluidData", auxColors);
						foamDensityShader.SetBuffer(applyDensityKernel, "cellStart", particleGrid.cellOffsets);
						foamDensityShader.SetBuffer(applyDensityKernel, "cellCounts", particleGrid.cellCounts);
						foamDensityShader.SetBuffer(applyDensityKernel, "dispatch", abstraction.foamCount.computeBuffer);
						foamDensityShader.DispatchIndirect(applyDensityKernel, abstraction.foamCount.computeBuffer);
					}
				}
			}
			else
			{
				activeFoamParticleCount = 0u;
			}
		}

		private void UpdateDiffuseCollisions(float deltaTime)
		{
			if (abstraction.foamCollisions)
			{
				foamCollisionShader.SetFloat("radiusScale", 0.3f);
				foamCollisionShader.SetFloat("colliderCount", colliderGrid.colliderCount);
				foamCollisionShader.SetInt("maxCells", 262144);
				foamCollisionShader.SetInt("cellsPerCollider", 8);
				foamCollisionShader.SetInt("shapeTypeCount", 7);
				foamCollisionShader.SetFloat("deltaTime", deltaTime);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "inputPositions", abstraction.foamPositions.computeBuffer);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "inputVelocities", abstraction.foamVelocities.computeBuffer);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "inputAttributes", abstraction.foamAttributes.computeBuffer);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "aabbs", colliderGrid.aabbsBuffer);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "transforms", colliderGrid.transformsBuffer);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "shapes", colliderGrid.shapesBuffer);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "rigidbodies", colliderGrid.rigidbodiesBuffer);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "sortedColliderIndices", colliderGrid.sortedColliderIndicesBuffer);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "levelPopulation", colliderGrid.levelPopulation);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "cellOffsets", colliderGrid.cellOffsetsBuffer);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "cellCounts", colliderGrid.cellCountsBuffer);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "triangleMeshHeaders", colliderGrid.triangleMeshHeaders);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "bihNodes", colliderGrid.bihNodes);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "triangles", colliderGrid.triangles);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "vertices", colliderGrid.vertices);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "edgeMeshHeaders", colliderGrid.edgeMeshHeaders);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "edgeBihNodes", colliderGrid.edgeBihNodes);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "edges", colliderGrid.edges);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "edgeVertices", colliderGrid.edgeVertices);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "distanceFieldHeaders", colliderGrid.distanceFieldHeaders);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "dfNodes", colliderGrid.dfNodes);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "heightFieldHeaders", colliderGrid.heightFieldHeaders);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "heightFieldSamples", colliderGrid.heightFieldSamples);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "worldToSolver", worldToSolverBuffer);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "solverToWorld", solverToWorldBuffer);
				foamCollisionShader.SetBuffer(solveDiffuseContactsKernel, "dispatch", abstraction.foamCount.computeBuffer);
				foamCollisionShader.DispatchIndirect(solveDiffuseContactsKernel, abstraction.foamCount.computeBuffer);
			}
		}

		private void UpdateDiffuseParticles(float deltaTime)
		{
			if (abstraction.GetRenderSystem<ObiFoamGenerator>() is ComputeFoamRenderSystem computeFoamRenderSystem && m_Solver.maxFoamParticles != 0 && particleGrid.sortedLinearVel != null)
			{
				foamShader.SetFloat("deltaTime", deltaTime);
				foamShader.SetFloat("randomSeed", (float)(Time.frameCount % 16535) + Random.value);
				foamShader.SetVector("gravity", m_Solver.parameters.gravity * m_Solver.parameters.foamGravityScale);
				foamShader.SetVector("agingOverPopulation", new Vector3(m_Solver.foamAccelAgingRange.x, m_Solver.foamAccelAgingRange.y, m_Solver.foamAccelAging));
				foamShader.SetInt("maxFoamParticles", abstraction.foamPositions.computeBuffer.count);
				foamShader.SetInt("minFluidNeighbors", abstraction.foamMinNeighbors);
				foamShader.SetInt("maxCells", particleGrid.maxCells);
				foamShader.SetInt("pointCount", simplexCounts.pointCount);
				foamShader.SetInt("edgeCount", simplexCounts.edgeCount);
				foamShader.SetInt("triangleCount", simplexCounts.triangleCount);
				foamShader.SetBuffer(sortDataKernel, "positions", positionsBuffer);
				foamShader.SetBuffer(sortDataKernel, "velocities", velocitiesBuffer);
				foamShader.SetBuffer(sortDataKernel, "angularVelocities", angularVelocitiesBuffer);
				foamShader.SetBuffer(sortDataKernel, "orientations", renderableOrientationsBuffer);
				foamShader.SetBuffer(sortDataKernel, "principalRadii", renderableRadiiBuffer);
				foamShader.SetBuffer(sortDataKernel, "sortedPositions", particleGrid.sortedPositions);
				foamShader.SetBuffer(sortDataKernel, "sortedVelocities", particleGrid.sortedLinearVel);
				foamShader.SetBuffer(sortDataKernel, "sortedAngularVelocities", particleGrid.sortedAngularVel);
				foamShader.SetBuffer(sortDataKernel, "sortedOrientations", particleGrid.sortedPrevPosOrientations);
				foamShader.SetBuffer(sortDataKernel, "sortedRadii", particleGrid.sortedPrincipalRadii);
				foamShader.SetBuffer(sortDataKernel, "sortedToOriginal", particleGrid.sortedFluidIndices);
				foamShader.SetBuffer(sortDataKernel, "dispatch", fluidDispatchBuffer);
				foamShader.DispatchIndirect(sortDataKernel, fluidDispatchBuffer);
				foamShader.SetBuffer(emitFoamKernel, "positions", positionsBuffer);
				foamShader.SetBuffer(emitFoamKernel, "velocities", velocitiesBuffer);
				foamShader.SetBuffer(emitFoamKernel, "angularVelocities", angularVelocitiesBuffer);
				foamShader.SetBuffer(emitFoamKernel, "fluidData", fluidDataBuffer);
				foamShader.SetBuffer(emitFoamKernel, "principalRadii", principalRadiiBuffer);
				foamShader.SetBuffer(emitFoamKernel, "outputPositions", abstraction.foamPositions.computeBuffer);
				foamShader.SetBuffer(emitFoamKernel, "outputVelocities", abstraction.foamVelocities.computeBuffer);
				foamShader.SetBuffer(emitFoamKernel, "outputColors", abstraction.foamColors.computeBuffer);
				foamShader.SetBuffer(emitFoamKernel, "outputAttributes", abstraction.foamAttributes.computeBuffer);
				foamShader.SetBuffer(emitFoamKernel, "dispatch", abstraction.foamCount.computeBuffer);
				foamShader.SetBuffer(emitShapeFoamKernel, "outputPositions", abstraction.foamPositions.computeBuffer);
				foamShader.SetBuffer(emitShapeFoamKernel, "outputVelocities", abstraction.foamVelocities.computeBuffer);
				foamShader.SetBuffer(emitShapeFoamKernel, "outputColors", abstraction.foamColors.computeBuffer);
				foamShader.SetBuffer(emitShapeFoamKernel, "outputAttributes", abstraction.foamAttributes.computeBuffer);
				foamShader.SetBuffer(emitShapeFoamKernel, "dispatch", abstraction.foamCount.computeBuffer);
				for (int i = 0; i < computeFoamRenderSystem.renderers.Count; i++)
				{
					if (computeFoamRenderSystem.renderers[i].actor.solverIndices?.computeBuffer != null)
					{
						int threadGroupsX = ComputeMath.ThreadGroupCount(computeFoamRenderSystem.renderers[i].actor.activeParticleCount, 128);
						foamShader.SetInt("activeParticleCount", computeFoamRenderSystem.renderers[i].actor.activeParticleCount);
						foamShader.SetVector("vorticityRange", computeFoamRenderSystem.renderers[i].vorticityRange);
						foamShader.SetVector("velocityRange", computeFoamRenderSystem.renderers[i].velocityRange);
						foamShader.SetFloat("foamGenerationRate", computeFoamRenderSystem.renderers[i].foamGenerationRate);
						foamShader.SetFloat("potentialIncrease", computeFoamRenderSystem.renderers[i].foamPotential);
						foamShader.SetFloat("potentialDiffusion", Mathf.Pow(1f - Mathf.Clamp01(computeFoamRenderSystem.renderers[i].foamPotentialDiffusion), deltaTime));
						foamShader.SetFloat("buoyancy", computeFoamRenderSystem.renderers[i].buoyancy);
						foamShader.SetFloat("drag", computeFoamRenderSystem.renderers[i].drag);
						foamShader.SetFloat("airDrag", Mathf.Pow(1f - Mathf.Clamp01(computeFoamRenderSystem.renderers[i].atmosphericDrag), deltaTime));
						foamShader.SetFloat("airAging", computeFoamRenderSystem.renderers[i].airAging);
						foamShader.SetFloat("isosurface", computeFoamRenderSystem.renderers[i].isosurface);
						foamShader.SetFloat("particleSize", computeFoamRenderSystem.renderers[i].size);
						foamShader.SetFloat("sizeRandom", computeFoamRenderSystem.renderers[i].sizeRandom);
						foamShader.SetFloat("lifetime", computeFoamRenderSystem.renderers[i].lifetime);
						foamShader.SetFloat("lifetimeRandom", computeFoamRenderSystem.renderers[i].lifetimeRandom);
						foamShader.SetVector("foamColor", computeFoamRenderSystem.renderers[i].color);
						if (computeFoamRenderSystem.renderers[i] is ObiFoamEmitter)
						{
							ObiFoamEmitter obiFoamEmitter = computeFoamRenderSystem.renderers[i] as ObiFoamEmitter;
							int particleNumberToEmit = obiFoamEmitter.GetParticleNumberToEmit(deltaTime);
							threadGroupsX = ComputeMath.ThreadGroupCount(particleNumberToEmit, 128);
							foamShader.SetInt("particlesToEmit", particleNumberToEmit);
							foamShader.SetInt("emitterShape", (int)obiFoamEmitter.shape);
							foamShader.SetVector("emitterPosition", (obiFoamEmitter.shapeTransform != null) ? abstraction.transform.InverseTransformPoint(obiFoamEmitter.shapeTransform.position) : Vector3.zero);
							foamShader.SetVector("emitterRotation", ((obiFoamEmitter.shapeTransform != null) ? (obiFoamEmitter.shapeTransform.rotation * Quaternion.Inverse(abstraction.transform.rotation)) : Quaternion.identity).AsVector4());
							foamShader.SetVector("emitterSize", obiFoamEmitter.shapeSize);
							foamShader.Dispatch(emitShapeFoamKernel, threadGroupsX, 1, 1);
						}
						else
						{
							foamShader.SetBuffer(emitFoamKernel, "activeParticles", computeFoamRenderSystem.renderers[i].actor.solverIndices.computeBuffer);
							foamShader.Dispatch(emitFoamKernel, threadGroupsX, 1, 1);
						}
					}
				}
				foamShader.SetBuffer(copyAliveKernel, "dispatch", abstraction.foamCount.computeBuffer);
				foamShader.Dispatch(copyAliveKernel, 1, 1, 1);
				foamShader.SetBuffer(updateFoamKernel, "cellOffsets", particleGrid.cellOffsets);
				foamShader.SetBuffer(updateFoamKernel, "cellCounts", particleGrid.cellCounts);
				foamShader.SetBuffer(updateFoamKernel, "gridHashToSortedIndex", particleGrid.cellHashToMortonIndex);
				foamShader.SetBuffer(updateFoamKernel, "levelPopulation", particleGrid.levelPopulation);
				foamShader.SetBuffer(updateFoamKernel, "solverBounds", reducedBounds);
				foamShader.SetBuffer(updateFoamKernel, "positions", particleGrid.sortedPositions);
				foamShader.SetBuffer(updateFoamKernel, "orientations", particleGrid.sortedPrevPosOrientations);
				foamShader.SetBuffer(updateFoamKernel, "principalRadii", particleGrid.sortedPrincipalRadii);
				foamShader.SetBuffer(updateFoamKernel, "velocities", particleGrid.sortedLinearVel);
				foamShader.SetBuffer(updateFoamKernel, "angularVelocities", particleGrid.sortedAngularVel);
				foamShader.SetBuffer(updateFoamKernel, "fluidMaterial", particleGrid.sortedFluidMaterials);
				foamShader.SetBuffer(updateFoamKernel, "fluidData", particleGrid.sortedFluidData);
				foamShader.SetBuffer(updateFoamKernel, "fluidSimplices", particleGrid.sortedSimplexToFluid);
				foamShader.SetBuffer(updateFoamKernel, "sortedToOriginal", particleGrid.sortedFluidIndices);
				foamShader.SetBuffer(updateFoamKernel, "inputPositions", abstraction.foamPositions.computeBuffer);
				foamShader.SetBuffer(updateFoamKernel, "inputVelocities", abstraction.foamVelocities.computeBuffer);
				foamShader.SetBuffer(updateFoamKernel, "inputColors", abstraction.foamColors.computeBuffer);
				foamShader.SetBuffer(updateFoamKernel, "inputAttributes", abstraction.foamAttributes.computeBuffer);
				foamShader.SetBuffer(updateFoamKernel, "outputPositions", auxPositions);
				foamShader.SetBuffer(updateFoamKernel, "outputVelocities", auxVelocities);
				foamShader.SetBuffer(updateFoamKernel, "outputColors", auxColors);
				foamShader.SetBuffer(updateFoamKernel, "outputAttributes", auxAttributes);
				foamShader.SetBuffer(updateFoamKernel, "dispatch", abstraction.foamCount.computeBuffer);
				foamShader.DispatchIndirect(updateFoamKernel, abstraction.foamCount.computeBuffer);
				foamShader.SetBuffer(copyKernel, "inputPositions", auxPositions);
				foamShader.SetBuffer(copyKernel, "inputVelocities", auxVelocities);
				foamShader.SetBuffer(copyKernel, "inputColors", auxColors);
				foamShader.SetBuffer(copyKernel, "inputAttributes", auxAttributes);
				foamShader.SetBuffer(copyKernel, "outputPositions", abstraction.foamPositions.computeBuffer);
				foamShader.SetBuffer(copyKernel, "outputVelocities", abstraction.foamVelocities.computeBuffer);
				foamShader.SetBuffer(copyKernel, "outputColors", abstraction.foamColors.computeBuffer);
				foamShader.SetBuffer(copyKernel, "outputAttributes", abstraction.foamAttributes.computeBuffer);
				foamShader.SetBuffer(copyKernel, "dispatch", abstraction.foamCount.computeBuffer);
				foamShader.DispatchIndirect(copyKernel, abstraction.foamCount.computeBuffer, 16u);
				AsyncGPUReadback.Request(abstraction.foamCount.computeBuffer, 4, 12, delegate(AsyncGPUReadbackRequest obj)
				{
					if (obj.done && !obj.hasError)
					{
						activeFoamParticleCount = obj.GetData<uint>()[0];
					}
				});
			}
			else
			{
				activeFoamParticleCount = 0u;
			}
		}

		private void IntegrateDiffuseParticles(float deltaTime)
		{
			foamShader.SetFloat("deltaTime", deltaTime);
			foamShader.SetBuffer(integrateFoamKernel, "outputPositions", abstraction.foamPositions.computeBuffer);
			foamShader.SetBuffer(integrateFoamKernel, "outputVelocities", abstraction.foamVelocities.computeBuffer);
			foamShader.SetBuffer(integrateFoamKernel, "dispatch", abstraction.foamCount.computeBuffer);
			foamShader.DispatchIndirect(integrateFoamKernel, abstraction.foamCount.computeBuffer);
		}

		public void SpatialQuery(ObiNativeQueryShapeList shapes, ObiNativeAffineTransformList transforms, ObiNativeQueryResultList results)
		{
			if (abstraction.queryResults.count != abstraction.maxQueryResults)
			{
				abstraction.queryResults.ResizeUninitialized((int)abstraction.maxQueryResults);
				abstraction.queryResults.SafeAsComputeBuffer<QueryResult>(GraphicsBuffer.Target.Counter);
			}
			spatialQueries.SpatialQuery(this, shapes.SafeAsComputeBuffer<QueryShape>(), transforms.SafeAsComputeBuffer<AffineTransform>(), results.computeBuffer);
		}

		public int GetParticleGridSize()
		{
			return 0;
		}

		public void GetParticleGrid(ObiNativeAabbList cells)
		{
		}
	}
}
