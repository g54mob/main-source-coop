using Fusion;
using UnityEngine;

namespace Features.SynchronizationGateModule
{
	public class SynchronizationGateParticipant
	{
		private const string ARRIVAL_PREFAB_NAME = "SynchronizationGateArrivalObject";

		private static NetworkObject _arrivalPrefab;

		private readonly NetworkRunner _runner;

		private readonly ISynchronizationGateLanes _lanes;

		private readonly string _ownerId;

		private SynchronizationGateArrival _arrival;

		public SynchronizationGateParticipant(NetworkRunner runner, ISynchronizationGateLanes lanes, string ownerId)
		{
			_runner = runner;
			_lanes = lanes;
			_ownerId = ownerId;
		}

		public void Reconcile(int lane, bool isReady)
		{
			bool num = lane >= 0 && lane < _lanes.LaneCount;
			bool flag = HasLiveArrival();
			if (flag && _arrival.Lane != lane)
			{
				DespawnArrival();
				flag = false;
			}
			bool isGateActive = num && !SynchronizationGateResolver.IsPassed(_lanes.GetCurrentVisit(lane), _lanes.GetPassedOnVisit(lane));
			int num2 = (num ? _lanes.GetCurrentVisit(lane) : 0);
			int arrivalVisit = (flag ? _arrival.Visit : 0);
			switch (SynchronizationGateResolver.ResolveArrival(isGateActive, isReady, flag, arrivalVisit, num2))
			{
			case SynchronizationGateArrivalAction.Spawn:
				SpawnArrival(lane, num2);
				break;
			case SynchronizationGateArrivalAction.Despawn:
				DespawnArrival();
				break;
			}
		}

		public void Dispose()
		{
			DespawnArrival();
		}

		private bool HasLiveArrival()
		{
			if (_arrival != null && _arrival.Object != null)
			{
				return _arrival.Object.IsValid;
			}
			return false;
		}

		private void SpawnArrival(int lane, int visit)
		{
			NetworkObject networkObject;
			try
			{
				networkObject = _runner.Spawn(ResolveArrivalPrefab());
			}
			catch (NetworkObjectSpawnException)
			{
				return;
			}
			if (!(networkObject == null))
			{
				if (!networkObject.TryGetComponent<SynchronizationGateArrival>(out var component) || !component.TryInitialize(_ownerId, lane, visit))
				{
					_runner.Despawn(networkObject);
				}
				else
				{
					_arrival = component;
				}
			}
		}

		private void DespawnArrival()
		{
			if (HasLiveArrival())
			{
				_runner.Despawn(_arrival.Object);
			}
			_arrival = null;
		}

		private static NetworkObject ResolveArrivalPrefab()
		{
			if (_arrivalPrefab == null)
			{
				_arrivalPrefab = Resources.Load<NetworkObject>("SynchronizationGateArrivalObject");
			}
			return _arrivalPrefab;
		}
	}
}
