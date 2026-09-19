using Features.Movement.Scripts;
using Fusion;
using UnityEngine;

namespace Features.RandomSoundPlayModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public sealed class TriggerRandomSoundPlayer : RandomSoundPlayerBase
	{
		private const int DefaultPlayerLayer = 7;

		[SerializeField]
		private Transform _soundPoint;

		[SerializeField]
		private LayerMask _playerLayerMask = 128;

		private void OnTriggerEnter(Collider other)
		{
			if (other == null || (_playerLayerMask.value & (1 << other.gameObject.layer)) == 0)
			{
				return;
			}
			PlayerCharacterMovableBase component = other.GetComponent<PlayerCharacterMovableBase>();
			if (!(component == null) && !(component.Object == null) && component.Object.IsValid)
			{
				PlayerRef inputAuthority = component.Object.InputAuthority;
				if (!(inputAuthority == PlayerRef.None))
				{
					TryPlayRandomSound(_soundPoint.position, inputAuthority.PlayerId);
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
