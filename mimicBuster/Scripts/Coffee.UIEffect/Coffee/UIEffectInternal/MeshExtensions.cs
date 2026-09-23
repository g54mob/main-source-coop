using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Coffee.UIEffectInternal
{
	internal static class MeshExtensions
	{
		internal static readonly InternalObjectPool<Mesh> s_MeshPool = new InternalObjectPool<Mesh>(delegate
		{
			Mesh mesh = new Mesh();
			mesh.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
			mesh.MarkDynamic();
			return mesh;
		}, (Mesh mesh) => mesh, delegate(Mesh mesh)
		{
			if (mesh != null)
			{
				mesh.Clear();
			}
		});

		public static Mesh Rent()
		{
			return s_MeshPool.Rent();
		}

		public static void Return(ref Mesh mesh)
		{
			s_MeshPool.Return(ref mesh);
		}

		public static void CopyTo(this Mesh self, Mesh dst)
		{
			if (!(self == null) && !(dst == null))
			{
				List<Vector3> toRelease = InternalListPool<Vector3>.Rent();
				List<Vector4> toRelease2 = InternalListPool<Vector4>.Rent();
				List<Color32> toRelease3 = InternalListPool<Color32>.Rent();
				List<int> toRelease4 = InternalListPool<int>.Rent();
				dst.Clear(keepVertexLayout: false);
				self.GetVertices(toRelease);
				dst.SetVertices(toRelease);
				self.GetTriangles(toRelease4, 0);
				dst.SetTriangles(toRelease4, 0);
				self.GetNormals(toRelease);
				dst.SetNormals(toRelease);
				self.GetTangents(toRelease2);
				dst.SetTangents(toRelease2);
				self.GetColors(toRelease3);
				dst.SetColors(toRelease3);
				self.GetUVs(0, toRelease2);
				dst.SetUVs(0, toRelease2);
				self.GetUVs(1, toRelease2);
				dst.SetUVs(1, toRelease2);
				self.GetUVs(2, toRelease2);
				dst.SetUVs(2, toRelease2);
				self.GetUVs(3, toRelease2);
				dst.SetUVs(3, toRelease2);
				dst.RecalculateBounds();
				InternalListPool<Vector3>.Return(ref toRelease);
				InternalListPool<Vector4>.Return(ref toRelease2);
				InternalListPool<Color32>.Return(ref toRelease3);
				InternalListPool<int>.Return(ref toRelease4);
			}
		}

		public static void CopyTo(this VertexHelper self, Mesh dst)
		{
			if (self != null && !(dst == null))
			{
				self.FillMesh(dst);
			}
		}

		public static void CopyTo(this Mesh self, VertexHelper dst)
		{
			if (!(self == null) && dst != null)
			{
				int vertexCount = self.vertexCount;
				int indexCount = self.triangles.Length;
				self.CopyTo(dst, vertexCount, indexCount);
			}
		}

		public static void CopyTo(this Mesh self, VertexHelper dst, int vertexCount, int indexCount)
		{
			if (!(self == null) && dst != null)
			{
				List<Vector3> toRelease = InternalListPool<Vector3>.Rent();
				List<Vector3> toRelease2 = InternalListPool<Vector3>.Rent();
				List<Vector4> toRelease3 = InternalListPool<Vector4>.Rent();
				List<Vector4> toRelease4 = InternalListPool<Vector4>.Rent();
				List<Vector4> toRelease5 = InternalListPool<Vector4>.Rent();
				List<Vector4> toRelease6 = InternalListPool<Vector4>.Rent();
				List<Vector4> toRelease7 = InternalListPool<Vector4>.Rent();
				List<Color32> toRelease8 = InternalListPool<Color32>.Rent();
				List<int> toRelease9 = InternalListPool<int>.Rent();
				self.GetVertices(toRelease);
				self.GetColors(toRelease8);
				self.GetUVs(0, toRelease3);
				self.GetUVs(1, toRelease4);
				self.GetUVs(2, toRelease5);
				self.GetUVs(3, toRelease6);
				self.GetNormals(toRelease2);
				self.GetTangents(toRelease7);
				self.GetIndices(toRelease9, 0);
				dst.Clear();
				for (int i = 0; i < vertexCount; i++)
				{
					dst.AddVert(toRelease.GetOrDefault(i), toRelease8.GetOrDefault(i), toRelease3.GetOrDefault(i), toRelease4.GetOrDefault(i), toRelease5.GetOrDefault(i), toRelease6.GetOrDefault(i), toRelease2.GetOrDefault(i), toRelease7.GetOrDefault(i));
				}
				int num = Mathf.Clamp(indexCount, 0, toRelease9.Count);
				for (int j = 0; j < num - 2; j += 3)
				{
					dst.AddTriangle(toRelease9[j], toRelease9[j + 1], toRelease9[j + 2]);
				}
				InternalListPool<Vector3>.Return(ref toRelease);
				InternalListPool<Vector3>.Return(ref toRelease2);
				InternalListPool<Vector4>.Return(ref toRelease3);
				InternalListPool<Vector4>.Return(ref toRelease4);
				InternalListPool<Vector4>.Return(ref toRelease5);
				InternalListPool<Vector4>.Return(ref toRelease6);
				InternalListPool<Vector4>.Return(ref toRelease7);
				InternalListPool<Color32>.Return(ref toRelease8);
				InternalListPool<int>.Return(ref toRelease9);
			}
		}

		private static T GetOrDefault<T>(this List<T> self, int index)
		{
			if (0 > index || index >= self.Count)
			{
				return default(T);
			}
			return self[index];
		}
	}
}
