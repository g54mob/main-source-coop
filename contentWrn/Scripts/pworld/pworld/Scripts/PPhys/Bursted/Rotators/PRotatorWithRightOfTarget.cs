using UnityEngine;

namespace pworld.Scripts.PPhys.Bursted.Rotators
{
	public class PRotatorWithRightOfTarget : PRotatorLookAt
	{
		public override Vector3 TargetUp => Vector3.Cross(LookAtDir, lookAt.right).normalized;
	}
}
