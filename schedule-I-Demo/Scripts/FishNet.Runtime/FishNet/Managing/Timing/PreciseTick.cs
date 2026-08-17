namespace FishNet.Managing.Timing
{
	public struct PreciseTick
	{
		public uint Tick;

		public double Percent;

		public PreciseTick(uint tick, double percent)
		{
			Tick = tick;
			Percent = percent;
		}

		public override string ToString()
		{
			return string.Format("Tick {0}, Percent {1}", Tick, Percent.ToString("000"));
		}
	}
}
