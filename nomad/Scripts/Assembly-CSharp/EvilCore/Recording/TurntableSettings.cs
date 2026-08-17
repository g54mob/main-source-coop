using System;
using UnityEngine;

namespace EvilCore.Recording
{
	[Serializable]
	public class TurntableSettings
	{
		public float speed = 30f;

		public float pitch;

		public Vector3 offset = Vector3.zero;

		public float startAngle;

		public float endAngle = 360f;

		public bool autoRotate = true;

		public float parameterSmoothTime = 0.3f;
	}
}
