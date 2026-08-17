using System;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	public abstract class BaseTexturesOutput<L> : OutputGenerator, IMultiLayer, IMultiInlet, IMultiOutlet where L : BaseTextureLayer, new()
	{
		public OutputLevel outputLevel = OutputLevel.Both;

		public L[] layers = new L[0];

		public override OutputLevel OutputLevel => outputLevel;

		public IList<IUnit> Layers
		{
			get
			{
				return layers;
			}
			set
			{
				layers = ArrayTools.Convert<L, IUnit>(value);
			}
		}

		public virtual bool Inversed => true;

		public virtual bool HideFirst => true;

		public abstract FinalizeAction FinalizeAction { get; }

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

		public MatrixWorld[] BaseGenerate(TileData data, StopToken stop)
		{
			if (layers.Length == 0)
			{
				return null;
			}
			MatrixWorld[] array = new MatrixWorld[layers.Length];
			float[] array2 = new float[layers.Length];
			if (stop != null && stop.stop)
			{
				return null;
			}
			for (int i = 0; i < layers.Length; i++)
			{
				if (stop != null && stop.stop)
				{
					return null;
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
				return null;
			}
			array.FillNulls(() => new MatrixWorld(data.area.full.rect, (Vector3)data.area.full.worldPos, (Vector3)data.area.full.worldSize));
			array[0].Fill(1f);
			Matrix[] matrices = array;
			Matrix.BlendLayers(matrices, array2);
			if (stop != null && stop.stop)
			{
				return null;
			}
			for (int num = 0; num < layers.Length; num++)
			{
				data.StoreProduct(layers[num], array[num]);
			}
			return array;
		}
	}
}
