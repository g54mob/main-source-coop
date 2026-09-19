using UnityEngine;

namespace Obi
{
	public class ComputeDensityConstraintsBatch : ComputeConstraintsBatchImpl, IDensityConstraintsBatchImpl, IConstraintsBatchImpl
	{
		public ComputeDensityConstraintsBatch(ComputeDensityConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Density;
		}

		public override void Evaluate(float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (base.solverImplementation.particleGrid.sortedFluidIndices != null && base.solverImplementation.cellCoordsBuffer != null)
			{
				ComputeShader constraintsShader = ((ComputeDensityConstraints)m_Constraints).constraintsShader;
				int updateDensitiesKernel = ((ComputeDensityConstraints)m_Constraints).updateDensitiesKernel;
				((ComputeDensityConstraints)m_Constraints).CopyDataInSortedOrder();
				constraintsShader.SetInt("maxNeighbors", base.solverImplementation.particleGrid.maxParticleNeighbors);
				constraintsShader.SetInt("mode", (int)base.solverImplementation.abstraction.parameters.mode);
				constraintsShader.SetFloat("deltaTime", substepTime);
				constraintsShader.SetVector("diffusionMask", base.solverAbstraction.parameters.diffusionMask);
				constraintsShader.SetBuffer(updateDensitiesKernel, "neighborCounts", base.solverImplementation.particleGrid.neighborCounts);
				constraintsShader.SetBuffer(updateDensitiesKernel, "neighbors", base.solverImplementation.particleGrid.neighbors);
				constraintsShader.SetBuffer(updateDensitiesKernel, "invMasses", base.solverImplementation.invMassesBuffer);
				constraintsShader.SetBuffer(updateDensitiesKernel, "sortedFluidData", base.solverImplementation.particleGrid.sortedFluidData);
				constraintsShader.SetBuffer(updateDensitiesKernel, "sortedPositions", base.solverImplementation.particleGrid.sortedPositions);
				constraintsShader.SetBuffer(updateDensitiesKernel, "sortedPrevPositions", base.solverImplementation.particleGrid.sortedPrevPosOrientations);
				constraintsShader.SetBuffer(updateDensitiesKernel, "sortedFluidMaterials", base.solverImplementation.particleGrid.sortedFluidMaterials);
				constraintsShader.SetBuffer(updateDensitiesKernel, "sortedPrincipalRadii", base.solverImplementation.particleGrid.sortedPrincipalRadii);
				constraintsShader.SetBuffer(updateDensitiesKernel, "renderableOrientations", base.solverImplementation.orientationDeltasIntBuffer);
				constraintsShader.SetBuffer(updateDensitiesKernel, "prevPositions", base.solverImplementation.prevPositionsBuffer);
				constraintsShader.SetBuffer(updateDensitiesKernel, "massCenters", base.solverImplementation.normalsBuffer);
				constraintsShader.SetBuffer(updateDensitiesKernel, "prevMassCenters", base.solverImplementation.renderablePositionsBuffer);
				constraintsShader.SetBuffer(updateDensitiesKernel, "dispatchBuffer", base.solverImplementation.fluidDispatchBuffer);
				constraintsShader.DispatchIndirect(updateDensitiesKernel, base.solverImplementation.fluidDispatchBuffer);
			}
		}

		public override void Apply(float substepTime)
		{
			if (base.solverImplementation.particleGrid.sortedFluidIndices != null && base.solverImplementation.cellCoordsBuffer != null)
			{
				ComputeShader constraintsShader = ((ComputeDensityConstraints)m_Constraints).constraintsShader;
				int applyPositionDeltaKernel = ((ComputeDensityConstraints)m_Constraints).applyPositionDeltaKernel;
				int applyKernel = ((ComputeDensityConstraints)m_Constraints).applyKernel;
				constraintsShader.SetBuffer(applyKernel, "neighborCounts", base.solverImplementation.particleGrid.neighborCounts);
				constraintsShader.SetBuffer(applyKernel, "neighbors", base.solverImplementation.particleGrid.neighbors);
				constraintsShader.SetBuffer(applyKernel, "invMasses", base.solverImplementation.invMassesBuffer);
				constraintsShader.SetBuffer(applyKernel, "sortedPositions", base.solverImplementation.particleGrid.sortedPositions);
				constraintsShader.SetBuffer(applyKernel, "sortedPrevPositions", base.solverImplementation.particleGrid.sortedPrevPosOrientations);
				constraintsShader.SetBuffer(applyKernel, "sortedFluidMaterials", base.solverImplementation.particleGrid.sortedFluidMaterials);
				constraintsShader.SetBuffer(applyKernel, "sortedPrincipalRadii", base.solverImplementation.particleGrid.sortedPrincipalRadii);
				constraintsShader.SetBuffer(applyKernel, "renderableOrientations", base.solverImplementation.orientationDeltasIntBuffer);
				constraintsShader.SetBuffer(applyKernel, "prevPositions", base.solverImplementation.prevPositionsBuffer);
				constraintsShader.SetBuffer(applyKernel, "massCenters", base.solverImplementation.normalsBuffer);
				constraintsShader.SetBuffer(applyKernel, "prevMassCenters", base.solverImplementation.renderablePositionsBuffer);
				constraintsShader.SetBuffer(applyKernel, "sortedFluidData", base.solverImplementation.particleGrid.sortedFluidData);
				constraintsShader.SetBuffer(applyKernel, "deltasAsInt", base.solverImplementation.positionDeltasIntBuffer);
				constraintsShader.SetBuffer(applyKernel, "positionConstraintCounts", base.solverImplementation.positionConstraintCountBuffer);
				constraintsShader.SetBuffer(applyKernel, "sortedToOriginal", base.solverImplementation.particleGrid.sortedFluidIndices);
				constraintsShader.SetBuffer(applyKernel, "dispatchBuffer", base.solverImplementation.fluidDispatchBuffer);
				constraintsShader.DispatchIndirect(applyKernel, base.solverImplementation.fluidDispatchBuffer);
				constraintsShader.SetBuffer(applyPositionDeltaKernel, "positions", base.solverImplementation.positionsBuffer);
				constraintsShader.SetBuffer(applyPositionDeltaKernel, "fluidData", base.solverImplementation.fluidDataBuffer);
				constraintsShader.SetBuffer(applyPositionDeltaKernel, "sortedFluidData", base.solverImplementation.particleGrid.sortedFluidData);
				constraintsShader.SetBuffer(applyPositionDeltaKernel, "renderableOrientations", base.solverImplementation.orientationDeltasIntBuffer);
				constraintsShader.SetBuffer(applyPositionDeltaKernel, "deltasAsInt", base.solverImplementation.positionDeltasIntBuffer);
				constraintsShader.SetBuffer(applyPositionDeltaKernel, "positionConstraintCounts", base.solverImplementation.positionConstraintCountBuffer);
				constraintsShader.SetBuffer(applyPositionDeltaKernel, "sortedToOriginal", base.solverImplementation.particleGrid.sortedFluidIndices);
				constraintsShader.SetBuffer(applyPositionDeltaKernel, "dispatchBuffer", base.solverImplementation.fluidDispatchBuffer);
				constraintsShader.DispatchIndirect(applyPositionDeltaKernel, base.solverImplementation.fluidDispatchBuffer);
			}
		}
	}
}
