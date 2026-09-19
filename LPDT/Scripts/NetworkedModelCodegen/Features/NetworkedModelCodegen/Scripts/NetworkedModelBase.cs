using System;
using Zenject;

namespace Features.NetworkedModelCodegen.Scripts
{
	public abstract class NetworkedModelBase
	{
		private Func<bool> _authorityProvider;

		public bool IsAttached { get; private set; }

		public bool IsAuthority
		{
			get
			{
				if (_authorityProvider != null)
				{
					return _authorityProvider();
				}
				return false;
			}
		}

		public event Action<bool> AttachmentChanged;

		[Inject]
		public void InjectDependencies(INetworkedModelRegistry registry)
		{
			registry.Register(this);
		}

		public void SetAttached(bool isAttached)
		{
			if (IsAttached != isAttached)
			{
				IsAttached = isAttached;
				if (isAttached)
				{
					OnAttach();
				}
				this.AttachmentChanged?.Invoke(isAttached);
			}
		}

		public void SetAuthorityProvider(Func<bool> authorityProvider)
		{
			_authorityProvider = authorityProvider;
		}

		public void RaiseFusionUpdate()
		{
			OnFusionUpdate();
		}

		protected virtual void OnAttach()
		{
		}

		protected virtual void OnFusionUpdate()
		{
		}
	}
}
