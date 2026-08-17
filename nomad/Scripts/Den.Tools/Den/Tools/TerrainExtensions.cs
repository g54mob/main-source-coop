using UnityEngine;

namespace Den.Tools
{
	public static class TerrainExtensions
	{
		public static int GetResolution(this Terrain terrain, TerrainControlType controlType)
		{
			TerrainData terrainData = terrain.terrainData;
			return controlType switch
			{
				TerrainControlType.Height => terrainData.heightmapResolution, 
				TerrainControlType.Splats => terrainData.alphamapResolution, 
				TerrainControlType.Grass => terrainData.detailResolution, 
				_ => 0, 
			};
		}

		public static Vector2D PixelSize(this Terrain terrain, TerrainControlType controlType)
		{
			Vector2D vector2D = (Vector2D)terrain.terrainData.size;
			int resolution = terrain.GetResolution(controlType);
			return vector2D / (resolution - 1);
		}

		public static Vector2D PixelSize(this Terrain terrain, int resolution)
		{
			return (Vector2D)terrain.terrainData.size / (resolution - 1);
		}

		public static CoordRect PixelRect(this Terrain terrain, Vector2D worldPos, Vector2D worldSize, TerrainControlType controlType)
		{
			Vector2D pixelSize = terrain.PixelSize(controlType);
			return CoordRect.WorldToPixel(worldPos, worldSize, pixelSize);
		}

		public static CoordRect PixelRect(this Terrain terrain, Vector2D worldPos, Vector2D worldSize, int resolution)
		{
			Vector2D pixelSize = terrain.PixelSize(resolution);
			return CoordRect.WorldToPixel(worldPos, worldSize, pixelSize);
		}

		public static CoordRect PixelRect(this Terrain terrain, TerrainControlType controlType)
		{
			int resolution = terrain.GetResolution(controlType);
			Vector2D vector2D = terrain.PixelSize(controlType);
			return new CoordRect(Mathf.RoundToInt(terrain.transform.position.x / vector2D.x), Mathf.RoundToInt(terrain.transform.position.z / vector2D.z), resolution, resolution);
		}

		public static CoordRect PixelRect(this Terrain terrain, int resolution)
		{
			Vector2D vector2D = terrain.PixelSize(resolution);
			return new CoordRect(Mathf.RoundToInt(terrain.transform.position.x / vector2D.x), Mathf.RoundToInt(terrain.transform.position.z / vector2D.z), resolution, resolution);
		}

		public static float HeightAlignedRatio(int heightResolution, int splatsResolution)
		{
			float num = 1f * (float)splatsResolution / (float)heightResolution;
			if (splatsResolution > heightResolution)
			{
				int num2 = (int)(num + 0.5f);
				int num3 = heightResolution * num2;
				if (num3 - splatsResolution >= -num2 || num3 - splatsResolution <= num2)
				{
					num = num2;
				}
			}
			else
			{
				int num4 = (int)(1f / num + 0.5f);
				int num5 = splatsResolution * num4;
				if (num5 - splatsResolution >= -num4 || num5 - splatsResolution <= num4)
				{
					num = 1f / (float)num4;
				}
			}
			return num;
		}

		public static void FastResize(this Terrain terrain, int resolution, Vector3 size)
		{
			if ((terrain.terrainData.size - size).sqrMagnitude > 0.01f || terrain.terrainData.heightmapResolution != resolution)
			{
				if (resolution <= 64)
				{
					terrain.terrainData.heightmapResolution = resolution;
					terrain.terrainData.size = new Vector3(size.x, size.y, size.z);
					return;
				}
				terrain.terrainData.heightmapResolution = 65;
				terrain.Flush();
				int num = (resolution - 1) / 64;
				terrain.terrainData.size = new Vector3(size.x / (float)num, size.y, size.z / (float)num);
				terrain.terrainData.heightmapResolution = resolution;
			}
		}

		public static bool Contains(this Terrain terrain, Vector3 pos)
		{
			Vector3 position = terrain.transform.position;
			Vector3 size = terrain.terrainData.size;
			if (pos.x > position.x && pos.z > position.z && pos.x < position.x + size.x && pos.z < position.z + size.z)
			{
				return true;
			}
			return false;
		}

		public static float SampleAverageHeight(this Terrain terrain, Vector3 pos, int pixelExtent)
		{
			TerrainData terrainData = terrain.terrainData;
			int heightmapResolution = terrainData.heightmapResolution;
			CoordRect c = new CoordRect(new Coord
			{
				x = (int)((pos.x - terrain.transform.position.x) / terrainData.size.x * (float)heightmapResolution),
				z = (int)((pos.z - terrain.transform.position.z) / terrainData.size.z * (float)heightmapResolution)
			}, pixelExtent);
			c = CoordRect.Intersected(c, new CoordRect(0, 0, heightmapResolution, heightmapResolution));
			float num = 0f;
			float[,] heights = terrainData.GetHeights(c.offset.x, c.offset.z, c.size.x, c.size.z);
			for (int i = 0; i < c.size.x; i++)
			{
				for (int j = 0; j < c.size.z; j++)
				{
					num += heights[j, i];
				}
			}
			return num / (float)(c.size.x * c.size.z) * terrainData.size.y;
		}

		public static Object Object(this DetailPrototype prot)
		{
			if (prot.renderMode == DetailRenderMode.VertexLit)
			{
				return prot.prototype;
			}
			return prot.prototypeTexture;
		}

		public static TerrainData Copy(this TerrainData src)
		{
			TerrainData terrainData = new TerrainData();
			terrainData.heightmapResolution = src.heightmapResolution;
			terrainData.SetHeights(0, 0, src.GetHeights(0, 0, src.heightmapResolution, src.heightmapResolution));
			terrainData.terrainLayers = src.terrainLayers;
			terrainData.alphamapResolution = src.alphamapResolution;
			terrainData.SetAlphamaps(0, 0, src.GetAlphamaps(0, 0, src.alphamapResolution, src.alphamapResolution));
			terrainData.detailPrototypes = src.detailPrototypes;
			terrainData.SetDetailResolution(src.detailResolution, src.detailResolutionPerPatch);
			int num = src.detailPrototypes.Length;
			for (int i = 0; i < num; i++)
			{
				terrainData.SetDetailLayer(0, 0, i, src.GetDetailLayer(0, 0, src.detailResolution, src.detailResolution, i));
			}
			terrainData.treePrototypes = src.treePrototypes;
			terrainData.treeInstances = src.treeInstances;
			terrainData.size = src.size;
			return terrainData;
		}

		public static bool CheckSplatsSum(this TerrainData data, out string error)
		{
			int alphamapResolution = data.alphamapResolution;
			float[,,] alphamaps = data.GetAlphamaps(0, 0, alphamapResolution, alphamapResolution);
			int length = alphamaps.GetLength(2);
			for (int i = 0; i < alphamapResolution; i++)
			{
				for (int j = 0; j < alphamapResolution; j++)
				{
					float num = 0f;
					for (int k = 0; k < length; k++)
					{
						num += alphamaps[i, j, k];
					}
					if (num < 0.999f || num > 1.01f)
					{
						error = $"Sum not equals to 1 at {i}, {j}, sum: {num}";
						return false;
					}
				}
			}
			error = null;
			return true;
		}
	}
}
