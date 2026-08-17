using System;
using Den.Tools;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Initial", name = "Positions", iconName = "GeneratorIcons/Position", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/wikis/object_generators/Scatter")]
	public class Positions200 : Generator, IOutlet<TransitionsList>, IUnit
	{
		public Vector3[] positions = new Vector3[1];

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsInitial.cs", 17);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (enabled)
			{
				TransitionsList transitionsList = new TransitionsList();
				for (int i = 0; i < positions.Length; i++)
				{
					Transition trs = new Transition(positions[i].x, positions[i].y, positions[i].z);
					transitionsList.Add(trs);
				}
				data.StoreProduct(this, transitionsList);
			}
		}
	}
}
