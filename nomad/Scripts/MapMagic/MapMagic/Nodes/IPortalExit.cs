namespace MapMagic.Nodes
{
	public interface IPortalExit<out T> : IOutlet<T>, IUnit where T : class
	{
		void AssignEnter(IPortalEnter<object> enter, Graph graph);

		IPortalEnter<T> RefreshEnter(Graph graph);
	}
}
