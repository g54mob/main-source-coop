using System;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Modifiers", name = "Normalize", disengageable = true, iconName = "GeneratorIcons/Normalize", drawInlets = false, drawOutlet = false, colorType = typeof(MatrixWorld), helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Normalize")]
	public class Normalize200 : Generator, IMultiInlet, IMultiOutlet
	{
		public class NormalizeLayer : IInlet<MatrixWorld>, IUnit, IOutlet<MatrixWorld>
		{
			public ulong id;

			public float Opacity { get; set; }

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

			public ulong LinkedOutletId { get; set; }

			public ulong LinkedGenId { get; set; }

			public void SetGen(Generator gen)
			{
				Gen = gen;
			}

			public NormalizeLayer(Generator gen)
			{
				Gen = gen;
			}

			public NormalizeLayer()
			{
				Opacity = 1f;
			}

			public IUnit ShallowCopy()
			{
				return (NormalizeLayer)MemberwiseClone();
			}
		}

		public NormalizeLayer[] layers = new NormalizeLayer[0];

		public NormalizeLayer[] Layers => layers;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixModifiers.cs", 345);
		}

		public void SetLayers(object[] ls)
		{
			layers = Array.ConvertAll(ls, (object i) => (NormalizeLayer)i);
		}

		public IEnumerable<IInlet<object>> Inlets()
		{
			for (int i = 0; i < layers.Length; i++)
			{
				yield return layers[i];
			}
		}

		public IEnumerable<IOutlet<object>> Outlets()
		{
			for (int i = 0; i < layers.Length; i++)
			{
				yield return layers[i];
			}
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (layers.Length == 0)
			{
				return;
			}
			MatrixWorld[] array = new MatrixWorld[layers.Length];
			float[] array2 = new float[layers.Length];
			if (stop != null && stop.stop)
			{
				return;
			}
			for (int i = 0; i < layers.Length; i++)
			{
				if (stop != null && stop.stop)
				{
					return;
				}
				MatrixWorld matrixWorld = data.ReadInletProduct(layers[i]);
				if (matrixWorld != null)
				{
					array[i] = new MatrixWorld(matrixWorld);
				}
				else
				{
					array[i] = new MatrixWorld(data.area.full.rect, (Vector3)data.area.full.worldPos, (Vector3)data.area.full.worldSize);
				}
				array2[i] = layers[i].Opacity;
			}
			if (stop != null && stop.stop)
			{
				return;
			}
			array.FillNulls(() => new MatrixWorld(data.area.full.rect, (Vector3)data.area.full.worldPos, (Vector3)data.area.full.worldSize));
			array[0].Fill(1f);
			Matrix[] matrices = array;
			Matrix.BlendLayers(matrices, array2);
			if (stop == null || !stop.stop)
			{
				for (int num = 0; num < layers.Length; num++)
				{
					data.StoreProduct(layers[num], array[num]);
				}
			}
		}
	}
}
