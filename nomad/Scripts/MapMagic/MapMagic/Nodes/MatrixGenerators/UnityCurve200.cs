using System;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Modifiers", name = "Unity Curve", iconName = "GeneratorIcons/UnityCurve", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/UnityCurve")]
	public class UnityCurve200 : Generator, IMultiInlet, IOutlet<MatrixWorld>, IUnit
	{
		[Val("Inlet", "Inlet")]
		public readonly IInlet<MatrixWorld> srcIn = new Inlet<MatrixWorld>();

		[Val("Mask", "Inlet")]
		public readonly IInlet<MatrixWorld> maskIn = new Inlet<MatrixWorld>();

		public AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f));

		public Vector2 min = new Vector2(0f, 0f);

		public Vector2 max = new Vector2(1f, 1f);

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixModifiers.cs", 181);
		}

		public IEnumerable<IInlet<object>> Inlets()
		{
			yield return srcIn;
			yield return maskIn;
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (stop != null && stop.stop)
			{
				return;
			}
			MatrixWorld matrixWorld = data.ReadInletProduct(srcIn);
			MatrixWorld matrixWorld2 = data.ReadInletProduct(maskIn);
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
				MatrixWorld matrixWorld3 = new MatrixWorld(matrixWorld);
				if (stop != null && stop.stop)
				{
					return;
				}
				AnimCurve animCurve = new AnimCurve(curve);
				for (int i = 0; i < matrixWorld3.arr.Length; i++)
				{
					matrixWorld3.arr[i] = animCurve.Evaluate(matrixWorld3.arr[i]);
				}
				if (stop == null || !stop.stop)
				{
					if (matrixWorld2 != null)
					{
						matrixWorld3.InvMix(matrixWorld, matrixWorld2);
					}
					data.StoreProduct(this, matrixWorld3);
				}
			}
		}
	}
}
