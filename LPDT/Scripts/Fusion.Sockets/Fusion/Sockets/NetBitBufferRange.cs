namespace Fusion.Sockets
{
	internal readonly struct NetBitBufferRange
	{
		public readonly int Offset;

		public readonly int Count;

		public NetBitBufferRange(int offset, int count)
		{
			Offset = offset;
			Count = count;
		}
	}
}
