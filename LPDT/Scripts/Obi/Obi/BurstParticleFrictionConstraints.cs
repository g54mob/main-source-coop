namespace Obi
{
	public class BurstParticleFrictionConstraints : BurstConstraintsImpl<BurstParticleFrictionConstraintsBatch>
	{
		public BurstParticleFrictionConstraints(BurstSolverImpl solver)
			: base(solver, Oni.ConstraintType.ParticleFriction)
		{
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			BurstParticleFrictionConstraintsBatch burstParticleFrictionConstraintsBatch = new BurstParticleFrictionConstraintsBatch(this);
			batches.Add(burstParticleFrictionConstraintsBatch);
			return burstParticleFrictionConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as BurstParticleFrictionConstraintsBatch);
			batch.Destroy();
		}

		public override int GetConstraintCount()
		{
			return ((BurstSolverImpl)base.solver).abstraction.particleContacts.count;
		}
	}
}
