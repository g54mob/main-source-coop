using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.Aerodynamics
{
	[Serializable]
	public class DownforcePoint
	{
		[Tooltip("Maximim force in [N] that can be applied as a result of downforce.\r\nPutting in a too large value will make the vehicle bottom out at high speeds if suspension is too soft.")]
		public float maxForce;

		[Tooltip("Position relative to the vehicle at which downforce will be applied. Marked by red arrow gizmo.\r\nY component should be at about the spring anchor height (i.e. WheelController position).")]
		public Vector3 position;
	}
}
