using UnityEngine;

namespace Obi
{
	public class ComputePinConstraintsBatch : ComputeConstraintsBatchImpl, IPinConstraintsBatchImpl, IConstraintsBatchImpl
	{
		private GraphicsBuffer colliderIndices;

		private GraphicsBuffer offsets;

		private GraphicsBuffer restDarboux;

		private GraphicsBuffer stiffnesses;

		public ComputePinConstraintsBatch(ComputePinConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Pin;
		}

		public void SetPinConstraints(ObiNativeIntList particleIndices, ObiNativeIntList colliderIndices, ObiNativeVector4List offsets, ObiNativeQuaternionList restDarbouxVectors, ObiNativeFloatList stiffnesses, ObiNativeFloatList lambdas, int count)
		{
			base.particleIndices = particleIndices.AsComputeBuffer<int>();
			this.colliderIndices = colliderIndices.AsComputeBuffer<int>();
			this.offsets = offsets.AsComputeBuffer<Vector4>();
			restDarboux = restDarbouxVectors.AsComputeBuffer<Quaternion>();
			this.stiffnesses = stiffnesses.AsComputeBuffer<Vector2>();
			base.lambdas = lambdas.AsComputeBuffer<Vector4>();
			lambdasList = lambdas;
			m_ConstraintCount = count;
		}

		public override void Initialize(float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (m_ConstraintCount > 0)
			{
				ComputeShader constraintsShader = ((ComputePinConstraints)m_Constraints).constraintsShader;
				int clearKernel = ((ComputePinConstraints)m_Constraints).clearKernel;
				int initializeKernel = ((ComputePinConstraints)m_Constraints).initializeKernel;
				constraintsShader.SetBuffer(clearKernel, "colliderIndices", colliderIndices);
				constraintsShader.SetBuffer(clearKernel, "shapes", base.solverImplementation.colliderGrid.shapesBuffer);
				constraintsShader.SetBuffer(clearKernel, "RW_rigidbodies", base.solverImplementation.colliderGrid.rigidbodiesBuffer);
				constraintsShader.SetBuffer(initializeKernel, "colliderIndices", colliderIndices);
				constraintsShader.SetBuffer(initializeKernel, "shapes", base.solverImplementation.colliderGrid.shapesBuffer);
				constraintsShader.SetBuffer(initializeKernel, "RW_rigidbodies", base.solverImplementation.colliderGrid.rigidbodiesBuffer);
				constraintsShader.SetInt("activeConstraintCount", m_ConstraintCount);
				int threadGroupsX = ComputeMath.ThreadGroupCount(m_ConstraintCount, 128);
				constraintsShader.Dispatch(clearKernel, threadGroupsX, 1, 1);
				constraintsShader.Dispatch(initializeKernel, threadGroupsX, 1, 1);
			}
			base.Initialize(stepTime, substepTime, steps, timeLeft);
		}

		public override void Evaluate(float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (m_ConstraintCount > 0)
			{
				ComputeShader constraintsShader = ((ComputePinConstraints)m_Constraints).constraintsShader;
				int projectKernel = ((ComputePinConstraints)m_Constraints).projectKernel;
				constraintsShader.SetBuffer(projectKernel, "particleIndices", particleIndices);
				constraintsShader.SetBuffer(projectKernel, "colliderIndices", colliderIndices);
				constraintsShader.SetBuffer(projectKernel, "offsets", offsets);
				constraintsShader.SetBuffer(projectKernel, "restDarboux", restDarboux);
				constraintsShader.SetBuffer(projectKernel, "stiffnesses", stiffnesses);
				constraintsShader.SetBuffer(projectKernel, "lambdas", lambdas);
				constraintsShader.SetBuffer(projectKernel, "transforms", base.solverImplementation.colliderGrid.transformsBuffer);
				constraintsShader.SetBuffer(projectKernel, "shapes", base.solverImplementation.colliderGrid.shapesBuffer);
				constraintsShader.SetBuffer(projectKernel, "rigidbodies", base.solverImplementation.colliderGrid.rigidbodiesBuffer);
				constraintsShader.SetBuffer(projectKernel, "positions", base.solverImplementation.positionsBuffer);
				constraintsShader.SetBuffer(projectKernel, "prevPositions", base.solverImplementation.prevPositionsBuffer);
				constraintsShader.SetBuffer(projectKernel, "orientations", base.solverImplementation.orientationsBuffer);
				constraintsShader.SetBuffer(projectKernel, "invMasses", base.solverImplementation.invMassesBuffer);
				constraintsShader.SetBuffer(projectKernel, "invRotationalMasses", base.solverImplementation.invRotationalMassesBuffer);
				constraintsShader.SetBuffer(projectKernel, "deltasAsInt", base.solverImplementation.positionDeltasIntBuffer);
				constraintsShader.SetBuffer(projectKernel, "positionConstraintCounts", base.solverImplementation.positionConstraintCountBuffer);
				constraintsShader.SetBuffer(projectKernel, "orientationDeltasAsInt", base.solverImplementation.orientationDeltasIntBuffer);
				constraintsShader.SetBuffer(projectKernel, "orientationConstraintCounts", base.solverImplementation.orientationConstraintCountBuffer);
				constraintsShader.SetBuffer(projectKernel, "linearDeltasAsInt", base.solverImplementation.rigidbodyLinearDeltasIntBuffer);
				constraintsShader.SetBuffer(projectKernel, "angularDeltasAsInt", base.solverImplementation.rigidbodyAngularDeltasIntBuffer);
				constraintsShader.SetBuffer(projectKernel, "inertialSolverFrame", base.solverImplementation.inertialFrameBuffer);
				constraintsShader.SetInt("activeConstraintCount", m_ConstraintCount);
				constraintsShader.SetFloat("stepTime", stepTime);
				constraintsShader.SetFloat("substepTime", substepTime);
				constraintsShader.SetInt("steps", steps);
				constraintsShader.SetFloat("timeLeft", timeLeft);
				int threadGroupsX = ComputeMath.ThreadGroupCount(m_ConstraintCount, 128);
				constraintsShader.Dispatch(projectKernel, threadGroupsX, 1, 1);
			}
		}

		public override void Apply(float substepTime)
		{
			if (m_ConstraintCount > 0)
			{
				Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
				ComputeShader constraintsShader = ((ComputePinConstraints)m_Constraints).constraintsShader;
				int applyKernel = ((ComputePinConstraints)m_Constraints).applyKernel;
				constraintsShader.SetBuffer(applyKernel, "particleIndices", particleIndices);
				constraintsShader.SetBuffer(applyKernel, "RW_positions", base.solverImplementation.positionsBuffer);
				constraintsShader.SetBuffer(applyKernel, "deltasAsInt", base.solverImplementation.positionDeltasIntBuffer);
				constraintsShader.SetBuffer(applyKernel, "positionConstraintCounts", base.solverImplementation.positionConstraintCountBuffer);
				constraintsShader.SetBuffer(applyKernel, "RW_orientations", base.solverImplementation.orientationsBuffer);
				constraintsShader.SetBuffer(applyKernel, "orientationDeltasAsInt", base.solverImplementation.orientationDeltasIntBuffer);
				constraintsShader.SetBuffer(applyKernel, "orientationConstraintCounts", base.solverImplementation.orientationConstraintCountBuffer);
				constraintsShader.SetInt("activeConstraintCount", m_ConstraintCount);
				constraintsShader.SetFloat("sorFactor", constraintParameters.SORFactor);
				int threadGroupsX = ComputeMath.ThreadGroupCount(m_ConstraintCount, 128);
				constraintsShader.Dispatch(applyKernel, threadGroupsX, 1, 1);
			}
		}

		public void ProjectRenderablePositions()
		{
			if (m_ConstraintCount > 0)
			{
				ComputeShader constraintsShader = ((ComputePinConstraints)m_Constraints).constraintsShader;
				int projectRenderableKernel = ((ComputePinConstraints)m_Constraints).projectRenderableKernel;
				constraintsShader.SetBuffer(projectRenderableKernel, "particleIndices", particleIndices);
				constraintsShader.SetBuffer(projectRenderableKernel, "colliderIndices", colliderIndices);
				constraintsShader.SetBuffer(projectRenderableKernel, "offsets", offsets);
				constraintsShader.SetBuffer(projectRenderableKernel, "restDarboux", restDarboux);
				constraintsShader.SetBuffer(projectRenderableKernel, "stiffnesses", stiffnesses);
				constraintsShader.SetBuffer(projectRenderableKernel, "transforms", base.solverImplementation.colliderGrid.transformsBuffer);
				constraintsShader.SetBuffer(projectRenderableKernel, "RW_positions", base.solverImplementation.renderablePositionsBuffer);
				constraintsShader.SetBuffer(projectRenderableKernel, "RW_orientations", base.solverImplementation.renderableOrientationsBuffer);
				constraintsShader.SetBuffer(projectRenderableKernel, "inertialSolverFrame", base.solverImplementation.inertialFrameBuffer);
				constraintsShader.SetInt("activeConstraintCount", m_ConstraintCount);
				int threadGroupsX = ComputeMath.ThreadGroupCount(m_ConstraintCount, 128);
				constraintsShader.Dispatch(projectRenderableKernel, threadGroupsX, 1, 1);
			}
		}

		public void RequestDataReadback()
		{
			lambdasList.Readback();
		}

		public void WaitForReadback()
		{
			lambdasList.WaitForReadback();
		}
	}
}
