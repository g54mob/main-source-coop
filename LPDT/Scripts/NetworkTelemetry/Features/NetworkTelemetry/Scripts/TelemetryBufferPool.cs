using System.Collections.Concurrent;

namespace Features.NetworkTelemetry.Scripts
{
	public class TelemetryBufferPool
	{
		private readonly ConcurrentBag<TelemetryPacket> _available = new ConcurrentBag<TelemetryPacket>();

		public TelemetryBufferPool(int initialSize)
		{
			for (int i = 0; i < initialSize; i++)
			{
				_available.Add(new TelemetryPacket());
			}
		}

		public TelemetryPacket Rent()
		{
			if (_available.TryTake(out var result))
			{
				return result;
			}
			return new TelemetryPacket();
		}

		public void Return(TelemetryPacket packet)
		{
			packet.Clear();
			_available.Add(packet);
		}
	}
}
