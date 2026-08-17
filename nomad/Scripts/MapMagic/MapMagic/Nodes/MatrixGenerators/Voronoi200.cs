using System;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Initial", name = "Voronoi", iconName = "GeneratorIcons/Voronoi", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Voronoi")]
	public class Voronoi200 : Generator, IOutlet<MatrixWorld>, IUnit
	{
		public enum BlendType
		{
			flat = 0,
			closest = 1,
			secondClosest = 2,
			cellular = 3,
			organic = 4
		}

		[Val("Intensity")]
		public float intensity = 1f;

		[Val("Cell Size", 0f)]
		public int cellSize = 50;

		[Val("Uniformity")]
		public float uniformity;

		[Val("Seed")]
		public int seed = 12345;

		[Val("Blend Type")]
		public BlendType blendType = BlendType.cellular;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixInitial.cs", 113);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			MatrixWorld matrixWorld = new MatrixWorld(data.area.full.rect, data.area.full.worldPos, data.area.full.worldSize, data.globals.height);
			Voronoi(matrixWorld, new Noise(data.random, seed), stop);
			data.StoreProduct(this, matrixWorld);
		}

		public void Voronoi(MatrixWorld matrix, Noise random, StopToken stop = null)
		{
			Vector3 worldPos = matrix.worldPos;
			Vector3 worldSize = matrix.worldSize;
			CoordRect rect = CoordRect.WorldToPixel((Vector2D)worldPos, (Vector2D)worldSize, (Vector2D)cellSize);
			worldPos = new Vector3(rect.offset.x * cellSize, worldPos.y, rect.offset.z * cellSize);
			worldSize = new Vector3(rect.size.x * cellSize, worldSize.y, rect.size.z * cellSize);
			rect.offset -= 1;
			rect.size += 2;
			worldPos.x -= cellSize;
			worldPos.z -= cellSize;
			worldSize.x += cellSize * 2;
			worldSize.z += cellSize * 2;
			PositionMatrix positionMatrix = new PositionMatrix(rect, worldPos, worldSize);
			positionMatrix.Scatter(uniformity, random);
			positionMatrix = positionMatrix.Relaxed();
			float num = intensity * (matrix.worldSize.x / (float)cellSize) * 0.05f;
			Coord min = matrix.rect.Min;
			Coord max = matrix.rect.Max;
			for (int i = min.x; i < max.x; i++)
			{
				if (stop != null && stop.stop)
				{
					break;
				}
				for (int j = min.z; j < max.z; j++)
				{
					Vector2D vector2D = new Vector2D((float)(i - matrix.rect.offset.x) / (float)(matrix.rect.size.x - 1), (float)(j - matrix.rect.offset.z) / (float)(matrix.rect.size.z - 1));
					Vector2D vector2D2 = new Vector2D(vector2D.x * matrix.worldSize.x + matrix.worldPos.x, vector2D.z * matrix.worldSize.z + matrix.worldPos.z);
					positionMatrix.GetTwoClosest((Vector3)vector2D2, out var closest, out var _, out var minDist, out var secondMinDist);
					float num2 = 0f;
					switch (blendType)
					{
					case BlendType.flat:
						num2 = closest.y;
						break;
					case BlendType.closest:
						num2 = minDist / (matrix.worldSize.x * 16f);
						break;
					case BlendType.secondClosest:
						num2 = secondMinDist / (matrix.worldSize.x * 16f);
						break;
					case BlendType.cellular:
						num2 = (secondMinDist - minDist) / (matrix.worldSize.x * 16f);
						break;
					case BlendType.organic:
						num2 = (secondMinDist + minDist) / 2f / (matrix.worldSize.x * 16f);
						break;
					}
					matrix[i, j] += num2 * num;
				}
			}
		}
	}
}
