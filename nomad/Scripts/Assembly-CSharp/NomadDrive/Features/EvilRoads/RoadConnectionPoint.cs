using System;
using UnityEngine;

namespace NomadDrive.Features.EvilRoads
{
	[Serializable]
	public class RoadConnectionPoint
	{
		[SerializeField]
		private Vector3 position;

		[SerializeField]
		private Vector3 direction;

		[SerializeField]
		private EvilRoad ownerRoad;

		[SerializeField]
		private bool isStartPoint;

		[SerializeField]
		private int splineIndex;

		public Vector3 Position => position;

		public Vector3 Direction => direction;

		public EvilRoad OwnerRoad => ownerRoad;

		public bool IsStartPoint => isStartPoint;

		public int SplineIndex => splineIndex;

		public RoadConnectionPoint(Vector3 position, Vector3 direction, EvilRoad ownerRoad, bool isStartPoint, int splineIndex)
		{
			this.position = position;
			this.direction = direction;
			this.ownerRoad = ownerRoad;
			this.isStartPoint = isStartPoint;
			this.splineIndex = splineIndex;
		}

		public float DistanceTo(RoadConnectionPoint other)
		{
			return Vector3.Distance(position, other.position);
		}

		public bool IsCompatibleWith(RoadConnectionPoint other)
		{
			if (other == null)
			{
				return false;
			}
			if ((object)other.ownerRoad == ownerRoad)
			{
				return false;
			}
			return true;
		}

		public void UpdatePosition(Vector3 newPosition, Vector3 newDirection)
		{
			position = newPosition;
			direction = newDirection;
		}

		public void DrawDebug(Color color, float duration = 1f)
		{
			Debug.DrawRay(position, direction * 2f, color, duration);
			Vector3 vector = Vector3.right * 0.5f;
			Vector3 vector2 = Vector3.up * 0.5f;
			Vector3 vector3 = Vector3.forward * 0.5f;
			Debug.DrawLine(position - vector, position + vector, color, duration);
			Debug.DrawLine(position - vector2, position + vector2, color, duration);
			Debug.DrawLine(position - vector3, position + vector3, color, duration);
		}
	}
}
