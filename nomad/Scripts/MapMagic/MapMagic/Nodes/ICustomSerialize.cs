namespace MapMagic.Nodes
{
	public interface ICustomSerialize
	{
		void OnBeforeSerialize(Graph graph);

		void OnAfterDeserialize(Graph graph);
	}
}
