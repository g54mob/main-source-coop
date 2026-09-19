using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	public class ComputeVolumeConstraintsBatch : ComputeConstraintsBatchImpl, IVolumeConstraintsBatchImpl, IConstraintsBatchImpl
	{
		private GraphicsBuffer firstTriangle;

		private GraphicsBuffer numTriangles;

		private GraphicsBuffer restVolumes;

		private GraphicsBuffer pressureStiffness;

		private GraphicsBuffer volumes;

		private GraphicsBuffer denominators;

		private GraphicsBuffer triangleConstraintIndex;

		private GraphicsBuffer particles;

		private GraphicsBuffer particleConstraintIndex;

		public ComputeVolumeConstraintsBatch(ComputeVolumeConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Volume;
		}

		public void SetVolumeConstraints(ObiNativeIntList triangles, ObiNativeIntList firstTriangle, ObiNativeIntList numTriangles, ObiNativeFloatList restVolumes, ObiNativeVector2List pressureStiffness, ObiNativeFloatList lambdas, int count)
		{
			volumes = new GraphicsBuffer(GraphicsBuffer.Target.Structured, count, 4);
			denominators = new GraphicsBuffer(GraphicsBuffer.Target.Structured, count, 4);
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			List<int> list3 = new List<int>();
			for (int i = 0; i < numTriangles.count; i++)
			{
				List<int> list4 = new List<int>();
				for (int j = 0; j < numTriangles[i]; j++)
				{
					int num = firstTriangle[i] + j;
					list4.Add(triangles[num * 3]);
					list4.Add(triangles[num * 3 + 1]);
					list4.Add(triangles[num * 3 + 2]);
					list2.Add(i);
					list2.Add(i);
					list2.Add(i);
					list3.Add(i);
				}
				list4.Sort();
				int num2 = list4.Unique((int x, int y) => x == y);
				if (num2 < list4.Count)
				{
					int num3 = list4.Count - num2;
					list4.RemoveRange(num2, num3);
					list2.RemoveRange(list2.Count - num3, num3);
				}
				list.AddRange(list4);
			}
			particles = new GraphicsBuffer(GraphicsBuffer.Target.Structured, list.Count, 4);
			particleConstraintIndex = new GraphicsBuffer(GraphicsBuffer.Target.Structured, list2.Count, 4);
			triangleConstraintIndex = new GraphicsBuffer(GraphicsBuffer.Target.Structured, list3.Count, 4);
			particles.SetData(list);
			particleConstraintIndex.SetData(list2);
			triangleConstraintIndex.SetData(list3);
			particleIndices = triangles.AsComputeBuffer<int>();
			this.firstTriangle = firstTriangle.AsComputeBuffer<int>();
			this.numTriangles = numTriangles.AsComputeBuffer<int>();
			this.restVolumes = restVolumes.AsComputeBuffer<float>();
			this.pressureStiffness = pressureStiffness.AsComputeBuffer<Vector2>();
			base.lambdas = lambdas.AsComputeBuffer<float>();
			m_ConstraintCount = count;
		}

		public override void Destroy()
		{
			volumes.Dispose();
			denominators.Dispose();
			particles.Dispose();
			particleConstraintIndex.Dispose();
			triangleConstraintIndex.Dispose();
		}

		public override void Evaluate(float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (m_ConstraintCount > 0)
			{
				ComputeShader constraintsShader = ((ComputeVolumeConstraints)m_Constraints).constraintsShader;
				int gradientsKernel = ((ComputeVolumeConstraints)m_Constraints).gradientsKernel;
				int volumeKernel = ((ComputeVolumeConstraints)m_Constraints).volumeKernel;
				int denominatorsKernel = ((ComputeVolumeConstraints)m_Constraints).denominatorsKernel;
				int constraintKernel = ((ComputeVolumeConstraints)m_Constraints).constraintKernel;
				int deltasKernel = ((ComputeVolumeConstraints)m_Constraints).deltasKernel;
				int num = particleIndices.count / 3;
				constraintsShader.SetInt("activeConstraintCount", m_ConstraintCount);
				constraintsShader.SetInt("trianglesCount", num);
				constraintsShader.SetInt("particlesCount", particles.count);
				constraintsShader.SetFloat("deltaTime", substepTime);
				constraintsShader.SetBuffer(gradientsKernel, "triangles", particleIndices);
				constraintsShader.SetBuffer(gradientsKernel, "gradients", base.solverImplementation.fluidDataBuffer);
				constraintsShader.SetBuffer(gradientsKernel, "positions", base.solverImplementation.positionsBuffer);
				int threadGroupsX = ComputeMath.ThreadGroupCount(num, 128);
				constraintsShader.Dispatch(gradientsKernel, threadGroupsX, 1, 1);
				constraintsShader.SetBuffer(volumeKernel, "triangles", particleIndices);
				constraintsShader.SetBuffer(volumeKernel, "gradients", base.solverImplementation.fluidDataBuffer);
				constraintsShader.SetBuffer(volumeKernel, "volumes", volumes);
				constraintsShader.SetBuffer(volumeKernel, "positions", base.solverImplementation.positionsBuffer);
				constraintsShader.SetBuffer(volumeKernel, "triangleConstraintIndex", triangleConstraintIndex);
				constraintsShader.Dispatch(volumeKernel, threadGroupsX, 1, 1);
				constraintsShader.SetBuffer(denominatorsKernel, "particles", particles);
				constraintsShader.SetBuffer(denominatorsKernel, "particleConstraintIndex", particleConstraintIndex);
				constraintsShader.SetBuffer(denominatorsKernel, "invMasses", base.solverImplementation.invMassesBuffer);
				constraintsShader.SetBuffer(denominatorsKernel, "gradients", base.solverImplementation.fluidDataBuffer);
				constraintsShader.SetBuffer(denominatorsKernel, "denominators", denominators);
				threadGroupsX = ComputeMath.ThreadGroupCount(particles.count, 128);
				constraintsShader.Dispatch(denominatorsKernel, threadGroupsX, 1, 1);
				constraintsShader.SetBuffer(constraintKernel, "denominators", denominators);
				constraintsShader.SetBuffer(constraintKernel, "volumes", volumes);
				constraintsShader.SetBuffer(constraintKernel, "restVolumes", restVolumes);
				constraintsShader.SetBuffer(constraintKernel, "pressureStiffness", pressureStiffness);
				constraintsShader.SetBuffer(constraintKernel, "lambdas", lambdas);
				threadGroupsX = ComputeMath.ThreadGroupCount(m_ConstraintCount, 128);
				constraintsShader.Dispatch(constraintKernel, threadGroupsX, 1, 1);
				constraintsShader.SetBuffer(deltasKernel, "particles", particles);
				constraintsShader.SetBuffer(deltasKernel, "particleConstraintIndex", particleConstraintIndex);
				constraintsShader.SetBuffer(deltasKernel, "lambdas", lambdas);
				constraintsShader.SetBuffer(deltasKernel, "invMasses", base.solverImplementation.invMassesBuffer);
				constraintsShader.SetBuffer(deltasKernel, "gradients", base.solverImplementation.fluidDataBuffer);
				constraintsShader.SetBuffer(deltasKernel, "deltasAsInt", base.solverImplementation.positionDeltasIntBuffer);
				constraintsShader.SetBuffer(deltasKernel, "positionConstraintCounts", base.solverImplementation.positionConstraintCountBuffer);
				threadGroupsX = ComputeMath.ThreadGroupCount(particles.count, 128);
				constraintsShader.Dispatch(deltasKernel, threadGroupsX, 1, 1);
			}
		}

		public override void Apply(float substepTime)
		{
			if (m_ConstraintCount > 0)
			{
				Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
				ComputeShader constraintsShader = ((ComputeVolumeConstraints)m_Constraints).constraintsShader;
				int applyKernel = ((ComputeVolumeConstraints)m_Constraints).applyKernel;
				constraintsShader.SetBuffer(applyKernel, "triangles", particleIndices);
				constraintsShader.SetBuffer(applyKernel, "firstTriangle", firstTriangle);
				constraintsShader.SetBuffer(applyKernel, "numTriangles", numTriangles);
				constraintsShader.SetBuffer(applyKernel, "positions", base.solverImplementation.positionsBuffer);
				constraintsShader.SetBuffer(applyKernel, "deltasAsInt", base.solverImplementation.positionDeltasIntBuffer);
				constraintsShader.SetBuffer(applyKernel, "positionConstraintCounts", base.solverImplementation.positionConstraintCountBuffer);
				constraintsShader.SetInt("activeConstraintCount", m_ConstraintCount);
				constraintsShader.SetFloat("sorFactor", constraintParameters.SORFactor);
				int threadGroupsX = ComputeMath.ThreadGroupCount(m_ConstraintCount, 128);
				constraintsShader.Dispatch(applyKernel, threadGroupsX, 1, 1);
			}
		}
	}
}
