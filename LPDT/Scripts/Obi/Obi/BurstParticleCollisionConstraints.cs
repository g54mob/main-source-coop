namespace Obi
{
	public class BurstParticleCollisionConstraints : BurstConstraintsImpl<BurstParticleCollisionConstraintsBatch>
	{
		public BurstParticleCollisionConstraints(BurstSolverImpl solver)
			: base(solver, Oni.ConstraintType.ParticleCollision)
		{
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			BurstParticleCollisionConstraintsBatch burstParticleCollisionConstraintsBatch = new BurstParticleCollisionConstraintsBatch(this);
			batches.Add(burstParticleCollisionConstraintsBatch);
			return burstParticleCollisionConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as BurstParticleCollisionConstraintsBatch);
			batch.Destroy();
		}

		public override int GetConstraintCount()
		{
			return ((BurstSolverImpl)base.solver).abstraction.particleContacts.count;
		}
	}
}
