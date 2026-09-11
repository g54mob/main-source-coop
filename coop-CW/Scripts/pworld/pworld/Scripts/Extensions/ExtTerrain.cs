using UnityEngine;

namespace pworld.Scripts.Extensions
{
	public static class ExtTerrain
	{
		public static Vector3 PGetHeight(this Terrain me, Vector3 worldPos)
		{
			worldPos.y = me.SampleHeight(worldPos) + Terrain.activeTerrain.GetPosition().y;
			return worldPos;
		}

		public static Vector2 PGet01Coordinate(this Terrain me, Vector3 worldPos)
		{
			Vector3 size = me.terrainData.size;
			Vector3 vector = worldPos - me.GetPosition();
			return new Vector2(vector.x / size.x, vector.z / size.z);
		}

		public static Vector3 PGetNormal(this Terrain me, Vector3 worldPos)
		{
			Vector2 vector = me.PGet01Coordinate(worldPos);
			return me.terrainData.GetInterpolatedNormal(vector.x, vector.y);
		}

		public static Color PGetAlphamapColorAtWorldPos(this Terrain me, int numAlphaTexture, Vector3 worldPos)
		{
			Vector2 me2 = me.PGet01Coordinate(worldPos);
			me2.x *= me.terrainData.alphamapWidth;
			me2.y *= me.terrainData.alphamapHeight;
			return me.terrainData.alphamapTextures[numAlphaTexture].GetPixel(me2.ToIVec().x, me2.ToIVec().y);
		}

		public static Vector3 PGetEdge(this Terrain me, Vector3 worldPos, bool right = true)
		{
			worldPos.x = (right ? me.terrainData.size.x : (0f - me.terrainData.size.x)) + me.GetPosition().x;
			return worldPos;
		}

		public static Vector3[] PGetCorners(this Terrain t)
		{
			return new Vector3[4]
			{
				t.PGetHeight(t.GetPosition() + Vector3.right * t.terrainData.size.x - Vector3.back * t.terrainData.size.z),
				t.PGetHeight(t.GetPosition() + Vector3.right * t.terrainData.size.x),
				t.PGetHeight(t.GetPosition()),
				t.PGetHeight(t.GetPosition() - Vector3.back * t.terrainData.size.z)
			};
		}
	}
}
