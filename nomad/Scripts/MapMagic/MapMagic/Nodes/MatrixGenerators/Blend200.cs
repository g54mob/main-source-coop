using System;
using System.Collections.Generic;
using Den.Tools.Matrices;
using MapMagic.Products;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Modifiers", name = "Blend", iconName = "GeneratorIcons/Blend", disengageable = true, colorType = typeof(MatrixWorld), helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Blend")]
	public class Blend200 : Generator, IMultiInlet, IOutlet<MatrixWorld>, IUnit
	{
		public class Layer
		{
			public readonly Inlet<MatrixWorld> inlet = new Inlet<MatrixWorld>();

			public BlendAlgorithm algorithm = BlendAlgorithm.add;

			public float opacity = 1f;

			public bool guiExpanded;
		}

		public enum BlendAlgorithm
		{
			mix = 0,
			add = 1,
			subtract = 2,
			multiply = 3,
			divide = 4,
			difference = 5,
			min = 6,
			max = 7,
			overlay = 8,
			hardLight = 9,
			softLight = 10
		}

		public Layer[] layers = new Layer[2]
		{
			new Layer(),
			new Layer()
		};

		public Layer[] Layers => layers;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixModifiers.cs", 256);
		}

		public void SetLayers(object[] ls)
		{
			layers = Array.ConvertAll(ls, (object i) => (Layer)i);
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
			if ((stop != null && stop.stop) || !enabled)
			{
				return;
			}
			MatrixWorld matrixWorld = new MatrixWorld(data.area.full.rect, data.area.full.worldPos, data.area.full.worldSize, data.globals.height);
			if ((stop != null && stop.stop) || (stop != null && stop.stop))
			{
				return;
			}
			for (int i = 0; i < layers.Length; i++)
			{
				Layer layer = layers[i];
				if (layer.inlet != null)
				{
					MatrixWorld matrixWorld2 = data.ReadInletProduct(layer.inlet);
					if (matrixWorld2 != null)
					{
						Blend(matrixWorld, matrixWorld2, layer.algorithm, layer.opacity);
					}
				}
			}
			data.StoreProduct(this, matrixWorld);
		}

		public static void Blend(Matrix m1, Matrix m2, BlendAlgorithm algorithm, float opacity = 1f)
		{
			switch (algorithm)
			{
			default:
				m1.Mix(m2, opacity);
				break;
			case BlendAlgorithm.add:
				m1.Add(m2, opacity);
				break;
			case BlendAlgorithm.subtract:
				m1.Subtract(m2, opacity);
				break;
			case BlendAlgorithm.multiply:
				m1.Multiply(m2, opacity);
				break;
			case BlendAlgorithm.divide:
				m1.Divide(m2, opacity);
				break;
			case BlendAlgorithm.difference:
				m1.Difference(m2, opacity);
				break;
			case BlendAlgorithm.min:
				m1.Min(m2, opacity);
				break;
			case BlendAlgorithm.max:
				m1.Max(m2, opacity);
				break;
			case BlendAlgorithm.overlay:
				m1.Overlay(m2, opacity);
				break;
			case BlendAlgorithm.hardLight:
				m1.HardLight(m2, opacity);
				break;
			case BlendAlgorithm.softLight:
				m1.SoftLight(m2, opacity);
				break;
			}
		}
	}
}
