using System;

namespace EvilCore.Recording
{
	[Serializable]
	public class DirectorSequenceEntry
	{
		public int cameraIndex;

		public float holdDuration = 5f;

		public TransitionType transitionType = TransitionType.Blend;

		public float blendDuration = 1f;
	}
}
