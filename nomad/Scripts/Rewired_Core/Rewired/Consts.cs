using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Rewired.Config;
using Rewired.Internal.Localization;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Interfaces;

namespace Rewired
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	public static class Consts
	{
		public const int systemPlayerId = 9999999;

		public const string menuRoot = "Window/Rewired";

		internal const int programVersion1 = 1;

		internal const int programVersion2 = 1;

		internal const int programVersion3 = 59;

		internal const int programVersion4 = 1;

		internal const int dataVersion = 1;

		internal const int unityMajorVersion = 6000;

		internal const string unityMajorVersionIdentifier = "U6000";

		internal const bool isTrial = false;

		internal const string copyrightYear = "2025";

		internal const string defaultNamespace = "Rewired";

		internal const LogLevelFlags defaultLogLevel = LogLevelFlags.Info | LogLevelFlags.Warning | LogLevelFlags.Error;

		internal const bool allowInputWhenEditorPaused = true;

		internal const string hwDefinitionVariantTag_RawInputDirectInput_xboxOneController_splitTriggers = "[SplitTriggers]";

		internal const string hwDefinitionVariantTag_RawInputDirectInput_xboxOneController_combinedTriggers = "[CombinedTriggers]";

		internal const float editorGUIUpdateInterval = 0.5f;

		internal const float joystickRefreshPollCheckTimeout = 1f;

		internal const float controllerRefreshWaitTimeout = 0.5f;

		internal const int buttonsPerHat = 8;

		internal const int keyboardKeyCount = 132;

		internal const int keyboardModifierKeyCount = 8;

		internal const int unityMouseButtonCount = 7;

		internal const int unityMouseAxisCount = 4;

		internal const int unityMaxJoysticks = 16;

		internal const int unityJoystickButtonCount = 20;

		internal const int unityJoystickStartingButtonKeycodeValue = 350;

		internal const int unityJoystickAxisCount = 29;

		internal const int unityJoystickLastJoystickIdWithButtonKeyCodes = 16;

		internal const string unityJoystickPrefix = "Joy";

		internal const string unityJoystickAxisSuffix = "Axis";

		internal const string unityJoystickButtonSuffix = "Button";

		internal const int directInputMaxButtons = 128;

		internal const int directInputMaxAxes = 32;

		internal const int directInputMaxHats = 4;

		internal const int directInputMaxSliders = 2;

		internal const int directInputMaxAxisValue = 65535;

		internal const int directInputMinAxisValue = -65535;

		internal const int directInputMaxHatValue = 36000;

		internal const int directInputHatZeroValue = -1;

		internal const int directInputHatSpan = 4500;

		internal const int directInputHatSpan4Way = 9000;

		internal const int directInput_hatValue_up = 0;

		internal const int directInput_hatValue_right = 9000;

		internal const int directInput_hatValue_down = 18000;

		internal const int directInput_hatValue_left = 27000;

		internal const int directInputLastDirectionValue = 31500;

		internal const int directInputLastDirectionValue4Way = 27000;

		internal const int directInputUnknownJoystickHatCount = 2;

		internal const int directInputUnknownJoystickHatButtonStartIndex = 128;

		internal const int directInputJoystickStateByteSize = 264;

		internal const int rawInputMaxButtons = 256;

		internal const int rawInputMaxAxes = 56;

		internal const int rawInputMaxHats = 4;

		internal const int rawInputMaxSliders = 2;

		internal const int rawInputMaxAxisValue = 65535;

		internal const int rawInputMinAxisValue = -65535;

		internal const int rawInputMaxHatValue = 36000;

		internal const int rawInputHatZeroValue = -1;

		internal const int rawInputHatSpan = 4500;

		internal const int rawInputHatSpan4Way = 9000;

		internal const int rawInput_hatValue_up = 0;

		internal const int rawInput_hatValue_right = 9000;

		internal const int rawInput_hatValue_down = 18000;

		internal const int rawInput_hatValue_left = 27000;

		internal const int rawInputLastDirectionValue = 31500;

		internal const int rawInputLastDirectionValue4Way = 27000;

		internal const int rawInputUnknownJoystickHatCount = 2;

		internal const int rawInputUnknownJoystickHatButtonStartIndex = 128;

		internal const int rawInputUnifiedMouseButtonCount = 5;

		internal const int rawInputUnifiedMouseAxisCount = 4;

		internal const float rawInputUnifiedMouseAxisUnityEquivalencyMultiplier = 0.5f;

		internal const int rawInputUnifiedKeyboardButtonCount = 132;

		internal const int osxMaxSticks = 4;

		internal const int osxMaxButtons = 128;

		internal const int osxMaxAxesPerStick = 14;

		internal const int osxMaxHatsPerStick = 4;

		internal const int osxMaxAxisValue = 65536;

		internal const int osxMinAxisValue = -65536;

		internal const int osxMaxPressureSensitiveButtonValue = 65536;

		internal const int osxMinPressureSensitiveButtonValue = 0;

		internal const int osxMaxHatValue = 36000;

		internal const int osxInputHatZeroValue = -1;

		internal const int osxHatSpan = 4500;

		internal const int osxHatSpan4Way = 9000;

		internal const int osx_hatValue_up = 0;

		internal const int osx_hatValue_right = 9000;

		internal const int osx_hatValue_down = 18000;

		internal const int osx_hatValue_left = 27000;

		internal const int osxLastDirectionValue = 31500;

		internal const int osxLastDirectionValue4Way = 27000;

		internal const int osxUnknownJoystickHatCount = 16;

		internal const int osxUnknownJoystickHatButtonStartIndex = 128;

		internal const int linuxMaxButtons = 256;

		internal const int linuxMaxAxes = 56;

		internal const int linuxMaxHats = 4;

		internal const int linuxMaxSliders = 2;

		internal const int linuxMaxAxisValue = 32767;

		internal const int linuxMinAxisValue = -32768;

		internal const int linuxMaxHatValue = 36000;

		internal const int linuxHatZeroValue = -1;

		internal const int linuxHatSpan = 4500;

		internal const int linuxHatSpan4Way = 9000;

		internal const int linux_hatValue_up = 0;

		internal const int linux_hatValue_right = 9000;

		internal const int linux_hatValue_down = 18000;

		internal const int linux_hatValue_left = 27000;

		internal const int linuxLastDirectionValue = 31500;

		internal const int linuxLastDirectionValue4Way = 27000;

		internal const int linuxUnknownJoystickHatCount = 2;

		internal const int linuxUnknownJoystickHatButtonStartIndex = 128;

		internal const int linuxUnifiedMouseButtonCount = 5;

		internal const int linuxUnifiedMouseAxisCount = 3;

		internal const float linuxUnifiedMouseAxisUnityEquivalencyMultiplier = 0.5f;

		internal const int sdl2MaxButtons = 256;

		internal const int sdl2MaxAxes = 56;

		internal const int sdl2MaxHats = 4;

		internal const int sdl2MaxSliders = 2;

		internal const int sdl2MaxAxisValue = 32768;

		internal const int sdl2MinAxisValue = -32767;

		internal const int sdl2AxisZeroValue = 0;

		internal const int sdl2MaxHatValue = 36000;

		internal const int sdl2HatZeroValue = -1;

		internal const int sdl2HatSpan = 4500;

		internal const int sdl2HatSpan4Way = 9000;

		internal const int sdl2_hatValue_up = 0;

		internal const int sdl2_hatValue_right = 9000;

		internal const int sdl2_hatValue_down = 18000;

		internal const int sdl2_hatValue_left = 27000;

		internal const int sdl2LastDirectionValue = 31500;

		internal const int sdl2LastDirectionValue4Way = 27000;

		internal const int sdl2UnknownJoystickHatCount = 2;

		internal const int sdl2UnknownJoystickHatButtonStartIndex = 128;

		internal const int sdl2UnifiedMouseButtonCount = 5;

		internal const int sdl2UnifiedMouseAxisCount = 3;

		internal const float sdl2UnifiedMouseAxisUnityEquivalencyMultiplier = 0.5f;

		internal const int windowsUWPMaxButtons = 256;

		internal const int windowsUWPMaxAxes = 56;

		internal const int windowsUWPMaxHats = 4;

		internal const int windowsUWPMaxSliders = 2;

		internal const int windowsUWPMaxAxisValue = 32767;

		internal const int windowsUWPMinAxisValue = -32768;

		internal const int windowsUWPMaxHatValue = 36000;

		internal const int windowsUWPHatZeroValue = -1;

		internal const int windowsUWPDirectionsPerHat = 8;

		internal const int windowsUWPHatSpan = 4500;

		internal const int windowsUWPHatSpan4Way = 9000;

		internal const int windowsUWPLastDirectionValue = 31500;

		internal const int windowsUWPLastDirectionValue4Way = 27000;

		internal const int windowsUWPUnknownJoystickHatCount = 2;

		internal const int windowsUWPUnknownJoystickHatButtonStartIndex = 128;

		internal const int windowsUWPUnifiedMouseButtonCount = 5;

		internal const int windowsUWPUnifiedMouseAxisCount = 3;

		internal const float windowsUWPUnifiedMouseAxisUnityEquivalencyMultiplier = 0.5f;

		internal const int windowsGamingInputHatZeroValue = -1;

		internal const int xInputMaxVibration = 65535;

		internal const int xInputMinVibration = 0;

		internal const float xInputAllowedVibrationInterval = 0.01f;

		internal const int customPlatformMaxButtons = 256;

		internal const int customPlatformMaxAxes = 128;

		internal const int internalDriverMaxButtons = 256;

		internal const int internalDriverMaxAxes = 56;

		internal const int internalDriverMaxHats = 4;

		internal const int internalDriverMaxSliders = 2;

		internal const int internalDriverMaxAxisValue = 65535;

		internal const int internalDriverMinAxisValue = -65535;

		internal const int internalDriverMaxHatValue = 36000;

		internal const int internalDriverHatZeroValue = -1;

		internal const int internalDriverHatSpan = 4500;

		internal const int internalDriverHatSpan4Way = 9000;

		internal const int internalDriver_hatValue_up = 0;

		internal const int internalDriver_hatValue_right = 9000;

		internal const int internalDriver_hatValue_down = 18000;

		internal const int internalDriver_hatValue_left = 27000;

		internal const int internalDriverLastDirectionValue = 31500;

		internal const int internalDriverLastDirectionValue4Way = 27000;

		internal const int internalDriverUnknownJoystickHatCount = 2;

		internal const int internalDriverUnknownJoystickHatButtonStartIndex = 128;

		internal const int internalDriverUnifiedMouseButtonCount = 5;

		internal const int internalDriverUnifiedMouseAxisCount = 3;

		internal const float internalDriverUnifiedMouseAxisUnityEquivalencyMultiplier = 0.5f;

		internal const int webGLMaxButtons = 256;

		internal const int webGLMaxAxes = 128;

		internal const int gameCoreMaxButtons = 256;

		internal const int gameCoreMaxAxes = 32;

		internal const int gameCoreMaxHats = 4;

		internal const int gameCoreUnknownJoystickButtonCount = 128;

		internal const int gameCoreUnknownJoystickAxisCount = 32;

		internal const int gameCoreUnknownJoystickHatCount = 2;

		internal const int appleGCControllerMaxButtons = 128;

		internal const int appleGCControllerMaxAxes = 32;

		internal const int appleGCControllerMaxCompoundElements = 32;

		internal const int appleGCControllerUnknownJoystickButtonCount = 128;

		internal const int appleGCControllerUnknownJoystickAxisCount = 32;

		internal const int windowsGamingInputMaxButtons = 128;

		internal const int windowsGamingInputMaxAxes = 32;

		internal const int windowsGamingInputMaxHats = 16;

		internal const int windowsGamingInputMaxCompoundElements = 32;

		internal const int windowsGamingInputUnknownJoystickButtonCount = 128;

		internal const int windowsGamingInputUnknownJoystickAxisCount = 32;

		internal const int windowsGamingInputUnknownJoystickHatCount = 16;

		internal const int unknownJoystickMaxButtons = 128;

		internal const int unknownJoystickMaxAxes = 32;

		internal const int unknownJoystickMaxHats = 16;

		internal const int unknownJoystickButtonsPerHat = 8;

		internal const int unknownJoystickAxisElementIdentifierStartIndex = 0;

		internal const int unknownJoystickButtonElementIdentifierStartIndex = 32;

		internal const int unknownJoystickHatElementIdentifierStartIndex = 160;

		internal const float unknownJoystickDefaultAxisDeadZone = 0.1f;

		internal const float defaultAbsoluteAxisPollingDeadZone = 0.7f;

		internal const float defaultRelativeAxisPollingDeadZone = 100f;

		internal const float defaultMouseXYAxisPollingDeadzone = 100f;

		internal const float defaultMouseOtherAxisPollingDeadzone = 2f;

		internal const float defaultButtonDeadZone = 0.5f;

		internal const float hardwareButtonDeadZone = 0.01f;

		internal const float axisDefaultSensitivity = 1f;

		internal const AxisSensitivityType axisDefaultSensitivityType = AxisSensitivityType.Multiplier;

		internal const float defaultButtonDoublePressSpeed = 0.3f;

		internal const float minDoubleButtonPressSpeed = 0.01f;

		internal const float maxDoubleButtonPressSpeed = 10f;

		internal const float defaultButtonShortPressTime = 0.25f;

		internal const float minButtonShortPressTime = 0f;

		internal const float maxButtonShortPressTime = 3.4028235E+38f;

		internal const float defaultButtonShortPressExpiresIn = 0f;

		internal const float minButtonShortPressExpiresIn = 0f;

		internal const float maxButtonShortPressExpiresIn = 3.4028235E+38f;

		internal const float defaultButtonLongPressTime = 1f;

		internal const float minButtonLongPressTime = 0f;

		internal const float maxButtonLongPressTime = 3.4028235E+38f;

		internal const float defaultButtonLongPressExpiresIn = 0f;

		internal const float minButtonLongPressExpiresIn = 0f;

		internal const float maxButtonLongPressExpiresIn = 3.4028235E+38f;

		internal const float defaultButtonRepeatDelay = 0f;

		internal const float defaultButtonRepeatRate = 30f;

		internal const float minButtonRepeatRate = 0.001f;

		internal const float mouseAxisPollingTimerLength = 1f;

		internal const float relativeAxisPollingTimerLength = 1f;

		internal const float fallbackPollingTimeout = 1f;

		internal const KeyCombinationOverrideMode defaultKeyCombinationOverrideMode = KeyCombinationOverrideMode.Cancel;

		internal const bool defaultGenerateKeyEventsOnKeyCombinationOverride = true;

		internal const string unknownJoystickName = "Unknown Controller";

		internal const float xInputControllerVibrationRenewalInterval = 1.5f;

		internal const int defaultInputThreadUpdateRateFPS = 240;

		internal const int maxInputThreadUpdateRateFPS = 2000;

		internal const int osxXInputOutputReportRefreshRateFPS = 60;

		internal const int defaultOutputRefreshRateFPS = 100;

		internal const int hidOutputReportRefreshRateFPS = 100;

		internal const int hidOutputReportThreadKillTimeout = 10000;

		internal const int joystickInputReportRingBufferCapacity = 60;

		internal const float joystickInputReportRingBufferCapacityDuration = 0.25f;

		internal const string resourecesDLLPath_windowsStandalone = "Libs/Rewired_Windows";

		internal const string resourecesDLLPath_osxStandalone = "Libs/Rewired_OSX";

		internal const string resourecesDLLPath_linux = "Libs/Rewired_Linux";

		internal const float defaultInputBehaviorAxisSensitivity = 1f;

		internal const float defaultInputBehaviorAxisSimulation_gravity = 3f;

		internal const float defaultInputBehaviorAxisSimulation_sensitivity = 3f;

		internal const bool defaultInputBehaviorAxisSmoothing_snap = true;

		internal const bool defaultInputBehaviorAxisSmoothing_instantReverse = false;

		internal const bool defaultInputBehaviorAxisSimulation_enabled = false;

		internal const int allFlagsIntEnum = -1;

		internal const float osxPreventSystemSleepInterval = 30f;

		internal const string schemaNameSpace = "http://guavaman.com/rewired";

		internal const string schemaBaseLocation = "http://guavaman.com/schemas/rewired/";

		internal const string schemaVersionControllerMap = "1.1";

		internal const string schemaVersionCalibrationMap = "1.3";

		internal const string schemaVersionInputBehavior = "1.4";

		internal const string schemaVersionControllerTemplateMap = "1.0";

		internal const string schemaVersionPlayerEnabledMapsHelperData = "1.0";

		internal const string schemaVersionPlayerControllerMapLayoutManagerData = "1.0";

		internal const int controllerMapDataVersion = 2;

		internal const int calibrationMapDataVersion = 4;

		internal const int inputBehaviorDataVersion = 5;

		internal const int controllerTemplateMapDataVersion = 1;

		internal const int playerMapEnablerDataVersion = 1;

		internal const int playerControllerMapLayoutManagerDataVersion = 1;

		internal static readonly PidVid[] questionablePidVids;

		internal static readonly int[] questionableVIDs;

		internal const int controllerElementType_trueElements_minValue = 0;

		internal const int controllerElementType_trueElements_maxValue = 99;

		internal const float pressureSensitiveButtonDeadZone = 0.001f;

		internal const string rewiredEditorAssembly = "Rewired_Editor";

		internal const string rewiredEditorInputEditorClassFullName = "Rewired.Editor.InputEditor";

		internal const string nintendoSwitchPluginEditorRuntimeAssembly = "Rewired_NintendoSwitch_EditorRuntime";

		internal const string nintendoSwitch2PluginEditorRuntimeAssembly = "Rewired_NintendoSwitch2_EditorRuntime";

		internal const string nintendoSwitchPluginInputManagerFullClassPath = "Rewired.Platforms.Switch.NintendoSwitchInputManager";

		internal const string nintendoSwitch2PluginInputManagerFullClassPath = "Rewired.Platforms.Switch2.NintendoSwitch2InputManager";

		internal const string nintendoSwitchPluginHWJoystickMapGuid_JoyConDual = "521b808c-0248-4526-bc10-f1d16ee76bf1";

		internal const string nintendoSwitchPluginHWJoystickMapGuid_Handheld = "1fbdd13b-0795-4173-8a95-a2a75de9d204";

		internal const string nintendoSwitch2PluginHWJoystickMapGuid_JoyConDual = "b5cb8488-8551-41c2-944a-64dfcf74b4c7";

		internal const string nintendoSwitch2PluginHWJoystickMapGuid_Handheld = "2560014c-e7a5-4675-bc63-1b46337b12cb";

		internal const string gameCorePluginEditorRuntimeAssembly = "Rewired_GameCore_EditorRuntime";

		internal const string gameCorePluginInputManagerFullClassPath = "Rewired.Platforms.GameCore.GameCoreInputManager";

		internal const string ps4PluginEditorRuntimeAssembly = "Rewired_PlayStation4_EditorRuntime";

		internal const string ps5PluginEditorRuntimeAssembly = "Rewired_PlayStation5_EditorRuntime";

		internal static Guid joystickGuid_unknownController;

		internal static Guid joystickGuid_appleMFiController;

		internal static Guid joystickGuid_standardizedGamepad;

		internal static Guid joystickGuid_steamController;

		internal static Guid joystickGuid_SonyDualShock4;

		internal static Guid joystickGuid_SonyPS4AimController;

		internal static Guid joystickGuid_SonyPS4Drums;

		internal static Guid joystickGuid_SonyPS4FlightStick;

		internal static Guid joystickGuid_SonyPS4Guitar;

		internal static Guid joystickGuid_SonyPS4SteeringWheel;

		internal static Guid joystickGuid_SonyDualSense;

		internal static Guid joystickGuid_NintendoSwitchHandheld;

		internal static Guid joystickGuid_NintendoSwitchJoyConDual;

		internal static Guid joystickGuid_NintendoSwitchJoyConL;

		internal static Guid joystickGuid_NintendoSwitchJoyConR;

		internal static Guid hardwareTypeGuid_universalKeyboard;

		internal static Guid hardwareTypeGuid_universalMouse;

		private static readonly Guid[] niPbzofDXUNfiYmAOnvTdsJMTuKx;

		internal static readonly ReadOnlyCollection<Guid> reservedHardwareTypeGuids;

		private static ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA[] lqoFlrhtEKuvqHyUvoWxtDzeqfKu;

		private static ReadOnlyCollection<ControllerElementIdentifier> COsILmmlDuCguIazUvYxjOiNkQRs;

		private static ReadOnlyCollection<ControllerElementIdentifier> vjVTHDUzwTVwBfOJzHgIqalZsMhF;

		internal static readonly IList<string> mouseAxisUnityNames;

		private static readonly string[] nRUtFnTSFxWrzrmOaFNjqQDLTxeN;

		internal static readonly IList<string> mouseButtonUnityNames;

		private static readonly string[] cABVxOTCWykdyFpklwXWwYCyWrlU;

		internal static readonly IList<string> keyboardKeyNames;

		private static readonly string[] ebtRoaiZzFbPCsDMgSqBOYreSvfn;

		internal static readonly IList<int> keyboardKeyValues;

		internal static readonly int[] _keyboardKeyValues;

		private static readonly IList<string> FdjDujnrAcRFxcCjBExPEtrJpfUhb;

		private static readonly string[] HGUhbunBUkfjLfkEXJemYuTdpcKTA;

		private static readonly IList<string> eZzmJkFSkFBLxafGPVyIfKrfhRww;

		private static readonly string[] bumjtwRanhXtGNjZxmbHQDUFniCb;

		internal static readonly Rewired.Utils.Interfaces.IReadOnlyDictionary<int, Keyboard.ModifierKeyInfo> modifierKeyInfo;

		public const int vendorId_sony = 1356;

		internal static readonly IList<PidVid> pidVids_sony_dualShock4;

		private static readonly PidVid[] xCUyxrlMTTmOxlimfimfcpENsMAIb;

		internal static readonly IList<string> productNames_sony_dualShock4;

		private static readonly string[] cLYzZvQJpRNKdpmtlEEjkSFWFTXDA;

		internal static readonly IList<PidVid> pidVids_sony_dualSense;

		private static readonly PidVid[] EDaVlyWQVLjYIIQmaaYUDpfiNKWh;

		internal static readonly IList<string> productNames_sony_dualSense;

		private static readonly string[] gcGuuJOjibFEMFLiNYksAWOpvXEX;

		private static readonly ControllerElementIdentifier[] noqaJHGuZeynOgjhzqRNFTetPjkvA;

		internal const int updateLoopTypeCount = 3;

		internal static int nintendoSwitchPlugin_minPluginVersion => 22;

		internal static int nintendoSwitch2Plugin_minPluginVersion => 1;

		internal static int gameCorePlugin_minPluginVersion => 1;

		internal static int ps4Plugin_minPluginVersion => 1;

		internal static int ps5Plugin_minPluginVersion => 1;

		internal static ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA[] commonMouseElementIdentifierInitOptions
		{
			get
			{
				if (lqoFlrhtEKuvqHyUvoWxtDzeqfKu != null)
				{
					return lqoFlrhtEKuvqHyUvoWxtDzeqfKu;
				}
				lqoFlrhtEKuvqHyUvoWxtDzeqfKu = new ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA[11]
				{
					new ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA
					{
						id = 0,
						name = "Mouse Horizontal",
						positiveName = "Mouse Right",
						negativeName = "Mouse Left",
						key = "move/horizontal",
						positiveKey = "move/right",
						negativeKey = "move/left",
						elementType = ControllerElementType.Axis,
						compoundElementType = CompoundControllerElementType.Axis2D
					},
					new ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA
					{
						id = 1,
						name = "Mouse Vertical",
						positiveName = "Mouse Up",
						negativeName = "Mouse Down",
						key = "move/vertical",
						positiveKey = "move/up",
						negativeKey = "move/down",
						elementType = ControllerElementType.Axis,
						compoundElementType = CompoundControllerElementType.Axis2D
					},
					new ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA
					{
						id = 2,
						name = "Mouse Wheel",
						positiveName = "Mouse Wheel Up",
						negativeName = "Mouse Wheel Down",
						key = "wheel/vertical",
						positiveKey = "wheel/up",
						negativeKey = "wheel/down",
						elementType = ControllerElementType.Axis,
						compoundElementType = CompoundControllerElementType.Axis2D
					},
					new ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA
					{
						id = 3,
						name = "Left Mouse Button",
						positiveName = string.Empty,
						negativeName = string.Empty,
						key = "left_button",
						positiveKey = string.Empty,
						negativeKey = string.Empty,
						elementType = ControllerElementType.Button,
						compoundElementType = CompoundControllerElementType.Axis2D
					},
					new ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA
					{
						id = 4,
						name = "Right Mouse Button",
						positiveName = string.Empty,
						negativeName = string.Empty,
						key = "right_button",
						positiveKey = string.Empty,
						negativeKey = string.Empty,
						elementType = ControllerElementType.Button,
						compoundElementType = CompoundControllerElementType.Axis2D
					},
					new ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA
					{
						id = 5,
						name = "Mouse Button 3",
						positiveName = string.Empty,
						negativeName = string.Empty,
						key = "middle_button",
						positiveKey = string.Empty,
						negativeKey = string.Empty,
						elementType = ControllerElementType.Button,
						compoundElementType = CompoundControllerElementType.Axis2D
					},
					new ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA
					{
						id = 6,
						name = "Mouse Button 4",
						positiveName = string.Empty,
						negativeName = string.Empty,
						key = "button_4",
						positiveKey = string.Empty,
						negativeKey = string.Empty,
						elementType = ControllerElementType.Button,
						compoundElementType = CompoundControllerElementType.Axis2D
					},
					new ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA
					{
						id = 7,
						name = "Mouse Button 5",
						positiveName = string.Empty,
						negativeName = string.Empty,
						key = "button_5",
						positiveKey = string.Empty,
						negativeKey = string.Empty,
						elementType = ControllerElementType.Button,
						compoundElementType = CompoundControllerElementType.Axis2D
					},
					new ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA
					{
						id = 8,
						name = "Mouse Button 6",
						positiveName = string.Empty,
						negativeName = string.Empty,
						key = "button_6",
						positiveKey = string.Empty,
						negativeKey = string.Empty,
						elementType = ControllerElementType.Button,
						compoundElementType = CompoundControllerElementType.Axis2D
					},
					new ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA
					{
						id = 9,
						name = "Mouse Button 7",
						positiveName = string.Empty,
						negativeName = string.Empty,
						key = "button_7",
						positiveKey = string.Empty,
						negativeKey = string.Empty,
						elementType = ControllerElementType.Button,
						compoundElementType = CompoundControllerElementType.Axis2D
					},
					new ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA
					{
						id = 10,
						name = "Mouse Wheel Horizontal",
						positiveName = "Mouse Wheel Right",
						negativeName = "Mouse Wheel Left",
						key = "wheel/horizontal",
						positiveKey = "wheel/right",
						negativeKey = "wheel/left",
						elementType = ControllerElementType.Axis,
						compoundElementType = CompoundControllerElementType.Axis2D
					}
				};
				return lqoFlrhtEKuvqHyUvoWxtDzeqfKu;
			}
		}

		internal static IList<ControllerElementIdentifier> unityUnifiedMouseElementIdentifiers => COsILmmlDuCguIazUvYxjOiNkQRs ?? (COsILmmlDuCguIazUvYxjOiNkQRs = new ReadOnlyCollection<ControllerElementIdentifier>(new ControllerElementIdentifier[11]
		{
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[0]),
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[1]),
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[2]),
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[10]),
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[3]),
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[4]),
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[5]),
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[6]),
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[7]),
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[8]),
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[9])
		}));

		internal static IList<ControllerElementIdentifier> rawInputUnifiedMouseElementIdentifiers => vjVTHDUzwTVwBfOJzHgIqalZsMhF ?? (vjVTHDUzwTVwBfOJzHgIqalZsMhF = new ReadOnlyCollection<ControllerElementIdentifier>(new ControllerElementIdentifier[9]
		{
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[0]),
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[1]),
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[2]),
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[3]),
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[4]),
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[5]),
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[6]),
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[7]),
			new ControllerElementIdentifier(commonMouseElementIdentifierInitOptions[10])
		}));

		public static IList<string> keyboardKeyKeys => FdjDujnrAcRFxcCjBExPEtrJpfUhb;

		public static IList<string> keyboardModifierKeyKeys => eZzmJkFSkFBLxafGPVyIfKrfhRww;

		internal static ControllerElementIdentifier[] unknownJoystickElementIdentifiers_orig => noqaJHGuZeynOgjhzqRNFTetPjkvA;

		static Consts()
		{
			joystickGuid_unknownController = Guid.Empty;
			joystickGuid_appleMFiController = new Guid("3d919cfa-468e-49f4-bce9-f6c43f2e7e62");
			joystickGuid_standardizedGamepad = new Guid("04c23ab3-2b99-4404-a5c4-f0df7e62938f");
			joystickGuid_steamController = new Guid("2694f4b9-9d84-4f55-9ee8-78fbba744b7d");
			joystickGuid_SonyDualShock4 = new Guid("cd9718bf-a87a-44bc-8716-60a0def28a9f");
			joystickGuid_SonyPS4AimController = new Guid("65ea105c-6390-4d11-a49b-13a402b1f2d9");
			joystickGuid_SonyPS4Drums = new Guid("7c338d42-ec21-4402-84ed-7ab547343c19");
			joystickGuid_SonyPS4FlightStick = new Guid("a75d195b-27a8-41ac-97db-2bd5a649a817");
			joystickGuid_SonyPS4Guitar = new Guid("274096a0-b4d5-413f-bb4c-7dd68cae6f0f");
			joystickGuid_SonyPS4SteeringWheel = new Guid("1b5a521b-6833-4c54-ab6c-bac653c93e9c");
			joystickGuid_SonyDualSense = new Guid("5286706d-19b4-4a45-b635-207ce78d8394");
			joystickGuid_NintendoSwitchHandheld = new Guid("1fbdd13b-0795-4173-8a95-a2a75de9d204");
			joystickGuid_NintendoSwitchJoyConDual = new Guid("521b808c-0248-4526-bc10-f1d16ee76bf1");
			joystickGuid_NintendoSwitchJoyConL = new Guid("3eb01142-da0e-4a86-8ae8-a15c2b1f2a04");
			joystickGuid_NintendoSwitchJoyConR = new Guid("605dc720-1b38-473d-a459-67d5857aa6ea");
			hardwareTypeGuid_universalKeyboard = new Guid("ae4830f963db4d4c90b31beb46ecaf49");
			hardwareTypeGuid_universalMouse = new Guid("ad60107cea394d9cb90656d39d07be95");
			niPbzofDXUNfiYmAOnvTdsJMTuKx = new Guid[3] { joystickGuid_unknownController, hardwareTypeGuid_universalKeyboard, hardwareTypeGuid_universalMouse };
			nRUtFnTSFxWrzrmOaFNjqQDLTxeN = new string[3] { "MouseAxis1", "MouseAxis2", "MouseAxis3" };
			cABVxOTCWykdyFpklwXWwYCyWrlU = new string[7] { "MouseButton0", "MouseButton1", "MouseButton2", "MouseButton3", "MouseButton4", "MouseButton5", "MouseButton6" };
			ebtRoaiZzFbPCsDMgSqBOYreSvfn = new string[132]
			{
				"None", "A", "B", "C", "D", "E", "F", "G", "H", "I",
				"J", "K", "L", "M", "N", "O", "P", "Q", "R", "S",
				"T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2",
				"3", "4", "5", "6", "7", "8", "9", "Keypad 0", "Keypad 1", "Keypad 2",
				"Keypad 3", "Keypad 4", "Keypad 5", "Keypad 6", "Keypad 7", "Keypad 8", "Keypad 9", "Keypad .", "Keypad /", "Keypad *",
				"Keypad -", "Keypad +", "Keypad Enter", "Keypad =", "Space", "Backspace", "Tab", "Clear", "Return", "Pause",
				"ESC", "!", "\"", "#", "$", "&", "'", "(", ")", "*",
				"+", ",", "-", ".", "/", ":", ";", "<", "=", ">",
				"?", "@", "[", "\\", "]", "^", "_", "Back Quote", "Delete", "Up Arrow",
				"Down Arrow", "Right Arrow", "Left Arrow", "Insert", "Home", "End", "Page Up", "Page Down", "F1", "F2",
				"F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12",
				"F13", "F14", "F15", "Numlock", "Caps Lock", "Scroll Lock", "Right Shift", "Left Shift", "Right Control", "Left Control",
				"Right Alt", "Left Alt", "Right Command", "Left Command", "Left Windows", "Right Windows", "AltGr", "Help", "Print", "SysReq",
				"Break", "Menu"
			};
			_keyboardKeyValues = new int[132]
			{
				0, 97, 98, 99, 100, 101, 102, 103, 104, 105,
				106, 107, 108, 109, 110, 111, 112, 113, 114, 115,
				116, 117, 118, 119, 120, 121, 122, 48, 49, 50,
				51, 52, 53, 54, 55, 56, 57, 256, 257, 258,
				259, 260, 261, 262, 263, 264, 265, 266, 267, 268,
				269, 270, 271, 272, 32, 8, 9, 12, 13, 19,
				27, 33, 34, 35, 36, 38, 39, 40, 41, 42,
				43, 44, 45, 46, 47, 58, 59, 60, 61, 62,
				63, 64, 91, 92, 93, 94, 95, 96, 127, 273,
				274, 275, 276, 277, 278, 279, 280, 281, 282, 283,
				284, 285, 286, 287, 288, 289, 290, 291, 292, 293,
				294, 295, 296, 300, 301, 302, 303, 304, 305, 306,
				307, 308, 309, 310, 311, 312, 313, 315, 316, 317,
				318, 319
			};
			HGUhbunBUkfjLfkEXJemYuTdpcKTA = new string[132]
			{
				"", "a", "b", "c", "d", "e", "f", "g", "h", "i",
				"j", "k", "l", "m", "n", "o", "p", "q", "r", "s",
				"t", "u", "v", "w", "x", "y", "z", "alpha_0", "alpha_1", "alpha_2",
				"alpha_3", "alpha_4", "alpha_5", "alpha_6", "alpha_7", "alpha_8", "alpha_9", "keypad_0", "keypad_1", "keypad_2",
				"keypad_3", "keypad_4", "keypad_5", "keypad_6", "keypad_7", "keypad_8", "keypad_9", "keypad_period", "keypad_slash", "keypad_asterisk",
				"keypad_minus", "keypad_plus", "keypad_enter", "keypad_equals", "space", "backspace", "tab", "clear", "return", "pause",
				"escape", "exclamation_point", "double_quote", "hash", "dollar_sign", "ampersand", "quote", "left_parenthesis", "right_parenthesis", "asterisk",
				"plus", "comma", "minus", "period", "slash", "colon", "semicolon", "less_than", "equals", "greater_than",
				"question_mark", "at", "left_bracket", "backslash", "right_bracket", "caret", "underscore", "back_quote", "delete", "up_arrow",
				"down_arrow", "right_arrow", "left_arrow", "insert", "home", "end", "page_up", "page_down", "f1", "f2",
				"f3", "f4", "f5", "f6", "f7", "f8", "f9", "f10", "f11", "f12",
				"f13", "f14", "f15", "num_lock", "caps_lock", "scroll_lock", "right_shift", "left_shift", "right_control", "left_control",
				"right_alt", "left_alt", "right_command", "left_command", "left_windows", "right_windows", "alt_gr", "help", "print_screen", "sys_req",
				"break", "menu"
			};
			bumjtwRanhXtGNjZxmbHQDUFniCb = new string[5] { "", "control", "alt", "shift", "command" };
			modifierKeyInfo = new ADictionary<int, Keyboard.ModifierKeyInfo>
			{
				{
					0,
					new Keyboard.ModifierKeyInfo(string.Empty, string.Empty, string.Empty, string.Empty)
				},
				{
					1,
					new Keyboard.ModifierKeyInfo("Ctrl", "Control", "control_short", "control")
				},
				{
					2,
					new Keyboard.ModifierKeyInfo("Alt", "Alt", "alt_short", "alt")
				},
				{
					3,
					new Keyboard.ModifierKeyInfo("Shift", "Shift", "shift_short", "shift")
				},
				{
					4,
					new Keyboard.ModifierKeyInfo("Cmd", "Command", "command_short", "command")
				}
			};
			xCUyxrlMTTmOxlimfimfcpENsMAIb = new PidVid[3]
			{
				new PidVid(1476, 1356),
				new PidVid(2976, 1356),
				new PidVid(2508, 1356)
			};
			cLYzZvQJpRNKdpmtlEEjkSFWFTXDA = new string[5] { "Sony Computer Entertainment Wireless Controller", "Sony Interactive Entertainment DUALSHOCK®4 USB Wireless Adaptor", "Wireless Controller", "Sony Interactive Entertainment Wireless Controller", "Wireless Controller Touchpad" };
			EDaVlyWQVLjYIIQmaaYUDpfiNKWh = new PidVid[2]
			{
				new PidVid(3302, 1356),
				new PidVid(3570, 1356)
			};
			gcGuuJOjibFEMFLiNYksAWOpvXEX = new string[1] { "DualSense Wireless Controller" };
			mouseAxisUnityNames = new ReadOnlyCollection<string>(nRUtFnTSFxWrzrmOaFNjqQDLTxeN);
			mouseButtonUnityNames = new ReadOnlyCollection<string>(cABVxOTCWykdyFpklwXWwYCyWrlU);
			keyboardKeyNames = new ReadOnlyCollection<string>(ebtRoaiZzFbPCsDMgSqBOYreSvfn);
			keyboardKeyValues = new ReadOnlyCollection<int>(_keyboardKeyValues);
			FdjDujnrAcRFxcCjBExPEtrJpfUhb = new ReadOnlyCollection<string>(HGUhbunBUkfjLfkEXJemYuTdpcKTA);
			eZzmJkFSkFBLxafGPVyIfKrfhRww = new ReadOnlyCollection<string>(bumjtwRanhXtGNjZxmbHQDUFniCb);
			pidVids_sony_dualShock4 = new ReadOnlyCollection<PidVid>(xCUyxrlMTTmOxlimfimfcpENsMAIb);
			productNames_sony_dualShock4 = new ReadOnlyCollection<string>(cLYzZvQJpRNKdpmtlEEjkSFWFTXDA);
			pidVids_sony_dualSense = new ReadOnlyCollection<PidVid>(EDaVlyWQVLjYIIQmaaYUDpfiNKWh);
			productNames_sony_dualSense = new ReadOnlyCollection<string>(gcGuuJOjibFEMFLiNYksAWOpvXEX);
			reservedHardwareTypeGuids = new ReadOnlyCollection<Guid>(niPbzofDXUNfiYmAOnvTdsJMTuKx);
			questionablePidVids = new PidVid[4]
			{
				new PidVid(0, 0),
				new PidVid(0, 65535),
				new PidVid(65535, 65535),
				new PidVid(65535, 0)
			};
			questionableVIDs = new int[2] { 0, 65535 };
			noqaJHGuZeynOgjhzqRNFTetPjkvA = QVIDkGjKPagdforjIbtNxWLsoqUA();
			if (132 != ebtRoaiZzFbPCsDMgSqBOYreSvfn.Length)
			{
				Logger.LogError("Consts.keyboardKeyCount does not match _keyboardKeyNames.Length!");
			}
			if (132 != _keyboardKeyValues.Length)
			{
				Logger.LogError("Consts.keyboardKeyCount does not match _keyboardKeyValues.Length!");
			}
			if (132 != HGUhbunBUkfjLfkEXJemYuTdpcKTA.Length)
			{
				Logger.LogError("Consts.keyboardKeyCount does not match _keyboardKeyStringKeys.Length!");
			}
		}

		private static ControllerElementIdentifier[] QVIDkGjKPagdforjIbtNxWLsoqUA()
		{
			int num = 0;
			List<ControllerElementIdentifier> list = new List<ControllerElementIdentifier>(288);
			ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA lVegNbJXhHfEVYqBGHidPgoEtALWA = new ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA();
			for (int i = 0; i < 32; i++)
			{
				lVegNbJXhHfEVYqBGHidPgoEtALWA.id = num;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.name = "Axis " + i;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.positiveName = string.Empty;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.negativeName = string.Empty;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.key = LocalizationManager.ConcatenateKeyStrings("axis", i.ToString());
				lVegNbJXhHfEVYqBGHidPgoEtALWA.positiveKey = string.Empty;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.negativeKey = string.Empty;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.elementType = ControllerElementType.Axis;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.compoundElementType = CompoundControllerElementType.Axis2D;
				list.Add(new ControllerElementIdentifier(lVegNbJXhHfEVYqBGHidPgoEtALWA));
				num++;
			}
			for (int j = 0; j < 128; j++)
			{
				lVegNbJXhHfEVYqBGHidPgoEtALWA.id = num;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.name = "Button " + j;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.positiveName = string.Empty;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.negativeName = string.Empty;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.key = LocalizationManager.ConcatenateKeyStrings("button", j.ToString());
				lVegNbJXhHfEVYqBGHidPgoEtALWA.positiveKey = string.Empty;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.negativeKey = string.Empty;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.elementType = ControllerElementType.Button;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.compoundElementType = CompoundControllerElementType.Axis2D;
				list.Add(new ControllerElementIdentifier(lVegNbJXhHfEVYqBGHidPgoEtALWA));
				num++;
			}
			int num2 = num;
			int num3 = num + 64;
			lVegNbJXhHfEVYqBGHidPgoEtALWA = new ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA();
			lVegNbJXhHfEVYqBGHidPgoEtALWA.elementType = ControllerElementType.Button;
			lVegNbJXhHfEVYqBGHidPgoEtALWA.compoundElementType = CompoundControllerElementType.Axis2D;
			for (int k = 0; k < 16; k++)
			{
				string a = LocalizationManager.ConcatenateKeyStrings("hat", k.ToString());
				lVegNbJXhHfEVYqBGHidPgoEtALWA.id = num2++;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.name = "Hat " + k + " Up";
				lVegNbJXhHfEVYqBGHidPgoEtALWA.key = LocalizationManager.AppendToKeyAsPath(a, "up");
				list.Add(new ControllerElementIdentifier(lVegNbJXhHfEVYqBGHidPgoEtALWA));
				lVegNbJXhHfEVYqBGHidPgoEtALWA.id = num3++;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.name = "Hat " + k + " Up Right";
				lVegNbJXhHfEVYqBGHidPgoEtALWA.key = LocalizationManager.AppendToKeyAsPath(a, "up_right");
				list.Add(new ControllerElementIdentifier(lVegNbJXhHfEVYqBGHidPgoEtALWA));
				lVegNbJXhHfEVYqBGHidPgoEtALWA.id = num2++;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.name = "Hat " + k + " Right";
				lVegNbJXhHfEVYqBGHidPgoEtALWA.key = LocalizationManager.AppendToKeyAsPath(a, "right");
				list.Add(new ControllerElementIdentifier(lVegNbJXhHfEVYqBGHidPgoEtALWA));
				lVegNbJXhHfEVYqBGHidPgoEtALWA.id = num3++;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.name = "Hat " + k + " Down Right";
				lVegNbJXhHfEVYqBGHidPgoEtALWA.key = LocalizationManager.AppendToKeyAsPath(a, "down_right");
				list.Add(new ControllerElementIdentifier(lVegNbJXhHfEVYqBGHidPgoEtALWA));
				lVegNbJXhHfEVYqBGHidPgoEtALWA.id = num2++;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.name = "Hat " + k + " Down";
				lVegNbJXhHfEVYqBGHidPgoEtALWA.key = LocalizationManager.AppendToKeyAsPath(a, "down");
				list.Add(new ControllerElementIdentifier(lVegNbJXhHfEVYqBGHidPgoEtALWA));
				lVegNbJXhHfEVYqBGHidPgoEtALWA.id = num3++;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.name = "Hat " + k + " Down Left";
				lVegNbJXhHfEVYqBGHidPgoEtALWA.key = LocalizationManager.AppendToKeyAsPath(a, "down_left");
				list.Add(new ControllerElementIdentifier(lVegNbJXhHfEVYqBGHidPgoEtALWA));
				lVegNbJXhHfEVYqBGHidPgoEtALWA.id = num2++;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.name = "Hat " + k + " Left";
				lVegNbJXhHfEVYqBGHidPgoEtALWA.key = LocalizationManager.AppendToKeyAsPath(a, "left");
				list.Add(new ControllerElementIdentifier(lVegNbJXhHfEVYqBGHidPgoEtALWA));
				lVegNbJXhHfEVYqBGHidPgoEtALWA.id = num3++;
				lVegNbJXhHfEVYqBGHidPgoEtALWA.name = "Hat " + k + " Up Left";
				lVegNbJXhHfEVYqBGHidPgoEtALWA.key = LocalizationManager.AppendToKeyAsPath(a, "up_left");
				list.Add(new ControllerElementIdentifier(lVegNbJXhHfEVYqBGHidPgoEtALWA));
			}
			return list.ToArray();
		}
	}
}
