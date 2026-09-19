using UnityEngine;

namespace Obi
{
	public class ComputeBendTwistConstraints : ComputeConstraintsImpl<ComputeBendTwistConstraintsBatch>
	{
		public ComputeShader constraintsShader;

		public int projectKernel;

		public int applyKernel;

		public ComputeBendTwistConstraints(ComputeSolverImpl solver)
			: base(solver, Oni.ConstraintType.BendTwist)
		{
			constraintsShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/BendTwistConstraints"));
			projectKernel = constraintsShader.FindKernel("Project");
			applyKernel = constraintsShader.FindKernel("Apply");
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			ComputeBendTwistConstraintsBatch computeBendTwistConstraintsBatch = new ComputeBendTwistConstraintsBatch(this);
			batches.Add(computeBendTwistConstraintsBatch);
			return computeBendTwistConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as ComputeBendTwistConstraintsBatch);
			batch.Destroy();
		}
	}
}
