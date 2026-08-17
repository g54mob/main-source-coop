using System;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.GUI;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Modifiers", name = "Split", iconName = "GeneratorIcons/Split", colorType = typeof(TransitionsList), disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/ObjectsGenerators/Split")]
	public class Split200 : Generator, IInlet<TransitionsList>, IUnit, IMultiLayer
	{
		public class SplitLayer : IOutlet<TransitionsList>, IUnit
		{
			public string name = "Object Layer";

			public bool heightConditionActive;

			public Vector2 heightCondition = new Vector2(0f, 1f);

			public bool rotationConditionActive;

			public Vector2 rotationCondition = new Vector2(0f, 360f);

			public bool scaleConditionActive;

			public Vector2 scaleCondition = new Vector2(0f, 100f);

			public float chance = 1f;

			public ulong id;

			public Generator Gen { get; private set; }

			public ulong Id
			{
				get
				{
					return id;
				}
				set
				{
					id = value;
				}
			}

			public void SetGen(Generator gen)
			{
				Gen = gen;
			}

			public IUnit ShallowCopy()
			{
				return (SplitLayer)MemberwiseClone();
			}
		}

		public enum MatchType
		{
			layered = 0,
			random = 1
		}

		public SplitLayer[] layers = new SplitLayer[0];

		public int guiExpanded = -1;

		[Val("Match")]
		public MatchType matchType = MatchType.random;

		[Val("Seed")]
		public int seed = 12345;

		public IList<IUnit> Layers
		{
			get
			{
				return layers;
			}
			set
			{
			}
		}

		public bool Inversed => true;

		public bool HideFirst => false;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsModifiers.cs", 339);
		}

		public void SetLayers(object[] ls)
		{
			layers = Array.ConvertAll(ls, (object i) => (SplitLayer)i);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (stop != null && stop.stop)
			{
				return;
			}
			TransitionsList transitionsList = data.ReadInletProduct(this);
			if (transitionsList == null || !enabled)
			{
				return;
			}
			Noise random = new Noise(data.random, seed);
			bool[] match = new bool[layers.Length];
			TransitionsList[] array = new TransitionsList[layers.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new TransitionsList();
			}
			for (int j = 0; j < transitionsList.count; j++)
			{
				if (stop != null && stop.stop)
				{
					return;
				}
				int num = PickObjLayer(transitionsList.arr[j], random, match);
				if (num >= 0)
				{
					array[num].Add(transitionsList.arr[j]);
				}
			}
			for (int k = 0; k < array.Length; k++)
			{
				if (stop != null && stop.stop)
				{
					break;
				}
				data.StoreProduct(layers[k], array[k]);
			}
		}

		private int PickObjLayer(Transition trs, Noise random, bool[] match = null)
		{
			if (match == null)
			{
				match = new bool[layers.Length];
			}
			int num = 0;
			float num2 = 0f;
			int result = 0;
			for (int i = 0; i < layers.Length; i++)
			{
				SplitLayer splitLayer = layers[i];
				float num3 = (trs.Yaw + 360f) % 360f;
				bool num4 = !splitLayer.heightConditionActive || (trs.pos.y >= splitLayer.heightCondition.x && trs.pos.y <= splitLayer.heightCondition.y);
				bool flag = !splitLayer.rotationConditionActive || (num3 >= splitLayer.rotationCondition.x && num3 <= splitLayer.rotationCondition.y);
				bool flag2 = !splitLayer.scaleConditionActive || (trs.scale.x >= splitLayer.scaleCondition.x && trs.scale.x <= splitLayer.scaleCondition.y);
				if (num4 && flag && flag2)
				{
					match[i] = true;
					num++;
					num2 += splitLayer.chance;
					result = i;
				}
				else
				{
					match[i] = false;
				}
			}
			switch (num)
			{
			case 0:
				return -1;
			default:
				if (matchType != MatchType.layered)
				{
					if (num > 1 && matchType == MatchType.random)
					{
						float num5 = random.Random(trs.hash);
						num5 *= num2;
						num2 = 0f;
						for (int j = 0; j < layers.Length; j++)
						{
							if (match[j])
							{
								SplitLayer splitLayer2 = layers[j];
								if (num5 > num2 && num5 < num2 + splitLayer2.chance)
								{
									return j;
								}
								num2 += splitLayer2.chance;
							}
						}
					}
					return -1;
				}
				goto case 1;
			case 1:
				return result;
			}
		}
	}
}
