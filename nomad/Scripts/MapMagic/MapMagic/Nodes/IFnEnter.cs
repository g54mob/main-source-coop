namespace MapMagic.Nodes
{
	public interface IFnEnter<out T> : IFnPortal<T>, IOutlet<T>, IUnit where T : class
	{
	}
}
