namespace Obi
{
	public class BurstStretchShearConstraints : BurstConstraintsImpl<BurstStretchShearConstraintsBatch>
	{
		public BurstStretchShearConstraints(BurstSolverImpl solver)
			: base(solver, Oni.ConstraintType.StretchShear)
		{
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			BurstStretchShearConstraintsBatch burstStretchShearConstraintsBatch = new BurstStretchShearConstraintsBatch(this);
			batches.Add(burstStretchShearConstraintsBatch);
			return burstStretchShearConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as BurstStretchShearConstraintsBatch);
			batch.Destroy();
		}
	}
}
