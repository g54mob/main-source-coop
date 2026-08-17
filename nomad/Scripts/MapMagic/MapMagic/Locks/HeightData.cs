using System;
using Den.Tools;
using Den.Tools.Matrices;
using MapMagic.Nodes;
using MapMagic.Nodes.MatrixGenerators;
using UnityEngine;

namespace MapMagic.Locks
{
	public class HeightData : ILockData
	{
		public float[,] heightsArr;

		private CoordCircle circle;

		private int resolution;

		public void Read(Terrain terrain, Lock lk)
		{
			TerrainData terrainData = terrain.terrainData;
			resolution = terrainData.heightmapResolution;
			circle = new CoordCircle(terrain, resolution, lk.worldPos, lk.worldRadius, lk.worldTransition);
			heightsArr = terrainData.GetHeights(circle.rect.offset.x, circle.rect.offset.z, circle.rect.size.x, circle.rect.size.z);
		}

		public void WriteInThread(IApplyData applyData)
		{
			if (applyData is HeightOutput200.IApplyHeightData && applyData.Resolution == resolution)
			{
				Matrix matrix = new Matrix(circle.rect);
				ImportMatrix(matrix, applyData);
				Matrix matrix2 = new Matrix(circle.rect);
				matrix2.ImportHeights(heightsArr);
				matrix2.ExtendCircular(circle.center, circle.radius - 1, circle.transition + 2, 50);
				matrix.BlendStamped(matrix, matrix2, circle.center.x, circle.center.z, circle.radius, circle.transition);
				ExportMatrix(matrix, applyData);
			}
		}

		public (Matrix heightSrc, Matrix heightDst) WriteWithHeightDelta(HeightOutput200.IApplyHeightData applyData)
		{
			Matrix matrix = new Matrix(circle.rect);
			ImportMatrix(matrix, applyData);
			Matrix matrix2 = new Matrix(circle.rect);
			matrix2.ImportHeights(heightsArr);
			Matrix matrix3 = new Matrix(matrix2);
			float heightDelta = GetHeightDelta(matrix2, matrix);
			matrix3.Add(heightDelta);
			matrix3.ExtendCircular(circle.center, circle.radius - 1, circle.transition + 2, 50);
			matrix.BlendStamped(matrix, matrix3, circle.center.x, circle.center.z, circle.radius, circle.transition);
			ExportMatrix(matrix, applyData);
			return (heightSrc: matrix2, heightDst: matrix);
		}

		public void WriteInApply(Terrain terrain, bool resizeTerrain = false)
		{
			if (heightsArr == null)
			{
				return;
			}
			TerrainData terrainData = terrain.terrainData;
			if (terrain.terrainData.heightmapResolution != resolution)
			{
				if (!resizeTerrain)
				{
					return;
				}
				Vector3 size = terrainData.size;
				terrainData.heightmapResolution = resolution;
				terrainData.size = size;
			}
			terrainData.SetHeights(circle.rect.offset.x, circle.rect.offset.z, heightsArr);
		}

		public void ApplyHeightDelta(Matrix src, Matrix dst)
		{
		}

		public void ResizeFrom(ILockData otherData)
		{
			HeightData heightData = (HeightData)otherData;
			Matrix matrix = new Matrix(heightData.circle.rect);
			matrix.ImportHeights(heightData.heightsArr);
			Matrix matrix2 = new Matrix(circle.rect);
			MatrixOps.Resize(matrix, matrix2);
			matrix2.ExportHeights(heightsArr);
		}

		private float GetHeightDelta(Matrix lockMatrix, Matrix genMatrix)
		{
			float avgInCircle = GetAvgInCircle(lockMatrix, circle.center, circle.radius);
			Vector2 minMaxInRadius = GetMinMaxInRadius(lockMatrix, circle.center, circle.radius);
			float num = GetAvgInCircle(genMatrix, circle.center, circle.radius) - avgInCircle;
			if (minMaxInRadius.x + num < 0f)
			{
				num = 0f - minMaxInRadius.x;
			}
			if (minMaxInRadius.y + num > 1f)
			{
				num = 1f - minMaxInRadius.y;
			}
			return num;
		}

		private static float GetAvgInCircle(Matrix matrix, Coord center, int radius)
		{
			Coord coord = center - radius;
			Coord coord2 = center + radius;
			float num = 0f;
			int num2 = 0;
			int num3 = (int)((float)Math.PI * (float)radius * 2f);
			float num4 = (float)Math.PI * 2f / (float)num3;
			for (int i = 0; i < num3; i++)
			{
				float f = num4 * (float)i;
				float num5 = Mathf.Sin(f);
				float num6 = Mathf.Cos(f);
				int num7 = center.x + (int)(num5 * (float)(radius - 2));
				int num8 = center.z + (int)(num6 * (float)(radius - 2));
				if (num7 >= coord.x && num7 < coord2.x && num8 >= coord.z && num8 < coord2.z)
				{
					int num9 = (num8 - matrix.rect.offset.z) * matrix.rect.size.x + num7 - matrix.rect.offset.x;
					float num10 = matrix.arr[num9];
					num += num10;
					num2++;
				}
			}
			if (num2 >= 0)
			{
				num /= (float)num2;
			}
			return num;
		}

		private static float GetAvgInCircle(float[,] heightsArr, Coord center, int radius)
		{
			Coord coord = center - radius;
			Coord coord2 = center + radius;
			float num = 0f;
			int num2 = 0;
			int num3 = (int)((float)Math.PI * (float)radius * 2f);
			float num4 = (float)Math.PI * 2f / (float)num3;
			for (int i = 0; i < num3; i++)
			{
				float f = num4 * (float)i;
				float num5 = Mathf.Sin(f);
				float num6 = Mathf.Cos(f);
				int num7 = center.x + (int)(num5 * (float)(radius - 2));
				int num8 = center.z + (int)(num6 * (float)(radius - 2));
				if (num7 >= coord.x && num7 < coord2.x && num8 >= coord.z && num8 < coord2.z)
				{
					float num9 = heightsArr[num7, num8];
					num += num9;
					num2++;
				}
			}
			if (num2 >= 0)
			{
				num /= (float)num2;
			}
			return num;
		}

		private static Vector2 GetMinMaxInRadius(Matrix matrix, Coord center, int radius)
		{
			Coord coord = center - radius;
			Coord coord2 = center + radius;
			float num = 200000000f;
			float num2 = -200000000f;
			for (int i = coord.x; i < coord2.x; i++)
			{
				for (int j = coord.z; j < coord2.z; j++)
				{
					if (!(Mathf.Sqrt((i - center.x) * (i - center.x) + (j - center.z) * (j - center.z)) > (float)(radius - 1)))
					{
						int num3 = (j - matrix.rect.offset.z) * matrix.rect.size.x + i - matrix.rect.offset.x;
						float num4 = matrix.arr[num3];
						if (num4 < num)
						{
							num = num4;
						}
						if (num4 > num2)
						{
							num2 = num4;
						}
					}
				}
			}
			return new Vector2(num, num2);
		}

		private static Vector2 GetMinMaxInRadius(float[,] heightsArr, Coord center, int radius)
		{
			Coord coord = center - radius;
			Coord coord2 = center + radius;
			float num = 200000000f;
			float num2 = -200000000f;
			for (int i = coord.x; i < coord2.x; i++)
			{
				for (int j = coord.z; j < coord2.z; j++)
				{
					if (!(Mathf.Sqrt((i - center.x) * (i - center.x) + (j - center.z) * (j - center.z)) > (float)(radius - 1)))
					{
						float num3 = heightsArr[i, j];
						if (num3 < num)
						{
							num = num3;
						}
						if (num3 > num2)
						{
							num2 = num3;
						}
					}
				}
			}
			return new Vector2(num, num2);
		}

		private static void ImportMatrix(Matrix terrainMatrix, IApplyData applyData)
		{
			if (applyData is HeightOutput200.ApplySetData applySetData)
			{
				terrainMatrix.ImportHeights(applySetData.heights2D, new Coord(0, 0));
			}
			else if (applyData is HeightOutput200.ApplySplitData applySplitData)
			{
				terrainMatrix.ImportHeightStrips(applySplitData.heights2DSplits, new Coord(0, 0));
			}
			else if (applyData is HeightOutput200.ApplyTexData applyTexData)
			{
				terrainMatrix.ImportRawFloat(applyTexData.texBytes, new Coord(0, 0), new Coord(applyTexData.res, applyTexData.res), 2f);
			}
		}

		private static void ExportMatrix(Matrix terrainMatrix, IApplyData applyData)
		{
			if (applyData is HeightOutput200.ApplySetData applySetData)
			{
				terrainMatrix.ExportHeights(applySetData.heights2D, new Coord(0, 0));
			}
			else if (applyData is HeightOutput200.ApplySplitData applySplitData)
			{
				terrainMatrix.ExportHeightStrips(applySplitData.heights2DSplits, new Coord(0, 0));
			}
			else if (applyData is HeightOutput200.ApplyTexData applyTexData)
			{
				terrainMatrix.ExportRawFloat(applyTexData.texBytes, new Coord(0, 0), new Coord(applyTexData.res, applyTexData.res), 0.5f);
			}
		}
	}
}
