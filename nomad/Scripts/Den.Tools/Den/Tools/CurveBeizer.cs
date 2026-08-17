using UnityEngine;

namespace Den.Tools
{
	public class CurveBeizer
	{
		public class Node
		{
			public enum TangentMode
			{
				Linear = 0,
				Smooth = 1,
				Correlated = 2,
				Manual = 3
			}

			public Vector2 pos;

			public Vector2 inTangent;

			public Vector2 outTangent;

			public TangentMode tangentMode = TangentMode.Smooth;

			public Node()
			{
			}

			public Node(Vector2 pos)
			{
				this.pos = pos;
			}

			public Node(Vector2 pos, Vector2 inTangent, Vector2 outTangent)
			{
				this.pos = pos;
				this.inTangent = inTangent;
				this.outTangent = outTangent;
			}

			public static Vector2 GetPoint(Node start, Node end, float p)
			{
				float num = 1f - p;
				return num * num * num * start.pos + 3f * p * num * num * (start.pos + start.outTangent) + 3f * p * p * num * (end.pos + end.inTangent) + p * p * p * end.pos;
			}
		}

		public Node[] points = new Node[0];

		public float[] lut = new float[256];

		public CurveBeizer()
		{
		}

		public CurveBeizer(Vector2 pos1, Vector2 pos2)
		{
			points = new Node[2]
			{
				new Node(pos1),
				new Node(pos2)
			};
		}

		public CurveBeizer(Node[] points)
		{
			this.points = points;
		}

		public float Evaluate(float input)
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

		public float[] GenerateLut(int steps = 256)
		{
			float[] result = new float[steps];
			GenerateLut(result);
			return result;
		}

		public float[] GenerateLut()
		{
			GenerateLut(lut);
			return lut;
		}

		public float[] GenerateLut(float[] lut)
		{
			int num = lut.Length;
			Vector2[][] array = new Vector2[points.Length][];
			int num2 = num / points.Length + 3;
			if (num2 > num)
			{
				num2 = num;
			}
			for (int i = 0; i < points.Length - 1; i++)
			{
				Vector2[] array2 = (array[i] = new Vector2[num2]);
				Node node = points[i];
				Node node2 = points[i + 1];
				for (int j = 0; j < num2; j++)
				{
					float num3 = 1f * (float)j / (float)(num2 - 1);
					float num4 = 1f - num3;
					array2[j] = num4 * num4 * num4 * node.pos + 3f * num3 * num4 * num4 * (node.pos + node.outTangent) + 3f * num3 * num3 * num4 * (node2.pos + node2.inTangent) + num3 * num3 * num3 * node2.pos;
				}
			}
			for (int k = 0; k < num - 1; k++)
			{
				float num5 = 1f * (float)k / (float)(num - 1);
				int num6 = 0;
				for (int l = 0; l < points.Length - 1; l++)
				{
					if (num5 > points[l].pos.x && num5 <= points[l + 1].pos.x)
					{
						num6 = l;
						break;
					}
				}
				Vector2[] array3 = array[num6];
				int num7 = 0;
				for (int m = 0; m < array3.Length - 1; m++)
				{
					if (num5 > array3[m].x && num5 <= array3[m + 1].x)
					{
						num7 = m;
						break;
					}
				}
				float x = array3[num7].x;
				float num8 = array3[num7 + 1].x - x;
				float num9 = (num5 - x) / num8;
				float num10 = array3[num7].y * (1f - num9) + array3[num7 + 1].y * num9;
				if (num5 < points[0].pos.x)
				{
					num10 = points[0].pos.y;
				}
				if (num5 > points[points.Length - 1].pos.x)
				{
					num10 = points[points.Length - 1].pos.y;
				}
				lut[k] = num10;
			}
			lut[lut.Length - 1] = points[points.Length - 1].pos.y;
			return lut;
		}

		public void RefreshNodeTangents(int num)
		{
			Node node = points[num];
			Vector2 vector = ((num != 0) ? points[num - 1].pos : points[num].pos);
			Vector2 vector2 = ((num != points.Length - 1) ? points[num + 1].pos : points[num].pos);
			Vector2 pos = points[num].pos;
			if (num == 0 || num == points.Length - 1)
			{
				node.inTangent = new Vector2(0f, 0f);
				node.outTangent = new Vector2(0f, 0f);
			}
			if (node.tangentMode == Node.TangentMode.Linear)
			{
				node.inTangent = vector * 0.333f + node.pos * 0.667f - node.pos;
				node.outTangent = vector2 * 0.333f + node.pos * 0.667f - node.pos;
			}
			if (node.tangentMode == Node.TangentMode.Smooth)
			{
				Vector2 vector3 = (vector - pos).normalized + pos;
				Vector2 vector4 = (vector2 - pos).normalized + pos;
				Vector2 normalized = (vector4 - vector3).normalized;
				Vector2 normalized2 = (vector3 - vector4).normalized;
				node.outTangent = normalized * (vector2.x - pos.x) / 2f;
				node.inTangent = normalized2 * (pos.x - vector.x) / 2f;
			}
		}

		public void Refresh()
		{
			for (int i = 0; i < points.Length; i++)
			{
				RefreshNodeTangents(i);
			}
			GenerateLut();
		}

		public void ReadAnimCurve(AnimationCurve animCurve)
		{
			if (points.Length != animCurve.keys.Length)
			{
				points = new Node[animCurve.keys.Length];
			}
			for (int i = 0; i < points.Length; i++)
			{
				points[i] = new Node(new Vector2(animCurve.keys[i].time, animCurve.keys[i].value), new Vector2(-0.1f, 0f), new Vector2(0.1f, 0f));
			}
			Refresh();
		}
	}
}
