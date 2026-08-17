using System.Collections.Generic;

namespace MapMagic.Nodes
{
	public interface IMultiOutlet
	{
		IEnumerable<IOutlet<object>> Outlets();
	}
}
