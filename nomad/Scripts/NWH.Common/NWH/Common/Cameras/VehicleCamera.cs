using UnityEngine;

namespace NWH.Common.Cameras
{
	public class VehicleCamera : MonoBehaviour
	{
		[Tooltip("Transform that this script is targeting. Can be left empty if head movement is not being used.")]
		public Transform target;

		public virtual void Awake()
		{
			if (target == null)
			{
				target = GetComponentInParent<Rigidbody>()?.transform;
			}
		}
	}
}
