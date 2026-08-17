using System.Collections.Generic;

namespace MapMagic.Nodes
{
	public interface IMultiLayer
	{
		IList<IUnit> Layers { get; set; }

		bool Inversed { get; }

		bool HideFirst { get; }
	}
}
