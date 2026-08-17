using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheVisualEngine
{
	[Serializable]
	public class TVEGameObjectData
	{
		public GameObject parentPrefab;

		public GameObject gameObject;

		public MeshFilter meshFilter;

		public Mesh originalMesh;

		public Mesh instanceMesh;

		public List<MeshCollider> meshColliders = new List<MeshCollider>();

		public List<Mesh> originalColliders = new List<Mesh>();

		public List<Mesh> instanceColliders = new List<Mesh>();

		public MeshRenderer meshRenderer;

		public Material[] originalMaterials;

		public Material[] instanceMaterials;

		public bool isZUp;
	}
}
