using System;
using Features.NetworkedModelCodegen.Scripts;

namespace Features.StoreModule.Scripts
{
	[NetworkedModel(ModelScope.Shop, ModelOwnership.Shared)]
	public sealed class StorePhaseModel : NetworkedModelBase
	{
		private bool _isDesiredActive;

		private bool _hasDesired;

		public Networked<bool> IsStoreActive { get; } = new Networked<bool>();

		public event Action<bool> OnStorePhaseChanged;

		public StorePhaseModel()
		{
			IsStoreActive.Changed += RaiseStorePhaseChanged;
		}

		public void SetStoreActive(bool isActive)
		{
			_isDesiredActive = isActive;
			_hasDesired = true;
			if (base.IsAttached)
			{
				IsStoreActive.Value = isActive;
			}
		}

		protected override void OnAttach()
		{
			if (_hasDesired && base.IsAuthority && IsStoreActive.Value != _isDesiredActive)
			{
				IsStoreActive.Value = _isDesiredActive;
			}
		}

		private void RaiseStorePhaseChanged(bool value)
		{
			this.OnStorePhaseChanged?.Invoke(value);
		}
	}
}
