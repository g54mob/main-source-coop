namespace MapMagic.Nodes
{
	public interface IPortalEnter<out T> : IInlet<T>, IUnit where T : class
	{
		string Name { get; set; }
	}
}
