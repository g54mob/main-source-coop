using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Petter.TitleCard
{
	public class Figure
	{
		public class GhostPoint
		{
			public Vector2 point;

			public Vector3 debugWorldPosition;

			public GhostPoint(Vector2 point)
			{
				this.point = point;
			}
		}

		public List<Vector2> points = new List<Vector2>();

		public GhostPoint ghostPoint;

		public float minPointDistance = 0.001f;

		public float radius = 40f;

		public Vector4 color = Color.white;

		public List<Vector3> debugWorldPositions = new List<Vector3>();

		public Vector2 LastPoint
		{
			get
			{
				List<Vector2> list = points;
				return list[list.Count - 1];
			}
		}

		public void DrawPoint(Vector2 uvCoord, Vector3 debugWorldPos)
		{
			if (Vector2.Distance(LastPoint, uvCoord) > minPointDistance)
			{
				points.Add(uvCoord);
				debugWorldPositions.Add(debugWorldPos);
			}
			ghostPoint = new GhostPoint(uvCoord)
			{
				debugWorldPosition = debugWorldPos
			};
		}

		public List<Vector2> GetPoints()
		{
			List<Vector2> list = new List<Vector2>(points);
			if (ghostPoint != null)
			{
				list.Add(ghostPoint.point);
			}
			return list;
		}

		public void FinishFigure()
		{
			if (ghostPoint != null)
			{
				points.Add(ghostPoint.point);
			}
			ghostPoint = null;
		}
	}
}
