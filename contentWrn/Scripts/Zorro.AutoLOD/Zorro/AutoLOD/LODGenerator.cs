using System;
using UnityEngine;

namespace Zorro.AutoLOD
{
	[CreateAssetMenu(menuName = "Zorro/AutoLOD/LODGenerator")]
	public class LODGenerator : ScriptableObject
	{
		[Serializable]
		public class LevelOfDetail
		{
			public float quality;

			public float screenPercentage;
		}

		public Mesh sourceMesh;

		public Material[] materials;

		public bool m_static;

		public Vector3 LocalPosition;

		public Vector3 LocalRotation;

		public LevelOfDetail[] LODs;

		public GameObject generatedPrefab;

		public void GenerateLODs()
		{
		}

		private Mesh LoadLOD(string path, int lod)
		{
			return null;
		}

		private Renderer CreateLODMesh(Mesh mesh, string name, Transform parent)
		{
			return null;
		}

		private void ClearSubassets()
		{
		}

		private Mesh SimplifyMesh(float quality)
		{
			return null;
		}
	}
}
