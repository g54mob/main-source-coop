using System;
using Unity.Mathematics;
using UnityEngine;

namespace MeshSplit.Scripts
{
	[Serializable]
	public class MeshSplitParameters
	{
		[Range(0.1f, 256f)]
		public float GridSize = 16f;

		public bool3 SplitAxes = new bool3(x: true, y: true, z: true);

		[Header("Parent attributes.")]
		public bool UseParentLayer = true;

		public bool UseParentStaticFlag = true;

		public bool UseParentMeshRendererSettings = true;

		[Header("Collisions.")]
		public bool GenerateColliders;

		public bool UseConvexColliders;
	}
}
