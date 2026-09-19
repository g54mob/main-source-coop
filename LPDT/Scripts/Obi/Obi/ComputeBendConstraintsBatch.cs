using UnityEngine;

namespace Obi
{
	public class ComputeBendConstraintsBatch : ComputeConstraintsBatchImpl, IBendConstraintsBatchImpl, IConstraintsBatchImpl
	{
		private GraphicsBuffer restBends;

		private GraphicsBuffer stiffnesses;

		public ComputeBendConstraintsBatch(ComputeBendConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Bending;
		}

		public void SetBendConstraints(ObiNativeIntList particleIndices, ObiNativeFloatList restBends, ObiNativeVector2List bendingStiffnesses, ObiNativeVector2List plasticity, ObiNativeFloatList lambdas, int count)
		{
			base.particleIndices = particleIndices.AsComputeBuffer<int>();
			this.restBends = restBends.AsComputeBuffer<float>();
			stiffnesses = bendingStiffnesses.AsComputeBuffer<Vector2>();
			base.lambdas = lambdas.AsComputeBuffer<float>();
			lambdasList = lambdas;
			m_ConstraintCount = count;
		}

		public override void Evaluate(float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (m_ConstraintCount > 0)
			{
				ComputeShader constraintsShader = ((ComputeBendConstraints)m_Constraints).constraintsShader;
				int projectKernel = ((ComputeBendConstraints)m_Constraints).projectKernel;
				constraintsShader.SetBuffer(projectKernel, "particleIndices", particleIndices);
				constraintsShader.SetBuffer(projectKernel, "restBends", restBends);
				constraintsShader.SetBuffer(projectKernel, "stiffnesses", stiffnesses);
				constraintsShader.SetBuffer(projectKernel, "lambdas", lambdas);
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

		public override void Apply(float deltaTime)
		{
			if (m_ConstraintCount > 0)
			{
				Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
				ComputeShader constraintsShader = ((ComputeBendConstraints)m_Constraints).constraintsShader;
				int applyKernel = ((ComputeBendConstraints)m_Constraints).applyKernel;
				constraintsShader.SetBuffer(applyKernel, "particleIndices", particleIndices);
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
