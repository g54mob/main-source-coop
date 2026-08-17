using System;
using Den.Tools;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Modifiers", name = "Spread", iconName = "GeneratorIcons/Spread", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/wikis/object_generators/Propagate")]
	public class Spread200 : Generator, IInlet<TransitionsList>, IUnit, IOutlet<TransitionsList>
	{
		public bool retainOriginals = true;

		public int seed = 12345;

		public Vector2 growth = new Vector2(2f, 3f);

		public Vector2 distance = new Vector2(1f, 2f);

		public float sizeFactor;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsModifiers.cs", 596);
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
			TransitionsList transitionsList2 = new TransitionsList();
			Noise random = new Noise(data.random, seed);
			for (int i = 0; i < transitionsList.count; i++)
			{
				Spread(transitionsList.arr[i], transitionsList2, random);
			}
			if (retainOriginals)
			{
				transitionsList2.Add(transitionsList);
			}
			data.StoreProduct(this, transitionsList2);
		}

		private void Spread(Transition trn, TransitionsList spreadedList, Noise random)
		{
			float num = random.Random(trn.hash);
			float num2 = growth.x + num * (growth.y - growth.x);
			num2 = num2 * (1f - sizeFactor) + num2 * trn.scale.x * sizeFactor;
			num2 = Mathf.Round(num2);
			for (int i = 0; (float)i < num2; i++)
			{
				float num3 = random.Random(trn.hash, i * 2);
				float num4 = (num = random.Random(trn.hash, i * 2 + 1));
				float f = num3 * (float)Math.PI * 2f;
				Vector2 vector = new Vector2(Mathf.Sin(f), Mathf.Cos(f));
				float num5 = distance.x + num4 * (distance.y - distance.x);
				num5 = num5 * (1f - sizeFactor) + num5 * trn.scale.x * sizeFactor;
				float x = trn.pos.x + vector.x * num5;
				float z = trn.pos.z + vector.y * num5;
				Transition trs = new Transition
				{
					pos = new Vector3(x, trn.pos.y, z),
					rotation = trn.rotation,
					scale = trn.scale,
					hash = (trn.hash << 1) + i
				};
				spreadedList.Add(trs);
			}
		}
	}
}
