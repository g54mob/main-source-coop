using UnityEngine;

namespace pworld.Scripts.PPhys
{
	public class PPhysRotatorWithRightOfTarget : PPhysRotation
	{
		public override Vector3 TargetUp => Vector3.Cross(Target, lookAt.right).normalized;
	}
}
