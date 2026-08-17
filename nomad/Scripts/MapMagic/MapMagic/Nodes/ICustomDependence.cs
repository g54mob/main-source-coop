using System.Collections.Generic;

namespace MapMagic.Nodes
{
	public interface ICustomDependence
	{
		IEnumerable<Generator> PriorGens();
	}
}
