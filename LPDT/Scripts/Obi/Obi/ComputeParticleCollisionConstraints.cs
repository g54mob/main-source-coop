using UnityEngine;

namespace Obi
{
	public class ComputeParticleCollisionConstraints : ComputeConstraintsImpl<ComputeParticleCollisionConstraintsBatch>
	{
		public ComputeShader constraintsShader;

		public int initializeKernel;

		public int projectKernel;

		public int applyKernel;

		public ComputeParticleCollisionConstraints(ComputeSolverImpl solver)
			: base(solver, Oni.ConstraintType.ParticleCollision)
		{
			constraintsShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/ParticleCollisionConstraints"));
			initializeKernel = constraintsShader.FindKernel("Initialize");
			projectKernel = constraintsShader.FindKernel("Project");
			applyKernel = constraintsShader.FindKernel("Apply");
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			ComputeParticleCollisionConstraintsBatch computeParticleCollisionConstraintsBatch = new ComputeParticleCollisionConstraintsBatch(this);
			batches.Add(computeParticleCollisionConstraintsBatch);
			return computeParticleCollisionConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as ComputeParticleCollisionConstraintsBatch);
			batch.Destroy();
		}
	}
}
