using UnityEngine;

namespace Obi
{
	public class ComputeAerodynamicConstraintsBatch : ComputeConstraintsBatchImpl, IAerodynamicConstraintsBatchImpl, IConstraintsBatchImpl
	{
		private GraphicsBuffer aerodynamicCoeffs;

		public ComputeAerodynamicConstraintsBatch(ComputeAerodynamicConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Aerodynamics;
		}

		public void SetAerodynamicConstraints(ObiNativeIntList particleIndices, ObiNativeFloatList aerodynamicCoeffs, int count)
		{
			base.particleIndices = particleIndices.AsComputeBuffer<int>();
			this.aerodynamicCoeffs = aerodynamicCoeffs.AsComputeBuffer<float>();
			m_ConstraintCount = count;
		}

		public override void Evaluate(float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (m_ConstraintCount > 0)
			{
				ComputeShader constraintsShader = ((ComputeAerodynamicConstraints)m_Constraints).constraintsShader;
				int projectKernel = ((ComputeAerodynamicConstraints)m_Constraints).projectKernel;
				constraintsShader.SetBuffer(projectKernel, "particleIndices", particleIndices);
				constraintsShader.SetBuffer(projectKernel, "aerodynamicCoeffs", aerodynamicCoeffs);
				constraintsShader.SetBuffer(projectKernel, "positions", base.solverImplementation.positionsBuffer);
				constraintsShader.SetBuffer(projectKernel, "normals", base.solverImplementation.normalsBuffer);
				constraintsShader.SetBuffer(projectKernel, "wind", base.solverImplementation.windBuffer);
				constraintsShader.SetBuffer(projectKernel, "invMasses", base.solverImplementation.invMassesBuffer);
				constraintsShader.SetBuffer(projectKernel, "velocities", base.solverImplementation.velocitiesBuffer);
				constraintsShader.SetInt("activeConstraintCount", m_ConstraintCount);
				constraintsShader.SetFloat("deltaTime", substepTime);
				int threadGroupsX = ComputeMath.ThreadGroupCount(m_ConstraintCount, 128);
				constraintsShader.Dispatch(projectKernel, threadGroupsX, 1, 1);
			}
		}

		public override void Apply(float substepTime)
		{
		}
	}
}
