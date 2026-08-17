using System;
using System.Collections.Generic;
using Den.Tools;
using MapMagic.Products;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Modifiers", name = "Combine", iconName = "GeneratorIcons/Combine", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/wikis/object_generators/Combine")]
	public class Combine200 : Generator, IMultiInlet, IOutlet<TransitionsList>, IUnit
	{
		public Inlet<TransitionsList>[] inlets = new Inlet<TransitionsList>[2]
		{
			new Inlet<TransitionsList>(),
			new Inlet<TransitionsList>()
		};

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsModifiers.cs", 562);
		}

		public IEnumerable<IInlet<object>> Inlets()
		{
			for (int i = 0; i < inlets.Length; i++)
			{
				yield return inlets[i];
			}
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (!enabled)
			{
				return;
			}
			TransitionsList transitionsList = new TransitionsList();
			bool flag = false;
			for (int i = 0; i < inlets.Length; i++)
			{
				TransitionsList transitionsList2 = data.ReadInletProduct(inlets[i]);
				if (transitionsList2 != null)
				{
					transitionsList.Add(transitionsList2);
					flag = true;
				}
			}
			if (!flag)
			{
				data.StoreProduct(this, null);
			}
			else
			{
				data.StoreProduct(this, transitionsList);
			}
		}
	}
}
