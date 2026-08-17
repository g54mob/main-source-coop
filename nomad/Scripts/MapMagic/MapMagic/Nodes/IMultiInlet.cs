using System.Collections.Generic;

namespace MapMagic.Nodes
{
	public interface IMultiInlet
	{
		IEnumerable<IInlet<object>> Inlets();
	}
}
