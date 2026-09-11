using UnityEngine;
using pworld.Scripts.Extensions;

namespace pworld.Scripts.PPhys
{
	public class PPhysScaleAround : PPhysSpringBase
	{
		public Transform around;

		public Transform affected;

		public override Vector3 Current
		{
			get
			{
				return base.transform.localScale;
			}
			set
			{
				base.transform.PScaleAround(base.transform.InverseTransformPoint(around.position), value);
				affected.PScaleAround(affected.InverseTransformPoint(around.position), value);
			}
		}
	}
}
