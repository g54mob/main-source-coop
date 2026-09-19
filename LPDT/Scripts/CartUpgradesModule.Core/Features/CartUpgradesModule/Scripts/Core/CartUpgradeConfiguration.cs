using Fusion;
using UnityEngine;

namespace Features.CartUpgradesModule.Scripts.Core
{
	[CreateAssetMenu(fileName = "CartUpgradeConfiguration_Default", menuName = "Configurations/CartUpgradesModule/CartUpgradeConfiguration")]
	public class CartUpgradeConfiguration : ScriptableObject
	{
		public const int BASE_CART = 0;

		public const int SIZE_CART = 1;

		[Header("Cart")]
		[SerializeField]
		private NetworkObject _baseCartPrefab;

		[SerializeField]
		private NetworkObject _sizeCartPrefab;

		[Header("Cannon")]
		[SerializeField]
		private int _cannonMaxShots = 4;

		[SerializeField]
		private float _cannonLowerDurationSeconds = 0.6f;

		public int CannonMaxShots => _cannonMaxShots;

		public float CannonLowerDurationSeconds => _cannonLowerDurationSeconds;

		public bool IsModuleSupported(CartUpgradeModule module)
		{
			return module switch
			{
				CartUpgradeModule.None => false, 
				CartUpgradeModule.Size => _sizeCartPrefab != null, 
				_ => true, 
			};
		}

		public int GetCartVariantIndex(int modulesMask)
		{
			if (!CartUpgradeModule.Size.IsInMask(modulesMask) || !(_sizeCartPrefab != null))
			{
				return 0;
			}
			return 1;
		}

		public bool IsCartPrefab(GameObject candidate)
		{
			if (candidate == null)
			{
				return false;
			}
			if (!(_baseCartPrefab != null) || !(_baseCartPrefab.gameObject == candidate))
			{
				if (_sizeCartPrefab != null)
				{
					return _sizeCartPrefab.gameObject == candidate;
				}
				return false;
			}
			return true;
		}

		public NetworkObject GetCartPrefab(int modulesMask)
		{
			if (GetCartVariantIndex(modulesMask) != 1)
			{
				return _baseCartPrefab;
			}
			return _sizeCartPrefab;
		}
	}
}
