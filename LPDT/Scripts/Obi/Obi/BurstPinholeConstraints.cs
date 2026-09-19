namespace Obi
{
	public class BurstPinholeConstraints : BurstConstraintsImpl<BurstPinholeConstraintsBatch>
	{
		public BurstPinholeConstraints(BurstSolverImpl solver)
			: base(solver, Oni.ConstraintType.Pinhole)
		{
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			BurstPinholeConstraintsBatch burstPinholeConstraintsBatch = new BurstPinholeConstraintsBatch(this);
			batches.Add(burstPinholeConstraintsBatch);
			return burstPinholeConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as BurstPinholeConstraintsBatch);
			batch.Destroy();
		}
	}
}
