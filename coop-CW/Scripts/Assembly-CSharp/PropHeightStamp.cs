using UnityEngine;

public class PropHeightStamp : MonoBehaviour
{
	public float offset = -0.01f;

	public int blurRange = 3;

	public LayerMask mask;

	public void ResetWorld()
	{
	}

	public void Execute()
	{
		Terrain terrain = GetTerrain();
		if (!terrain)
		{
			Debug.LogError("No terrain found");
			return;
		}
		TerrainChunk terrainChunk = GetTerrainChunk(terrain);
		bool[,] array = new bool[terrainChunk.heights.GetLength(0), terrainChunk.heights.GetLength(1)];
		for (int i = 0; i < terrainChunk.heights.GetLength(0); i++)
		{
			for (int j = 0; j < terrainChunk.heights.GetLength(1); j++)
			{
				float num = terrainChunk.heights[i, j];
				float height = GetHeight(terrain, terrainChunk.startID + new Vector2Int(j, i));
				if ((height + offset - terrain.transform.position.y) / terrain.terrainData.size.y > num + 0.0001f)
				{
					array[i, j] = true;
				}
				else
				{
					array[i, j] = false;
				}
				float num2 = Mathf.InverseLerp(base.transform.position.y, base.transform.position.y + base.transform.localScale.y, height + offset);
				terrainChunk.heights[i, j] = num2;
			}
		}
		if (blurRange > 0)
		{
			for (int k = 0; k < terrainChunk.heights.GetLength(0); k++)
			{
				for (int l = 0; l < terrainChunk.heights.GetLength(1); l++)
				{
					float heightBefore = terrainChunk.heights[k, l];
					float blurredHeight = GetBlurredHeight(terrainChunk.heights, heightBefore, k, l, blurRange, array);
					terrainChunk.heights[k, l] = blurredHeight;
				}
			}
		}
		CreateTexture(terrainChunk.heights);
	}

	private Texture2D CreateTexture(float[,] heights)
	{
		Texture2D texture2D = new Texture2D(heights.GetLength(1), heights.GetLength(0), TextureFormat.ARGB32, mipChain: true, linear: true);
		for (int i = 0; i < texture2D.height; i++)
		{
			for (int j = 0; j < texture2D.width; j++)
			{
				texture2D.SetPixel(j, i, new Color(heights[i, j], 0f, 0f, 1f));
			}
		}
		texture2D.Apply();
		return texture2D;
	}

	private float GetBlurredHeight(float[,] heights, float heightBefore, int terrainX, int terrainY, int blurRange, bool[,] wasEdited)
	{
		float num = 0f;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < blurRange * 2; i++)
		{
			for (int j = 0; j < blurRange * 2; j++)
			{
				int num4 = terrainX + i - blurRange;
				int num5 = terrainY + j - blurRange;
				if (num4 >= 0 && num4 < heights.GetLength(0) && num5 >= 0 && num5 < heights.GetLength(1))
				{
					if (wasEdited[num4, num5])
					{
						num3++;
					}
					num += heights[num4, num5];
					num2++;
				}
			}
		}
		float b = num / (float)num2;
		float t = (float)num3 / num;
		return Mathf.Lerp(heightBefore, b, t);
	}

	private float GetHeight(Terrain terrain, Vector2Int vector2)
	{
		Ray ray = new Ray(TerrainIDToWorldPos(terrain, vector2) + Vector3.up * base.transform.localScale.y, Vector3.down);
		RaycastHit[] array = Physics.RaycastAll(ray, 50000f, mask);
		float terrainHeight = float.NegativeInfinity;
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i].transform.GetComponent<Terrain>())
			{
				terrainHeight = array[i].point.y;
			}
		}
		float num = float.NegativeInfinity;
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j].point.y > num && PropIsGrounded(terrainHeight, ray, array[j].collider))
			{
				num = array[j].point.y;
			}
		}
		return num;
	}

	private bool PropIsGrounded(float terrainHeight, Ray ray, Collider col)
	{
		ray.origin = new Vector3(ray.origin.x, base.transform.position.y, ray.origin.z);
		ray.direction = Vector3.up;
		RaycastHit[] array = Physics.RaycastAll(ray, 50000f, mask);
		for (int i = 0; i < array.Length; i++)
		{
			if (!(array[i].collider != col) && array[i].point.y > terrainHeight)
			{
				return false;
			}
		}
		return true;
	}

	private TerrainChunk GetTerrainChunk(Terrain terrain)
	{
		Vector3 minPos = GetMinPos();
		Vector2Int startID = WorldPosToTerrainID(terrain, minPos);
		Vector3 maxPos = GetMaxPos();
		Vector2Int vector2Int = WorldPosToTerrainID(terrain, maxPos);
		TerrainChunk obj = new TerrainChunk
		{
			startID = startID
		};
		Vector2Int vector2Int2 = new Vector2Int(vector2Int.x - startID.x, vector2Int.y - startID.y);
		float[,] heights = terrain.terrainData.GetHeights(startID.x, startID.y, vector2Int2.x, vector2Int2.y);
		obj.heights = heights;
		return obj;
	}

	private Vector3 TerrainIDToWorldPos(Terrain terrain, Vector2Int terrainID)
	{
		Vector3 vector = new Vector3(terrainID.x, 0f, terrainID.y);
		vector /= (float)(terrain.terrainData.heightmapResolution - 1);
		vector *= terrain.terrainData.size.x;
		vector.y = terrain.terrainData.GetHeight(terrainID.x, terrainID.y);
		return vector + terrain.transform.position;
	}

	private Vector2Int WorldPosToTerrainID(Terrain terrain, Vector3 min)
	{
		min -= terrain.transform.position;
		min /= terrain.terrainData.size.x;
		min *= (float)(terrain.terrainData.heightmapResolution - 1);
		return new Vector2Int(Mathf.RoundToInt(min.x), Mathf.RoundToInt(min.z));
	}

	private Vector3 GetMinPos()
	{
		return base.transform.position - base.transform.localScale * 0.5f;
	}

	private Vector3 GetMaxPos()
	{
		return base.transform.position + base.transform.localScale * 0.5f;
	}

	private Terrain GetTerrain()
	{
		RaycastHit[] array = Physics.RaycastAll(new Ray(base.transform.position + Vector3.up * base.transform.localScale.y * 0.5f, Vector3.down), 50000f);
		for (int i = 0; i < array.Length; i++)
		{
			Terrain component = array[i].transform.GetComponent<Terrain>();
			if ((bool)component)
			{
				return component;
			}
		}
		return null;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireCube(base.transform.position + Vector3.up * base.transform.localScale.y * 0.5f, base.transform.localScale);
	}
}
