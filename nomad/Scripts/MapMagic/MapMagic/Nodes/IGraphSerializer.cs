namespace MapMagic.Nodes
{
	public interface IGraphSerializer
	{
		void Serialize(Graph graph);

		void Deserialize(Graph graph);
	}
}
