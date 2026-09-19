using System.Collections.Generic;

namespace Obi
{
	public abstract class ComputeConstraintsImpl<T> : IComputeConstraintsImpl, IConstraints where T : ComputeConstraintsBatchImpl
	{
		protected ComputeSolverImpl m_Solver;

		public List<T> batches = new List<T>();

		protected Oni.ConstraintType m_ConstraintType;

		public Oni.ConstraintType constraintType => m_ConstraintType;

		public ISolverImpl solver => m_Solver;

		public ComputeConstraintsImpl(ComputeSolverImpl solver, Oni.ConstraintType constraintType)
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

		public void Initialize(float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (batches.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < batches.Count; i++)
			{
				if (batches[i].enabled)
				{
					batches[i].Initialize(stepTime, substepTime, steps, timeLeft);
				}
			}
		}

		public void Project(float stepTime, float substepTime, int substeps, float timeLeft)
		{
			switch (m_Solver.abstraction.GetConstraintParameters(m_ConstraintType).evaluationOrder)
			{
			case Oni.ConstraintParameters.EvaluationOrder.Sequential:
				EvaluateSequential(stepTime, substepTime, substeps, timeLeft);
				break;
			case Oni.ConstraintParameters.EvaluationOrder.Parallel:
				EvaluateParallel(stepTime, substepTime, substeps, timeLeft);
				break;
			}
		}

		protected virtual void EvaluateSequential(float stepTime, float substepTime, int substeps, float timeLeft)
		{
			for (int i = 0; i < batches.Count; i++)
			{
				if (batches[i].enabled)
				{
					batches[i].Evaluate(stepTime, substepTime, substeps, timeLeft);
					batches[i].Apply(substepTime);
				}
			}
		}

		protected virtual void EvaluateParallel(float stepTime, float substepTime, int substeps, float timeLeft)
		{
			for (int i = 0; i < batches.Count; i++)
			{
				if (batches[i].enabled)
				{
					batches[i].Evaluate(stepTime, substepTime, substeps, timeLeft);
				}
			}
			for (int j = 0; j < batches.Count; j++)
			{
				if (batches[j].enabled)
				{
					batches[j].Apply(substepTime);
				}
			}
		}
	}
}
