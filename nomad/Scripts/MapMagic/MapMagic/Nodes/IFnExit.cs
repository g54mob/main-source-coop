namespace MapMagic.Nodes
{
	public interface IFnExit<out T> : IFnPortal<T>, IInlet<T>, IUnit, IRelevant where T : class
	{
	}
}
