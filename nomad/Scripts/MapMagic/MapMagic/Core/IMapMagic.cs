using MapMagic.Nodes;

namespace MapMagic.Core
{
	public interface IMapMagic
	{
		Graph Graph { get; }

		Globals Globals { get; }

		void Refresh(bool clearAll);

		float GetProgress();

		bool IsGenerating();

		bool ContainsGraph(Graph graph);
	}
}
