using System;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Modifiers", name = "Slide", iconName = "GeneratorIcons/Slide", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/ObjectsGenerators/Slide")]
	public class SlideGenerator200 : Generator, IMultiInlet, IOutlet<TransitionsList>, IUnit
	{
		[Val("Input", "Inlet")]
		public readonly Inlet<TransitionsList> srcIn = new Inlet<TransitionsList>();

		[Val("Stratum", "Inlet")]
		public readonly Inlet<MatrixWorld> stratumIn = new Inlet<MatrixWorld>();

		[Val("Blur")]
		public int smooth = 2;

		[Val("Iterations")]
		public int iterations = 100;

		[Val("Move Factor")]
		public float moveFactor = 0.2f;

		[Val("Stop Slope")]
		public float stopSlope = 10f;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsModifiers.cs", 1234);
		}

		public IEnumerable<IInlet<object>> Inlets()
		{
			yield return srcIn;
			yield return stratumIn;
		}

		public override void Generate(TileData data, StopToken stop)
		{
			TransitionsList transitionsList = data.ReadInletProduct(srcIn);
			MatrixWorld matrixWorld = data.ReadInletProduct(stratumIn);
			if (transitionsList == null)
			{
				return;
			}
			if (!enabled || matrixWorld == null)
			{
				data.StoreProduct(this, transitionsList);
				return;
			}
			TransitionsList transitionsList2 = new TransitionsList(transitionsList);
			if (smooth != 0)
			{
				Matrix[] array = MatrixOps.GenerateMips(matrixWorld, smooth);
				matrixWorld = new MatrixWorld(array[array.Length - 1], matrixWorld.worldPos, matrixWorld.worldSize);
			}
			float stopDelta = Mathf.Tan(stopSlope * ((float)Math.PI / 180f)) * data.area.PixelSize.x / data.globals.height;
			int num = (int)((float)iterations / 512f * (float)data.area.active.rect.size.x);
			for (int i = 0; i < transitionsList2.count; i++)
			{
				Slide(ref transitionsList2.arr[i], matrixWorld, num, moveFactor, stopDelta, data.globals.height);
			}
			data.StoreProduct(this, transitionsList2);
		}

		public static void Slide(ref Transition trn, MatrixWorld stratum, int iterations, float moveFactor, float stopDelta, float terrainHeight)
		{
			for (int i = 0; i < iterations; i++)
			{
				Coord coord = stratum.WorldToPixel(trn.pos.x, trn.pos.z);
				if (stratum.rect.Contains(coord.x, coord.z, 1.0001f))
				{
					float num = stratum[coord.x, coord.z];
					float num2 = stratum[coord.x + 1, coord.z];
					float num3 = stratum[coord.x, coord.z + 1];
					float num4 = stratum[coord.x + 1, coord.z + 1];
					float num5 = num3 - num4;
					float num6 = num - num2;
					float num7 = num2 - num4;
					float num8 = num - num3;
					float num9 = ((num5 > 0f) ? num5 : (0f - num5));
					float num10 = ((num6 > 0f) ? num6 : (0f - num6));
					float num11 = ((num9 > num10) ? num9 : num10);
					float num12 = ((num7 > 0f) ? num7 : (0f - num7));
					float num13 = ((num8 > 0f) ? num8 : (0f - num8));
					float num14 = ((num12 > num13) ? num12 : num13);
					if (!(((num11 > num14) ? num11 : num14) < stopDelta))
					{
						float num15 = (num5 + num6) / 2f;
						float num16 = (num7 + num8) / 2f;
						trn.pos.x += num15 * (terrainHeight * moveFactor);
						trn.pos.z += num16 * (terrainHeight * moveFactor);
					}
					continue;
				}
				break;
			}
		}
	}
}
