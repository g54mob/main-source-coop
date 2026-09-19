namespace Obi
{
	public class BurstDistanceConstraints : BurstConstraintsImpl<BurstDistanceConstraintsBatch>
	{
		public BurstDistanceConstraints(BurstSolverImpl solver)
			: base(solver, Oni.ConstraintType.Distance)
		{
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			BurstDistanceConstraintsBatch burstDistanceConstraintsBatch = new BurstDistanceConstraintsBatch(this);
			batches.Add(burstDistanceConstraintsBatch);
			return burstDistanceConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as BurstDistanceConstraintsBatch);
			batch.Destroy();
		}
	}
}
