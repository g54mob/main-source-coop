using System.Collections.Generic;
using UnityEngine;

namespace PaintIn3D
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwHitScreenFill")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Hit Screen Fill")]
	public class CwHitScreenFill : CwHitScreen
	{
		[SerializeField]
		private float fillSpacing = 5f;

		public float FillSpacing
		{
			get
			{
				return fillSpacing;
			}
			set
			{
				fillSpacing = value;
			}
		}

		protected override void OnFingerUp(Link link)
		{
			Rect area = GetArea(link.History);
			area.xMin += fillSpacing * 0.25f;
			area.yMin += fillSpacing * 0.25f;
			area.xMax -= fillSpacing * 0.25f;
			area.yMax -= fillSpacing * 0.25f;
			if (!(fillSpacing > 0f) || !(area.width > 0f) || !(area.height > 0f) || link.History.Count <= 0)
			{
				return;
			}
			int num = Mathf.CeilToInt(area.width / fillSpacing);
			int num2 = Mathf.CeilToInt(area.height / fillSpacing);
			Vector2 vector = area.center - new Vector2(num, num2) * fillSpacing * 0.5f;
			for (int i = 0; i <= num2; i++)
			{
				for (int j = 0; j <= num; j++)
				{
					Vector2 vector2 = vector + new Vector2(j, i) * fillSpacing;
					if (Contains(link.History, vector2))
					{
						PaintAt(null, base.Connector.HitCache, vector2, vector2, preview: false, 1f, null);
					}
				}
			}
		}

		private static Rect GetArea(List<Vector2> points)
		{
			if (points != null && points.Count > 0)
			{
				Rect result = new Rect(points[0], Vector2.zero);
				{
					foreach (Vector2 point in points)
					{
						result.min = Vector2.Min(result.min, point);
						result.max = Vector2.Max(result.max, point);
					}
					return result;
				}
			}
			return default(Rect);
		}

		private static double LineSide(Vector2 a, Vector2 b, Vector2 p)
		{
			return (b.y - a.y) * (p.x - a.x) - (b.x - a.x) * (p.y - a.y);
		}

		private static bool Contains(List<Vector2> points, Vector2 xy)
		{
			Vector2 a = points[0];
			int num = 0;
			for (int num2 = points.Count - 1; num2 >= 0; num2--)
			{
				Vector2 vector = points[num2];
				if (a.y <= xy.y)
				{
					if (vector.y > xy.y && LineSide(a, vector, xy) > 0.0)
					{
						num++;
					}
				}
				else if (vector.y <= xy.y && LineSide(a, vector, xy) < 0.0)
				{
					num--;
				}
				a = vector;
			}
			return num != 0;
		}
	}
}
