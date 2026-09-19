using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.CameraModelModule;
using Features.LevelLightModule.Scripts;
using Features.LineArmModule.Scripts;
using Features.MouseVisibilityModule.Scripts;
using Features.Movement.Scripts;
using Features.StrechArmsModule.Scripts;
using UnityEngine;

namespace Features.PlayerStatesModule.Scripts.Systems.StateMachine.States
{
	public class LocalPlayerStoreState : LocalPlayerStateBase
	{
		private static readonly int IsDeadHash = Animator.StringToHash("IsDead");

		public override PlayerState StateEnum => PlayerState.Store;

		protected override void OnEnter()
		{
			LightObjectsGroupService.SetLightObjectGroupState(LightObjectGroup.Player, state: false);
			LightObjectsGroupService.SetLightObjectGroupState(LightObjectGroup.FlashLight, state: false);
			CameraModel.AddPerlinDisableReason(PerlinDisableReasonEnum.Store);
			CameraModel.AddCameraReason(Features.CameraModelModule.CameraType.CardTableCamera, CameraReasonEnum.Store);
			MouseVisibilityModel.AddMouseVisibilityRequest(new MouseVisibilityRequest(0, isMouseVisible: true));
			InteractModel.ChangeCursorVisibility(visible: false);
			CloseSpectatorAndDeathWindows();
			ApplyStoreBodyWhenReady().Forget();
			ApplyStoreArmWhenReady().Forget();
		}

		protected override void OnExit()
		{
			LightObjectsGroupService.SetLightObjectGroupState(LightObjectGroup.FlashLight, state: true);
			CameraModel.RemoveCameraReason(Features.CameraModelModule.CameraType.CardTableCamera, CameraReasonEnum.Store);
			CameraModel.RemovePerlinDisableReason(PerlinDisableReasonEnum.Store);
			PlayerMovableModel.RemoveLockMovementReason(LockMovementReasonEnum.Store);
			if (PlayerMovableModel.Rotator != null)
			{
				PlayerMovableModel.Rotator.IsResetRotation = false;
			}
		}

		private async UniTaskVoid ApplyStoreBodyWhenReady()
		{
			try
			{
				await UniTask.WaitUntil(IsStoreBodyReady, PlayerLoopTiming.Update, base.CancellationToken);
			}
			catch (OperationCanceledException)
			{
				return;
			}
			if (PlayerMovableModel.NetworkedAnimator.CompositeAnimator.GetBool(IsDeadHash))
			{
				PlayerMovableModel.NetworkedAnimator.SetBool(IsDeadHash, boolValue: false);
			}
			PlayerMovableModel.LocalMovable.ForceCrouch(isCrouching: false);
			PlayerMovableModel.Rotator.IsResetRotation = true;
			PlayerMovableModel.AddLockMovementReason(LockMovementReasonEnum.Store);
		}

		private async UniTaskVoid ApplyStoreArmWhenReady()
		{
			try
			{
				await UniTask.WaitUntil(IsStoreArmReady, PlayerLoopTiming.Update, base.CancellationToken);
			}
			catch (OperationCanceledException)
			{
				return;
			}
			Dictionary<LineArmType, LineArmControllerBase> allLineArmsForPlayer = LineArmsModel.GetAllLineArmsForPlayer(MultiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			LineArmControllerBase lineArmControllerBase = allLineArmsForPlayer[LineArmType.RightArmDefault];
			LineArmControllerBase lineArmControllerBase2 = allLineArmsForPlayer[LineArmType.RightArmByMouse];
			lineArmControllerBase.UnJoinAll(throwItem: false);
			lineArmControllerBase.enabled = false;
			lineArmControllerBase2.enabled = true;
			int playerId = MultiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			ArmVisualsController armVisualsController = ArmVisualsControllerModel.GetArmVisualsController(playerId, Arm.Right);
			armVisualsController.ReduceSolverUpdateEnabled = false;
			armVisualsController.SetLineArmController(LineArmType.RightArmByMouse);
			ArmVisualsController armVisualsController2 = ArmVisualsControllerModel.GetArmVisualsController(playerId, Arm.Left);
			if (armVisualsController2 != null)
			{
				armVisualsController2.SnapToStoreIdlePose();
			}
		}

		private bool IsStoreBodyReady()
		{
			if (PlayerMovableModel.LocalMovable != null && PlayerMovableModel.FreeFlyMovable != null && PlayerMovableModel.Rotator != null)
			{
				return PlayerMovableModel.NetworkedAnimator != null;
			}
			return false;
		}

		private bool IsStoreArmReady()
		{
			int playerId = MultiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (ArmVisualsControllerModel.GetArmVisualsController(playerId, Arm.Right) == null)
			{
				return false;
			}
			Dictionary<LineArmType, LineArmControllerBase> allLineArmsForPlayer = LineArmsModel.GetAllLineArmsForPlayer(playerId);
			if (allLineArmsForPlayer != null && allLineArmsForPlayer.ContainsKey(LineArmType.RightArmDefault))
			{
				return allLineArmsForPlayer.ContainsKey(LineArmType.RightArmByMouse);
			}
			return false;
		}
	}
}
