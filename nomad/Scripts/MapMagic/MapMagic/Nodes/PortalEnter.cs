using System;
using MapMagic.Products;

namespace MapMagic.Nodes
{
	[Serializable]
	[GeneratorMenu(name = "Generic Portal Enter")]
	public class PortalEnter<T> : Generator, IInlet<T>, IUnit, IPortalEnter<T> where T : class, ICloneable
	{
		public string name = "Portal";

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public override void Generate(TileData data, StopToken stop)
		{
		}
	}
}
