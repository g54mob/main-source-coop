using UnityEngine;

namespace Features.StoreModule.Scripts
{
	public class StoreEntity : MonoBehaviour
	{
		[field: SerializeField]
		public StoreTableBehaviour StoreTableBehaviour { get; private set; }

		[field: SerializeField]
		public StoreActivationComponent StoreActivationComponent { get; private set; }
	}
}
