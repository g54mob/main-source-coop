using Features.ItemsModule.Scripts;
using UnityEngine;

namespace Features.CollectingModule.Scripts
{
	[CreateAssetMenu(menuName = "Items/CollectItemDiceConfig", fileName = "CollectItemDiceConfig", order = 0)]
	public class CollectItemDiceConfig : ItemConfig
	{
		[SerializeField]
		private GameObject _natTwentyVfxPrefab;

		[SerializeField]
		private float _vfxScale = 0.3f;

		[SerializeField]
		private float _upAlignmentDot = 0.9f;

		[SerializeField]
		private float _restLinearSpeed = 0.08f;

		[SerializeField]
		private float _restAngularSpeed = 0.35f;

		public GameObject NatTwentyVfxPrefab => _natTwentyVfxPrefab;

		public float VfxScale => _vfxScale;

		public float UpAlignmentDot => _upAlignmentDot;

		public float RestLinearSpeed => _restLinearSpeed;

		public float RestAngularSpeed => _restAngularSpeed;
	}
}
