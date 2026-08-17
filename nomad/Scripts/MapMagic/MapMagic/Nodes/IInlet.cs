namespace MapMagic.Nodes
{
	public interface IInlet<out T> : IUnit where T : class
	{
		ulong LinkedOutletId { get; set; }

		ulong LinkedGenId { get; set; }
	}
}
