using UnityEngine;

namespace pworld.Scripts.PPhys
{
	public class PPhysScale : PPhysSpringBase
	{
		public override Vector3 Current
		{
			get
			{
				return base.transform.lossyScale;
			}
			set
			{
				base.transform.localScale = base.transform.parent.InverseTransformVector(value);
			}
		}
	}
}
