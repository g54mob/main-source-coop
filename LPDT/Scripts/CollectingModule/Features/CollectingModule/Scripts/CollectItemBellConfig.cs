using FMODUnity;
using Features.ItemsModule.Scripts;
using UnityEngine;

namespace Features.CollectingModule.Scripts
{
	[CreateAssetMenu(menuName = "Items/CollectItemBellConfig", fileName = "CollectItemBellConfig", order = 0)]
	public class CollectItemBellConfig : ItemConfig
	{
		[SerializeField]
		private EventReference _bellSound;

		[SerializeField]
		private float _forceThreshold = 25f;

		[SerializeField]
		private float _playCooldown = 0.2f;

		public EventReference BellSound => _bellSound;

		public float ForceThreshold => _forceThreshold;

		public float PlayCooldown => _playCooldown;
	}
}
