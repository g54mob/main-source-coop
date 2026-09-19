namespace Obi
{
	public class BurstChainConstraints : BurstConstraintsImpl<BurstChainConstraintsBatch>
	{
		public BurstChainConstraints(BurstSolverImpl solver)
			: base(solver, Oni.ConstraintType.Chain)
		{
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			BurstChainConstraintsBatch burstChainConstraintsBatch = new BurstChainConstraintsBatch(this);
			batches.Add(burstChainConstraintsBatch);
			return burstChainConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as BurstChainConstraintsBatch);
			batch.Destroy();
		}
	}
}
