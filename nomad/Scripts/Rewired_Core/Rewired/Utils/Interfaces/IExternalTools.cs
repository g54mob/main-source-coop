using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Utils.Interfaces
{
	public interface IExternalTools
	{
		bool isEditorPaused { get; }

		bool UnityInput_IsTouchPressureSupported { get; }

		event Action<bool> EditorPausedStateChangedEvent;

		event Action<uint, bool> XboxOneInput_OnGamepadStateChange;

		void Destroy();

		object GetPlatformInitializer();

		string GetFocusedEditorWindowTitle();

		bool IsEditorSceneViewFocused();

		bool LinuxInput_IsJoystickPreconfigured(string name);

		int XboxOneInput_GetUserIdForGamepad(uint id);

		ulong XboxOneInput_GetControllerId(uint unityJoystickId);

		bool XboxOneInput_IsGamepadActive(uint unityJoystickId);

		string XboxOneInput_GetControllerType(ulong xboxControllerId);

		uint XboxOneInput_GetJoystickId(ulong xboxControllerId);

		void XboxOne_Gamepad_UpdatePlugin();

		bool XboxOne_Gamepad_SetGamepadVibration(ulong xboxOneJoystickId, float leftMotor, float rightMotor, float leftTriggerLevel, float rightTriggerLevel);

		void XboxOne_Gamepad_PulseVibrateMotor(ulong xboxOneJoystickId, int motor, float startLevel, float endLevel, ulong durationMS);

		void GetDeviceVIDPIDs(out List<int> vids, out List<int> pids);

		int GetAndroidAPILevel();

		void WindowsStandalone_ForwardRawInput(IntPtr rawInputHeaderIndices, IntPtr rawInputDataIndices, uint indicesCount, IntPtr rawInputData, uint rawInputDataSize);

		bool UnityUI_Graphic_GetRaycastTarget(object graphic);

		void UnityUI_Graphic_SetRaycastTarget(object graphic, bool value);

		float UnityInput_GetTouchPressure(ref Touch touch);

		float UnityInput_GetTouchMaximumPossiblePressure(ref Touch touch);

		IControllerTemplate CreateControllerTemplate(Guid typeGuid, object payload);

		Type[] GetControllerTemplateTypes();

		Type[] GetControllerTemplateInterfaceTypes();
	}
}
