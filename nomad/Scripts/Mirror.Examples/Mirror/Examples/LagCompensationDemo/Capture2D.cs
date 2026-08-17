using UnityEngine;

namespace Mirror.Examples.LagCompensationDemo
{
	public struct Capture2D : Capture
	{
		public Vector2 position;

		public Vector2 size;

		public double timestamp { get; set; }

		public Capture2D(double timestamp, Vector2 position, Vector2 size)
		{
			this.timestamp = timestamp;
			this.position = position;
			this.size = size;
		}

		public void DrawGizmo()
		{
			Gizmos.DrawWireCube(position, size);
		}

		public static Capture2D Interpolate(Capture2D from, Capture2D to, double t)
		{
			return new Capture2D(0.0, Vector2.LerpUnclamped(from.position, to.position, (float)t), Vector2.LerpUnclamped(from.size, to.size, (float)t));
		}

		public override string ToString()
		{
			return $"(time={timestamp} pos={position} size={size})";
		}
	}
}
