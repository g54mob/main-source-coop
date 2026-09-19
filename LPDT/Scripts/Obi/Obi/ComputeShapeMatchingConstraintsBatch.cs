using UnityEngine;

namespace Obi
{
	public class ComputeShapeMatchingConstraintsBatch : ComputeConstraintsBatchImpl, IShapeMatchingConstraintsBatchImpl, IConstraintsBatchImpl
	{
		private GraphicsBuffer firstIndexBuffer;

		private GraphicsBuffer numIndicesBuffer;

		private GraphicsBuffer explicitGroupBuffer;

		private GraphicsBuffer shapeMaterialParametersBuffer;

		private GraphicsBuffer restComsBuffer;

		private GraphicsBuffer comsBuffer;

		private GraphicsBuffer constraintOrientationsBuffer;

		private GraphicsBuffer AqqBuffer;

		private GraphicsBuffer linearTransformsBuffer;

		private GraphicsBuffer plasticDeformationsBuffer;

		private ObiNativeVector4List m_RestComs;

		private ObiNativeVector4List m_Coms;

		private ObiNativeQuaternionList m_ConstraintOrientations;

		private bool m_RecalculateRestShape;

		public ComputeShapeMatchingConstraintsBatch(ComputeShapeMatchingConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.ShapeMatching;
		}

		public void SetShapeMatchingConstraints(ObiNativeIntList particleIndices, ObiNativeIntList firstIndex, ObiNativeIntList numIndices, ObiNativeIntList explicitGroup, ObiNativeFloatList shapeMaterialParameters, ObiNativeVector4List restComs, ObiNativeVector4List coms, ObiNativeQuaternionList constraintOrientations, ObiNativeMatrix4x4List linearTransforms, ObiNativeMatrix4x4List plasticDeformations, ObiNativeFloatList lambdas, int count)
		{
			base.particleIndices = particleIndices.AsComputeBuffer<int>();
			firstIndexBuffer = firstIndex.AsComputeBuffer<int>();
			numIndicesBuffer = numIndices.AsComputeBuffer<int>();
			explicitGroupBuffer = explicitGroup.AsComputeBuffer<int>();
			shapeMaterialParametersBuffer = shapeMaterialParameters.AsComputeBuffer<float>();
			restComsBuffer = restComs.AsComputeBuffer<Vector4>();
			comsBuffer = coms.AsComputeBuffer<Vector4>();
			constraintOrientationsBuffer = constraintOrientations.AsComputeBuffer<Quaternion>();
			linearTransformsBuffer = linearTransforms.AsComputeBuffer<Matrix4x4>();
			plasticDeformationsBuffer = plasticDeformations.AsComputeBuffer<Matrix4x4>();
			if (AqqBuffer != null)
			{
				AqqBuffer.Dispose();
			}
			AqqBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, count, 64);
			m_RestComs = restComs;
			m_Coms = coms;
			m_ConstraintOrientations = constraintOrientations;
			m_ConstraintCount = count;
		}

		public override void Destroy()
		{
			if (AqqBuffer != null)
			{
				AqqBuffer.Dispose();
			}
		}

		public override void Evaluate(float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (m_ConstraintCount > 0)
			{
				ComputeShader constraintsShader = ((ComputeShapeMatchingConstraints)m_Constraints).constraintsShader;
				int threadGroupsX = ComputeMath.ThreadGroupCount(m_ConstraintCount, 128);
				constraintsShader.SetInt("activeConstraintCount", m_ConstraintCount);
				constraintsShader.SetFloat("deltaTime", substepTime);
				if (m_RecalculateRestShape)
				{
					m_RecalculateRestShape = false;
					int restStateKernel = ((ComputeShapeMatchingConstraints)m_Constraints).restStateKernel;
					constraintsShader.SetBuffer(restStateKernel, "particleIndices", particleIndices);
					constraintsShader.SetBuffer(restStateKernel, "firstIndex", firstIndexBuffer);
					constraintsShader.SetBuffer(restStateKernel, "numIndices", numIndicesBuffer);
					constraintsShader.SetBuffer(restStateKernel, "RW_restComs", restComsBuffer);
					constraintsShader.SetBuffer(restStateKernel, "RW_Aqq", AqqBuffer);
					constraintsShader.SetBuffer(restStateKernel, "RW_deformation", plasticDeformationsBuffer);
					constraintsShader.SetBuffer(restStateKernel, "restPositions", base.solverImplementation.restPositionsBuffer);
					constraintsShader.SetBuffer(restStateKernel, "restOrientations", base.solverImplementation.restOrientationsBuffer);
					constraintsShader.SetBuffer(restStateKernel, "invMasses", base.solverImplementation.invMassesBuffer);
					constraintsShader.SetBuffer(restStateKernel, "invRotationalMasses", base.solverImplementation.invRotationalMassesBuffer);
					constraintsShader.SetBuffer(restStateKernel, "principalRadii", base.solverImplementation.principalRadiiBuffer);
					constraintsShader.Dispatch(restStateKernel, threadGroupsX, 1, 1);
					m_RestComs.Readback();
					m_RestComs.WaitForReadback();
				}
				int projectKernel = ((ComputeShapeMatchingConstraints)m_Constraints).projectKernel;
				int plasticityKernel = ((ComputeShapeMatchingConstraints)m_Constraints).plasticityKernel;
				constraintsShader.SetBuffer(projectKernel, "particleIndices", particleIndices);
				constraintsShader.SetBuffer(projectKernel, "firstIndex", firstIndexBuffer);
				constraintsShader.SetBuffer(projectKernel, "numIndices", numIndicesBuffer);
				constraintsShader.SetBuffer(projectKernel, "explicitGroup", explicitGroupBuffer);
				constraintsShader.SetBuffer(projectKernel, "shapeMaterialParameters", shapeMaterialParametersBuffer);
				constraintsShader.SetBuffer(projectKernel, "restComs", restComsBuffer);
				constraintsShader.SetBuffer(projectKernel, "coms", comsBuffer);
				constraintsShader.SetBuffer(projectKernel, "constraintOrientations", constraintOrientationsBuffer);
				constraintsShader.SetBuffer(projectKernel, "Aqq", AqqBuffer);
				constraintsShader.SetBuffer(projectKernel, "RW_linearTransforms", linearTransformsBuffer);
				constraintsShader.SetBuffer(projectKernel, "deformation", plasticDeformationsBuffer);
				constraintsShader.SetBuffer(projectKernel, "positions", base.solverImplementation.positionsBuffer);
				constraintsShader.SetBuffer(projectKernel, "restPositions", base.solverImplementation.restPositionsBuffer);
				constraintsShader.SetBuffer(projectKernel, "orientations", base.solverImplementation.orientationsBuffer);
				constraintsShader.SetBuffer(projectKernel, "restOrientations", base.solverImplementation.restOrientationsBuffer);
				constraintsShader.SetBuffer(projectKernel, "invMasses", base.solverImplementation.invMassesBuffer);
				constraintsShader.SetBuffer(projectKernel, "invRotationalMasses", base.solverImplementation.invRotationalMassesBuffer);
				constraintsShader.SetBuffer(projectKernel, "principalRadii", base.solverImplementation.principalRadiiBuffer);
				constraintsShader.SetBuffer(projectKernel, "deltasAsInt", base.solverImplementation.positionDeltasIntBuffer);
				constraintsShader.SetBuffer(projectKernel, "positionConstraintCounts", base.solverImplementation.positionConstraintCountBuffer);
				constraintsShader.Dispatch(projectKernel, threadGroupsX, 1, 1);
				constraintsShader.SetBuffer(plasticityKernel, "particleIndices", particleIndices);
				constraintsShader.SetBuffer(plasticityKernel, "firstIndex", firstIndexBuffer);
				constraintsShader.SetBuffer(plasticityKernel, "numIndices", numIndicesBuffer);
				constraintsShader.SetBuffer(plasticityKernel, "shapeMaterialParameters", shapeMaterialParametersBuffer);
				constraintsShader.SetBuffer(plasticityKernel, "RW_restComs", restComsBuffer);
				constraintsShader.SetBuffer(plasticityKernel, "constraintOrientations", constraintOrientationsBuffer);
				constraintsShader.SetBuffer(plasticityKernel, "RW_Aqq", AqqBuffer);
				constraintsShader.SetBuffer(plasticityKernel, "linearTransforms", linearTransformsBuffer);
				constraintsShader.SetBuffer(plasticityKernel, "RW_deformation", plasticDeformationsBuffer);
				constraintsShader.SetBuffer(plasticityKernel, "restPositions", base.solverImplementation.restPositionsBuffer);
				constraintsShader.SetBuffer(plasticityKernel, "restOrientations", base.solverImplementation.restOrientationsBuffer);
				constraintsShader.SetBuffer(plasticityKernel, "invMasses", base.solverImplementation.invMassesBuffer);
				constraintsShader.SetBuffer(plasticityKernel, "invRotationalMasses", base.solverImplementation.invRotationalMassesBuffer);
				constraintsShader.SetBuffer(plasticityKernel, "principalRadii", base.solverImplementation.principalRadiiBuffer);
				constraintsShader.Dispatch(plasticityKernel, threadGroupsX, 1, 1);
			}
		}

		public override void Apply(float substepTime)
		{
			if (m_ConstraintCount > 0)
			{
				Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
				ComputeShader constraintsShader = ((ComputeShapeMatchingConstraints)m_Constraints).constraintsShader;
				int applyKernel = ((ComputeShapeMatchingConstraints)m_Constraints).applyKernel;
				constraintsShader.SetBuffer(applyKernel, "particleIndices", particleIndices);
				constraintsShader.SetBuffer(applyKernel, "firstIndex", firstIndexBuffer);
				constraintsShader.SetBuffer(applyKernel, "numIndices", numIndicesBuffer);
				constraintsShader.SetBuffer(applyKernel, "RW_positions", base.solverImplementation.positionsBuffer);
				constraintsShader.SetBuffer(applyKernel, "positionConstraintCounts", base.solverImplementation.positionConstraintCountBuffer);
				constraintsShader.SetBuffer(applyKernel, "deltasAsInt", base.solverImplementation.positionDeltasIntBuffer);
				constraintsShader.SetInt("activeConstraintCount", m_ConstraintCount);
				constraintsShader.SetFloat("sorFactor", constraintParameters.SORFactor);
				int threadGroupsX = ComputeMath.ThreadGroupCount(m_ConstraintCount, 128);
				constraintsShader.Dispatch(applyKernel, threadGroupsX, 1, 1);
			}
		}

		public void CalculateRestShapeMatching()
		{
			m_RecalculateRestShape = true;
		}

		public void RequestDataReadback()
		{
			m_Coms.Readback();
			m_ConstraintOrientations.Readback();
		}

		public void WaitForReadback()
		{
			m_Coms.WaitForReadback();
			m_ConstraintOrientations.WaitForReadback();
		}
	}
}
