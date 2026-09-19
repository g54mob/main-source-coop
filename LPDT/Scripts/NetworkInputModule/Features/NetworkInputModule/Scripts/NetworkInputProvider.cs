using System;
using Features.InputModule.Scripts.Generated;
using Fusion;
using NetworkServices.NetworkEvents;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Features.NetworkInputModule.Scripts
{
	public class NetworkInputProvider : IInitializable, IDisposable
	{
		private NetworkRunnerEventBus _eventBus;

		private IInputService _inputService;

		private NetworkInputActions _networkInputActions;

		public NetworkInputProvider(NetworkRunnerEventBus eventBus, IInputService inputService)
		{
			_eventBus = eventBus;
			_inputService = inputService;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<OnInputEvent>(OnInputEvent);
			InputDefaultActions spectatorLeft = _inputService.SpectatorLeft;
			spectatorLeft.Started = (Action)Delegate.Combine(spectatorLeft.Started, new Action(SetSpectatorLeftStarted));
			InputDefaultActions spectatorLeft2 = _inputService.SpectatorLeft;
			spectatorLeft2.Performed = (Action)Delegate.Combine(spectatorLeft2.Performed, new Action(SetSpectatorLeftPerformed));
			InputDefaultActions spectatorLeft3 = _inputService.SpectatorLeft;
			spectatorLeft3.Canceled = (Action)Delegate.Combine(spectatorLeft3.Canceled, new Action(SetSpectatorLeftCanceled));
			InputDefaultActions spectatorRight = _inputService.SpectatorRight;
			spectatorRight.Started = (Action)Delegate.Combine(spectatorRight.Started, new Action(SetSpectatorRightStarted));
			InputDefaultActions spectatorRight2 = _inputService.SpectatorRight;
			spectatorRight2.Performed = (Action)Delegate.Combine(spectatorRight2.Performed, new Action(SetSpectatorRightPerformed));
			InputDefaultActions spectatorRight3 = _inputService.SpectatorRight;
			spectatorRight3.Canceled = (Action)Delegate.Combine(spectatorRight3.Canceled, new Action(SetSpectatorRightCanceled));
			InputDefaultActions openSettings = _inputService.OpenSettings;
			openSettings.Started = (Action)Delegate.Combine(openSettings.Started, new Action(SetOpenSettingsStarted));
			InputDefaultActions openSettings2 = _inputService.OpenSettings;
			openSettings2.Performed = (Action)Delegate.Combine(openSettings2.Performed, new Action(SetOpenSettingsPerformed));
			InputDefaultActions openSettings3 = _inputService.OpenSettings;
			openSettings3.Canceled = (Action)Delegate.Combine(openSettings3.Canceled, new Action(SetOpenSettingsCanceled));
			InputVector2Actions movement = _inputService.Movement;
			movement.VectorChangedPerformed = (Action<Vector2>)Delegate.Combine(movement.VectorChangedPerformed, new Action<Vector2>(SetMovement));
			InputVector2Actions movement2 = _inputService.Movement;
			movement2.VectorChangedCanceled = (Action<Vector2>)Delegate.Combine(movement2.VectorChangedCanceled, new Action<Vector2>(SetMovement));
			InputDefaultActions jump = _inputService.Jump;
			jump.Started = (Action)Delegate.Combine(jump.Started, new Action(SetJumpStarted));
			InputDefaultActions jump2 = _inputService.Jump;
			jump2.Performed = (Action)Delegate.Combine(jump2.Performed, new Action(SetJumpPerformed));
			InputDefaultActions jump3 = _inputService.Jump;
			jump3.Canceled = (Action)Delegate.Combine(jump3.Canceled, new Action(SetJumpCanceled));
			InputVector2Actions rotation = _inputService.Rotation;
			rotation.VectorChangedPerformed = (Action<Vector2>)Delegate.Combine(rotation.VectorChangedPerformed, new Action<Vector2>(SetRotation));
			InputVector2Actions rotation2 = _inputService.Rotation;
			rotation2.VectorChangedCanceled = (Action<Vector2>)Delegate.Combine(rotation2.VectorChangedCanceled, new Action<Vector2>(SetRotation));
			InputDefaultActions changeMouseVisibility = _inputService.ChangeMouseVisibility;
			changeMouseVisibility.Started = (Action)Delegate.Combine(changeMouseVisibility.Started, new Action(SetChangeMouseVisibilityStarted));
			InputDefaultActions changeMouseVisibility2 = _inputService.ChangeMouseVisibility;
			changeMouseVisibility2.Performed = (Action)Delegate.Combine(changeMouseVisibility2.Performed, new Action(SetChangeMouseVisibilityPerformed));
			InputDefaultActions changeMouseVisibility3 = _inputService.ChangeMouseVisibility;
			changeMouseVisibility3.Canceled = (Action)Delegate.Combine(changeMouseVisibility3.Canceled, new Action(SetChangeMouseVisibilityCanceled));
			InputDefaultActions crouch = _inputService.Crouch;
			crouch.Started = (Action)Delegate.Combine(crouch.Started, new Action(SetCrouchStarted));
			InputDefaultActions crouch2 = _inputService.Crouch;
			crouch2.Performed = (Action)Delegate.Combine(crouch2.Performed, new Action(SetCrouchPerformed));
			InputDefaultActions crouch3 = _inputService.Crouch;
			crouch3.Canceled = (Action)Delegate.Combine(crouch3.Canceled, new Action(SetCrouchCanceled));
			InputDefaultActions sprint = _inputService.Sprint;
			sprint.Started = (Action)Delegate.Combine(sprint.Started, new Action(SetSprintStarted));
			InputDefaultActions sprint2 = _inputService.Sprint;
			sprint2.Performed = (Action)Delegate.Combine(sprint2.Performed, new Action(SetSprintPerformed));
			InputDefaultActions sprint3 = _inputService.Sprint;
			sprint3.Canceled = (Action)Delegate.Combine(sprint3.Canceled, new Action(SetSprintCanceled));
			InputDefaultActions grabItem = _inputService.GrabItem;
			grabItem.Started = (Action)Delegate.Combine(grabItem.Started, new Action(SetGrabItemStarted));
			InputDefaultActions grabItem2 = _inputService.GrabItem;
			grabItem2.Performed = (Action)Delegate.Combine(grabItem2.Performed, new Action(SetGrabItemPerformed));
			InputDefaultActions grabItem3 = _inputService.GrabItem;
			grabItem3.Canceled = (Action)Delegate.Combine(grabItem3.Canceled, new Action(SetGrabItemCanceled));
			InputDefaultActions dropAllFromArms = _inputService.DropAllFromArms;
			dropAllFromArms.Started = (Action)Delegate.Combine(dropAllFromArms.Started, new Action(SetDropAllFromArmsStarted));
			InputDefaultActions dropAllFromArms2 = _inputService.DropAllFromArms;
			dropAllFromArms2.Performed = (Action)Delegate.Combine(dropAllFromArms2.Performed, new Action(SetDropAllFromArmsPerformed));
			InputDefaultActions dropAllFromArms3 = _inputService.DropAllFromArms;
			dropAllFromArms3.Canceled = (Action)Delegate.Combine(dropAllFromArms3.Canceled, new Action(SetDropAllFromArmsCanceled));
			InputVector2Actions armItemDistanceChange = _inputService.ArmItemDistanceChange;
			armItemDistanceChange.VectorChangedPerformed = (Action<Vector2>)Delegate.Combine(armItemDistanceChange.VectorChangedPerformed, new Action<Vector2>(SetArmItemDistanceChange));
			InputVector2Actions armItemDistanceChange2 = _inputService.ArmItemDistanceChange;
			armItemDistanceChange2.VectorChangedCanceled = (Action<Vector2>)Delegate.Combine(armItemDistanceChange2.VectorChangedCanceled, new Action<Vector2>(SetArmItemDistanceChange));
			InputDefaultActions itemInteract = _inputService.ItemInteract;
			itemInteract.Started = (Action)Delegate.Combine(itemInteract.Started, new Action(SetItemInteractStarted));
			InputDefaultActions itemInteract2 = _inputService.ItemInteract;
			itemInteract2.Performed = (Action)Delegate.Combine(itemInteract2.Performed, new Action(SetItemInteractPerformed));
			InputDefaultActions itemInteract3 = _inputService.ItemInteract;
			itemInteract3.Canceled = (Action)Delegate.Combine(itemInteract3.Canceled, new Action(SetItemInteractCanceled));
			InputDefaultActions mouseForward = _inputService.MouseForward;
			mouseForward.Started = (Action)Delegate.Combine(mouseForward.Started, new Action(SetMouseForwardStarted));
			InputDefaultActions mouseForward2 = _inputService.MouseForward;
			mouseForward2.Performed = (Action)Delegate.Combine(mouseForward2.Performed, new Action(SetMouseForwardPerformed));
			InputDefaultActions mouseForward3 = _inputService.MouseForward;
			mouseForward3.Canceled = (Action)Delegate.Combine(mouseForward3.Canceled, new Action(SetMouseForwardCanceled));
			InputDefaultActions bodyEmote = _inputService.BodyEmote;
			bodyEmote.Started = (Action)Delegate.Combine(bodyEmote.Started, new Action(SetBodyEmoteStarted));
			InputDefaultActions bodyEmote2 = _inputService.BodyEmote;
			bodyEmote2.Performed = (Action)Delegate.Combine(bodyEmote2.Performed, new Action(SetBodyEmotePerformed));
			InputDefaultActions bodyEmote3 = _inputService.BodyEmote;
			bodyEmote3.Canceled = (Action)Delegate.Combine(bodyEmote3.Canceled, new Action(SetBodyEmoteCanceled));
			InputDefaultActions faceEmote = _inputService.FaceEmote;
			faceEmote.Started = (Action)Delegate.Combine(faceEmote.Started, new Action(SetFaceEmoteStarted));
			InputDefaultActions faceEmote2 = _inputService.FaceEmote;
			faceEmote2.Performed = (Action)Delegate.Combine(faceEmote2.Performed, new Action(SetFaceEmotePerformed));
			InputDefaultActions faceEmote3 = _inputService.FaceEmote;
			faceEmote3.Canceled = (Action)Delegate.Combine(faceEmote3.Canceled, new Action(SetFaceEmoteCanceled));
			InputDefaultActions handEmote = _inputService.HandEmote;
			handEmote.Started = (Action)Delegate.Combine(handEmote.Started, new Action(SetHandEmoteStarted));
			InputDefaultActions handEmote2 = _inputService.HandEmote;
			handEmote2.Performed = (Action)Delegate.Combine(handEmote2.Performed, new Action(SetHandEmotePerformed));
			InputDefaultActions handEmote3 = _inputService.HandEmote;
			handEmote3.Canceled = (Action)Delegate.Combine(handEmote3.Canceled, new Action(SetHandEmoteCanceled));
			InputVector2Actions emoteNavigation = _inputService.EmoteNavigation;
			emoteNavigation.VectorChangedPerformed = (Action<Vector2>)Delegate.Combine(emoteNavigation.VectorChangedPerformed, new Action<Vector2>(SetEmoteNavigation));
			InputVector2Actions emoteNavigation2 = _inputService.EmoteNavigation;
			emoteNavigation2.VectorChangedCanceled = (Action<Vector2>)Delegate.Combine(emoteNavigation2.VectorChangedCanceled, new Action<Vector2>(SetEmoteNavigation));
			InputDefaultActions hotBar_ = _inputService.HotBar_1;
			hotBar_.Started = (Action)Delegate.Combine(hotBar_.Started, new Action(SetHotBar_1Started));
			InputDefaultActions hotBar_2 = _inputService.HotBar_1;
			hotBar_2.Performed = (Action)Delegate.Combine(hotBar_2.Performed, new Action(SetHotBar_1Performed));
			InputDefaultActions hotBar_3 = _inputService.HotBar_1;
			hotBar_3.Canceled = (Action)Delegate.Combine(hotBar_3.Canceled, new Action(SetHotBar_1Canceled));
			InputDefaultActions hotBar_4 = _inputService.HotBar_2;
			hotBar_4.Started = (Action)Delegate.Combine(hotBar_4.Started, new Action(SetHotBar_2Started));
			InputDefaultActions hotBar_5 = _inputService.HotBar_2;
			hotBar_5.Performed = (Action)Delegate.Combine(hotBar_5.Performed, new Action(SetHotBar_2Performed));
			InputDefaultActions hotBar_6 = _inputService.HotBar_2;
			hotBar_6.Canceled = (Action)Delegate.Combine(hotBar_6.Canceled, new Action(SetHotBar_2Canceled));
			InputDefaultActions hotBar_7 = _inputService.HotBar_3;
			hotBar_7.Started = (Action)Delegate.Combine(hotBar_7.Started, new Action(SetHotBar_3Started));
			InputDefaultActions hotBar_8 = _inputService.HotBar_3;
			hotBar_8.Performed = (Action)Delegate.Combine(hotBar_8.Performed, new Action(SetHotBar_3Performed));
			InputDefaultActions hotBar_9 = _inputService.HotBar_3;
			hotBar_9.Canceled = (Action)Delegate.Combine(hotBar_9.Canceled, new Action(SetHotBar_3Canceled));
			InputDefaultActions hotBar_10 = _inputService.HotBar_4;
			hotBar_10.Started = (Action)Delegate.Combine(hotBar_10.Started, new Action(SetHotBar_4Started));
			InputDefaultActions hotBar_11 = _inputService.HotBar_4;
			hotBar_11.Performed = (Action)Delegate.Combine(hotBar_11.Performed, new Action(SetHotBar_4Performed));
			InputDefaultActions hotBar_12 = _inputService.HotBar_4;
			hotBar_12.Canceled = (Action)Delegate.Combine(hotBar_12.Canceled, new Action(SetHotBar_4Canceled));
			InputDefaultActions hotBar_13 = _inputService.HotBar_5;
			hotBar_13.Started = (Action)Delegate.Combine(hotBar_13.Started, new Action(SetHotBar_5Started));
			InputDefaultActions hotBar_14 = _inputService.HotBar_5;
			hotBar_14.Performed = (Action)Delegate.Combine(hotBar_14.Performed, new Action(SetHotBar_5Performed));
			InputDefaultActions hotBar_15 = _inputService.HotBar_5;
			hotBar_15.Canceled = (Action)Delegate.Combine(hotBar_15.Canceled, new Action(SetHotBar_5Canceled));
			InputDefaultActions hotBar_16 = _inputService.HotBar_6;
			hotBar_16.Started = (Action)Delegate.Combine(hotBar_16.Started, new Action(SetHotBar_6Started));
			InputDefaultActions hotBar_17 = _inputService.HotBar_6;
			hotBar_17.Performed = (Action)Delegate.Combine(hotBar_17.Performed, new Action(SetHotBar_6Performed));
			InputDefaultActions hotBar_18 = _inputService.HotBar_6;
			hotBar_18.Canceled = (Action)Delegate.Combine(hotBar_18.Canceled, new Action(SetHotBar_6Canceled));
			InputDefaultActions hotBar_19 = _inputService.HotBar_7;
			hotBar_19.Started = (Action)Delegate.Combine(hotBar_19.Started, new Action(SetHotBar_7Started));
			InputDefaultActions hotBar_20 = _inputService.HotBar_7;
			hotBar_20.Performed = (Action)Delegate.Combine(hotBar_20.Performed, new Action(SetHotBar_7Performed));
			InputDefaultActions hotBar_21 = _inputService.HotBar_7;
			hotBar_21.Canceled = (Action)Delegate.Combine(hotBar_21.Canceled, new Action(SetHotBar_7Canceled));
			InputDefaultActions hotBar_22 = _inputService.HotBar_8;
			hotBar_22.Started = (Action)Delegate.Combine(hotBar_22.Started, new Action(SetHotBar_8Started));
			InputDefaultActions hotBar_23 = _inputService.HotBar_8;
			hotBar_23.Performed = (Action)Delegate.Combine(hotBar_23.Performed, new Action(SetHotBar_8Performed));
			InputDefaultActions hotBar_24 = _inputService.HotBar_8;
			hotBar_24.Canceled = (Action)Delegate.Combine(hotBar_24.Canceled, new Action(SetHotBar_8Canceled));
			InputDefaultActions hotBar_25 = _inputService.HotBar_9;
			hotBar_25.Started = (Action)Delegate.Combine(hotBar_25.Started, new Action(SetHotBar_9Started));
			InputDefaultActions hotBar_26 = _inputService.HotBar_9;
			hotBar_26.Performed = (Action)Delegate.Combine(hotBar_26.Performed, new Action(SetHotBar_9Performed));
			InputDefaultActions hotBar_27 = _inputService.HotBar_9;
			hotBar_27.Canceled = (Action)Delegate.Combine(hotBar_27.Canceled, new Action(SetHotBar_9Canceled));
			InputDefaultActions hotBar_28 = _inputService.HotBar_0;
			hotBar_28.Started = (Action)Delegate.Combine(hotBar_28.Started, new Action(SetHotBar_0Started));
			InputDefaultActions hotBar_29 = _inputService.HotBar_0;
			hotBar_29.Performed = (Action)Delegate.Combine(hotBar_29.Performed, new Action(SetHotBar_0Performed));
			InputDefaultActions hotBar_30 = _inputService.HotBar_0;
			hotBar_30.Canceled = (Action)Delegate.Combine(hotBar_30.Canceled, new Action(SetHotBar_0Canceled));
			InputDefaultActions turnOnOffOverlay = _inputService.TurnOnOffOverlay;
			turnOnOffOverlay.Started = (Action)Delegate.Combine(turnOnOffOverlay.Started, new Action(SetTurnOnOffOverlayStarted));
			InputDefaultActions turnOnOffOverlay2 = _inputService.TurnOnOffOverlay;
			turnOnOffOverlay2.Performed = (Action)Delegate.Combine(turnOnOffOverlay2.Performed, new Action(SetTurnOnOffOverlayPerformed));
			InputDefaultActions turnOnOffOverlay3 = _inputService.TurnOnOffOverlay;
			turnOnOffOverlay3.Canceled = (Action)Delegate.Combine(turnOnOffOverlay3.Canceled, new Action(SetTurnOnOffOverlayCanceled));
			InputVector2Actions anyVectorChange = _inputService.AnyVectorChange;
			anyVectorChange.VectorChangedPerformed = (Action<Vector2>)Delegate.Combine(anyVectorChange.VectorChangedPerformed, new Action<Vector2>(SetAnyVectorChange));
			InputVector2Actions anyVectorChange2 = _inputService.AnyVectorChange;
			anyVectorChange2.VectorChangedCanceled = (Action<Vector2>)Delegate.Combine(anyVectorChange2.VectorChangedCanceled, new Action<Vector2>(SetAnyVectorChange));
			InputDefaultActions uIApply = _inputService.UIApply;
			uIApply.Started = (Action)Delegate.Combine(uIApply.Started, new Action(SetUIApplyStarted));
			InputDefaultActions uIApply2 = _inputService.UIApply;
			uIApply2.Performed = (Action)Delegate.Combine(uIApply2.Performed, new Action(SetUIApplyPerformed));
			InputDefaultActions uIApply3 = _inputService.UIApply;
			uIApply3.Canceled = (Action)Delegate.Combine(uIApply3.Canceled, new Action(SetUIApplyCanceled));
			InputDefaultActions uIApplyWindow = _inputService.UIApplyWindow;
			uIApplyWindow.Started = (Action)Delegate.Combine(uIApplyWindow.Started, new Action(SetUIApplyWindowStarted));
			InputDefaultActions uIApplyWindow2 = _inputService.UIApplyWindow;
			uIApplyWindow2.Performed = (Action)Delegate.Combine(uIApplyWindow2.Performed, new Action(SetUIApplyWindowPerformed));
			InputDefaultActions uIApplyWindow3 = _inputService.UIApplyWindow;
			uIApplyWindow3.Canceled = (Action)Delegate.Combine(uIApplyWindow3.Canceled, new Action(SetUIApplyWindowCanceled));
			InputDefaultActions uIBack = _inputService.UIBack;
			uIBack.Started = (Action)Delegate.Combine(uIBack.Started, new Action(SetUIBackStarted));
			InputDefaultActions uIBack2 = _inputService.UIBack;
			uIBack2.Performed = (Action)Delegate.Combine(uIBack2.Performed, new Action(SetUIBackPerformed));
			InputDefaultActions uIBack3 = _inputService.UIBack;
			uIBack3.Canceled = (Action)Delegate.Combine(uIBack3.Canceled, new Action(SetUIBackCanceled));
			InputVector2Actions navigate = _inputService.Navigate;
			navigate.VectorChangedPerformed = (Action<Vector2>)Delegate.Combine(navigate.VectorChangedPerformed, new Action<Vector2>(SetNavigate));
			InputVector2Actions navigate2 = _inputService.Navigate;
			navigate2.VectorChangedCanceled = (Action<Vector2>)Delegate.Combine(navigate2.VectorChangedCanceled, new Action<Vector2>(SetNavigate));
			InputDefaultActions submit = _inputService.Submit;
			submit.Started = (Action)Delegate.Combine(submit.Started, new Action(SetSubmitStarted));
			InputDefaultActions submit2 = _inputService.Submit;
			submit2.Performed = (Action)Delegate.Combine(submit2.Performed, new Action(SetSubmitPerformed));
			InputDefaultActions submit3 = _inputService.Submit;
			submit3.Canceled = (Action)Delegate.Combine(submit3.Canceled, new Action(SetSubmitCanceled));
			InputDefaultActions cancel = _inputService.Cancel;
			cancel.Started = (Action)Delegate.Combine(cancel.Started, new Action(SetCancelStarted));
			InputDefaultActions cancel2 = _inputService.Cancel;
			cancel2.Performed = (Action)Delegate.Combine(cancel2.Performed, new Action(SetCancelPerformed));
			InputDefaultActions cancel3 = _inputService.Cancel;
			cancel3.Canceled = (Action)Delegate.Combine(cancel3.Canceled, new Action(SetCancelCanceled));
			InputVector2Actions point = _inputService.Point;
			point.VectorChangedPerformed = (Action<Vector2>)Delegate.Combine(point.VectorChangedPerformed, new Action<Vector2>(SetPoint));
			InputVector2Actions point2 = _inputService.Point;
			point2.VectorChangedCanceled = (Action<Vector2>)Delegate.Combine(point2.VectorChangedCanceled, new Action<Vector2>(SetPoint));
			InputDefaultActions click = _inputService.Click;
			click.Started = (Action)Delegate.Combine(click.Started, new Action(SetClickStarted));
			InputDefaultActions click2 = _inputService.Click;
			click2.Performed = (Action)Delegate.Combine(click2.Performed, new Action(SetClickPerformed));
			InputDefaultActions click3 = _inputService.Click;
			click3.Canceled = (Action)Delegate.Combine(click3.Canceled, new Action(SetClickCanceled));
			InputVector2Actions scrollWheel = _inputService.ScrollWheel;
			scrollWheel.VectorChangedPerformed = (Action<Vector2>)Delegate.Combine(scrollWheel.VectorChangedPerformed, new Action<Vector2>(SetScrollWheel));
			InputVector2Actions scrollWheel2 = _inputService.ScrollWheel;
			scrollWheel2.VectorChangedCanceled = (Action<Vector2>)Delegate.Combine(scrollWheel2.VectorChangedCanceled, new Action<Vector2>(SetScrollWheel));
			InputDefaultActions middleClick = _inputService.MiddleClick;
			middleClick.Started = (Action)Delegate.Combine(middleClick.Started, new Action(SetMiddleClickStarted));
			InputDefaultActions middleClick2 = _inputService.MiddleClick;
			middleClick2.Performed = (Action)Delegate.Combine(middleClick2.Performed, new Action(SetMiddleClickPerformed));
			InputDefaultActions middleClick3 = _inputService.MiddleClick;
			middleClick3.Canceled = (Action)Delegate.Combine(middleClick3.Canceled, new Action(SetMiddleClickCanceled));
			InputDefaultActions rightClick = _inputService.RightClick;
			rightClick.Started = (Action)Delegate.Combine(rightClick.Started, new Action(SetRightClickStarted));
			InputDefaultActions rightClick2 = _inputService.RightClick;
			rightClick2.Performed = (Action)Delegate.Combine(rightClick2.Performed, new Action(SetRightClickPerformed));
			InputDefaultActions rightClick3 = _inputService.RightClick;
			rightClick3.Canceled = (Action)Delegate.Combine(rightClick3.Canceled, new Action(SetRightClickCanceled));
			InputDefaultActions trackedDevicePosition = _inputService.TrackedDevicePosition;
			trackedDevicePosition.Started = (Action)Delegate.Combine(trackedDevicePosition.Started, new Action(SetTrackedDevicePositionStarted));
			InputDefaultActions trackedDevicePosition2 = _inputService.TrackedDevicePosition;
			trackedDevicePosition2.Performed = (Action)Delegate.Combine(trackedDevicePosition2.Performed, new Action(SetTrackedDevicePositionPerformed));
			InputDefaultActions trackedDevicePosition3 = _inputService.TrackedDevicePosition;
			trackedDevicePosition3.Canceled = (Action)Delegate.Combine(trackedDevicePosition3.Canceled, new Action(SetTrackedDevicePositionCanceled));
			InputDefaultActions trackedDeviceOrientation = _inputService.TrackedDeviceOrientation;
			trackedDeviceOrientation.Started = (Action)Delegate.Combine(trackedDeviceOrientation.Started, new Action(SetTrackedDeviceOrientationStarted));
			InputDefaultActions trackedDeviceOrientation2 = _inputService.TrackedDeviceOrientation;
			trackedDeviceOrientation2.Performed = (Action)Delegate.Combine(trackedDeviceOrientation2.Performed, new Action(SetTrackedDeviceOrientationPerformed));
			InputDefaultActions trackedDeviceOrientation3 = _inputService.TrackedDeviceOrientation;
			trackedDeviceOrientation3.Canceled = (Action)Delegate.Combine(trackedDeviceOrientation3.Canceled, new Action(SetTrackedDeviceOrientationCanceled));
			InputDefaultActions upgradesBack = _inputService.UpgradesBack;
			upgradesBack.Started = (Action)Delegate.Combine(upgradesBack.Started, new Action(SetUpgradesBackStarted));
			InputDefaultActions upgradesBack2 = _inputService.UpgradesBack;
			upgradesBack2.Performed = (Action)Delegate.Combine(upgradesBack2.Performed, new Action(SetUpgradesBackPerformed));
			InputDefaultActions upgradesBack3 = _inputService.UpgradesBack;
			upgradesBack3.Canceled = (Action)Delegate.Combine(upgradesBack3.Canceled, new Action(SetUpgradesBackCanceled));
			InputDefaultActions additionalNavigationLeft = _inputService.AdditionalNavigationLeft;
			additionalNavigationLeft.Started = (Action)Delegate.Combine(additionalNavigationLeft.Started, new Action(SetAdditionalNavigationLeftStarted));
			InputDefaultActions additionalNavigationLeft2 = _inputService.AdditionalNavigationLeft;
			additionalNavigationLeft2.Performed = (Action)Delegate.Combine(additionalNavigationLeft2.Performed, new Action(SetAdditionalNavigationLeftPerformed));
			InputDefaultActions additionalNavigationLeft3 = _inputService.AdditionalNavigationLeft;
			additionalNavigationLeft3.Canceled = (Action)Delegate.Combine(additionalNavigationLeft3.Canceled, new Action(SetAdditionalNavigationLeftCanceled));
			InputDefaultActions additionalNavigationRight = _inputService.AdditionalNavigationRight;
			additionalNavigationRight.Started = (Action)Delegate.Combine(additionalNavigationRight.Started, new Action(SetAdditionalNavigationRightStarted));
			InputDefaultActions additionalNavigationRight2 = _inputService.AdditionalNavigationRight;
			additionalNavigationRight2.Performed = (Action)Delegate.Combine(additionalNavigationRight2.Performed, new Action(SetAdditionalNavigationRightPerformed));
			InputDefaultActions additionalNavigationRight3 = _inputService.AdditionalNavigationRight;
			additionalNavigationRight3.Canceled = (Action)Delegate.Combine(additionalNavigationRight3.Canceled, new Action(SetAdditionalNavigationRightCanceled));
			InputDefaultActions extraAdditionalNavigationLeft = _inputService.ExtraAdditionalNavigationLeft;
			extraAdditionalNavigationLeft.Started = (Action)Delegate.Combine(extraAdditionalNavigationLeft.Started, new Action(SetExtraAdditionalNavigationLeftStarted));
			InputDefaultActions extraAdditionalNavigationLeft2 = _inputService.ExtraAdditionalNavigationLeft;
			extraAdditionalNavigationLeft2.Performed = (Action)Delegate.Combine(extraAdditionalNavigationLeft2.Performed, new Action(SetExtraAdditionalNavigationLeftPerformed));
			InputDefaultActions extraAdditionalNavigationLeft3 = _inputService.ExtraAdditionalNavigationLeft;
			extraAdditionalNavigationLeft3.Canceled = (Action)Delegate.Combine(extraAdditionalNavigationLeft3.Canceled, new Action(SetExtraAdditionalNavigationLeftCanceled));
			InputDefaultActions extraAdditionalNavigationRight = _inputService.ExtraAdditionalNavigationRight;
			extraAdditionalNavigationRight.Started = (Action)Delegate.Combine(extraAdditionalNavigationRight.Started, new Action(SetExtraAdditionalNavigationRightStarted));
			InputDefaultActions extraAdditionalNavigationRight2 = _inputService.ExtraAdditionalNavigationRight;
			extraAdditionalNavigationRight2.Performed = (Action)Delegate.Combine(extraAdditionalNavigationRight2.Performed, new Action(SetExtraAdditionalNavigationRightPerformed));
			InputDefaultActions extraAdditionalNavigationRight3 = _inputService.ExtraAdditionalNavigationRight;
			extraAdditionalNavigationRight3.Canceled = (Action)Delegate.Combine(extraAdditionalNavigationRight3.Canceled, new Action(SetExtraAdditionalNavigationRightCanceled));
			InputDefaultActions holdApply = _inputService.HoldApply;
			holdApply.Started = (Action)Delegate.Combine(holdApply.Started, new Action(SetHoldApplyStarted));
			InputDefaultActions holdApply2 = _inputService.HoldApply;
			holdApply2.Performed = (Action)Delegate.Combine(holdApply2.Performed, new Action(SetHoldApplyPerformed));
			InputDefaultActions holdApply3 = _inputService.HoldApply;
			holdApply3.Canceled = (Action)Delegate.Combine(holdApply3.Canceled, new Action(SetHoldApplyCanceled));
			InputDefaultActions pushToTalk = _inputService.PushToTalk;
			pushToTalk.Started = (Action)Delegate.Combine(pushToTalk.Started, new Action(SetPushToTalkStarted));
			InputDefaultActions pushToTalk2 = _inputService.PushToTalk;
			pushToTalk2.Performed = (Action)Delegate.Combine(pushToTalk2.Performed, new Action(SetPushToTalkPerformed));
			InputDefaultActions pushToTalk3 = _inputService.PushToTalk;
			pushToTalk3.Canceled = (Action)Delegate.Combine(pushToTalk3.Canceled, new Action(SetPushToTalkCanceled));
			InputDefaultActions gameplayApply = _inputService.GameplayApply;
			gameplayApply.Started = (Action)Delegate.Combine(gameplayApply.Started, new Action(SetGameplayApplyStarted));
			InputDefaultActions gameplayApply2 = _inputService.GameplayApply;
			gameplayApply2.Performed = (Action)Delegate.Combine(gameplayApply2.Performed, new Action(SetGameplayApplyPerformed));
			InputDefaultActions gameplayApply3 = _inputService.GameplayApply;
			gameplayApply3.Canceled = (Action)Delegate.Combine(gameplayApply3.Canceled, new Action(SetGameplayApplyCanceled));
		}

		public void Dispose()
		{
			_eventBus.Unsubscribe<OnInputEvent>(OnInputEvent);
			InputDefaultActions spectatorLeft = _inputService.SpectatorLeft;
			spectatorLeft.Started = (Action)Delegate.Remove(spectatorLeft.Started, new Action(SetSpectatorLeftStarted));
			InputDefaultActions spectatorLeft2 = _inputService.SpectatorLeft;
			spectatorLeft2.Performed = (Action)Delegate.Remove(spectatorLeft2.Performed, new Action(SetSpectatorLeftPerformed));
			InputDefaultActions spectatorLeft3 = _inputService.SpectatorLeft;
			spectatorLeft3.Canceled = (Action)Delegate.Remove(spectatorLeft3.Canceled, new Action(SetSpectatorLeftCanceled));
			InputDefaultActions spectatorRight = _inputService.SpectatorRight;
			spectatorRight.Started = (Action)Delegate.Remove(spectatorRight.Started, new Action(SetSpectatorRightStarted));
			InputDefaultActions spectatorRight2 = _inputService.SpectatorRight;
			spectatorRight2.Performed = (Action)Delegate.Remove(spectatorRight2.Performed, new Action(SetSpectatorRightPerformed));
			InputDefaultActions spectatorRight3 = _inputService.SpectatorRight;
			spectatorRight3.Canceled = (Action)Delegate.Remove(spectatorRight3.Canceled, new Action(SetSpectatorRightCanceled));
			InputDefaultActions openSettings = _inputService.OpenSettings;
			openSettings.Started = (Action)Delegate.Remove(openSettings.Started, new Action(SetOpenSettingsStarted));
			InputDefaultActions openSettings2 = _inputService.OpenSettings;
			openSettings2.Performed = (Action)Delegate.Remove(openSettings2.Performed, new Action(SetOpenSettingsPerformed));
			InputDefaultActions openSettings3 = _inputService.OpenSettings;
			openSettings3.Canceled = (Action)Delegate.Remove(openSettings3.Canceled, new Action(SetOpenSettingsCanceled));
			InputVector2Actions movement = _inputService.Movement;
			movement.VectorChangedPerformed = (Action<Vector2>)Delegate.Remove(movement.VectorChangedPerformed, new Action<Vector2>(SetMovement));
			InputVector2Actions movement2 = _inputService.Movement;
			movement2.VectorChangedCanceled = (Action<Vector2>)Delegate.Remove(movement2.VectorChangedCanceled, new Action<Vector2>(SetMovement));
			InputDefaultActions jump = _inputService.Jump;
			jump.Started = (Action)Delegate.Remove(jump.Started, new Action(SetJumpStarted));
			InputDefaultActions jump2 = _inputService.Jump;
			jump2.Performed = (Action)Delegate.Remove(jump2.Performed, new Action(SetJumpPerformed));
			InputDefaultActions jump3 = _inputService.Jump;
			jump3.Canceled = (Action)Delegate.Remove(jump3.Canceled, new Action(SetJumpCanceled));
			InputVector2Actions rotation = _inputService.Rotation;
			rotation.VectorChangedPerformed = (Action<Vector2>)Delegate.Remove(rotation.VectorChangedPerformed, new Action<Vector2>(SetRotation));
			InputVector2Actions rotation2 = _inputService.Rotation;
			rotation2.VectorChangedCanceled = (Action<Vector2>)Delegate.Remove(rotation2.VectorChangedCanceled, new Action<Vector2>(SetRotation));
			InputDefaultActions changeMouseVisibility = _inputService.ChangeMouseVisibility;
			changeMouseVisibility.Started = (Action)Delegate.Remove(changeMouseVisibility.Started, new Action(SetChangeMouseVisibilityStarted));
			InputDefaultActions changeMouseVisibility2 = _inputService.ChangeMouseVisibility;
			changeMouseVisibility2.Performed = (Action)Delegate.Remove(changeMouseVisibility2.Performed, new Action(SetChangeMouseVisibilityPerformed));
			InputDefaultActions changeMouseVisibility3 = _inputService.ChangeMouseVisibility;
			changeMouseVisibility3.Canceled = (Action)Delegate.Remove(changeMouseVisibility3.Canceled, new Action(SetChangeMouseVisibilityCanceled));
			InputDefaultActions crouch = _inputService.Crouch;
			crouch.Started = (Action)Delegate.Remove(crouch.Started, new Action(SetCrouchStarted));
			InputDefaultActions crouch2 = _inputService.Crouch;
			crouch2.Performed = (Action)Delegate.Remove(crouch2.Performed, new Action(SetCrouchPerformed));
			InputDefaultActions crouch3 = _inputService.Crouch;
			crouch3.Canceled = (Action)Delegate.Remove(crouch3.Canceled, new Action(SetCrouchCanceled));
			InputDefaultActions sprint = _inputService.Sprint;
			sprint.Started = (Action)Delegate.Remove(sprint.Started, new Action(SetSprintStarted));
			InputDefaultActions sprint2 = _inputService.Sprint;
			sprint2.Performed = (Action)Delegate.Remove(sprint2.Performed, new Action(SetSprintPerformed));
			InputDefaultActions sprint3 = _inputService.Sprint;
			sprint3.Canceled = (Action)Delegate.Remove(sprint3.Canceled, new Action(SetSprintCanceled));
			InputDefaultActions grabItem = _inputService.GrabItem;
			grabItem.Started = (Action)Delegate.Remove(grabItem.Started, new Action(SetGrabItemStarted));
			InputDefaultActions grabItem2 = _inputService.GrabItem;
			grabItem2.Performed = (Action)Delegate.Remove(grabItem2.Performed, new Action(SetGrabItemPerformed));
			InputDefaultActions grabItem3 = _inputService.GrabItem;
			grabItem3.Canceled = (Action)Delegate.Remove(grabItem3.Canceled, new Action(SetGrabItemCanceled));
			InputDefaultActions dropAllFromArms = _inputService.DropAllFromArms;
			dropAllFromArms.Started = (Action)Delegate.Remove(dropAllFromArms.Started, new Action(SetDropAllFromArmsStarted));
			InputDefaultActions dropAllFromArms2 = _inputService.DropAllFromArms;
			dropAllFromArms2.Performed = (Action)Delegate.Remove(dropAllFromArms2.Performed, new Action(SetDropAllFromArmsPerformed));
			InputDefaultActions dropAllFromArms3 = _inputService.DropAllFromArms;
			dropAllFromArms3.Canceled = (Action)Delegate.Remove(dropAllFromArms3.Canceled, new Action(SetDropAllFromArmsCanceled));
			InputVector2Actions armItemDistanceChange = _inputService.ArmItemDistanceChange;
			armItemDistanceChange.VectorChangedPerformed = (Action<Vector2>)Delegate.Remove(armItemDistanceChange.VectorChangedPerformed, new Action<Vector2>(SetArmItemDistanceChange));
			InputVector2Actions armItemDistanceChange2 = _inputService.ArmItemDistanceChange;
			armItemDistanceChange2.VectorChangedCanceled = (Action<Vector2>)Delegate.Remove(armItemDistanceChange2.VectorChangedCanceled, new Action<Vector2>(SetArmItemDistanceChange));
			InputDefaultActions itemInteract = _inputService.ItemInteract;
			itemInteract.Started = (Action)Delegate.Remove(itemInteract.Started, new Action(SetItemInteractStarted));
			InputDefaultActions itemInteract2 = _inputService.ItemInteract;
			itemInteract2.Performed = (Action)Delegate.Remove(itemInteract2.Performed, new Action(SetItemInteractPerformed));
			InputDefaultActions itemInteract3 = _inputService.ItemInteract;
			itemInteract3.Canceled = (Action)Delegate.Remove(itemInteract3.Canceled, new Action(SetItemInteractCanceled));
			InputDefaultActions mouseForward = _inputService.MouseForward;
			mouseForward.Started = (Action)Delegate.Remove(mouseForward.Started, new Action(SetMouseForwardStarted));
			InputDefaultActions mouseForward2 = _inputService.MouseForward;
			mouseForward2.Performed = (Action)Delegate.Remove(mouseForward2.Performed, new Action(SetMouseForwardPerformed));
			InputDefaultActions mouseForward3 = _inputService.MouseForward;
			mouseForward3.Canceled = (Action)Delegate.Remove(mouseForward3.Canceled, new Action(SetMouseForwardCanceled));
			InputDefaultActions bodyEmote = _inputService.BodyEmote;
			bodyEmote.Started = (Action)Delegate.Remove(bodyEmote.Started, new Action(SetBodyEmoteStarted));
			InputDefaultActions bodyEmote2 = _inputService.BodyEmote;
			bodyEmote2.Performed = (Action)Delegate.Remove(bodyEmote2.Performed, new Action(SetBodyEmotePerformed));
			InputDefaultActions bodyEmote3 = _inputService.BodyEmote;
			bodyEmote3.Canceled = (Action)Delegate.Remove(bodyEmote3.Canceled, new Action(SetBodyEmoteCanceled));
			InputDefaultActions faceEmote = _inputService.FaceEmote;
			faceEmote.Started = (Action)Delegate.Remove(faceEmote.Started, new Action(SetFaceEmoteStarted));
			InputDefaultActions faceEmote2 = _inputService.FaceEmote;
			faceEmote2.Performed = (Action)Delegate.Remove(faceEmote2.Performed, new Action(SetFaceEmotePerformed));
			InputDefaultActions faceEmote3 = _inputService.FaceEmote;
			faceEmote3.Canceled = (Action)Delegate.Remove(faceEmote3.Canceled, new Action(SetFaceEmoteCanceled));
			InputDefaultActions handEmote = _inputService.HandEmote;
			handEmote.Started = (Action)Delegate.Remove(handEmote.Started, new Action(SetHandEmoteStarted));
			InputDefaultActions handEmote2 = _inputService.HandEmote;
			handEmote2.Performed = (Action)Delegate.Remove(handEmote2.Performed, new Action(SetHandEmotePerformed));
			InputDefaultActions handEmote3 = _inputService.HandEmote;
			handEmote3.Canceled = (Action)Delegate.Remove(handEmote3.Canceled, new Action(SetHandEmoteCanceled));
			InputVector2Actions emoteNavigation = _inputService.EmoteNavigation;
			emoteNavigation.VectorChangedPerformed = (Action<Vector2>)Delegate.Remove(emoteNavigation.VectorChangedPerformed, new Action<Vector2>(SetEmoteNavigation));
			InputVector2Actions emoteNavigation2 = _inputService.EmoteNavigation;
			emoteNavigation2.VectorChangedCanceled = (Action<Vector2>)Delegate.Remove(emoteNavigation2.VectorChangedCanceled, new Action<Vector2>(SetEmoteNavigation));
			InputDefaultActions hotBar_ = _inputService.HotBar_1;
			hotBar_.Started = (Action)Delegate.Remove(hotBar_.Started, new Action(SetHotBar_1Started));
			InputDefaultActions hotBar_2 = _inputService.HotBar_1;
			hotBar_2.Performed = (Action)Delegate.Remove(hotBar_2.Performed, new Action(SetHotBar_1Performed));
			InputDefaultActions hotBar_3 = _inputService.HotBar_1;
			hotBar_3.Canceled = (Action)Delegate.Remove(hotBar_3.Canceled, new Action(SetHotBar_1Canceled));
			InputDefaultActions hotBar_4 = _inputService.HotBar_2;
			hotBar_4.Started = (Action)Delegate.Remove(hotBar_4.Started, new Action(SetHotBar_2Started));
			InputDefaultActions hotBar_5 = _inputService.HotBar_2;
			hotBar_5.Performed = (Action)Delegate.Remove(hotBar_5.Performed, new Action(SetHotBar_2Performed));
			InputDefaultActions hotBar_6 = _inputService.HotBar_2;
			hotBar_6.Canceled = (Action)Delegate.Remove(hotBar_6.Canceled, new Action(SetHotBar_2Canceled));
			InputDefaultActions hotBar_7 = _inputService.HotBar_3;
			hotBar_7.Started = (Action)Delegate.Remove(hotBar_7.Started, new Action(SetHotBar_3Started));
			InputDefaultActions hotBar_8 = _inputService.HotBar_3;
			hotBar_8.Performed = (Action)Delegate.Remove(hotBar_8.Performed, new Action(SetHotBar_3Performed));
			InputDefaultActions hotBar_9 = _inputService.HotBar_3;
			hotBar_9.Canceled = (Action)Delegate.Remove(hotBar_9.Canceled, new Action(SetHotBar_3Canceled));
			InputDefaultActions hotBar_10 = _inputService.HotBar_4;
			hotBar_10.Started = (Action)Delegate.Remove(hotBar_10.Started, new Action(SetHotBar_4Started));
			InputDefaultActions hotBar_11 = _inputService.HotBar_4;
			hotBar_11.Performed = (Action)Delegate.Remove(hotBar_11.Performed, new Action(SetHotBar_4Performed));
			InputDefaultActions hotBar_12 = _inputService.HotBar_4;
			hotBar_12.Canceled = (Action)Delegate.Remove(hotBar_12.Canceled, new Action(SetHotBar_4Canceled));
			InputDefaultActions hotBar_13 = _inputService.HotBar_5;
			hotBar_13.Started = (Action)Delegate.Remove(hotBar_13.Started, new Action(SetHotBar_5Started));
			InputDefaultActions hotBar_14 = _inputService.HotBar_5;
			hotBar_14.Performed = (Action)Delegate.Remove(hotBar_14.Performed, new Action(SetHotBar_5Performed));
			InputDefaultActions hotBar_15 = _inputService.HotBar_5;
			hotBar_15.Canceled = (Action)Delegate.Remove(hotBar_15.Canceled, new Action(SetHotBar_5Canceled));
			InputDefaultActions hotBar_16 = _inputService.HotBar_6;
			hotBar_16.Started = (Action)Delegate.Remove(hotBar_16.Started, new Action(SetHotBar_6Started));
			InputDefaultActions hotBar_17 = _inputService.HotBar_6;
			hotBar_17.Performed = (Action)Delegate.Remove(hotBar_17.Performed, new Action(SetHotBar_6Performed));
			InputDefaultActions hotBar_18 = _inputService.HotBar_6;
			hotBar_18.Canceled = (Action)Delegate.Remove(hotBar_18.Canceled, new Action(SetHotBar_6Canceled));
			InputDefaultActions hotBar_19 = _inputService.HotBar_7;
			hotBar_19.Started = (Action)Delegate.Remove(hotBar_19.Started, new Action(SetHotBar_7Started));
			InputDefaultActions hotBar_20 = _inputService.HotBar_7;
			hotBar_20.Performed = (Action)Delegate.Remove(hotBar_20.Performed, new Action(SetHotBar_7Performed));
			InputDefaultActions hotBar_21 = _inputService.HotBar_7;
			hotBar_21.Canceled = (Action)Delegate.Remove(hotBar_21.Canceled, new Action(SetHotBar_7Canceled));
			InputDefaultActions hotBar_22 = _inputService.HotBar_8;
			hotBar_22.Started = (Action)Delegate.Remove(hotBar_22.Started, new Action(SetHotBar_8Started));
			InputDefaultActions hotBar_23 = _inputService.HotBar_8;
			hotBar_23.Performed = (Action)Delegate.Remove(hotBar_23.Performed, new Action(SetHotBar_8Performed));
			InputDefaultActions hotBar_24 = _inputService.HotBar_8;
			hotBar_24.Canceled = (Action)Delegate.Remove(hotBar_24.Canceled, new Action(SetHotBar_8Canceled));
			InputDefaultActions hotBar_25 = _inputService.HotBar_9;
			hotBar_25.Started = (Action)Delegate.Remove(hotBar_25.Started, new Action(SetHotBar_9Started));
			InputDefaultActions hotBar_26 = _inputService.HotBar_9;
			hotBar_26.Performed = (Action)Delegate.Remove(hotBar_26.Performed, new Action(SetHotBar_9Performed));
			InputDefaultActions hotBar_27 = _inputService.HotBar_9;
			hotBar_27.Canceled = (Action)Delegate.Remove(hotBar_27.Canceled, new Action(SetHotBar_9Canceled));
			InputDefaultActions hotBar_28 = _inputService.HotBar_0;
			hotBar_28.Started = (Action)Delegate.Remove(hotBar_28.Started, new Action(SetHotBar_0Started));
			InputDefaultActions hotBar_29 = _inputService.HotBar_0;
			hotBar_29.Performed = (Action)Delegate.Remove(hotBar_29.Performed, new Action(SetHotBar_0Performed));
			InputDefaultActions hotBar_30 = _inputService.HotBar_0;
			hotBar_30.Canceled = (Action)Delegate.Remove(hotBar_30.Canceled, new Action(SetHotBar_0Canceled));
			InputDefaultActions turnOnOffOverlay = _inputService.TurnOnOffOverlay;
			turnOnOffOverlay.Started = (Action)Delegate.Remove(turnOnOffOverlay.Started, new Action(SetTurnOnOffOverlayStarted));
			InputDefaultActions turnOnOffOverlay2 = _inputService.TurnOnOffOverlay;
			turnOnOffOverlay2.Performed = (Action)Delegate.Remove(turnOnOffOverlay2.Performed, new Action(SetTurnOnOffOverlayPerformed));
			InputDefaultActions turnOnOffOverlay3 = _inputService.TurnOnOffOverlay;
			turnOnOffOverlay3.Canceled = (Action)Delegate.Remove(turnOnOffOverlay3.Canceled, new Action(SetTurnOnOffOverlayCanceled));
			InputVector2Actions anyVectorChange = _inputService.AnyVectorChange;
			anyVectorChange.VectorChangedPerformed = (Action<Vector2>)Delegate.Remove(anyVectorChange.VectorChangedPerformed, new Action<Vector2>(SetAnyVectorChange));
			InputVector2Actions anyVectorChange2 = _inputService.AnyVectorChange;
			anyVectorChange2.VectorChangedCanceled = (Action<Vector2>)Delegate.Remove(anyVectorChange2.VectorChangedCanceled, new Action<Vector2>(SetAnyVectorChange));
			InputDefaultActions uIApply = _inputService.UIApply;
			uIApply.Started = (Action)Delegate.Remove(uIApply.Started, new Action(SetUIApplyStarted));
			InputDefaultActions uIApply2 = _inputService.UIApply;
			uIApply2.Performed = (Action)Delegate.Remove(uIApply2.Performed, new Action(SetUIApplyPerformed));
			InputDefaultActions uIApply3 = _inputService.UIApply;
			uIApply3.Canceled = (Action)Delegate.Remove(uIApply3.Canceled, new Action(SetUIApplyCanceled));
			InputDefaultActions uIApplyWindow = _inputService.UIApplyWindow;
			uIApplyWindow.Started = (Action)Delegate.Remove(uIApplyWindow.Started, new Action(SetUIApplyWindowStarted));
			InputDefaultActions uIApplyWindow2 = _inputService.UIApplyWindow;
			uIApplyWindow2.Performed = (Action)Delegate.Remove(uIApplyWindow2.Performed, new Action(SetUIApplyWindowPerformed));
			InputDefaultActions uIApplyWindow3 = _inputService.UIApplyWindow;
			uIApplyWindow3.Canceled = (Action)Delegate.Remove(uIApplyWindow3.Canceled, new Action(SetUIApplyWindowCanceled));
			InputDefaultActions uIBack = _inputService.UIBack;
			uIBack.Started = (Action)Delegate.Remove(uIBack.Started, new Action(SetUIBackStarted));
			InputDefaultActions uIBack2 = _inputService.UIBack;
			uIBack2.Performed = (Action)Delegate.Remove(uIBack2.Performed, new Action(SetUIBackPerformed));
			InputDefaultActions uIBack3 = _inputService.UIBack;
			uIBack3.Canceled = (Action)Delegate.Remove(uIBack3.Canceled, new Action(SetUIBackCanceled));
			InputVector2Actions navigate = _inputService.Navigate;
			navigate.VectorChangedPerformed = (Action<Vector2>)Delegate.Remove(navigate.VectorChangedPerformed, new Action<Vector2>(SetNavigate));
			InputVector2Actions navigate2 = _inputService.Navigate;
			navigate2.VectorChangedCanceled = (Action<Vector2>)Delegate.Remove(navigate2.VectorChangedCanceled, new Action<Vector2>(SetNavigate));
			InputDefaultActions submit = _inputService.Submit;
			submit.Started = (Action)Delegate.Remove(submit.Started, new Action(SetSubmitStarted));
			InputDefaultActions submit2 = _inputService.Submit;
			submit2.Performed = (Action)Delegate.Remove(submit2.Performed, new Action(SetSubmitPerformed));
			InputDefaultActions submit3 = _inputService.Submit;
			submit3.Canceled = (Action)Delegate.Remove(submit3.Canceled, new Action(SetSubmitCanceled));
			InputDefaultActions cancel = _inputService.Cancel;
			cancel.Started = (Action)Delegate.Remove(cancel.Started, new Action(SetCancelStarted));
			InputDefaultActions cancel2 = _inputService.Cancel;
			cancel2.Performed = (Action)Delegate.Remove(cancel2.Performed, new Action(SetCancelPerformed));
			InputDefaultActions cancel3 = _inputService.Cancel;
			cancel3.Canceled = (Action)Delegate.Remove(cancel3.Canceled, new Action(SetCancelCanceled));
			InputVector2Actions point = _inputService.Point;
			point.VectorChangedPerformed = (Action<Vector2>)Delegate.Remove(point.VectorChangedPerformed, new Action<Vector2>(SetPoint));
			InputVector2Actions point2 = _inputService.Point;
			point2.VectorChangedCanceled = (Action<Vector2>)Delegate.Remove(point2.VectorChangedCanceled, new Action<Vector2>(SetPoint));
			InputDefaultActions click = _inputService.Click;
			click.Started = (Action)Delegate.Remove(click.Started, new Action(SetClickStarted));
			InputDefaultActions click2 = _inputService.Click;
			click2.Performed = (Action)Delegate.Remove(click2.Performed, new Action(SetClickPerformed));
			InputDefaultActions click3 = _inputService.Click;
			click3.Canceled = (Action)Delegate.Remove(click3.Canceled, new Action(SetClickCanceled));
			InputVector2Actions scrollWheel = _inputService.ScrollWheel;
			scrollWheel.VectorChangedPerformed = (Action<Vector2>)Delegate.Remove(scrollWheel.VectorChangedPerformed, new Action<Vector2>(SetScrollWheel));
			InputVector2Actions scrollWheel2 = _inputService.ScrollWheel;
			scrollWheel2.VectorChangedCanceled = (Action<Vector2>)Delegate.Remove(scrollWheel2.VectorChangedCanceled, new Action<Vector2>(SetScrollWheel));
			InputDefaultActions middleClick = _inputService.MiddleClick;
			middleClick.Started = (Action)Delegate.Remove(middleClick.Started, new Action(SetMiddleClickStarted));
			InputDefaultActions middleClick2 = _inputService.MiddleClick;
			middleClick2.Performed = (Action)Delegate.Remove(middleClick2.Performed, new Action(SetMiddleClickPerformed));
			InputDefaultActions middleClick3 = _inputService.MiddleClick;
			middleClick3.Canceled = (Action)Delegate.Remove(middleClick3.Canceled, new Action(SetMiddleClickCanceled));
			InputDefaultActions rightClick = _inputService.RightClick;
			rightClick.Started = (Action)Delegate.Remove(rightClick.Started, new Action(SetRightClickStarted));
			InputDefaultActions rightClick2 = _inputService.RightClick;
			rightClick2.Performed = (Action)Delegate.Remove(rightClick2.Performed, new Action(SetRightClickPerformed));
			InputDefaultActions rightClick3 = _inputService.RightClick;
			rightClick3.Canceled = (Action)Delegate.Remove(rightClick3.Canceled, new Action(SetRightClickCanceled));
			InputDefaultActions trackedDevicePosition = _inputService.TrackedDevicePosition;
			trackedDevicePosition.Started = (Action)Delegate.Remove(trackedDevicePosition.Started, new Action(SetTrackedDevicePositionStarted));
			InputDefaultActions trackedDevicePosition2 = _inputService.TrackedDevicePosition;
			trackedDevicePosition2.Performed = (Action)Delegate.Remove(trackedDevicePosition2.Performed, new Action(SetTrackedDevicePositionPerformed));
			InputDefaultActions trackedDevicePosition3 = _inputService.TrackedDevicePosition;
			trackedDevicePosition3.Canceled = (Action)Delegate.Remove(trackedDevicePosition3.Canceled, new Action(SetTrackedDevicePositionCanceled));
			InputDefaultActions trackedDeviceOrientation = _inputService.TrackedDeviceOrientation;
			trackedDeviceOrientation.Started = (Action)Delegate.Remove(trackedDeviceOrientation.Started, new Action(SetTrackedDeviceOrientationStarted));
			InputDefaultActions trackedDeviceOrientation2 = _inputService.TrackedDeviceOrientation;
			trackedDeviceOrientation2.Performed = (Action)Delegate.Remove(trackedDeviceOrientation2.Performed, new Action(SetTrackedDeviceOrientationPerformed));
			InputDefaultActions trackedDeviceOrientation3 = _inputService.TrackedDeviceOrientation;
			trackedDeviceOrientation3.Canceled = (Action)Delegate.Remove(trackedDeviceOrientation3.Canceled, new Action(SetTrackedDeviceOrientationCanceled));
			InputDefaultActions upgradesBack = _inputService.UpgradesBack;
			upgradesBack.Started = (Action)Delegate.Remove(upgradesBack.Started, new Action(SetUpgradesBackStarted));
			InputDefaultActions upgradesBack2 = _inputService.UpgradesBack;
			upgradesBack2.Performed = (Action)Delegate.Remove(upgradesBack2.Performed, new Action(SetUpgradesBackPerformed));
			InputDefaultActions upgradesBack3 = _inputService.UpgradesBack;
			upgradesBack3.Canceled = (Action)Delegate.Remove(upgradesBack3.Canceled, new Action(SetUpgradesBackCanceled));
			InputDefaultActions additionalNavigationLeft = _inputService.AdditionalNavigationLeft;
			additionalNavigationLeft.Started = (Action)Delegate.Remove(additionalNavigationLeft.Started, new Action(SetAdditionalNavigationLeftStarted));
			InputDefaultActions additionalNavigationLeft2 = _inputService.AdditionalNavigationLeft;
			additionalNavigationLeft2.Performed = (Action)Delegate.Remove(additionalNavigationLeft2.Performed, new Action(SetAdditionalNavigationLeftPerformed));
			InputDefaultActions additionalNavigationLeft3 = _inputService.AdditionalNavigationLeft;
			additionalNavigationLeft3.Canceled = (Action)Delegate.Remove(additionalNavigationLeft3.Canceled, new Action(SetAdditionalNavigationLeftCanceled));
			InputDefaultActions additionalNavigationRight = _inputService.AdditionalNavigationRight;
			additionalNavigationRight.Started = (Action)Delegate.Remove(additionalNavigationRight.Started, new Action(SetAdditionalNavigationRightStarted));
			InputDefaultActions additionalNavigationRight2 = _inputService.AdditionalNavigationRight;
			additionalNavigationRight2.Performed = (Action)Delegate.Remove(additionalNavigationRight2.Performed, new Action(SetAdditionalNavigationRightPerformed));
			InputDefaultActions additionalNavigationRight3 = _inputService.AdditionalNavigationRight;
			additionalNavigationRight3.Canceled = (Action)Delegate.Remove(additionalNavigationRight3.Canceled, new Action(SetAdditionalNavigationRightCanceled));
			InputDefaultActions extraAdditionalNavigationLeft = _inputService.ExtraAdditionalNavigationLeft;
			extraAdditionalNavigationLeft.Started = (Action)Delegate.Remove(extraAdditionalNavigationLeft.Started, new Action(SetExtraAdditionalNavigationLeftStarted));
			InputDefaultActions extraAdditionalNavigationLeft2 = _inputService.ExtraAdditionalNavigationLeft;
			extraAdditionalNavigationLeft2.Performed = (Action)Delegate.Remove(extraAdditionalNavigationLeft2.Performed, new Action(SetExtraAdditionalNavigationLeftPerformed));
			InputDefaultActions extraAdditionalNavigationLeft3 = _inputService.ExtraAdditionalNavigationLeft;
			extraAdditionalNavigationLeft3.Canceled = (Action)Delegate.Remove(extraAdditionalNavigationLeft3.Canceled, new Action(SetExtraAdditionalNavigationLeftCanceled));
			InputDefaultActions extraAdditionalNavigationRight = _inputService.ExtraAdditionalNavigationRight;
			extraAdditionalNavigationRight.Started = (Action)Delegate.Remove(extraAdditionalNavigationRight.Started, new Action(SetExtraAdditionalNavigationRightStarted));
			InputDefaultActions extraAdditionalNavigationRight2 = _inputService.ExtraAdditionalNavigationRight;
			extraAdditionalNavigationRight2.Performed = (Action)Delegate.Remove(extraAdditionalNavigationRight2.Performed, new Action(SetExtraAdditionalNavigationRightPerformed));
			InputDefaultActions extraAdditionalNavigationRight3 = _inputService.ExtraAdditionalNavigationRight;
			extraAdditionalNavigationRight3.Canceled = (Action)Delegate.Remove(extraAdditionalNavigationRight3.Canceled, new Action(SetExtraAdditionalNavigationRightCanceled));
			InputDefaultActions holdApply = _inputService.HoldApply;
			holdApply.Started = (Action)Delegate.Remove(holdApply.Started, new Action(SetHoldApplyStarted));
			InputDefaultActions holdApply2 = _inputService.HoldApply;
			holdApply2.Performed = (Action)Delegate.Remove(holdApply2.Performed, new Action(SetHoldApplyPerformed));
			InputDefaultActions holdApply3 = _inputService.HoldApply;
			holdApply3.Canceled = (Action)Delegate.Remove(holdApply3.Canceled, new Action(SetHoldApplyCanceled));
			InputDefaultActions pushToTalk = _inputService.PushToTalk;
			pushToTalk.Started = (Action)Delegate.Remove(pushToTalk.Started, new Action(SetPushToTalkStarted));
			InputDefaultActions pushToTalk2 = _inputService.PushToTalk;
			pushToTalk2.Performed = (Action)Delegate.Remove(pushToTalk2.Performed, new Action(SetPushToTalkPerformed));
			InputDefaultActions pushToTalk3 = _inputService.PushToTalk;
			pushToTalk3.Canceled = (Action)Delegate.Remove(pushToTalk3.Canceled, new Action(SetPushToTalkCanceled));
			InputDefaultActions gameplayApply = _inputService.GameplayApply;
			gameplayApply.Started = (Action)Delegate.Remove(gameplayApply.Started, new Action(SetGameplayApplyStarted));
			InputDefaultActions gameplayApply2 = _inputService.GameplayApply;
			gameplayApply2.Performed = (Action)Delegate.Remove(gameplayApply2.Performed, new Action(SetGameplayApplyPerformed));
			InputDefaultActions gameplayApply3 = _inputService.GameplayApply;
			gameplayApply3.Canceled = (Action)Delegate.Remove(gameplayApply3.Canceled, new Action(SetGameplayApplyCanceled));
		}

		private void OnInputEvent(OnInputEvent onInputEvent)
		{
			OnInput(onInputEvent.Runner, onInputEvent.Input);
		}

		public void OnInput(NetworkRunner _, NetworkInput input)
		{
			input.Set(_networkInputActions);
			_networkInputActions.SpectatorLeftPhase = default(NetworkButtons);
			_networkInputActions.SpectatorRightPhase = default(NetworkButtons);
			_networkInputActions.OpenSettingsPhase = default(NetworkButtons);
			_networkInputActions.JumpPhase = default(NetworkButtons);
			_networkInputActions.ChangeMouseVisibilityPhase = default(NetworkButtons);
			_networkInputActions.CrouchPhase = default(NetworkButtons);
			_networkInputActions.SprintPhase = default(NetworkButtons);
			_networkInputActions.GrabItemPhase = default(NetworkButtons);
			_networkInputActions.DropAllFromArmsPhase = default(NetworkButtons);
			_networkInputActions.ItemInteractPhase = default(NetworkButtons);
			_networkInputActions.MouseForwardPhase = default(NetworkButtons);
			_networkInputActions.BodyEmotePhase = default(NetworkButtons);
			_networkInputActions.FaceEmotePhase = default(NetworkButtons);
			_networkInputActions.HandEmotePhase = default(NetworkButtons);
			_networkInputActions.HotBar_1Phase = default(NetworkButtons);
			_networkInputActions.HotBar_2Phase = default(NetworkButtons);
			_networkInputActions.HotBar_3Phase = default(NetworkButtons);
			_networkInputActions.HotBar_4Phase = default(NetworkButtons);
			_networkInputActions.HotBar_5Phase = default(NetworkButtons);
			_networkInputActions.HotBar_6Phase = default(NetworkButtons);
			_networkInputActions.HotBar_7Phase = default(NetworkButtons);
			_networkInputActions.HotBar_8Phase = default(NetworkButtons);
			_networkInputActions.HotBar_9Phase = default(NetworkButtons);
			_networkInputActions.HotBar_0Phase = default(NetworkButtons);
			_networkInputActions.TurnOnOffOverlayPhase = default(NetworkButtons);
			_networkInputActions.UIApplyPhase = default(NetworkButtons);
			_networkInputActions.UIApplyWindowPhase = default(NetworkButtons);
			_networkInputActions.UIBackPhase = default(NetworkButtons);
			_networkInputActions.SubmitPhase = default(NetworkButtons);
			_networkInputActions.CancelPhase = default(NetworkButtons);
			_networkInputActions.ClickPhase = default(NetworkButtons);
			_networkInputActions.MiddleClickPhase = default(NetworkButtons);
			_networkInputActions.RightClickPhase = default(NetworkButtons);
			_networkInputActions.TrackedDevicePositionPhase = default(NetworkButtons);
			_networkInputActions.TrackedDeviceOrientationPhase = default(NetworkButtons);
			_networkInputActions.UpgradesBackPhase = default(NetworkButtons);
			_networkInputActions.AdditionalNavigationLeftPhase = default(NetworkButtons);
			_networkInputActions.AdditionalNavigationRightPhase = default(NetworkButtons);
			_networkInputActions.ExtraAdditionalNavigationLeftPhase = default(NetworkButtons);
			_networkInputActions.ExtraAdditionalNavigationRightPhase = default(NetworkButtons);
			_networkInputActions.HoldApplyPhase = default(NetworkButtons);
			_networkInputActions.PushToTalkPhase = default(NetworkButtons);
			_networkInputActions.GameplayApplyPhase = default(NetworkButtons);
		}

		private void SetSpectatorLeftStarted()
		{
			_networkInputActions.SpectatorLeftPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetSpectatorLeftPerformed()
		{
			_networkInputActions.SpectatorLeftPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetSpectatorLeftCanceled()
		{
			_networkInputActions.SpectatorLeftPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetSpectatorRightStarted()
		{
			_networkInputActions.SpectatorRightPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetSpectatorRightPerformed()
		{
			_networkInputActions.SpectatorRightPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetSpectatorRightCanceled()
		{
			_networkInputActions.SpectatorRightPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetOpenSettingsStarted()
		{
			_networkInputActions.OpenSettingsPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetOpenSettingsPerformed()
		{
			_networkInputActions.OpenSettingsPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetOpenSettingsCanceled()
		{
			_networkInputActions.OpenSettingsPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetMovement(Vector2 input)
		{
			_networkInputActions.MovementInput = input;
		}

		private void SetJumpStarted()
		{
			_networkInputActions.JumpPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetJumpPerformed()
		{
			_networkInputActions.JumpPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetJumpCanceled()
		{
			_networkInputActions.JumpPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetRotation(Vector2 input)
		{
			_networkInputActions.RotationInput = input;
		}

		private void SetChangeMouseVisibilityStarted()
		{
			_networkInputActions.ChangeMouseVisibilityPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetChangeMouseVisibilityPerformed()
		{
			_networkInputActions.ChangeMouseVisibilityPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetChangeMouseVisibilityCanceled()
		{
			_networkInputActions.ChangeMouseVisibilityPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetCrouchStarted()
		{
			_networkInputActions.CrouchPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetCrouchPerformed()
		{
			_networkInputActions.CrouchPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetCrouchCanceled()
		{
			_networkInputActions.CrouchPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetSprintStarted()
		{
			_networkInputActions.SprintPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetSprintPerformed()
		{
			_networkInputActions.SprintPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetSprintCanceled()
		{
			_networkInputActions.SprintPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetGrabItemStarted()
		{
			_networkInputActions.GrabItemPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetGrabItemPerformed()
		{
			_networkInputActions.GrabItemPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetGrabItemCanceled()
		{
			_networkInputActions.GrabItemPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetDropAllFromArmsStarted()
		{
			_networkInputActions.DropAllFromArmsPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetDropAllFromArmsPerformed()
		{
			_networkInputActions.DropAllFromArmsPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetDropAllFromArmsCanceled()
		{
			_networkInputActions.DropAllFromArmsPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetArmItemDistanceChange(Vector2 input)
		{
			_networkInputActions.ArmItemDistanceChangeInput = input;
		}

		private void SetItemInteractStarted()
		{
			_networkInputActions.ItemInteractPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetItemInteractPerformed()
		{
			_networkInputActions.ItemInteractPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetItemInteractCanceled()
		{
			_networkInputActions.ItemInteractPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetMouseForwardStarted()
		{
			_networkInputActions.MouseForwardPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetMouseForwardPerformed()
		{
			_networkInputActions.MouseForwardPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetMouseForwardCanceled()
		{
			_networkInputActions.MouseForwardPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetBodyEmoteStarted()
		{
			_networkInputActions.BodyEmotePhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetBodyEmotePerformed()
		{
			_networkInputActions.BodyEmotePhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetBodyEmoteCanceled()
		{
			_networkInputActions.BodyEmotePhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetFaceEmoteStarted()
		{
			_networkInputActions.FaceEmotePhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetFaceEmotePerformed()
		{
			_networkInputActions.FaceEmotePhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetFaceEmoteCanceled()
		{
			_networkInputActions.FaceEmotePhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetHandEmoteStarted()
		{
			_networkInputActions.HandEmotePhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetHandEmotePerformed()
		{
			_networkInputActions.HandEmotePhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetHandEmoteCanceled()
		{
			_networkInputActions.HandEmotePhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetEmoteNavigation(Vector2 input)
		{
			_networkInputActions.EmoteNavigationInput = input;
		}

		private void SetHotBar_1Started()
		{
			_networkInputActions.HotBar_1Phase.Set(InputActionPhase.Started, state: true);
		}

		private void SetHotBar_1Performed()
		{
			_networkInputActions.HotBar_1Phase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetHotBar_1Canceled()
		{
			_networkInputActions.HotBar_1Phase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetHotBar_2Started()
		{
			_networkInputActions.HotBar_2Phase.Set(InputActionPhase.Started, state: true);
		}

		private void SetHotBar_2Performed()
		{
			_networkInputActions.HotBar_2Phase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetHotBar_2Canceled()
		{
			_networkInputActions.HotBar_2Phase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetHotBar_3Started()
		{
			_networkInputActions.HotBar_3Phase.Set(InputActionPhase.Started, state: true);
		}

		private void SetHotBar_3Performed()
		{
			_networkInputActions.HotBar_3Phase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetHotBar_3Canceled()
		{
			_networkInputActions.HotBar_3Phase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetHotBar_4Started()
		{
			_networkInputActions.HotBar_4Phase.Set(InputActionPhase.Started, state: true);
		}

		private void SetHotBar_4Performed()
		{
			_networkInputActions.HotBar_4Phase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetHotBar_4Canceled()
		{
			_networkInputActions.HotBar_4Phase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetHotBar_5Started()
		{
			_networkInputActions.HotBar_5Phase.Set(InputActionPhase.Started, state: true);
		}

		private void SetHotBar_5Performed()
		{
			_networkInputActions.HotBar_5Phase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetHotBar_5Canceled()
		{
			_networkInputActions.HotBar_5Phase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetHotBar_6Started()
		{
			_networkInputActions.HotBar_6Phase.Set(InputActionPhase.Started, state: true);
		}

		private void SetHotBar_6Performed()
		{
			_networkInputActions.HotBar_6Phase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetHotBar_6Canceled()
		{
			_networkInputActions.HotBar_6Phase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetHotBar_7Started()
		{
			_networkInputActions.HotBar_7Phase.Set(InputActionPhase.Started, state: true);
		}

		private void SetHotBar_7Performed()
		{
			_networkInputActions.HotBar_7Phase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetHotBar_7Canceled()
		{
			_networkInputActions.HotBar_7Phase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetHotBar_8Started()
		{
			_networkInputActions.HotBar_8Phase.Set(InputActionPhase.Started, state: true);
		}

		private void SetHotBar_8Performed()
		{
			_networkInputActions.HotBar_8Phase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetHotBar_8Canceled()
		{
			_networkInputActions.HotBar_8Phase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetHotBar_9Started()
		{
			_networkInputActions.HotBar_9Phase.Set(InputActionPhase.Started, state: true);
		}

		private void SetHotBar_9Performed()
		{
			_networkInputActions.HotBar_9Phase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetHotBar_9Canceled()
		{
			_networkInputActions.HotBar_9Phase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetHotBar_0Started()
		{
			_networkInputActions.HotBar_0Phase.Set(InputActionPhase.Started, state: true);
		}

		private void SetHotBar_0Performed()
		{
			_networkInputActions.HotBar_0Phase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetHotBar_0Canceled()
		{
			_networkInputActions.HotBar_0Phase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetTurnOnOffOverlayStarted()
		{
			_networkInputActions.TurnOnOffOverlayPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetTurnOnOffOverlayPerformed()
		{
			_networkInputActions.TurnOnOffOverlayPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetTurnOnOffOverlayCanceled()
		{
			_networkInputActions.TurnOnOffOverlayPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetAnyVectorChange(Vector2 input)
		{
			_networkInputActions.AnyVectorChangeInput = input;
		}

		private void SetUIApplyStarted()
		{
			_networkInputActions.UIApplyPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetUIApplyPerformed()
		{
			_networkInputActions.UIApplyPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetUIApplyCanceled()
		{
			_networkInputActions.UIApplyPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetUIApplyWindowStarted()
		{
			_networkInputActions.UIApplyWindowPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetUIApplyWindowPerformed()
		{
			_networkInputActions.UIApplyWindowPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetUIApplyWindowCanceled()
		{
			_networkInputActions.UIApplyWindowPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetUIBackStarted()
		{
			_networkInputActions.UIBackPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetUIBackPerformed()
		{
			_networkInputActions.UIBackPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetUIBackCanceled()
		{
			_networkInputActions.UIBackPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetNavigate(Vector2 input)
		{
			_networkInputActions.NavigateInput = input;
		}

		private void SetSubmitStarted()
		{
			_networkInputActions.SubmitPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetSubmitPerformed()
		{
			_networkInputActions.SubmitPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetSubmitCanceled()
		{
			_networkInputActions.SubmitPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetCancelStarted()
		{
			_networkInputActions.CancelPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetCancelPerformed()
		{
			_networkInputActions.CancelPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetCancelCanceled()
		{
			_networkInputActions.CancelPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetPoint(Vector2 input)
		{
			_networkInputActions.PointInput = input;
		}

		private void SetClickStarted()
		{
			_networkInputActions.ClickPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetClickPerformed()
		{
			_networkInputActions.ClickPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetClickCanceled()
		{
			_networkInputActions.ClickPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetScrollWheel(Vector2 input)
		{
			_networkInputActions.ScrollWheelInput = input;
		}

		private void SetMiddleClickStarted()
		{
			_networkInputActions.MiddleClickPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetMiddleClickPerformed()
		{
			_networkInputActions.MiddleClickPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetMiddleClickCanceled()
		{
			_networkInputActions.MiddleClickPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetRightClickStarted()
		{
			_networkInputActions.RightClickPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetRightClickPerformed()
		{
			_networkInputActions.RightClickPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetRightClickCanceled()
		{
			_networkInputActions.RightClickPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetTrackedDevicePositionStarted()
		{
			_networkInputActions.TrackedDevicePositionPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetTrackedDevicePositionPerformed()
		{
			_networkInputActions.TrackedDevicePositionPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetTrackedDevicePositionCanceled()
		{
			_networkInputActions.TrackedDevicePositionPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetTrackedDeviceOrientationStarted()
		{
			_networkInputActions.TrackedDeviceOrientationPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetTrackedDeviceOrientationPerformed()
		{
			_networkInputActions.TrackedDeviceOrientationPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetTrackedDeviceOrientationCanceled()
		{
			_networkInputActions.TrackedDeviceOrientationPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetUpgradesBackStarted()
		{
			_networkInputActions.UpgradesBackPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetUpgradesBackPerformed()
		{
			_networkInputActions.UpgradesBackPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetUpgradesBackCanceled()
		{
			_networkInputActions.UpgradesBackPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetAdditionalNavigationLeftStarted()
		{
			_networkInputActions.AdditionalNavigationLeftPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetAdditionalNavigationLeftPerformed()
		{
			_networkInputActions.AdditionalNavigationLeftPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetAdditionalNavigationLeftCanceled()
		{
			_networkInputActions.AdditionalNavigationLeftPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetAdditionalNavigationRightStarted()
		{
			_networkInputActions.AdditionalNavigationRightPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetAdditionalNavigationRightPerformed()
		{
			_networkInputActions.AdditionalNavigationRightPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetAdditionalNavigationRightCanceled()
		{
			_networkInputActions.AdditionalNavigationRightPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetExtraAdditionalNavigationLeftStarted()
		{
			_networkInputActions.ExtraAdditionalNavigationLeftPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetExtraAdditionalNavigationLeftPerformed()
		{
			_networkInputActions.ExtraAdditionalNavigationLeftPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetExtraAdditionalNavigationLeftCanceled()
		{
			_networkInputActions.ExtraAdditionalNavigationLeftPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetExtraAdditionalNavigationRightStarted()
		{
			_networkInputActions.ExtraAdditionalNavigationRightPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetExtraAdditionalNavigationRightPerformed()
		{
			_networkInputActions.ExtraAdditionalNavigationRightPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetExtraAdditionalNavigationRightCanceled()
		{
			_networkInputActions.ExtraAdditionalNavigationRightPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetHoldApplyStarted()
		{
			_networkInputActions.HoldApplyPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetHoldApplyPerformed()
		{
			_networkInputActions.HoldApplyPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetHoldApplyCanceled()
		{
			_networkInputActions.HoldApplyPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetPushToTalkStarted()
		{
			_networkInputActions.PushToTalkPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetPushToTalkPerformed()
		{
			_networkInputActions.PushToTalkPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetPushToTalkCanceled()
		{
			_networkInputActions.PushToTalkPhase.Set(InputActionPhase.Canceled, state: true);
		}

		private void SetGameplayApplyStarted()
		{
			_networkInputActions.GameplayApplyPhase.Set(InputActionPhase.Started, state: true);
		}

		private void SetGameplayApplyPerformed()
		{
			_networkInputActions.GameplayApplyPhase.Set(InputActionPhase.Performed, state: true);
		}

		private void SetGameplayApplyCanceled()
		{
			_networkInputActions.GameplayApplyPhase.Set(InputActionPhase.Canceled, state: true);
		}
	}
}
