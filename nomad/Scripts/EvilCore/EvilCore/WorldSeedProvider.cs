namespace EvilCore
{
	public class WorldSeedProvider : IWorldSeedProvider
	{
		public int Seed { get; private set; }

		public bool HasSeed { get; private set; }

		public void SetSeed(int seed)
		{
			Seed = seed;
			HasSeed = true;
		}

		public void Clear()
		{
			Seed = 0;
			HasSeed = false;
		}
	}
}
