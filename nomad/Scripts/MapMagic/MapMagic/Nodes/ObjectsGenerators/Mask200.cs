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
	[GeneratorMenu(menu = "Objects/Modifiers", name = "Mask", iconName = "GeneratorIcons/ObjectsMask", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/wikis/object_generators/Clean_Up")]
	public class Mask200 : Generator, IMultiInlet, IOutlet<TransitionsList>, IUnit
	{
		[Val("Masked", "Inlet")]
		public readonly Inlet<TransitionsList> srcIn = new Inlet<TransitionsList>();

		[Val("Unmasked", "Inlet")]
		public readonly Inlet<TransitionsList> invIn = new Inlet<TransitionsList>();

		[Val("Mask", "Inlet")]
		public readonly Inlet<MatrixWorld> maskIn = new Inlet<MatrixWorld>();

		[Val("Seed")]
		public int seed = 12345;

		[Val("Invert")]
		public bool invert;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsModifiers.cs", 164);
		}

		public IEnumerable<IInlet<object>> Inlets()
		{
			yield return srcIn;
			yield return invIn;
			yield return maskIn;
		}

		public override void Generate(TileData data, StopToken stop)
		{
			TransitionsList transitionsList = data.ReadInletProduct(srcIn);
			TransitionsList transitionsList2 = data.ReadInletProduct(invIn);
			if (transitionsList == null && transitionsList2 == null)
			{
				return;
			}
			if (!enabled)
			{
				data.StoreProduct(this, transitionsList ?? transitionsList2);
				return;
			}
			MatrixWorld matrixWorld = data.ReadInletProduct(maskIn);
			if (matrixWorld == null)
			{
				data.StoreProduct(this, transitionsList);
				return;
			}
			TransitionsList transitionsList3 = new TransitionsList();
			Noise random = new Noise(data.random, seed);
			if (transitionsList != null)
			{
				Mask(transitionsList, transitionsList3, matrixWorld, random, invert, stop);
			}
			if (transitionsList2 != null)
			{
				Mask(transitionsList2, transitionsList3, matrixWorld, random, !invert, stop);
			}
			data.StoreProduct(this, transitionsList3);
		}

		public static void Mask(TransitionsList src, TransitionsList dst, MatrixWorld mask, Noise random, bool invert, StopToken stop = null)
		{
			for (int i = 0; i < src.count; i++)
			{
				if (stop != null && stop.stop)
				{
					break;
				}
				Vector3 pos = src.arr[i].pos;
				if (!(pos.x <= mask.worldPos.x) && !(pos.x >= mask.worldPos.x + mask.worldSize.x) && !(pos.z <= mask.worldPos.z) && !(pos.z >= mask.worldPos.z + mask.worldSize.z))
				{
					float worldValue = mask.GetWorldValue(pos.x, pos.z);
					float num = random.Random(src.arr[i].hash);
					if (worldValue < num && invert)
					{
						dst.Add(src.arr[i]);
					}
					if (worldValue > num && !invert)
					{
						dst.Add(src.arr[i]);
					}
				}
			}
		}
	}
}
