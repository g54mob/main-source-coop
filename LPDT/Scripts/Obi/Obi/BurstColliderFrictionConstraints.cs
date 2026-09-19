namespace Obi
{
	public class BurstColliderFrictionConstraints : BurstConstraintsImpl<BurstColliderFrictionConstraintsBatch>
	{
		public BurstColliderFrictionConstraints(BurstSolverImpl solver)
			: base(solver, Oni.ConstraintType.Friction)
		{
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			BurstColliderFrictionConstraintsBatch burstColliderFrictionConstraintsBatch = new BurstColliderFrictionConstraintsBatch(this);
			batches.Add(burstColliderFrictionConstraintsBatch);
			return burstColliderFrictionConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as BurstColliderFrictionConstraintsBatch);
			batch.Destroy();
		}

		public override int GetConstraintCount()
		{
			return ((BurstSolverImpl)base.solver).abstraction.colliderContacts.count;
		}
	}
}
