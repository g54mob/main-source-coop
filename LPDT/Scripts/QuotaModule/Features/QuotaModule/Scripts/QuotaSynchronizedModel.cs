using System;
using Features.NetworkedModelCodegen.Scripts;
using GameplayEvents;

namespace Features.QuotaModule.Scripts
{
	[NetworkedModel(ModelScope.Level, ModelOwnership.Shared)]
	public class QuotaSynchronizedModel : NetworkedModelBase
	{
		private readonly GameplayEventBus _gameplayEventBus;

		public Networked<float> CurrentQuota { get; } = new Networked<float>();

		public Networked<float> MaxQuota { get; } = new Networked<float>();

		public event Action<float, float> OnQuotaChanged;

		public QuotaSynchronizedModel(GameplayEventBus gameplayEventBus)
		{
			_gameplayEventBus = gameplayEventBus;
			CurrentQuota.Changed += delegate
			{
				RaiseQuotaChanged();
			};
			MaxQuota.Changed += delegate
			{
				RaiseQuotaChanged();
			};
		}

		private void RaiseQuotaChanged()
		{
			this.OnQuotaChanged?.Invoke(CurrentQuota.Value, MaxQuota.Value);
			_gameplayEventBus.Publish(new OnSessionQuotaChangedEvent(CurrentQuota.Value, MaxQuota.Value));
		}
	}
}
