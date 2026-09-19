using System.Collections.Generic;
using System.Linq;
using Fusion;

namespace Features.SynchronizationGateModule
{
	public class SynchronizationGateAuthority
	{
		private readonly NetworkRunner _runner;

		private readonly ISynchronizationGateLanes _lanes;

		public SynchronizationGateAuthority(NetworkRunner runner, ISynchronizationGateLanes lanes)
		{
			_runner = runner;
			_lanes = lanes;
		}

		public int CountPresentParticipants()
		{
			return _runner.ActivePlayers.Count();
		}

		public int CountArrivals(int lane)
		{
			int currentVisit = _lanes.GetCurrentVisit(lane);
			HashSet<string> hashSet = null;
			foreach (NetworkObject allNetworkObject in _runner.GetAllNetworkObjects())
			{
				if (allNetworkObject.TryGetComponent<SynchronizationGateArrival>(out var component) && component.Lane == lane && component.Visit == currentVisit)
				{
					if (hashSet == null)
					{
						hashSet = new HashSet<string>();
					}
					hashSet.Add(component.OwnerIdValue);
				}
			}
			return hashSet?.Count ?? 0;
		}

		public void ResolveLanes()
		{
			int num = _runner.Tick;
			for (int i = 0; i < _lanes.LaneCount; i++)
			{
				if (!SynchronizationGateResolver.IsPassed(_lanes.GetCurrentVisit(i), _lanes.GetPassedOnVisit(i)))
				{
					int arrivedCount = CountArrivals(i);
					int waitStartTick = _lanes.GetWaitStartTick(i);
					int num2 = SynchronizationGateResolver.NextWaitStartTick(arrivedCount, waitStartTick, num);
					if (num2 != waitStartTick)
					{
						_lanes.SetWaitStartTick(i, num2);
					}
					int startTick = ((num2 >= 0) ? num2 : num);
					if (SynchronizationGateResolver.ResolveCohort(CountPresentParticipants(), arrivedCount, num, startTick, _lanes.GetWindowTicks(i)).IsResolved)
					{
						_lanes.SetPassedOnVisit(i, _lanes.GetCurrentVisit(i));
						_lanes.SetWaitStartTick(i, -1);
					}
				}
			}
		}
	}
}
