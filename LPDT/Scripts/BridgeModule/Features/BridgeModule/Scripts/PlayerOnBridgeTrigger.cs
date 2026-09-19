using System.Collections.Generic;
using UnityEngine;

namespace Features.BridgeModule.Scripts
{
	public class PlayerOnBridgeTrigger : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _layerMask;

		private List<Transform> _playersOnBridge = new List<Transform>();

		public List<Transform> PlayersOnBridge => _playersOnBridge;

		private void OnTriggerEnter(Collider other)
		{
			if ((_layerMask.value & (1 << other.gameObject.layer)) != 0)
			{
				_playersOnBridge.Add(other.transform);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if ((_layerMask.value & (1 << other.gameObject.layer)) != 0)
			{
				_playersOnBridge.Remove(other.transform);
			}
		}
	}
}
