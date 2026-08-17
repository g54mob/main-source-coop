using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.Rigging
{
	[Serializable]
	public class Bone
	{
		[Tooltip("    Should the object be positioned between two points. Also affects position besides rotation.")]
		public bool doubleSided;

		[Tooltip("    Should the object be rotated to look at the target?")]
		public bool lookAtTarget = true;

		[Tooltip("    Should the object be stretched between pivot and target?")]
		public bool stretchToTarget = true;

		[Tooltip("    The transform that represents the lookAtTarget and stretch target.")]
		public Transform targetTransform;

		[Tooltip("    Second target. Object will be stretched positioned between targetTransform and targetTransformB if doubleSided is\r\n    true.")]
		public Transform targetTransformB;

		[Tooltip("    The transform that represents the bone.")]
		public Transform thisTransform;

		private float _initDistance;

		private float _initZScale;

		public void Initialize()
		{
			_initDistance = Vector3.Distance(thisTransform.position, targetTransform.position);
			_initZScale = thisTransform.localScale.z;
		}

		public void Update(Vector3 forward, Vector3 up)
		{
			if (doubleSided)
			{
				Vector3 position = (targetTransform.position + targetTransformB.position) / 2f;
				thisTransform.position = position;
				thisTransform.LookAt(targetTransform, up);
				return;
			}
			if (lookAtTarget)
			{
				Vector3 eulerAngles = Quaternion.LookRotation(targetTransform.position - thisTransform.position, up).eulerAngles;
				thisTransform.rotation = Quaternion.Euler(eulerAngles);
			}
			if (stretchToTarget && _initDistance != 0f)
			{
				float z = Vector3.Distance(thisTransform.position, targetTransform.position) / _initDistance * _initZScale;
				Vector3 localScale = thisTransform.localScale;
				thisTransform.localScale = new Vector3(localScale.x, localScale.y, z);
			}
		}
	}
}
