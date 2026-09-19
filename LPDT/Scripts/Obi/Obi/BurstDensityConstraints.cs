using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstDensityConstraints : BurstConstraintsImpl<BurstDensityConstraintsBatch>
	{
		[BurstCompile]
		public struct ClearFluidDataJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeList<int> fluidParticles;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> fluidData;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> massCenters;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> prevMassCenters;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4x4> moments;

			public void Execute(int i)
			{
				int index = fluidParticles[i];
				fluidData[index] = float4.zero;
				massCenters[index] = float4.zero;
				prevMassCenters[index] = float4.zero;
				moments[index] = float4x4.zero;
			}
		}

		[BurstCompile]
		public struct UpdateInteractionsJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<float4> positions;

			[ReadOnly]
			public NativeArray<float4> fluidMaterials;

			[ReadOnly]
			public Poly6Kernel densityKernel;

			[ReadOnly]
			public SpikyKernel gradientKernel;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<FluidInteraction> pairs;

			[ReadOnly]
			public BatchData batchData;

			public void Execute(int i)
			{
				FluidInteraction value = pairs[i];
				value.gradient = new float4((positions[value.particleA] - positions[value.particleB]).xyz, 0f);
				float num = math.length(value.gradient);
				value.gradient /= num + 1.1754944E-38f;
				value.avgKernel = (densityKernel.W(num, fluidMaterials[value.particleA].x) + densityKernel.W(num, fluidMaterials[value.particleB].x)) * 0.5f;
				value.avgGradient = (gradientKernel.W(num, fluidMaterials[value.particleA].x) + gradientKernel.W(num, fluidMaterials[value.particleB].x)) * 0.5f;
				pairs[i] = value;
			}
		}

		[BurstCompile]
		public struct CalculateLambdasJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeList<int> fluidParticles;

			[ReadOnly]
			public NativeArray<float4> positions;

			[ReadOnly]
			public NativeArray<float4> prevPositions;

			[ReadOnly]
			public NativeArray<float4> principalRadii;

			[ReadOnly]
			public NativeArray<float4> fluidMaterials;

			[ReadOnly]
			public Poly6Kernel densityKernel;

			[ReadOnly]
			public SpikyKernel gradientKernel;

			[ReadOnly]
			public Oni.SolverParameters solverParams;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> fluidData;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> massCenters;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> prevMassCenters;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4x4> moments;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<quaternion> matchingRotations;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> deltas;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<int> counts;

			public void Execute(int p)
			{
				int index = fluidParticles[p];
				float num = math.pow(principalRadii[index].x * 2f, (float)(3 - solverParams.mode));
				float4 value = fluidData[index];
				float num2 = num * gradientKernel.W(0f, fluidMaterials[index].x);
				value += new float4(densityKernel.W(0f, fluidMaterials[index].x), 0f, num2, num2 * num2 + value[2] * value[2]);
				massCenters[index] += new float4(positions[index].xyz, 1f) / positions[index].w;
				prevMassCenters[index] += new float4(prevPositions[index].xyz, 1f) / positions[index].w;
				float num3 = math.max(0f, value[0] * num - 1f) * fluidMaterials[index].w;
				value[1] = (0f - num3) / (positions[index].w * value[3] + 1.1754944E-38f);
				fluidData[index] = value;
				float num4 = massCenters[index][3];
				massCenters[index] /= massCenters[index][3];
				prevMassCenters[index] /= prevMassCenters[index][3];
				moments[index] += (BurstMath.multrnsp4(positions[index], prevPositions[index]) + float4x4.identity * math.pow(principalRadii[index].x, 2f) * 0.001f) / positions[index].w;
				moments[index] -= num4 * BurstMath.multrnsp4(massCenters[index], prevMassCenters[index]);
				matchingRotations[index] = BurstMath.ExtractRotation(moments[index], quaternion.identity, 5);
				float4 float5 = new float4(massCenters[index].xyz + math.rotate(matchingRotations[index], (prevPositions[index] - prevMassCenters[index]).xyz), 0f);
				deltas[index] += (float5 - positions[index]) * fluidMaterials[index].z;
				counts[index]++;
			}
		}

		[BurstCompile]
		public struct ApplyPositionDeltasJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeList<int> fluidParticles;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> positions;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> deltas;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<int> counts;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> normals;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4x4> anisotropies;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<quaternion> matchingRotations;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> linearFromAngular;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> fluidData;

			public void Execute(int p)
			{
				int index = fluidParticles[p];
				if (counts[index] > 0)
				{
					positions[index] += new float4(deltas[index].xyz, 0f) / counts[index];
					deltas[index] = float4.zero;
					counts[index] = 0;
				}
				normals[index] = float4.zero;
				anisotropies[index] = float4x4.zero;
				linearFromAngular[index] = float4.zero;
				matchingRotations[index] = new quaternion(0f, 0f, 0f, 0f);
				float4 value = fluidData[index];
				value.z = 0f;
				fluidData[index] = value;
			}
		}

		[BurstCompile]
		public struct ApplyAtmosphereJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeList<int> fluidParticles;

			[ReadOnly]
			public NativeArray<float4> wind;

			[ReadOnly]
			public NativeArray<float4> fluidInterface;

			[ReadOnly]
			public NativeArray<float4> fluidMaterials2;

			[ReadOnly]
			public NativeArray<float4> principalRadii;

			[ReadOnly]
			public NativeArray<float4> normals;

			[ReadOnly]
			public NativeArray<float4> fluidData;

			[ReadOnly]
			public NativeArray<float4> linearFromAngular;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> positions;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> prevPositions;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> linearAccelerations;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> vorticityAccelerations;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> vorticity;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4x4> angularDiffusion;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> angularVelocities;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> velocities;

			[ReadOnly]
			public float dt;

			[ReadOnly]
			public Oni.SolverParameters solverParams;

			public void Execute(int p)
			{
				int index = fluidParticles[p];
				float num = math.pow(principalRadii[index].x * 2f, (float)(3 - solverParams.mode));
				float4 float5 = velocities[index] - wind[index];
				velocities[index] -= fluidInterface[index].x * float5 * math.max(0f, 1f - fluidData[index][0] * num) * dt;
				velocities[index] += fluidInterface[index].y * normals[index] * dt;
				angularVelocities[index] += new float4(fluidMaterials2[index].z * math.cross(-normals[index].xyz, -float5.xyz), 0f) * dt;
				angularVelocities[index] -= fluidMaterials2[index].w * angularDiffusion[index].c0;
				velocities[index] += fluidMaterials2[index].x * linearAccelerations[index] * dt;
				vorticity[index] += fluidMaterials2[index].x * (vorticityAccelerations[index] * 0.5f - vorticity[index]) * dt;
				vorticity[index] -= fluidMaterials2[index].y * angularDiffusion[index].c1;
				linearAccelerations[index] = float4.zero;
				vorticityAccelerations[index] = float4.zero;
				angularDiffusion[index] = float4x4.zero;
				positions[index] += new float4(linearFromAngular[index].xyz * dt, 0f);
				prevPositions[index] += new float4(linearFromAngular[index].xyz * dt, 0f);
			}
		}

		[BurstCompile]
		public struct AverageSmoothPositionsJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeList<int> fluidParticles;

			[ReadOnly]
			public NativeArray<float4> renderablePositions;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4x4> anisotropies;

			public void Execute(int p)
			{
				int index = fluidParticles[p];
				float4x4 value = anisotropies[index];
				if (value.c3.w > 0f)
				{
					value.c3 /= value.c3.w;
				}
				else
				{
					value.c3.xyz = renderablePositions[index].xyz;
				}
				anisotropies[index] = value;
			}
		}

		[BurstCompile]
		public struct AverageAnisotropyJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeList<int> fluidParticles;

			[ReadOnly]
			public NativeArray<float4> principalRadii;

			[ReadOnly]
			public float maxAnisotropy;

			[ReadOnly]
			public NativeArray<float4x4> anisotropies;

			[ReadOnly]
			public NativeArray<float> life;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> fluidData;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> renderablePositions;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<quaternion> renderableOrientations;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> renderableRadii;

			[ReadOnly]
			public Oni.SolverParameters solverParams;

			public void Execute(int p)
			{
				int index = fluidParticles[p];
				if (anisotropies[index].c3.w > 0f && anisotropies[index].c0[0] + anisotropies[index].c1[1] + anisotropies[index].c2[2] > 0.01f)
				{
					BurstMath.EigenSolve(math.float3x3(anisotropies[index] / anisotropies[index].c3.w), out var S, out var V);
					float num = S[0];
					float3 float5 = math.max(S, new float3(num / maxAnisotropy)) / num * principalRadii[index].x;
					renderableOrientations[index] = quaternion.LookRotationSafe(V.c2, V.c1);
					renderableRadii[index] = new float4(float5.xyz, 1f);
				}
				else
				{
					float num2 = principalRadii[index].x / maxAnisotropy;
					renderableOrientations[index] = quaternion.identity;
					renderableRadii[index] = new float4(num2, num2, num2, 1f);
					float4 value = fluidData[index];
					value.x = 1f / math.pow(math.abs(num2 * 2f), (float)(3 - solverParams.mode));
					fluidData[index] = value;
				}
				renderablePositions[index] = math.lerp(renderablePositions[index], anisotropies[index].c3, math.min((maxAnisotropy - 1f) / 3f, 1f));
				float4 value2 = renderableRadii[index];
				value2.w = ((life[index] <= 0f) ? 0f : value2.w);
				renderableRadii[index] = value2;
			}
		}

		public NativeList<int> fluidParticles;

		public BurstDensityConstraints(BurstSolverImpl solver)
			: base(solver, Oni.ConstraintType.Density)
		{
			fluidParticles = new NativeList<int>(Allocator.Persistent);
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			BurstDensityConstraintsBatch burstDensityConstraintsBatch = new BurstDensityConstraintsBatch(this);
			batches.Add(burstDensityConstraintsBatch);
			return burstDensityConstraintsBatch;
		}

		public override void Dispose()
		{
			fluidParticles.Dispose();
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as BurstDensityConstraintsBatch);
			batch.Destroy();
		}

		protected override JobHandle EvaluateSequential(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return EvaluateParallel(inputDeps, stepTime, substepTime, steps, timeLeft);
		}

		protected override JobHandle EvaluateParallel(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			inputDeps = UpdateInteractions(inputDeps);
			for (int i = 0; i < batches.Count; i++)
			{
				if (batches[i].enabled)
				{
					inputDeps = batches[i].Evaluate(inputDeps, stepTime, substepTime, steps, timeLeft);
					m_Solver.ScheduleBatchedJobsIfNeeded();
				}
			}
			inputDeps = CalculateLambdas(inputDeps, substepTime);
			for (int j = 0; j < batches.Count; j++)
			{
				if (batches[j].enabled)
				{
					inputDeps = batches[j].ViscosityAndVorticity(inputDeps);
					m_Solver.ScheduleBatchedJobsIfNeeded();
				}
			}
			inputDeps = IJobParallelForExtensions.Schedule(new ApplyPositionDeltasJob
			{
				fluidParticles = fluidParticles,
				positions = m_Solver.positions,
				deltas = m_Solver.positionDeltas,
				counts = m_Solver.positionConstraintCounts,
				anisotropies = m_Solver.anisotropies,
				normals = m_Solver.normals,
				fluidData = m_Solver.fluidData,
				matchingRotations = m_Solver.orientationDeltas,
				linearFromAngular = m_Solver.restPositions
			}, fluidParticles.Length, 64, inputDeps);
			for (int k = 0; k < batches.Count; k++)
			{
				if (batches[k].enabled)
				{
					inputDeps = batches[k].Apply(inputDeps, substepTime);
					m_Solver.ScheduleBatchedJobsIfNeeded();
				}
			}
			return inputDeps;
		}

		public JobHandle CalculateVelocityCorrections(JobHandle inputDeps, float deltaTime)
		{
			for (int i = 0; i < batches.Count; i++)
			{
				if (batches[i].enabled)
				{
					inputDeps = batches[i].CalculateNormals(inputDeps, deltaTime);
					m_Solver.ScheduleBatchedJobsIfNeeded();
				}
			}
			return inputDeps;
		}

		public JobHandle ApplyVelocityCorrections(JobHandle inputDeps, float deltaTime)
		{
			inputDeps = ApplyAtmosphere(inputDeps, deltaTime);
			m_Solver.ScheduleBatchedJobsIfNeeded();
			return inputDeps;
		}

		public JobHandle CalculateAnisotropyLaplacianSmoothing(JobHandle inputDeps)
		{
			if (((BurstSolverImpl)base.solver).abstraction.parameters.maxAnisotropy <= 1f)
			{
				return inputDeps;
			}
			for (int i = 0; i < batches.Count; i++)
			{
				if (batches[i].enabled)
				{
					inputDeps = batches[i].AccumulateSmoothPositions(inputDeps);
					m_Solver.ScheduleBatchedJobsIfNeeded();
				}
			}
			inputDeps = AverageSmoothPositions(inputDeps);
			for (int j = 0; j < batches.Count; j++)
			{
				if (batches[j].enabled)
				{
					inputDeps = batches[j].AccumulateAnisotropy(inputDeps);
					m_Solver.ScheduleBatchedJobsIfNeeded();
				}
			}
			return AverageAnisotropy(inputDeps);
		}

		private JobHandle UpdateInteractions(JobHandle inputDeps)
		{
			inputDeps = IJobParallelForExtensions.Schedule(new ClearFluidDataJob
			{
				fluidParticles = fluidParticles,
				fluidData = m_Solver.fluidData,
				massCenters = m_Solver.normals,
				prevMassCenters = m_Solver.renderablePositions,
				moments = m_Solver.anisotropies
			}, fluidParticles.Length, 64, inputDeps);
			return IJobParallelForExtensions.Schedule(new UpdateInteractionsJob
			{
				pairs = m_Solver.fluidInteractions,
				positions = m_Solver.positions,
				fluidMaterials = m_Solver.fluidMaterials,
				densityKernel = new Poly6Kernel(((BurstSolverImpl)base.solver).abstraction.parameters.mode == Oni.SolverParameters.Mode.Mode2D),
				gradientKernel = new SpikyKernel(((BurstSolverImpl)base.solver).abstraction.parameters.mode == Oni.SolverParameters.Mode.Mode2D)
			}, ((BurstSolverImpl)base.solver).fluidInteractions.Length, 64, inputDeps);
		}

		private JobHandle CalculateLambdas(JobHandle inputDeps, float deltaTime)
		{
			return IJobParallelForExtensions.Schedule(new CalculateLambdasJob
			{
				fluidParticles = fluidParticles,
				positions = m_Solver.positions,
				prevPositions = m_Solver.prevPositions,
				matchingRotations = m_Solver.restPositions.Reinterpret<quaternion>(),
				principalRadii = m_Solver.principalRadii,
				fluidMaterials = m_Solver.fluidMaterials,
				densityKernel = new Poly6Kernel(m_Solver.abstraction.parameters.mode == Oni.SolverParameters.Mode.Mode2D),
				gradientKernel = new SpikyKernel(m_Solver.abstraction.parameters.mode == Oni.SolverParameters.Mode.Mode2D),
				fluidData = m_Solver.fluidData,
				massCenters = m_Solver.normals,
				prevMassCenters = m_Solver.renderablePositions,
				moments = m_Solver.anisotropies,
				deltas = m_Solver.positionDeltas,
				counts = m_Solver.positionConstraintCounts,
				solverParams = m_Solver.abstraction.parameters
			}, fluidParticles.Length, 64, inputDeps);
		}

		private JobHandle ApplyAtmosphere(JobHandle inputDeps, float deltaTime)
		{
			return IJobParallelForExtensions.Schedule(new ApplyAtmosphereJob
			{
				fluidParticles = fluidParticles,
				wind = m_Solver.wind,
				fluidInterface = m_Solver.fluidInterface,
				fluidMaterials2 = m_Solver.fluidMaterials2,
				principalRadii = m_Solver.principalRadii,
				normals = m_Solver.normals,
				fluidData = m_Solver.fluidData,
				velocities = m_Solver.velocities,
				angularVelocities = m_Solver.angularVelocities,
				vorticity = m_Solver.restOrientations.Reinterpret<float4>(),
				vorticityAccelerations = m_Solver.orientationDeltas.Reinterpret<float4>(),
				linearAccelerations = m_Solver.positionDeltas,
				linearFromAngular = m_Solver.restPositions,
				angularDiffusion = m_Solver.anisotropies,
				positions = m_Solver.positions,
				prevPositions = m_Solver.prevPositions,
				dt = deltaTime,
				solverParams = m_Solver.abstraction.parameters
			}, fluidParticles.Length, 64, inputDeps);
		}

		private JobHandle AverageSmoothPositions(JobHandle inputDeps)
		{
			return IJobParallelForExtensions.Schedule(new AverageSmoothPositionsJob
			{
				fluidParticles = fluidParticles,
				renderablePositions = m_Solver.renderablePositions,
				anisotropies = m_Solver.anisotropies
			}, fluidParticles.Length, 64, inputDeps);
		}

		private JobHandle AverageAnisotropy(JobHandle inputDeps)
		{
			return IJobParallelForExtensions.Schedule(new AverageAnisotropyJob
			{
				fluidParticles = fluidParticles,
				renderablePositions = m_Solver.renderablePositions,
				renderableOrientations = m_Solver.renderableOrientations,
				principalRadii = m_Solver.principalRadii,
				anisotropies = m_Solver.anisotropies,
				maxAnisotropy = m_Solver.abstraction.parameters.maxAnisotropy,
				renderableRadii = m_Solver.renderableRadii,
				fluidData = m_Solver.fluidData,
				life = m_Solver.life,
				solverParams = m_Solver.abstraction.parameters
			}, fluidParticles.Length, 64, inputDeps);
		}
	}
}
