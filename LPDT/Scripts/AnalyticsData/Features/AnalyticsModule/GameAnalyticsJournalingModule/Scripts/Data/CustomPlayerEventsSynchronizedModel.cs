using System;
using Features.SynchronizedModelsModule.Scripts.DataStreamingSynchronizer;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data
{
	[Serializable]
	public class CustomPlayerEventsSynchronizedModel : DataStreamSynchronizableBaseWithCustomData<CustomPlayerEventsSynchronizedModel, CustomPlayerEventRequest>
	{
		public override bool IsNeedToSynchronizeOnSpawn => false;

		public event Action<CustomPlayerEventRequest> OnPlayerCustomEvent;

		protected override void SetNewValues(CustomPlayerEventsSynchronizedModel model)
		{
		}

		protected override void OnSetNewCustomValues(CustomPlayerEventRequest data)
		{
			this.OnPlayerCustomEvent?.Invoke(data);
		}

		public void SendPlayerEvent(int ownerPlayerId, string evenName, AnalyticsEventQuotaGroup quotaGroup = AnalyticsEventQuotaGroup.General)
		{
			base.Data1 = new CustomPlayerEventRequest
			{
				OwnerPlayerId = ownerPlayerId,
				EvenName = evenName,
				QuotaGroup = quotaGroup
			};
			CustomSynchronize();
		}

		public override void OnDisconnect()
		{
			this.OnPlayerCustomEvent = null;
		}
	}
}
