using UnityEngine;

namespace Obi
{
	public class ComputePinholeConstraintsBatch : ComputeConstraintsBatchImpl, IPinholeConstraintsBatchImpl, IConstraintsBatchImpl
	{
		private GraphicsBuffer colliderIndices;

		private GraphicsBuffer offsets;

		private GraphicsBuffer edgeMus;

		private GraphicsBuffer edgeRanges;

		private GraphicsBuffer edgeRangeMus;

		private GraphicsBuffer relativeVelocities;

		private GraphicsBuffer parameters;

		public ComputePinholeConstraintsBatch(ComputePinholeConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Pinhole;
		}

		public void SetPinholeConstraints(ObiNativeIntList particleIndices, ObiNativeIntList colliderIndices, ObiNativeVector4List offsets, ObiNativeFloatList edgeMus, ObiNativeIntList edgeRanges, ObiNativeFloatList edgeRangeMus, ObiNativeFloatList parameters, ObiNativeFloatList relativeVelocities, ObiNativeFloatList lambdas, int count)
		{
			base.particleIndices = particleIndices.AsComputeBuffer<int>();
			this.colliderIndices = colliderIndices.AsComputeBuffer<int>();
			this.offsets = offsets.AsComputeBuffer<Vector4>();
			this.edgeMus = edgeMus.AsComputeBuffer<float>();
			this.edgeRanges = edgeRanges.AsComputeBuffer<Vector2Int>();
			this.edgeRangeMus = edgeRangeMus.AsComputeBuffer<Vector2>();
			this.parameters = parameters.AsComputeBuffer<float>();
			base.lambdas = lambdas.AsComputeBuffer<float>();
			this.relativeVelocities = relativeVelocities.AsComputeBuffer<float>();
			lambdasList = lambdas;
			m_ConstraintCount = count;
		}

		public override void Initialize(float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (m_ConstraintCount > 0)
			{
				ComputeShader constraintsShader = ((ComputePinholeConstraints)m_Constraints).constraintsShader;
				int clearKernel = ((ComputePinholeConstraints)m_Constraints).clearKernel;
				int initializeKernel = ((ComputePinholeConstraints)m_Constraints).initializeKernel;
				constraintsShader.SetBuffer(clearKernel, "colliderIndices", colliderIndices);
				constraintsShader.SetBuffer(clearKernel, "shapes", base.solverImplementation.colliderGrid.shapesBuffer);
				constraintsShader.SetBuffer(clearKernel, "RW_rigidbodies", base.solverImplementation.colliderGrid.rigidbodiesBuffer);
				constraintsShader.SetBuffer(initializeKernel, "particleIndices", particleIndices);
				constraintsShader.SetBuffer(initializeKernel, "colliderIndices", colliderIndices);
				constraintsShader.SetBuffer(initializeKernel, "offsets", offsets);
				constraintsShader.SetBuffer(initializeKernel, "edgeMus", edgeMus);
				constraintsShader.SetBuffer(initializeKernel, "edgeRanges", edgeRanges);
				constraintsShader.SetBuffer(initializeKernel, "edgeRangeMus", edgeRangeMus);
				constraintsShader.SetBuffer(initializeKernel, "relativeVelocities", relativeVelocities);
				constraintsShader.SetBuffer(initializeKernel, "parameters", parameters);
				constraintsShader.SetBuffer(initializeKernel, "deformableEdges", base.solverImplementation.deformableEdgesBuffer);
				constraintsShader.SetBuffer(initializeKernel, "positions", base.solverImplementation.positionsBuffer);
				constraintsShader.SetBuffer(initializeKernel, "prevPositions", base.solverImplementation.prevPositionsBuffer);
				constraintsShader.SetBuffer(initializeKernel, "invMasses", base.solverImplementation.invMassesBuffer);
				constraintsShader.SetBuffer(initializeKernel, "colliderIndices", colliderIndices);
				constraintsShader.SetBuffer(initializeKernel, "transforms", base.solverImplementation.colliderGrid.transformsBuffer);
				constraintsShader.SetBuffer(initializeKernel, "shapes", base.solverImplementation.colliderGrid.shapesBuffer);
				constraintsShader.SetBuffer(initializeKernel, "rigidbodies", base.solverImplementation.colliderGrid.rigidbodiesBuffer);
				constraintsShader.SetBuffer(initializeKernel, "RW_rigidbodies", base.solverImplementation.colliderGrid.rigidbodiesBuffer);
				constraintsShader.SetBuffer(initializeKernel, "linearDeltasAsInt", base.solverImplementation.rigidbodyLinearDeltasIntBuffer);
				constraintsShader.SetBuffer(initializeKernel, "angularDeltasAsInt", base.solverImplementation.rigidbodyAngularDeltasIntBuffer);
				constraintsShader.SetBuffer(initializeKernel, "inertialSolverFrame", base.solverImplementation.inertialFrameBuffer);
				constraintsShader.SetInt("activeConstraintCount", m_ConstraintCount);
				constraintsShader.SetFloat("stepTime", stepTime);
				constraintsShader.SetFloat("substepTime", substepTime);
				constraintsShader.SetInt("steps", steps);
				constraintsShader.SetFloat("timeLeft", timeLeft);
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
				ComputeShader constraintsShader = ((ComputePinholeConstraints)m_Constraints).constraintsShader;
				int projectKernel = ((ComputePinholeConstraints)m_Constraints).projectKernel;
				constraintsShader.SetBuffer(projectKernel, "particleIndices", particleIndices);
				constraintsShader.SetBuffer(projectKernel, "colliderIndices", colliderIndices);
				constraintsShader.SetBuffer(projectKernel, "offsets", offsets);
				constraintsShader.SetBuffer(projectKernel, "edgeMus", edgeMus);
				constraintsShader.SetBuffer(projectKernel, "parameters", parameters);
				constraintsShader.SetBuffer(projectKernel, "lambdas", lambdas);
				constraintsShader.SetBuffer(projectKernel, "transforms", base.solverImplementation.colliderGrid.transformsBuffer);
				constraintsShader.SetBuffer(projectKernel, "shapes", base.solverImplementation.colliderGrid.shapesBuffer);
				constraintsShader.SetBuffer(projectKernel, "rigidbodies", base.solverImplementation.colliderGrid.rigidbodiesBuffer);
				constraintsShader.SetBuffer(projectKernel, "deformableEdges", base.solverImplementation.deformableEdgesBuffer);
				constraintsShader.SetBuffer(projectKernel, "positions", base.solverImplementation.positionsBuffer);
				constraintsShader.SetBuffer(projectKernel, "prevPositions", base.solverImplementation.prevPositionsBuffer);
				constraintsShader.SetBuffer(projectKernel, "invMasses", base.solverImplementation.invMassesBuffer);
				constraintsShader.SetBuffer(projectKernel, "deltasAsInt", base.solverImplementation.positionDeltasIntBuffer);
				constraintsShader.SetBuffer(projectKernel, "positionConstraintCounts", base.solverImplementation.positionConstraintCountBuffer);
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
				ComputeShader constraintsShader = ((ComputePinholeConstraints)m_Constraints).constraintsShader;
				int applyKernel = ((ComputePinholeConstraints)m_Constraints).applyKernel;
				constraintsShader.SetBuffer(applyKernel, "particleIndices", particleIndices);
				constraintsShader.SetBuffer(applyKernel, "deformableEdges", base.solverImplementation.deformableEdgesBuffer);
				constraintsShader.SetBuffer(applyKernel, "RW_positions", base.solverImplementation.positionsBuffer);
				constraintsShader.SetBuffer(applyKernel, "deltasAsInt", base.solverImplementation.positionDeltasIntBuffer);
				constraintsShader.SetBuffer(applyKernel, "positionConstraintCounts", base.solverImplementation.positionConstraintCountBuffer);
				constraintsShader.SetInt("activeConstraintCount", m_ConstraintCount);
				constraintsShader.SetFloat("sorFactor", constraintParameters.SORFactor);
				int threadGroupsX = ComputeMath.ThreadGroupCount(m_ConstraintCount, 128);
				constraintsShader.Dispatch(applyKernel, threadGroupsX, 1, 1);
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
