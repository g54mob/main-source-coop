namespace Obi
{
	public interface IObiPathDataChannel
	{
		int Count { get; }

		bool Dirty { get; }

		void Clean();

		void RemoveAt(int index);
	}
}
