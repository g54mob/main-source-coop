using Den.Tools;
using MapMagic.Core;
using MapMagic.Nodes;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration
{
	[CreateAssetMenu(menuName = "NomadDrive/World Generation/MapMagic Object Config", fileName = "MapMagicObjectConfig")]
	public class MapMagicObjectConfig : ScriptableObject
	{
		public Graph graph;

		public int mainRange = 8;

		public int generateRange = 4;

		public bool generateInfinite = true;

		public int retainMargin = 1;

		public Vector2D tileSize = new Vector2D(256f, 256f);

		public bool genAroundMainCam = true;

		public MapMagicObject.Resolution tileResolution = MapMagicObject.Resolution._65;

		[Tooltip("Unity tag applied to spawned terrain tile GameObjects. Leave empty to skip tagging.")]
		public string terrainTag;

		[Tooltip("Unity layer applied to spawned terrain tile GameObjects. Leave empty to skip.")]
		public string terrainLayer;

		[Tooltip("Concrete terrain materialTemplate assigned to MapMagic at runtime. Empty falls back to MapMagic's Shader.Find(\"HDRP/TerrainLit\") default. Assigning a real material asset gives the build's shader stripper a keyword anchor so it keeps only the variants this material needs instead of the entire TerrainLit matrix.")]
		public Material terrainMaterial;
	}
}
