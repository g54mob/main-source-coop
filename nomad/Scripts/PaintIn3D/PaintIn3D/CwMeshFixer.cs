using System;
using System.Collections.Generic;
using PaintCore;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace PaintIn3D
{
	[ExecuteInEditMode]
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwMeshFixer")]
	public class CwMeshFixer : ScriptableObject
	{
		[Serializable]
		public class Pair
		{
			public Mesh Source;

			public Mesh Output;
		}

		private class Ring
		{
			public List<Edge> Edges = new List<Edge>();

			public Edge GetEdge(int index)
			{
				if (index < 0)
				{
					index = Edges.Count - 1;
				}
				else if (index >= Edges.Count)
				{
					index = 0;
				}
				return Edges[index];
			}

			public bool IsClockwise(Vector2[] coords)
			{
				float num = 0f;
				for (int i = 0; i < Edges.Count; i++)
				{
					Vector2 vector = coords[Edges[i].IndexA];
					Vector2 vector2 = coords[Edges[i].IndexB];
					num += (vector2.x - vector.x) * (vector2.y + vector.y);
				}
				return num > 0f;
			}
		}

		private class Edge
		{
			public bool Used;

			public int IndexA;

			public int IndexB;
		}

		private class Insertion
		{
			public int Index;

			public int NewIndex;

			public Vector2 NewCoord;
		}

		[SerializeField]
		private Mesh source;

		[SerializeField]
		private Mesh mesh;

		[SerializeField]
		private List<Pair> meshes;

		[SerializeField]
		private CwCoord coord;

		[SerializeField]
		private bool generateUV;

		[SerializeField]
		[Range(0.01f, 1f)]
		private float angleError = 0.08f;

		[SerializeField]
		[Range(0.01f, 1f)]
		private float areaError = 0.15f;

		[SerializeField]
		[Range(10f, 180f)]
		private float hardAngle = 88f;

		[SerializeField]
		[Range(0.0001f, 0.1f)]
		private float packMargin = 0.00390625f;

		[SerializeField]
		private bool fixOverflow = true;

		[SerializeField]
		private bool fixSeams = true;

		[SerializeField]
		private float border = 0.005f;

		private static Dictionary<Mesh, Mesh> cacheFirst = new Dictionary<Mesh, Mesh>();

		private static Dictionary<Mesh, Mesh> cacheSecond = new Dictionary<Mesh, Mesh>();

		private static Dictionary<Mesh, Mesh> cacheThird = new Dictionary<Mesh, Mesh>();

		private static Dictionary<Mesh, Mesh> cacheFourth = new Dictionary<Mesh, Mesh>();

		public List<Pair> Meshes
		{
			get
			{
				if (meshes == null)
				{
					meshes = new List<Pair>();
				}
				return meshes;
			}
		}

		public CwCoord Coord
		{
			get
			{
				return coord;
			}
			set
			{
				coord = value;
			}
		}

		public bool GenerateUV
		{
			get
			{
				return generateUV;
			}
			set
			{
				generateUV = value;
			}
		}

		public float AngleError
		{
			get
			{
				return angleError;
			}
			set
			{
				angleError = value;
			}
		}

		public float AreaError
		{
			get
			{
				return areaError;
			}
			set
			{
				areaError = value;
			}
		}

		public float HardAngle
		{
			get
			{
				return hardAngle;
			}
			set
			{
				hardAngle = value;
			}
		}

		public float PackMargin
		{
			get
			{
				return packMargin;
			}
			set
			{
				packMargin = value;
			}
		}

		public bool FixOverflow
		{
			get
			{
				return fixOverflow;
			}
			set
			{
				fixOverflow = value;
			}
		}

		public bool FixSeams
		{
			get
			{
				return fixSeams;
			}
			set
			{
				fixSeams = value;
			}
		}

		public float Border
		{
			get
			{
				return border;
			}
			set
			{
				border = value;
			}
		}

		public static Mesh GetCachedMesh(Mesh source, CwCoord coord, bool allowGeneration = true)
		{
			return coord switch
			{
				CwCoord.First => TryGetCachedMesh(cacheFirst, source, coord, allowGeneration), 
				CwCoord.Second => TryGetCachedMesh(cacheSecond, source, coord, allowGeneration), 
				CwCoord.Third => TryGetCachedMesh(cacheThird, source, coord, allowGeneration), 
				CwCoord.Fourth => TryGetCachedMesh(cacheFourth, source, coord, allowGeneration), 
				_ => null, 
			};
		}

		private static Mesh TryGetCachedMesh(Dictionary<Mesh, Mesh> cache, Mesh source, CwCoord coord, bool allowGeneration = true)
		{
			Mesh value = null;
			if (source != null && !cache.TryGetValue(source, out value) && allowGeneration)
			{
				value = new Mesh();
				value.hideFlags = HideFlags.DontSave;
				value.name = source.name + " (Auto Fixed Seams)";
				Generate(source, value, generateUV: false, fixOverflow: true, fixSeams: true, coord, 0.005f);
				cache.Add(source, value);
			}
			return value;
		}

		public void AddMesh(Mesh mesh)
		{
			if (mesh != null)
			{
				Meshes.Add(new Pair
				{
					Source = mesh
				});
			}
		}

		public void ConvertLegacy()
		{
			if (source != null)
			{
				Meshes.Add(new Pair
				{
					Source = source,
					Output = mesh
				});
				source = null;
				mesh = null;
			}
		}

		[ContextMenu("Generate")]
		public void Generate()
		{
			if (meshes == null)
			{
				return;
			}
			foreach (Pair mesh in meshes)
			{
				if (mesh.Source != null)
				{
					if (mesh.Output == null)
					{
						mesh.Output = new Mesh();
					}
					mesh.Output.name = mesh.Source.name + " (Fixed)";
					Generate(mesh.Source, mesh.Output, generateUV, fixOverflow, fixSeams, coord, border);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(mesh.Output);
					mesh.Output = null;
				}
			}
		}

		public static void Generate(Mesh source, Mesh output, bool generateUV, bool fixOverflow, bool fixSeams, CwCoord coord, float border)
		{
			DoGenerate(source, output, fixOverflow, fixSeams, coord, border);
		}

		private static void DoGenerate(Mesh source, Mesh output, bool fixOverflow, bool fixSeams, CwCoord coord, float border)
		{
			if (!(source != null) || !(output != null) || border == 0f)
			{
				return;
			}
			output.Clear(keepVertexLayout: false);
			Dictionary<Vector2Int, List<Edge>> dictionary = new Dictionary<Vector2Int, List<Edge>>();
			List<Insertion> list = new List<Insertion>();
			List<List<int>> list2 = new List<List<int>>();
			Vector2[] array = null;
			switch (coord)
			{
			case CwCoord.First:
				array = source.uv;
				break;
			case CwCoord.Second:
				array = source.uv2;
				break;
			case CwCoord.Third:
				array = source.uv3;
				break;
			case CwCoord.Fourth:
				array = source.uv4;
				break;
			}
			if (array.Length != 0)
			{
				double num = 0.0;
				double num2 = 0.0;
				for (int i = 0; i < array.Length; i++)
				{
					num += (double)array[i].x;
					num2 += (double)array[i].y;
				}
				num /= (double)array.Length;
				num2 /= (double)array.Length;
				int num3 = Mathf.FloorToInt((float)num);
				int num4 = Mathf.FloorToInt((float)num2);
				if (num3 != 0 || num4 != 0)
				{
					Vector2 vector = new Vector2(-num3, -num4);
					for (int j = 0; j < array.Length; j++)
					{
						array[j] += vector;
					}
				}
			}
			if (fixSeams)
			{
				int vertexCount = source.vertexCount;
				for (int k = 0; k < source.subMeshCount; k++)
				{
					List<int> list3 = new List<int>();
					source.GetTriangles(list3, k);
					if (array.Length != 0)
					{
						for (int l = 0; l < list3.Count; l += 3)
						{
							AddTriangle(dictionary, array, list3[l], list3[l + 1], list3[l + 2]);
						}
					}
					foreach (KeyValuePair<Vector2Int, List<Edge>> item in dictionary)
					{
						foreach (Edge item2 in item.Value)
						{
							if (item2.Used)
							{
								continue;
							}
							item2.Used = true;
							Ring ring = TraceEdges(dictionary, array, item2);
							if (ring.Edges.Count > 2)
							{
								for (int m = 0; m < ring.Edges.Count; m++)
								{
									Edge edge = ring.GetEdge(m - 1);
									Edge edge2 = ring.GetEdge(m);
									Edge edge3 = ring.GetEdge(m + 1);
									Insertion insertion = new Insertion();
									Insertion insertion2 = new Insertion();
									insertion.Index = edge2.IndexA;
									insertion.NewCoord = GetCoord(array, border, edge.IndexA, edge2.IndexA, edge2.IndexB);
									insertion.NewIndex = vertexCount++;
									insertion2.Index = edge2.IndexB;
									insertion2.NewCoord = GetCoord(array, border, edge2.IndexA, edge2.IndexB, edge3.IndexB);
									insertion2.NewIndex = vertexCount++;
									list.Add(insertion);
									list.Add(insertion2);
									list3.Add(insertion.Index);
									list3.Add(insertion2.Index);
									list3.Add(insertion.NewIndex);
									list3.Add(insertion2.NewIndex);
									list3.Add(insertion.NewIndex);
									list3.Add(insertion2.Index);
								}
							}
						}
					}
					list2.Add(list3);
				}
			}
			else
			{
				for (int n = 0; n < source.subMeshCount; n++)
				{
					List<int> list4 = new List<int>();
					source.GetTriangles(list4, n);
					list2.Add(list4);
				}
			}
			AddFixSeamData(source, output, list2, list, coord);
		}

		private static Vector2 GetCoord(Vector2[] coords, float border, int indexA, int indexB, int indexC)
		{
			Vector2 vector = coords[indexA];
			Vector2 vector2 = coords[indexB];
			Vector2 vector3 = coords[indexC];
			Vector2 normalized = (vector - vector2).normalized;
			normalized = -new Vector2(0f - normalized.y, normalized.x);
			Vector2 normalized2 = (vector2 - vector3).normalized;
			normalized2 = -new Vector2(0f - normalized2.y, normalized2.x);
			Vector2 vector4 = normalized + normalized2;
			float sqrMagnitude = vector4.sqrMagnitude;
			if (sqrMagnitude > 0f)
			{
				sqrMagnitude = Mathf.Sqrt(sqrMagnitude);
				vector2 += vector4 / sqrMagnitude * border;
			}
			return vector2;
		}

		private static void AddCoord(List<Vector4> coords, Insertion insertion, bool write)
		{
			Vector4 item = coords[insertion.Index];
			if (write)
			{
				item.x = insertion.NewCoord.x;
				item.y = insertion.NewCoord.y;
			}
			coords.Add(item);
		}

		private static void AddFixSeamData(Mesh source, Mesh output, List<List<int>> submeshes, List<Insertion> insertions, CwCoord coord)
		{
			output.bindposes = source.bindposes;
			output.bounds = source.bounds;
			output.subMeshCount = source.subMeshCount;
			output.indexFormat = source.indexFormat;
			if (source.vertexCount + insertions.Count * 2 >= 65535)
			{
				output.indexFormat = IndexFormat.UInt32;
			}
			List<BoneWeight> list = new List<BoneWeight>();
			source.GetBoneWeights(list);
			List<Color32> list2 = new List<Color32>();
			source.GetColors(list2);
			List<Vector3> list3 = new List<Vector3>();
			source.GetNormals(list3);
			List<Vector4> list4 = new List<Vector4>();
			source.GetTangents(list4);
			List<Vector4> list5 = new List<Vector4>();
			source.GetUVs(0, list5);
			List<Vector4> list6 = new List<Vector4>();
			source.GetUVs(1, list6);
			List<Vector4> list7 = new List<Vector4>();
			source.GetUVs(2, list7);
			List<Vector4> list8 = new List<Vector4>();
			source.GetUVs(3, list8);
			List<Vector3> list9 = new List<Vector3>();
			source.GetVertices(list9);
			List<byte> list10 = new List<byte>(source.GetBonesPerVertex());
			List<BoneWeight1> list11 = new List<BoneWeight1>(source.GetAllBoneWeights());
			List<int> list12 = new List<int>();
			if (list10.Count > 0)
			{
				int num = 0;
				foreach (byte item in list10)
				{
					list12.Add(num);
					num += item;
				}
				list.Clear();
			}
			foreach (Insertion insertion in insertions)
			{
				if (list10.Count > 0)
				{
					int num2 = list12[insertion.Index];
					byte b = list10[insertion.Index];
					list10.Add(b);
					for (int i = 0; i < b; i++)
					{
						list11.Add(list11[num2 + i]);
					}
				}
				if (list.Count > 0)
				{
					list.Add(list[insertion.Index]);
				}
				if (list2.Count > 0)
				{
					list2.Add(list2[insertion.Index]);
				}
				if (list3.Count > 0)
				{
					list3.Add(list3[insertion.Index]);
				}
				if (list4.Count > 0)
				{
					list4.Add(list4[insertion.Index]);
				}
				if (list5.Count > 0)
				{
					AddCoord(list5, insertion, coord == CwCoord.First);
				}
				if (list6.Count > 0)
				{
					AddCoord(list6, insertion, coord == CwCoord.Second);
				}
				if (list7.Count > 0)
				{
					AddCoord(list7, insertion, coord == CwCoord.Third);
				}
				if (list8.Count > 0)
				{
					AddCoord(list8, insertion, coord == CwCoord.Fourth);
				}
				list9.Add(list9[insertion.Index]);
			}
			output.SetVertices(list9);
			if (list.Count > 0)
			{
				output.boneWeights = list.ToArray();
			}
			if (list10.Count > 0)
			{
				NativeArray<byte> bonesPerVertex = new NativeArray<byte>(list10.ToArray(), Allocator.Temp);
				NativeArray<BoneWeight1> weights = new NativeArray<BoneWeight1>(list11.ToArray(), Allocator.Temp);
				output.SetBoneWeights(bonesPerVertex, weights);
				weights.Dispose();
				bonesPerVertex.Dispose();
			}
			output.SetColors(list2);
			output.SetNormals(list3);
			output.SetTangents(list4);
			output.SetUVs(0, list5);
			output.SetUVs(1, list6);
			output.SetUVs(2, list7);
			output.SetUVs(3, list8);
			List<Vector3> list13 = new List<Vector3>();
			List<Vector3> list14 = new List<Vector3>();
			List<Vector3> list15 = new List<Vector3>();
			if (source.blendShapeCount > 0)
			{
				Vector3[] array = new Vector3[source.vertexCount];
				Vector3[] array2 = new Vector3[source.vertexCount];
				Vector3[] array3 = new Vector3[source.vertexCount];
				for (int j = 0; j < source.blendShapeCount; j++)
				{
					string blendShapeName = source.GetBlendShapeName(j);
					int blendShapeFrameCount = source.GetBlendShapeFrameCount(j);
					for (int k = 0; k < blendShapeFrameCount; k++)
					{
						source.GetBlendShapeFrameVertices(j, k, array, array2, array3);
						list13.Clear();
						list14.Clear();
						list15.Clear();
						list13.AddRange(array);
						list14.AddRange(array2);
						list15.AddRange(array3);
						foreach (Insertion insertion2 in insertions)
						{
							list13.Add(list13[insertion2.Index]);
							list14.Add(list14[insertion2.Index]);
							list15.Add(list15[insertion2.Index]);
						}
						output.AddBlendShapeFrame(blendShapeName, source.GetBlendShapeFrameWeight(j, k), list13.ToArray(), list14.ToArray(), list15.ToArray());
					}
				}
			}
			for (int l = 0; l < submeshes.Count; l++)
			{
				output.SetTriangles(submeshes[l], l);
			}
		}

		private static Ring TraceEdges(Dictionary<Vector2Int, List<Edge>> allEdges, Vector2[] coords, Edge edge)
		{
			Ring ring = new Ring();
			Vector2 vector = coords[edge.IndexB];
			Vector2 vector2 = vector;
			ring.Edges.Add(edge);
			List<Edge> o = null;
			while (TryGetEdges(allEdges, vector, out o))
			{
				foreach (Edge item in o)
				{
					if (!item.Used)
					{
						edge = item;
						vector = coords[edge.IndexB];
						ring.Edges.Add(edge);
						edge.Used = true;
						if (vector != vector2)
						{
							goto IL_0023;
						}
					}
				}
				break;
				IL_0023:;
			}
			return ring;
		}

		private static Vector2Int VectorToVectorInt(Vector2 v)
		{
			float num = v.x * 16384f;
			float num2 = v.y * 16384f;
			return new Vector2Int((int)num, (int)num2);
		}

		private static bool TryGetEdges(Dictionary<Vector2Int, List<Edge>> allEdges, Vector2 coord, out List<Edge> o)
		{
			Vector2Int key = VectorToVectorInt(coord);
			if (allEdges.TryGetValue(key, out o))
			{
				return true;
			}
			return false;
		}

		private static void AddTriangle(Dictionary<Vector2Int, List<Edge>> allEdges, Vector2[] coords, int indexA, int indexB, int indexC)
		{
			Vector2 vector = coords[indexA];
			Vector2 vector2 = coords[indexB];
			Vector2 vector3 = coords[indexC];
			Vector2 vector4 = vector2 - vector;
			if (Vector3.Cross(rhs: vector3 - vector, lhs: vector4).sqrMagnitude >= 0f)
			{
				if ((vector2.x - vector.x) * (vector3.y - vector.y) - (vector3.x - vector.x) * (vector2.y - vector.y) >= 0f)
				{
					TryAddEdge(allEdges, coords, indexB, indexA);
					TryAddEdge(allEdges, coords, indexC, indexB);
					TryAddEdge(allEdges, coords, indexA, indexC);
				}
				else
				{
					TryAddEdge(allEdges, coords, indexA, indexB);
					TryAddEdge(allEdges, coords, indexB, indexC);
					TryAddEdge(allEdges, coords, indexC, indexA);
				}
			}
		}

		private static void TryAddEdge(Dictionary<Vector2Int, List<Edge>> allEdges, Vector2[] coords, int indexA, int indexB)
		{
			Vector2 vector = coords[indexA];
			Vector2 coordB = coords[indexB];
			Edge edge = new Edge();
			edge.IndexA = indexA;
			edge.IndexB = indexB;
			if (MarkEdgeUsed(allEdges, coords, vector, coordB))
			{
				edge.Used = true;
			}
			List<Edge> o = null;
			if (!TryGetEdges(allEdges, vector, out o))
			{
				o = new List<Edge>();
				Vector2Int key = VectorToVectorInt(vector);
				allEdges.Add(key, o);
			}
			o.Add(edge);
		}

		private static bool MarkEdgeUsed(Dictionary<Vector2Int, List<Edge>> allEdges, Vector2[] coords, Vector2 coordA, Vector2 coordB)
		{
			List<Edge> o = null;
			if (TryGetEdges(allEdges, coordB, out o))
			{
				foreach (Edge item in o)
				{
					if (coords[item.IndexB] == coordA)
					{
						item.Used = true;
						return true;
					}
				}
			}
			return false;
		}
	}
}
