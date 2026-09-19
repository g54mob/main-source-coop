using Fusion;
using UnityEngine;
using Zenject;

namespace Features.CartUpgradesModule.Scripts.Core
{
	public class CartTierMarker : MonoBehaviour, ICartTierCarrier
	{
		[SerializeField]
		private int _tier;

		[SerializeField]
		private NetworkObject _networkObject;

		private ICartTierRegistry _cartTierRegistry;

		private bool _isRegistered;

		public int Tier => _tier;

		public NetworkObject NetworkObject => _networkObject;

		public Transform Transform => base.transform;

		[Inject]
		public void InjectDependencies(ICartTierRegistry cartTierRegistry)
		{
			_cartTierRegistry = cartTierRegistry;
			Register();
		}

		private void OnEnable()
		{
			Register();
		}

		private void OnDestroy()
		{
			Unregister();
		}

		private void Register()
		{
			if (!_isRegistered && _cartTierRegistry != null)
			{
				_cartTierRegistry.Register(this);
				_isRegistered = true;
			}
		}

		private void Unregister()
		{
			if (_isRegistered)
			{
				_cartTierRegistry.Unregister(this);
				_isRegistered = false;
			}
		}
	}
}
