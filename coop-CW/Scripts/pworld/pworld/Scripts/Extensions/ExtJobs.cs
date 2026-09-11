using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Jobs;

namespace pworld.Scripts.Extensions
{
	public static class ExtJobs
	{
		public static void Rotate(this TransformAccess _me, Vector3 _eulers, [DefaultValue("Space.Self")] Space _relativeTo)
		{
			Quaternion quaternion = Quaternion.Euler(_eulers.x, _eulers.y, _eulers.z);
			if (_relativeTo == Space.Self)
			{
				_me.localRotation *= quaternion;
			}
			else
			{
				_me.rotation *= Quaternion.Inverse(_me.rotation) * quaternion * _me.rotation;
			}
		}
	}
}
