using UnityEngine;

namespace Features.JacuzziBeachInteractableModule.Scripts
{
	public class JacuzziPlayerTriggerZone : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _playerLayerMask;

		[SerializeField]
		private JacuzziBeachInteractableBehaviour _jacuzziBeachInteractableBehaviour;

		private void OnTriggerEnter(Collider other)
		{
			if ((_playerLayerMask.value & (1 << other.gameObject.layer)) != 0)
			{
				_jacuzziBeachInteractableBehaviour.NotifyPlayerColliderEntered(other);
			}
		}

		private void OnTriggerStay(Collider other)
		{
			if ((_playerLayerMask.value & (1 << other.gameObject.layer)) != 0)
			{
				_jacuzziBeachInteractableBehaviour.NotifyPlayerColliderEntered(other);
			}
		}
	}
}
