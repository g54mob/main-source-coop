using UnityEngine;

namespace Mirror.Examples.Common.Controllers.Player
{
	[ExecuteInEditMode]
	[AddComponentMenu("")]
	public class PerlinNoise : MonoBehaviour
	{
		public float scale = 20f;

		public float heightMultiplier = 0.03f;

		public float offsetX = 5f;

		public float offsetY = 5f;

		[ContextMenu("Generate Terrain")]
		private void GenerateTerrain()
		{
			Terrain component = GetComponent<Terrain>();
			if (component == null)
			{
				Debug.LogError("No Terrain component found on this GameObject.");
			}
			else
			{
				component.terrainData = GenerateTerrainData(component.terrainData);
			}
		}

		private TerrainData GenerateTerrainData(TerrainData terrainData)
		{
			int heightmapResolution = terrainData.heightmapResolution;
			int heightmapResolution2 = terrainData.heightmapResolution;
			float[,] array = new float[heightmapResolution, heightmapResolution2];
			for (int i = 0; i < heightmapResolution; i++)
			{
				for (int j = 0; j < heightmapResolution2; j++)
				{
					float x = (float)i / (float)heightmapResolution * scale + offsetX;
					float y = (float)j / (float)heightmapResolution2 * scale + offsetY;
					array[i, j] = Mathf.PerlinNoise(x, y) * heightMultiplier;
				}
			}
			terrainData.SetHeights(0, 0, array);
			return terrainData;
		}
	}
}
