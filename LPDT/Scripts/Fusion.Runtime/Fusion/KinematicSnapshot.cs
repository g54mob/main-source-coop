using UnityEngine;

namespace Fusion
{
	public struct KinematicSnapshot
	{
		public Vector3 WorldPosition;

		public Quaternion WorldRotation;

		public Vector3 LinearVelocity;

		public Vector3 AngularVelocity;

		public bool IsSleeping;

		public override readonly string ToString()
		{
			return $"[Position: {WorldPosition}, Rotation: {WorldRotation}, Linear Velocity: {LinearVelocity}, Angular Velocity: {AngularVelocity}, IsSleeping: {IsSleeping}]";
		}
	}
}
