namespace Features.SynchronizationGateModule
{
	public interface ISynchronizationGateLanes
	{
		int LaneCount { get; }

		int GetCurrentVisit(int lane);

		int GetPassedOnVisit(int lane);

		void SetPassedOnVisit(int lane, int visit);

		int GetWindowTicks(int lane);

		int GetWaitStartTick(int lane);

		void SetWaitStartTick(int lane, int tick);
	}
}
