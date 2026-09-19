using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	public class ObiEdgeMeshContainer
	{
		public Dictionary<EdgeCollider2D, ObiEdgeMeshHandle> handles;

		public ObiNativeEdgeMeshHeaderList headers;

		public ObiNativeBIHNodeList bihNodes;

		public ObiNativeEdgeList edges;

		public ObiNativeVector2List vertices;

		public ObiEdgeMeshContainer()
		{
			handles = new Dictionary<EdgeCollider2D, ObiEdgeMeshHandle>();
			headers = new ObiNativeEdgeMeshHeaderList();
			bihNodes = new ObiNativeBIHNodeList();
			edges = new ObiNativeEdgeList();
			vertices = new ObiNativeVector2List();
		}

		public ObiEdgeMeshHandle GetOrCreateEdgeMesh(EdgeCollider2D source)
		{
			if (!handles.TryGetValue(source, out var value))
			{
				Vector2[] points = source.points;
				int[] array = new int[source.edgeCount * 2];
				for (int i = 0; i < source.edgeCount; i++)
				{
					array[i * 2] = i;
					array[i * 2 + 1] = i + 1;
				}
				IBounded[] elements = new IBounded[source.edgeCount];
				for (int j = 0; j < source.edgeCount; j++)
				{
					elements[j] = new Edge(j, j + 1, points[j], points[j + 1]);
				}
				BIHNode[] array2 = BIH.Build(ref elements);
				Edge[] array3 = Array.ConvertAll(elements, (IBounded x) => (Edge)(object)x);
				value = new ObiEdgeMeshHandle(source, headers.count);
				handles.Add(source, value);
				headers.Add(new EdgeMeshHeader(bihNodes.count, array2.Length, edges.count, array3.Length, vertices.count, points.Length));
				bihNodes.AddRange(array2);
				edges.AddRange(array3);
				vertices.AddRange(points);
			}
			return value;
		}

		public void DestroyEdgeMesh(ObiEdgeMeshHandle handle)
		{
			if (handle == null || !handle.isValid || handle.index >= handles.Count)
			{
				return;
			}
			EdgeMeshHeader edgeMeshHeader = headers[handle.index];
			for (int i = 0; i < headers.count; i++)
			{
				EdgeMeshHeader value = headers[i];
				if (value.firstEdge > edgeMeshHeader.firstEdge)
				{
					value.firstNode -= edgeMeshHeader.nodeCount;
					value.firstEdge -= edgeMeshHeader.edgeCount;
					value.firstVertex -= edgeMeshHeader.vertexCount;
					headers[i] = value;
				}
			}
			foreach (KeyValuePair<EdgeCollider2D, ObiEdgeMeshHandle> handle2 in handles)
			{
				if (handle2.Value.index > handle.index)
				{
					handle2.Value.index--;
				}
			}
			bihNodes.RemoveRange(edgeMeshHeader.firstNode, edgeMeshHeader.nodeCount);
			edges.RemoveRange(edgeMeshHeader.firstEdge, edgeMeshHeader.edgeCount);
			vertices.RemoveRange(edgeMeshHeader.firstVertex, edgeMeshHeader.vertexCount);
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
			if (edges != null)
			{
				edges.Dispose();
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
