using UnityEngine;

namespace pworld.Scripts.Extensions
{
	public static class ExtQuaternion
	{
		public static Quaternion LookRotation(Vector3 tarF, Vector3 tarU)
		{
			Quaternion me = Quaternion.identity;
			me.PLookRotation(tarF, tarU);
			return me;
		}

		public static bool PLookRotation(this ref Quaternion me, Vector3 tarF, Vector3 tarU)
		{
			if (tarF.sqrMagnitude == 0f || tarU.sqrMagnitude == 0f)
			{
				return false;
			}
			me = Quaternion.LookRotation(tarF, tarU);
			return true;
		}
	}
}
