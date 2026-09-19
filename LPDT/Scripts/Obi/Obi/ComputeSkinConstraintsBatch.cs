using UnityEngine;

namespace Obi
{
	public class ComputeSkinConstraintsBatch : ComputeConstraintsBatchImpl, ISkinConstraintsBatchImpl, IConstraintsBatchImpl
	{
		private GraphicsBuffer skinPoints;

		private GraphicsBuffer skinNormalsBuffer;

		private GraphicsBuffer skinRadiiBackstopBuffer;

		private GraphicsBuffer skinComplianceBuffer;

		public ComputeSkinConstraintsBatch(ComputeSkinConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Skin;
		}

		public void SetSkinConstraints(ObiNativeIntList particleIndices, ObiNativeVector4List skinPoints, ObiNativeVector4List skinNormals, ObiNativeFloatList skinRadiiBackstop, ObiNativeFloatList skinCompliance, ObiNativeFloatList lambdas, int count)
		{
			base.particleIndices = particleIndices.AsComputeBuffer<int>();
			this.skinPoints = skinPoints.AsComputeBuffer<Vector4>();
			skinNormalsBuffer = skinNormals.AsComputeBuffer<Vector4>();
			skinRadiiBackstopBuffer = skinRadiiBackstop.AsComputeBuffer<float>();
			skinComplianceBuffer = skinCompliance.AsComputeBuffer<float>();
			base.lambdas = lambdas.AsComputeBuffer<float>();
			lambdasList = lambdas;
			m_ConstraintCount = count;
		}

		public override void Evaluate(float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (m_ConstraintCount > 0)
			{
				ComputeShader constraintsShader = ((ComputeSkinConstraints)m_Constraints).constraintsShader;
				int projectKernel = ((ComputeSkinConstraints)m_Constraints).projectKernel;
				constraintsShader.SetBuffer(projectKernel, "particleIndices", particleIndices);
				constraintsShader.SetBuffer(projectKernel, "skinPoints", skinPoints);
				constraintsShader.SetBuffer(projectKernel, "skinNormals", skinNormalsBuffer);
				constraintsShader.SetBuffer(projectKernel, "skinRadiiBackstop", skinRadiiBackstopBuffer);
				constraintsShader.SetBuffer(projectKernel, "skinCompliance", skinComplianceBuffer);
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

		public override void Apply(float substepTime)
		{
			if (m_ConstraintCount > 0)
			{
				Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
				ComputeShader constraintsShader = ((ComputeSkinConstraints)m_Constraints).constraintsShader;
				int applyKernel = ((ComputeSkinConstraints)m_Constraints).applyKernel;
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
