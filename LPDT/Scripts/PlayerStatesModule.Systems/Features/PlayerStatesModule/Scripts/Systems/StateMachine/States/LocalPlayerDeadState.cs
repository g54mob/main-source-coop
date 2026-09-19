using System.Linq;
using Features.CameraModelModule;
using Features.LevelLightModule.Scripts;
using Features.RagdollModule.Scripts;
using Fusion;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.PlayerStatesModule.Scripts.Systems.StateMachine.States
{
	public class LocalPlayerDeadState : LocalPlayerStateBase
	{
		public override PlayerState StateEnum => PlayerState.Dead;

		protected override void OnEnter()
		{
			LightObjectsGroupService.SetLightObjectGroupState(LightObjectGroup.Player, state: true);
			if (PlayersRagdollModel.TryGetPlayerRagdoll(MultiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var ragdoll))
			{
				ragdoll.AddSimulationReason(RagdollSimulationReasonEnum.Death);
			}
			Transform cameraPositionTransform = PlayerMovableModel.LocalMovable.CameraPositionTransform;
			CameraModel.Cameras[Features.CameraModelModule.CameraType.FPCamera].SetTrackingTarget(cameraPositionTransform);
			CameraModel.Cameras[Features.CameraModelModule.CameraType.TPCamera].SetTrackingTarget(cameraPositionTransform);
			RequestMouseHidden();
			if (SpectatorWindow.WindowStatus == WindowStatus.Showed)
			{
				SpectatorWindow.Close();
			}
			if (GameOverWindow.WindowStatus != WindowStatus.Showed && QuotaCompletedWindow.WindowStatus != WindowStatus.Showed)
			{
				DeathWindow.Open();
			}
			PlayerMovableModel.LocalMovable.ForceCrouch(isCrouching: false);
			InteractModel.ChangeCursorVisibility(visible: false);
			StatsViewModel.ChangeStatsVisibility(visible: false);
			RumBuffsViewModel.ChangeBuffsVisibility(visible: false);
			PushToTalkViewModel.ChangePushToTalkVisibility(visible: false);
			SetDefaultLineArmController();
			CameraModel.Cameras[Features.CameraModelModule.CameraType.TPCamera].SetBestRotation(Quaternion.LookRotation(CameraModel.CameraObject.transform.forward).eulerAngles.y);
			CameraModel.AddPerlinDisableReason(PerlinDisableReasonEnum.Dead);
			SpectatorModel.CurrentSpectatablePlayer = MultiplayerModel.NetworkRunner.ActivePlayers.ToList().IndexOf(MultiplayerModel.NetworkRunner.ActivePlayers.First((PlayerRef p) => p == MultiplayerModel.NetworkRunner.LocalPlayer));
			SpectatorModel.InvokeSpectatableChanged();
		}

		protected override void OnExit()
		{
			CameraSpectatorFollowService.End();
			PlayerState nextStateInTransition = LocalPlayerStateMachine.NextStateInTransition;
			if ((nextStateInTransition == PlayerState.Alive || nextStateInTransition == PlayerState.Store) && PlayersRagdollModel.TryGetPlayerRagdoll(MultiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var ragdoll))
			{
				ragdoll.RemoveSimulationReason(RagdollSimulationReasonEnum.Death);
			}
			CameraModel.RemovePerlinDisableReason(PerlinDisableReasonEnum.Dead);
		}
	}
}
