using UnityEngine;

namespace Obi
{
	public class ComputeChainConstraints : ComputeConstraintsImpl<ComputeChainConstraintsBatch>
	{
		public ComputeShader constraintsShader;

		public int projectKernel;

		public int applyKernel;

		public ComputeChainConstraints(ComputeSolverImpl solver)
			: base(solver, Oni.ConstraintType.Chain)
		{
			constraintsShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/ChainConstraints"));
			projectKernel = constraintsShader.FindKernel("Project");
			applyKernel = constraintsShader.FindKernel("Apply");
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			ComputeChainConstraintsBatch computeChainConstraintsBatch = new ComputeChainConstraintsBatch(this);
			batches.Add(computeChainConstraintsBatch);
			return computeChainConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as ComputeChainConstraintsBatch);
			batch.Destroy();
		}
	}
}
