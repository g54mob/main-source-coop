using System;
using UnityEngine;

namespace EvilCore.Recording
{
	[Serializable]
	public class OrbitSettings
	{
		public Vector3 targetPosition;

		public float radius = 5f;

		public float height = 2f;

		public float startAngle;

		public float endAngle = 360f;

		public float speed = 30f;

		public bool lookAtTarget = true;

		public Vector3 targetOffset;

		public bool autoRotate = true;

		public float parameterSmoothTime = 0.3f;
	}
}
