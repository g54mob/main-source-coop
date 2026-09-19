using System;
using System.Collections.Generic;
using System.Threading;
using Core.StateMachineModule.Scripts;
using Cysharp.Threading.Tasks;
using Features.AudioDevicesModule.Scripts.PushToTalk.Views;
using Features.CameraModelModule;
using Features.HUDModule.Scripts;
using Features.LevelLightModule.Scripts;
using Features.LineArmModule.Scripts;
using Features.LineArmModule.Scripts.Data;
using Features.MouseVisibilityModule.Scripts;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.RagdollModule.Scripts;
using Features.RumModule.Scripts;
using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Features.StrechArmsModule.Scripts;
using Features.ViewSystemModule.Scripts.Windows;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

namespace Features.PlayerStatesModule.Scripts.Systems.StateMachine
{
	public abstract class LocalPlayerStateBase : StateBase
	{
		protected CameraModel CameraModel;

		protected PlayerMovableModel PlayerMovableModel;

		protected MultiplayerModel MultiplayerModel;

		protected IPlayerStateService PlayerStateService;

		protected PlayerStatesConfiguration PlayerStatesConfiguration;

		protected MouseVisibilityModel MouseVisibilityModel;

		protected InteractModel InteractModel;

		protected StatsViewModel StatsViewModel;

		protected RumBuffsViewModel RumBuffsViewModel;

		protected ILightObjectsGroupService LightObjectsGroupService;

		protected LineArmsModel LineArmsModel;

		protected ArmVisualsControllerModel ArmVisualsControllerModel;

		protected PlayersRagdollModel PlayersRagdollModel;

		protected SpectatorWindow SpectatorWindow;

		protected DeathWindow DeathWindow;

		protected GameOverWindow GameOverWindow;

		protected QuotaCompletedWindow QuotaCompletedWindow;

		protected SpectatorModel SpectatorModel;

		protected ILoadingScreenService LoadingScreenService;

		protected LocalPlayerStateMachine LocalPlayerStateMachine;

		protected PushToTalkViewModel PushToTalkViewModel;

		protected ICameraSpectatorFollowService CameraSpectatorFollowService;

		private CancellationTokenSource _cts;

		public abstract PlayerState StateEnum { get; }

		protected CancellationToken CancellationToken => _cts?.Token ?? CancellationToken.None;

		[Inject]
		public void InjectDependencies(CameraModel cameraModel, PlayerMovableModel playerMovableModel, MultiplayerModel multiplayerModel, IPlayerStateService playerStateService, PlayerStatesConfiguration playerStatesConfiguration, MouseVisibilityModel mouseVisibilityModel, InteractModel interactModel, StatsViewModel statsViewModel, RumBuffsViewModel rumBuffsViewModel, ILightObjectsGroupService lightObjectsGroupService, LineArmsModel lineArmsModel, ArmVisualsControllerModel armVisualsControllerModel, PlayersRagdollModel playersRagdollModel, SpectatorWindow spectatorWindow, DeathWindow deathWindow, GameOverWindow gameOverWindow, QuotaCompletedWindow quotaCompletedWindow, SpectatorModel spectatorModel, ILoadingScreenService loadingScreenService, LocalPlayerStateMachine localPlayerStateMachine, PushToTalkViewModel pushToTalkViewModel, ICameraSpectatorFollowService cameraSpectatorFollowService)
		{
			CameraModel = cameraModel;
			PlayerMovableModel = playerMovableModel;
			MultiplayerModel = multiplayerModel;
			PlayerStateService = playerStateService;
			PlayerStatesConfiguration = playerStatesConfiguration;
			MouseVisibilityModel = mouseVisibilityModel;
			InteractModel = interactModel;
			StatsViewModel = statsViewModel;
			RumBuffsViewModel = rumBuffsViewModel;
			LightObjectsGroupService = lightObjectsGroupService;
			LineArmsModel = lineArmsModel;
			ArmVisualsControllerModel = armVisualsControllerModel;
			PlayersRagdollModel = playersRagdollModel;
			SpectatorWindow = spectatorWindow;
			DeathWindow = deathWindow;
			GameOverWindow = gameOverWindow;
			QuotaCompletedWindow = quotaCompletedWindow;
			SpectatorModel = spectatorModel;
			LoadingScreenService = loadingScreenService;
			LocalPlayerStateMachine = localPlayerStateMachine;
			PushToTalkViewModel = pushToTalkViewModel;
			CameraSpectatorFollowService = cameraSpectatorFollowService;
		}

		public sealed override void Enter()
		{
			_cts?.Cancel();
			_cts?.Dispose();
			_cts = new CancellationTokenSource();
			try
			{
				OnEnter();
			}
			catch (Exception)
			{
				_cts?.Cancel();
				_cts?.Dispose();
				_cts = null;
				throw;
			}
		}

		public sealed override void Exit()
		{
			try
			{
				OnExit();
			}
			finally
			{
				_cts?.Cancel();
				_cts?.Dispose();
				_cts = null;
			}
		}

		protected abstract void OnEnter();

		protected virtual void OnExit()
		{
		}

		protected void SetCameraTrackingToLocalPlayer()
		{
			CameraSpectatorFollowService.End();
			if (CameraModel.Cameras.ContainsKey(Features.CameraModelModule.CameraType.FPCamera))
			{
				Transform cameraPositionTransform = PlayerMovableModel.LocalMovable.CameraPositionTransform;
				CameraModel.Cameras[Features.CameraModelModule.CameraType.FPCamera].SetTrackingTarget(cameraPositionTransform);
				CameraModel.Cameras[Features.CameraModelModule.CameraType.TPCamera].SetTrackingTarget(cameraPositionTransform);
				CameraModel.Cameras[Features.CameraModelModule.CameraType.FPFollowCamera].SetTrackingTarget(cameraPositionTransform);
				CameraModel.Cameras[Features.CameraModelModule.CameraType.FPFollowCamera].SetLookAtTarget(PlayerMovableModel.Rotator.RotationObject);
			}
		}

		protected void CloseSpectatorAndDeathWindows()
		{
			if (SpectatorWindow.WindowStatus == WindowStatus.Showed)
			{
				SpectatorWindow.Close();
			}
			if (DeathWindow.WindowStatus == WindowStatus.Showed)
			{
				DeathWindow.Close();
			}
		}

		protected void SetDefaultLineArmController()
		{
			if (GetLocalRightArmVisualsController() != null)
			{
				SetDefaultLineArmControllerInternal();
			}
			else
			{
				ArmVisualsControllerModel.OnPlayerArmVisualsControllerRegistered += OnArmVisualsControllerRegistered;
			}
		}

		private ArmVisualsController GetLocalRightArmVisualsController()
		{
			return ArmVisualsControllerModel.GetArmVisualsController(MultiplayerModel.NetworkRunner.LocalPlayer.PlayerId, Arm.Right);
		}

		private void SetDefaultLineArmControllerInternal()
		{
			ArmVisualsControllerModel.OnPlayerArmVisualsControllerRegistered -= OnArmVisualsControllerRegistered;
			int playerId = MultiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			Dictionary<LineArmType, LineArmControllerBase> allLineArmsForPlayer = LineArmsModel.GetAllLineArmsForPlayer(playerId);
			if (allLineArmsForPlayer.TryGetValue(LineArmType.RightArmByMouse, out var value))
			{
				if (value.enabled)
				{
					value.UnJoinAll(throwItem: false);
				}
				value.enabled = false;
				if (allLineArmsForPlayer.TryGetValue(LineArmType.RightArmDefault, out var value2))
				{
					value2.enabled = true;
					ArmVisualsController localRightArmVisualsController = GetLocalRightArmVisualsController();
					localRightArmVisualsController.ReduceSolverUpdateEnabled = true;
					localRightArmVisualsController.SetLineArmController(LineArmType.RightArmDefault);
				}
			}
		}

		private void OnArmVisualsControllerRegistered(int playerId, Arm arm)
		{
			if (playerId == MultiplayerModel.NetworkRunner.LocalPlayer.PlayerId && arm == Arm.Right)
			{
				SetDefaultLineArmControllerInternal();
			}
		}

		protected void RequestMouseHidden()
		{
			MouseVisibilityModel.AddMouseVisibilityRequest(new MouseVisibilityRequest(0, isMouseVisible: false));
		}

		protected void SetCommonHudVisible(bool isVisible)
		{
			InteractModel.ChangeCursorVisibility(isVisible);
			StatsViewModel.ChangeStatsVisibility(isVisible);
			RumBuffsViewModel.ChangeBuffsVisibility(isVisible);
			PushToTalkViewModel.ChangePushToTalkVisibility(isVisible);
		}

		protected void RunAliveSequence(Action onAfterDelay = null)
		{
			SetCameraTrackingToLocalPlayer();
			CloseSpectatorAndDeathWindows();
			PlayerMovableModel.IsLookingAtCamera = false;
			AliveTailDelayed(onAfterDelay).Forget();
		}

		private async UniTaskVoid AliveTailDelayed(Action onAfterDelay)
		{
			try
			{
				await UniTask.WaitForSeconds(2, ignoreTimeScale: false, PlayerLoopTiming.Update, CancellationToken);
				onAfterDelay?.Invoke();
			}
			catch (OperationCanceledException)
			{
			}
		}
	}
}
