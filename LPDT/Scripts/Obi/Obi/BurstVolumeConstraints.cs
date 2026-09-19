namespace Obi
{
	public class BurstVolumeConstraints : BurstConstraintsImpl<BurstVolumeConstraintsBatch>
	{
		public BurstVolumeConstraints(BurstSolverImpl solver)
			: base(solver, Oni.ConstraintType.Volume)
		{
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			BurstVolumeConstraintsBatch burstVolumeConstraintsBatch = new BurstVolumeConstraintsBatch(this);
			batches.Add(burstVolumeConstraintsBatch);
			return burstVolumeConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as BurstVolumeConstraintsBatch);
			batch.Destroy();
		}
	}
}
