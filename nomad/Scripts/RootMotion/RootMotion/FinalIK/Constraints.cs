using System;
using UnityEngine;

namespace RootMotion.FinalIK
{
	[Serializable]
	public class Constraints
	{
		public Transform transform;

		public Transform target;

		public Vector3 positionOffset;

		public Vector3 position;

		[Range(0f, 1f)]
		public float positionWeight;

		public Vector3 rotationOffset;

		public Vector3 rotation;

		[Range(0f, 1f)]
		public float rotationWeight;

		private Vector3 defaultLocalPosition;

		private Quaternion defaultLocalRotation;

		public bool IsValid()
		{
			return transform != null;
		}

		public void Initiate(Transform transform)
		{
			this.transform = transform;
			position = transform.position;
			rotation = transform.eulerAngles;
			defaultLocalPosition = transform.localPosition;
			defaultLocalRotation = transform.localRotation;
		}

		public void FixTransforms()
		{
			transform.localPosition = defaultLocalPosition;
			transform.localRotation = defaultLocalRotation;
		}

		public void Update()
		{
			if (IsValid())
			{
				if (target != null)
				{
					position = target.position;
				}
				transform.position += positionOffset;
				if (positionWeight > 0f)
				{
					transform.position = Vector3.Lerp(transform.position, position, positionWeight);
				}
				if (target != null)
				{
					rotation = target.eulerAngles;
				}
				transform.rotation = Quaternion.Euler(rotationOffset) * transform.rotation;
				if (rotationWeight > 0f)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation), rotationWeight);
				}
			}
		}
	}
}
