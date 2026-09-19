using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstDensityConstraintsBatch : BurstConstraintsBatchImpl, IDensityConstraintsBatchImpl, IConstraintsBatchImpl
	{
		[BurstCompile]
		public struct UpdateDensitiesJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<float4> positions;

			[ReadOnly]
			public NativeArray<float4> prevPositions;

			[ReadOnly]
			public NativeArray<float4> fluidMaterials;

			[ReadOnly]
			public NativeArray<float4> principalRadii;

			[ReadOnly]
			public NativeArray<FluidInteraction> pairs;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> fluidData;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4x4> moments;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> massCenters;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> prevMassCenters;

			[ReadOnly]
			public Poly6Kernel densityKernel;

			[ReadOnly]
			public BatchData batchData;

			[ReadOnly]
			public Oni.SolverParameters solverParams;

			public void Execute(int workItemIndex)
			{
				batchData.GetConstraintRange(workItemIndex, out var start, out var end);
				for (int i = start; i < end; i++)
				{
					FluidInteraction fluidInteraction = pairs[i];
					float num = math.pow(principalRadii[fluidInteraction.particleA].x * 2f, (float)(3 - solverParams.mode));
					float num2 = math.pow(principalRadii[fluidInteraction.particleB].x * 2f, (float)(3 - solverParams.mode));
					float num3 = num2 * fluidInteraction.avgGradient;
					float num4 = num * fluidInteraction.avgGradient;
					float num5 = num2 / num;
					float num6 = num / num2;
					fluidData[fluidInteraction.particleA] += new float4(num5 * fluidInteraction.avgKernel, 0f, num3, num3 * num3);
					fluidData[fluidInteraction.particleB] += new float4(num6 * fluidInteraction.avgKernel, 0f, num4, num4 * num4);
					float num7 = fluidInteraction.avgKernel / ((densityKernel.W(0f, fluidMaterials[fluidInteraction.particleA].x) + densityKernel.W(0f, fluidMaterials[fluidInteraction.particleB].x)) * 0.5f);
					massCenters[fluidInteraction.particleA] += num7 * new float4(positions[fluidInteraction.particleB].xyz, 1f) / positions[fluidInteraction.particleB].w;
					massCenters[fluidInteraction.particleB] += num7 * new float4(positions[fluidInteraction.particleA].xyz, 1f) / positions[fluidInteraction.particleA].w;
					prevMassCenters[fluidInteraction.particleA] += num7 * new float4(prevPositions[fluidInteraction.particleB].xyz, 1f) / positions[fluidInteraction.particleB].w;
					prevMassCenters[fluidInteraction.particleB] += num7 * new float4(prevPositions[fluidInteraction.particleA].xyz, 1f) / positions[fluidInteraction.particleA].w;
					moments[fluidInteraction.particleA] += num7 * (BurstMath.multrnsp4(positions[fluidInteraction.particleB], prevPositions[fluidInteraction.particleB]) + float4x4.identity * math.pow(principalRadii[fluidInteraction.particleB].x, 2f) * 0.001f) / positions[fluidInteraction.particleB].w;
					moments[fluidInteraction.particleB] += num7 * (BurstMath.multrnsp4(positions[fluidInteraction.particleA], prevPositions[fluidInteraction.particleA]) + float4x4.identity * math.pow(principalRadii[fluidInteraction.particleA].x, 2f) * 0.001f) / positions[fluidInteraction.particleA].w;
				}
			}
		}

		[BurstCompile]
		public struct ApplyDensityConstraintsJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<float4> principalRadii;

			[ReadOnly]
			public NativeArray<float4> fluidMaterials;

			[ReadOnly]
			public NativeArray<FluidInteraction> pairs;

			[ReadOnly]
			public Poly6Kernel densityKernel;

			[ReadOnly]
			public CohesionKernel cohesionKernel;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> positions;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> fluidData;

			[ReadOnly]
			public BatchData batchData;

			[ReadOnly]
			public float sorFactor;

			[ReadOnly]
			public Oni.SolverParameters solverParams;

			public void Execute(int workItemIndex)
			{
				batchData.GetConstraintRange(workItemIndex, out var start, out var end);
				for (int i = start; i < end; i++)
				{
					FluidInteraction fluidInteraction = pairs[i];
					float num = math.pow(principalRadii[fluidInteraction.particleA].x * 2f, (float)(3 - solverParams.mode));
					float num2 = math.pow(principalRadii[fluidInteraction.particleB].x * 2f, (float)(3 - solverParams.mode));
					float r = math.length(positions[fluidInteraction.particleA].xyz - positions[fluidInteraction.particleB].xyz);
					float num3 = (cohesionKernel.W(r, fluidMaterials[fluidInteraction.particleA].x * 1.4f) + cohesionKernel.W(r, fluidMaterials[fluidInteraction.particleB].x * 1.4f)) * 0.5f;
					float num4 = 0.2f * num3 * (1f - math.saturate(math.abs(fluidMaterials[fluidInteraction.particleA].y - fluidMaterials[fluidInteraction.particleB].y))) * (fluidMaterials[fluidInteraction.particleA].y + fluidMaterials[fluidInteraction.particleB].y) * 0.5f;
					float num5 = (0f - num4) / (positions[fluidInteraction.particleA].w * fluidData[fluidInteraction.particleA][3] + 1.1754944E-38f);
					float num6 = (0f - num4) / (positions[fluidInteraction.particleB].w * fluidData[fluidInteraction.particleB][3] + 1.1754944E-38f);
					float4 float5 = fluidInteraction.gradient * fluidInteraction.avgGradient * ((fluidData[fluidInteraction.particleA][1] + num5) * num2 + (fluidData[fluidInteraction.particleB][1] + num6) * num) * sorFactor;
					float5.w = 0f;
					positions[fluidInteraction.particleA] += float5 * positions[fluidInteraction.particleA].w;
					positions[fluidInteraction.particleB] -= float5 * positions[fluidInteraction.particleB].w;
				}
			}
		}

		[BurstCompile]
		public struct ViscosityVorticityJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<float4> positions;

			[ReadOnly]
			public NativeArray<float4> prevPositions;

			[ReadOnly]
			public NativeArray<quaternion> matchingRotations;

			[ReadOnly]
			public NativeArray<float4> fluidParams;

			[ReadOnly]
			public NativeArray<FluidInteraction> pairs;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> massCenters;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> prevMassCenters;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> deltas;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<int> counts;

			[ReadOnly]
			public BatchData batchData;

			public void Execute(int workItemIndex)
			{
				batchData.GetConstraintRange(workItemIndex, out var start, out var end);
				for (int i = start; i < end; i++)
				{
					FluidInteraction fluidInteraction = pairs[i];
					float num = math.min(fluidParams[fluidInteraction.particleA].z, fluidParams[fluidInteraction.particleB].z);
					float4 float5 = new float4(massCenters[fluidInteraction.particleB].xyz + math.rotate(matchingRotations[fluidInteraction.particleB], (prevPositions[fluidInteraction.particleA] - prevMassCenters[fluidInteraction.particleB]).xyz), 0f);
					float4 float6 = new float4(massCenters[fluidInteraction.particleA].xyz + math.rotate(matchingRotations[fluidInteraction.particleA], (prevPositions[fluidInteraction.particleB] - prevMassCenters[fluidInteraction.particleA]).xyz), 0f);
					deltas[fluidInteraction.particleA] += (float5 - positions[fluidInteraction.particleA]) * num;
					deltas[fluidInteraction.particleB] += (float6 - positions[fluidInteraction.particleB]) * num;
					counts[fluidInteraction.particleA]++;
					counts[fluidInteraction.particleB]++;
				}
			}
		}

		[BurstCompile]
		public struct NormalsJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<float> invMasses;

			[ReadOnly]
			public NativeArray<float4> velocities;

			[ReadOnly]
			public NativeArray<float4> angularVelocities;

			[ReadOnly]
			public NativeArray<float4> positions;

			[ReadOnly]
			public NativeArray<float4> vorticity;

			[ReadOnly]
			public NativeArray<float4> principalRadii;

			[ReadOnly]
			public NativeArray<float4> fluidMaterials;

			[ReadOnly]
			public NativeArray<float4> fluidMaterials2;

			[ReadOnly]
			public NativeArray<float4> fluidInterface;

			[ReadOnly]
			public NativeArray<FluidInteraction> pairs;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> fluidData;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> userData;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> normals;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> linearAccelerations;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> vorticityAccelerations;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> linearFromAngular;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4x4> angularDiffusion;

			[ReadOnly]
			public Poly6Kernel densityKernel;

			[ReadOnly]
			public SpikyKernel gradKernel;

			[ReadOnly]
			public BatchData batchData;

			[ReadOnly]
			public Oni.SolverParameters solverParams;

			[ReadOnly]
			public float dt;

			public void Execute(int workItemIndex)
			{
				batchData.GetConstraintRange(workItemIndex, out var start, out var end);
				for (int i = start; i < end; i++)
				{
					FluidInteraction fluidInteraction = pairs[i];
					float num = math.pow(principalRadii[fluidInteraction.particleA].x * 2f, (float)(3 - solverParams.mode));
					float num2 = math.pow(principalRadii[fluidInteraction.particleB].x * 2f, (float)(3 - solverParams.mode));
					float num3 = invMasses[fluidInteraction.particleA] / fluidData[fluidInteraction.particleA].x;
					float num4 = invMasses[fluidInteraction.particleB] / fluidData[fluidInteraction.particleB].x;
					float3 float5 = velocities[fluidInteraction.particleA].xyz - velocities[fluidInteraction.particleB].xyz;
					float3 float6 = angularVelocities[fluidInteraction.particleA].xyz - angularVelocities[fluidInteraction.particleB].xyz;
					float3 float7 = vorticity[fluidInteraction.particleA].xyz - vorticity[fluidInteraction.particleB].xyz;
					float4 float8 = new float4((positions[fluidInteraction.particleA] - positions[fluidInteraction.particleB]).xyz, 0f);
					float num5 = math.length(float8);
					float num6 = (gradKernel.W(num5, fluidMaterials[fluidInteraction.particleA].x) + gradKernel.W(num5, fluidMaterials[fluidInteraction.particleB].x)) * 0.5f;
					float num7 = (densityKernel.W(num5, fluidMaterials[fluidInteraction.particleA].x) + densityKernel.W(num5, fluidMaterials[fluidInteraction.particleB].x)) * 0.5f;
					float num8 = (densityKernel.W(0f, fluidMaterials[fluidInteraction.particleA].x) + densityKernel.W(0f, fluidMaterials[fluidInteraction.particleB].x)) * 0.5f;
					float num9 = (fluidInterface[fluidInteraction.particleA].w + fluidInterface[fluidInteraction.particleB].w) * num7 * dt;
					float4 float9 = (userData[fluidInteraction.particleB] - userData[fluidInteraction.particleA]) * solverParams.diffusionMask * num9;
					userData[fluidInteraction.particleA] += num2 / num * float9;
					userData[fluidInteraction.particleB] -= num / num2 * float9;
					float4 float10 = float8 / (num5 + 1E-07f);
					float4 float11 = float10 * num6;
					float num10 = (fluidMaterials[fluidInteraction.particleA].x + fluidMaterials[fluidInteraction.particleB].x) * 0.5f;
					normals[fluidInteraction.particleA] += float11 * num10 * num2;
					normals[fluidInteraction.particleB] -= float11 * num10 * num;
					float4 value = fluidData[fluidInteraction.particleA];
					float4 value2 = fluidData[fluidInteraction.particleB];
					float num11 = math.length(float5) + 1E-07f;
					float num12 = 1f - math.min(1f, num5 / (num10 + 1E-07f));
					float num13 = num11 * (1f - math.dot(float5 / num11, float10.xyz)) * num12;
					value.z += num13;
					value2.z += num13;
					fluidData[fluidInteraction.particleA] = value;
					fluidData[fluidInteraction.particleB] = value2;
					float3 float12 = math.cross(float5, float11.xyz);
					float3 float13 = math.cross(float7, float11.xyz);
					linearAccelerations[fluidInteraction.particleA] += new float4(float13 / invMasses[fluidInteraction.particleB] * num3, 0f);
					linearAccelerations[fluidInteraction.particleB] += new float4(float13 / invMasses[fluidInteraction.particleA] * num4, 0f);
					vorticityAccelerations[fluidInteraction.particleA] += new float4(float12 / invMasses[fluidInteraction.particleB] * num3, 0f);
					vorticityAccelerations[fluidInteraction.particleB] += new float4(float12 / invMasses[fluidInteraction.particleA] * num4, 0f);
					float4x4 value3 = angularDiffusion[fluidInteraction.particleA];
					float4x4 value4 = angularDiffusion[fluidInteraction.particleB];
					value3.c0 += new float4(float6 * num7 / invMasses[fluidInteraction.particleB] * num3, 0f);
					value4.c0 -= new float4(float6 * num7 / invMasses[fluidInteraction.particleA] * num4, 0f);
					value3.c1 += new float4(float7 * num7 / invMasses[fluidInteraction.particleB] * num3, 0f);
					value4.c1 -= new float4(float7 * num7 / invMasses[fluidInteraction.particleA] * num4, 0f);
					angularDiffusion[fluidInteraction.particleA] = value3;
					angularDiffusion[fluidInteraction.particleB] = value4;
					linearFromAngular[fluidInteraction.particleA] += new float4(math.cross(angularVelocities[fluidInteraction.particleB].xyz, float8.xyz) * num7 / num8, 0f);
					linearFromAngular[fluidInteraction.particleB] -= new float4(math.cross(angularVelocities[fluidInteraction.particleA].xyz, float8.xyz) * num7 / num8, 0f);
				}
			}
		}

		[BurstCompile]
		public struct AccumulateSmoothPositionsJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<float4> renderablePositions;

			[ReadOnly]
			public NativeArray<float4> fluidMaterials;

			[ReadOnly]
			public Poly6Kernel densityKernel;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4x4> anisotropies;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<FluidInteraction> pairs;

			[ReadOnly]
			public BatchData batchData;

			public void Execute(int workItemIndex)
			{
				batchData.GetConstraintRange(workItemIndex, out var start, out var end);
				for (int i = start; i < end; i++)
				{
					FluidInteraction value = pairs[i];
					float r = math.length((renderablePositions[value.particleA] - renderablePositions[value.particleB]).xyz);
					value.avgKernel = (densityKernel.W(r, fluidMaterials[value.particleA].x) + densityKernel.W(r, fluidMaterials[value.particleB].x)) * 0.5f;
					float4x4 value2 = anisotropies[value.particleA];
					float4x4 value3 = anisotropies[value.particleB];
					value2.c3 += new float4(renderablePositions[value.particleB].xyz, 1f) * value.avgKernel;
					value3.c3 += new float4(renderablePositions[value.particleA].xyz, 1f) * value.avgKernel;
					anisotropies[value.particleA] = value2;
					anisotropies[value.particleB] = value3;
					pairs[i] = value;
				}
			}
		}

		[BurstCompile]
		public struct AccumulateAnisotropyJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<float4> renderablePositions;

			[ReadOnly]
			public NativeArray<FluidInteraction> pairs;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4x4> anisotropies;

			[ReadOnly]
			public BatchData batchData;

			public void Execute(int workItemIndex)
			{
				batchData.GetConstraintRange(workItemIndex, out var start, out var end);
				for (int i = start; i < end; i++)
				{
					FluidInteraction fluidInteraction = pairs[i];
					float4 column = (renderablePositions[fluidInteraction.particleB] - anisotropies[fluidInteraction.particleA].c3) * fluidInteraction.avgKernel;
					float4 column2 = (renderablePositions[fluidInteraction.particleA] - anisotropies[fluidInteraction.particleB].c3) * fluidInteraction.avgKernel;
					anisotropies[fluidInteraction.particleA] += BurstMath.multrnsp4(in column, column);
					anisotropies[fluidInteraction.particleB] += BurstMath.multrnsp4(in column2, column2);
				}
			}
		}

		public BatchData batchData;

		public BurstDensityConstraintsBatch(BurstDensityConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Density;
		}

		public override JobHandle Initialize(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return inputDeps;
		}

		public override JobHandle Evaluate(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return new UpdateDensitiesJob
			{
				pairs = ((BurstSolverImpl)base.constraints.solver).fluidInteractions,
				positions = base.solverImplementation.positions,
				prevPositions = base.solverImplementation.prevPositions,
				principalRadii = base.solverImplementation.principalRadii,
				fluidMaterials = base.solverImplementation.fluidMaterials,
				fluidData = base.solverImplementation.fluidData,
				moments = base.solverImplementation.anisotropies,
				massCenters = base.solverImplementation.normals,
				prevMassCenters = base.solverImplementation.renderablePositions,
				densityKernel = new Poly6Kernel(base.solverAbstraction.parameters.mode == Oni.SolverParameters.Mode.Mode2D),
				batchData = batchData,
				solverParams = base.solverAbstraction.parameters
			}.Schedule(innerloopBatchCount: (!batchData.isLast) ? 1 : batchData.workItemCount, arrayLength: batchData.workItemCount, dependsOn: inputDeps);
		}

		public override JobHandle Apply(JobHandle inputDeps, float substepTime)
		{
			Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
			return new ApplyDensityConstraintsJob
			{
				principalRadii = base.solverImplementation.principalRadii,
				fluidMaterials = base.solverImplementation.fluidMaterials,
				pairs = ((BurstSolverImpl)base.constraints.solver).fluidInteractions,
				densityKernel = new Poly6Kernel(base.solverAbstraction.parameters.mode == Oni.SolverParameters.Mode.Mode2D),
				positions = base.solverImplementation.positions,
				fluidData = base.solverImplementation.fluidData,
				batchData = batchData,
				solverParams = base.solverAbstraction.parameters,
				sorFactor = constraintParameters.SORFactor
			}.Schedule(innerloopBatchCount: (!batchData.isLast) ? 1 : batchData.workItemCount, arrayLength: batchData.workItemCount, dependsOn: inputDeps);
		}

		public JobHandle CalculateNormals(JobHandle inputDeps, float deltaTime)
		{
			int innerloopBatchCount = ((!batchData.isLast) ? 1 : batchData.workItemCount);
			return IJobParallelForExtensions.Schedule(new NormalsJob
			{
				invMasses = base.solverImplementation.invMasses,
				positions = base.solverImplementation.positions,
				principalRadii = base.solverImplementation.principalRadii,
				fluidMaterials = base.solverImplementation.fluidMaterials,
				fluidMaterials2 = base.solverImplementation.fluidMaterials2,
				fluidData = base.solverImplementation.fluidData,
				fluidInterface = base.solverImplementation.fluidInterface,
				velocities = base.solverImplementation.velocities,
				angularVelocities = base.solverImplementation.angularVelocities,
				vorticityAccelerations = base.solverImplementation.orientationDeltas.Reinterpret<float4>(),
				vorticity = base.solverImplementation.restOrientations.Reinterpret<float4>(),
				linearAccelerations = base.solverImplementation.positionDeltas,
				linearFromAngular = base.solverImplementation.restPositions,
				angularDiffusion = base.solverImplementation.anisotropies,
				userData = base.solverImplementation.userData,
				pairs = ((BurstSolverImpl)base.constraints.solver).fluidInteractions,
				normals = base.solverImplementation.normals,
				densityKernel = new Poly6Kernel(base.solverAbstraction.parameters.mode == Oni.SolverParameters.Mode.Mode2D),
				gradKernel = new SpikyKernel(base.solverAbstraction.parameters.mode == Oni.SolverParameters.Mode.Mode2D),
				solverParams = base.solverAbstraction.parameters,
				batchData = batchData,
				dt = deltaTime
			}, batchData.workItemCount, innerloopBatchCount, inputDeps);
		}

		public JobHandle ViscosityAndVorticity(JobHandle inputDeps)
		{
			return new ViscosityVorticityJob
			{
				positions = base.solverImplementation.positions,
				prevPositions = base.solverImplementation.prevPositions,
				matchingRotations = base.solverImplementation.restPositions.Reinterpret<quaternion>(),
				pairs = ((BurstSolverImpl)base.constraints.solver).fluidInteractions,
				massCenters = base.solverImplementation.normals,
				prevMassCenters = base.solverImplementation.renderablePositions,
				fluidParams = base.solverImplementation.fluidMaterials,
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				batchData = batchData
			}.Schedule(innerloopBatchCount: (!batchData.isLast) ? 1 : batchData.workItemCount, arrayLength: batchData.workItemCount, dependsOn: inputDeps);
		}

		public JobHandle AccumulateSmoothPositions(JobHandle inputDeps)
		{
			return new AccumulateSmoothPositionsJob
			{
				renderablePositions = base.solverImplementation.renderablePositions,
				anisotropies = base.solverImplementation.anisotropies,
				fluidMaterials = base.solverImplementation.fluidMaterials,
				densityKernel = new Poly6Kernel(base.solverAbstraction.parameters.mode == Oni.SolverParameters.Mode.Mode2D),
				pairs = ((BurstSolverImpl)base.constraints.solver).fluidInteractions,
				batchData = batchData
			}.Schedule(innerloopBatchCount: (!batchData.isLast) ? 1 : batchData.workItemCount, arrayLength: batchData.workItemCount, dependsOn: inputDeps);
		}

		public JobHandle AccumulateAnisotropy(JobHandle inputDeps)
		{
			return new AccumulateAnisotropyJob
			{
				renderablePositions = base.solverImplementation.renderablePositions,
				anisotropies = base.solverImplementation.anisotropies,
				pairs = ((BurstSolverImpl)base.constraints.solver).fluidInteractions,
				batchData = batchData
			}.Schedule(innerloopBatchCount: (!batchData.isLast) ? 1 : batchData.workItemCount, arrayLength: batchData.workItemCount, dependsOn: inputDeps);
		}
	}
}
