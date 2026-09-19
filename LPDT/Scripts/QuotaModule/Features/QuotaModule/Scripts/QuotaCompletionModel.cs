using System;
using Features.NetworkedModelCodegen.Scripts;

namespace Features.QuotaModule.Scripts
{
	[NetworkedModel(ModelScope.Level, ModelOwnership.Shared)]
	public class QuotaCompletionModel : NetworkedModelBase
	{
		public Networked<bool> IsQuotaCompleted { get; } = new Networked<bool>();

		public Networked<bool> IsBellActivated { get; } = new Networked<bool>();

		public event Action<bool> OnQuotaCompleted;

		public event Action<bool> OnBellActivated;

		public QuotaCompletionModel()
		{
			IsQuotaCompleted.Changed += RaiseQuotaCompleted;
			IsBellActivated.Changed += RaiseBellActivated;
		}

		private void RaiseQuotaCompleted(bool value)
		{
			this.OnQuotaCompleted?.Invoke(value);
		}

		private void RaiseBellActivated(bool value)
		{
			this.OnBellActivated?.Invoke(value);
		}
	}
}
