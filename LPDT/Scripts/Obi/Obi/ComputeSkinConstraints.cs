using UnityEngine;

namespace Obi
{
	public class ComputeSkinConstraints : ComputeConstraintsImpl<ComputeSkinConstraintsBatch>
	{
		public ComputeShader constraintsShader;

		public int projectKernel;

		public int applyKernel;

		public ComputeSkinConstraints(ComputeSolverImpl solver)
			: base(solver, Oni.ConstraintType.Skin)
		{
			constraintsShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/SkinConstraints"));
			projectKernel = constraintsShader.FindKernel("Project");
			applyKernel = constraintsShader.FindKernel("Apply");
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			ComputeSkinConstraintsBatch computeSkinConstraintsBatch = new ComputeSkinConstraintsBatch(this);
			batches.Add(computeSkinConstraintsBatch);
			return computeSkinConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as ComputeSkinConstraintsBatch);
			batch.Destroy();
		}
	}
}
