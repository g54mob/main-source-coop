using System;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Modifiers", name = "Floor", iconName = null, disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/wikis/object_generators/Clean_Up")]
	public class Floor200 : Generator, IMultiInlet, IOutlet<TransitionsList>, IUnit
	{
		public enum Relativity
		{
			absolute = 0,
			relative = 1
		}

		[Val("Input", "Inlet")]
		public readonly Inlet<TransitionsList> srcIn = new Inlet<TransitionsList>();

		[Val("Height", "Inlet")]
		public readonly Inlet<MatrixWorld> heightIn = new Inlet<MatrixWorld>();

		[Val("Relativity")]
		public Relativity relativity;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsModifiers.cs", 283);
		}

		public IEnumerable<IInlet<object>> Inlets()
		{
			yield return srcIn;
			yield return heightIn;
		}

		public override void Generate(TileData data, StopToken stop)
		{
			TransitionsList transitionsList = data.ReadInletProduct(srcIn);
			if (transitionsList == null)
			{
				return;
			}
			if (!enabled)
			{
				data.StoreProduct(this, transitionsList);
				return;
			}
			MatrixWorld heights = data.ReadInletProduct(heightIn);
			TransitionsList transitionsList2 = new TransitionsList(transitionsList);
			for (int i = 0; i < transitionsList2.count; i++)
			{
				if (stop != null && stop.stop)
				{
					return;
				}
				Floor(ref transitionsList2.arr[i], heights);
			}
			data.StoreProduct(this, transitionsList2);
		}

		public void Floor(ref Transition trn, MatrixWorld heights)
		{
			if (heights == null)
			{
				trn.pos.y = 0f;
			}
			else if (!(trn.pos.x <= heights.worldPos.x) && !(trn.pos.x >= heights.worldPos.x + heights.worldSize.x) && !(trn.pos.z <= heights.worldPos.z) && !(trn.pos.z >= heights.worldPos.z + heights.worldSize.z))
			{
				float num = heights.GetWorldInterpolatedValue(trn.pos.x, trn.pos.z);
				if (num > 1f)
				{
					num = 1f;
				}
				num *= heights.worldSize.y;
				if (relativity == Relativity.relative)
				{
					trn.pos.y += num;
				}
				else
				{
					trn.pos.y = num;
				}
			}
		}
	}
}
