using Features.ItemsModule.Scripts;
using UnityEngine;

namespace Features.CollectingModule.Scripts
{
	[CreateAssetMenu(menuName = "Items/CollectItemWatchingEyeConfig", fileName = "CollectItemWatchingEyeConfig", order = 0)]
	public class CollectItemWatchingEyeConfig : ItemConfig
	{
		[SerializeField]
		private float _eyesHorizontalLimit = 70f;

		[SerializeField]
		private float _eyesVerticalLimitTop = 55f;

		[SerializeField]
		private float _eyesVerticalLimitBottom = -50f;

		[SerializeField]
		private float _maxTargetDistance = 50f;

		[SerializeField]
		private float _eyesLerpSpeed = 10f;

		public float EyesHorizontalLimit => _eyesHorizontalLimit;

		public float EyesVerticalLimitTop => _eyesVerticalLimitTop;

		public float EyesVerticalLimitBottom => _eyesVerticalLimitBottom;

		public float MaxTargetDistance => _maxTargetDistance;

		public float EyesLerpSpeed => _eyesLerpSpeed;
	}
}
