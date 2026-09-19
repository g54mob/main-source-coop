using UnityEngine;

namespace Obi
{
	public class ComputePinholeConstraints : ComputeConstraintsImpl<ComputePinholeConstraintsBatch>
	{
		public ComputeShader constraintsShader;

		public int clearKernel;

		public int initializeKernel;

		public int projectKernel;

		public int applyKernel;

		public ComputePinholeConstraints(ComputeSolverImpl solver)
			: base(solver, Oni.ConstraintType.Pinhole)
		{
			constraintsShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/PinholeConstraints"));
			clearKernel = constraintsShader.FindKernel("Clear");
			initializeKernel = constraintsShader.FindKernel("Initialize");
			projectKernel = constraintsShader.FindKernel("Project");
			applyKernel = constraintsShader.FindKernel("Apply");
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			ComputePinholeConstraintsBatch computePinholeConstraintsBatch = new ComputePinholeConstraintsBatch(this);
			batches.Add(computePinholeConstraintsBatch);
			return computePinholeConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as ComputePinholeConstraintsBatch);
			batch.Destroy();
		}

		public void RequestDataReadback()
		{
			foreach (ComputePinholeConstraintsBatch batch in batches)
			{
				batch.RequestDataReadback();
			}
		}

		public void WaitForReadback()
		{
			foreach (ComputePinholeConstraintsBatch batch in batches)
			{
				batch.WaitForReadback();
			}
		}
	}
}
