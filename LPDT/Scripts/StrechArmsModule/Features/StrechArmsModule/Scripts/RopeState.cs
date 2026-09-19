using System;
using System.Runtime.InteropServices;
using Fusion;
using UnityEngine;

namespace Features.StrechArmsModule.Scripts
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 48)]
	[NetworkStructWeaved(12)]
	public struct RopeState : INetworkStruct
	{
		[FieldOffset(0)]
		public Vector3 position;

		[FieldOffset(12)]
		public Vector3 velocity;

		[FieldOffset(24)]
		public Vector3 externalForces;

		[FieldOffset(36)]
		public Vector3 externalTorques;
	}
}
