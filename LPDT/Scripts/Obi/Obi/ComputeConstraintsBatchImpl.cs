using UnityEngine;

namespace Obi
{
	public abstract class ComputeConstraintsBatchImpl : IConstraintsBatchImpl
	{
		protected IComputeConstraintsImpl m_Constraints;

		protected Oni.ConstraintType m_ConstraintType;

		protected bool m_Enabled = true;

		protected int m_ConstraintCount;

		protected GraphicsBuffer particleIndices;

		protected GraphicsBuffer lambdas;

		protected ObiNativeFloatList lambdasList;

		public Oni.ConstraintType constraintType => m_ConstraintType;

		public bool enabled
		{
			get
			{
				return m_Enabled;
			}
			set
			{
				if (m_Enabled != value)
				{
					m_Enabled = value;
				}
			}
		}

		public IConstraints constraints => m_Constraints;

		public ObiSolver solverAbstraction => ((ComputeSolverImpl)m_Constraints.solver).abstraction;

		public ComputeSolverImpl solverImplementation => (ComputeSolverImpl)m_Constraints.solver;

		public virtual void Initialize(float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (lambdasList != null)
			{
				lambdasList.WipeToZero();
				lambdasList.Upload();
			}
		}

		public abstract void Evaluate(float stepTime, float substepTime, int steps, float timeLeft);

		public abstract void Apply(float substepTime);

		public ComputeConstraintsBatchImpl()
		{
		}

		public virtual void Destroy()
		{
		}

		public void SetDependency(IConstraintsBatchImpl batch)
		{
		}

		public void SetConstraintCount(int constraintCount)
		{
			m_ConstraintCount = constraintCount;
		}

		public int GetConstraintCount()
		{
			return m_ConstraintCount;
		}
	}
}
