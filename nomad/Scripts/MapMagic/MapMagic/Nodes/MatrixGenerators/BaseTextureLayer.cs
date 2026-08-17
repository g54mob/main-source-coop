using System;
using Den.Tools.Matrices;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	public class BaseTextureLayer : IInlet<MatrixWorld>, IUnit, IOutlet<MatrixWorld>
	{
		public string name = "Layer";

		public int channelNum;

		[SerializeField]
		private float opacity = 1f;

		public Generator gen;

		public ulong id;

		public float Opacity
		{
			get
			{
				return opacity;
			}
			set
			{
				opacity = value;
			}
		}

		public Generator Gen
		{
			get
			{
				return gen;
			}
			private set
			{
				gen = value;
			}
		}

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
			this.gen = gen;
		}

		public IUnit ShallowCopy()
		{
			return (BaseTextureLayer)MemberwiseClone();
		}
	}
}
