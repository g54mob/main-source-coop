using UnityEngine;

namespace Obi
{
	public class ComputeColliderFrictionConstraints : ComputeConstraintsImpl<ComputeColliderFrictionConstraintsBatch>
	{
		public ComputeShader constraintsShader;

		public int projectKernel;

		public int applyKernel;

		public ComputeColliderFrictionConstraints(ComputeSolverImpl solver)
			: base(solver, Oni.ConstraintType.Friction)
		{
			constraintsShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/ColliderFrictionConstraints"));
			projectKernel = constraintsShader.FindKernel("Project");
			applyKernel = constraintsShader.FindKernel("Apply");
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			ComputeColliderFrictionConstraintsBatch computeColliderFrictionConstraintsBatch = new ComputeColliderFrictionConstraintsBatch(this);
			batches.Add(computeColliderFrictionConstraintsBatch);
			return computeColliderFrictionConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as ComputeColliderFrictionConstraintsBatch);
			batch.Destroy();
		}
	}
}
