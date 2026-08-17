using System;

namespace EvilCore.Recording
{
	[Serializable]
	public class PanTiltSettings
	{
		public float startPan;

		public float endPan = 90f;

		public float startTilt;

		public float endTilt;

		public float speed = 1f;
	}
}
