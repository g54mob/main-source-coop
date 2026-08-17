using System;
using UnityEngine;

namespace EvilCore.Recording
{
	[Serializable]
	public class StaticSettings
	{
		public bool useLookAtTarget;

		public Vector3 lookAtPosition;

		public bool smoothLookAt;

		public float smoothSpeed = 2f;
	}
}
