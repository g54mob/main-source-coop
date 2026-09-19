namespace Obi
{
	public class BurstBendConstraints : BurstConstraintsImpl<BurstBendConstraintsBatch>
	{
		public BurstBendConstraints(BurstSolverImpl solver)
			: base(solver, Oni.ConstraintType.Bending)
		{
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			BurstBendConstraintsBatch burstBendConstraintsBatch = new BurstBendConstraintsBatch(this);
			batches.Add(burstBendConstraintsBatch);
			return burstBendConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as BurstBendConstraintsBatch);
			batch.Destroy();
		}
	}
}
