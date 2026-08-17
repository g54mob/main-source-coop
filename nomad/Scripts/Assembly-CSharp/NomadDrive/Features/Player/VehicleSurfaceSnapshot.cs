using Mirror;
using UnityEngine;

namespace NomadDrive.Features.Player
{
	public struct VehicleSurfaceSnapshot : Snapshot
	{
		public Vector3 localPosition;

		public Quaternion localRotation;

		public double remoteTime { get; set; }

		public double localTime { get; set; }

		public VehicleSurfaceSnapshot(double remoteTime, double localTime, Vector3 localPosition, Quaternion localRotation)
		{
			this.remoteTime = remoteTime;
			this.localTime = localTime;
			this.localPosition = localPosition;
			this.localRotation = localRotation;
		}

		public static VehicleSurfaceSnapshot Interpolate(VehicleSurfaceSnapshot from, VehicleSurfaceSnapshot to, double t)
		{
			return new VehicleSurfaceSnapshot(0.0, 0.0, Vector3.LerpUnclamped(from.localPosition, to.localPosition, (float)t), Quaternion.SlerpUnclamped(from.localRotation, to.localRotation, (float)t));
		}
	}
}
