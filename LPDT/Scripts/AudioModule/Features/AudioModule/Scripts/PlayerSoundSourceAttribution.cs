using Features.AudioServiceModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AudioModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerSoundSourceAttribution : NetworkBehaviour
	{
		[SerializeField]
		private SoundSourceBehaviour[] _playerSoundSources;

		public override void Spawned()
		{
			if (_playerSoundSources == null || _playerSoundSources.Length == 0 || base.Object == null || !base.Object.IsValid)
			{
				return;
			}
			int playerId = base.Object.InputAuthority.PlayerId;
			for (int i = 0; i < _playerSoundSources.Length; i++)
			{
				SoundSourceBehaviour soundSourceBehaviour = _playerSoundSources[i];
				if (!(soundSourceBehaviour == null) && soundSourceBehaviour.Kind == SoundSourceKind.Player)
				{
					soundSourceBehaviour.SetAttributedPlayerId(playerId);
				}
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
