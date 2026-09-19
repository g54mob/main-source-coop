using Features.NetworkedModelCodegen.Scripts;

namespace Features.StoreModule.Scripts
{
	[NetworkedModel(ModelScope.Shop, ModelOwnership.Individual)]
	public sealed class StoreReadyModel : NetworkedModelBase
	{
		private bool _isDesiredReady;

		private bool _hasDesired;

		public Networked<bool> IsReady { get; } = new Networked<bool>();

		public StoreReadyModel()
		{
			base.AttachmentChanged += OnAttachmentChanged;
		}

		public void SetReady(bool isReady)
		{
			_isDesiredReady = isReady;
			_hasDesired = true;
			if (base.IsAttached)
			{
				IsReady.Value = isReady;
			}
		}

		protected override void OnAttach()
		{
			if (_hasDesired && base.IsAuthority && IsReady.Value != _isDesiredReady)
			{
				IsReady.Value = _isDesiredReady;
			}
		}

		private void OnAttachmentChanged(bool isAttached)
		{
			if (!isAttached)
			{
				_isDesiredReady = false;
				_hasDesired = false;
			}
		}
	}
}
