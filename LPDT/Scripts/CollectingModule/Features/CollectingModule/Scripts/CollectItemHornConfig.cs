using FMODUnity;
using Features.ItemsModule.Scripts;
using UnityEngine;

namespace Features.CollectingModule.Scripts
{
	[CreateAssetMenu(menuName = "Items/CollectItemHornConfig", fileName = "CollectItemHornConfig", order = 0)]
	public class CollectItemHornConfig : ItemConfig
	{
		[SerializeField]
		private EventReference _hornSound;

		public EventReference HornSound => _hornSound;
	}
}
