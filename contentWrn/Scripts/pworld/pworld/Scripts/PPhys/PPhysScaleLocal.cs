using UnityEngine;

namespace pworld.Scripts.PPhys
{
	public class PPhysScaleLocal : PPhysSpringBase
	{
		public override Vector3 Current
		{
			get
			{
				return base.transform.localScale;
			}
			set
			{
				base.transform.localScale = value;
			}
		}
	}
}
