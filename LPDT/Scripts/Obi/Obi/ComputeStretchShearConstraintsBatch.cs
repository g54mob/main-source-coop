using UnityEngine;

namespace Obi
{
	public class ComputeStretchShearConstraintsBatch : ComputeConstraintsBatchImpl, IStretchShearConstraintsBatchImpl, IConstraintsBatchImpl
	{
		private GraphicsBuffer orientationIndices;

		private GraphicsBuffer restLengths;

		private GraphicsBuffer restOrientations;

		private GraphicsBuffer stiffnesses;

		public ComputeStretchShearConstraintsBatch(ComputeStretchShearConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.StretchShear;
		}

		public void SetStretchShearConstraints(ObiNativeIntList particleIndices, ObiNativeIntList orientationIndices, ObiNativeFloatList restLengths, ObiNativeQuaternionList restOrientations, ObiNativeVector3List stiffnesses, ObiNativeFloatList lambdas, int count)
		{
			base.particleIndices = particleIndices.AsComputeBuffer<int>();
			this.orientationIndices = orientationIndices.AsComputeBuffer<int>();
			this.restLengths = restLengths.AsComputeBuffer<float>();
			this.restOrientations = restOrientations.AsComputeBuffer<Quaternion>();
			this.stiffnesses = stiffnesses.AsComputeBuffer<Vector3>();
			base.lambdas = lambdas.AsComputeBuffer<float>();
			lambdasList = lambdas;
			m_ConstraintCount = count;
		}

		public override void Evaluate(float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (m_ConstraintCount > 0)
			{
				ComputeShader constraintsShader = ((ComputeStretchShearConstraints)m_Constraints).constraintsShader;
				int projectKernel = ((ComputeStretchShearConstraints)m_Constraints).projectKernel;
				constraintsShader.SetBuffer(projectKernel, "particleIndices", particleIndices);
				constraintsShader.SetBuffer(projectKernel, "orientationIndices", orientationIndices);
				constraintsShader.SetBuffer(projectKernel, "restLengths", restLengths);
				constraintsShader.SetBuffer(projectKernel, "restOrientations", restOrientations);
				constraintsShader.SetBuffer(projectKernel, "stiffnesses", stiffnesses);
				constraintsShader.SetBuffer(projectKernel, "lambdas", lambdas);
				constraintsShader.SetBuffer(projectKernel, "positions", base.solverImplementation.positionsBuffer);
				constraintsShader.SetBuffer(projectKernel, "orientations", base.solverImplementation.orientationsBuffer);
				constraintsShader.SetBuffer(projectKernel, "invMasses", base.solverImplementation.invMassesBuffer);
				constraintsShader.SetBuffer(projectKernel, "invRotationalMasses", base.solverImplementation.invRotationalMassesBuffer);
				constraintsShader.SetBuffer(projectKernel, "deltasAsInt", base.solverImplementation.positionDeltasIntBuffer);
				constraintsShader.SetBuffer(projectKernel, "orientationDeltasAsInt", base.solverImplementation.orientationDeltasIntBuffer);
				constraintsShader.SetBuffer(projectKernel, "positionConstraintCounts", base.solverImplementation.positionConstraintCountBuffer);
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
				ComputeShader constraintsShader = ((ComputeStretchShearConstraints)m_Constraints).constraintsShader;
				int applyKernel = ((ComputeStretchShearConstraints)m_Constraints).applyKernel;
				constraintsShader.SetBuffer(applyKernel, "particleIndices", particleIndices);
				constraintsShader.SetBuffer(applyKernel, "orientationIndices", orientationIndices);
				constraintsShader.SetBuffer(applyKernel, "positions", base.solverImplementation.positionsBuffer);
				constraintsShader.SetBuffer(applyKernel, "orientations", base.solverImplementation.orientationsBuffer);
				constraintsShader.SetBuffer(applyKernel, "deltasAsInt", base.solverImplementation.positionDeltasIntBuffer);
				constraintsShader.SetBuffer(applyKernel, "orientationDeltasAsInt", base.solverImplementation.orientationDeltasIntBuffer);
				constraintsShader.SetBuffer(applyKernel, "positionConstraintCounts", base.solverImplementation.positionConstraintCountBuffer);
				constraintsShader.SetBuffer(applyKernel, "orientationConstraintCounts", base.solverImplementation.orientationConstraintCountBuffer);
				constraintsShader.SetInt("activeConstraintCount", m_ConstraintCount);
				constraintsShader.SetFloat("sorFactor", constraintParameters.SORFactor);
				int threadGroupsX = ComputeMath.ThreadGroupCount(m_ConstraintCount, 128);
				constraintsShader.Dispatch(applyKernel, threadGroupsX, 1, 1);
			}
		}
	}
}
