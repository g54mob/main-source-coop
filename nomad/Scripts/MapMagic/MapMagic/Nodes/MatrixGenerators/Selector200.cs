using System;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Modifiers", name = "Selector", iconName = "GeneratorIcons/Selector", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Selector")]
	public class Selector200 : Generator, IInlet<MatrixWorld>, IUnit, IOutlet<MatrixWorld>
	{
		public enum RangeDet
		{
			Transition = 0,
			MinMax = 1
		}

		public enum Units
		{
			Map = 0,
			World = 1
		}

		public RangeDet rangeDet;

		public Units units;

		public Vector2 from = new Vector2(0.4f, 0.6f);

		public Vector2 to = new Vector2(1f, 1f);

		public float tFrom;

		public float tTo;

		public float tTransition;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixModifiers.cs", 581);
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
			else
			{
				if (stop != null && stop.stop)
				{
					return;
				}
				MatrixWorld matrixWorld2 = new MatrixWorld(matrixWorld);
				if (rangeDet == RangeDet.Transition)
				{
					SwitchRangeTransitionToMinmax();
				}
				if (stop == null || !stop.stop)
				{
					Select(matrixWorld2, from, to, units == Units.World, data.globals.height);
					if (stop == null || !stop.stop)
					{
						data.StoreProduct(this, matrixWorld2);
					}
				}
			}
		}

		public void SwitchRangeMinmaxToTransition()
		{
			tFrom = (from.x + from.y) / 2f;
			tTo = (to.x + to.y) / 2f;
			tTransition = from.y - from.x;
		}

		public void SwitchRangeTransitionToMinmax()
		{
			from.x = tFrom - tTransition / 2f;
			from.y = tFrom + tTransition / 2f;
			to.x = tTo - tTransition / 2f;
			to.y = tTo + tTransition / 2f;
		}

		public static void Select(MatrixWorld dst, Vector2 from, Vector2 to, bool inWorldUnits, float worldHeight)
		{
			float num = from.x;
			if (inWorldUnits)
			{
				num /= worldHeight;
			}
			float num2 = from.y;
			if (inWorldUnits)
			{
				num2 /= worldHeight;
			}
			float num3 = to.x;
			if (inWorldUnits)
			{
				num3 /= worldHeight;
			}
			float num4 = to.y;
			if (inWorldUnits)
			{
				num4 /= worldHeight;
			}
			dst.SelectRange(num, num2, num3, num4);
		}
	}
}
