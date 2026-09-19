namespace Obi
{
	public struct BIHNode
	{
		public int firstChild;

		public int start;

		public int count;

		public int axis;

		public float min;

		public float max;

		public BIHNode(int start, int count)
		{
			firstChild = -1;
			this.start = start;
			this.count = count;
			axis = 0;
			min = float.MinValue;
			max = float.MaxValue;
		}
	}
}
