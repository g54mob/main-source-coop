using UnityEngine;

namespace MapMagic.Nodes
{
	public interface IApplyData
	{
		int Resolution { get; }

		void Apply(Terrain terrain);
	}
}
