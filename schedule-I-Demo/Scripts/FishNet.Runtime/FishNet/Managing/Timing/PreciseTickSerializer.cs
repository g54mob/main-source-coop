using FishNet.Serializing;
using GameKit.Utilities;

namespace FishNet.Managing.Timing
{
	public static class PreciseTickSerializer
	{
		public static void WritePreciseTick(this Writer writer, PreciseTick value)
		{
			writer.WriteTickUnpacked(value.Tick);
			byte value2 = (byte)(Maths.ClampDouble(value.Percent, 0.0, 1.0) * 100.0);
			writer.WriteByte(value2);
		}

		public static PreciseTick ReadPreciseTick(this Reader reader)
		{
			uint tick = reader.ReadTickUnpacked();
			double percent = Maths.ClampDouble((float)(int)reader.ReadByte() / 100f, 0.0, 1.0);
			return new PreciseTick(tick, percent);
		}
	}
}
