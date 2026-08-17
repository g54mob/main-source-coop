using System;
using System.Collections.Generic;
using UnityEngine;

namespace PaintCore
{
	public class CwHitCache
	{
		[NonSerialized]
		private bool cached;

		[NonSerialized]
		private List<IHitPoint> hitPoints = new List<IHitPoint>();

		[NonSerialized]
		private List<IHitLine> hitLines = new List<IHitLine>();

		[NonSerialized]
		private List<IHitTriangle> hitTriangles = new List<IHitTriangle>();

		[NonSerialized]
		private List<IHitQuad> hitQuads = new List<IHitQuad>();

		[NonSerialized]
		private List<IHitCoord> hitCoords = new List<IHitCoord>();

		[NonSerialized]
		private static List<IHit> hits = new List<IHit>();

		public bool Cached => cached;

		public void InvokePoint(GameObject gameObject, bool preview, int priority, float pressure, Vector3 position, Quaternion rotation)
		{
			if (!cached)
			{
				Cache(gameObject);
			}
			int seed = UnityEngine.Random.Range(-2147483648, 2147483647);
			for (int i = 0; i < hitPoints.Count; i++)
			{
				hitPoints[i].HandleHitPoint(preview, priority, pressure, seed, position, rotation);
			}
		}

		public void InvokeLine(GameObject gameObject, bool preview, int priority, float pressure, Vector3 position, Vector3 endPosition, Quaternion rotation, bool clip)
		{
			if (!cached)
			{
				Cache(gameObject);
			}
			int seed = UnityEngine.Random.Range(-2147483648, 2147483647);
			for (int i = 0; i < hitLines.Count; i++)
			{
				hitLines[i].HandleHitLine(preview, priority, pressure, seed, position, endPosition, rotation, clip);
			}
		}

		public void InvokeTriangle(GameObject gameObject, bool preview, int priority, float pressure, CwHit hit, Quaternion rotation)
		{
			Vector3 positionA = default(Vector3);
			Vector3 positionB = default(Vector3);
			Vector3 positionC = default(Vector3);
			if (CwMeshCache.GetTrianglePositions(hit, ref positionA, ref positionB, ref positionC))
			{
				InvokeTriangle(gameObject, preview, priority, pressure, positionA, positionB, positionC, rotation);
			}
		}

		public void InvokeTriangle(GameObject gameObject, bool preview, int priority, float pressure, Vector3 positionA, Vector3 positionB, Vector3 positionC, Quaternion rotation)
		{
			if (!cached)
			{
				Cache(gameObject);
			}
			int seed = UnityEngine.Random.Range(-2147483648, 2147483647);
			for (int i = 0; i < hitTriangles.Count; i++)
			{
				hitTriangles[i].HandleHitTriangle(preview, priority, pressure, seed, positionA, positionB, positionC, rotation);
			}
		}

		public void InvokeQuad(GameObject gameObject, bool preview, int priority, float pressure, Vector3 position, Vector3 endPosition, Vector3 position2, Vector3 endPosition2, Quaternion rotation, bool clip)
		{
			if (!cached)
			{
				Cache(gameObject);
			}
			int seed = UnityEngine.Random.Range(-2147483648, 2147483647);
			for (int i = 0; i < hitQuads.Count; i++)
			{
				hitQuads[i].HandleHitQuad(preview, priority, pressure, seed, position, endPosition, position2, endPosition2, rotation, clip);
			}
		}

		public void InvokeCoord(GameObject gameObject, bool preview, int priority, float pressure, CwHit hit, Quaternion rotation)
		{
			if (!cached)
			{
				Cache(gameObject);
			}
			int seed = UnityEngine.Random.Range(-2147483648, 2147483647);
			for (int i = 0; i < hitCoords.Count; i++)
			{
				hitCoords[i].HandleHitCoord(preview, priority, pressure, seed, hit, rotation);
			}
		}

		public void Clear()
		{
			cached = false;
			hitPoints.Clear();
			hitLines.Clear();
			hitTriangles.Clear();
			hitQuads.Clear();
			hitCoords.Clear();
		}

		private void Cache(GameObject gameObject)
		{
			cached = true;
			gameObject.GetComponentsInChildren(hits);
			hitPoints.Clear();
			hitLines.Clear();
			hitTriangles.Clear();
			hitQuads.Clear();
			hitCoords.Clear();
			for (int i = 0; i < hits.Count; i++)
			{
				IHit hit = hits[i];
				if (hit is IHitPoint item)
				{
					hitPoints.Add(item);
				}
				if (hit is IHitLine item2)
				{
					hitLines.Add(item2);
				}
				if (hit is IHitTriangle item3)
				{
					hitTriangles.Add(item3);
				}
				if (hit is IHitQuad item4)
				{
					hitQuads.Add(item4);
				}
				if (hit is IHitCoord item5)
				{
					hitCoords.Add(item5);
				}
			}
		}
	}
}
