using System;
using Den.Tools;
using Den.Tools.GUI;
using MapMagic.Products;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Modifiers", name = "Randomize", iconName = "GeneratorIcons/Random", colorType = typeof(TransitionsList), disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/ObjectsGenerators/Randomize")]
	public class Randomize211 : Generator, IInlet<TransitionsList>, IUnit, IOutlet<TransitionsList>
	{
		[Val("Seed")]
		public int seed = 12345;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsModifiers.cs", 135);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			TransitionsList transitionsList = data.ReadInletProduct(this);
			if (transitionsList == null)
			{
				return;
			}
			if (!enabled)
			{
				data.StoreProduct(this, transitionsList);
				return;
			}
			TransitionsList transitionsList2 = new TransitionsList(transitionsList);
			Noise noise = new Noise(data.random, seed);
			for (int i = 0; i < transitionsList2.count; i++)
			{
				if (stop != null && stop.stop)
				{
					return;
				}
				transitionsList2.arr[i].id = i;
				transitionsList2.arr[i].hash = noise.RandomInt(i);
			}
			data.StoreProduct(this, transitionsList2);
		}
	}
}
