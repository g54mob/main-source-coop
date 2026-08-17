using System;
using Den.Tools;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes
{
	[Serializable]
	public class PositioningSettings
	{
		public bool objHeight = true;

		public bool relativeHeight = true;

		public bool guiHeight;

		public bool useRotation = true;

		public bool takeTerrainNormal;

		public bool rotateYonly;

		public bool regardPrefabRotation;

		public bool guiRotation;

		public bool useScale = true;

		public bool scaleYonly;

		public bool regardPrefabScale;

		public bool guiScale;

		public void MoveRotateScale(ref Transition trs, TileData data)
		{
			if (!objHeight)
			{
				trs.pos.y = 0f;
			}
			float num = 0f;
			if (relativeHeight && data.heights != null)
			{
				num = data.heights.GetWorldInterpolatedValue(trs.pos.x, trs.pos.z, roundToShort: true);
			}
			if (num > 1f)
			{
				num = 1f;
			}
			num *= data.globals.height;
			trs.pos.y += num;
			if (!useScale)
			{
				trs.scale = new Vector3(1f, 1f, 1f);
			}
			else if (scaleYonly)
			{
				trs.scale = new Vector3(1f, trs.scale.y, 1f);
			}
			if (!useRotation)
			{
				trs.rotation = Quaternion.identity;
			}
			else if (takeTerrainNormal)
			{
				Vector3 terrainNormal = GetTerrainNormal(trs.pos.x, trs.pos.z, data.heights, data.globals.height, data.area.PixelSize.x);
				Vector3 forward = Vector3.Cross(trs.rotation * new Vector3(0f, 0f, 1f), terrainNormal);
				trs.rotation = Quaternion.LookRotation(forward, terrainNormal);
			}
			else if (rotateYonly)
			{
				trs.rotation = Quaternion.Euler(0f, trs.Yaw, 0f);
			}
		}

		public static Vector3 GetTerrainNormal(float fx, float fz, MatrixWorld heightmap, float heightFactor, float pixelSize)
		{
			Coord c = heightmap.WorldToPixel(fx, fz);
			int pos = heightmap.rect.GetPos(c);
			float num2;
			float num = (num2 = heightmap.arr[pos]);
			if (c.x >= heightmap.rect.offset.x + 1)
			{
				num2 = heightmap.arr[pos - 1];
			}
			float num3 = num;
			if (c.x <= heightmap.rect.offset.x + heightmap.rect.size.x - 1)
			{
				num3 = heightmap.arr[pos + 1];
			}
			float num4 = num;
			if (c.z >= heightmap.rect.offset.z + 1)
			{
				num4 = heightmap.arr[pos - heightmap.rect.size.x];
			}
			float num5 = num;
			if (c.z <= heightmap.rect.offset.z + heightmap.rect.size.z - 1)
			{
				num5 = heightmap.arr[pos + heightmap.rect.size.z];
			}
			return new Vector3((num2 - num3) * heightFactor, pixelSize * 2f, (num4 - num5) * heightFactor).normalized;
		}

		public static bool SkipOnBiome(ref Transition trs, BiomeBlend biomeBlend, MatrixWorld biomeMask, Noise random)
		{
			float num = biomeMask?.GetWorldInterpolatedValue(trs.pos.x, trs.pos.z) ?? 1f;
			if (num < 1E-05f)
			{
				return true;
			}
			switch (biomeBlend)
			{
			case BiomeBlend.Sharp:
				return num < 0.5f;
			case BiomeBlend.Random:
			{
				float num2 = random.Random((int)trs.pos.x, (int)trs.pos.y);
				if (num > 0.5f)
				{
					num2 = 1f - num2;
				}
				return num < num2;
			}
			case BiomeBlend.Scale:
				trs.scale *= num;
				return num < 0.0001f;
			case BiomeBlend.Pure:
				return num < 0.9999f;
			default:
				return false;
			}
		}
	}
}
