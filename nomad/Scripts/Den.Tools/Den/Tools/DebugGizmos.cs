using System;
using System.Collections.Generic;
using Den.Tools.Matrices;
using Den.Tools.Tasks;
using UnityEngine;

namespace Den.Tools
{
	public static class DebugGizmos
	{
		public class GizmoGroup
		{
			public bool enabled = true;

			public List<IGizmo> gizmos = new List<IGizmo>();

			public bool colorOverride;

			public Color color;
		}

		public interface IGizmo
		{
			Color Color { get; set; }

			void Draw();
		}

		private struct LineGizmo : IGizmo
		{
			public Vector3 from;

			public Vector3 to;

			public Color Color { get; set; }

			public void Draw()
			{
			}
		}

		private struct PolyLineGizmo : IGizmo
		{
			public Vector3[] points;

			public Color Color { get; set; }

			public void Draw()
			{
			}
		}

		private struct SphereGizmo : IGizmo
		{
			public Vector3 pos;

			public float radius;

			public Color Color { get; set; }

			public void Draw()
			{
			}
		}

		private struct RectGizmo : IGizmo
		{
			public Vector2D pos;

			public Vector2D size;

			public Color Color { get; set; }

			public void Draw()
			{
			}
		}

		private struct GridGizmo : IGizmo
		{
			public Vector3 pos;

			public Vector3 size;

			public int cellsX;

			public int cellsZ;

			public Color Color { get; set; }

			public void Draw()
			{
			}
		}

		private struct DotGizmo : IGizmo
		{
			public Vector3 pos;

			public float size;

			public Color Color { get; set; }

			public void Draw()
			{
			}
		}

		private struct CircleGizmo : IGizmo
		{
			public Vector3 pos;

			public float radius;

			public Color Color { get; set; }

			public void Draw()
			{
			}
		}

		private struct MultiLineGizmo : IGizmo
		{
			public (Vector3, Vector3)[] lines;

			public Color Color { get; set; }

			public void Draw()
			{
			}
		}

		private struct LabelGizmo : IGizmo
		{
			public Vector3 pos;

			public string label;

			public Color Color { get; set; }

			public void Draw()
			{
			}
		}

		public static Dictionary<string, GizmoGroup> allGizmos = new Dictionary<string, GizmoGroup>();

		public static Action<Matrix, string> onPreviewMatrix;

		public static Action<Matrix, string> onPreviewMatrixAdd;

		public static void DrawLine(string name, Vector3 from, Vector3 to, Color color = default(Color), bool additive = false)
		{
			LineGizmo lineGizmo = new LineGizmo
			{
				from = from,
				to = to
			};
			AddGizmo(name, lineGizmo, color, additive);
		}

		public static void DrawRay(string name, Vector3 pos, Vector3 dir, Color color = default(Color), bool additive = false)
		{
			LineGizmo lineGizmo = new LineGizmo
			{
				from = pos,
				to = pos + dir
			};
			AddGizmo(name, lineGizmo, color, additive);
		}

		public static void DrawPolyLine(string name, Vector3[] points, Color color = default(Color), bool additive = false)
		{
			PolyLineGizmo polyLineGizmo = new PolyLineGizmo
			{
				points = points
			};
			AddGizmo(name, polyLineGizmo, color, additive);
		}

		public static void DrawSphere(string name, Vector3 pos, float radius, Color color = default(Color), bool additive = false)
		{
			SphereGizmo sphereGizmo = new SphereGizmo
			{
				pos = pos,
				radius = radius
			};
			AddGizmo(name, sphereGizmo, color, additive);
		}

		public static void DrawDot(string name, Vector3 pos, float size, Color color = default(Color), bool additive = false)
		{
			DotGizmo dotGizmo = new DotGizmo
			{
				pos = pos,
				size = size
			};
			AddGizmo(name, dotGizmo, color, additive);
		}

		public static void DrawCircle(string name, Vector3 pos, float radius, Color color = default(Color), bool additive = false)
		{
			CircleGizmo circleGizmo = new CircleGizmo
			{
				pos = pos,
				radius = radius
			};
			AddGizmo(name, circleGizmo, color, additive);
		}

		public static void DrawGrid(string name, Vector3 worldPos, Vector3 worldSize, int cellsX, int cellsZ, Color color = default(Color), bool additive = false)
		{
			GridGizmo gridGizmo = new GridGizmo
			{
				pos = worldPos,
				size = worldSize,
				cellsX = cellsX,
				cellsZ = cellsZ
			};
			AddGizmo(name, gridGizmo, color, additive);
		}

		public static void DrawGrid(string name, CoordRect rect, Vector3 worldPos, Vector3 worldSize, Color color = default(Color), bool additive = false)
		{
			GridGizmo gridGizmo = new GridGizmo
			{
				pos = worldPos,
				size = worldSize,
				cellsX = rect.size.x,
				cellsZ = rect.size.z
			};
			AddGizmo(name, gridGizmo, color, additive);
		}

		public static void DrawRect(string name, Vector2D worldPos, Vector2D worldSize, Color color = default(Color), bool additive = false)
		{
			RectGizmo rectGizmo = new RectGizmo
			{
				pos = worldPos,
				size = worldSize
			};
			AddGizmo(name, rectGizmo, color, additive);
		}

		public static void DrawRect(string name, Vector3 worldPos, Vector3 worldSize, Color color = default(Color), bool additive = false)
		{
			RectGizmo rectGizmo = new RectGizmo
			{
				pos = new Vector2D(worldPos.x, worldPos.z),
				size = new Vector2D(worldSize.x, worldSize.z)
			};
			AddGizmo(name, rectGizmo, color, additive);
		}

		public static void DrawMultipleLines(string name, (Vector3, Vector3)[] lines, Color color = default(Color), bool additive = false)
		{
			MultiLineGizmo multiLineGizmo = new MultiLineGizmo
			{
				lines = lines
			};
			AddGizmo(name, multiLineGizmo, color, additive);
		}

		public static void DrawLabel(string name, Vector3 worldPos, string label, Color color = default(Color), bool additive = false)
		{
			LabelGizmo labelGizmo = new LabelGizmo
			{
				pos = worldPos,
				label = label
			};
			AddGizmo(name, labelGizmo, color, additive);
		}

		public static void AddGizmo(string name, IGizmo gizmo, Color color, bool additive = false)
		{
			if (color.r == 0f && color.g == 0f && color.b == 0f && color.a == 0f)
			{
				color = Color.white;
			}
			gizmo.Color = color;
			if (allGizmos.TryGetValue(name, out var value))
			{
				if (!additive)
				{
					value.gizmos.Clear();
				}
				value.gizmos.Add(gizmo);
				allGizmos[name] = value;
			}
			else
			{
				value = new GizmoGroup();
				value.gizmos.Add(gizmo);
				allGizmos.Add(name, value);
			}
		}

		public static void Clear(string name)
		{
			allGizmos.Remove(name);
		}

		public static void Clear()
		{
			allGizmos.Clear();
		}

		public static void ToMatrixPreview(this Matrix matrix, string name = null)
		{
			onPreviewMatrix?.Invoke(matrix, name);
		}

		public static void ToMatrixPreviewCopy(this Matrix matrix, string name = null)
		{
			onPreviewMatrix?.Invoke(new Matrix(matrix), name);
		}

		public static void ToMatrixPreviewAdd(this Matrix matrix, string name = null)
		{
			onPreviewMatrixAdd?.Invoke(new Matrix(matrix), name);
		}

		public static void ToMatrixPreviewMainthread(this Matrix matrix, string name = null)
		{
			CoroutineManager.Enqueue(delegate
			{
				onPreviewMatrix?.Invoke(matrix, name);
			});
		}

		public static void ToMatrixPreviewCopyMainthread(this Matrix matrix, string name = null)
		{
			Matrix m2 = new Matrix(matrix);
			CoroutineManager.Enqueue(delegate
			{
				onPreviewMatrix?.Invoke(m2, name);
			});
		}

		public static void ToMatrixPreviewAddMainthread(this Matrix matrix, string name = null)
		{
			CoroutineManager.Enqueue(delegate
			{
				onPreviewMatrixAdd?.Invoke(new Matrix(matrix), name);
			});
		}

		public static Vector3 PositionHandle(Vector3 src)
		{
			return src;
		}

		public static Vector2D PositionHandle(Vector2D src)
		{
			return (Vector2D)PositionHandle((Vector3)src);
		}
	}
}
