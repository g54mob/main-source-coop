using MapMagic.Products;

namespace MapMagic.Nodes
{
	public interface ICustomComplexity
	{
		float Complexity { get; }

		float Progress(TileData data);
	}
}
