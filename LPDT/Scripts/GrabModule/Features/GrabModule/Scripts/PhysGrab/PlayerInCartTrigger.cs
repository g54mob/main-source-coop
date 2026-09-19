using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Features.GrabModule.Scripts.PhysGrab
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerInCartTrigger : NetworkBehaviour
	{
		[SerializeField]
		private LayerMask _layerMask;

		private List<int> _playersInCase = new List<int>();

		public List<int> PlayersInCase => _playersInCase;

		private void OnTriggerEnter(Collider other)
		{
			if ((_layerMask.value & (1 << other.gameObject.layer)) != 0 && other.TryGetComponent<NetworkObject>(out var component) && !_playersInCase.Contains(component.InputAuthority.PlayerId))
			{
				_playersInCase.Add(component.InputAuthority.PlayerId);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if ((_layerMask.value & (1 << other.gameObject.layer)) != 0 && other.TryGetComponent<NetworkObject>(out var component) && _playersInCase.Contains(component.InputAuthority.PlayerId))
			{
				_playersInCase.Remove(component.InputAuthority.PlayerId);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
