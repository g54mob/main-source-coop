using System;
using UnityEngine;

namespace RootMotion.FinalIK
{
	public class OffsetPose : MonoBehaviour
	{
		[Serializable]
		public class EffectorLink
		{
			[Tooltip("The effector type (this is just an enum)")]
			public FullBodyBipedEffector effector;

			[Tooltip("Offset of the effector in this pose")]
			public Vector3 offset;

			[Tooltip("Pin position relative to the solver root Transform")]
			public Vector3 pin;

			[Tooltip("Pin weight vector")]
			public Vector3 pinWeight;

			[Tooltip("Only applies for end effectors (hands, feet)")]
			public Vector3 rotationOffset;

			public void Apply(IKSolverFullBodyBiped solver, float weight, Quaternion rotation)
			{
				IKEffector iKEffector = solver.GetEffector(effector);
				iKEffector.positionOffset += rotation * offset * weight;
				Vector3 vector = solver.GetRoot().position + rotation * pin - iKEffector.bone.position;
				Vector3 vector2 = pinWeight * Mathf.Abs(weight);
				iKEffector.positionOffset = new Vector3(Mathf.Lerp(iKEffector.positionOffset.x, vector.x, vector2.x), Mathf.Lerp(iKEffector.positionOffset.y, vector.y, vector2.y), Mathf.Lerp(iKEffector.positionOffset.z, vector.z, vector2.z));
				if (iKEffector.isEndEffector)
				{
					iKEffector.bone.localRotation *= Quaternion.Euler(rotationOffset * weight);
				}
			}
		}

		public EffectorLink[] effectorLinks = new EffectorLink[0];

		public void Apply(IKSolverFullBodyBiped solver, float weight)
		{
			for (int i = 0; i < effectorLinks.Length; i++)
			{
				effectorLinks[i].Apply(solver, weight, solver.GetRoot().rotation);
			}
		}

		public void Apply(IKSolverFullBodyBiped solver, float weight, Quaternion rotation)
		{
			for (int i = 0; i < effectorLinks.Length; i++)
			{
				effectorLinks[i].Apply(solver, weight, rotation);
			}
		}
	}
}
