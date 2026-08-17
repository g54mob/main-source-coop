using MapMagic.Expose;
using MapMagic.Products;

namespace MapMagic.Nodes
{
	public interface IBiome : IUnit
	{
		Graph SubGraph { get; }

		Override Override { get; set; }

		TileData SubData(TileData parent);
	}
}
