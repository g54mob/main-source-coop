using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	public class ObiTriangleMeshContainer
	{
		public Dictionary<Mesh, ObiTriangleMeshHandle> handles;

		public ObiNativeTriangleMeshHeaderList headers;

		public ObiNativeBIHNodeList bihNodes;

		public ObiNativeTriangleList triangles;

		public ObiNativeVector3List vertices;

		public ObiTriangleMeshContainer()
		{
			handles = new Dictionary<Mesh, ObiTriangleMeshHandle>();
			headers = new ObiNativeTriangleMeshHeaderList();
			bihNodes = new ObiNativeBIHNodeList();
			triangles = new ObiNativeTriangleList();
			vertices = new ObiNativeVector3List();
		}

		public ObiTriangleMeshHandle GetOrCreateTriangleMesh(Mesh source)
		{
			ObiTriangleMeshHandle value = new ObiTriangleMeshHandle(null);
			if (source != null && !handles.TryGetValue(source, out value))
			{
				if (source.isReadable)
				{
					int[] array = source.triangles;
					Vector3[] array2 = source.vertices;
					IBounded[] elements = new IBounded[array.Length / 3];
					for (int i = 0; i < elements.Length; i++)
					{
						int num = array[i * 3];
						int num2 = array[i * 3 + 1];
						int num3 = array[i * 3 + 2];
						elements[i] = new Triangle(num, num2, num3, array2[num], array2[num2], array2[num3]);
					}
					BIHNode[] array3 = BIH.Build(ref elements);
					Triangle[] array4 = Array.ConvertAll(elements, (IBounded x) => (Triangle)(object)x);
					value = new ObiTriangleMeshHandle(source, headers.count);
					handles.Add(source, value);
					headers.Add(new TriangleMeshHeader(bihNodes.count, array3.Length, triangles.count, array4.Length, vertices.count, array2.Length));
					bihNodes.AddRange(array3);
					triangles.AddRange(array4);
					vertices.AddRange(array2);
				}
				else
				{
					value = new ObiTriangleMeshHandle(source);
					handles.Add(source, value);
				}
			}
			return value;
		}

		public void DestroyTriangleMesh(ObiTriangleMeshHandle handle)
		{
			if (handle == null || !handle.isValid || handle.index >= handles.Count)
			{
				return;
			}
			TriangleMeshHeader triangleMeshHeader = headers[handle.index];
			for (int i = 0; i < headers.count; i++)
			{
				TriangleMeshHeader value = headers[i];
				if (value.firstTriangle > triangleMeshHeader.firstTriangle)
				{
					value.firstNode -= triangleMeshHeader.nodeCount;
					value.firstTriangle -= triangleMeshHeader.triangleCount;
					value.firstVertex -= triangleMeshHeader.vertexCount;
					headers[i] = value;
				}
			}
			foreach (KeyValuePair<Mesh, ObiTriangleMeshHandle> handle2 in handles)
			{
				if (handle2.Value.index > handle.index)
				{
					handle2.Value.index--;
				}
			}
			bihNodes.RemoveRange(triangleMeshHeader.firstNode, triangleMeshHeader.nodeCount);
			triangles.RemoveRange(triangleMeshHeader.firstTriangle, triangleMeshHeader.triangleCount);
			vertices.RemoveRange(triangleMeshHeader.firstVertex, triangleMeshHeader.vertexCount);
			headers.RemoveAt(handle.index);
			handles.Remove(handle.owner);
			handle.Invalidate();
		}

		public void Dispose()
		{
			if (headers != null)
			{
				headers.Dispose();
			}
			if (triangles != null)
			{
				triangles.Dispose();
			}
			if (vertices != null)
			{
				vertices.Dispose();
			}
			if (bihNodes != null)
			{
				bihNodes.Dispose();
			}
		}
	}
}
