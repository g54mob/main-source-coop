using System;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	public class MultiPropertiesTest : Generator, IOutlet<MatrixWorld>, IUnit, IPrepare
	{
		[Val("Bool")]
		public bool boolean;

		[Val("Vector2")]
		public Vector2 vector2;

		[Val("Vector3")]
		public Vector3 vector3;

		[Val("Vector4")]
		public Vector4 vector4;

		[Val("Color")]
		public Color color;

		[Val("Texture")]
		public Texture2D texture;

		[Val("Terrain Layer")]
		public TerrainLayer terrainLayer;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixInitial.cs", 574);
		}

		public void Prepare(TileData data, Terrain terrain)
		{
		}

		public override void Generate(TileData data, StopToken stop)
		{
			Debug.Log("Bool " + boolean + "\nVector2 " + vector2.ToString() + "\nVector3 " + vector3.ToString() + "\nVector4 " + vector4.ToString() + "\nColor " + color.ToString() + "\nTexture " + ((texture != null) ? "assigned" : "null") + "\nTerrain Layer " + ((terrainLayer != null) ? "assigned" : "null"));
		}
	}
}
