using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Obi
{
	public class BurstSolverImpl : ISolverImpl
	{
		private const int maxBatches = 17;

		private ConstraintBatcher<ContactProvider> collisionConstraintBatcher;

		private ConstraintBatcher<FluidInteractionProvider> fluidConstraintBatcher;

		private IBurstConstraintsImpl[] constraints;

		private int[] padding = new int[18];

		private BurstJobHandle jobHandle;

		public ParticleGrid particleGrid;

		public NativeArray<BatchData> particleBatchData;

		public NativeArray<FluidInteraction> fluidInteractions;

		public NativeArray<BatchData> fluidBatchData;

		private BurstColliderWorld colliderGrid;

		private NativeArray<int> deformableTriangles;

		private NativeArray<float2> deformableUVs;

		private NativeArray<int> deformableEdges;

		public NativeArray<int> simplices;

		public SimplexCounts simplexCounts;

		private BurstInertialFrame m_InertialFrame;

		private int scheduledJobCounter;

		public NativeArray<int> activeParticles;

		public NativeArray<int> deadParticles;

		public NativeArray<float4> positions;

		public NativeArray<float4> restPositions;

		public NativeArray<float4> prevPositions;

		public NativeArray<float4> renderablePositions;

		public NativeArray<quaternion> orientations;

		public NativeArray<quaternion> restOrientations;

		public NativeArray<quaternion> prevOrientations;

		public NativeArray<quaternion> renderableOrientations;

		public NativeArray<float4> velocities;

		public NativeArray<float4> angularVelocities;

		public NativeArray<float> invMasses;

		public NativeArray<float> invRotationalMasses;

		public NativeArray<float4> externalForces;

		public NativeArray<float4> externalTorques;

		public NativeArray<float4> wind;

		public NativeArray<float4> positionDeltas;

		public NativeArray<quaternion> orientationDeltas;

		public NativeArray<int> positionConstraintCounts;

		public NativeArray<int> orientationConstraintCounts;

		public NativeArray<float4> colors;

		public NativeArray<int> collisionMaterials;

		public NativeArray<int> phases;

		public NativeArray<int> filters;

		public NativeArray<float4> renderableRadii;

		public NativeArray<float4> principalRadii;

		public NativeArray<float4> normals;

		private NativeArray<float4> tangents;

		public NativeArray<float> life;

		public NativeArray<float4> fluidData;

		public NativeArray<float4> userData;

		public NativeArray<float4> fluidInterface;

		public NativeArray<float4> fluidMaterials;

		public NativeArray<float4> fluidMaterials2;

		public NativeArray<float4x4> anisotropies;

		public NativeArray<float4> auxPositions;

		public NativeArray<float4> auxVelocities;

		public NativeArray<float4> auxColors;

		public NativeArray<float4> auxAttributes;

		public NativeArray<int4> cellCoords;

		public NativeArray<BurstAabb> simplexBounds;

		public NativeArray<BurstAabb> reducedBounds;

		public BurstAabb solverBounds;

		private ConstraintSorter<BurstContact> contactSorter;

		public ObiSolver abstraction { get; }

		public int particleCount => abstraction.positions.count;

		public int activeParticleCount => abstraction.activeParticles.count;

		public BurstInertialFrame inertialFrame => m_InertialFrame;

		public BurstAffineTransform solverToWorld => m_InertialFrame.frame;

		public BurstAffineTransform worldToSolver => m_InertialFrame.frame.Inverse();

		public uint activeFoamParticleCount { get; private set; }

		public BurstSolverImpl(ObiSolver solver)
		{
			abstraction = solver;
			jobHandle = new BurstJobHandle();
			contactSorter = new ConstraintSorter<BurstContact>();
			GetOrCreateColliderWorld();
			if (colliderGrid != null)
			{
				colliderGrid.IncreaseReferenceCount();
			}
			particleGrid = new ParticleGrid();
			collisionConstraintBatcher = new ConstraintBatcher<ContactProvider>(17);
			fluidConstraintBatcher = new ConstraintBatcher<FluidInteractionProvider>(17);
			constraints = new IBurstConstraintsImpl[18];
			constraints[0] = new BurstTetherConstraints(this);
			constraints[1] = new BurstVolumeConstraints(this);
			constraints[2] = new BurstChainConstraints(this);
			constraints[4] = new BurstBendConstraints(this);
			constraints[5] = new BurstDistanceConstraints(this);
			constraints[6] = new BurstShapeMatchingConstraints(this);
			constraints[7] = new BurstBendTwistConstraints(this);
			constraints[8] = new BurstStretchShearConstraints(this);
			constraints[9] = new BurstPinConstraints(this);
			constraints[10] = new BurstParticleCollisionConstraints(this);
			constraints[11] = new BurstDensityConstraints(this);
			constraints[12] = new BurstColliderCollisionConstraints(this);
			constraints[13] = new BurstSkinConstraints(this);
			constraints[14] = new BurstAerodynamicConstraints(this);
			constraints[15] = new BurstStitchConstraints(this);
			constraints[16] = new BurstParticleFrictionConstraints(this);
			constraints[17] = new BurstColliderFrictionConstraints(this);
			constraints[3] = new BurstPinholeConstraints(this);
			(constraints[12] as BurstColliderCollisionConstraints).CreateConstraintsBatch();
			(constraints[17] as BurstColliderFrictionConstraints).CreateConstraintsBatch();
		}

		public void Destroy()
		{
			for (int i = 0; i < constraints.Length; i++)
			{
				if (constraints[i] != null)
				{
					constraints[i].Dispose();
				}
			}
			particleGrid.Dispose();
			if (colliderGrid != null)
			{
				colliderGrid.DecreaseReferenceCount();
			}
			collisionConstraintBatcher.Dispose();
			fluidConstraintBatcher.Dispose();
			if (simplexBounds.IsCreated)
			{
				simplexBounds.Dispose();
			}
			if (reducedBounds.IsCreated)
			{
				reducedBounds.Dispose();
			}
			if (tangents.IsCreated)
			{
				tangents.Dispose();
			}
			if (particleBatchData.IsCreated)
			{
				particleBatchData.Dispose();
			}
			if (fluidInteractions.IsCreated)
			{
				fluidInteractions.Dispose();
			}
			if (fluidBatchData.IsCreated)
			{
				fluidBatchData.Dispose();
			}
			if (auxPositions.IsCreated)
			{
				auxPositions.Dispose();
			}
			if (auxVelocities.IsCreated)
			{
				auxVelocities.Dispose();
			}
			if (auxColors.IsCreated)
			{
				auxColors.Dispose();
			}
			if (auxAttributes.IsCreated)
			{
				auxAttributes.Dispose();
			}
		}

		public void ScheduleBatchedJobsIfNeeded()
		{
			if (scheduledJobCounter++ > 16)
			{
				scheduledJobCounter = 0;
				JobHandle.ScheduleBatchedJobs();
			}
		}

		private void GetOrCreateColliderWorld()
		{
			colliderGrid = Object.FindObjectOfType<BurstColliderWorld>();
			if (colliderGrid == null)
			{
				GameObject gameObject = new GameObject("BurstCollisionWorld", typeof(BurstColliderWorld));
				colliderGrid = gameObject.GetComponent<BurstColliderWorld>();
			}
		}

		public void InitializeFrame(Vector4 translation, Vector4 scale, Quaternion rotation)
		{
			m_InertialFrame = new BurstInertialFrame(translation, scale, rotation);
		}

		public void UpdateFrame(Vector4 translation, Vector4 scale, Quaternion rotation, float deltaTime)
		{
			m_InertialFrame.Update(translation, scale, rotation, deltaTime);
		}

		public IObiJobHandle ApplyFrame(float worldLinearInertiaScale, float worldAngularInertiaScale, float deltaTime)
		{
			float4x4 float4x5 = float4x4.TRS(float3.zero, inertialFrame.frame.rotation, math.rcp(inertialFrame.frame.scale.xyz));
			float4x4 a = math.transpose(float4x5);
			float4 angularVel = math.mul(a, math.mul(float4x4.Scale(inertialFrame.angularVelocity.xyz), float4x5)).diagonal();
			float4 eulerAccel = math.mul(a, math.mul(float4x4.Scale(inertialFrame.angularAcceleration.xyz), float4x5)).diagonal();
			float4 inertialAccel = math.mul(a, inertialFrame.acceleration);
			ApplyInertialForcesJob jobData = new ApplyInertialForcesJob
			{
				activeParticles = activeParticles,
				positions = positions,
				velocities = velocities,
				invMasses = invMasses,
				angularVel = angularVel,
				inertialAccel = inertialAccel,
				eulerAccel = eulerAccel,
				worldLinearInertiaScale = worldLinearInertiaScale,
				worldAngularInertiaScale = worldAngularInertiaScale,
				wind = wind,
				ambientWind = new float4(abstraction.parameters.ambientWind, 0f),
				inertialFrame = inertialFrame,
				deltaTime = deltaTime,
				inertialWind = (abstraction.windSpace == Space.World)
			};
			jobHandle.jobHandle = IJobParallelForExtensions.Schedule(jobData, activeParticleCount, 64);
			return jobHandle;
		}

		public void SetDeformableTriangles(ObiNativeIntList indices, ObiNativeVector2List uvs)
		{
			deformableTriangles = indices.AsNativeArray<int>();
			deformableUVs = uvs.AsNativeArray<float2>();
		}

		public void SetDeformableEdges(ObiNativeIntList indices)
		{
			deformableEdges = indices.AsNativeArray<int>();
		}

		public void SetSimplices(ObiNativeIntList simplices, SimplexCounts counts)
		{
			this.simplices = simplices.AsNativeArray<int>();
			simplexCounts = counts;
			cellCoords = abstraction.cellCoords.AsNativeArray<int4>();
			if (simplexBounds.IsCreated)
			{
				simplexBounds.Dispose();
			}
			simplexBounds = new NativeArray<BurstAabb>(counts.simplexCount, Allocator.Persistent);
			if (reducedBounds.IsCreated)
			{
				reducedBounds.Dispose();
			}
			reducedBounds = new NativeArray<BurstAabb>(counts.simplexCount, Allocator.Persistent);
		}

		public void SetActiveParticles(ObiNativeIntList activeIndices)
		{
			activeParticles = activeIndices.AsNativeArray<int>();
		}

		public IObiJobHandle UpdateBounds(IObiJobHandle inputDeps, float stepTime)
		{
			if (!(inputDeps is BurstJobHandle burstJobHandle))
			{
				return inputDeps;
			}
			CalculateSimplexBoundsJob jobData = new CalculateSimplexBoundsJob
			{
				radii = principalRadii,
				fluidMaterials = fluidMaterials,
				positions = positions,
				velocities = velocities,
				simplices = simplices,
				simplexCounts = simplexCounts,
				particleMaterialIndices = collisionMaterials,
				collisionMaterials = ObiColliderWorld.GetInstance().collisionMaterials.AsNativeArray<BurstCollisionMaterial>(),
				parameters = abstraction.parameters,
				simplexBounds = simplexBounds,
				reducedBounds = reducedBounds,
				dt = stepTime
			};
			burstJobHandle.jobHandle = IJobParallelForExtensions.Schedule(jobData, simplexCounts.simplexCount, 64, burstJobHandle.jobHandle);
			int num = 4;
			int num2 = simplexCounts.simplexCount;
			int num3 = 1;
			while (num2 > 1)
			{
				BoundsReductionJob jobData2 = new BoundsReductionJob
				{
					bounds = reducedBounds,
					stride = num3,
					size = num
				};
				burstJobHandle.jobHandle = IJobParallelForExtensions.Schedule(jobData2, num2, 1, burstJobHandle.jobHandle);
				num2 = (int)math.ceil((float)num2 / (float)num);
				num3 *= num;
			}
			NativeReference<int> countReference = abstraction.deadParticles.GetCountReference(Allocator.TempJob);
			UpdateParticleLifetimesJob jobData3 = new UpdateParticleLifetimesJob
			{
				activeParticles = activeParticles,
				life = life,
				deadParticles = deadParticles,
				deadParticleCount = countReference,
				dt = stepTime
			};
			burstJobHandle.jobHandle = IJobParallelForExtensions.Schedule(jobData3, activeParticleCount, 64, burstJobHandle.jobHandle);
			burstJobHandle.jobHandle.Complete();
			abstraction.deadParticles.count = countReference.Value;
			countReference.Dispose();
			return burstJobHandle;
		}

		public void GetBounds(ref Vector3 min, ref Vector3 max)
		{
			if (reducedBounds.IsCreated && reducedBounds.Length > 0)
			{
				solverBounds.min = reducedBounds[0].min;
				solverBounds.max = reducedBounds[0].max;
			}
			min = solverBounds.min.xyz;
			max = solverBounds.max.xyz;
		}

		public int GetConstraintCount(Oni.ConstraintType type)
		{
			if (type > Oni.ConstraintType.Tether && (int)type < constraints.Length)
			{
				return constraints[(int)type].GetConstraintCount();
			}
			return 0;
		}

		public void SetParameters(Oni.SolverParameters parameters)
		{
		}

		public void SetConstraintGroupParameters(Oni.ConstraintType type, ref Oni.ConstraintParameters parameters)
		{
		}

		public void ParticleCountChanged(ObiSolver solver)
		{
			deadParticles = abstraction.deadParticles.AsNativeArray<int>(abstraction.deadParticles.capacity);
			positions = abstraction.positions.AsNativeArray<float4>();
			restPositions = abstraction.restPositions.AsNativeArray<float4>();
			prevPositions = abstraction.prevPositions.AsNativeArray<float4>();
			renderablePositions = abstraction.renderablePositions.AsNativeArray<float4>();
			orientations = abstraction.orientations.AsNativeArray<quaternion>();
			restOrientations = abstraction.restOrientations.AsNativeArray<quaternion>();
			prevOrientations = abstraction.prevOrientations.AsNativeArray<quaternion>();
			renderableOrientations = abstraction.renderableOrientations.AsNativeArray<quaternion>();
			colors = abstraction.colors.AsNativeArray<float4>();
			velocities = abstraction.velocities.AsNativeArray<float4>();
			angularVelocities = abstraction.angularVelocities.AsNativeArray<float4>();
			invMasses = abstraction.invMasses.AsNativeArray<float>();
			invRotationalMasses = abstraction.invRotationalMasses.AsNativeArray<float>();
			externalForces = abstraction.externalForces.AsNativeArray<float4>();
			externalTorques = abstraction.externalTorques.AsNativeArray<float4>();
			wind = abstraction.wind.AsNativeArray<float4>();
			positionDeltas = abstraction.positionDeltas.AsNativeArray<float4>();
			orientationDeltas = abstraction.orientationDeltas.AsNativeArray<quaternion>();
			positionConstraintCounts = abstraction.positionConstraintCounts.AsNativeArray<int>();
			orientationConstraintCounts = abstraction.orientationConstraintCounts.AsNativeArray<int>();
			collisionMaterials = abstraction.collisionMaterials.AsNativeArray<int>();
			phases = abstraction.phases.AsNativeArray<int>();
			filters = abstraction.filters.AsNativeArray<int>();
			renderableRadii = abstraction.renderableRadii.AsNativeArray<float4>();
			principalRadii = abstraction.principalRadii.AsNativeArray<float4>();
			normals = abstraction.normals.AsNativeArray<float4>();
			life = abstraction.life.AsNativeArray<float>();
			fluidData = abstraction.fluidData.AsNativeArray<float4>();
			userData = abstraction.userData.AsNativeArray<float4>();
			fluidInterface = abstraction.fluidInterface.AsNativeArray<float4>();
			fluidMaterials = abstraction.fluidMaterials.AsNativeArray<float4>();
			fluidMaterials2 = abstraction.fluidMaterials2.AsNativeArray<float4>();
			anisotropies = abstraction.anisotropies.AsNativeArray<float4x4>();
			cellCoords = abstraction.cellCoords.AsNativeArray<int4>();
			if (tangents.IsCreated)
			{
				tangents.Dispose();
			}
			tangents = new NativeArray<float4>(normals.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
		}

		public void MaxFoamParticleCountChanged(ObiSolver solver)
		{
			if (auxPositions.IsCreated)
			{
				auxPositions.Dispose();
			}
			if (auxVelocities.IsCreated)
			{
				auxVelocities.Dispose();
			}
			if (auxColors.IsCreated)
			{
				auxColors.Dispose();
			}
			if (auxAttributes.IsCreated)
			{
				auxAttributes.Dispose();
			}
			auxPositions = new NativeArray<float4>((int)abstraction.maxFoamParticles, Allocator.Persistent);
			auxVelocities = new NativeArray<float4>((int)abstraction.maxFoamParticles, Allocator.Persistent);
			auxColors = new NativeArray<float4>((int)abstraction.maxFoamParticles, Allocator.Persistent);
			auxAttributes = new NativeArray<float4>((int)abstraction.maxFoamParticles, Allocator.Persistent);
		}

		public void SetRigidbodyArrays(ObiSolver solver)
		{
		}

		public IConstraintsBatchImpl CreateConstraintsBatch(Oni.ConstraintType type)
		{
			return constraints[(int)type].CreateConstraintsBatch();
		}

		public void DestroyConstraintsBatch(IConstraintsBatchImpl batch)
		{
			if (batch != null)
			{
				constraints[(int)batch.constraintType].RemoveBatch(batch);
			}
		}

		public void FinishSimulation()
		{
			abstraction.externalForces.WipeToZero();
			abstraction.externalTorques.WipeToZero();
			abstraction.startPositions.CopyFrom(abstraction.endPositions);
			abstraction.startOrientations.CopyFrom(abstraction.endOrientations);
			abstraction.endPositions.CopyFrom(abstraction.positions);
			abstraction.endOrientations.CopyFrom(abstraction.orientations);
		}

		public void PushData()
		{
		}

		public void RequestReadback()
		{
		}

		public IObiJobHandle CollisionDetection(IObiJobHandle inputDeps, float stepTime)
		{
			if (!(inputDeps is BurstJobHandle burstJobHandle))
			{
				return inputDeps;
			}
			burstJobHandle.jobHandle = FindFluidParticles(burstJobHandle.jobHandle);
			burstJobHandle.jobHandle = GenerateContacts(burstJobHandle.jobHandle, stepTime);
			return burstJobHandle;
		}

		protected JobHandle FindFluidParticles(JobHandle inputDeps)
		{
			BurstDensityConstraints burstDensityConstraints = constraints[11] as BurstDensityConstraints;
			return new FindFluidParticlesJob
			{
				activeParticles = activeParticles,
				phases = phases,
				fluidParticles = burstDensityConstraints.fluidParticles
			}.Schedule(inputDeps);
		}

		protected JobHandle GenerateContacts(JobHandle inputDeps, float deltaTime)
		{
			if (fluidInteractions.IsCreated)
			{
				fluidInteractions.Dispose();
			}
			if (fluidBatchData.IsCreated)
			{
				fluidBatchData.Dispose();
			}
			if (particleBatchData.IsCreated)
			{
				particleBatchData.Dispose();
			}
			Oni.ConstraintParameters constraintParameters = abstraction.GetConstraintParameters(Oni.ConstraintType.Collision);
			Oni.ConstraintParameters constraintParameters2 = abstraction.GetConstraintParameters(Oni.ConstraintType.ParticleCollision);
			Oni.ConstraintParameters constraintParameters3 = abstraction.GetConstraintParameters(Oni.ConstraintType.Density);
			if (constraintParameters.enabled || constraintParameters2.enabled || constraintParameters3.enabled)
			{
				JobHandle job = inputDeps;
				JobHandle job2 = inputDeps;
				if (constraintParameters2.enabled || constraintParameters3.enabled)
				{
					particleGrid.Update(this, inputDeps);
					job = particleGrid.GenerateContacts(this, deltaTime);
				}
				if (constraintParameters.enabled)
				{
					job2 = colliderGrid.GenerateContacts(this, deltaTime, inputDeps);
				}
				JobHandle.CombineDependencies(job, job2).Complete();
				particleBatchData = new NativeArray<BatchData>(17, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				fluidInteractions = new NativeArray<FluidInteraction>(particleGrid.fluidInteractionQueue.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				fluidBatchData = new NativeArray<BatchData>(17, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				abstraction.contactEffectiveMasses.ResizeUninitialized(colliderGrid.colliderContactQueue.Count);
				abstraction.particleContactEffectiveMasses.ResizeUninitialized(particleGrid.particleContactQueue.Count);
				NativeArray<BurstContact> outputArray = new NativeArray<BurstContact>(particleGrid.particleContactQueue.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
				NativeArray<BurstContact> sortedConstraints = new NativeArray<BurstContact>(particleGrid.particleContactQueue.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
				NativeArray<FluidInteraction> nativeArray = new NativeArray<FluidInteraction>(particleGrid.fluidInteractionQueue.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
				abstraction.particleContacts.ResizeUninitialized(particleGrid.particleContactQueue.Count);
				DequeueIntoArrayJob<BurstContact> jobData = new DequeueIntoArrayJob<BurstContact>
				{
					InputQueue = particleGrid.particleContactQueue,
					OutputArray = outputArray
				};
				abstraction.colliderContacts.ResizeUninitialized(colliderGrid.colliderContactQueue.Count);
				DequeueIntoArrayJob<BurstContact> jobData2 = new DequeueIntoArrayJob<BurstContact>
				{
					InputQueue = colliderGrid.colliderContactQueue,
					OutputArray = abstraction.colliderContacts.AsNativeArray<BurstContact>()
				};
				JobHandle handle = JobHandle.CombineDependencies(job1: new DequeueIntoArrayJob<FluidInteraction>
				{
					InputQueue = particleGrid.fluidInteractionQueue,
					OutputArray = nativeArray
				}.Schedule(), job0: jobData.Schedule(), job2: jobData2.Schedule());
				handle = contactSorter.SortConstraints(simplexCounts.simplexCount, outputArray, ref sortedConstraints, handle);
				ContactProvider constraintDesc = new ContactProvider
				{
					contacts = sortedConstraints,
					sortedContacts = abstraction.particleContacts.AsNativeArray<BurstContact>(),
					simplices = simplices,
					simplexCounts = simplexCounts
				};
				FluidInteractionProvider constraintDesc2 = new FluidInteractionProvider
				{
					interactions = nativeArray,
					sortedInteractions = fluidInteractions
				};
				NativeArray<int> activeBatchCount = new NativeArray<int>(1, Allocator.TempJob);
				JobHandle job3 = collisionConstraintBatcher.BatchConstraints(ref constraintDesc, particleCount, ref particleBatchData, ref activeBatchCount, handle);
				NativeArray<int> activeBatchCount2 = new NativeArray<int>(1, Allocator.TempJob);
				JobHandle job4 = fluidConstraintBatcher.BatchConstraints(ref constraintDesc2, particleCount, ref fluidBatchData, ref activeBatchCount2, handle);
				JobHandle.CombineDependencies(job3, job4).Complete();
				BurstParticleCollisionConstraints burstParticleCollisionConstraints = constraints[10] as BurstParticleCollisionConstraints;
				BurstParticleFrictionConstraints burstParticleFrictionConstraints = constraints[16] as BurstParticleFrictionConstraints;
				for (int i = 0; i < burstParticleCollisionConstraints.batches.Count; i++)
				{
					burstParticleCollisionConstraints.batches[i].enabled = false;
				}
				for (int j = 0; j < burstParticleFrictionConstraints.batches.Count; j++)
				{
					burstParticleFrictionConstraints.batches[j].enabled = false;
				}
				for (int k = 0; k < activeBatchCount[0]; k++)
				{
					if (k == burstParticleCollisionConstraints.batches.Count)
					{
						burstParticleCollisionConstraints.CreateConstraintsBatch();
						burstParticleFrictionConstraints.CreateConstraintsBatch();
					}
					burstParticleCollisionConstraints.batches[k].enabled = true;
					burstParticleFrictionConstraints.batches[k].enabled = true;
					burstParticleCollisionConstraints.batches[k].batchData = particleBatchData[k];
					burstParticleFrictionConstraints.batches[k].batchData = particleBatchData[k];
				}
				BurstDensityConstraints burstDensityConstraints = constraints[11] as BurstDensityConstraints;
				for (int l = 0; l < burstDensityConstraints.batches.Count; l++)
				{
					burstDensityConstraints.batches[l].enabled = false;
				}
				for (int m = 0; m < activeBatchCount2[0]; m++)
				{
					if (m == burstDensityConstraints.batches.Count)
					{
						burstDensityConstraints.CreateConstraintsBatch();
					}
					burstDensityConstraints.batches[m].enabled = true;
					burstDensityConstraints.batches[m].batchData = fluidBatchData[m];
				}
				outputArray.Dispose();
				nativeArray.Dispose();
				sortedConstraints.Dispose();
				activeBatchCount.Dispose();
				activeBatchCount2.Dispose();
				inputDeps = colliderGrid.ApplyForceZones(this, deltaTime, inputDeps);
			}
			return inputDeps;
		}

		public IObiJobHandle Substep(IObiJobHandle handle, float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (!(handle is BurstJobHandle burstJobHandle))
			{
				return handle;
			}
			burstJobHandle.jobHandle = constraints[14].Project(burstJobHandle.jobHandle, stepTime, substepTime, steps, timeLeft);
			PredictPositionsJob jobData = new PredictPositionsJob
			{
				activeParticles = activeParticles,
				phases = phases,
				buoyancies = fluidInterface,
				externalForces = externalForces,
				inverseMasses = invMasses,
				positions = positions,
				previousPositions = prevPositions,
				velocities = velocities,
				externalTorques = externalTorques,
				inverseRotationalMasses = invRotationalMasses,
				orientations = orientations,
				previousOrientations = prevOrientations,
				angularVelocities = angularVelocities,
				gravity = new float4(abstraction.parameters.gravity, 0f),
				deltaTime = substepTime,
				is2D = (abstraction.parameters.mode == Oni.SolverParameters.Mode.Mode2D)
			};
			burstJobHandle.jobHandle = IJobParallelForExtensions.Schedule(jobData, activeParticles.Length, 128, burstJobHandle.jobHandle);
			burstJobHandle.jobHandle = ApplyConstraints(burstJobHandle.jobHandle, stepTime, substepTime, steps, timeLeft);
			burstJobHandle.jobHandle = EnforceLimits(burstJobHandle.jobHandle);
			UpdateVelocitiesJob jobData2 = new UpdateVelocitiesJob
			{
				activeParticles = activeParticles,
				inverseMasses = invMasses,
				previousPositions = prevPositions,
				positions = positions,
				velocities = velocities,
				inverseRotationalMasses = invRotationalMasses,
				previousOrientations = prevOrientations,
				orientations = orientations,
				angularVelocities = angularVelocities,
				deltaTime = substepTime,
				is2D = (abstraction.parameters.mode == Oni.SolverParameters.Mode.Mode2D)
			};
			burstJobHandle.jobHandle = IJobParallelForExtensions.Schedule(jobData2, activeParticles.Length, 128, burstJobHandle.jobHandle);
			burstJobHandle.jobHandle = CalculateVelocityCorrections(burstJobHandle.jobHandle, substepTime);
			int num = (int)math.round(timeLeft / substepTime);
			int num2 = (int)math.ceil((float)abstraction.substeps / (float)abstraction.foamSubsteps);
			if (num % num2 == 0)
			{
				burstJobHandle.jobHandle = UpdateDiffuseParticles(burstJobHandle.jobHandle, substepTime * (float)num2);
			}
			burstJobHandle.jobHandle = ApplyVelocityCorrections(burstJobHandle.jobHandle, substepTime);
			UpdatePositionsJob jobData3 = new UpdatePositionsJob
			{
				activeParticles = activeParticles,
				positions = positions,
				previousPositions = prevPositions,
				velocities = velocities,
				orientations = orientations,
				previousOrientations = prevOrientations,
				angularVelocities = angularVelocities,
				velocityScale = math.pow(1f - math.saturate(abstraction.parameters.damping), substepTime),
				sleepThreshold = abstraction.parameters.sleepThreshold,
				maxVelocity = abstraction.parameters.maxVelocity,
				maxAngularVelocity = abstraction.parameters.maxAngularVelocity
			};
			burstJobHandle.jobHandle = IJobParallelForExtensions.Schedule(jobData3, activeParticles.Length, 128, burstJobHandle.jobHandle);
			return burstJobHandle;
		}

		private JobHandle CalculateVelocityCorrections(JobHandle inputDeps, float deltaTime)
		{
			if (abstraction.GetConstraintParameters(Oni.ConstraintType.Density).enabled && constraints[11] is BurstDensityConstraints burstDensityConstraints)
			{
				return burstDensityConstraints.CalculateVelocityCorrections(inputDeps, deltaTime);
			}
			return inputDeps;
		}

		private JobHandle ApplyVelocityCorrections(JobHandle inputDeps, float deltaTime)
		{
			if (abstraction.GetConstraintParameters(Oni.ConstraintType.Density).enabled && constraints[11] is BurstDensityConstraints burstDensityConstraints)
			{
				return burstDensityConstraints.ApplyVelocityCorrections(inputDeps, deltaTime);
			}
			return inputDeps;
		}

		private JobHandle ApplyConstraints(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			int num = 0;
			for (int i = 0; i < 18; i++)
			{
				Oni.ConstraintParameters constraintParameters = abstraction.GetConstraintParameters((Oni.ConstraintType)i);
				if (constraintParameters.enabled)
				{
					num = math.max(num, constraintParameters.iterations);
					inputDeps = constraints[i].Initialize(inputDeps, stepTime, substepTime, steps, timeLeft);
				}
			}
			for (int j = 0; j < 18; j++)
			{
				Oni.ConstraintParameters constraintParameters2 = abstraction.GetConstraintParameters((Oni.ConstraintType)j);
				if (constraintParameters2.enabled && constraintParameters2.iterations > 0)
				{
					padding[j] = (int)math.ceil((float)num / (float)constraintParameters2.iterations);
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
					if (l != 14 && abstraction.GetConstraintParameters((Oni.ConstraintType)l).enabled && k % padding[l] == 0)
					{
						inputDeps = constraints[l].Project(inputDeps, stepTime, substepTime, steps, timeLeft);
					}
				}
			}
			for (int m = 0; m < 18; m++)
			{
				if (m != 14)
				{
					Oni.ConstraintParameters constraintParameters3 = abstraction.GetConstraintParameters((Oni.ConstraintType)m);
					if (constraintParameters3.enabled && constraintParameters3.iterations > 0)
					{
						inputDeps = constraints[m].Project(inputDeps, stepTime, substepTime, steps, timeLeft);
					}
				}
			}
			Oni.ConstraintParameters constraintParameters4 = abstraction.GetConstraintParameters(Oni.ConstraintType.ParticleCollision);
			if (constraintParameters4.enabled && constraintParameters4.iterations > 0)
			{
				inputDeps = constraints[10].Project(inputDeps, stepTime, substepTime, steps, timeLeft);
			}
			constraintParameters4 = abstraction.GetConstraintParameters(Oni.ConstraintType.Collision);
			if (constraintParameters4.enabled && constraintParameters4.iterations > 0)
			{
				inputDeps = constraints[12].Project(inputDeps, stepTime, substepTime, steps, timeLeft);
			}
			return inputDeps;
		}

		private JobHandle EnforceLimits(JobHandle inputDeps)
		{
			if (!abstraction.useLimits)
			{
				return inputDeps;
			}
			return IJobParallelForExtensions.Schedule(new EnforceLimitsJob
			{
				activeParticles = activeParticles,
				positions = positions,
				prevPositions = prevPositions,
				life = life,
				phases = phases,
				boundaryLimits = new BurstAabb(new float4(abstraction.boundaryLimits.min, 0f), new float4(abstraction.boundaryLimits.max, 0f)),
				killOffLimits = abstraction.killOffLimitsParticles
			}, activeParticleCount, 64, inputDeps);
		}

		public IObiJobHandle ApplyInterpolation(IObiJobHandle inputDeps, ObiNativeVector4List startPositions, ObiNativeQuaternionList startOrientations, float stepTime, float unsimulatedTime)
		{
			if (inputDeps == null)
			{
				inputDeps = new BurstJobHandle();
			}
			if (!(inputDeps is BurstJobHandle burstJobHandle))
			{
				return inputDeps;
			}
			InterpolationJob jobData = new InterpolationJob
			{
				positions = positions,
				endPositions = abstraction.endPositions.AsNativeArray<float4>(),
				startPositions = startPositions.AsNativeArray<float4>(),
				renderablePositions = renderablePositions,
				orientations = orientations,
				endOrientations = abstraction.endOrientations.AsNativeArray<quaternion>(),
				startOrientations = startOrientations.AsNativeArray<quaternion>(),
				renderableOrientations = renderableOrientations,
				principalRadii = principalRadii,
				renderableRadii = renderableRadii,
				blendFactor = ((stepTime > 0f) ? (unsimulatedTime / stepTime) : 0f),
				interpolationMode = abstraction.parameters.interpolation
			};
			burstJobHandle.jobHandle = IJobParallelForExtensions.Schedule(jobData, abstraction.positions.count, 128, burstJobHandle.jobHandle);
			ResetNormals jobData2 = new ResetNormals
			{
				phases = phases,
				normals = normals,
				tangents = tangents
			};
			burstJobHandle.jobHandle = IJobParallelForExtensions.Schedule(jobData2, normals.Length, 128, burstJobHandle.jobHandle);
			UpdateTriangleNormalsJob jobData3 = new UpdateTriangleNormalsJob
			{
				renderPositions = renderablePositions,
				deformableTriangles = deformableTriangles,
				deformableTriangleUVs = deformableUVs,
				normals = normals,
				tangents = tangents
			};
			burstJobHandle.jobHandle = IJobParallelForExtensions.Schedule(jobData3, deformableTriangles.Length / 3, 1, burstJobHandle.jobHandle);
			UpdateEdgeNormalsJob jobData4 = new UpdateEdgeNormalsJob
			{
				renderPositions = renderablePositions,
				velocities = velocities,
				deformableEdges = deformableEdges,
				wind = wind,
				normals = normals
			};
			burstJobHandle.jobHandle = IJobParallelForExtensions.Schedule(jobData4, deformableEdges.Length / 2, 1, burstJobHandle.jobHandle);
			RenderableOrientationFromNormals jobData5 = new RenderableOrientationFromNormals
			{
				phases = phases,
				normals = normals,
				tangents = tangents,
				renderableOrientations = renderableOrientations
			};
			burstJobHandle.jobHandle = IJobParallelForExtensions.Schedule(jobData5, normals.Length, 128, burstJobHandle.jobHandle);
			Oni.ConstraintParameters constraintParameters = abstraction.GetConstraintParameters(Oni.ConstraintType.Pin);
			if (constraintParameters.enabled && constraintParameters.iterations > 0)
			{
				BurstPinConstraints burstPinConstraints = constraints[9] as BurstPinConstraints;
				if (Application.isPlaying && burstPinConstraints != null)
				{
					burstJobHandle.jobHandle = burstPinConstraints.ProjectRenderablePositions(burstJobHandle.jobHandle);
				}
			}
			Oni.ConstraintParameters constraintParameters2 = abstraction.GetConstraintParameters(Oni.ConstraintType.Density);
			if (constraintParameters2.enabled && constraintParameters2.iterations > 0)
			{
				BurstDensityConstraints burstDensityConstraints = constraints[11] as BurstDensityConstraints;
				if (Application.isPlaying && burstDensityConstraints != null)
				{
					burstJobHandle.jobHandle = burstDensityConstraints.CalculateAnisotropyLaplacianSmoothing(burstJobHandle.jobHandle);
				}
			}
			return burstJobHandle;
		}

		private unsafe JobHandle UpdateDiffuseParticles(JobHandle inputDeps, float deltaTime)
		{
			if (abstraction.GetRenderSystem<ObiFoamGenerator>() is BurstFoamRenderSystem burstFoamRenderSystem)
			{
				int* ptr = (int*)abstraction.foamCount.AddressOfElement(0);
				for (int i = 0; i < burstFoamRenderSystem.renderers.Count; i++)
				{
					float randomSeed = (float)(Time.frameCount % 16535) + UnityEngine.Random.value;
					if (burstFoamRenderSystem.renderers[i] is ObiFoamEmitter)
					{
						ObiFoamEmitter obiFoamEmitter = burstFoamRenderSystem.renderers[i] as ObiFoamEmitter;
						int particleNumberToEmit = obiFoamEmitter.GetParticleNumberToEmit(deltaTime);
						inputDeps = IJobParallelForExtensions.Schedule(new EmitParticlesJob
						{
							outputPositions = abstraction.foamPositions.AsNativeArray<float4>(),
							outputVelocities = abstraction.foamVelocities.AsNativeArray<float4>(),
							outputColors = abstraction.foamColors.AsNativeArray<float4>(),
							outputAttributes = abstraction.foamAttributes.AsNativeArray<float4>(),
							dispatchBuffer = abstraction.foamCount.AsNativeArray<int>(),
							emitterShape = (uint)obiFoamEmitter.shape,
							emitterPosition = new float4((obiFoamEmitter.shapeTransform != null) ? abstraction.transform.InverseTransformPoint(obiFoamEmitter.shapeTransform.position) : Vector3.zero, 0f),
							emitterRotation = ((obiFoamEmitter.shapeTransform != null) ? (obiFoamEmitter.shapeTransform.rotation * Quaternion.Inverse(abstraction.transform.rotation)) : Quaternion.identity),
							emitterSize = new float4(obiFoamEmitter.shapeSize, 0f),
							randomSeed = randomSeed,
							buoyancy = burstFoamRenderSystem.renderers[i].buoyancy,
							drag = burstFoamRenderSystem.renderers[i].drag,
							airdrag = math.pow(1f - math.saturate(burstFoamRenderSystem.renderers[i].atmosphericDrag), deltaTime),
							airAging = burstFoamRenderSystem.renderers[i].airAging,
							particleSize = burstFoamRenderSystem.renderers[i].size,
							sizeRandom = burstFoamRenderSystem.renderers[i].sizeRandom,
							lifetime = burstFoamRenderSystem.renderers[i].lifetime,
							lifetimeRandom = burstFoamRenderSystem.renderers[i].lifetimeRandom,
							foamColor = (Vector4)burstFoamRenderSystem.renderers[i].color,
							deltaTime = deltaTime
						}, particleNumberToEmit, 128, inputDeps);
					}
					else
					{
						inputDeps = IJobParallelForExtensions.Schedule(new GenerateParticlesJob
						{
							activeParticles = new NativeArray<int>(burstFoamRenderSystem.renderers[i].actor.solverIndices.AsNativeArray<int>(), Allocator.TempJob),
							positions = positions,
							velocities = velocities,
							angularVelocities = angularVelocities,
							principalRadii = principalRadii,
							fluidData = fluidData,
							outputPositions = abstraction.foamPositions.AsNativeArray<float4>(),
							outputVelocities = abstraction.foamVelocities.AsNativeArray<float4>(),
							outputColors = abstraction.foamColors.AsNativeArray<float4>(),
							outputAttributes = abstraction.foamAttributes.AsNativeArray<float4>(),
							dispatchBuffer = abstraction.foamCount.AsNativeArray<int>(),
							randomSeed = randomSeed,
							vorticityRange = burstFoamRenderSystem.renderers[i].vorticityRange,
							velocityRange = burstFoamRenderSystem.renderers[i].velocityRange,
							foamGenerationRate = burstFoamRenderSystem.renderers[i].foamGenerationRate,
							potentialIncrease = burstFoamRenderSystem.renderers[i].foamPotential,
							potentialDiffusion = math.pow(1f - math.saturate(burstFoamRenderSystem.renderers[i].foamPotentialDiffusion), deltaTime),
							buoyancy = burstFoamRenderSystem.renderers[i].buoyancy,
							drag = burstFoamRenderSystem.renderers[i].drag,
							airdrag = math.pow(1f - math.saturate(burstFoamRenderSystem.renderers[i].atmosphericDrag), deltaTime),
							isosurface = burstFoamRenderSystem.renderers[i].isosurface,
							airAging = burstFoamRenderSystem.renderers[i].airAging,
							particleSize = burstFoamRenderSystem.renderers[i].size,
							sizeRandom = burstFoamRenderSystem.renderers[i].sizeRandom,
							lifetime = burstFoamRenderSystem.renderers[i].lifetime,
							lifetimeRandom = burstFoamRenderSystem.renderers[i].lifetimeRandom,
							foamColor = (Vector4)burstFoamRenderSystem.renderers[i].color,
							deltaTime = deltaTime
						}, burstFoamRenderSystem.renderers[i].actor.activeParticleCount, 128, inputDeps);
					}
				}
				inputDeps = new UpdateParticlesJob
				{
					positions = positions,
					orientations = renderableOrientations,
					principalRadii = renderableRadii,
					velocities = velocities,
					angularVelocities = angularVelocities,
					fluidData = fluidData,
					fluidMaterial = fluidMaterials,
					simplices = simplices,
					simplexCounts = simplexCounts,
					grid = particleGrid.grid,
					gridLevels = particleGrid.grid.populatedLevels.GetKeyArray(Allocator.TempJob),
					densityKernel = new Poly6Kernel(abstraction.parameters.mode == Oni.SolverParameters.Mode.Mode2D),
					inputPositions = abstraction.foamPositions.AsNativeArray<float4>(),
					inputVelocities = abstraction.foamVelocities.AsNativeArray<float4>(),
					inputColors = abstraction.foamColors.AsNativeArray<float4>(),
					inputAttributes = abstraction.foamAttributes.AsNativeArray<float4>(),
					outputPositions = auxPositions,
					outputVelocities = auxVelocities,
					outputColors = auxColors,
					outputAttributes = auxAttributes,
					dispatchBuffer = abstraction.foamCount.AsNativeArray<int>(),
					parameters = abstraction.parameters,
					agingOverPopulation = new Vector3(abstraction.foamAccelAgingRange.x, abstraction.foamAccelAgingRange.y, abstraction.foamAccelAging),
					minFluidNeighbors = abstraction.foamMinNeighbors,
					currentAliveParticles = ptr[3],
					deltaTime = deltaTime
				}.Schedule(ptr + 3, 64, inputDeps);
				inputDeps = new CopyJob
				{
					inputPositions = auxPositions,
					inputVelocities = auxVelocities,
					inputColors = auxColors,
					inputAttributes = auxAttributes,
					outputPositions = abstraction.foamPositions.AsNativeArray<float4>(),
					outputVelocities = abstraction.foamVelocities.AsNativeArray<float4>(),
					outputColors = abstraction.foamColors.AsNativeArray<float4>(),
					outputAttributes = abstraction.foamAttributes.AsNativeArray<float4>(),
					dispatchBuffer = abstraction.foamCount.AsNativeArray<int>()
				}.Schedule(ptr + 7, 256, inputDeps);
				activeFoamParticleCount = (uint)ptr[3];
			}
			return inputDeps;
		}

		public void SpatialQuery(ObiNativeQueryShapeList shapes, ObiNativeAffineTransformList transforms, ObiNativeQueryResultList results)
		{
			NativeQueue<BurstQueryResult> nativeQueue = new NativeQueue<BurstQueryResult>(Allocator.Persistent);
			particleGrid.SpatialQuery(this, shapes.AsNativeArray<BurstQueryShape>(), transforms.AsNativeArray<BurstAffineTransform>(), nativeQueue).Complete();
			int count = nativeQueue.Count;
			results.ResizeUninitialized(count);
			new DequeueIntoArrayJob<BurstQueryResult>
			{
				InputQueue = nativeQueue,
				OutputArray = results.AsNativeArray<BurstQueryResult>()
			}.Schedule().Complete();
			nativeQueue.Dispose();
		}

		public int GetParticleGridSize()
		{
			return particleGrid.grid.usedCells.Length;
		}

		public void GetParticleGrid(ObiNativeAabbList cells)
		{
			particleGrid.GetCells(cells);
		}
	}
}
