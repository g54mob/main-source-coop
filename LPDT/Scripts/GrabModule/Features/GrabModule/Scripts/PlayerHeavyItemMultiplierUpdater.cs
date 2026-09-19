using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.GrabModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerHeavyItemMultiplierUpdater : NetworkBehaviour
	{
		[SerializeField]
		private float _aliveHeavyItemMultiplier = 1f;

		[SerializeField]
		private float _deadHeavyItemMultiplier = 0.5f;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		private IPlayerStateService _playersStatesService;

		private bool _isInitialized;

		[Inject]
		private void InjectDependencies(IPlayerStateService playersStatesService)
		{
			_playersStatesService = playersStatesService;
		}

		public override void Spawned()
		{
			_isInitialized = true;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_isInitialized = false;
		}

		private void Update()
		{
			if (_isInitialized)
			{
				PlayerState playerState = _playersStatesService.GetPlayerState(base.Object.StateAuthority.PlayerId);
				if (playerState == PlayerState.Dead || playerState == PlayerState.PreDeadCrouch)
				{
					_simplePointGrabable.HeavyItemMultiplier = _deadHeavyItemMultiplier;
				}
				else
				{
					_simplePointGrabable.HeavyItemMultiplier = _aliveHeavyItemMultiplier;
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
