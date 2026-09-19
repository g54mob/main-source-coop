using UnityEngine;

namespace Obi
{
	public class ComputeShapeMatchingConstraints : ComputeConstraintsImpl<ComputeShapeMatchingConstraintsBatch>
	{
		public ComputeShader constraintsShader;

		public int projectKernel;

		public int plasticityKernel;

		public int restStateKernel;

		public int applyKernel;

		public ComputeShapeMatchingConstraints(ComputeSolverImpl solver)
			: base(solver, Oni.ConstraintType.ShapeMatching)
		{
			constraintsShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/ShapeMatchingConstraints"));
			projectKernel = constraintsShader.FindKernel("Project");
			plasticityKernel = constraintsShader.FindKernel("PlasticDeformation");
			restStateKernel = constraintsShader.FindKernel("CalculateRestShapeMatching");
			applyKernel = constraintsShader.FindKernel("Apply");
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			ComputeShapeMatchingConstraintsBatch computeShapeMatchingConstraintsBatch = new ComputeShapeMatchingConstraintsBatch(this);
			batches.Add(computeShapeMatchingConstraintsBatch);
			return computeShapeMatchingConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as ComputeShapeMatchingConstraintsBatch);
			batch.Destroy();
		}

		public void RequestDataReadback()
		{
			foreach (ComputeShapeMatchingConstraintsBatch batch in batches)
			{
				batch.RequestDataReadback();
			}
		}

		public void WaitForReadback()
		{
			foreach (ComputeShapeMatchingConstraintsBatch batch in batches)
			{
				batch.WaitForReadback();
			}
		}
	}
}
