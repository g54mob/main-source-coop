using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Fusion
{
	internal class SimulationMessage : ILogDumpable
	{
		public SimulationMessageHeader Header;

		public int RefCount;

		private PooledList<byte> _payload;

		public Span<byte> Payload
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _payload.AsSpan();
			}
		}

		public void Dump(StringBuilder builder)
		{
			builder.Append($"{Header}:{RefCount}");
		}

		internal void PoolInit(Simulation simulation, in SimulationMessageHeader header)
		{
			Header = header;
			RefCount = 1;
			_payload = simulation.AcquireByteList(header.PayloadNumBytes);
			_payload.Resize(header.PayloadNumBytes);
		}

		public void PoolReset(Simulation simulation)
		{
			Assert.Always(RefCount == 0, "RefCount == 0");
			simulation.Release(ref _payload);
			Header = default(SimulationMessageHeader);
			_payload = default(PooledList<byte>);
		}
	}
}
