using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class LocalInputActions : IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable, IDisposable
{
	public struct UIActions
	{
		private LocalInputActions m_Wrapper;

		public InputAction SpectatorLeft => m_Wrapper.m_UI_SpectatorLeft;

		public InputAction SpectatorRight => m_Wrapper.m_UI_SpectatorRight;

		public InputAction OpenSettings => m_Wrapper.m_UI_OpenSettings;

		public bool enabled => Get().enabled;

		public UIActions(LocalInputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_UI;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(UIActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IUIActions instance)
		{
			if (instance != null && !m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
				SpectatorLeft.started += instance.OnSpectatorLeft;
				SpectatorLeft.performed += instance.OnSpectatorLeft;
				SpectatorLeft.canceled += instance.OnSpectatorLeft;
				SpectatorRight.started += instance.OnSpectatorRight;
				SpectatorRight.performed += instance.OnSpectatorRight;
				SpectatorRight.canceled += instance.OnSpectatorRight;
				OpenSettings.started += instance.OnOpenSettings;
				OpenSettings.performed += instance.OnOpenSettings;
				OpenSettings.canceled += instance.OnOpenSettings;
			}
		}

		private void UnregisterCallbacks(IUIActions instance)
		{
			SpectatorLeft.started -= instance.OnSpectatorLeft;
			SpectatorLeft.performed -= instance.OnSpectatorLeft;
			SpectatorLeft.canceled -= instance.OnSpectatorLeft;
			SpectatorRight.started -= instance.OnSpectatorRight;
			SpectatorRight.performed -= instance.OnSpectatorRight;
			SpectatorRight.canceled -= instance.OnSpectatorRight;
			OpenSettings.started -= instance.OnOpenSettings;
			OpenSettings.performed -= instance.OnOpenSettings;
			OpenSettings.canceled -= instance.OnOpenSettings;
		}

		public void RemoveCallbacks(IUIActions instance)
		{
			if (m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IUIActions instance)
		{
			foreach (IUIActions uIActionsCallbackInterface in m_Wrapper.m_UIActionsCallbackInterfaces)
			{
				UnregisterCallbacks(uIActionsCallbackInterface);
			}
			m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public struct MovementMapActions
	{
		private LocalInputActions m_Wrapper;

		public InputAction Movement => m_Wrapper.m_MovementMap_Movement;

		public InputAction Jump => m_Wrapper.m_MovementMap_Jump;

		public InputAction Rotation => m_Wrapper.m_MovementMap_Rotation;

		public InputAction ChangeMouseVisibility => m_Wrapper.m_MovementMap_ChangeMouseVisibility;

		public InputAction Crouch => m_Wrapper.m_MovementMap_Crouch;

		public InputAction Sprint => m_Wrapper.m_MovementMap_Sprint;

		public bool enabled => Get().enabled;

		public MovementMapActions(LocalInputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_MovementMap;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(MovementMapActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IMovementMapActions instance)
		{
			if (instance != null && !m_Wrapper.m_MovementMapActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_MovementMapActionsCallbackInterfaces.Add(instance);
				Movement.started += instance.OnMovement;
				Movement.performed += instance.OnMovement;
				Movement.canceled += instance.OnMovement;
				Jump.started += instance.OnJump;
				Jump.performed += instance.OnJump;
				Jump.canceled += instance.OnJump;
				Rotation.started += instance.OnRotation;
				Rotation.performed += instance.OnRotation;
				Rotation.canceled += instance.OnRotation;
				ChangeMouseVisibility.started += instance.OnChangeMouseVisibility;
				ChangeMouseVisibility.performed += instance.OnChangeMouseVisibility;
				ChangeMouseVisibility.canceled += instance.OnChangeMouseVisibility;
				Crouch.started += instance.OnCrouch;
				Crouch.performed += instance.OnCrouch;
				Crouch.canceled += instance.OnCrouch;
				Sprint.started += instance.OnSprint;
				Sprint.performed += instance.OnSprint;
				Sprint.canceled += instance.OnSprint;
			}
		}

		private void UnregisterCallbacks(IMovementMapActions instance)
		{
			Movement.started -= instance.OnMovement;
			Movement.performed -= instance.OnMovement;
			Movement.canceled -= instance.OnMovement;
			Jump.started -= instance.OnJump;
			Jump.performed -= instance.OnJump;
			Jump.canceled -= instance.OnJump;
			Rotation.started -= instance.OnRotation;
			Rotation.performed -= instance.OnRotation;
			Rotation.canceled -= instance.OnRotation;
			ChangeMouseVisibility.started -= instance.OnChangeMouseVisibility;
			ChangeMouseVisibility.performed -= instance.OnChangeMouseVisibility;
			ChangeMouseVisibility.canceled -= instance.OnChangeMouseVisibility;
			Crouch.started -= instance.OnCrouch;
			Crouch.performed -= instance.OnCrouch;
			Crouch.canceled -= instance.OnCrouch;
			Sprint.started -= instance.OnSprint;
			Sprint.performed -= instance.OnSprint;
			Sprint.canceled -= instance.OnSprint;
		}

		public void RemoveCallbacks(IMovementMapActions instance)
		{
			if (m_Wrapper.m_MovementMapActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IMovementMapActions instance)
		{
			foreach (IMovementMapActions movementMapActionsCallbackInterface in m_Wrapper.m_MovementMapActionsCallbackInterfaces)
			{
				UnregisterCallbacks(movementMapActionsCallbackInterface);
			}
			m_Wrapper.m_MovementMapActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public struct ArmMapActions
	{
		private LocalInputActions m_Wrapper;

		public InputAction GrabItem => m_Wrapper.m_ArmMap_GrabItem;

		public InputAction DropAllFromArms => m_Wrapper.m_ArmMap_DropAllFromArms;

		public InputAction ArmItemDistanceChange => m_Wrapper.m_ArmMap_ArmItemDistanceChange;

		public InputAction ItemInteract => m_Wrapper.m_ArmMap_ItemInteract;

		public InputAction MouseForward => m_Wrapper.m_ArmMap_MouseForward;

		public InputAction MouseBack => m_Wrapper.m_ArmMap_MouseBack;

		public bool enabled => Get().enabled;

		public ArmMapActions(LocalInputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_ArmMap;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(ArmMapActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IArmMapActions instance)
		{
			if (instance != null && !m_Wrapper.m_ArmMapActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_ArmMapActionsCallbackInterfaces.Add(instance);
				GrabItem.started += instance.OnGrabItem;
				GrabItem.performed += instance.OnGrabItem;
				GrabItem.canceled += instance.OnGrabItem;
				DropAllFromArms.started += instance.OnDropAllFromArms;
				DropAllFromArms.performed += instance.OnDropAllFromArms;
				DropAllFromArms.canceled += instance.OnDropAllFromArms;
				ArmItemDistanceChange.started += instance.OnArmItemDistanceChange;
				ArmItemDistanceChange.performed += instance.OnArmItemDistanceChange;
				ArmItemDistanceChange.canceled += instance.OnArmItemDistanceChange;
				ItemInteract.started += instance.OnItemInteract;
				ItemInteract.performed += instance.OnItemInteract;
				ItemInteract.canceled += instance.OnItemInteract;
				MouseForward.started += instance.OnMouseForward;
				MouseForward.performed += instance.OnMouseForward;
				MouseForward.canceled += instance.OnMouseForward;
				MouseBack.started += instance.OnMouseBack;
				MouseBack.performed += instance.OnMouseBack;
				MouseBack.canceled += instance.OnMouseBack;
			}
		}

		private void UnregisterCallbacks(IArmMapActions instance)
		{
			GrabItem.started -= instance.OnGrabItem;
			GrabItem.performed -= instance.OnGrabItem;
			GrabItem.canceled -= instance.OnGrabItem;
			DropAllFromArms.started -= instance.OnDropAllFromArms;
			DropAllFromArms.performed -= instance.OnDropAllFromArms;
			DropAllFromArms.canceled -= instance.OnDropAllFromArms;
			ArmItemDistanceChange.started -= instance.OnArmItemDistanceChange;
			ArmItemDistanceChange.performed -= instance.OnArmItemDistanceChange;
			ArmItemDistanceChange.canceled -= instance.OnArmItemDistanceChange;
			ItemInteract.started -= instance.OnItemInteract;
			ItemInteract.performed -= instance.OnItemInteract;
			ItemInteract.canceled -= instance.OnItemInteract;
			MouseForward.started -= instance.OnMouseForward;
			MouseForward.performed -= instance.OnMouseForward;
			MouseForward.canceled -= instance.OnMouseForward;
			MouseBack.started -= instance.OnMouseBack;
			MouseBack.performed -= instance.OnMouseBack;
			MouseBack.canceled -= instance.OnMouseBack;
		}

		public void RemoveCallbacks(IArmMapActions instance)
		{
			if (m_Wrapper.m_ArmMapActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IArmMapActions instance)
		{
			foreach (IArmMapActions armMapActionsCallbackInterface in m_Wrapper.m_ArmMapActionsCallbackInterfaces)
			{
				UnregisterCallbacks(armMapActionsCallbackInterface);
			}
			m_Wrapper.m_ArmMapActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public struct EmotionsActions
	{
		private LocalInputActions m_Wrapper;

		public InputAction BodyEmote => m_Wrapper.m_Emotions_BodyEmote;

		public InputAction FaceEmote => m_Wrapper.m_Emotions_FaceEmote;

		public InputAction HandEmote => m_Wrapper.m_Emotions_HandEmote;

		public InputAction EmoteNavigation => m_Wrapper.m_Emotions_EmoteNavigation;

		public bool enabled => Get().enabled;

		public EmotionsActions(LocalInputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_Emotions;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(EmotionsActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IEmotionsActions instance)
		{
			if (instance != null && !m_Wrapper.m_EmotionsActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_EmotionsActionsCallbackInterfaces.Add(instance);
				BodyEmote.started += instance.OnBodyEmote;
				BodyEmote.performed += instance.OnBodyEmote;
				BodyEmote.canceled += instance.OnBodyEmote;
				FaceEmote.started += instance.OnFaceEmote;
				FaceEmote.performed += instance.OnFaceEmote;
				FaceEmote.canceled += instance.OnFaceEmote;
				HandEmote.started += instance.OnHandEmote;
				HandEmote.performed += instance.OnHandEmote;
				HandEmote.canceled += instance.OnHandEmote;
				EmoteNavigation.started += instance.OnEmoteNavigation;
				EmoteNavigation.performed += instance.OnEmoteNavigation;
				EmoteNavigation.canceled += instance.OnEmoteNavigation;
			}
		}

		private void UnregisterCallbacks(IEmotionsActions instance)
		{
			BodyEmote.started -= instance.OnBodyEmote;
			BodyEmote.performed -= instance.OnBodyEmote;
			BodyEmote.canceled -= instance.OnBodyEmote;
			FaceEmote.started -= instance.OnFaceEmote;
			FaceEmote.performed -= instance.OnFaceEmote;
			FaceEmote.canceled -= instance.OnFaceEmote;
			HandEmote.started -= instance.OnHandEmote;
			HandEmote.performed -= instance.OnHandEmote;
			HandEmote.canceled -= instance.OnHandEmote;
			EmoteNavigation.started -= instance.OnEmoteNavigation;
			EmoteNavigation.performed -= instance.OnEmoteNavigation;
			EmoteNavigation.canceled -= instance.OnEmoteNavigation;
		}

		public void RemoveCallbacks(IEmotionsActions instance)
		{
			if (m_Wrapper.m_EmotionsActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IEmotionsActions instance)
		{
			foreach (IEmotionsActions emotionsActionsCallbackInterface in m_Wrapper.m_EmotionsActionsCallbackInterfaces)
			{
				UnregisterCallbacks(emotionsActionsCallbackInterface);
			}
			m_Wrapper.m_EmotionsActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public struct HotBarActions
	{
		private LocalInputActions m_Wrapper;

		public InputAction HotBar_1 => m_Wrapper.m_HotBar_HotBar_1;

		public InputAction HotBar_2 => m_Wrapper.m_HotBar_HotBar_2;

		public InputAction HotBar_3 => m_Wrapper.m_HotBar_HotBar_3;

		public InputAction HotBar_4 => m_Wrapper.m_HotBar_HotBar_4;

		public InputAction HotBar_5 => m_Wrapper.m_HotBar_HotBar_5;

		public InputAction HotBar_6 => m_Wrapper.m_HotBar_HotBar_6;

		public InputAction HotBar_7 => m_Wrapper.m_HotBar_HotBar_7;

		public InputAction HotBar_8 => m_Wrapper.m_HotBar_HotBar_8;

		public InputAction HotBar_9 => m_Wrapper.m_HotBar_HotBar_9;

		public InputAction HotBar_0 => m_Wrapper.m_HotBar_HotBar_0;

		public bool enabled => Get().enabled;

		public HotBarActions(LocalInputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_HotBar;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(HotBarActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IHotBarActions instance)
		{
			if (instance != null && !m_Wrapper.m_HotBarActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_HotBarActionsCallbackInterfaces.Add(instance);
				HotBar_1.started += instance.OnHotBar_1;
				HotBar_1.performed += instance.OnHotBar_1;
				HotBar_1.canceled += instance.OnHotBar_1;
				HotBar_2.started += instance.OnHotBar_2;
				HotBar_2.performed += instance.OnHotBar_2;
				HotBar_2.canceled += instance.OnHotBar_2;
				HotBar_3.started += instance.OnHotBar_3;
				HotBar_3.performed += instance.OnHotBar_3;
				HotBar_3.canceled += instance.OnHotBar_3;
				HotBar_4.started += instance.OnHotBar_4;
				HotBar_4.performed += instance.OnHotBar_4;
				HotBar_4.canceled += instance.OnHotBar_4;
				HotBar_5.started += instance.OnHotBar_5;
				HotBar_5.performed += instance.OnHotBar_5;
				HotBar_5.canceled += instance.OnHotBar_5;
				HotBar_6.started += instance.OnHotBar_6;
				HotBar_6.performed += instance.OnHotBar_6;
				HotBar_6.canceled += instance.OnHotBar_6;
				HotBar_7.started += instance.OnHotBar_7;
				HotBar_7.performed += instance.OnHotBar_7;
				HotBar_7.canceled += instance.OnHotBar_7;
				HotBar_8.started += instance.OnHotBar_8;
				HotBar_8.performed += instance.OnHotBar_8;
				HotBar_8.canceled += instance.OnHotBar_8;
				HotBar_9.started += instance.OnHotBar_9;
				HotBar_9.performed += instance.OnHotBar_9;
				HotBar_9.canceled += instance.OnHotBar_9;
				HotBar_0.started += instance.OnHotBar_0;
				HotBar_0.performed += instance.OnHotBar_0;
				HotBar_0.canceled += instance.OnHotBar_0;
			}
		}

		private void UnregisterCallbacks(IHotBarActions instance)
		{
			HotBar_1.started -= instance.OnHotBar_1;
			HotBar_1.performed -= instance.OnHotBar_1;
			HotBar_1.canceled -= instance.OnHotBar_1;
			HotBar_2.started -= instance.OnHotBar_2;
			HotBar_2.performed -= instance.OnHotBar_2;
			HotBar_2.canceled -= instance.OnHotBar_2;
			HotBar_3.started -= instance.OnHotBar_3;
			HotBar_3.performed -= instance.OnHotBar_3;
			HotBar_3.canceled -= instance.OnHotBar_3;
			HotBar_4.started -= instance.OnHotBar_4;
			HotBar_4.performed -= instance.OnHotBar_4;
			HotBar_4.canceled -= instance.OnHotBar_4;
			HotBar_5.started -= instance.OnHotBar_5;
			HotBar_5.performed -= instance.OnHotBar_5;
			HotBar_5.canceled -= instance.OnHotBar_5;
			HotBar_6.started -= instance.OnHotBar_6;
			HotBar_6.performed -= instance.OnHotBar_6;
			HotBar_6.canceled -= instance.OnHotBar_6;
			HotBar_7.started -= instance.OnHotBar_7;
			HotBar_7.performed -= instance.OnHotBar_7;
			HotBar_7.canceled -= instance.OnHotBar_7;
			HotBar_8.started -= instance.OnHotBar_8;
			HotBar_8.performed -= instance.OnHotBar_8;
			HotBar_8.canceled -= instance.OnHotBar_8;
			HotBar_9.started -= instance.OnHotBar_9;
			HotBar_9.performed -= instance.OnHotBar_9;
			HotBar_9.canceled -= instance.OnHotBar_9;
			HotBar_0.started -= instance.OnHotBar_0;
			HotBar_0.performed -= instance.OnHotBar_0;
			HotBar_0.canceled -= instance.OnHotBar_0;
		}

		public void RemoveCallbacks(IHotBarActions instance)
		{
			if (m_Wrapper.m_HotBarActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IHotBarActions instance)
		{
			foreach (IHotBarActions hotBarActionsCallbackInterface in m_Wrapper.m_HotBarActionsCallbackInterfaces)
			{
				UnregisterCallbacks(hotBarActionsCallbackInterface);
			}
			m_Wrapper.m_HotBarActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public struct DebugMapActions
	{
		private LocalInputActions m_Wrapper;

		public InputAction TurnOnOffOverlay => m_Wrapper.m_DebugMap_TurnOnOffOverlay;

		public bool enabled => Get().enabled;

		public DebugMapActions(LocalInputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_DebugMap;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(DebugMapActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IDebugMapActions instance)
		{
			if (instance != null && !m_Wrapper.m_DebugMapActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_DebugMapActionsCallbackInterfaces.Add(instance);
				TurnOnOffOverlay.started += instance.OnTurnOnOffOverlay;
				TurnOnOffOverlay.performed += instance.OnTurnOnOffOverlay;
				TurnOnOffOverlay.canceled += instance.OnTurnOnOffOverlay;
			}
		}

		private void UnregisterCallbacks(IDebugMapActions instance)
		{
			TurnOnOffOverlay.started -= instance.OnTurnOnOffOverlay;
			TurnOnOffOverlay.performed -= instance.OnTurnOnOffOverlay;
			TurnOnOffOverlay.canceled -= instance.OnTurnOnOffOverlay;
		}

		public void RemoveCallbacks(IDebugMapActions instance)
		{
			if (m_Wrapper.m_DebugMapActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IDebugMapActions instance)
		{
			foreach (IDebugMapActions debugMapActionsCallbackInterface in m_Wrapper.m_DebugMapActionsCallbackInterfaces)
			{
				UnregisterCallbacks(debugMapActionsCallbackInterface);
			}
			m_Wrapper.m_DebugMapActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public struct AnyVectorValueMapActions
	{
		private LocalInputActions m_Wrapper;

		public InputAction AnyVectorChange => m_Wrapper.m_AnyVectorValueMap_AnyVectorChange;

		public bool enabled => Get().enabled;

		public AnyVectorValueMapActions(LocalInputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_AnyVectorValueMap;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(AnyVectorValueMapActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IAnyVectorValueMapActions instance)
		{
			if (instance != null && !m_Wrapper.m_AnyVectorValueMapActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_AnyVectorValueMapActionsCallbackInterfaces.Add(instance);
				AnyVectorChange.started += instance.OnAnyVectorChange;
				AnyVectorChange.performed += instance.OnAnyVectorChange;
				AnyVectorChange.canceled += instance.OnAnyVectorChange;
			}
		}

		private void UnregisterCallbacks(IAnyVectorValueMapActions instance)
		{
			AnyVectorChange.started -= instance.OnAnyVectorChange;
			AnyVectorChange.performed -= instance.OnAnyVectorChange;
			AnyVectorChange.canceled -= instance.OnAnyVectorChange;
		}

		public void RemoveCallbacks(IAnyVectorValueMapActions instance)
		{
			if (m_Wrapper.m_AnyVectorValueMapActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IAnyVectorValueMapActions instance)
		{
			foreach (IAnyVectorValueMapActions anyVectorValueMapActionsCallbackInterface in m_Wrapper.m_AnyVectorValueMapActionsCallbackInterfaces)
			{
				UnregisterCallbacks(anyVectorValueMapActionsCallbackInterface);
			}
			m_Wrapper.m_AnyVectorValueMapActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public struct UIMapActions
	{
		private LocalInputActions m_Wrapper;

		public InputAction UIApply => m_Wrapper.m_UIMap_UIApply;

		public InputAction UIApplyWindow => m_Wrapper.m_UIMap_UIApplyWindow;

		public InputAction UIBack => m_Wrapper.m_UIMap_UIBack;

		public InputAction Navigate => m_Wrapper.m_UIMap_Navigate;

		public InputAction Submit => m_Wrapper.m_UIMap_Submit;

		public InputAction Cancel => m_Wrapper.m_UIMap_Cancel;

		public InputAction Point => m_Wrapper.m_UIMap_Point;

		public InputAction Click => m_Wrapper.m_UIMap_Click;

		public InputAction ScrollWheel => m_Wrapper.m_UIMap_ScrollWheel;

		public InputAction MiddleClick => m_Wrapper.m_UIMap_MiddleClick;

		public InputAction RightClick => m_Wrapper.m_UIMap_RightClick;

		public InputAction TrackedDevicePosition => m_Wrapper.m_UIMap_TrackedDevicePosition;

		public InputAction TrackedDeviceOrientation => m_Wrapper.m_UIMap_TrackedDeviceOrientation;

		public InputAction UpgradesBack => m_Wrapper.m_UIMap_UpgradesBack;

		public InputAction AdditionalNavigationLeft => m_Wrapper.m_UIMap_AdditionalNavigationLeft;

		public InputAction AdditionalNavigationRight => m_Wrapper.m_UIMap_AdditionalNavigationRight;

		public InputAction ExtraAdditionalNavigationLeft => m_Wrapper.m_UIMap_ExtraAdditionalNavigationLeft;

		public InputAction ExtraAdditionalNavigationRight => m_Wrapper.m_UIMap_ExtraAdditionalNavigationRight;

		public InputAction HoldApply => m_Wrapper.m_UIMap_HoldApply;

		public bool enabled => Get().enabled;

		public UIMapActions(LocalInputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_UIMap;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(UIMapActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IUIMapActions instance)
		{
			if (instance != null && !m_Wrapper.m_UIMapActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_UIMapActionsCallbackInterfaces.Add(instance);
				UIApply.started += instance.OnUIApply;
				UIApply.performed += instance.OnUIApply;
				UIApply.canceled += instance.OnUIApply;
				UIApplyWindow.started += instance.OnUIApplyWindow;
				UIApplyWindow.performed += instance.OnUIApplyWindow;
				UIApplyWindow.canceled += instance.OnUIApplyWindow;
				UIBack.started += instance.OnUIBack;
				UIBack.performed += instance.OnUIBack;
				UIBack.canceled += instance.OnUIBack;
				Navigate.started += instance.OnNavigate;
				Navigate.performed += instance.OnNavigate;
				Navigate.canceled += instance.OnNavigate;
				Submit.started += instance.OnSubmit;
				Submit.performed += instance.OnSubmit;
				Submit.canceled += instance.OnSubmit;
				Cancel.started += instance.OnCancel;
				Cancel.performed += instance.OnCancel;
				Cancel.canceled += instance.OnCancel;
				Point.started += instance.OnPoint;
				Point.performed += instance.OnPoint;
				Point.canceled += instance.OnPoint;
				Click.started += instance.OnClick;
				Click.performed += instance.OnClick;
				Click.canceled += instance.OnClick;
				ScrollWheel.started += instance.OnScrollWheel;
				ScrollWheel.performed += instance.OnScrollWheel;
				ScrollWheel.canceled += instance.OnScrollWheel;
				MiddleClick.started += instance.OnMiddleClick;
				MiddleClick.performed += instance.OnMiddleClick;
				MiddleClick.canceled += instance.OnMiddleClick;
				RightClick.started += instance.OnRightClick;
				RightClick.performed += instance.OnRightClick;
				RightClick.canceled += instance.OnRightClick;
				TrackedDevicePosition.started += instance.OnTrackedDevicePosition;
				TrackedDevicePosition.performed += instance.OnTrackedDevicePosition;
				TrackedDevicePosition.canceled += instance.OnTrackedDevicePosition;
				TrackedDeviceOrientation.started += instance.OnTrackedDeviceOrientation;
				TrackedDeviceOrientation.performed += instance.OnTrackedDeviceOrientation;
				TrackedDeviceOrientation.canceled += instance.OnTrackedDeviceOrientation;
				UpgradesBack.started += instance.OnUpgradesBack;
				UpgradesBack.performed += instance.OnUpgradesBack;
				UpgradesBack.canceled += instance.OnUpgradesBack;
				AdditionalNavigationLeft.started += instance.OnAdditionalNavigationLeft;
				AdditionalNavigationLeft.performed += instance.OnAdditionalNavigationLeft;
				AdditionalNavigationLeft.canceled += instance.OnAdditionalNavigationLeft;
				AdditionalNavigationRight.started += instance.OnAdditionalNavigationRight;
				AdditionalNavigationRight.performed += instance.OnAdditionalNavigationRight;
				AdditionalNavigationRight.canceled += instance.OnAdditionalNavigationRight;
				ExtraAdditionalNavigationLeft.started += instance.OnExtraAdditionalNavigationLeft;
				ExtraAdditionalNavigationLeft.performed += instance.OnExtraAdditionalNavigationLeft;
				ExtraAdditionalNavigationLeft.canceled += instance.OnExtraAdditionalNavigationLeft;
				ExtraAdditionalNavigationRight.started += instance.OnExtraAdditionalNavigationRight;
				ExtraAdditionalNavigationRight.performed += instance.OnExtraAdditionalNavigationRight;
				ExtraAdditionalNavigationRight.canceled += instance.OnExtraAdditionalNavigationRight;
				HoldApply.started += instance.OnHoldApply;
				HoldApply.performed += instance.OnHoldApply;
				HoldApply.canceled += instance.OnHoldApply;
			}
		}

		private void UnregisterCallbacks(IUIMapActions instance)
		{
			UIApply.started -= instance.OnUIApply;
			UIApply.performed -= instance.OnUIApply;
			UIApply.canceled -= instance.OnUIApply;
			UIApplyWindow.started -= instance.OnUIApplyWindow;
			UIApplyWindow.performed -= instance.OnUIApplyWindow;
			UIApplyWindow.canceled -= instance.OnUIApplyWindow;
			UIBack.started -= instance.OnUIBack;
			UIBack.performed -= instance.OnUIBack;
			UIBack.canceled -= instance.OnUIBack;
			Navigate.started -= instance.OnNavigate;
			Navigate.performed -= instance.OnNavigate;
			Navigate.canceled -= instance.OnNavigate;
			Submit.started -= instance.OnSubmit;
			Submit.performed -= instance.OnSubmit;
			Submit.canceled -= instance.OnSubmit;
			Cancel.started -= instance.OnCancel;
			Cancel.performed -= instance.OnCancel;
			Cancel.canceled -= instance.OnCancel;
			Point.started -= instance.OnPoint;
			Point.performed -= instance.OnPoint;
			Point.canceled -= instance.OnPoint;
			Click.started -= instance.OnClick;
			Click.performed -= instance.OnClick;
			Click.canceled -= instance.OnClick;
			ScrollWheel.started -= instance.OnScrollWheel;
			ScrollWheel.performed -= instance.OnScrollWheel;
			ScrollWheel.canceled -= instance.OnScrollWheel;
			MiddleClick.started -= instance.OnMiddleClick;
			MiddleClick.performed -= instance.OnMiddleClick;
			MiddleClick.canceled -= instance.OnMiddleClick;
			RightClick.started -= instance.OnRightClick;
			RightClick.performed -= instance.OnRightClick;
			RightClick.canceled -= instance.OnRightClick;
			TrackedDevicePosition.started -= instance.OnTrackedDevicePosition;
			TrackedDevicePosition.performed -= instance.OnTrackedDevicePosition;
			TrackedDevicePosition.canceled -= instance.OnTrackedDevicePosition;
			TrackedDeviceOrientation.started -= instance.OnTrackedDeviceOrientation;
			TrackedDeviceOrientation.performed -= instance.OnTrackedDeviceOrientation;
			TrackedDeviceOrientation.canceled -= instance.OnTrackedDeviceOrientation;
			UpgradesBack.started -= instance.OnUpgradesBack;
			UpgradesBack.performed -= instance.OnUpgradesBack;
			UpgradesBack.canceled -= instance.OnUpgradesBack;
			AdditionalNavigationLeft.started -= instance.OnAdditionalNavigationLeft;
			AdditionalNavigationLeft.performed -= instance.OnAdditionalNavigationLeft;
			AdditionalNavigationLeft.canceled -= instance.OnAdditionalNavigationLeft;
			AdditionalNavigationRight.started -= instance.OnAdditionalNavigationRight;
			AdditionalNavigationRight.performed -= instance.OnAdditionalNavigationRight;
			AdditionalNavigationRight.canceled -= instance.OnAdditionalNavigationRight;
			ExtraAdditionalNavigationLeft.started -= instance.OnExtraAdditionalNavigationLeft;
			ExtraAdditionalNavigationLeft.performed -= instance.OnExtraAdditionalNavigationLeft;
			ExtraAdditionalNavigationLeft.canceled -= instance.OnExtraAdditionalNavigationLeft;
			ExtraAdditionalNavigationRight.started -= instance.OnExtraAdditionalNavigationRight;
			ExtraAdditionalNavigationRight.performed -= instance.OnExtraAdditionalNavigationRight;
			ExtraAdditionalNavigationRight.canceled -= instance.OnExtraAdditionalNavigationRight;
			HoldApply.started -= instance.OnHoldApply;
			HoldApply.performed -= instance.OnHoldApply;
			HoldApply.canceled -= instance.OnHoldApply;
		}

		public void RemoveCallbacks(IUIMapActions instance)
		{
			if (m_Wrapper.m_UIMapActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IUIMapActions instance)
		{
			foreach (IUIMapActions uIMapActionsCallbackInterface in m_Wrapper.m_UIMapActionsCallbackInterfaces)
			{
				UnregisterCallbacks(uIMapActionsCallbackInterface);
			}
			m_Wrapper.m_UIMapActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public struct VoiceActions
	{
		private LocalInputActions m_Wrapper;

		public InputAction PushToTalk => m_Wrapper.m_Voice_PushToTalk;

		public bool enabled => Get().enabled;

		public VoiceActions(LocalInputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_Voice;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(VoiceActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IVoiceActions instance)
		{
			if (instance != null && !m_Wrapper.m_VoiceActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_VoiceActionsCallbackInterfaces.Add(instance);
				PushToTalk.started += instance.OnPushToTalk;
				PushToTalk.performed += instance.OnPushToTalk;
				PushToTalk.canceled += instance.OnPushToTalk;
			}
		}

		private void UnregisterCallbacks(IVoiceActions instance)
		{
			PushToTalk.started -= instance.OnPushToTalk;
			PushToTalk.performed -= instance.OnPushToTalk;
			PushToTalk.canceled -= instance.OnPushToTalk;
		}

		public void RemoveCallbacks(IVoiceActions instance)
		{
			if (m_Wrapper.m_VoiceActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IVoiceActions instance)
		{
			foreach (IVoiceActions voiceActionsCallbackInterface in m_Wrapper.m_VoiceActionsCallbackInterfaces)
			{
				UnregisterCallbacks(voiceActionsCallbackInterface);
			}
			m_Wrapper.m_VoiceActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public struct GameplayInteractionActions
	{
		private LocalInputActions m_Wrapper;

		public InputAction GameplayApply => m_Wrapper.m_GameplayInteraction_GameplayApply;

		public bool enabled => Get().enabled;

		public GameplayInteractionActions(LocalInputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_GameplayInteraction;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(GameplayInteractionActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IGameplayInteractionActions instance)
		{
			if (instance != null && !m_Wrapper.m_GameplayInteractionActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_GameplayInteractionActionsCallbackInterfaces.Add(instance);
				GameplayApply.started += instance.OnGameplayApply;
				GameplayApply.performed += instance.OnGameplayApply;
				GameplayApply.canceled += instance.OnGameplayApply;
			}
		}

		private void UnregisterCallbacks(IGameplayInteractionActions instance)
		{
			GameplayApply.started -= instance.OnGameplayApply;
			GameplayApply.performed -= instance.OnGameplayApply;
			GameplayApply.canceled -= instance.OnGameplayApply;
		}

		public void RemoveCallbacks(IGameplayInteractionActions instance)
		{
			if (m_Wrapper.m_GameplayInteractionActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IGameplayInteractionActions instance)
		{
			foreach (IGameplayInteractionActions gameplayInteractionActionsCallbackInterface in m_Wrapper.m_GameplayInteractionActionsCallbackInterfaces)
			{
				UnregisterCallbacks(gameplayInteractionActionsCallbackInterface);
			}
			m_Wrapper.m_GameplayInteractionActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public interface IUIActions
	{
		void OnSpectatorLeft(InputAction.CallbackContext context);

		void OnSpectatorRight(InputAction.CallbackContext context);

		void OnOpenSettings(InputAction.CallbackContext context);
	}

	public interface IMovementMapActions
	{
		void OnMovement(InputAction.CallbackContext context);

		void OnJump(InputAction.CallbackContext context);

		void OnRotation(InputAction.CallbackContext context);

		void OnChangeMouseVisibility(InputAction.CallbackContext context);

		void OnCrouch(InputAction.CallbackContext context);

		void OnSprint(InputAction.CallbackContext context);
	}

	public interface IArmMapActions
	{
		void OnGrabItem(InputAction.CallbackContext context);

		void OnDropAllFromArms(InputAction.CallbackContext context);

		void OnArmItemDistanceChange(InputAction.CallbackContext context);

		void OnItemInteract(InputAction.CallbackContext context);

		void OnMouseForward(InputAction.CallbackContext context);

		void OnMouseBack(InputAction.CallbackContext context);
	}

	public interface IEmotionsActions
	{
		void OnBodyEmote(InputAction.CallbackContext context);

		void OnFaceEmote(InputAction.CallbackContext context);

		void OnHandEmote(InputAction.CallbackContext context);

		void OnEmoteNavigation(InputAction.CallbackContext context);
	}

	public interface IHotBarActions
	{
		void OnHotBar_1(InputAction.CallbackContext context);

		void OnHotBar_2(InputAction.CallbackContext context);

		void OnHotBar_3(InputAction.CallbackContext context);

		void OnHotBar_4(InputAction.CallbackContext context);

		void OnHotBar_5(InputAction.CallbackContext context);

		void OnHotBar_6(InputAction.CallbackContext context);

		void OnHotBar_7(InputAction.CallbackContext context);

		void OnHotBar_8(InputAction.CallbackContext context);

		void OnHotBar_9(InputAction.CallbackContext context);

		void OnHotBar_0(InputAction.CallbackContext context);
	}

	public interface IDebugMapActions
	{
		void OnTurnOnOffOverlay(InputAction.CallbackContext context);
	}

	public interface IAnyVectorValueMapActions
	{
		void OnAnyVectorChange(InputAction.CallbackContext context);
	}

	public interface IUIMapActions
	{
		void OnUIApply(InputAction.CallbackContext context);

		void OnUIApplyWindow(InputAction.CallbackContext context);

		void OnUIBack(InputAction.CallbackContext context);

		void OnNavigate(InputAction.CallbackContext context);

		void OnSubmit(InputAction.CallbackContext context);

		void OnCancel(InputAction.CallbackContext context);

		void OnPoint(InputAction.CallbackContext context);

		void OnClick(InputAction.CallbackContext context);

		void OnScrollWheel(InputAction.CallbackContext context);

		void OnMiddleClick(InputAction.CallbackContext context);

		void OnRightClick(InputAction.CallbackContext context);

		void OnTrackedDevicePosition(InputAction.CallbackContext context);

		void OnTrackedDeviceOrientation(InputAction.CallbackContext context);

		void OnUpgradesBack(InputAction.CallbackContext context);

		void OnAdditionalNavigationLeft(InputAction.CallbackContext context);

		void OnAdditionalNavigationRight(InputAction.CallbackContext context);

		void OnExtraAdditionalNavigationLeft(InputAction.CallbackContext context);

		void OnExtraAdditionalNavigationRight(InputAction.CallbackContext context);

		void OnHoldApply(InputAction.CallbackContext context);
	}

	public interface IVoiceActions
	{
		void OnPushToTalk(InputAction.CallbackContext context);
	}

	public interface IGameplayInteractionActions
	{
		void OnGameplayApply(InputAction.CallbackContext context);
	}

	private readonly InputActionMap m_UI;

	private List<IUIActions> m_UIActionsCallbackInterfaces = new List<IUIActions>();

	private readonly InputAction m_UI_SpectatorLeft;

	private readonly InputAction m_UI_SpectatorRight;

	private readonly InputAction m_UI_OpenSettings;

	private readonly InputActionMap m_MovementMap;

	private List<IMovementMapActions> m_MovementMapActionsCallbackInterfaces = new List<IMovementMapActions>();

	private readonly InputAction m_MovementMap_Movement;

	private readonly InputAction m_MovementMap_Jump;

	private readonly InputAction m_MovementMap_Rotation;

	private readonly InputAction m_MovementMap_ChangeMouseVisibility;

	private readonly InputAction m_MovementMap_Crouch;

	private readonly InputAction m_MovementMap_Sprint;

	private readonly InputActionMap m_ArmMap;

	private List<IArmMapActions> m_ArmMapActionsCallbackInterfaces = new List<IArmMapActions>();

	private readonly InputAction m_ArmMap_GrabItem;

	private readonly InputAction m_ArmMap_DropAllFromArms;

	private readonly InputAction m_ArmMap_ArmItemDistanceChange;

	private readonly InputAction m_ArmMap_ItemInteract;

	private readonly InputAction m_ArmMap_MouseForward;

	private readonly InputAction m_ArmMap_MouseBack;

	private readonly InputActionMap m_Emotions;

	private List<IEmotionsActions> m_EmotionsActionsCallbackInterfaces = new List<IEmotionsActions>();

	private readonly InputAction m_Emotions_BodyEmote;

	private readonly InputAction m_Emotions_FaceEmote;

	private readonly InputAction m_Emotions_HandEmote;

	private readonly InputAction m_Emotions_EmoteNavigation;

	private readonly InputActionMap m_HotBar;

	private List<IHotBarActions> m_HotBarActionsCallbackInterfaces = new List<IHotBarActions>();

	private readonly InputAction m_HotBar_HotBar_1;

	private readonly InputAction m_HotBar_HotBar_2;

	private readonly InputAction m_HotBar_HotBar_3;

	private readonly InputAction m_HotBar_HotBar_4;

	private readonly InputAction m_HotBar_HotBar_5;

	private readonly InputAction m_HotBar_HotBar_6;

	private readonly InputAction m_HotBar_HotBar_7;

	private readonly InputAction m_HotBar_HotBar_8;

	private readonly InputAction m_HotBar_HotBar_9;

	private readonly InputAction m_HotBar_HotBar_0;

	private readonly InputActionMap m_DebugMap;

	private List<IDebugMapActions> m_DebugMapActionsCallbackInterfaces = new List<IDebugMapActions>();

	private readonly InputAction m_DebugMap_TurnOnOffOverlay;

	private readonly InputActionMap m_AnyVectorValueMap;

	private List<IAnyVectorValueMapActions> m_AnyVectorValueMapActionsCallbackInterfaces = new List<IAnyVectorValueMapActions>();

	private readonly InputAction m_AnyVectorValueMap_AnyVectorChange;

	private readonly InputActionMap m_UIMap;

	private List<IUIMapActions> m_UIMapActionsCallbackInterfaces = new List<IUIMapActions>();

	private readonly InputAction m_UIMap_UIApply;

	private readonly InputAction m_UIMap_UIApplyWindow;

	private readonly InputAction m_UIMap_UIBack;

	private readonly InputAction m_UIMap_Navigate;

	private readonly InputAction m_UIMap_Submit;

	private readonly InputAction m_UIMap_Cancel;

	private readonly InputAction m_UIMap_Point;

	private readonly InputAction m_UIMap_Click;

	private readonly InputAction m_UIMap_ScrollWheel;

	private readonly InputAction m_UIMap_MiddleClick;

	private readonly InputAction m_UIMap_RightClick;

	private readonly InputAction m_UIMap_TrackedDevicePosition;

	private readonly InputAction m_UIMap_TrackedDeviceOrientation;

	private readonly InputAction m_UIMap_UpgradesBack;

	private readonly InputAction m_UIMap_AdditionalNavigationLeft;

	private readonly InputAction m_UIMap_AdditionalNavigationRight;

	private readonly InputAction m_UIMap_ExtraAdditionalNavigationLeft;

	private readonly InputAction m_UIMap_ExtraAdditionalNavigationRight;

	private readonly InputAction m_UIMap_HoldApply;

	private readonly InputActionMap m_Voice;

	private List<IVoiceActions> m_VoiceActionsCallbackInterfaces = new List<IVoiceActions>();

	private readonly InputAction m_Voice_PushToTalk;

	private readonly InputActionMap m_GameplayInteraction;

	private List<IGameplayInteractionActions> m_GameplayInteractionActionsCallbackInterfaces = new List<IGameplayInteractionActions>();

	private readonly InputAction m_GameplayInteraction_GameplayApply;

	private int m_KeyboardandMouseSchemeIndex = -1;

	private int m_TouchscreenSchemeIndex = -1;

	public InputActionAsset asset { get; }

	public InputBinding? bindingMask
	{
		get
		{
			return asset.bindingMask;
		}
		set
		{
			asset.bindingMask = value;
		}
	}

	public ReadOnlyArray<InputDevice>? devices
	{
		get
		{
			return asset.devices;
		}
		set
		{
			asset.devices = value;
		}
	}

	public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

	public IEnumerable<InputBinding> bindings => asset.bindings;

	public UIActions UI => new UIActions(this);

	public MovementMapActions MovementMap => new MovementMapActions(this);

	public ArmMapActions ArmMap => new ArmMapActions(this);

	public EmotionsActions Emotions => new EmotionsActions(this);

	public HotBarActions HotBar => new HotBarActions(this);

	public DebugMapActions DebugMap => new DebugMapActions(this);

	public AnyVectorValueMapActions AnyVectorValueMap => new AnyVectorValueMapActions(this);

	public UIMapActions UIMap => new UIMapActions(this);

	public VoiceActions Voice => new VoiceActions(this);

	public GameplayInteractionActions GameplayInteraction => new GameplayInteractionActions(this);

	public InputControlScheme KeyboardandMouseScheme
	{
		get
		{
			if (m_KeyboardandMouseSchemeIndex == -1)
			{
				m_KeyboardandMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and Mouse");
			}
			return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
		}
	}

	public InputControlScheme TouchscreenScheme
	{
		get
		{
			if (m_TouchscreenSchemeIndex == -1)
			{
				m_TouchscreenSchemeIndex = asset.FindControlSchemeIndex("Touchscreen");
			}
			return asset.controlSchemes[m_TouchscreenSchemeIndex];
		}
	}

	public LocalInputActions()
	{
		asset = InputActionAsset.FromJson("{\r\n    \"version\": 1,\r\n    \"name\": \"LocalInputActions\",\r\n    \"maps\": [\r\n        {\r\n            \"name\": \"UI\",\r\n            \"id\": \"1c1b286f-794d-4f5b-bee8-31efdd2a9472\",\r\n            \"actions\": [\r\n                {\r\n                    \"name\": \"SpectatorLeft\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"aaace8c1-9d55-4f57-b4a7-ff295d02b025\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"SpectatorRight\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"8cbb61ed-a6d9-4244-ae5d-63ef5921ca28\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"OpenSettings\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"c13fd5e6-9c9a-471e-b452-d10545c48a51\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                }\r\n            ],\r\n            \"bindings\": [\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"08efb68b-536a-4a41-b07d-d691a6fdd2d3\",\r\n                    \"path\": \"<Keyboard>/a\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"SpectatorLeft\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"c23a0048-32ee-4147-9d37-77815dbd3670\",\r\n                    \"path\": \"<Keyboard>/leftArrow\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"SpectatorLeft\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"573b80bb-cc28-484b-a617-dcfbff70eb29\",\r\n                    \"path\": \"<Gamepad>/leftShoulder\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"SpectatorLeft\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"43afe74f-86c0-485b-a3ae-b7e17be07b28\",\r\n                    \"path\": \"<Gamepad>/leftStick/left\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"StickDeadzone(min=0.7,max=1)\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"SpectatorLeft\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"45d08377-f5d3-465f-aa59-d931b6b529ba\",\r\n                    \"path\": \"<Keyboard>/d\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"SpectatorRight\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"e8c30afd-7ae7-4538-b9bf-d91ff821730c\",\r\n                    \"path\": \"<Keyboard>/rightArrow\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"SpectatorRight\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"ac4d9e19-fb54-4fe1-8d1f-86a6c7c769ea\",\r\n                    \"path\": \"<Gamepad>/rightShoulder\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"SpectatorRight\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"799c9871-52dd-4331-ba13-0a7ab896db9a\",\r\n                    \"path\": \"<Gamepad>/leftStick/right\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"StickDeadzone(min=0.7,max=1)\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"SpectatorRight\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"16ee9965-fd58-4d79-a45c-94daef236f9b\",\r\n                    \"path\": \"<Gamepad>/{Menu}\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"OpenSettings\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"d671ea3e-bf09-4790-9f6e-5cdfa2b2aea8\",\r\n                    \"path\": \"<Gamepad>/start\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"OpenSettings\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"539523fa-7632-4612-83c5-d995ac0bdc8c\",\r\n                    \"path\": \"<DualShockGamepad>/start\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"OpenSettings\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"name\": \"MovementMap\",\r\n            \"id\": \"bebdb075-2a45-4749-9d27-e1e135f74719\",\r\n            \"actions\": [\r\n                {\r\n                    \"name\": \"Movement\",\r\n                    \"type\": \"Value\",\r\n                    \"id\": \"634a7e06-26bd-4e67-8c44-01f0513e9f6f\",\r\n                    \"expectedControlType\": \"Vector2\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                },\r\n                {\r\n                    \"name\": \"Jump\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"aed89727-f0f6-4655-8506-e13255354297\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Rotation\",\r\n                    \"type\": \"Value\",\r\n                    \"id\": \"2cb331b4-6242-4787-811e-9dd0f2eef5e2\",\r\n                    \"expectedControlType\": \"Vector2\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                },\r\n                {\r\n                    \"name\": \"ChangeMouseVisibility\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"66b7ba01-e353-443d-9b81-bc0cfa59107e\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Crouch\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"838c7bdb-690c-4f94-a17b-2a6dd01c7d86\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Sprint\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"a11027af-2935-42fd-8539-5e86deb80827\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                }\r\n            ],\r\n            \"bindings\": [\r\n                {\r\n                    \"name\": \"WASD\",\r\n                    \"id\": \"78c59ed6-1b5a-4904-ae3e-4ca2b19448e3\",\r\n                    \"path\": \"2DVector(mode=1)\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Movement\",\r\n                    \"isComposite\": true,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"up\",\r\n                    \"id\": \"4ad86955-fd11-488b-b29d-7a8cb5ac297d\",\r\n                    \"path\": \"<Keyboard>/w\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"Movement\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"down\",\r\n                    \"id\": \"ff37c92b-b9db-4627-9c0a-b56b12623eae\",\r\n                    \"path\": \"<Keyboard>/s\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"Movement\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"left\",\r\n                    \"id\": \"fd2d4901-442c-4ab0-8d09-a4440a355456\",\r\n                    \"path\": \"<Keyboard>/a\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"Movement\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"right\",\r\n                    \"id\": \"5a16be20-e144-4183-9f7f-4ab3cf69cf4a\",\r\n                    \"path\": \"<Keyboard>/d\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"Movement\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"cab3e669-4949-4fde-b9da-12627b571041\",\r\n                    \"path\": \"<Gamepad>/leftStick\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Movement\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"7abfae3b-398f-460c-b33f-250ef1d9999b\",\r\n                    \"path\": \"<Keyboard>/space\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Keyboard and Mouse\",\r\n                    \"action\": \"Jump\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"b1b3baaf-bc9e-4a8a-ae26-26122f37a362\",\r\n                    \"path\": \"<Gamepad>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Keyboard and Mouse\",\r\n                    \"action\": \"Jump\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"47736d85-f6f1-47c4-8b8c-655a10a7f206\",\r\n                    \"path\": \"<Mouse>/delta\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Rotation\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"048a0c2d-2279-4452-9f5f-a3fb91800426\",\r\n                    \"path\": \"<Gamepad>/rightStick\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"ScaleVector2(x=50,y=50)\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Rotation\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"ec953ca8-d375-4467-a147-ed0d2b4c5d38\",\r\n                    \"path\": \"<Keyboard>/tab\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"ChangeMouseVisibility\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"6130a401-67d6-429c-a932-cf81117e3767\",\r\n                    \"path\": \"<Keyboard>/ctrl\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Keyboard and Mouse\",\r\n                    \"action\": \"Crouch\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"f8f438a6-2e13-40c0-b398-5993e449fa5b\",\r\n                    \"path\": \"<Gamepad>/rightStickPress\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Keyboard and Mouse\",\r\n                    \"action\": \"Crouch\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"07ce26f0-a659-42a6-9824-0768f620843f\",\r\n                    \"path\": \"<Keyboard>/leftShift\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Keyboard and Mouse\",\r\n                    \"action\": \"Sprint\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"4b756443-8d01-43e2-8eb7-14ffa53cfd7d\",\r\n                    \"path\": \"<Gamepad>/leftStickPress\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Keyboard and Mouse\",\r\n                    \"action\": \"Sprint\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"name\": \"ArmMap\",\r\n            \"id\": \"12eb2f2d-9877-47ab-90e8-618b078c8fc5\",\r\n            \"actions\": [\r\n                {\r\n                    \"name\": \"GrabItem\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"e1316e46-bca9-4a0d-9cd1-bfa54ac7acf3\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"DropAllFromArms\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"f28cee0a-d88f-4f1c-a0c7-8547ad3c80f8\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"Hold(duration=1)\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"ArmItemDistanceChange\",\r\n                    \"type\": \"Value\",\r\n                    \"id\": \"adee205e-1f5a-4e0f-bbcd-48dc5784801c\",\r\n                    \"expectedControlType\": \"Vector2\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                },\r\n                {\r\n                    \"name\": \"ItemInteract\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"241c94ab-102a-4653-91f3-46ce8c5e7f68\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"MouseForward\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"ab9b6ed2-f5fc-4a0a-bd9a-b8a3dc152764\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"MouseBack\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"8269cf62-9a0d-40aa-9c45-cfe2dfcb8cc0\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                }\r\n            ],\r\n            \"bindings\": [\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"41df135b-21b4-424a-82f7-30af8af79a2b\",\r\n                    \"path\": \"<Mouse>/leftButton\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"GrabItem\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"e821600f-ce02-4126-9c3a-1d25bccbe102\",\r\n                    \"path\": \"<Gamepad>/rightTrigger\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"GrabItem\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"f582773b-045d-4b15-9c28-c4f9faad4344\",\r\n                    \"path\": \"<Keyboard>/q\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"DropAllFromArms\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"fcd44a10-6ee1-4203-9efc-4cb886af2b75\",\r\n                    \"path\": \"<Gamepad>/buttonNorth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"DropAllFromArms\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"35366914-501c-4aed-8892-aea2b55e5351\",\r\n                    \"path\": \"<Mouse>/scroll\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"ArmItemDistanceChange\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"9cfa3aba-6d8d-43de-adb9-965d06fcffb8\",\r\n                    \"path\": \"<Mouse>/rightButton\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"ItemInteract\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"b1087c08-336b-436f-b180-3f979334d169\",\r\n                    \"path\": \"<Gamepad>/buttonWest\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"ItemInteract\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"69a880bb-80fc-41d8-be13-a3992cf35a0a\",\r\n                    \"path\": \"<Mouse>/{Forward}\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"MouseForward\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"16b296a8-f9db-462d-825d-ebf716e045f4\",\r\n                    \"path\": \"<Gamepad>/rightStick/up\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"MouseForward\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"e2c80fbd-12c6-478e-931b-2ec3a5882aa7\",\r\n                    \"path\": \"<Mouse>/{Back}\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"MouseBack\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"0f9c2946-730e-4d92-b700-b3a0f3f74fba\",\r\n                    \"path\": \"<Gamepad>/leftStick/down\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"MouseBack\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"name\": \"Emotions\",\r\n            \"id\": \"81f9b87b-05b4-4a95-989d-de119d3a25fb\",\r\n            \"actions\": [\r\n                {\r\n                    \"name\": \"BodyEmote\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"cf8741e2-bbf4-4ebd-9ec8-0429b26c6710\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"FaceEmote\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"8e6fe9a1-0873-469c-81f3-3d95f549f8ca\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"HandEmote\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"e12fd4eb-204c-44cd-b9b4-c2fde1e9017e\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"EmoteNavigation\",\r\n                    \"type\": \"Value\",\r\n                    \"id\": \"44fc4e46-ef32-42cb-9146-f81779dd2381\",\r\n                    \"expectedControlType\": \"Vector2\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                }\r\n            ],\r\n            \"bindings\": [\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"9c0587a6-c247-4292-a2c2-a8ee39a31bb2\",\r\n                    \"path\": \"<Keyboard>/z\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"BodyEmote\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"31fccf99-c999-45f7-954b-432135892103\",\r\n                    \"path\": \"<Gamepad>/leftShoulder\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"BodyEmote\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"20745b63-43ba-41e0-ba22-0a205f4ce69b\",\r\n                    \"path\": \"<Keyboard>/x\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"FaceEmote\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"3dc8e04a-e5c3-4339-a4c5-cecb47a495e0\",\r\n                    \"path\": \"<Keyboard>/c\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"HandEmote\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"ac80009c-43e4-4027-a60b-5a08130cb3e3\",\r\n                    \"path\": \"<Gamepad>/dpad\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"ScaleVector2(x=20,y=20)\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"EmoteNavigation\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"name\": \"HotBar\",\r\n            \"id\": \"b0ae8b84-eeeb-4c36-9e6f-c8b1f06205f2\",\r\n            \"actions\": [\r\n                {\r\n                    \"name\": \"HotBar_1\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"7f0a3bee-5d47-437e-ba96-986bad65ae1e\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"HotBar_2\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"066363ee-3099-4f06-b751-192fe8b7972a\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"HotBar_3\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"5ba0aa58-4ab6-402a-8443-b74fbe1f1e96\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"HotBar_4\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"7af89943-f2bf-49dc-91d6-f7b33fa95063\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"HotBar_5\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"bb2eefa1-50a8-40b1-8a1e-bb10d74484ea\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"HotBar_6\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"f2edf130-0810-4df8-95e8-084bc06c31e9\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"HotBar_7\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"ac848469-3731-4eb6-9ea1-9b98ab3e23fd\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"HotBar_8\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"0bd14086-8be7-4dfd-8611-6f5f75a5cdf1\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"HotBar_9\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"f13812da-0c08-430b-9e63-cc3e37dbc055\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"HotBar_0\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"af5baf03-acdc-4044-b81b-be76f798bbb3\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                }\r\n            ],\r\n            \"bindings\": [\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"62f520cc-e9f5-4e20-96dd-0b2703384a3e\",\r\n                    \"path\": \"<Keyboard>/1\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"HotBar_1\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"e788f8a0-6d7e-48f1-95b9-48a0a88f0a43\",\r\n                    \"path\": \"<Keyboard>/2\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"HotBar_2\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"e8ec9fd3-1821-4e29-9a2d-18eb042263fc\",\r\n                    \"path\": \"<Keyboard>/3\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"HotBar_3\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"24e9597b-93f0-4b79-8985-67293e2b4886\",\r\n                    \"path\": \"<Keyboard>/4\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"HotBar_4\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"ae34fc65-94b9-4926-8d7a-7021d5e5df12\",\r\n                    \"path\": \"<Keyboard>/5\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"HotBar_5\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"3b60f820-be27-407a-b070-7eb8d0f3e809\",\r\n                    \"path\": \"<Keyboard>/6\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"HotBar_6\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"54cb5059-a372-4d64-8714-38698c88a369\",\r\n                    \"path\": \"<Keyboard>/7\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"HotBar_7\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"7113fddc-06a2-4271-94d7-eee63547ace2\",\r\n                    \"path\": \"<Keyboard>/8\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"HotBar_8\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"9bb60852-6f46-4c8f-89fd-0b36a6c20f38\",\r\n                    \"path\": \"<Keyboard>/9\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"HotBar_9\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"12219776-15a5-4c25-bf26-6ff06ef36a1c\",\r\n                    \"path\": \"<Keyboard>/0\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"HotBar_0\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"name\": \"DebugMap\",\r\n            \"id\": \"2f56ff50-726e-4121-9747-524a5353eb64\",\r\n            \"actions\": [\r\n                {\r\n                    \"name\": \"TurnOnOffOverlay\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"36dcadc6-7779-42e8-8ffc-7fcd374552b0\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                }\r\n            ],\r\n            \"bindings\": [\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"574a24d5-14d3-4ff1-a418-43c17468d21b\",\r\n                    \"path\": \"<Keyboard>/f7\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"TurnOnOffOverlay\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"name\": \"AnyVectorValueMap\",\r\n            \"id\": \"3dad715e-216f-4b89-89ec-de8e4246949e\",\r\n            \"actions\": [\r\n                {\r\n                    \"name\": \"AnyVectorChange\",\r\n                    \"type\": \"Value\",\r\n                    \"id\": \"b3c1771b-a64b-4341-b5b1-3ada37cfdbaa\",\r\n                    \"expectedControlType\": \"Vector2\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                }\r\n            ],\r\n            \"bindings\": [\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"37a549db-0607-489d-a7bf-f858a17a3376\",\r\n                    \"path\": \"<Gamepad>/rightStick\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"StickDeadzone(min=0.7,max=1)\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"AnyVectorChange\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"62aa296a-77cc-486f-8dd7-c1d0e948c89c\",\r\n                    \"path\": \"<Gamepad>/leftStick\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"StickDeadzone(min=0.7,max=1)\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"AnyVectorChange\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"04694f01-5ba1-4767-b542-25d9ec91aefd\",\r\n                    \"path\": \"<Mouse>/delta\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"StickDeadzone(min=0.7,max=1)\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"AnyVectorChange\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"name\": \"UIMap\",\r\n            \"id\": \"dbff103b-1499-46ef-9dee-8ee6b5b4ecdd\",\r\n            \"actions\": [\r\n                {\r\n                    \"name\": \"UIApply\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"d3c0fb82-78c4-4af3-9c8d-20d4fdc0b8ae\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"UIApplyWindow\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"e4085e9a-f7fd-400b-9edd-fcca893597d6\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"UIBack\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"436ccd87-3243-49b2-80b7-c5823fe0fa37\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Navigate\",\r\n                    \"type\": \"PassThrough\",\r\n                    \"id\": \"e0512bf8-5e2d-450b-a2b1-f7ca956ca2a8\",\r\n                    \"expectedControlType\": \"Vector2\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Submit\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"3ac19de4-28f8-456f-95e6-c539aefa1464\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Cancel\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"b39181a8-3ad8-4189-8b3e-c4f5dd09d974\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"Point\",\r\n                    \"type\": \"PassThrough\",\r\n                    \"id\": \"b355c238-661f-4ba5-8b33-22bca156e853\",\r\n                    \"expectedControlType\": \"Vector2\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                },\r\n                {\r\n                    \"name\": \"Click\",\r\n                    \"type\": \"PassThrough\",\r\n                    \"id\": \"8b1d178d-6e45-4e57-bf05-c0feb3c60270\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": true\r\n                },\r\n                {\r\n                    \"name\": \"ScrollWheel\",\r\n                    \"type\": \"PassThrough\",\r\n                    \"id\": \"f6b098b7-0fcf-4618-a522-7c5e9695a1b5\",\r\n                    \"expectedControlType\": \"Vector2\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"MiddleClick\",\r\n                    \"type\": \"PassThrough\",\r\n                    \"id\": \"2398c695-aae0-431f-9cf6-5df84a14bb45\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"RightClick\",\r\n                    \"type\": \"PassThrough\",\r\n                    \"id\": \"ce14ab94-e046-4b28-8a1e-f3c47d34368c\",\r\n                    \"expectedControlType\": \"Button\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"TrackedDevicePosition\",\r\n                    \"type\": \"PassThrough\",\r\n                    \"id\": \"0f894193-1e14-4ef8-997e-09c2a8bb663a\",\r\n                    \"expectedControlType\": \"Vector3\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"TrackedDeviceOrientation\",\r\n                    \"type\": \"PassThrough\",\r\n                    \"id\": \"fd73c0d7-15cf-4126-a387-37066fd0d253\",\r\n                    \"expectedControlType\": \"Quaternion\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"UpgradesBack\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"c2ed003d-afe2-4ec4-ae29-f73274707cf2\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"AdditionalNavigationLeft\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"4438e06b-839f-4a87-9d90-d967f6605268\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"AdditionalNavigationRight\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"96994a36-37a1-4d0f-aa9c-30a3c552d542\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"ExtraAdditionalNavigationLeft\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"e245408e-3a4d-49b9-9100-2a41ed1c6038\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"ExtraAdditionalNavigationRight\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"2e2e366f-91f6-46ee-84e1-1bf0d61449f9\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                },\r\n                {\r\n                    \"name\": \"HoldApply\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"babf7b82-fe5d-4792-b67f-0c1c5a6052df\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"Hold(duration=1)\",\r\n                    \"initialStateCheck\": false\r\n                }\r\n            ],\r\n            \"bindings\": [\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"2f8c936a-0b2a-42da-a5fe-1b0f375345b2\",\r\n                    \"path\": \"<Keyboard>/enter\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"UIApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"232a45f2-c412-4746-bd80-cb188f44cd07\",\r\n                    \"path\": \"<Keyboard>/space\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"UIApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"328b1148-704b-48e8-adce-b605bdf97115\",\r\n                    \"path\": \"<GXDKGamepad>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"596b93ec-61d5-42f0-b86d-872b0680f636\",\r\n                    \"path\": \"<XInputController>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"fc1d366f-e6e1-47b2-aa0a-cb3e238c3778\",\r\n                    \"path\": \"<DualShockGamepad>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"bd6281c0-9527-4c9d-aac9-0f61ff07f6b1\",\r\n                    \"path\": \"<AndroidGamepad>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"ee181fa4-8d5e-4957-9ed7-8f2847b3ce33\",\r\n                    \"path\": \"<iOSGameController>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"d5c8a47c-1d45-4778-85b1-4126a2603175\",\r\n                    \"path\": \"<NimbusGamepadHid>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"7113f9c4-4953-49e2-8353-ed61bd64432f\",\r\n                    \"path\": \"<SwitchProControllerHID>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"b68156aa-a067-4b26-8ffb-a5f66d85a16d\",\r\n                    \"path\": \"<NPad>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"fd5a4b17-f947-4797-8f8e-103196f9b31f\",\r\n                    \"path\": \"<SteamDeck>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"deb217c7-b5bd-4aa9-bd53-169144e2392d\",\r\n                    \"path\": \"<Keyboard>/space\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"UIApplyWindow\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"f491950b-b7d0-4e50-8024-7eea8e056c61\",\r\n                    \"path\": \"<GXDKGamepad>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIApplyWindow\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"df6d8f18-b16e-4ff2-9e85-02fd00c1e641\",\r\n                    \"path\": \"<XInputController>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIApplyWindow\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"33632214-3abe-4a04-9409-71ef4d86fb95\",\r\n                    \"path\": \"<DualShockGamepad>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIApplyWindow\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"4e63d4b4-7eca-4d7a-a924-104e43fd2c5c\",\r\n                    \"path\": \"<AndroidGamepad>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIApplyWindow\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"6a16f3c1-03fe-4df2-a86e-e7a12b3dcda9\",\r\n                    \"path\": \"<iOSGameController>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIApplyWindow\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"0a11c35e-bab5-4dca-b1ff-a8b5b3980608\",\r\n                    \"path\": \"<NimbusGamepadHid>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIApplyWindow\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"de5a609c-627f-40e2-8a00-43d1f6904000\",\r\n                    \"path\": \"<SwitchProControllerHID>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIApplyWindow\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"19b23537-9811-4c6e-8017-12c86875896c\",\r\n                    \"path\": \"<NPad>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIApplyWindow\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"b2af33c6-7ce8-4747-ada0-db917e270808\",\r\n                    \"path\": \"<SteamDeck>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIApplyWindow\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"105d4955-a39f-48e7-8ed1-595096808077\",\r\n                    \"path\": \"<Keyboard>/escape\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"UIBack\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"bbea2ff7-f58b-441d-9387-f324b1205dfa\",\r\n                    \"path\": \"<GXDKGamepad>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIBack\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"d874a5b5-66cf-43b8-8f7e-e0b6afe87220\",\r\n                    \"path\": \"<XInputController>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIBack\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"0ce4e7aa-9d8b-4a8d-95e0-be7aa77cc014\",\r\n                    \"path\": \"<DualShockGamepad>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIBack\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"cfd252c3-0233-41c5-b694-3e0215caff6b\",\r\n                    \"path\": \"<AndroidGamepad>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIBack\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"e88c0b2a-b761-46a6-be74-52f2f5f5447d\",\r\n                    \"path\": \"<iOSGameController>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIBack\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"b3a4b0de-3cdc-48fd-b4aa-ae59d80fcfd3\",\r\n                    \"path\": \"<NimbusGamepadHid>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIBack\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"c9e214fd-6b24-4a37-9604-f432a60b02c8\",\r\n                    \"path\": \"<NPad>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIBack\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"794e6060-4e2e-40c9-b604-1184a70c4ed0\",\r\n                    \"path\": \"<SwitchProControllerHID>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIBack\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"f39e5939-eaac-48a0-bb5f-8ea2c08f18da\",\r\n                    \"path\": \"<SteamDeck>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"UIBack\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"e15afcdc-2d9b-44eb-b938-9a4a0bd6a581\",\r\n                    \"path\": \"<XRController>/deviceRotation\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"TrackedDeviceOrientation\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"Gamepad\",\r\n                    \"id\": \"92b9c3ca-e04f-4c35-9499-ace48b1d7a7f\",\r\n                    \"path\": \"2DVector\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"StickDeadzone(min=0.7,max=1)\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": true,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"up\",\r\n                    \"id\": \"4ff98fad-dc06-4367-9cbe-9cf898011fb6\",\r\n                    \"path\": \"<Gamepad>/leftStick/up\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"up\",\r\n                    \"id\": \"7beaaccd-2b40-4e11-815e-3530910484bf\",\r\n                    \"path\": \"<Gamepad>/rightStick/up\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"up\",\r\n                    \"id\": \"8b9166fb-c6d0-45b8-b705-2301e6c41a3e\",\r\n                    \"path\": \"<Gamepad>/dpad/up\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"down\",\r\n                    \"id\": \"545b2fa7-0b39-428f-a177-38eb902fac9e\",\r\n                    \"path\": \"<Gamepad>/leftStick/down\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"down\",\r\n                    \"id\": \"5f812501-b0a8-4922-a61e-09a29a6285df\",\r\n                    \"path\": \"<Gamepad>/rightStick/down\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"down\",\r\n                    \"id\": \"24db225a-ad95-4993-9333-619c79d1e2bf\",\r\n                    \"path\": \"<Gamepad>/dpad/down\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"left\",\r\n                    \"id\": \"785aab64-9fe5-469d-879f-e73ecf41545f\",\r\n                    \"path\": \"<Gamepad>/leftStick/left\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"left\",\r\n                    \"id\": \"9824fa6b-a698-4720-ae3f-6ca20b12042e\",\r\n                    \"path\": \"<Gamepad>/rightStick/left\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"left\",\r\n                    \"id\": \"19054c2f-4c76-409a-b193-8646357a0940\",\r\n                    \"path\": \"<Gamepad>/dpad/left\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"right\",\r\n                    \"id\": \"8ebdf560-babb-4fb0-b7ef-dbc79fa5369b\",\r\n                    \"path\": \"<Gamepad>/leftStick/right\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"right\",\r\n                    \"id\": \"9da5a5db-498f-4f00-b37c-9a074e5e7b9e\",\r\n                    \"path\": \"<Gamepad>/rightStick/right\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"right\",\r\n                    \"id\": \"9d16842c-968d-40d9-834e-778922e3677b\",\r\n                    \"path\": \"<Gamepad>/dpad/right\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"Joystick\",\r\n                    \"id\": \"524cd834-ea6c-42e7-97b3-8780681b0bd7\",\r\n                    \"path\": \"2DVector\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"StickDeadzone(min=0.7,max=1)\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": true,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"up\",\r\n                    \"id\": \"9790d067-f446-4bb6-81d5-c71698fb4259\",\r\n                    \"path\": \"<Joystick>/stick/up\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"down\",\r\n                    \"id\": \"e1321174-87e2-42fc-b2f2-485d1efbb993\",\r\n                    \"path\": \"<Joystick>/stick/down\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"left\",\r\n                    \"id\": \"0d795090-469c-48e5-8496-a29b255f6d03\",\r\n                    \"path\": \"<Joystick>/stick/left\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"right\",\r\n                    \"id\": \"7c1d7c22-d345-40c9-b201-1af1d4282c94\",\r\n                    \"path\": \"<Joystick>/stick/right\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"Keyboard\",\r\n                    \"id\": \"c72394e0-d33b-4345-b5fc-a84599901eb0\",\r\n                    \"path\": \"2DVector\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": true,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"up\",\r\n                    \"id\": \"ab42693a-5337-4253-a901-374797b731a8\",\r\n                    \"path\": \"<Keyboard>/w\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"up\",\r\n                    \"id\": \"9100cd91-4385-44ce-b444-049acab89971\",\r\n                    \"path\": \"<Keyboard>/upArrow\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"down\",\r\n                    \"id\": \"1a3fc74f-34d4-4d15-8d00-f41902b38301\",\r\n                    \"path\": \"<Keyboard>/s\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"down\",\r\n                    \"id\": \"02799145-3d9f-4617-99e1-9f4c756dab42\",\r\n                    \"path\": \"<Keyboard>/downArrow\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"left\",\r\n                    \"id\": \"9a9f7161-7170-4be3-9175-f025e1aa5dcb\",\r\n                    \"path\": \"<Keyboard>/a\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"left\",\r\n                    \"id\": \"a274d8ec-f2c7-4ba5-896d-aa2fa21ba4cd\",\r\n                    \"path\": \"<Keyboard>/leftArrow\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"right\",\r\n                    \"id\": \"df1bf2cd-e07d-4d9e-abfd-c37705be9bf9\",\r\n                    \"path\": \"<Keyboard>/d\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"right\",\r\n                    \"id\": \"a29c04f9-6107-4657-b0d8-494cb6c44855\",\r\n                    \"path\": \"<Keyboard>/rightArrow\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Navigate\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": true\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"3e0bb5a1-ab6c-41ce-8b67-074d40f36eee\",\r\n                    \"path\": \"<Keyboard>/enter\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Submit\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"035740c7-ad18-4d57-9866-bfa844cfdf7b\",\r\n                    \"path\": \"<GXDKGamepad>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Submit\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"d0368032-c734-4018-b1a1-107968c2626a\",\r\n                    \"path\": \"<XInputController>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Submit\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"24852d9b-9bf0-4213-8ea6-cdebee52671f\",\r\n                    \"path\": \"<DualShockGamepad>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Submit\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"4971004a-7e76-45ec-9fd4-56f6c84896e1\",\r\n                    \"path\": \"<AndroidGamepad>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Submit\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"d499d2fa-b980-48c5-9480-5f9c86bbba8d\",\r\n                    \"path\": \"<iOSGameController>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Submit\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"a6291477-3094-432b-8fc6-d6cd5df2b64c\",\r\n                    \"path\": \"<NimbusGamepadHid>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Submit\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"aa3349e4-3a34-40e9-8ac9-40259e6b8d38\",\r\n                    \"path\": \"<SwitchProControllerHID>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Submit\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"249635e5-0f1f-43d2-b0fe-db101499cd5a\",\r\n                    \"path\": \"<NPad>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Submit\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"9178709d-f914-4271-af9d-d721ee100444\",\r\n                    \"path\": \"<SteamDeck>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Submit\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"ed2da905-ed10-428d-9906-5b53c9704334\",\r\n                    \"path\": \"<Keyboard>/escape\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"Cancel\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"48ab6516-4885-487e-a8a5-75a258c746b8\",\r\n                    \"path\": \"<GXDKGamepad>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Cancel\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"135db7f6-2480-4dd1-8be2-864eb8c6ec35\",\r\n                    \"path\": \"<XInputController>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Cancel\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"e8b109be-b60e-4413-930a-4d41233b15aa\",\r\n                    \"path\": \"<DualShockGamepad>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Cancel\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"b366f234-60f1-4abf-bcc0-447c592b6a9f\",\r\n                    \"path\": \"<AndroidGamepad>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Cancel\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"f93980a2-04f9-40ea-aaca-e4d0425d4e27\",\r\n                    \"path\": \"<iOSGameController>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Cancel\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"83a96cc0-b47d-4339-aeaa-d01d4862093a\",\r\n                    \"path\": \"<NimbusGamepadHid>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Cancel\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"33e643be-6245-41f6-b07a-4b7accaf46d0\",\r\n                    \"path\": \"<NPad>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Cancel\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"dea7f382-2de2-4f42-9ec3-9e3d056a4b4a\",\r\n                    \"path\": \"<SwitchProControllerHID>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Cancel\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"954f0a9d-b101-43e5-9423-b2be46632cc1\",\r\n                    \"path\": \"<SteamDeck>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"Cancel\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"2231146d-145d-40d0-a85b-410ae2406823\",\r\n                    \"path\": \"<Mouse>/position\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Point\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"e0ff40fe-b6e6-4f28-a92f-1a758ecc90e3\",\r\n                    \"path\": \"<Pen>/position\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Point\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"bd5c4dd1-b51d-4bc0-b844-29e2456c782c\",\r\n                    \"path\": \"<Touchscreen>/touch*/position\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Point\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"41e2a892-01fc-4bc9-8887-f7dbbc193eea\",\r\n                    \"path\": \"<Mouse>/leftButton\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Click\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"531ae7ae-f7ae-44a2-bd10-97b61ad830d3\",\r\n                    \"path\": \"<Pen>/tip\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Click\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"b338625f-cd20-42d7-933d-67ca52f5585b\",\r\n                    \"path\": \"<Touchscreen>/touch*/press\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Click\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"ce487632-1c36-4273-9e1b-0556b56d7ec7\",\r\n                    \"path\": \"<XRController>/trigger\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"Click\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"845d54dd-920a-4d1c-8b64-6d00d64003eb\",\r\n                    \"path\": \"<Mouse>/scroll\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"ScrollWheel\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"2b3d57bd-3fdc-43e4-aeda-cca54abbe482\",\r\n                    \"path\": \"<Mouse>/middleButton\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"MiddleClick\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"428a924f-c505-47a7-b372-093f86377729\",\r\n                    \"path\": \"<Mouse>/rightButton\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"RightClick\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"ce27cb2c-41e2-4989-ad5b-62c9d34b9059\",\r\n                    \"path\": \"<XRController>/devicePosition\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"TrackedDevicePosition\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"cc7e17ff-c546-4bfb-952d-85bf6598c739\",\r\n                    \"path\": \"<Keyboard>/tab\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"UpgradesBack\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"64df4a33-65bc-4f82-9eea-5b7b0d289140\",\r\n                    \"path\": \"<Keyboard>/q\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AdditionalNavigationLeft\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"cd83d132-15dc-4a66-9cd8-cd36dfd926c3\",\r\n                    \"path\": \"<Gamepad>/leftShoulder\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AdditionalNavigationLeft\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"6b283bcb-043d-49b9-8e8c-49a9be524e4a\",\r\n                    \"path\": \"<Keyboard>/z\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"ExtraAdditionalNavigationLeft\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"da63c737-bc53-4a2e-af41-4a72d1aea09e\",\r\n                    \"path\": \"<Gamepad>/leftTrigger\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"ExtraAdditionalNavigationLeft\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"a82f066b-e1c7-4fb1-bdc2-287941ee08cb\",\r\n                    \"path\": \"<Keyboard>/e\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AdditionalNavigationRight\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"6e551ba7-e87c-4f34-81f3-d04b6ad61533\",\r\n                    \"path\": \"<Gamepad>/rightShoulder\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"AdditionalNavigationRight\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"097ddf1c-6839-4810-98c3-37325730ef96\",\r\n                    \"path\": \"<Keyboard>/c\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"ExtraAdditionalNavigationRight\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"82dec15d-3036-4cc9-a68a-dd4f228ff2f7\",\r\n                    \"path\": \"<Gamepad>/rightTrigger\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"\",\r\n                    \"action\": \"ExtraAdditionalNavigationRight\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"0c8016a2-9d13-4d7f-8f7f-1bfc2700706e\",\r\n                    \"path\": \"<XInputController>/buttonNorth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"HoldApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"128317aa-8f98-4e7c-a170-e5658da7e1a9\",\r\n                    \"path\": \"<GXDKGamepad>/buttonNorth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"HoldApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"c35262db-d3f3-44df-ad4c-a98244c82573\",\r\n                    \"path\": \"<DualShockGamepad>/buttonNorth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"HoldApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"3f6dd93c-73f4-4e3d-b578-8440102912ff\",\r\n                    \"path\": \"<AndroidGamepad>/buttonNorth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"HoldApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"ad51a7b1-8d5a-4e0d-b620-0f1d2a6011d5\",\r\n                    \"path\": \"<iOSGameController>/buttonNorth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"HoldApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"b441d435-a9a7-4443-a9b7-5c2d60cd6a10\",\r\n                    \"path\": \"<NimbusGamepadHid>/buttonNorth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"HoldApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"5f1b0ba9-e6ff-4cfe-bda4-058def190dc6\",\r\n                    \"path\": \"<SwitchProControllerHID>/buttonWest\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"HoldApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"38f903ea-9ae5-4631-a934-d4f78c03396a\",\r\n                    \"path\": \"<NPad>/buttonWest\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"HoldApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"b5e38875-08fa-4a8a-aa86-d7e076e86780\",\r\n                    \"path\": \"<SteamDeck>/buttonNorth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"HoldApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"name\": \"Voice\",\r\n            \"id\": \"f4c3cf4f-a442-4609-afd9-72a7e6bb2c63\",\r\n            \"actions\": [\r\n                {\r\n                    \"name\": \"PushToTalk\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"0a08043a-f1f5-45b4-9b2d-4f167d19f145\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                }\r\n            ],\r\n            \"bindings\": [\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"84218c69-ddf1-4c49-9a5f-827c137df7c4\",\r\n                    \"path\": \"<Keyboard>/v\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \";Keyboard and Mouse\",\r\n                    \"action\": \"PushToTalk\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"name\": \"GameplayInteraction\",\r\n            \"id\": \"1bfbf789-93dd-4d32-a81c-2924d2509f18\",\r\n            \"actions\": [\r\n                {\r\n                    \"name\": \"GameplayApply\",\r\n                    \"type\": \"Button\",\r\n                    \"id\": \"641ab2cf-7f2a-4361-b488-25a6f9cfe282\",\r\n                    \"expectedControlType\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"interactions\": \"\",\r\n                    \"initialStateCheck\": false\r\n                }\r\n            ],\r\n            \"bindings\": [\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"acb88970-3398-4af9-89b8-a95eadd000e3\",\r\n                    \"path\": \"<Keyboard>/space\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Keyboard and Mouse\",\r\n                    \"action\": \"GameplayApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"59a3a2a3-c728-46d8-a16b-1b691104874f\",\r\n                    \"path\": \"<GXDKGamepad>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"GameplayApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"0c608bea-03d7-4f1a-bf03-8298ae125476\",\r\n                    \"path\": \"<XInputController>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"GameplayApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"f79ce604-2c07-481b-8bb0-37b0a2d11d5e\",\r\n                    \"path\": \"<DualShockGamepad>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"GameplayApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"b60b6e17-6b29-4f0a-9cf0-6f68a4ace6a8\",\r\n                    \"path\": \"<AndroidGamepad>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"GameplayApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"9270bcf5-1750-4e76-84f0-895fbfd061f6\",\r\n                    \"path\": \"<iOSGameController>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"GameplayApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"40f6a853-e373-48a3-aae8-f17cf87651fa\",\r\n                    \"path\": \"<NimbusGamepadHid>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"GameplayApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"f43bbdcf-ffba-4b43-9808-a6ed3175b2ef\",\r\n                    \"path\": \"<SwitchProControllerHID>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"GameplayApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"555b9451-fc4e-484e-9228-ae2bf98f0239\",\r\n                    \"path\": \"<NPad>/buttonEast\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"GameplayApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                },\r\n                {\r\n                    \"name\": \"\",\r\n                    \"id\": \"c52a60c2-13a3-4cdd-84e2-a4f3fca7b3db\",\r\n                    \"path\": \"<SteamDeck>/buttonSouth\",\r\n                    \"interactions\": \"\",\r\n                    \"processors\": \"\",\r\n                    \"groups\": \"Touchscreen\",\r\n                    \"action\": \"GameplayApply\",\r\n                    \"isComposite\": false,\r\n                    \"isPartOfComposite\": false\r\n                }\r\n            ]\r\n        }\r\n    ],\r\n    \"controlSchemes\": [\r\n        {\r\n            \"name\": \"Keyboard and Mouse\",\r\n            \"bindingGroup\": \"Keyboard and Mouse\",\r\n            \"devices\": []\r\n        },\r\n        {\r\n            \"name\": \"Touchscreen\",\r\n            \"bindingGroup\": \"Touchscreen\",\r\n            \"devices\": []\r\n        }\r\n    ]\r\n}");
		m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
		m_UI_SpectatorLeft = m_UI.FindAction("SpectatorLeft", throwIfNotFound: true);
		m_UI_SpectatorRight = m_UI.FindAction("SpectatorRight", throwIfNotFound: true);
		m_UI_OpenSettings = m_UI.FindAction("OpenSettings", throwIfNotFound: true);
		m_MovementMap = asset.FindActionMap("MovementMap", throwIfNotFound: true);
		m_MovementMap_Movement = m_MovementMap.FindAction("Movement", throwIfNotFound: true);
		m_MovementMap_Jump = m_MovementMap.FindAction("Jump", throwIfNotFound: true);
		m_MovementMap_Rotation = m_MovementMap.FindAction("Rotation", throwIfNotFound: true);
		m_MovementMap_ChangeMouseVisibility = m_MovementMap.FindAction("ChangeMouseVisibility", throwIfNotFound: true);
		m_MovementMap_Crouch = m_MovementMap.FindAction("Crouch", throwIfNotFound: true);
		m_MovementMap_Sprint = m_MovementMap.FindAction("Sprint", throwIfNotFound: true);
		m_ArmMap = asset.FindActionMap("ArmMap", throwIfNotFound: true);
		m_ArmMap_GrabItem = m_ArmMap.FindAction("GrabItem", throwIfNotFound: true);
		m_ArmMap_DropAllFromArms = m_ArmMap.FindAction("DropAllFromArms", throwIfNotFound: true);
		m_ArmMap_ArmItemDistanceChange = m_ArmMap.FindAction("ArmItemDistanceChange", throwIfNotFound: true);
		m_ArmMap_ItemInteract = m_ArmMap.FindAction("ItemInteract", throwIfNotFound: true);
		m_ArmMap_MouseForward = m_ArmMap.FindAction("MouseForward", throwIfNotFound: true);
		m_ArmMap_MouseBack = m_ArmMap.FindAction("MouseBack", throwIfNotFound: true);
		m_Emotions = asset.FindActionMap("Emotions", throwIfNotFound: true);
		m_Emotions_BodyEmote = m_Emotions.FindAction("BodyEmote", throwIfNotFound: true);
		m_Emotions_FaceEmote = m_Emotions.FindAction("FaceEmote", throwIfNotFound: true);
		m_Emotions_HandEmote = m_Emotions.FindAction("HandEmote", throwIfNotFound: true);
		m_Emotions_EmoteNavigation = m_Emotions.FindAction("EmoteNavigation", throwIfNotFound: true);
		m_HotBar = asset.FindActionMap("HotBar", throwIfNotFound: true);
		m_HotBar_HotBar_1 = m_HotBar.FindAction("HotBar_1", throwIfNotFound: true);
		m_HotBar_HotBar_2 = m_HotBar.FindAction("HotBar_2", throwIfNotFound: true);
		m_HotBar_HotBar_3 = m_HotBar.FindAction("HotBar_3", throwIfNotFound: true);
		m_HotBar_HotBar_4 = m_HotBar.FindAction("HotBar_4", throwIfNotFound: true);
		m_HotBar_HotBar_5 = m_HotBar.FindAction("HotBar_5", throwIfNotFound: true);
		m_HotBar_HotBar_6 = m_HotBar.FindAction("HotBar_6", throwIfNotFound: true);
		m_HotBar_HotBar_7 = m_HotBar.FindAction("HotBar_7", throwIfNotFound: true);
		m_HotBar_HotBar_8 = m_HotBar.FindAction("HotBar_8", throwIfNotFound: true);
		m_HotBar_HotBar_9 = m_HotBar.FindAction("HotBar_9", throwIfNotFound: true);
		m_HotBar_HotBar_0 = m_HotBar.FindAction("HotBar_0", throwIfNotFound: true);
		m_DebugMap = asset.FindActionMap("DebugMap", throwIfNotFound: true);
		m_DebugMap_TurnOnOffOverlay = m_DebugMap.FindAction("TurnOnOffOverlay", throwIfNotFound: true);
		m_AnyVectorValueMap = asset.FindActionMap("AnyVectorValueMap", throwIfNotFound: true);
		m_AnyVectorValueMap_AnyVectorChange = m_AnyVectorValueMap.FindAction("AnyVectorChange", throwIfNotFound: true);
		m_UIMap = asset.FindActionMap("UIMap", throwIfNotFound: true);
		m_UIMap_UIApply = m_UIMap.FindAction("UIApply", throwIfNotFound: true);
		m_UIMap_UIApplyWindow = m_UIMap.FindAction("UIApplyWindow", throwIfNotFound: true);
		m_UIMap_UIBack = m_UIMap.FindAction("UIBack", throwIfNotFound: true);
		m_UIMap_Navigate = m_UIMap.FindAction("Navigate", throwIfNotFound: true);
		m_UIMap_Submit = m_UIMap.FindAction("Submit", throwIfNotFound: true);
		m_UIMap_Cancel = m_UIMap.FindAction("Cancel", throwIfNotFound: true);
		m_UIMap_Point = m_UIMap.FindAction("Point", throwIfNotFound: true);
		m_UIMap_Click = m_UIMap.FindAction("Click", throwIfNotFound: true);
		m_UIMap_ScrollWheel = m_UIMap.FindAction("ScrollWheel", throwIfNotFound: true);
		m_UIMap_MiddleClick = m_UIMap.FindAction("MiddleClick", throwIfNotFound: true);
		m_UIMap_RightClick = m_UIMap.FindAction("RightClick", throwIfNotFound: true);
		m_UIMap_TrackedDevicePosition = m_UIMap.FindAction("TrackedDevicePosition", throwIfNotFound: true);
		m_UIMap_TrackedDeviceOrientation = m_UIMap.FindAction("TrackedDeviceOrientation", throwIfNotFound: true);
		m_UIMap_UpgradesBack = m_UIMap.FindAction("UpgradesBack", throwIfNotFound: true);
		m_UIMap_AdditionalNavigationLeft = m_UIMap.FindAction("AdditionalNavigationLeft", throwIfNotFound: true);
		m_UIMap_AdditionalNavigationRight = m_UIMap.FindAction("AdditionalNavigationRight", throwIfNotFound: true);
		m_UIMap_ExtraAdditionalNavigationLeft = m_UIMap.FindAction("ExtraAdditionalNavigationLeft", throwIfNotFound: true);
		m_UIMap_ExtraAdditionalNavigationRight = m_UIMap.FindAction("ExtraAdditionalNavigationRight", throwIfNotFound: true);
		m_UIMap_HoldApply = m_UIMap.FindAction("HoldApply", throwIfNotFound: true);
		m_Voice = asset.FindActionMap("Voice", throwIfNotFound: true);
		m_Voice_PushToTalk = m_Voice.FindAction("PushToTalk", throwIfNotFound: true);
		m_GameplayInteraction = asset.FindActionMap("GameplayInteraction", throwIfNotFound: true);
		m_GameplayInteraction_GameplayApply = m_GameplayInteraction.FindAction("GameplayApply", throwIfNotFound: true);
	}

	~LocalInputActions()
	{
	}

	public void Dispose()
	{
		UnityEngine.Object.Destroy(asset);
	}

	public bool Contains(InputAction action)
	{
		return asset.Contains(action);
	}

	public IEnumerator<InputAction> GetEnumerator()
	{
		return asset.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Enable()
	{
		asset.Enable();
	}

	public void Disable()
	{
		asset.Disable();
	}

	public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
	{
		return asset.FindAction(actionNameOrId, throwIfNotFound);
	}

	public int FindBinding(InputBinding bindingMask, out InputAction action)
	{
		return asset.FindBinding(bindingMask, out action);
	}
}
