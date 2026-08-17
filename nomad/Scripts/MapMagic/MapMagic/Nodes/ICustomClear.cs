using MapMagic.Products;

namespace MapMagic.Nodes
{
	public interface ICustomClear
	{
		void OnClearing(Graph graph, TileData data, ref bool isReady, bool totalRebuild = false);
	}
}
