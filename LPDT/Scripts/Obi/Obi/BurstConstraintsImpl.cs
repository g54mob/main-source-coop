using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;

namespace Obi
{
	public abstract class BurstConstraintsImpl<T> : IBurstConstraintsImpl, IConstraints where T : BurstConstraintsBatchImpl
	{
		protected BurstSolverImpl m_Solver;

		public List<T> batches = new List<T>();

		protected Oni.ConstraintType m_ConstraintType;

		public Oni.ConstraintType constraintType => m_ConstraintType;

		public ISolverImpl solver => m_Solver;

		public BurstConstraintsImpl(BurstSolverImpl solver, Oni.ConstraintType constraintType)
		{
			m_ConstraintType = constraintType;
			m_Solver = solver;
		}

		public virtual void Dispose()
		{
		}

		public abstract IConstraintsBatchImpl CreateConstraintsBatch();

		public abstract void RemoveBatch(IConstraintsBatchImpl batch);

		public virtual int GetConstraintCount()
		{
			int num = 0;
			if (batches == null)
			{
				return num;
			}
			foreach (T batch in batches)
			{
				if (batch != null)
				{
					num += batch.GetConstraintCount();
				}
			}
			return num;
		}

		public JobHandle Initialize(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (batches.Count > 0)
			{
				NativeArray<JobHandle> jobs = new NativeArray<JobHandle>(batches.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
				for (int i = 0; i < batches.Count; i++)
				{
					jobs[i] = (batches[i].enabled ? batches[i].Initialize(inputDeps, stepTime, substepTime, steps, timeLeft) : inputDeps);
				}
				JobHandle result = JobHandle.CombineDependencies(jobs);
				jobs.Dispose();
				return result;
			}
			return inputDeps;
		}

		public JobHandle Project(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			switch (m_Solver.abstraction.GetConstraintParameters(m_ConstraintType).evaluationOrder)
			{
			case Oni.ConstraintParameters.EvaluationOrder.Sequential:
				inputDeps = EvaluateSequential(inputDeps, stepTime, substepTime, steps, timeLeft);
				break;
			case Oni.ConstraintParameters.EvaluationOrder.Parallel:
				inputDeps = EvaluateParallel(inputDeps, stepTime, substepTime, steps, timeLeft);
				break;
			}
			return inputDeps;
		}

		protected virtual JobHandle EvaluateSequential(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			for (int i = 0; i < batches.Count; i++)
			{
				if (batches[i].enabled)
				{
					inputDeps = batches[i].Evaluate(inputDeps, stepTime, substepTime, steps, timeLeft);
					inputDeps = batches[i].Apply(inputDeps, substepTime);
					m_Solver.ScheduleBatchedJobsIfNeeded();
				}
			}
			return inputDeps;
		}

		protected virtual JobHandle EvaluateParallel(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			for (int i = 0; i < batches.Count; i++)
			{
				if (batches[i].enabled)
				{
					inputDeps = batches[i].Evaluate(inputDeps, stepTime, substepTime, steps, timeLeft);
					m_Solver.ScheduleBatchedJobsIfNeeded();
				}
			}
			for (int j = 0; j < batches.Count; j++)
			{
				if (batches[j].enabled)
				{
					inputDeps = batches[j].Apply(inputDeps, substepTime);
					m_Solver.ScheduleBatchedJobsIfNeeded();
				}
			}
			return inputDeps;
		}
	}
}
