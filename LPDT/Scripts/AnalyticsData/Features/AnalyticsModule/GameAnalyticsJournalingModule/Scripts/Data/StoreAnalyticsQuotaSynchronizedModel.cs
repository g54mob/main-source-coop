using System;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data
{
	[Serializable]
	public class StoreAnalyticsQuotaSynchronizedModel : JsonSynchronizableBaseWithCustomData<StoreAnalyticsQuotaSynchronizedModel, StoreAnalyticsQuotaResponseData>
	{
		public int SessionId;

		public int RequiredEventCount;

		public StoreAnalyticsPhase Phase;

		public override RPCType RPCType => RPCType.InAllWays;

		public override bool IsNeedToSynchronizeOnSpawn => false;

		public event Action OnSessionStateChanged;

		public event Action<StoreAnalyticsQuotaResponseData> OnQuotaResponseReceived;

		protected override void SetNewValues(StoreAnalyticsQuotaSynchronizedModel synchronizable, bool isSynchronizedOnStart)
		{
			SessionId = synchronizable.SessionId;
			RequiredEventCount = synchronizable.RequiredEventCount;
			Phase = synchronizable.Phase;
			this.OnSessionStateChanged?.Invoke();
		}

		protected override void SetNewCustomValues(StoreAnalyticsQuotaResponseData data)
		{
			this.OnQuotaResponseReceived?.Invoke(data);
		}

		public void BeginQuotaRound(int sessionId, int requiredEventCount)
		{
			SessionId = sessionId;
			RequiredEventCount = requiredEventCount;
			Phase = StoreAnalyticsPhase.AwaitingQuota;
			Synchronize();
		}

		public void CompleteRound()
		{
			Phase = StoreAnalyticsPhase.Completed;
			Synchronize();
		}

		public void ReportLocalQuota(int playerId, int sessionId, bool hasEnoughQuota)
		{
			base.Data1 = new StoreAnalyticsQuotaResponseData
			{
				PlayerId = playerId,
				SessionId = sessionId,
				HasEnoughQuota = hasEnoughQuota
			};
			CustomSynchronize();
		}
	}
}
