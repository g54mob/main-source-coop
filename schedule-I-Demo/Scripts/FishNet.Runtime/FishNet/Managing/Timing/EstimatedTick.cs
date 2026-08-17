using System.Runtime.CompilerServices;

namespace FishNet.Managing.Timing
{
	public struct EstimatedTick
	{
		public enum OldTickOption : byte
		{
			Discard = 0,
			SetLastRemoteTick = 1,
			SetRemoteTick = 2
		}

		public uint LocalTick;

		public uint RemoteTick;

		public uint LastRemoteTick;

		public bool IsUnset => LocalTick == 0;

		public bool IsCurrent(TimeManager tm)
		{
			if (!IsUnset)
			{
				return LocalTick == tm.LocalTick;
			}
			return false;
		}

		public uint LocalTickDifference(TimeManager tm)
		{
			long num = tm.LocalTick - LocalTick;
			if (num < 0)
			{
				return 0u;
			}
			if (num > uint.MaxValue)
			{
				num = 4294967295L;
			}
			return (uint)num;
		}

		public bool Update(TimeManager tm, uint remoteTick, OldTickOption oldTickOption = OldTickOption.Discard)
		{
			LastRemoteTick = remoteTick;
			if (oldTickOption != OldTickOption.SetRemoteTick && remoteTick <= RemoteTick)
			{
				return false;
			}
			LocalTick = tm.LocalTick;
			RemoteTick = remoteTick;
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint Value(TimeManager tm)
		{
			bool isCurrent;
			return Value(tm, out isCurrent);
		}

		public uint Value(TimeManager tm, out bool isCurrent)
		{
			isCurrent = IsCurrent(tm);
			if (tm == null)
			{
				return 0u;
			}
			if (IsUnset)
			{
				return 0u;
			}
			return tm.LocalTick - LocalTick + RemoteTick;
		}

		public void Reset()
		{
			LocalTick = 0u;
			RemoteTick = 0u;
			LastRemoteTick = 0u;
		}
	}
}
