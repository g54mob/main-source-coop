using System;

namespace MapMagic.Nodes
{
	[Serializable]
	public class Layer : IUnit
	{
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

		public ulong LinkedOutletId { get; set; }

		public ulong LinkedGenId { get; set; }

		public void SetGen(Generator gen)
		{
			Gen = gen;
		}

		public IUnit ShallowCopy()
		{
			return (Layer)MemberwiseClone();
		}
	}
}
