using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheVisualEngine
{
	[Serializable]
	public class TVEModelData
	{
		public Mesh mesh;

		public float height;

		public float radius;

		public List<float> variationMask;

		public List<float> occlusionMask;

		public List<float> detailMask;

		public List<float> heightMask;

		public List<Vector2> detailCoord;

		public List<float> motion2Mask;

		public List<float> motion3Mask;

		public List<Vector3> pivotPositions;
	}
}
