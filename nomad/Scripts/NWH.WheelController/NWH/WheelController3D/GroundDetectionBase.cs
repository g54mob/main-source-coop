using UnityEngine;

namespace NWH.WheelController3D
{
	[RequireComponent(typeof(WheelController))]
	public abstract class GroundDetectionBase : MonoBehaviour
	{
		public abstract bool WheelCast(in Vector3 origin, in Vector3 direction, in float distance, in float radius, in float width, ref WheelHit result, LayerMask layerMask);
	}
}
