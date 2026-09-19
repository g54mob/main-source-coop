namespace Obi
{
	public class BurstAerodynamicConstraints : BurstConstraintsImpl<BurstAerodynamicConstraintsBatch>
	{
		public BurstAerodynamicConstraints(BurstSolverImpl solver)
			: base(solver, Oni.ConstraintType.Aerodynamics)
		{
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			BurstAerodynamicConstraintsBatch burstAerodynamicConstraintsBatch = new BurstAerodynamicConstraintsBatch(this);
			batches.Add(burstAerodynamicConstraintsBatch);
			return burstAerodynamicConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as BurstAerodynamicConstraintsBatch);
			batch.Destroy();
		}
	}
}
