using System;
using UnityEngine;

namespace EvilCore.Recording
{
	[Serializable]
	public class DollySettings
	{
		public DollyDirection direction;

		public float speed = 2f;

		public bool lookAtTarget;

		public Vector3 lookAtPosition;
	}
}
