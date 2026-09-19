using UnityEngine;

namespace Obi
{
	public class ComputeParticleFrictionConstraints : ComputeConstraintsImpl<ComputeParticleFrictionConstraintsBatch>
	{
		public ComputeShader constraintsShader;

		public int projectKernel;

		public int applyKernel;

		public ComputeParticleFrictionConstraints(ComputeSolverImpl solver)
			: base(solver, Oni.ConstraintType.ParticleFriction)
		{
			constraintsShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/ParticleFrictionConstraints"));
			projectKernel = constraintsShader.FindKernel("Project");
			applyKernel = constraintsShader.FindKernel("Apply");
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			ComputeParticleFrictionConstraintsBatch computeParticleFrictionConstraintsBatch = new ComputeParticleFrictionConstraintsBatch(this);
			batches.Add(computeParticleFrictionConstraintsBatch);
			return computeParticleFrictionConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as ComputeParticleFrictionConstraintsBatch);
			batch.Destroy();
		}
	}
}
