using UnityEngine;

namespace Features.KrakenModule.Scripts
{
	public class TentacleContext : MonoBehaviour
	{
		[field: SerializeField]
		public TentacleController TentacleController { get; private set; }
	}
}
