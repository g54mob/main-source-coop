using System;
using UnityEngine;

namespace RootMotion.FinalIK
{
	public class FABRIKBendGoal : MonoBehaviour
	{
		public FABRIK ik;

		[Range(0f, 1f)]
		public float weight = 1f;

		private void Start()
		{
			IKSolverFABRIK solver = ik.solver;
			solver.OnPreIteration = (IKSolver.IterationDelegate)Delegate.Combine(solver.OnPreIteration, new IKSolver.IterationDelegate(OnPreIteration));
		}

		private void OnPreIteration(int it)
		{
			if (it == 0 && !(weight <= 0f))
			{
				Vector3 vector = base.transform.position - ik.solver.bones[0].transform.position;
				vector *= weight;
				IKSolver.Bone[] bones = ik.solver.bones;
				for (int i = 0; i < bones.Length; i++)
				{
					bones[i].solverPosition += vector;
				}
			}
		}

		private void OnDestroy()
		{
			if (ik != null)
			{
				IKSolverFABRIK solver = ik.solver;
				solver.OnPreIteration = (IKSolver.IterationDelegate)Delegate.Remove(solver.OnPreIteration, new IKSolver.IterationDelegate(OnPreIteration));
			}
		}
	}
}
