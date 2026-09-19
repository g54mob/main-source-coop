using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public abstract class ObiConstraints<T> : IObiConstraints where T : class, IObiConstraintsBatch
	{
		[NonSerialized]
		protected ObiSolver m_Solver;

		[HideInInspector]
		public List<T> batches = new List<T>();

		public int batchCount
		{
			get
			{
				if (batches != null)
				{
					return batches.Count;
				}
				return 0;
			}
		}

		public void Merge(ObiActor actor, IObiConstraints other)
		{
			if (other is ObiConstraints<T> obiConstraints && other.GetConstraintType().HasValue)
			{
				int value = (int)other.GetConstraintType().Value;
				actor.solverBatchOffsets[value].Clear();
				int num = Mathf.Max(0, obiConstraints.batchCount - batchCount);
				for (int i = 0; i < num; i++)
				{
					AddBatch(CreateBatch());
				}
				for (int j = 0; j < other.batchCount; j++)
				{
					actor.solverBatchOffsets[value].Add(batches[j].activeConstraintCount);
					batches[j].Merge(actor, obiConstraints.batches[j]);
				}
			}
		}

		public IObiConstraintsBatch GetBatch(int i)
		{
			if (batches != null && i >= 0 && i < batches.Count)
			{
				return batches[i];
			}
			return null;
		}

		public int GetConstraintCount()
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
					num += batch.constraintCount;
				}
			}
			return num;
		}

		public int GetActiveConstraintCount()
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
					num += batch.activeConstraintCount;
				}
			}
			return num;
		}

		public void DeactivateAllConstraints()
		{
			if (batches == null)
			{
				return;
			}
			foreach (T batch in batches)
			{
				batch?.DeactivateAllConstraints();
			}
		}

		public void ActivateAllConstraints()
		{
			if (batches == null)
			{
				return;
			}
			foreach (T batch in batches)
			{
				batch?.ActivateAllConstraints();
			}
		}

		public T GetFirstBatch()
		{
			if (batches == null || batches.Count <= 0)
			{
				return null;
			}
			return batches[0];
		}

		public Oni.ConstraintType? GetConstraintType()
		{
			if (batches != null && batches.Count > 0)
			{
				return batches[0].constraintType;
			}
			return null;
		}

		public void Clear()
		{
			RemoveFromSolver();
			if (batches != null)
			{
				batches.Clear();
			}
		}

		public virtual T CreateBatch(T source = null)
		{
			return null;
		}

		public void AddBatch(T batch)
		{
			if (batch != null)
			{
				batches.Add(batch);
			}
		}

		public bool RemoveBatch(T batch)
		{
			if (batches == null || batch == null)
			{
				return false;
			}
			return batches.Remove(batch);
		}

		public bool AddToSolver(ObiSolver solver)
		{
			if (m_Solver != null || batches == null)
			{
				return false;
			}
			m_Solver = solver;
			foreach (T batch in batches)
			{
				batch.AddToSolver(m_Solver);
			}
			return true;
		}

		public bool RemoveFromSolver()
		{
			if (m_Solver == null || batches == null)
			{
				return false;
			}
			foreach (T batch in batches)
			{
				batch.RemoveFromSolver(m_Solver);
			}
			m_Solver = null;
			return true;
		}
	}
}
