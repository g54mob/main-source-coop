using System;
using UnityEngine;
using pworld.Scripts.PPhys.Bursted.Handlers;

namespace pworld.Scripts.PPhys.Bursted.Springs
{
	public class PSpringLocal : PBurstSpringWorkerPosition
	{
		public Vector3 target;

		public Transform inLocalOf;

		public override Vector3 Target => inLocalOf.TransformPoint(target);

		private void Awake()
		{
			if (inLocalOf == null)
			{
				inLocalOf = base.transform.parent;
			}
			if (inLocalOf == null)
			{
				throw new Exception("Gotta have a parent");
			}
		}

		public void Update()
		{
			UpdateTarget();
		}
	}
}
