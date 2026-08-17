using UnityEngine;

namespace FishNet.Object.Prediction
{
	internal struct SetInterpolationSmootherData
	{
		public Transform GraphicalObject;

		public byte Interpolation;

		public NetworkObject NetworkObject;

		public bool SmoothPosition;

		public bool SmoothRotation;

		public bool SmoothScale;

		public float TeleportThreshold;
	}
}
