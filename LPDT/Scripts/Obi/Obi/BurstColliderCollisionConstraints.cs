namespace Obi
{
	public class BurstColliderCollisionConstraints : BurstConstraintsImpl<BurstColliderCollisionConstraintsBatch>
	{
		public BurstColliderCollisionConstraints(BurstSolverImpl solver)
			: base(solver, Oni.ConstraintType.Collision)
		{
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			BurstColliderCollisionConstraintsBatch burstColliderCollisionConstraintsBatch = new BurstColliderCollisionConstraintsBatch(this);
			batches.Add(burstColliderCollisionConstraintsBatch);
			return burstColliderCollisionConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as BurstColliderCollisionConstraintsBatch);
			batch.Destroy();
		}

		public override int GetConstraintCount()
		{
			return ((BurstSolverImpl)base.solver).abstraction.colliderContacts.count;
		}
	}
}
