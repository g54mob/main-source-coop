using Features.ItemsModule.Scripts;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Units
{
	public class CoinRobCarriedCoinVisuals : MonoBehaviour
	{
		[Header("Item configs (for coin type detection)")]
		[SerializeField]
		private ItemConfig _goldenCoinConfig;

		[SerializeField]
		private ItemConfig _silverCoinConfig;

		[Header("Fake coin meshes")]
		[SerializeField]
		private GameObject _goldCoinObject;

		[SerializeField]
		private GameObject _silverCoinObject;

		[Header("Carried coin VFX")]
		[SerializeField]
		private GameObject _carriedCoinVfxObject;

		private CoinRobCarriedCoinType _currentCoinType;

		public CoinRobCarriedCoinType CurrentCoinType => _currentCoinType;

		public bool IsShowingCoin => _currentCoinType != CoinRobCarriedCoinType.None;

		public CoinRobCarriedCoinType ResolveCoinType(IItem item)
		{
			if (item == null)
			{
				return CoinRobCarriedCoinType.None;
			}
			if (item is MonoItem monoItem && monoItem.DefaultConfig != null)
			{
				if (monoItem.DefaultConfig == _goldenCoinConfig)
				{
					return CoinRobCarriedCoinType.Gold;
				}
				if (monoItem.DefaultConfig == _silverCoinConfig)
				{
					return CoinRobCarriedCoinType.Silver;
				}
			}
			if (_goldenCoinConfig != null && item.MaxCurrencyValue >= _goldenCoinConfig.CurrencyValue)
			{
				return CoinRobCarriedCoinType.Gold;
			}
			return CoinRobCarriedCoinType.Silver;
		}

		public void Apply(CoinRobCarriedCoinType coinType)
		{
			_currentCoinType = coinType;
			bool flag = coinType == CoinRobCarriedCoinType.Gold;
			bool flag2 = coinType == CoinRobCarriedCoinType.Silver;
			SetActive(_goldCoinObject, flag);
			SetActive(_silverCoinObject, flag2);
			SetActive(_carriedCoinVfxObject, flag || flag2);
		}

		public void Hide()
		{
			Apply(CoinRobCarriedCoinType.None);
		}

		private void Awake()
		{
			Hide();
		}

		private static void SetActive(GameObject target, bool isActive)
		{
			if (target != null)
			{
				target.SetActive(isActive);
			}
		}
	}
}
