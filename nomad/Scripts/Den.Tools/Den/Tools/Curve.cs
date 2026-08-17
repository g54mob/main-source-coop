using System;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	public class Curve
	{
		[Serializable]
		public class Node
		{
			public Vector2 pos;

			public float inTangent;

			public float outTangent;

			public bool linear;

			public Node()
			{
			}

			public Node(Vector2 pos)
			{
				this.pos = pos;
				inTangent = 0f;
				outTangent = 0f;
				linear = false;
			}

			public Node(Vector2 pos, float inTangent, float outTangent)
			{
				this.pos = pos;
				this.inTangent = inTangent;
				this.outTangent = outTangent;
				linear = false;
			}

			public Node(Node src)
			{
				pos = src.pos;
				inTangent = src.inTangent;
				outTangent = src.outTangent;
				linear = src.linear;
			}
		}

		public Node[] points = new Node[0];

		public float[] lut = new float[256];

		public Curve()
		{
		}

		public Curve(AnimationCurve animCurve)
		{
			ReadAnimCurve(animCurve);
			Refresh();
		}

		public Curve(Vector2 pos1, Vector2 pos2)
		{
			points = new Node[2]
			{
				new Node(pos1),
				new Node(pos2)
			};
			Refresh();
		}

		public Curve(Node[] points)
		{
			this.points = points;
		}

		public Curve(Curve src)
		{
			points = new Node[src.points.Length];
			Copy(src, this);
		}

		public static void Copy(Curve src, Curve dst)
		{
			if (dst.points.Length != src.points.Length)
			{
				dst.points = new Node[src.points.Length];
			}
			for (int i = 0; i < src.points.Length; i++)
			{
				dst.points[i] = new Node(src.points[i]);
			}
		}

		public Vector2[] GetPositions()
		{
			Vector2[] array = new Vector2[points.Length];
			for (int i = 0; i < points.Length; i++)
			{
				array[i] = points[i].pos;
			}
			return array;
		}

		public void ReadAnimCurve(AnimationCurve animCurve)
		{
			if (points.Length != animCurve.keys.Length)
			{
				points = new Node[animCurve.keys.Length];
			}
			for (int i = 0; i < points.Length; i++)
			{
				points[i] = new Node(new Vector2(animCurve.keys[i].time, animCurve.keys[i].value), animCurve.keys[i].inTangent, animCurve.keys[i].outTangent);
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
				Node node = points[i];
				animCurve.keys[i] = new Keyframe(node.pos.x, node.pos.y, node.inTangent, node.outTangent);
			}
		}

		public float EvaluatePrecise(float time)
		{
			if (time <= points[0].pos.x)
			{
				return points[0].pos.y;
			}
			if (time >= points[points.Length - 1].pos.x)
			{
				return points[points.Length - 1].pos.y;
			}
			for (int i = 0; i < points.Length - 1; i++)
			{
				if (time > points[i].pos.x && time <= points[i + 1].pos.x)
				{
					float num = points[i + 1].pos.x - points[i].pos.x;
					float x = (time - points[i].pos.x) / num;
					return EvaluatePrecise(points[i], points[i + 1], x);
				}
			}
			return 0f;
		}

		public static float EvaluatePrecise(Node prev, Node next, float x)
		{
			if (prev.linear && next.linear)
			{
				return prev.pos.y * (1f - x) + next.pos.y * x;
			}
			float num = next.pos.x - prev.pos.x;
			float num2 = 3f * x * x - 2f * x * x * x;
			float num3 = prev.pos.y * (1f - num2) + next.pos.y * num2;
			float num4 = x * (1f - x) * x;
			float num5 = x * (1f - x) * (1f - x);
			float num6 = prev.outTangent * num5 + (0f - next.inTangent) * num4;
			return num3 + num6 * num;
		}

		public void UpdateLut()
		{
			for (int i = 0; i < lut.Length; i++)
			{
				lut[i] = EvaluatePrecise(1f * (float)i / (float)(lut.Length - 1));
			}
		}

		public float EvaluateLuted(float input)
		{
			float num = 1f / (float)(lut.Length - 1);
			int num2 = (int)(input / num);
			int num3 = num2 + 1;
			float num4 = (float)num2 * num;
			float num5 = (input - num4) / num;
			float num6 = lut[num2];
			float num7 = lut[num3];
			return num6 * (1f - num5) + num7 * num5;
		}

		public void ResetNodeTangents(int num)
		{
			Vector2 pos = points[num].pos;
			float num2 = 0f;
			Vector2 pos2 = points[num - 1].pos;
			Vector2 pos3 = points[num + 1].pos;
			float num3 = (pos.y - pos2.y) / (pos.x - pos2.x);
			float num4 = (pos.y - pos3.y) / (pos.x - pos3.x);
			float num5 = (pos.x - pos2.x) / (pos3.x - pos2.x);
			num5 = (pos - pos2).magnitude / ((pos - pos3).magnitude + (pos - pos2).magnitude);
			num5 = 2f * num5 * num5 * num5 - 3f * num5 * num5 + 2f * num5;
			num2 = num3 * (1f - num5) + num4 * num5;
			points[num].outTangent = num2;
			points[num].inTangent = num2;
		}

		public void ResetFirstTangent()
		{
			if (points.Length == 2)
			{
				points[0].outTangent = 1f;
				return;
			}
			Vector2 pos = points[0].pos;
			Vector2 pos2 = points[1].pos;
			float num = pos2.x - pos.x;
			Vector2 vector = new Vector2(1f, points[1].inTangent);
			Vector2 vector2 = pos2 - vector * num / 2f;
			float outTangent = (pos.y - vector2.y) / (pos.x - vector2.x);
			points[0].outTangent = outTangent;
		}

		public void ResetLastTangent()
		{
			if (points.Length == 2)
			{
				points[points.Length - 1].inTangent = 1f;
				return;
			}
			Vector2 pos = points[points.Length - 1].pos;
			Vector2 pos2 = points[points.Length - 2].pos;
			float num = pos2.x - pos.x;
			Vector2 vector = new Vector2(1f, points[1].inTangent);
			Vector2 vector2 = pos2 - vector * num / 2f;
			float inTangent = (vector2.y - pos.y) / (vector2.x - pos.x);
			points[points.Length - 1].inTangent = inTangent;
		}

		public void Refresh(bool updateLut = true)
		{
			points[0].linear = true;
			points[points.Length - 1].linear = true;
			for (int i = 1; i < points.Length - 1; i++)
			{
				ResetNodeTangents(i);
			}
			ResetFirstTangent();
			ResetLastTangent();
			if (updateLut)
			{
				UpdateLut();
			}
		}
	}
}
