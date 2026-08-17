using Mirror;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Networking
{
	public struct VehicleSnapshot : Snapshot
	{
		public Vector3 position;

		public Quaternion rotation;

		public Vector3 velocity;

		public Vector3 angularVelocity;

		public double remoteTime { get; set; }

		public double localTime { get; set; }

		public VehicleSnapshot(double remoteTime, double localTime, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
		{
			this.remoteTime = remoteTime;
			this.localTime = localTime;
			this.position = position;
			this.rotation = rotation;
			this.velocity = velocity;
			this.angularVelocity = angularVelocity;
		}

		public static VehicleSnapshot Interpolate(VehicleSnapshot from, VehicleSnapshot to, double t)
		{
			return new VehicleSnapshot(0.0, 0.0, Vector3.LerpUnclamped(from.position, to.position, (float)t), Quaternion.SlerpUnclamped(from.rotation, to.rotation, (float)t), Vector3.LerpUnclamped(from.velocity, to.velocity, (float)t), Vector3.LerpUnclamped(from.angularVelocity, to.angularVelocity, (float)t));
		}
	}
}
