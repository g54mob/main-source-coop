using Unity.Jobs;

namespace Obi
{
	public class BurstPinConstraints : BurstConstraintsImpl<BurstPinConstraintsBatch>
	{
		public BurstPinConstraints(BurstSolverImpl solver)
			: base(solver, Oni.ConstraintType.Pin)
		{
		}

		public override IConstraintsBatchImpl CreateConstraintsBatch()
		{
			BurstPinConstraintsBatch burstPinConstraintsBatch = new BurstPinConstraintsBatch(this);
			batches.Add(burstPinConstraintsBatch);
			return burstPinConstraintsBatch;
		}

		public override void RemoveBatch(IConstraintsBatchImpl batch)
		{
			batches.Remove(batch as BurstPinConstraintsBatch);
			batch.Destroy();
		}

		public JobHandle ProjectRenderablePositions(JobHandle inputDeps)
		{
			for (int i = 0; i < batches.Count; i++)
			{
				if (batches[i].enabled)
				{
					inputDeps = batches[i].ProjectRenderablePositions(inputDeps);
					m_Solver.ScheduleBatchedJobsIfNeeded();
				}
			}
			return inputDeps;
		}
	}
}
