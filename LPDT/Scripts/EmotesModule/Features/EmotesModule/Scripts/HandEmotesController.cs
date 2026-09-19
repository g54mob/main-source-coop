using Features.Movement.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.EmotesModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class HandEmotesController : NetworkBehaviour
	{
		[SerializeField]
		private NetworkedCompositeAnimator _networkedCompositeAnimator;

		private EmotesConfiguration _emotesConfiguration;

		private EmotesTriggerModel _emotesTriggerModel;

		private PlayersStatesSynchronizer _playersStatesSynchronizer;

		[Inject]
		public void InjectDependencies(EmotesConfiguration emotesConfiguration, PlayersStatesSynchronizer playersStatesSynchronizer, EmotesTriggerModel emotesTriggerModel)
		{
			_emotesConfiguration = emotesConfiguration;
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_emotesTriggerModel = emotesTriggerModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (base.HasStateAuthority)
			{
				_emotesTriggerModel.OnHandEmoteTriggered += SetHandEmoting;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			if (base.HasStateAuthority)
			{
				_emotesTriggerModel.OnHandEmoteTriggered -= SetHandEmoting;
			}
		}

		private void SetHandEmoting(HandEmoteType handEmoteType)
		{
			if (!(base.Object == null) && _playersStatesSynchronizer.TryGetState(base.Object.StateAuthority.PlayerId, out var state) && state == PlayerState.Alive && _emotesConfiguration.HandEmotesPool.TryGetValue(handEmoteType, out var value))
			{
				_networkedCompositeAnimator.SetTrigger(Animator.StringToHash(value));
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
