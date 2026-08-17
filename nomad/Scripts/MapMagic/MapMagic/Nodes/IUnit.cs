namespace MapMagic.Nodes
{
	public interface IUnit
	{
		Generator Gen { get; }

		ulong Id { get; set; }

		void SetGen(Generator gen);

		IUnit ShallowCopy();
	}
}
