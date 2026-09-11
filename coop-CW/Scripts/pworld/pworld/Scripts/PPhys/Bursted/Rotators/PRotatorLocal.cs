using System;
using UnityEngine;
using pworld.Scripts.PPhys.Bursted.Handlers;

namespace pworld.Scripts.PPhys.Bursted.Rotators
{
	public class PRotatorLocal : PBurstSpringWorkerRotation
	{
		public Vector3 mForward = Vector3.forward;

		public Vector3 mUp = Vector3.up;

		public Transform parent;

		public override Quaternion Target
		{
			get
			{
				Vector3 upwards = parent.TransformDirection(mUp);
				return Quaternion.LookRotation(parent.TransformDirection(mForward), upwards);
			}
		}

		public void Awake()
		{
			if (parent == null && base.transform.parent == null)
			{
				throw new Exception("parent is null");
			}
			if (parent == null)
			{
				parent = base.transform.parent;
			}
		}

		private void Update()
		{
			UpdateTarget();
		}
	}
}
