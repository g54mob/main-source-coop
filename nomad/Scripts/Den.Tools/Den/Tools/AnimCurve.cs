using System;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	public class AnimCurve
	{
		[Serializable]
		public struct Point
		{
			public float time;

			public float val;

			public float inTangent;

			public float outTangent;

			public Point(Keyframe key)
			{
				time = key.time;
				val = key.value;
				inTangent = key.inTangent;
				outTangent = key.outTangent;
			}

			public Point(float time, float val, float inTangent, float outTangent)
			{
				this.time = time;
				this.val = val;
				this.inTangent = inTangent;
				this.outTangent = outTangent;
			}
		}

		public Point[] points = new Point[0];

		public AnimCurve()
		{
		}

		public AnimCurve(AnimationCurve animCurve)
		{
			ReadAnimCurve(animCurve);
		}

		public AnimCurve(Point[] points)
		{
			this.points = points;
		}

		public void ReadAnimCurve(AnimationCurve animCurve)
		{
			if (points.Length != animCurve.keys.Length)
			{
				points = new Point[animCurve.keys.Length];
			}
			for (int i = 0; i < points.Length; i++)
			{
				points[i] = new Point(animCurve.keys[i]);
			}
		}

		public void WriteToAnimCurve(AnimationCurve animCurve)
		{
			if (points.Length != animCurve.keys.Length)
			{
				animCurve.keys = new Keyframe[points.Length];
			}
			for (int i = 0; i < points.Length; i++)
			{
				Point point = points[i];
				animCurve.keys[i] = new Keyframe(point.time, point.val, point.inTangent, point.outTangent);
			}
		}

		public float Evaluate(float time, int segment = -1)
		{
			if (time <= points[0].time)
			{
				return points[0].val;
			}
			if (time >= points[points.Length - 1].time)
			{
				return points[points.Length - 1].val;
			}
			for (int i = 0; i < points.Length - 1; i++)
			{
				if (time > points[i].time && time <= points[i + 1].time)
				{
					Point point = points[i];
					Point point2 = points[i + 1];
					float num = point2.time - point.time;
					float num2 = (time - point.time) / num;
					float num3 = num2 * num2;
					float num4 = num3 * num2;
					float num5 = 2f * num4 - 3f * num3 + 1f;
					float num6 = num4 - 2f * num3 + num2;
					float num7 = num4 - num3;
					float num8 = -2f * num4 + 3f * num3;
					return num5 * point.val + num6 * point.outTangent * num + num7 * point2.inTangent * num + num8 * point2.val;
				}
			}
			return 0f;
		}
	}
}
