namespace Ami.BroAudio.Runtime
{
	public struct ClipSelectionContext
	{
		public int Value { get; set; }

		public ClipSelectionContext(int value)
		{
			Value = value;
		}

		public static implicit operator ClipSelectionContext(int value)
		{
			return new ClipSelectionContext(value);
		}
	}
}
