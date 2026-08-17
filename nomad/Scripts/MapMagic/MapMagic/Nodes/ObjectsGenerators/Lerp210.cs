using System;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Modifiers", name = "Lerp", iconName = "GeneratorIcons/ObjectsMask", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/wikis/object_generators/Clean_Up")]
	public class Lerp210 : Generator, IMultiInlet, IOutlet<TransitionsList>, IUnit
	{
		[Val("Original", "Inlet")]
		public readonly Inlet<TransitionsList> srcIn = new Inlet<TransitionsList>();

		[Val("Modified", "Inlet")]
		public readonly Inlet<TransitionsList> modIn = new Inlet<TransitionsList>();

		[Val("Mask", "Inlet")]
		public readonly Inlet<MatrixWorld> maskIn = new Inlet<MatrixWorld>();

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsModifiers.cs", 225);
		}

		public virtual IEnumerable<IInlet<object>> Inlets()
		{
			yield return srcIn;
			yield return modIn;
			yield return maskIn;
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (stop != null && stop.stop)
			{
				return;
			}
			TransitionsList transitionsList = data.ReadInletProduct(srcIn);
			if (transitionsList == null)
			{
				return;
			}
			TransitionsList transitionsList2 = data.ReadInletProduct(modIn);
			if (transitionsList2 == null)
			{
				return;
			}
			MatrixWorld matrixWorld = data.ReadInletProduct(maskIn);
			if (!enabled || matrixWorld == null)
			{
				data.StoreProduct(this, transitionsList2);
			}
			else if (stop == null || !stop.stop)
			{
				TransitionsList transitionsList3 = new TransitionsList(transitionsList.count);
				Lerp(transitionsList, transitionsList2, transitionsList3, matrixWorld);
				if (stop == null || !stop.stop)
				{
					data.StoreProduct(this, transitionsList3);
				}
			}
		}

		public void Lerp(TransitionsList list1, TransitionsList list2, TransitionsList dst, MatrixWorld mask)
		{
			Dictionary<int, Transition> dictionary = new Dictionary<int, Transition>();
			for (int i = 0; i < list1.count; i++)
			{
				dictionary.Add(list1.arr[i].id, list1.arr[i]);
			}
			for (int j = 0; j < list2.count; j++)
			{
				Transition transition = list2.arr[j];
				if (dictionary.TryGetValue(transition.id, out var value) && !(value.pos.x <= mask.worldPos.x) && !(value.pos.x >= mask.worldPos.x + mask.worldSize.x) && !(value.pos.z <= mask.worldPos.z) && !(value.pos.z >= mask.worldPos.z + mask.worldSize.z))
				{
					float worldInterpolatedValue = mask.GetWorldInterpolatedValue(value.pos.x, value.pos.z);
					value.pos = value.pos * (1f - worldInterpolatedValue) + transition.pos * worldInterpolatedValue;
					value.rotation = Quaternion.Lerp(value.rotation, transition.rotation, worldInterpolatedValue);
					value.scale = value.scale * worldInterpolatedValue + transition.scale * (1f - worldInterpolatedValue);
					dst.Add(value);
				}
			}
		}
	}
}
