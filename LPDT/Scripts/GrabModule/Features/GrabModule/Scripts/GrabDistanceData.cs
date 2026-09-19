using System;

namespace Features.GrabModule.Scripts
{
	[Serializable]
	public class GrabDistanceData
	{
		public float AdditionalGrabDistance;

		public float AdditionalMinJointDistance;

		public float AdditionalMaxJointDistance;

		public float AdditionalUnJoinSqrDistance;

		public bool IsWithGrabJointDistanceOverride;

		public float GrabJointDistanceOverride;
	}
}
