using System;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.GUI;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Modifiers", name = "Rarefy", iconName = "GeneratorIcons/Rarefy", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/wikis/object_generators/Rerefy")]
	public class Rarefy200 : Generator, IInlet<TransitionsList>, IUnit, IMultiInlet, IOutlet<TransitionsList>
	{
		[Serializable]
		public struct Layer
		{
			public Inlet<TransitionsList> inlet;

			public ulong id;

			[Val("Dist")]
			public float distance;

			[Val("SF")]
			public float sizeFactor;

			public Layer(Generator gen)
			{
				id = Den.Tools.Id.Generate();
				inlet = new Inlet<TransitionsList>();
				distance = 1f;
				sizeFactor = 0f;
			}
		}

		public Layer[] layers = new Layer[0];

		public bool self = true;

		public float distance = 1f;

		public float sizeFactor;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsModifiers.cs", 472);
		}

		public IEnumerable<IInlet<object>> Inlets()
		{
			for (int i = 0; i < layers.Length; i++)
			{
				yield return layers[i].inlet;
			}
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
			PosTab posTab = new PosTab((Vector3)data.area.full.worldPos, (Vector3)data.area.full.worldSize, 16);
			posTab.Add(transitionsList);
			(PosTab, float, float)[] array = new(PosTab, float, float)[layers.Length];
			for (int i = 0; i < array.Length; i++)
			{
				TransitionsList transitionsList2 = data.ReadInletProduct(layers[i].inlet);
				if (transitionsList2 != null)
				{
					array[i].Item1 = new PosTab((Vector3)data.area.full.worldPos, (Vector3)data.area.full.worldSize, 16);
					array[i].Item1.Add(transitionsList2);
					array[i].Item2 = layers[i].distance;
					array[i].Item3 = layers[i].sizeFactor;
				}
			}
			if (stop == null || !stop.stop)
			{
				PosTab posTab2 = Rarefied(posTab, array, stop);
				data.StoreProduct(this, posTab2.ToTransitionsList());
			}
		}

		private PosTab Rarefied(PosTab src, (PosTab posTab, float distance, float sizeFactor)[] subtrahends, StopToken stop = null)
		{
			PosTab posTab = new PosTab(src.pos, src.size, src.resolution);
			foreach (Transition item in src.All())
			{
				if (stop != null && stop.stop)
				{
					return posTab;
				}
				float num = distance * (1f - sizeFactor) + distance * item.scale.x * sizeFactor;
				if (self && posTab.IsAnyObjInRange(item.pos.x, item.pos.z, num + num))
				{
					continue;
				}
				bool flag = false;
				for (int i = 0; i < subtrahends.Length; i++)
				{
					if (subtrahends[i].posTab != null && subtrahends[i].posTab.IsAnyObjInRange(item.pos.x, item.pos.z, num + subtrahends[i].distance))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					posTab.Add(item);
				}
			}
			posTab.Flush();
			return posTab;
		}
	}
}
