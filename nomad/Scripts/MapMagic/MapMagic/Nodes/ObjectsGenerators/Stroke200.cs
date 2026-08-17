using System;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Modifiers", name = "Stroke", iconName = "GeneratorIcons/Flatten", disengageable = true, colorType = typeof(TransitionsList), helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/ObjectsGenerators/Stroke")]
	public class Stroke200 : Generator, IInlet<TransitionsList>, IUnit, IOutlet<MatrixWorld>
	{
		[Val("Radius")]
		public float radius = 1f;

		[Val("Hardness")]
		public float hardness = 0.5f;

		[Val("Size Factor")]
		public float sizeFactor;

		public bool noiseFallof;

		public float noiseAmount = 10f;

		public float noiseSize = 5f;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsModifiers.cs", 856);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			TransitionsList transitionsList = data.ReadInletProduct(this);
			if (transitionsList == null || !enabled)
			{
				return;
			}
			MatrixWorld matrixWorld = new MatrixWorld(data.area.full.rect, data.area.full.worldPos, data.area.full.worldSize, data.globals.height);
			Noise noise = null;
			if (noiseFallof)
			{
				noise = new Noise(data.random, 12345);
			}
			for (int i = 0; i < transitionsList.count; i++)
			{
				if (stop != null && stop.stop)
				{
					return;
				}
				Transition transition = transitionsList.arr[i];
				StampMax(matrixWorld, (Vector2D)transition.pos, radius * (1f - sizeFactor) + radius * transition.scale.y * sizeFactor, hardness, noise, noiseAmount, noiseSize);
			}
			matrixWorld.Clamp01();
			data.StoreProduct(this, matrixWorld);
		}

		private static void StampMax(MatrixWorld matrix, Vector2D center, float radius, float hardness, Noise noise = null, float noiseAmount = 0f, float noiseSize = 20f)
		{
			Vector2D center2 = (Vector2D)matrix.WorldToPixelInterpolated(center.x, center.z);
			float num = matrix.WorldDistToPixelInterpolated(radius);
			CoordRect coordRect = CoordRect.Intersected(c2: new CoordRect(center2, num), c1: matrix.rect);
			Coord min = coordRect.Min;
			Coord max = coordRect.Max;
			Coord coord = default(Coord);
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					coord.x = i;
					coord.z = j;
					float num2 = coord.GetInterpolatedFalloff(center2, num, hardness);
					if (num2 < 1E-05f)
					{
						continue;
					}
					if (noise != null)
					{
						float num3 = num2;
						if (num2 > 0.5f)
						{
							num3 = 1f - num2;
						}
						num2 += (noise.Fractal(i, j, noiseSize) * 2f - 1f) * num3 * noiseAmount;
					}
					int num4 = (j - matrix.rect.offset.z) * matrix.rect.size.x + i - matrix.rect.offset.x;
					if (num2 > matrix.arr[num4])
					{
						matrix.arr[num4] = num2;
					}
				}
			}
		}
	}
}
