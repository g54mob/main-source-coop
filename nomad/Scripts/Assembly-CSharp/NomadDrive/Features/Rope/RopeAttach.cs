using System;
using UnityEngine;

namespace NomadDrive.Features.Rope
{
	[Serializable]
	public class RopeAttach
	{
		public Transform obj;

		public Vector3 rot;

		[Range(0f, 1f)]
		public float alpha;
	}
}
