using System;
using UnityEngine;

namespace Features.AIModule.Scripts.CustomNavMesh
{
	[Serializable]
	public class LinkMoverData
	{
		public NavMeshAreas NavMeshAreas;

		public OffMeshLinkMoveMethod LinkMoveMethod;

		public AnimationCurve Curve = new AnimationCurve();

		public float Duration;

		public float Height;
	}
}
