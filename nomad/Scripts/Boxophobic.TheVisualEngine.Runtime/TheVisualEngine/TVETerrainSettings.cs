using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheVisualEngine
{
	[Serializable]
	public class TVETerrainSettings
	{
		public Texture terrainAlbedo;

		public Texture terrainNormal;

		public Texture terrainShader;

		public Texture terrainFeature;

		[Space(10f)]
		public bool useCustomTextures;

		[Space(10f)]
		public Texture terrainControl01;

		public Texture terrainControl02;

		public Texture terrainControl03;

		public Texture terrainControl04;

		public Texture terrainHolesMask;

		[Space(10f)]
		public bool useLayersOrderAsID;

		[Space(10f)]
		public List<TVETerrainLayerSettings> terrainLayers = new List<TVETerrainLayerSettings>();
	}
}
