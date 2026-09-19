using System;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Features.InputModule.Scripts.Generated
{
	public class InputService : LocalInputActions.IUIActions, LocalInputActions.IMovementMapActions, LocalInputActions.IArmMapActions, LocalInputActions.IEmotionsActions, LocalInputActions.IHotBarActions, LocalInputActions.IDebugMapActions, LocalInputActions.IAnyVectorValueMapActions, LocalInputActions.IUIMapActions, LocalInputActions.IVoiceActions, LocalInputActions.IGameplayInteractionActions, IInputService, IInitializable, IDisposable
	{
		private LocalInputActions _localInputActions;

		public InputDefaultActions SpectatorLeft { get; set; } = new InputDefaultActions();

		public InputDefaultActions SpectatorRight { get; set; } = new InputDefaultActions();

		public InputDefaultActions OpenSettings { get; set; } = new InputDefaultActions();

		public InputVector2Actions Movement { get; set; } = new InputVector2Actions();

		public InputDefaultActions Jump { get; set; } = new InputDefaultActions();

		public InputVector2Actions Rotation { get; set; } = new InputVector2Actions();

		public InputDefaultActions ChangeMouseVisibility { get; set; } = new InputDefaultActions();

		public InputDefaultActions Crouch { get; set; } = new InputDefaultActions();

		public InputDefaultActions Sprint { get; set; } = new InputDefaultActions();

		public InputDefaultActions GrabItem { get; set; } = new InputDefaultActions();

		public InputDefaultActions DropAllFromArms { get; set; } = new InputDefaultActions();

		public InputVector2Actions ArmItemDistanceChange { get; set; } = new InputVector2Actions();

		public InputDefaultActions ItemInteract { get; set; } = new InputDefaultActions();

		public InputDefaultActions MouseForward { get; set; } = new InputDefaultActions();

		public InputDefaultActions MouseBack { get; set; } = new InputDefaultActions();

		public InputDefaultActions BodyEmote { get; set; } = new InputDefaultActions();

		public InputDefaultActions FaceEmote { get; set; } = new InputDefaultActions();

		public InputDefaultActions HandEmote { get; set; } = new InputDefaultActions();

		public InputVector2Actions EmoteNavigation { get; set; } = new InputVector2Actions();

		public InputDefaultActions HotBar_1 { get; set; } = new InputDefaultActions();

		public InputDefaultActions HotBar_2 { get; set; } = new InputDefaultActions();

		public InputDefaultActions HotBar_3 { get; set; } = new InputDefaultActions();

		public InputDefaultActions HotBar_4 { get; set; } = new InputDefaultActions();

		public InputDefaultActions HotBar_5 { get; set; } = new InputDefaultActions();

		public InputDefaultActions HotBar_6 { get; set; } = new InputDefaultActions();

		public InputDefaultActions HotBar_7 { get; set; } = new InputDefaultActions();

		public InputDefaultActions HotBar_8 { get; set; } = new InputDefaultActions();

		public InputDefaultActions HotBar_9 { get; set; } = new InputDefaultActions();

		public InputDefaultActions HotBar_0 { get; set; } = new InputDefaultActions();

		public InputDefaultActions TurnOnOffOverlay { get; set; } = new InputDefaultActions();

		public InputVector2Actions AnyVectorChange { get; set; } = new InputVector2Actions();

		public InputDefaultActions UIApply { get; set; } = new InputDefaultActions();

		public InputDefaultActions UIApplyWindow { get; set; } = new InputDefaultActions();

		public InputDefaultActions UIBack { get; set; } = new InputDefaultActions();

		public InputVector2Actions Navigate { get; set; } = new InputVector2Actions();

		public InputDefaultActions Submit { get; set; } = new InputDefaultActions();

		public InputDefaultActions Cancel { get; set; } = new InputDefaultActions();

		public InputVector2Actions Point { get; set; } = new InputVector2Actions();

		public InputDefaultActions Click { get; set; } = new InputDefaultActions();

		public InputVector2Actions ScrollWheel { get; set; } = new InputVector2Actions();

		public InputDefaultActions MiddleClick { get; set; } = new InputDefaultActions();

		public InputDefaultActions RightClick { get; set; } = new InputDefaultActions();

		public InputDefaultActions TrackedDevicePosition { get; set; } = new InputDefaultActions();

		public InputDefaultActions TrackedDeviceOrientation { get; set; } = new InputDefaultActions();

		public InputDefaultActions UpgradesBack { get; set; } = new InputDefaultActions();

		public InputDefaultActions AdditionalNavigationLeft { get; set; } = new InputDefaultActions();

		public InputDefaultActions AdditionalNavigationRight { get; set; } = new InputDefaultActions();

		public InputDefaultActions ExtraAdditionalNavigationLeft { get; set; } = new InputDefaultActions();

		public InputDefaultActions ExtraAdditionalNavigationRight { get; set; } = new InputDefaultActions();

		public InputDefaultActions HoldApply { get; set; } = new InputDefaultActions();

		public InputDefaultActions PushToTalk { get; set; } = new InputDefaultActions();

		public InputDefaultActions GameplayApply { get; set; } = new InputDefaultActions();

		public InputService(LocalInputActions localInputActions)
		{
			_localInputActions = localInputActions;
		}

		public void Initialize()
		{
			Enable();
		}

		public void Dispose()
		{
			Disable();
		}

		public void Enable()
		{
			_localInputActions.Enable();
			SetControlsCallback();
		}

		public void Disable()
		{
			_localInputActions.Disable();
			RemoveControlsCallback();
		}

		private void SetControlsCallback()
		{
			_localInputActions.UI.SetCallbacks(this);
			_localInputActions.MovementMap.SetCallbacks(this);
			_localInputActions.ArmMap.SetCallbacks(this);
			_localInputActions.Emotions.SetCallbacks(this);
			_localInputActions.HotBar.SetCallbacks(this);
			_localInputActions.DebugMap.SetCallbacks(this);
			_localInputActions.AnyVectorValueMap.SetCallbacks(this);
			_localInputActions.UIMap.SetCallbacks(this);
			_localInputActions.Voice.SetCallbacks(this);
			_localInputActions.GameplayInteraction.SetCallbacks(this);
		}

		private void RemoveControlsCallback()
		{
			_localInputActions.UI.RemoveCallbacks(this);
			_localInputActions.MovementMap.RemoveCallbacks(this);
			_localInputActions.ArmMap.RemoveCallbacks(this);
			_localInputActions.Emotions.RemoveCallbacks(this);
			_localInputActions.HotBar.RemoveCallbacks(this);
			_localInputActions.DebugMap.RemoveCallbacks(this);
			_localInputActions.AnyVectorValueMap.RemoveCallbacks(this);
			_localInputActions.UIMap.RemoveCallbacks(this);
			_localInputActions.Voice.RemoveCallbacks(this);
			_localInputActions.GameplayInteraction.RemoveCallbacks(this);
		}

		public void EnableUI()
		{
			_localInputActions.UI.Enable();
			_localInputActions.UI.SetCallbacks(this);
		}

		public void DisableUI()
		{
			_localInputActions.UI.Disable();
			_localInputActions.UI.RemoveCallbacks(this);
		}

		public void EnableMovementMap()
		{
			_localInputActions.MovementMap.Enable();
			_localInputActions.MovementMap.SetCallbacks(this);
		}

		public void DisableMovementMap()
		{
			_localInputActions.MovementMap.Disable();
			_localInputActions.MovementMap.RemoveCallbacks(this);
		}

		public void EnableArmMap()
		{
			_localInputActions.ArmMap.Enable();
			_localInputActions.ArmMap.SetCallbacks(this);
		}

		public void DisableArmMap()
		{
			_localInputActions.ArmMap.Disable();
			_localInputActions.ArmMap.RemoveCallbacks(this);
		}

		public void EnableEmotions()
		{
			_localInputActions.Emotions.Enable();
			_localInputActions.Emotions.SetCallbacks(this);
		}

		public void DisableEmotions()
		{
			_localInputActions.Emotions.Disable();
			_localInputActions.Emotions.RemoveCallbacks(this);
		}

		public void EnableHotBar()
		{
			_localInputActions.HotBar.Enable();
			_localInputActions.HotBar.SetCallbacks(this);
		}

		public void DisableHotBar()
		{
			_localInputActions.HotBar.Disable();
			_localInputActions.HotBar.RemoveCallbacks(this);
		}

		public void EnableDebugMap()
		{
			_localInputActions.DebugMap.Enable();
			_localInputActions.DebugMap.SetCallbacks(this);
		}

		public void DisableDebugMap()
		{
			_localInputActions.DebugMap.Disable();
			_localInputActions.DebugMap.RemoveCallbacks(this);
		}

		public void EnableAnyVectorValueMap()
		{
			_localInputActions.AnyVectorValueMap.Enable();
			_localInputActions.AnyVectorValueMap.SetCallbacks(this);
		}

		public void DisableAnyVectorValueMap()
		{
			_localInputActions.AnyVectorValueMap.Disable();
			_localInputActions.AnyVectorValueMap.RemoveCallbacks(this);
		}

		public void EnableUIMap()
		{
			_localInputActions.UIMap.Enable();
			_localInputActions.UIMap.SetCallbacks(this);
		}

		public void DisableUIMap()
		{
			_localInputActions.UIMap.Disable();
			_localInputActions.UIMap.RemoveCallbacks(this);
		}

		public void EnableVoice()
		{
			_localInputActions.Voice.Enable();
			_localInputActions.Voice.SetCallbacks(this);
		}

		public void DisableVoice()
		{
			_localInputActions.Voice.Disable();
			_localInputActions.Voice.RemoveCallbacks(this);
		}

		public void EnableGameplayInteraction()
		{
			_localInputActions.GameplayInteraction.Enable();
			_localInputActions.GameplayInteraction.SetCallbacks(this);
		}

		public void DisableGameplayInteraction()
		{
			_localInputActions.GameplayInteraction.Disable();
			_localInputActions.GameplayInteraction.RemoveCallbacks(this);
		}

		public void OnSpectatorLeft(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				SpectatorLeft.Started?.Invoke();
			}
			if (context.performed)
			{
				SpectatorLeft.Performed?.Invoke();
			}
			if (context.canceled)
			{
				SpectatorLeft.Canceled?.Invoke();
			}
		}

		public void OnSpectatorRight(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				SpectatorRight.Started?.Invoke();
			}
			if (context.performed)
			{
				SpectatorRight.Performed?.Invoke();
			}
			if (context.canceled)
			{
				SpectatorRight.Canceled?.Invoke();
			}
		}

		public void OnOpenSettings(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				OpenSettings.Started?.Invoke();
			}
			if (context.performed)
			{
				OpenSettings.Performed?.Invoke();
			}
			if (context.canceled)
			{
				OpenSettings.Canceled?.Invoke();
			}
		}

		public void OnMovement(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				Movement.Started?.Invoke();
			}
			if (context.performed)
			{
				Movement.Performed?.Invoke();
			}
			if (context.canceled)
			{
				Movement.Canceled?.Invoke();
			}
			Vector2 vector = context.ReadValue<Vector2>();
			InputDevice device = context.control.device;
			if (context.started)
			{
				Movement.VectorChangedStarted?.Invoke(vector);
			}
			if (context.started)
			{
				Movement.VectorChangedWithDeviceCallbackStarted?.Invoke(device, vector);
			}
			if (context.performed)
			{
				Movement.VectorChangedPerformed?.Invoke(vector);
			}
			if (context.performed)
			{
				Movement.VectorChangedWithDeviceCallbackPerformed?.Invoke(device, vector);
			}
			if (context.canceled)
			{
				Movement.VectorChangedCanceled?.Invoke(vector);
			}
			if (context.canceled)
			{
				Movement.VectorChangedWithDeviceCallbackCanceled?.Invoke(device, vector);
			}
		}

		public void OnJump(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				Jump.Started?.Invoke();
			}
			if (context.performed)
			{
				Jump.Performed?.Invoke();
			}
			if (context.canceled)
			{
				Jump.Canceled?.Invoke();
			}
		}

		public void OnRotation(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				Rotation.Started?.Invoke();
			}
			if (context.performed)
			{
				Rotation.Performed?.Invoke();
			}
			if (context.canceled)
			{
				Rotation.Canceled?.Invoke();
			}
			Vector2 vector = context.ReadValue<Vector2>();
			InputDevice device = context.control.device;
			if (context.started)
			{
				Rotation.VectorChangedStarted?.Invoke(vector);
			}
			if (context.started)
			{
				Rotation.VectorChangedWithDeviceCallbackStarted?.Invoke(device, vector);
			}
			if (context.performed)
			{
				Rotation.VectorChangedPerformed?.Invoke(vector);
			}
			if (context.performed)
			{
				Rotation.VectorChangedWithDeviceCallbackPerformed?.Invoke(device, vector);
			}
			if (context.canceled)
			{
				Rotation.VectorChangedCanceled?.Invoke(vector);
			}
			if (context.canceled)
			{
				Rotation.VectorChangedWithDeviceCallbackCanceled?.Invoke(device, vector);
			}
		}

		public void OnChangeMouseVisibility(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				ChangeMouseVisibility.Started?.Invoke();
			}
			if (context.performed)
			{
				ChangeMouseVisibility.Performed?.Invoke();
			}
			if (context.canceled)
			{
				ChangeMouseVisibility.Canceled?.Invoke();
			}
		}

		public void OnCrouch(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				Crouch.Started?.Invoke();
			}
			if (context.performed)
			{
				Crouch.Performed?.Invoke();
			}
			if (context.canceled)
			{
				Crouch.Canceled?.Invoke();
			}
		}

		public void OnSprint(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				Sprint.Started?.Invoke();
			}
			if (context.performed)
			{
				Sprint.Performed?.Invoke();
			}
			if (context.canceled)
			{
				Sprint.Canceled?.Invoke();
			}
		}

		public void OnGrabItem(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				GrabItem.Started?.Invoke();
			}
			if (context.performed)
			{
				GrabItem.Performed?.Invoke();
			}
			if (context.canceled)
			{
				GrabItem.Canceled?.Invoke();
			}
		}

		public void OnDropAllFromArms(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				DropAllFromArms.Started?.Invoke();
			}
			if (context.performed)
			{
				DropAllFromArms.Performed?.Invoke();
			}
			if (context.canceled)
			{
				DropAllFromArms.Canceled?.Invoke();
			}
		}

		public void OnArmItemDistanceChange(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				ArmItemDistanceChange.Started?.Invoke();
			}
			if (context.performed)
			{
				ArmItemDistanceChange.Performed?.Invoke();
			}
			if (context.canceled)
			{
				ArmItemDistanceChange.Canceled?.Invoke();
			}
			Vector2 vector = context.ReadValue<Vector2>();
			InputDevice device = context.control.device;
			if (context.started)
			{
				ArmItemDistanceChange.VectorChangedStarted?.Invoke(vector);
			}
			if (context.started)
			{
				ArmItemDistanceChange.VectorChangedWithDeviceCallbackStarted?.Invoke(device, vector);
			}
			if (context.performed)
			{
				ArmItemDistanceChange.VectorChangedPerformed?.Invoke(vector);
			}
			if (context.performed)
			{
				ArmItemDistanceChange.VectorChangedWithDeviceCallbackPerformed?.Invoke(device, vector);
			}
			if (context.canceled)
			{
				ArmItemDistanceChange.VectorChangedCanceled?.Invoke(vector);
			}
			if (context.canceled)
			{
				ArmItemDistanceChange.VectorChangedWithDeviceCallbackCanceled?.Invoke(device, vector);
			}
		}

		public void OnItemInteract(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				ItemInteract.Started?.Invoke();
			}
			if (context.performed)
			{
				ItemInteract.Performed?.Invoke();
			}
			if (context.canceled)
			{
				ItemInteract.Canceled?.Invoke();
			}
		}

		public void OnMouseForward(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				MouseForward.Started?.Invoke();
			}
			if (context.performed)
			{
				MouseForward.Performed?.Invoke();
			}
			if (context.canceled)
			{
				MouseForward.Canceled?.Invoke();
			}
		}

		public void OnMouseBack(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				MouseBack.Started?.Invoke();
			}
			if (context.performed)
			{
				MouseBack.Performed?.Invoke();
			}
			if (context.canceled)
			{
				MouseBack.Canceled?.Invoke();
			}
		}

		public void OnBodyEmote(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				BodyEmote.Started?.Invoke();
			}
			if (context.performed)
			{
				BodyEmote.Performed?.Invoke();
			}
			if (context.canceled)
			{
				BodyEmote.Canceled?.Invoke();
			}
		}

		public void OnFaceEmote(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				FaceEmote.Started?.Invoke();
			}
			if (context.performed)
			{
				FaceEmote.Performed?.Invoke();
			}
			if (context.canceled)
			{
				FaceEmote.Canceled?.Invoke();
			}
		}

		public void OnHandEmote(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				HandEmote.Started?.Invoke();
			}
			if (context.performed)
			{
				HandEmote.Performed?.Invoke();
			}
			if (context.canceled)
			{
				HandEmote.Canceled?.Invoke();
			}
		}

		public void OnEmoteNavigation(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				EmoteNavigation.Started?.Invoke();
			}
			if (context.performed)
			{
				EmoteNavigation.Performed?.Invoke();
			}
			if (context.canceled)
			{
				EmoteNavigation.Canceled?.Invoke();
			}
			Vector2 vector = context.ReadValue<Vector2>();
			InputDevice device = context.control.device;
			if (context.started)
			{
				EmoteNavigation.VectorChangedStarted?.Invoke(vector);
			}
			if (context.started)
			{
				EmoteNavigation.VectorChangedWithDeviceCallbackStarted?.Invoke(device, vector);
			}
			if (context.performed)
			{
				EmoteNavigation.VectorChangedPerformed?.Invoke(vector);
			}
			if (context.performed)
			{
				EmoteNavigation.VectorChangedWithDeviceCallbackPerformed?.Invoke(device, vector);
			}
			if (context.canceled)
			{
				EmoteNavigation.VectorChangedCanceled?.Invoke(vector);
			}
			if (context.canceled)
			{
				EmoteNavigation.VectorChangedWithDeviceCallbackCanceled?.Invoke(device, vector);
			}
		}

		public void OnHotBar_1(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				HotBar_1.Started?.Invoke();
			}
			if (context.performed)
			{
				HotBar_1.Performed?.Invoke();
			}
			if (context.canceled)
			{
				HotBar_1.Canceled?.Invoke();
			}
		}

		public void OnHotBar_2(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				HotBar_2.Started?.Invoke();
			}
			if (context.performed)
			{
				HotBar_2.Performed?.Invoke();
			}
			if (context.canceled)
			{
				HotBar_2.Canceled?.Invoke();
			}
		}

		public void OnHotBar_3(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				HotBar_3.Started?.Invoke();
			}
			if (context.performed)
			{
				HotBar_3.Performed?.Invoke();
			}
			if (context.canceled)
			{
				HotBar_3.Canceled?.Invoke();
			}
		}

		public void OnHotBar_4(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				HotBar_4.Started?.Invoke();
			}
			if (context.performed)
			{
				HotBar_4.Performed?.Invoke();
			}
			if (context.canceled)
			{
				HotBar_4.Canceled?.Invoke();
			}
		}

		public void OnHotBar_5(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				HotBar_5.Started?.Invoke();
			}
			if (context.performed)
			{
				HotBar_5.Performed?.Invoke();
			}
			if (context.canceled)
			{
				HotBar_5.Canceled?.Invoke();
			}
		}

		public void OnHotBar_6(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				HotBar_6.Started?.Invoke();
			}
			if (context.performed)
			{
				HotBar_6.Performed?.Invoke();
			}
			if (context.canceled)
			{
				HotBar_6.Canceled?.Invoke();
			}
		}

		public void OnHotBar_7(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				HotBar_7.Started?.Invoke();
			}
			if (context.performed)
			{
				HotBar_7.Performed?.Invoke();
			}
			if (context.canceled)
			{
				HotBar_7.Canceled?.Invoke();
			}
		}

		public void OnHotBar_8(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				HotBar_8.Started?.Invoke();
			}
			if (context.performed)
			{
				HotBar_8.Performed?.Invoke();
			}
			if (context.canceled)
			{
				HotBar_8.Canceled?.Invoke();
			}
		}

		public void OnHotBar_9(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				HotBar_9.Started?.Invoke();
			}
			if (context.performed)
			{
				HotBar_9.Performed?.Invoke();
			}
			if (context.canceled)
			{
				HotBar_9.Canceled?.Invoke();
			}
		}

		public void OnHotBar_0(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				HotBar_0.Started?.Invoke();
			}
			if (context.performed)
			{
				HotBar_0.Performed?.Invoke();
			}
			if (context.canceled)
			{
				HotBar_0.Canceled?.Invoke();
			}
		}

		public void OnTurnOnOffOverlay(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				TurnOnOffOverlay.Started?.Invoke();
			}
			if (context.performed)
			{
				TurnOnOffOverlay.Performed?.Invoke();
			}
			if (context.canceled)
			{
				TurnOnOffOverlay.Canceled?.Invoke();
			}
		}

		public void OnAnyVectorChange(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				AnyVectorChange.Started?.Invoke();
			}
			if (context.performed)
			{
				AnyVectorChange.Performed?.Invoke();
			}
			if (context.canceled)
			{
				AnyVectorChange.Canceled?.Invoke();
			}
			Vector2 vector = context.ReadValue<Vector2>();
			InputDevice device = context.control.device;
			if (context.started)
			{
				AnyVectorChange.VectorChangedStarted?.Invoke(vector);
			}
			if (context.started)
			{
				AnyVectorChange.VectorChangedWithDeviceCallbackStarted?.Invoke(device, vector);
			}
			if (context.performed)
			{
				AnyVectorChange.VectorChangedPerformed?.Invoke(vector);
			}
			if (context.performed)
			{
				AnyVectorChange.VectorChangedWithDeviceCallbackPerformed?.Invoke(device, vector);
			}
			if (context.canceled)
			{
				AnyVectorChange.VectorChangedCanceled?.Invoke(vector);
			}
			if (context.canceled)
			{
				AnyVectorChange.VectorChangedWithDeviceCallbackCanceled?.Invoke(device, vector);
			}
		}

		public void OnUIApply(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				UIApply.Started?.Invoke();
			}
			if (context.performed)
			{
				UIApply.Performed?.Invoke();
			}
			if (context.canceled)
			{
				UIApply.Canceled?.Invoke();
			}
		}

		public void OnUIApplyWindow(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				UIApplyWindow.Started?.Invoke();
			}
			if (context.performed)
			{
				UIApplyWindow.Performed?.Invoke();
			}
			if (context.canceled)
			{
				UIApplyWindow.Canceled?.Invoke();
			}
		}

		public void OnUIBack(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				UIBack.Started?.Invoke();
			}
			if (context.performed)
			{
				UIBack.Performed?.Invoke();
			}
			if (context.canceled)
			{
				UIBack.Canceled?.Invoke();
			}
		}

		public void OnNavigate(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				Navigate.Started?.Invoke();
			}
			if (context.performed)
			{
				Navigate.Performed?.Invoke();
			}
			if (context.canceled)
			{
				Navigate.Canceled?.Invoke();
			}
			Vector2 vector = context.ReadValue<Vector2>();
			InputDevice device = context.control.device;
			if (context.started)
			{
				Navigate.VectorChangedStarted?.Invoke(vector);
			}
			if (context.started)
			{
				Navigate.VectorChangedWithDeviceCallbackStarted?.Invoke(device, vector);
			}
			if (context.performed)
			{
				Navigate.VectorChangedPerformed?.Invoke(vector);
			}
			if (context.performed)
			{
				Navigate.VectorChangedWithDeviceCallbackPerformed?.Invoke(device, vector);
			}
			if (context.canceled)
			{
				Navigate.VectorChangedCanceled?.Invoke(vector);
			}
			if (context.canceled)
			{
				Navigate.VectorChangedWithDeviceCallbackCanceled?.Invoke(device, vector);
			}
		}

		public void OnSubmit(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				Submit.Started?.Invoke();
			}
			if (context.performed)
			{
				Submit.Performed?.Invoke();
			}
			if (context.canceled)
			{
				Submit.Canceled?.Invoke();
			}
		}

		public void OnCancel(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				Cancel.Started?.Invoke();
			}
			if (context.performed)
			{
				Cancel.Performed?.Invoke();
			}
			if (context.canceled)
			{
				Cancel.Canceled?.Invoke();
			}
		}

		public void OnPoint(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				Point.Started?.Invoke();
			}
			if (context.performed)
			{
				Point.Performed?.Invoke();
			}
			if (context.canceled)
			{
				Point.Canceled?.Invoke();
			}
			Vector2 vector = context.ReadValue<Vector2>();
			InputDevice device = context.control.device;
			if (context.started)
			{
				Point.VectorChangedStarted?.Invoke(vector);
			}
			if (context.started)
			{
				Point.VectorChangedWithDeviceCallbackStarted?.Invoke(device, vector);
			}
			if (context.performed)
			{
				Point.VectorChangedPerformed?.Invoke(vector);
			}
			if (context.performed)
			{
				Point.VectorChangedWithDeviceCallbackPerformed?.Invoke(device, vector);
			}
			if (context.canceled)
			{
				Point.VectorChangedCanceled?.Invoke(vector);
			}
			if (context.canceled)
			{
				Point.VectorChangedWithDeviceCallbackCanceled?.Invoke(device, vector);
			}
		}

		public void OnClick(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				Click.Started?.Invoke();
			}
			if (context.performed)
			{
				Click.Performed?.Invoke();
			}
			if (context.canceled)
			{
				Click.Canceled?.Invoke();
			}
		}

		public void OnScrollWheel(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				ScrollWheel.Started?.Invoke();
			}
			if (context.performed)
			{
				ScrollWheel.Performed?.Invoke();
			}
			if (context.canceled)
			{
				ScrollWheel.Canceled?.Invoke();
			}
			Vector2 vector = context.ReadValue<Vector2>();
			InputDevice device = context.control.device;
			if (context.started)
			{
				ScrollWheel.VectorChangedStarted?.Invoke(vector);
			}
			if (context.started)
			{
				ScrollWheel.VectorChangedWithDeviceCallbackStarted?.Invoke(device, vector);
			}
			if (context.performed)
			{
				ScrollWheel.VectorChangedPerformed?.Invoke(vector);
			}
			if (context.performed)
			{
				ScrollWheel.VectorChangedWithDeviceCallbackPerformed?.Invoke(device, vector);
			}
			if (context.canceled)
			{
				ScrollWheel.VectorChangedCanceled?.Invoke(vector);
			}
			if (context.canceled)
			{
				ScrollWheel.VectorChangedWithDeviceCallbackCanceled?.Invoke(device, vector);
			}
		}

		public void OnMiddleClick(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				MiddleClick.Started?.Invoke();
			}
			if (context.performed)
			{
				MiddleClick.Performed?.Invoke();
			}
			if (context.canceled)
			{
				MiddleClick.Canceled?.Invoke();
			}
		}

		public void OnRightClick(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				RightClick.Started?.Invoke();
			}
			if (context.performed)
			{
				RightClick.Performed?.Invoke();
			}
			if (context.canceled)
			{
				RightClick.Canceled?.Invoke();
			}
		}

		public void OnTrackedDevicePosition(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				TrackedDevicePosition.Started?.Invoke();
			}
			if (context.performed)
			{
				TrackedDevicePosition.Performed?.Invoke();
			}
			if (context.canceled)
			{
				TrackedDevicePosition.Canceled?.Invoke();
			}
		}

		public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				TrackedDeviceOrientation.Started?.Invoke();
			}
			if (context.performed)
			{
				TrackedDeviceOrientation.Performed?.Invoke();
			}
			if (context.canceled)
			{
				TrackedDeviceOrientation.Canceled?.Invoke();
			}
		}

		public void OnUpgradesBack(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				UpgradesBack.Started?.Invoke();
			}
			if (context.performed)
			{
				UpgradesBack.Performed?.Invoke();
			}
			if (context.canceled)
			{
				UpgradesBack.Canceled?.Invoke();
			}
		}

		public void OnAdditionalNavigationLeft(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				AdditionalNavigationLeft.Started?.Invoke();
			}
			if (context.performed)
			{
				AdditionalNavigationLeft.Performed?.Invoke();
			}
			if (context.canceled)
			{
				AdditionalNavigationLeft.Canceled?.Invoke();
			}
		}

		public void OnAdditionalNavigationRight(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				AdditionalNavigationRight.Started?.Invoke();
			}
			if (context.performed)
			{
				AdditionalNavigationRight.Performed?.Invoke();
			}
			if (context.canceled)
			{
				AdditionalNavigationRight.Canceled?.Invoke();
			}
		}

		public void OnExtraAdditionalNavigationLeft(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				ExtraAdditionalNavigationLeft.Started?.Invoke();
			}
			if (context.performed)
			{
				ExtraAdditionalNavigationLeft.Performed?.Invoke();
			}
			if (context.canceled)
			{
				ExtraAdditionalNavigationLeft.Canceled?.Invoke();
			}
		}

		public void OnExtraAdditionalNavigationRight(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				ExtraAdditionalNavigationRight.Started?.Invoke();
			}
			if (context.performed)
			{
				ExtraAdditionalNavigationRight.Performed?.Invoke();
			}
			if (context.canceled)
			{
				ExtraAdditionalNavigationRight.Canceled?.Invoke();
			}
		}

		public void OnHoldApply(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				HoldApply.Started?.Invoke();
			}
			if (context.performed)
			{
				HoldApply.Performed?.Invoke();
			}
			if (context.canceled)
			{
				HoldApply.Canceled?.Invoke();
			}
		}

		public void OnPushToTalk(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				PushToTalk.Started?.Invoke();
			}
			if (context.performed)
			{
				PushToTalk.Performed?.Invoke();
			}
			if (context.canceled)
			{
				PushToTalk.Canceled?.Invoke();
			}
		}

		public void OnGameplayApply(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				GameplayApply.Started?.Invoke();
			}
			if (context.performed)
			{
				GameplayApply.Performed?.Invoke();
			}
			if (context.canceled)
			{
				GameplayApply.Canceled?.Invoke();
			}
		}
	}
}
