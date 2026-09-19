using UnityEngine;

namespace Features.ItemsModule.Scripts
{
	[CreateAssetMenu(menuName = "ItemCollision/ItemCollisionConfig", fileName = "ItemCollisionConfig", order = 0)]
	public class ItemCollisionConfig : ScriptableObject
	{
		[Header("Fragility Settings")]
		[SerializeField]
		[Range(0f, 1f)]
		[Tooltip("0 - cant break, 1 - from any damage received max cost loss percent damage")]
		private float _fragility = 0.5f;

		[SerializeField]
		[Range(0.01f, 1f)]
		private float _maxCostLossPercentPerHit = 0.3f;

		[SerializeField]
		private float _minCollisionForce = 1f;

		public float Fragility => _fragility;

		public float MaxCostLossPercentPerHit => _maxCostLossPercentPerHit;

		public float MinCollisionForce => _minCollisionForce;
	}
}
