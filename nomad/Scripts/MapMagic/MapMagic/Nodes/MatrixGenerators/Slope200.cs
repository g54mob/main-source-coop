using System;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Modifiers", name = "Slope", iconName = "GeneratorIcons/Slope", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Slope")]
	public class Slope200 : Generator, IInlet<MatrixWorld>, IUnit, IOutlet<MatrixWorld>
	{
		[Val("From")]
		public float from = 30f;

		[Val("To")]
		public float to = 90f;

		[Val("Smooth Range")]
		public float range = 30f;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixModifiers.cs", 517);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			MatrixWorld matrixWorld = data.ReadInletProduct(this);
			if (matrixWorld == null)
			{
				return;
			}
			if (!enabled)
			{
				data.StoreProduct(this, matrixWorld);
			}
			else if (stop == null || !stop.stop)
			{
				MatrixWorld product = Slope(matrixWorld, data.globals.height, from, to, range);
				if (stop == null || !stop.stop)
				{
					data.StoreProduct(this, product);
				}
			}
		}

		public static MatrixWorld Slope(MatrixWorld heights, float height, float from, float to, float range)
		{
			return Slope(heights, heights.worldPos, heights.worldSize, height, from, to, range);
		}

		public static MatrixWorld Slope(Matrix heights, Vector3 worldPos, Vector3 worldSize, float height, float from, float to, float range)
		{
			MatrixWorld matrixWorld = new MatrixWorld(heights.rect, worldPos, worldSize);
			MatrixOps.Delta(heights, matrixWorld);
			float num = from - range / 2f;
			float num2 = from + range / 2f;
			float num3 = to - range / 2f;
			float num4 = to + range / 2f;
			float num5 = 1f * worldSize.x / (float)heights.rect.size.x;
			float minFrom = Mathf.Tan(num * ((float)Math.PI / 180f)) * num5 / height;
			float minTo = Mathf.Tan(num2 * ((float)Math.PI / 180f)) * num5 / height;
			float maxFrom = Mathf.Tan(num3 * ((float)Math.PI / 180f)) * num5 / height;
			float maxTo = Mathf.Tan(num4 * ((float)Math.PI / 180f)) * num5 / height;
			if (num3 > 89.9f)
			{
				maxFrom = 20000000f;
			}
			if (num4 > 89.9f)
			{
				maxTo = 20000000f;
			}
			if (from < 1E-05f)
			{
				minFrom = -1f;
				minTo = -1f;
			}
			matrixWorld.SelectRange(minFrom, minTo, maxFrom, maxTo);
			return matrixWorld;
		}
	}
}
