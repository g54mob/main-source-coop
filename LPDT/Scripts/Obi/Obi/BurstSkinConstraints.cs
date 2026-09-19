namespace Obi
{
	public class BurstSkinConstraints : BurstConstraintsImpl<BurstSkinConstraintsBatch>
	{
		public BurstSkinConstraints(BurstSolverImpl solver)
			: base(solver, Oni.ConstraintType.Skin)
		{
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			BurstSkinConstraintsBatch burstSkinConstraintsBatch = new BurstSkinConstraintsBatch(this);
			batches.Add(burstSkinConstraintsBatch);
			return burstSkinConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as BurstSkinConstraintsBatch);
			batch.Destroy();
		}
	}
}
