using System;
using System.Collections.Generic;
using CW.Common;
using UnityEngine;

namespace PaintCore
{
	[ExecuteInEditMode]
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwCoordCopier")]
	public class CwCoordCopier : ScriptableObject
	{
		public enum Coord
		{
			First = 0,
			Second = 1,
			Third = 2,
			Fourth = 3,
			None = 4
		}

		[SerializeField]
		private Mesh source;

		[SerializeField]
		private Coord first = Coord.Second;

		[SerializeField]
		private Coord second = Coord.None;

		[SerializeField]
		private Coord third = Coord.None;

		[SerializeField]
		private Coord fourth = Coord.None;

		[SerializeField]
		private Mesh mesh;

		[NonSerialized]
		private static List<BoneWeight> boneWeights = new List<BoneWeight>();

		[NonSerialized]
		private static List<Color32> colors = new List<Color32>();

		[NonSerialized]
		private static List<Vector3> positions = new List<Vector3>();

		[NonSerialized]
		private static List<Vector3> normals = new List<Vector3>();

		[NonSerialized]
		private static List<Vector4> tangents = new List<Vector4>();

		[NonSerialized]
		private static List<Vector4> coords0 = new List<Vector4>();

		[NonSerialized]
		private static List<Vector4> coords1 = new List<Vector4>();

		[NonSerialized]
		private static List<Vector4> coords2 = new List<Vector4>();

		[NonSerialized]
		private static List<Vector4> coords3 = new List<Vector4>();

		[NonSerialized]
		private static List<Vector4> coordsNone = new List<Vector4>();

		[NonSerialized]
		private static List<int> indices = new List<int>();

		public Mesh Source
		{
			get
			{
				return source;
			}
			set
			{
				source = value;
			}
		}

		public Coord First
		{
			get
			{
				return first;
			}
			set
			{
				first = value;
			}
		}

		public Coord Second
		{
			get
			{
				return second;
			}
			set
			{
				second = value;
			}
		}

		public Coord Third
		{
			get
			{
				return third;
			}
			set
			{
				third = value;
			}
		}

		public Coord Fourth
		{
			get
			{
				return fourth;
			}
			set
			{
				fourth = value;
			}
		}

		public List<Vector4> GetCoords(Coord coord)
		{
			return coord switch
			{
				Coord.First => coords0, 
				Coord.Second => coords1, 
				Coord.Third => coords2, 
				Coord.Fourth => coords3, 
				_ => coordsNone, 
			};
		}

		public void Generate()
		{
			if (!(source != null))
			{
				return;
			}
			if (mesh == null)
			{
				mesh = new Mesh();
			}
			mesh.Clear(keepVertexLayout: false);
			mesh.name = source.name + " (Copied Coords)";
			mesh.bindposes = source.bindposes;
			mesh.bounds = source.bounds;
			mesh.subMeshCount = source.subMeshCount;
			mesh.indexFormat = source.indexFormat;
			source.GetBoneWeights(boneWeights);
			source.GetColors(colors);
			source.GetNormals(normals);
			source.GetTangents(tangents);
			source.GetUVs(0, coords0);
			source.GetUVs(1, coords1);
			source.GetUVs(2, coords2);
			source.GetUVs(3, coords3);
			source.GetVertices(positions);
			mesh.SetVertices(positions);
			for (int i = 0; i < source.subMeshCount; i++)
			{
				source.GetTriangles(indices, i);
				mesh.SetTriangles(indices, i);
			}
			mesh.boneWeights = boneWeights.ToArray();
			mesh.SetColors(colors);
			mesh.SetNormals(normals);
			mesh.SetTangents(tangents);
			mesh.SetUVs(0, GetCoords(first));
			mesh.SetUVs(1, GetCoords(second));
			mesh.SetUVs(2, GetCoords(third));
			mesh.SetUVs(3, GetCoords(fourth));
			if (source.blendShapeCount <= 0)
			{
				return;
			}
			Vector3[] deltaVertices = new Vector3[source.vertexCount];
			Vector3[] deltaNormals = new Vector3[source.vertexCount];
			Vector3[] deltaTangents = new Vector3[source.vertexCount];
			for (int j = 0; j < source.blendShapeCount; j++)
			{
				string blendShapeName = source.GetBlendShapeName(j);
				int blendShapeFrameCount = source.GetBlendShapeFrameCount(j);
				for (int k = 0; k < blendShapeFrameCount; k++)
				{
					source.GetBlendShapeFrameVertices(j, k, deltaVertices, deltaNormals, deltaTangents);
					mesh.AddBlendShapeFrame(blendShapeName, source.GetBlendShapeFrameWeight(j, k), deltaVertices, deltaNormals, deltaTangents);
				}
			}
		}

		protected virtual void OnDestroy()
		{
			CwHelper.Destroy(mesh);
		}
	}
}
