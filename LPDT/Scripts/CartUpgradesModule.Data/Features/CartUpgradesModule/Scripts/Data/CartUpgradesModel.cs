using System;
using Features.NetworkedModelCodegen.Scripts;

namespace Features.CartUpgradesModule.Scripts.Data
{
	[NetworkedModel(ModelScope.Run, ModelOwnership.Shared)]
	public sealed class CartUpgradesModel : NetworkedModelBase
	{
		private int _desiredModules;

		private bool _hasDesiredModules;

		public Networked<int> UpgradeModules { get; } = new Networked<int>();

		public event Action<int> OnUpgradeModulesChanged;

		public CartUpgradesModel()
		{
			UpgradeModules.Changed += RaiseUpgradeModulesChanged;
			base.AttachmentChanged += HandleAttachmentChanged;
		}

		public void AddUpgradeModules(int modulesMask)
		{
			_desiredModules |= modulesMask | (base.IsAttached ? UpgradeModules.Value : 0);
			_hasDesiredModules = true;
			if (base.IsAttached && UpgradeModules.Value != _desiredModules)
			{
				UpgradeModules.Value = _desiredModules;
			}
		}

		public void ResetUpgradeModules()
		{
			_desiredModules = 0;
			_hasDesiredModules = true;
			if (base.IsAttached && UpgradeModules.Value != 0)
			{
				UpgradeModules.Value = 0;
			}
		}

		protected override void OnAttach()
		{
			if (_hasDesiredModules && base.IsAuthority && UpgradeModules.Value != _desiredModules)
			{
				UpgradeModules.Value = _desiredModules;
			}
		}

		private void HandleAttachmentChanged(bool isAttached)
		{
			if (!isAttached)
			{
				_desiredModules = 0;
				_hasDesiredModules = false;
				RaiseUpgradeModulesChanged(0);
			}
		}

		private void RaiseUpgradeModulesChanged(int modulesMask)
		{
			this.OnUpgradeModulesChanged?.Invoke(modulesMask);
		}
	}
}
