namespace Obi
{
	public class BurstShapeMatchingConstraints : BurstConstraintsImpl<BurstShapeMatchingConstraintsBatch>
	{
		public BurstShapeMatchingConstraints(BurstSolverImpl solver)
			: base(solver, Oni.ConstraintType.ShapeMatching)
		{
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			BurstShapeMatchingConstraintsBatch burstShapeMatchingConstraintsBatch = new BurstShapeMatchingConstraintsBatch(this);
			batches.Add(burstShapeMatchingConstraintsBatch);
			return burstShapeMatchingConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as BurstShapeMatchingConstraintsBatch);
			batch.Destroy();
		}
	}
}
