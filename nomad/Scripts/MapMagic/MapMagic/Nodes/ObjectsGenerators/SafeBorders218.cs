using System;
using Den.Tools;
using Den.Tools.GUI;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Modifiers", name = "Safe Borders", section = 2, colorType = typeof(TransitionsList), iconName = "GeneratorIcons/SafeBorders", helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/ShrinkScale")]
	public class SafeBorders218 : Generator, IInlet<TransitionsList>, IUnit, IOutlet<TransitionsList>
	{
		[Val("Edge Dist")]
		public float edgeDist = 10f;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsModifiers.cs", 1412);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (stop != null && stop.stop)
			{
				return;
			}
			TransitionsList transitionsList = data.ReadInletProduct(this);
			if (transitionsList == null)
			{
				return;
			}
			Vector2D vector2D = data.area.active.worldPos + edgeDist;
			Vector2D vector2D2 = data.area.active.worldPos + data.area.active.worldSize - edgeDist;
			TransitionsList transitionsList2 = new TransitionsList();
			for (int i = 0; i < transitionsList.count; i++)
			{
				if (stop != null && stop.stop)
				{
					return;
				}
				Vector3 pos = transitionsList.arr[i].pos;
				if (!(pos.x <= vector2D.x) && !(pos.x >= vector2D2.x) && !(pos.z <= vector2D.z) && !(pos.z >= vector2D2.z))
				{
					transitionsList2.Add(transitionsList.arr[i]);
				}
			}
			if (stop == null || !stop.stop)
			{
				data.StoreProduct(this, transitionsList2);
			}
		}
	}
}
