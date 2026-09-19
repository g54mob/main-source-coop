namespace Obi
{
	public struct HeightFieldHeader
	{
		public int firstSample;

		public int sampleCount;

		public HeightFieldHeader(int firstSample, int sampleCount)
		{
			this.firstSample = firstSample;
			this.sampleCount = sampleCount;
		}
	}
}
