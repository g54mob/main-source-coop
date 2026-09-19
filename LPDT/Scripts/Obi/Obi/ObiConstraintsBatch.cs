using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	public abstract class ObiConstraintsBatch : IObiConstraintsBatch
	{
		[HideInInspector]
		[SerializeField]
		protected List<int> m_IDs = new List<int>();

		[HideInInspector]
		[SerializeField]
		protected List<int> m_IDToIndex = new List<int>();

		[HideInInspector]
		[SerializeField]
		protected int m_ConstraintCount;

		[HideInInspector]
		[SerializeField]
		protected int m_ActiveConstraintCount;

		[HideInInspector]
		[SerializeField]
		protected int m_InitialActiveConstraintCount;

		[HideInInspector]
		public ObiNativeIntList particleIndices = new ObiNativeIntList();

		[HideInInspector]
		public ObiNativeFloatList lambdas = new ObiNativeFloatList();

		public int constraintCount => m_ConstraintCount;

		public int activeConstraintCount
		{
			get
			{
				return m_ActiveConstraintCount;
			}
			set
			{
				m_ActiveConstraintCount = value;
			}
		}

		public virtual int initialActiveConstraintCount
		{
			get
			{
				return m_InitialActiveConstraintCount;
			}
			set
			{
				m_InitialActiveConstraintCount = value;
			}
		}

		public abstract Oni.ConstraintType constraintType { get; }

		public abstract IConstraintsBatchImpl implementation { get; }

		public virtual void Merge(ObiActor actor, IObiConstraintsBatch other)
		{
			m_ConstraintCount += other.constraintCount;
			m_ActiveConstraintCount += other.activeConstraintCount;
			m_InitialActiveConstraintCount += other.initialActiveConstraintCount;
		}

		protected abstract void SwapConstraints(int sourceIndex, int destIndex);

		public abstract void GetParticlesInvolved(int index, List<int> particles);

		public virtual void AddToSolver(ObiSolver solver)
		{
		}

		public virtual void RemoveFromSolver(ObiSolver solver)
		{
			particleIndices.Dispose();
			lambdas.Dispose();
		}

		protected virtual void CopyConstraint(ObiConstraintsBatch batch, int constraintIndex)
		{
		}

		private void InnerSwapConstraints(int sourceIndex, int destIndex)
		{
			m_IDToIndex[m_IDs[sourceIndex]] = destIndex;
			m_IDToIndex[m_IDs[destIndex]] = sourceIndex;
			m_IDs.Swap(sourceIndex, destIndex);
			SwapConstraints(sourceIndex, destIndex);
		}

		protected void RegisterConstraint()
		{
			m_IDs.Add(m_ConstraintCount);
			m_IDToIndex.Add(m_ConstraintCount);
			m_ConstraintCount++;
		}

		public virtual void Clear()
		{
			m_ConstraintCount = 0;
			m_ActiveConstraintCount = 0;
			m_IDs.Clear();
			m_IDToIndex.Clear();
			particleIndices.Clear();
			lambdas.Clear();
		}

		public int GetConstraintIndex(int constraintId)
		{
			if (constraintId < 0 || constraintId >= constraintCount)
			{
				return -1;
			}
			return m_IDToIndex[constraintId];
		}

		public bool IsConstraintActive(int index)
		{
			return index < m_ActiveConstraintCount;
		}

		public bool ActivateConstraint(int constraintIndex)
		{
			if (constraintIndex < m_ActiveConstraintCount)
			{
				return false;
			}
			InnerSwapConstraints(constraintIndex, m_ActiveConstraintCount);
			m_ActiveConstraintCount++;
			return true;
		}

		public bool DeactivateConstraint(int constraintIndex)
		{
			if (constraintIndex >= m_ActiveConstraintCount)
			{
				return false;
			}
			m_ActiveConstraintCount--;
			InnerSwapConstraints(constraintIndex, m_ActiveConstraintCount);
			return true;
		}

		public void DeactivateAllConstraints()
		{
			m_ActiveConstraintCount = 0;
		}

		public void ActivateAllConstraints()
		{
			m_ActiveConstraintCount = m_ConstraintCount;
		}

		public void RemoveConstraint(int constraintIndex)
		{
			SwapConstraints(constraintIndex, constraintCount - 1);
			m_IDs.RemoveAt(constraintCount - 1);
			m_IDToIndex.RemoveAt(constraintCount - 1);
			m_ConstraintCount--;
			m_ActiveConstraintCount = Mathf.Min(m_ActiveConstraintCount, m_ConstraintCount);
		}

		public void ParticlesSwapped(int index, int newIndex)
		{
			for (int i = 0; i < particleIndices.count; i++)
			{
				if (particleIndices[i] == newIndex)
				{
					particleIndices[i] = index;
				}
				else if (particleIndices[i] == index)
				{
					particleIndices[i] = newIndex;
				}
			}
		}
	}
}
