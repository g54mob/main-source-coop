using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheVisualEngine
{
	[Serializable]
	public class TVEInstanced
	{
		public int instancedDataID;

		public int renderDataID;

		public List<int> renderLayers;

		public int renderPass;

		public Material material;

		public Mesh mesh;

		public List<TVEElement> elements = new List<TVEElement>();

		public List<Renderer> renderers = new List<Renderer>();

		public Matrix4x4[] matrices;

		public Vector4[] parameters;

		public MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		public int propertyBlockCount = -1;
	}
}
