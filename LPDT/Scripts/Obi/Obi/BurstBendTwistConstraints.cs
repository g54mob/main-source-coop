namespace Obi
{
	public class BurstBendTwistConstraints : BurstConstraintsImpl<BurstBendTwistConstraintsBatch>
	{
		public BurstBendTwistConstraints(BurstSolverImpl solver)
			: base(solver, Oni.ConstraintType.BendTwist)
		{
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			BurstBendTwistConstraintsBatch burstBendTwistConstraintsBatch = new BurstBendTwistConstraintsBatch(this);
			batches.Add(burstBendTwistConstraintsBatch);
			return burstBendTwistConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as BurstBendTwistConstraintsBatch);
			batch.Destroy();
		}
	}
}
