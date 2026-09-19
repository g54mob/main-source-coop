using System;
using Features.MouseVisibilityModule.Scripts;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.RagdollModule.Scripts;
using UnityEngine;

namespace Features.ThrowModule.Scripts
{
	public class LocalPlayerThrowService : ILocalPlayerThrowService
	{
		private static readonly int IsTemporalStunInProgressHash = Animator.StringToHash("IsTemporalStunInProgress");

		private static readonly int IsThrowInProgressHash = Animator.StringToHash("IsThrowInProgress");

		private static readonly int IsStunFromThrowHash = Animator.StringToHash("IsStunFromThrow");

		private static readonly int ThrowStartedHash = Animator.StringToHash("ThrowStarted");

		private readonly MultiplayerModel _multiplayerModel;

		private readonly PlayersRagdollModel _playersRagdollModel;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly MouseVisibilityModel _mouseVisibilityModel;

		private readonly PlayerThrowConfiguration _playerThrowConfiguration;

		private StunDurationPreset _pendingStunPreset;

		public event Action OnThrowEntered;

		public LocalPlayerThrowService(MultiplayerModel multiplayerModel, PlayersRagdollModel playersRagdollModel, PlayerMovableModel playerMovableModel, MouseVisibilityModel mouseVisibilityModel, PlayerThrowConfiguration playerThrowConfiguration)
		{
			_multiplayerModel = multiplayerModel;
			_playersRagdollModel = playersRagdollModel;
			_playerMovableModel = playerMovableModel;
			_mouseVisibilityModel = mouseVisibilityModel;
			_playerThrowConfiguration = playerThrowConfiguration;
		}

		public void EnterThrow(StunDurationPreset stunDurationPreset = StunDurationPreset.Default)
		{
			_pendingStunPreset = stunDurationPreset;
			_playerMovableModel.NetworkedAnimator.SetBool(IsThrowInProgressHash, boolValue: true);
			_playerMovableModel.NetworkedAnimator.SetBool(IsStunFromThrowHash, boolValue: true);
			_playerMovableModel.NetworkedAnimator.SetTrigger(ThrowStartedHash);
			_playerMovableModel.MovablePhysics.ChangePlayerPhysMaterial(_playerThrowConfiguration.ThrowPhysicsMaterial);
			_playerMovableModel.AddOnlyHeadRotationReason(OnlyHeadRotationReasonEnum.Throw);
			_mouseVisibilityModel.AddMouseVisibilityRequest(new MouseVisibilityRequest(0, isMouseVisible: false));
			if (TryGetLocalRagdoll(out var ragdoll))
			{
				ragdoll.AddSimulationReason(RagdollSimulationReasonEnum.Throw);
			}
			this.OnThrowEntered?.Invoke();
		}

		public StunDurationPreset ConsumePendingStunPreset()
		{
			StunDurationPreset pendingStunPreset = _pendingStunPreset;
			_pendingStunPreset = StunDurationPreset.Default;
			return pendingStunPreset;
		}

		public void ExitThrow(bool isGoingToDead)
		{
			if (TryGetLocalRagdoll(out var ragdoll))
			{
				ragdoll.RemoveSimulationReason(RagdollSimulationReasonEnum.Throw);
			}
			if (!isGoingToDead)
			{
				_playerMovableModel.NetworkedAnimator.SetBool(IsTemporalStunInProgressHash, boolValue: true);
			}
			_playerMovableModel.NetworkedAnimator.SetBool(IsThrowInProgressHash, boolValue: false);
			_playerMovableModel.MovablePhysics.ChangePlayerPhysMaterial(_playerMovableModel.MovablePhysics.PlayerPhysicsMaterial);
			_playerMovableModel.RemoveOnlyHeadRotationReason(OnlyHeadRotationReasonEnum.Throw);
		}

		private bool TryGetLocalRagdoll(out PlayerRagdollEntity ragdoll)
		{
			return _playersRagdollModel.TryGetPlayerRagdoll(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out ragdoll);
		}
	}
}
