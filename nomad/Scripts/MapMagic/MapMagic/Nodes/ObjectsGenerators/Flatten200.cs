using System;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Modifiers", name = "Flatten", iconName = "GeneratorIcons/Flatten", disengageable = true, colorType = typeof(TransitionsList), helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/ObjectsGenerators/Flatten")]
	public class Flatten200 : Generator, IMultiInlet, IOutlet<MatrixWorld>, IUnit
	{
		[Val("Positions", "Inlet")]
		public readonly Inlet<TransitionsList> positionsIn = new Inlet<TransitionsList>();

		[Val("Heights", "Inlet")]
		public readonly Inlet<MatrixWorld> heightsIn = new Inlet<MatrixWorld>();

		[Val("Radius")]
		public float radius = 1f;

		[Val("Hardness")]
		public float hardness = 0.5f;

		[Val("Size Factor")]
		public float sizeFactor;

		public bool noiseFallof;

		public float noiseAmount = 1f;

		public float noiseSize = 10f;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsModifiers.cs", 762);
		}

		public IEnumerable<IInlet<object>> Inlets()
		{
			yield return positionsIn;
			yield return heightsIn;
		}

		public override void Generate(TileData data, StopToken stop)
		{
			TransitionsList transitionsList = data.ReadInletProduct(positionsIn);
			MatrixWorld matrixWorld = data.ReadInletProduct(heightsIn);
			if (matrixWorld == null)
			{
				return;
			}
			if (transitionsList == null)
			{
				data.StoreProduct(this, matrixWorld);
			}
			else
			{
				if (!enabled)
				{
					return;
				}
				matrixWorld = new MatrixWorld(matrixWorld);
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
					StampLevel(matrixWorld, matrixWorld.GetWorldInterpolatedValue(transition.pos.x, transition.pos.z), (Vector2D)transition.pos, radius * (1f - sizeFactor) + radius * transition.scale.y * sizeFactor, hardness, noise, noiseAmount, noiseSize);
				}
				data.StoreProduct(this, matrixWorld);
			}
		}

		public static void StampLevel(MatrixWorld matrix, float level, Vector2D center, float radius, float hardness, Noise noise = null, float noiseAmount = 0f, float noiseSize = 20f)
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
					float num2 = coord.GetInterpolatedFalloff(center2, num, hardness, 2);
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
					matrix.arr[num4] = level * num2 + matrix.arr[num4] * (1f - num2);
				}
			}
		}
	}
}
