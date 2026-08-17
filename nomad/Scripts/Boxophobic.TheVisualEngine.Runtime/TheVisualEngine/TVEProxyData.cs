using System;
using UnityEngine;

namespace TheVisualEngine
{
	[Serializable]
	public class TVEProxyData
	{
		public GameObject blitGameObject;

		public TVETerrain blitTVETerrain;

		public Mesh blitMesh;

		public Shader blitShader;

		public Material blitMaterial;

		public int bakeCoord;

		public int bakeData;

		public bool bakeAlbedoAsSRGB = true;

		public int saveSize = 512;

		public bool saveAsSRGB = true;

		public bool saveAsDefault = true;
	}
}
