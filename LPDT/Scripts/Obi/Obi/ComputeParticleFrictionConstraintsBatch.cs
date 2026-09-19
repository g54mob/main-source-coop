using UnityEngine;

namespace Obi
{
	public class ComputeParticleFrictionConstraintsBatch : ComputeConstraintsBatchImpl, IParticleCollisionConstraintsBatchImpl, IConstraintsBatchImpl
	{
		public ComputeParticleFrictionConstraintsBatch(ComputeParticleFrictionConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.ParticleFriction;
		}

		public override void Evaluate(float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (base.solverImplementation.simplexCounts.simplexCount > 0 && base.solverImplementation.activeParticleCount > 0)
			{
				ComputeShader constraintsShader = ((ComputeParticleFrictionConstraints)m_Constraints).constraintsShader;
				int projectKernel = ((ComputeParticleFrictionConstraints)m_Constraints).projectKernel;
				constraintsShader.SetInt("pointCount", base.solverAbstraction.simplexCounts.pointCount);
				constraintsShader.SetInt("edgeCount", base.solverAbstraction.simplexCounts.edgeCount);
				constraintsShader.SetInt("triangleCount", base.solverAbstraction.simplexCounts.triangleCount);
				constraintsShader.SetBuffer(projectKernel, "particleContacts", base.solverAbstraction.particleContacts.computeBuffer);
				constraintsShader.SetBuffer(projectKernel, "effectiveMasses", base.solverAbstraction.particleContactEffectiveMasses.computeBuffer);
				constraintsShader.SetBuffer(projectKernel, "dispatchBuffer", base.solverImplementation.particleGrid.dispatchBuffer);
				constraintsShader.SetBuffer(projectKernel, "collisionMaterials", base.solverImplementation.colliderGrid.materialsBuffer);
				constraintsShader.SetBuffer(projectKernel, "simplices", base.solverImplementation.simplices);
				constraintsShader.SetBuffer(projectKernel, "collisionMaterialIndices", base.solverImplementation.collisionMaterialIndexBuffer);
				constraintsShader.SetBuffer(projectKernel, "positions", base.solverImplementation.positionsBuffer);
				constraintsShader.SetBuffer(projectKernel, "orientations", base.solverImplementation.orientationsBuffer);
				constraintsShader.SetBuffer(projectKernel, "prevPositions", base.solverImplementation.prevPositionsBuffer);
				constraintsShader.SetBuffer(projectKernel, "prevOrientations", base.solverImplementation.prevOrientationsBuffer);
				constraintsShader.SetBuffer(projectKernel, "principalRadii", base.solverImplementation.principalRadiiBuffer);
				constraintsShader.SetBuffer(projectKernel, "invMasses", base.solverImplementation.invMassesBuffer);
				constraintsShader.SetBuffer(projectKernel, "invRotationalMasses", base.solverImplementation.invRotationalMassesBuffer);
				constraintsShader.SetBuffer(projectKernel, "positionConstraintCounts", base.solverImplementation.positionConstraintCountBuffer);
				constraintsShader.SetBuffer(projectKernel, "deltasAsInt", base.solverImplementation.positionDeltasIntBuffer);
				constraintsShader.SetBuffer(projectKernel, "orientationConstraintCounts", base.solverImplementation.orientationConstraintCountBuffer);
				constraintsShader.SetBuffer(projectKernel, "orientationDeltasAsInt", base.solverImplementation.orientationDeltasIntBuffer);
				constraintsShader.SetFloat("stepTime", stepTime);
				constraintsShader.SetFloat("substepTime", substepTime);
				constraintsShader.DispatchIndirect(projectKernel, base.solverImplementation.particleGrid.dispatchBuffer);
			}
		}

		public override void Apply(float substepTime)
		{
			ComputeShader constraintsShader = ((ComputeParticleFrictionConstraints)m_Constraints).constraintsShader;
			int applyKernel = ((ComputeParticleFrictionConstraints)m_Constraints).applyKernel;
			if (base.solverImplementation.activeParticleCount > 0)
			{
				Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
				constraintsShader.SetBuffer(applyKernel, "particleIndices", base.solverImplementation.activeParticlesBuffer);
				constraintsShader.SetBuffer(applyKernel, "positions", base.solverImplementation.positionsBuffer);
				constraintsShader.SetBuffer(applyKernel, "orientations", base.solverImplementation.orientationsBuffer);
				constraintsShader.SetBuffer(applyKernel, "positionConstraintCounts", base.solverImplementation.positionConstraintCountBuffer);
				constraintsShader.SetBuffer(applyKernel, "deltasAsInt", base.solverImplementation.positionDeltasIntBuffer);
				constraintsShader.SetBuffer(applyKernel, "orientationConstraintCounts", base.solverImplementation.orientationConstraintCountBuffer);
				constraintsShader.SetBuffer(applyKernel, "orientationDeltasAsInt", base.solverImplementation.orientationDeltasIntBuffer);
				constraintsShader.SetInt("particleCount", base.solverAbstraction.activeParticleCount);
				constraintsShader.SetFloat("sorFactor", constraintParameters.SORFactor);
				int threadGroupsX = ComputeMath.ThreadGroupCount(base.solverAbstraction.activeParticleCount, 128);
				constraintsShader.Dispatch(applyKernel, threadGroupsX, 1, 1);
			}
		}
	}
}
