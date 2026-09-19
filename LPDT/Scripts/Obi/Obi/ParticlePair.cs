namespace Obi
{
	public struct ParticlePair
	{
		public int first;

		public int second;

		public int this[int index]
		{
			get
			{
				if (index != 0)
				{
					return second;
				}
				return first;
			}
			set
			{
				if (index == 0)
				{
					first = value;
				}
				else
				{
					second = value;
				}
			}
		}

		public ParticlePair(int first, int second)
		{
			this.first = first;
			this.second = second;
		}
	}
}
