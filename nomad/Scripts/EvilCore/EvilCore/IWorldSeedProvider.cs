namespace EvilCore
{
	public interface IWorldSeedProvider
	{
		int Seed { get; }

		bool HasSeed { get; }

		void SetSeed(int seed);

		void Clear();
	}
}
