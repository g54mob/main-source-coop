using UnityEngine;

namespace Obi
{
	public class ComputeBendTwistConstraintsBatch : ComputeConstraintsBatchImpl, IBendTwistConstraintsBatchImpl, IConstraintsBatchImpl
	{
		private GraphicsBuffer orientationIndices;

		private GraphicsBuffer restDarboux;

		private GraphicsBuffer stiffnesses;

		private GraphicsBuffer plasticity;

		public ComputeBendTwistConstraintsBatch(ComputeBendTwistConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.BendTwist;
		}

		public void SetBendTwistConstraints(ObiNativeIntList orientationIndices, ObiNativeQuaternionList restDarboux, ObiNativeVector3List stiffnesses, ObiNativeVector2List plasticity, ObiNativeFloatList lambdas, int count)
		{
			this.orientationIndices = orientationIndices.AsComputeBuffer<int>();
			this.restDarboux = restDarboux.AsComputeBuffer<Quaternion>();
			this.stiffnesses = stiffnesses.AsComputeBuffer<Vector3>();
			this.plasticity = plasticity.AsComputeBuffer<Vector2>();
			base.lambdas = lambdas.AsComputeBuffer<float>();
			lambdasList = lambdas;
			m_ConstraintCount = count;
		}

		public override void Evaluate(float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (m_ConstraintCount > 0)
			{
				ComputeShader constraintsShader = ((ComputeBendTwistConstraints)m_Constraints).constraintsShader;
				int projectKernel = ((ComputeBendTwistConstraints)m_Constraints).projectKernel;
				constraintsShader.SetBuffer(projectKernel, "orientationIndices", orientationIndices);
				constraintsShader.SetBuffer(projectKernel, "restDarboux", restDarboux);
				constraintsShader.SetBuffer(projectKernel, "stiffnesses", stiffnesses);
				constraintsShader.SetBuffer(projectKernel, "plasticity", plasticity);
				constraintsShader.SetBuffer(projectKernel, "lambdas", lambdas);
				constraintsShader.SetBuffer(projectKernel, "orientations", base.solverImplementation.orientationsBuffer);
				constraintsShader.SetBuffer(projectKernel, "invRotationalMasses", base.solverImplementation.invRotationalMassesBuffer);
				constraintsShader.SetBuffer(projectKernel, "orientationDeltasAsInt", base.solverImplementation.orientationDeltasIntBuffer);
				constraintsShader.SetBuffer(projectKernel, "orientationConstraintCounts", base.solverImplementation.orientationConstraintCountBuffer);
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
				ComputeShader constraintsShader = ((ComputeBendTwistConstraints)m_Constraints).constraintsShader;
				int applyKernel = ((ComputeBendTwistConstraints)m_Constraints).applyKernel;
				constraintsShader.SetBuffer(applyKernel, "orientationIndices", orientationIndices);
				constraintsShader.SetBuffer(applyKernel, "orientations", base.solverImplementation.orientationsBuffer);
				constraintsShader.SetBuffer(applyKernel, "orientationDeltasAsInt", base.solverImplementation.orientationDeltasIntBuffer);
				constraintsShader.SetBuffer(applyKernel, "orientationConstraintCounts", base.solverImplementation.orientationConstraintCountBuffer);
				constraintsShader.SetInt("activeConstraintCount", m_ConstraintCount);
				constraintsShader.SetFloat("sorFactor", constraintParameters.SORFactor);
				int threadGroupsX = ComputeMath.ThreadGroupCount(m_ConstraintCount, 128);
				constraintsShader.Dispatch(applyKernel, threadGroupsX, 1, 1);
			}
		}
	}
}
