using System.Text;

namespace Fusion
{
	internal readonly struct SimulationMessagePacketData
	{
		public readonly SimulationMessage Message;

		public readonly Tick Tick;

		public readonly ulong Sequence;

		public SimulationMessagePacketData(SimulationMessage message, Tick tick, ulong sequence)
		{
			Message = message;
			Tick = tick;
			Sequence = sequence;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[Msg: ").Append(Message);
			stringBuilder.Append(", Tick: ").Append(Tick.Raw);
			stringBuilder.Append(", Sequence: ").Append(Sequence);
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}
	}
}
