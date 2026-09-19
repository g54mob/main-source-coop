using UnityEngine;

namespace Features.ItemsModule.Scripts
{
	[CreateAssetMenu(menuName = "Items/ItemConfig", fileName = "ItemConfig", order = 0)]
	public class ItemConfig : ScriptableObject
	{
		[SerializeField]
		private ItemType _itemType;

		[SerializeField]
		private int _currencyValue;

		[SerializeField]
		[Range(0f, 1f)]
		private float _maxAdditionalCurrencyPercent;

		[SerializeField]
		[Range(0f, 1f)]
		private float _maxReduceCurrencyPercent;

		[SerializeField]
		private bool _isCollectable;

		[SerializeField]
		private ItemCollisionConfig _collisionConfig;

		[field: SerializeField]
		public bool IsReducible { get; private set; } = true;

		[field: SerializeField]
		public bool CanDestroyWithoutMoney { get; private set; }

		[field: SerializeField]
		public float DamageOnCollide { get; private set; }

		public ItemType ItemType => _itemType;

		public int CurrencyValue => _currencyValue;

		public bool IsCollectable => _isCollectable;

		public ItemCollisionConfig CollisionConfig => _collisionConfig;

		public float MaxAdditionalCurrencyPercent => _maxAdditionalCurrencyPercent;

		public float MaxReduceCurrencyPercent => _maxReduceCurrencyPercent;
	}
}
