using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes
{
	public interface IPrepare
	{
		ulong Id { get; set; }

		void Prepare(TileData data, Terrain terrain);
	}
}
