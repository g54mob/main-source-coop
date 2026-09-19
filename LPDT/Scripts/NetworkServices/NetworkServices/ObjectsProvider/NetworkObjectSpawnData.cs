using UnityEngine;

namespace NetworkServices.ObjectsProvider
{
	public class NetworkObjectSpawnData : MonoBehaviour
	{
		[field: SerializeField]
		public bool Injectable { get; set; }
	}
}
