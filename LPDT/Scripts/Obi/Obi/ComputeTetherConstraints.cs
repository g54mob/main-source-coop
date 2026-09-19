using UnityEngine;

namespace Obi
{
	public class ComputeTetherConstraints : ComputeConstraintsImpl<ComputeTetherConstraintsBatch>
	{
		public ComputeShader constraintsShader;

		public int projectKernel;

		public int applyKernel;

		public ComputeTetherConstraints(ComputeSolverImpl solver)
			: base(solver, Oni.ConstraintType.Tether)
		{
			constraintsShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/TetherConstraints"));
			projectKernel = constraintsShader.FindKernel("Project");
			applyKernel = constraintsShader.FindKernel("Apply");
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			ComputeTetherConstraintsBatch computeTetherConstraintsBatch = new ComputeTetherConstraintsBatch(this);
			batches.Add(computeTetherConstraintsBatch);
			return computeTetherConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as ComputeTetherConstraintsBatch);
			batch.Destroy();
		}
	}
}
