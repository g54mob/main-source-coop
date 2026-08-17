using System;
using Den.Tools;
using UnityEngine;

namespace MapMagic.Nodes
{
	[Serializable]
	public class Inlet<T> : IInlet<T>, IUnit where T : class
	{
		[SerializeField]
		private Generator gen;

		public ulong id;

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
				if (id == 0L)
				{
					id = Den.Tools.Id.Generate();
				}
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

		public IUnit ShallowCopy()
		{
			return (Inlet<T>)MemberwiseClone();
		}
	}
}
