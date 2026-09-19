using System;

namespace Features.SynchronizationGateModule
{
	public static class SynchronizationGateResolver
	{
		public const int WAIT_NOT_STARTED = -1;

		public static int NextWaitStartTick(int arrivedCount, int currentStartTick, int currentTick)
		{
			if (arrivedCount <= 0)
			{
				return -1;
			}
			if (currentStartTick < 0)
			{
				return currentTick;
			}
			return currentStartTick;
		}

		public static bool IsPassed(int currentVisit, int passedOnVisit)
		{
			return passedOnVisit >= currentVisit;
		}

		public static SynchronizationGateState ResolveCohort(int participantCount, int arrivedCount, int currentTick, int startTick, int windowTicks)
		{
			int num = currentTick - startTick;
			int ticksRemaining = ((windowTicks > 0) ? Math.Max(0, windowTicks - num) : 0);
			if (participantCount > 0 && arrivedCount >= participantCount)
			{
				return new SynchronizationGateState(GateStateType.Passed, arrivedCount, participantCount, ticksRemaining);
			}
			if (windowTicks > 0 && num > windowTicks)
			{
				return new SynchronizationGateState(GateStateType.TimedOut, arrivedCount, participantCount, 0);
			}
			return new SynchronizationGateState(GateStateType.Pending, arrivedCount, participantCount, ticksRemaining);
		}

		public static SynchronizationGateArrivalAction ResolveArrival(bool isGateActive, bool isReady, bool hasArrival, int arrivalVisit, int currentVisit)
		{
			if (hasArrival)
			{
				if (!isGateActive || !isReady || arrivalVisit != currentVisit)
				{
					return SynchronizationGateArrivalAction.Despawn;
				}
				return SynchronizationGateArrivalAction.None;
			}
			if (isGateActive && isReady)
			{
				return SynchronizationGateArrivalAction.Spawn;
			}
			return SynchronizationGateArrivalAction.None;
		}
	}
}
