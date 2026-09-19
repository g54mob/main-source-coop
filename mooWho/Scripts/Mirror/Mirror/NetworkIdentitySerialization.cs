namespace Mirror
{
	public struct NetworkIdentitySerialization
	{
		public int tick;

		public NetworkWriter ownerWriter;

		public NetworkWriter observersWriter;

		public void ResetWriters()
		{
			ownerWriter.Position = 0;
			observersWriter.Position = 0;
		}
	}
}
