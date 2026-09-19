#define DEBUG
namespace Fusion.Sockets
{
	internal struct NetSequencer
	{
		private int _shift;

		private int _bytes;

		private ulong _mask;

		private ulong _sequence;

		public readonly int Bits => _bytes * 8;

		public readonly int Bytes => _bytes;

		public ulong Sequence
		{
			readonly get
			{
				return _sequence;
			}
			set
			{
				Assert.Check(value <= _mask, "value <= _mask");
				_sequence = value & _mask;
			}
		}

		public void Reset()
		{
			_sequence = 0uL;
		}

		public NetSequencer(int bytes)
		{
			_bytes = bytes;
			_sequence = 0uL;
			_mask = (ulong)((1L << bytes * 8) - 1);
			_shift = (8 - bytes) * 8;
		}

		public ulong Next()
		{
			return _sequence = NextAfter(_sequence);
		}

		public ulong NextAfter(ulong sequence)
		{
			return (sequence + 1) & _mask;
		}

		public int Distance(ulong from, ulong to)
		{
			to <<= _shift;
			from <<= _shift;
			long num = (long)(from - to) >> _shift;
			Assert.Check(num >= int.MinValue && num <= int.MaxValue, "r >= int.MinValue && r <= int.MaxValue");
			return (int)num;
		}
	}
}
