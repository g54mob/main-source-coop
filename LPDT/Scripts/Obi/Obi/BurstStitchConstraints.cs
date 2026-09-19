namespace Obi
{
	public class BurstStitchConstraints : BurstConstraintsImpl<BurstStitchConstraintsBatch>
	{
		public BurstStitchConstraints(BurstSolverImpl solver)
			: base(solver, Oni.ConstraintType.Stitch)
		{
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			BurstStitchConstraintsBatch burstStitchConstraintsBatch = new BurstStitchConstraintsBatch(this);
			batches.Add(burstStitchConstraintsBatch);
			return burstStitchConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as BurstStitchConstraintsBatch);
			batch.Destroy();
		}
	}
}
