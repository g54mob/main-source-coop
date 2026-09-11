using UnityEngine;

namespace pworld.Scripts.PPhys
{
	public class PPhysPositionalLocal : PPhysSpringBase
	{
		public override Vector3 Current
		{
			get
			{
				return base.transform.localPosition;
			}
			set
			{
				base.transform.localPosition = value;
			}
		}
	}
}
