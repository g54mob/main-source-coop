namespace Obi
{
	public class BurstTetherConstraints : BurstConstraintsImpl<BurstTetherConstraintsBatch>
	{
		public BurstTetherConstraints(BurstSolverImpl solver)
			: base(solver, Oni.ConstraintType.Distance)
		{
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			BurstTetherConstraintsBatch burstTetherConstraintsBatch = new BurstTetherConstraintsBatch(this);
			batches.Add(burstTetherConstraintsBatch);
			return burstTetherConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as BurstTetherConstraintsBatch);
			batch.Destroy();
		}
	}
}
