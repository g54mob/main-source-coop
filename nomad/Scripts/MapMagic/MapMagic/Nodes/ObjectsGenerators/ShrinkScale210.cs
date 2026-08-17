using System;
using Den.Tools;
using Den.Tools.GUI;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Modifiers", name = "Shrink Scale", section = 2, colorType = typeof(TransitionsList), iconName = "GeneratorIcons/ShrinkScale", helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/ShrinkScale")]
	public class ShrinkScale210 : Generator, IInlet<TransitionsList>, IUnit, IOutlet<TransitionsList>
	{
		[Val("Multiply")]
		public float multiply = 0.001f;

		[Val("Invert")]
		public bool invert;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsModifiers.cs", 1373);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (stop != null && stop.stop)
			{
				return;
			}
			TransitionsList transitionsList = data.ReadInletProduct(this);
			if (transitionsList == null || (stop != null && stop.stop))
			{
				return;
			}
			TransitionsList transitionsList2 = new TransitionsList(transitionsList);
			if (stop != null && stop.stop)
			{
				return;
			}
			if (!invert)
			{
				for (int i = 0; i < transitionsList2.count; i++)
				{
					transitionsList2.arr[i].scale = Vector3.one + transitionsList2.arr[i].scale * multiply;
				}
			}
			else
			{
				for (int j = 0; j < transitionsList2.count; j++)
				{
					transitionsList2.arr[j].scale = (transitionsList2.arr[j].scale - Vector3.one) / multiply;
				}
			}
			if (stop == null || !stop.stop)
			{
				data.StoreProduct(this, transitionsList2);
			}
		}
	}
}
