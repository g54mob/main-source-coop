using System;
using System.Collections.Generic;
using System.Linq;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.SynchronizedModelsModule.Scripts.DataStreamingSynchronizer;
using UnityEngine;

namespace Features.NetworkRandomModule.Scripts
{
	[Serializable]
	public class NetworkRandomRequestModel : DataStreamSynchronizableBaseWithCustomData<NetworkRandomRequestModel, NetworkRandomInjectRequest>, ISessionCleanup
	{
		[field: SerializeField]
		public List<NetworkRandomInjectRequest> Requests { get; private set; } = new List<NetworkRandomInjectRequest>();

		public override bool IsNeedToSynchronizeOnSpawn => true;

		public event Action<NetworkRandomInjectRequest> OnRequestAdded;

		public void AddRequest(int consumerId, int seed, bool synchronize = false)
		{
			AddRequest(new NetworkRandomInjectRequest(consumerId, seed), synchronize);
		}

		public void AddRequest(NetworkRandomInjectRequest request, bool synchronize = false)
		{
			if (TryAddRequest(request) && synchronize)
			{
				base.Data1 = request;
				CustomSynchronize();
			}
		}

		protected override void SetNewValues(NetworkRandomRequestModel model)
		{
			List<NetworkRandomInjectRequest> list = model.Requests.Where((NetworkRandomInjectRequest request) => Requests.All((NetworkRandomInjectRequest existingRequest) => !IsSameRequest(existingRequest, request))).ToList();
			Requests = model.Requests;
			foreach (NetworkRandomInjectRequest item in list)
			{
				this.OnRequestAdded?.Invoke(item);
			}
		}

		protected override void OnSetNewCustomValues(NetworkRandomInjectRequest data)
		{
			TryAddRequest(data);
		}

		public void Cleanup()
		{
			Requests.Clear();
		}

		private bool TryAddRequest(NetworkRandomInjectRequest request)
		{
			if (Requests.Any((NetworkRandomInjectRequest existingRequest) => IsSameRequest(existingRequest, request)))
			{
				return false;
			}
			Requests.Add(request);
			this.OnRequestAdded?.Invoke(request);
			return true;
		}

		private static bool IsSameRequest(NetworkRandomInjectRequest left, NetworkRandomInjectRequest right)
		{
			if (left.ConsumerId == right.ConsumerId)
			{
				return left.Seed == right.Seed;
			}
			return false;
		}
	}
}
