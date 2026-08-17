using System;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Initial", name = "Random", iconName = "GeneratorIcons/Random", disengageable = true, colorType = typeof(TransitionsList), helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/ObjectsGenerators/Random")]
	public class Random207 : Generator, IInlet<MatrixWorld>, IUnit, IOutlet<TransitionsList>
	{
		[Val("Seed")]
		public int seed = 12345;

		[Val("Density")]
		public float density = 10f;

		[Val("Uniformity")]
		public float uniformity = 0.1f;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsInitial.cs", 101);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (enabled)
			{
				MatrixWorld prob = data.ReadInletProduct(this);
				Noise rnd = new Noise(data.random, seed);
				float num = data.area.active.worldSize.x * data.area.active.worldSize.z * (density / 1000000f);
				PosTab posTab = new PosTab((Vector3)data.area.full.worldPos, (Vector3)data.area.full.worldSize, 16);
				RandomScatter((int)num, uniformity, (Vector3)data.area.full.worldPos, (Vector3)data.area.full.worldSize, posTab, rnd, prob);
				TransitionsList product = posTab.ToTransitionsList();
				data.StoreProduct(this, product);
			}
		}

		public static void RandomScatter(int count, float uniformity, Vector3 offset, Vector3 size, PosTab posTab, Noise rnd, MatrixWorld prob, StopToken stop = null)
		{
			int num = (int)(uniformity * 100f);
			if (num < 1)
			{
				num = 1;
			}
			for (int i = 0; i < count; i++)
			{
				if (stop != null && stop.stop)
				{
					return;
				}
				float x = 0f;
				float z = 0f;
				float num2 = 0f;
				for (int j = 0; j < num; j++)
				{
					float num3 = offset.x + 1f + rnd.Random((int)posTab.pos.x, (int)posTab.pos.z, i * num + j, 0) * (size.x - 2.01f);
					float num4 = offset.z + 1f + rnd.Random((int)posTab.pos.x, (int)posTab.pos.z, i * num + j, 1) * (size.z - 2.01f);
					Transition transition = posTab.Closest(num3, num4, 0.001f);
					float num5 = (transition.pos.x - num3) * (transition.pos.x - num3) + (transition.pos.z - num4) * (transition.pos.z - num4);
					float num6 = (num3 - offset.x) * 2f;
					if (num6 * num6 < num5)
					{
						num5 = num6 * num6;
					}
					num6 = (num4 - offset.z) * 2f;
					if (num6 * num6 < num5)
					{
						num5 = num6 * num6;
					}
					num6 = (offset.x + size.x - num3) * 2f;
					if (num6 * num6 < num5)
					{
						num5 = num6 * num6;
					}
					num6 = (offset.z + size.z - num4) * 2f;
					if (num6 * num6 < num5)
					{
						num5 = num6 * num6;
					}
					if (prob != null)
					{
						float worldInterpolatedValue = prob.GetWorldInterpolatedValue(num3, num4);
						num5 *= worldInterpolatedValue;
					}
					if (num5 > num2)
					{
						num2 = num5;
						x = num3;
						z = num4;
					}
				}
				if (num2 > 0.001f)
				{
					Transition trs = new Transition(x, z);
					posTab.Add(trs);
				}
			}
			posTab.Flush();
		}
	}
}
