using UnityEngine;

namespace Obi
{
	public class ComputeChainConstraintsBatch : ComputeConstraintsBatchImpl, IChainConstraintsBatchImpl, IConstraintsBatchImpl
	{
		private GraphicsBuffer firstIndex;

		private GraphicsBuffer numIndices;

		private GraphicsBuffer restLengths;

		private GraphicsBuffer ni;

		private GraphicsBuffer diagonals;

		public ComputeChainConstraintsBatch(ComputeChainConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Chain;
		}

		public void SetChainConstraints(ObiNativeIntList particleIndices, ObiNativeVector2List restLengths, ObiNativeIntList firstIndex, ObiNativeIntList numIndex, int count)
		{
			base.particleIndices = particleIndices.AsComputeBuffer<int>();
			this.firstIndex = firstIndex.AsComputeBuffer<int>();
			numIndices = numIndex.AsComputeBuffer<int>();
			this.restLengths = restLengths.AsComputeBuffer<Vector2>();
			int num = 0;
			for (int i = 0; i < numIndex.count; i++)
			{
				num += numIndex[i] - 1;
			}
			ni = new GraphicsBuffer(GraphicsBuffer.Target.Structured, num, 16);
			diagonals = new GraphicsBuffer(GraphicsBuffer.Target.Structured, num, 12);
			m_ConstraintCount = count;
		}

		public override void Destroy()
		{
			ni.Dispose();
			diagonals.Dispose();
		}

		public override void Evaluate(float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (m_ConstraintCount > 0)
			{
				ComputeShader constraintsShader = ((ComputeChainConstraints)m_Constraints).constraintsShader;
				int projectKernel = ((ComputeChainConstraints)m_Constraints).projectKernel;
				constraintsShader.SetBuffer(projectKernel, "particleIndices", particleIndices);
				constraintsShader.SetBuffer(projectKernel, "firstIndex", firstIndex);
				constraintsShader.SetBuffer(projectKernel, "numIndices", numIndices);
				constraintsShader.SetBuffer(projectKernel, "restLengths", restLengths);
				constraintsShader.SetBuffer(projectKernel, "ni", ni);
				constraintsShader.SetBuffer(projectKernel, "diagonals", diagonals);
				constraintsShader.SetBuffer(projectKernel, "positions", base.solverImplementation.positionsBuffer);
				constraintsShader.SetBuffer(projectKernel, "invMasses", base.solverImplementation.invMassesBuffer);
				constraintsShader.SetBuffer(projectKernel, "deltasAsInt", base.solverImplementation.positionDeltasIntBuffer);
				constraintsShader.SetBuffer(projectKernel, "positionConstraintCounts", base.solverImplementation.positionConstraintCountBuffer);
				constraintsShader.SetInt("activeConstraintCount", m_ConstraintCount);
				constraintsShader.SetFloat("deltaTime", substepTime);
				int threadGroupsX = ComputeMath.ThreadGroupCount(m_ConstraintCount, 128);
				constraintsShader.Dispatch(projectKernel, threadGroupsX, 1, 1);
			}
		}

		public override void Apply(float substepTime)
		{
			if (m_ConstraintCount > 0)
			{
				Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
				ComputeShader constraintsShader = ((ComputeChainConstraints)m_Constraints).constraintsShader;
				int applyKernel = ((ComputeChainConstraints)m_Constraints).applyKernel;
				constraintsShader.SetBuffer(applyKernel, "particleIndices", particleIndices);
				constraintsShader.SetBuffer(applyKernel, "firstIndex", firstIndex);
				constraintsShader.SetBuffer(applyKernel, "numIndices", numIndices);
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
