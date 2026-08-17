using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class VRIKArmMocap : MonoBehaviour
	{
		public VRIK ik;

		public Transform leftElbowTarget;

		public Transform rightElbowTarget;

		private void Start()
		{
			IKSolverVR solver = ik.solver;
			solver.OnPostUpdate = (IKSolver.UpdateDelegate)Delegate.Combine(solver.OnPostUpdate, new IKSolver.UpdateDelegate(AfterVRIK));
		}

		private void AfterVRIK()
		{
			UpdateArm(ik.references.leftUpperArm, ik.references.leftForearm, ik.references.leftHand, leftElbowTarget, ik.solver.leftArm.target);
			UpdateArm(ik.references.rightUpperArm, ik.references.rightForearm, ik.references.rightHand, rightElbowTarget, ik.solver.rightArm.target);
		}

		private static void UpdateArm(Transform upperArm, Transform forearm, Transform hand, Transform elbowTarget, Transform handTarget)
		{
			if (!(elbowTarget == null) && !(handTarget == null))
			{
				upperArm.rotation = Quaternion.FromToRotation(forearm.position - upperArm.position, elbowTarget.position - upperArm.position) * upperArm.rotation;
				forearm.rotation = Quaternion.FromToRotation(hand.position - forearm.position, handTarget.position - forearm.position) * forearm.rotation;
			}
		}

		private void OnDestroy()
		{
			if (ik != null)
			{
				IKSolverVR solver = ik.solver;
				solver.OnPostUpdate = (IKSolver.UpdateDelegate)Delegate.Remove(solver.OnPostUpdate, new IKSolver.UpdateDelegate(AfterVRIK));
			}
		}
	}
}
