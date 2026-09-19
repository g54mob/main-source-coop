using System;
using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	[NetworkStructWeaved(1)]
	public struct TickTimer : INetworkStruct
	{
		[FieldOffset(0)]
		private int _target;

		public const int SIZE = 4;

		public const int WORD_COUNT = 1;

		internal const int BYTE_OF__TARGET = 0;

		internal const int BYTE_COUNT_OF__TARGET = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public static TickTimer None => default(TickTimer);

		public readonly bool IsRunning => _target > 0;

		public readonly int? TargetTick => (_target > 0) ? _target : 0;

		public bool Expired(NetworkRunner runner)
		{
			return BehaviourUtils.IsAlive(runner) && runner.IsRunning && _target > 0 && _target <= runner.Simulation.Tick;
		}

		public bool ExpiredOrNotRunning(NetworkRunner runner)
		{
			return _target == 0 || !runner.IsRunning || Expired(runner);
		}

		public int? RemainingTicks(NetworkRunner runner)
		{
			if (BehaviourUtils.IsNotAlive(runner) || !runner.IsRunning)
			{
				return null;
			}
			if (IsRunning)
			{
				return Math.Max(0, _target - (int)runner.Simulation.Tick);
			}
			return null;
		}

		public float? RemainingTime(NetworkRunner runner)
		{
			int? num = RemainingTicks(runner);
			if (num.HasValue)
			{
				return (float)num.Value * runner.DeltaTime;
			}
			return null;
		}

		public static TickTimer CreateFromSeconds(NetworkRunner runner, float delayInSeconds)
		{
			if (BehaviourUtils.IsNotAlive(runner) || !runner.IsRunning)
			{
				return default(TickTimer);
			}
			TickTimer result = default(TickTimer);
			result._target = (int)runner.Simulation.Tick + (int)Math.Ceiling(delayInSeconds / runner.DeltaTime);
			return result;
		}

		public static TickTimer CreateFromTicks(NetworkRunner runner, int ticks)
		{
			if (BehaviourUtils.IsNotAlive(runner) || !runner.IsRunning)
			{
				return default(TickTimer);
			}
			TickTimer result = default(TickTimer);
			result._target = (int)runner.Simulation.Tick + ticks;
			return result;
		}

		public override readonly string ToString()
		{
			return _target.ToString();
		}
	}
}
