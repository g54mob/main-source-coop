using System;
using NWH.Common.Vehicles;

namespace NWH.WheelController3D
{
	[Serializable]
	public class _Wheel
	{
		public bool power;

		public bool steer;

		public bool handbrake;

		public WheelUAPI wheelUAPI;
	}
}
