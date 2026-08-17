using System;
using UnityEngine;

namespace TheVisualEngine
{
	[Serializable]
	public class TVETerrainLayerSettings
	{
		[HideInInspector]
		public string name = "";

		[HideInInspector]
		public bool isInitialized;

		[Space(10f)]
		[Range(1f, 16f)]
		public int layerID = 1;

		[Space(10f)]
		[ColorUsage(false, true)]
		public Color layerColor = Color.white;

		[Space(10f)]
		public bool useCustomLayer;

		[Space(10f)]
		public TerrainLayer terrainLayer;

		[Space(10f)]
		public bool useCustomTextures;

		[Space(10f)]
		public Texture layerAlbedo;

		public Texture layerNormal;

		public Texture layerShader;

		[Space(10f)]
		public bool useCustomSettings;

		[Space(10f)]
		public Color layerSpecular = Color.black;

		public Vector4 layerRemapMin = Vector4.zero;

		public Vector4 layerRemapMax = Vector4.one;

		[Range(0f, 1f)]
		public float layerSmoothness = 1f;

		[Range(-8f, 8f)]
		public float layerNormalScale = 1f;

		[Space(10f)]
		public bool useCustomCoords;

		[Space(10f)]
		public TVEUVMode layerUVMode = TVEUVMode.Scale;

		public Vector4 layerUVValue = new Vector4(1f, 1f, 0f, 0f);
	}
}
