using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired.Config;
using Rewired.Platforms;
using Rewired.Platforms.Custom;
using Rewired.Utils;
using Rewired.Utils.Attributes;
using Rewired.Utils.Classes.Data;

namespace Rewired.Data
{
	[Serializable]
	[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
	public sealed class ConfigVars : IConfigVars_Internal
	{
		private static class wiZphJfCAOXzeMBqwEUGhOvNCXUsA
		{
			public const string TrVhQMavKEfrfUwrAJhrQcpjCpgQ = "updateLoop";

			public const string tMpqHUIwNooNHbJWAKEDzASawMPK = "alwaysUseUnityInput";

			public const string YriJkWkpGUqmaqbthuEDxqIHOfkE = "windowsStandalonePrimaryInputSource";

			public const string dJcrEJtTTloUjIQFLxfVAkefgJwg = "osx_primaryInputSource";

			public const string ggbuUGUUyuqORPXVblPThfOimrcn = "linux_primaryInputSource";

			public const string kmCXKDPDNTigBOHbsfmMbghsThyK = "windowsUWP_primaryInputSource";

			public const string fMpaLhiWBRkLCKnKnhCAmzndvdtV = "xboxOne_primaryInputSource";

			public const string wzviCwBQseYKSkIVuQCyncdXFxrK = "gameCoreXboxOne_primaryInputSource";

			public const string SFoFSuMArbzhZUYKgcfqtzrAHXaB = "gameCoreScarlett_primaryInputSource";

			public const string suWAUpiHRyGgxUuculoTnCcsKuaoA = "ps4_primaryInputSource";

			public const string lTHVqKQRsDesDCQjGAdqvDGyiaMe = "ps5_primaryInputSource";

			public const string yEakLAveipfMAeVFONvpbAnloIZC = "webGL_primaryInputSource";

			public const string vIjdlvfcpXMCnZKhzgFJeUyWspbRA = "useXInput";

			public const string QQnKMwkluDFGyEfyTzWTPDWbwAzj = "windowsStandalone_useSteamRawInputControllerWorkaround";

			public const string jzdUClaGtwbSQkfYIvDoKQFCCZEzA = "osxStandalone_useEnhancedDeviceSupport";

			public const string nHDnRHXzfrBtCoEhaJzkqOlwKLTO = "android_supportUnknownGamepads";

			public const string wItcLkfddvNKoezGPUMsFaHHlfLMb = "ps4_assignJoysticksByPS4JoyId";

			public const string rAcLKxuzpORChZSiJsUhluKTYzji = "useSteamControllerSupport";

			public const string MsgdJpdYKeLIeXxjCzNEWqBMXcZg = "logToScreen";

			public const string XQYkHHEwFVgggGwUiTfctHQqUreY = "runInEditMode";

			public const string OjCRrRocMqmESQHRWwYvwDeIuWGA = "allowInputInEditorSceneView";

			public const string nkCabNUicKgqTezVZMnfuRXCkloR = "maxJoysticksPerPlayer";

			public const string xTwGeXICobrIRLcKlQjajGhKEfllA = "autoAssignJoysticks";

			public const string dkIkCRJCCGdNetivturSEBQHZGkF = "assignJoysticksToPlayingPlayersOnly";

			public const string yTWibRBFzcwmwCDdWVjpEBsHAYxaA = "distributeJoysticksEvenly";

			public const string fHjCTbTFdULsOtRaAdfmorDDtpHJ = "reassignJoystickToPreviousOwnerOnReconnect";

			public const string rZcRQlEokErmNQnMmzHsoRXUmeZG = "defaultJoystickAxis2DDeadZoneType";

			public const string hvPreIkbynrpeKghVlmLOeQDMKiJ = "defaultJoystickAxis2DSensitivityType";

			public const string ZyvovbchikDQILhxuukaPczWWogp = "defaultAxisSensitivityType";

			public const string TwVsXGKlQcWrXkvQgOcLmUhmcWXT = "force4WayHats";

			public const string QTSkrpDnZoQOMYHdBsVrVtSvxDKc = "throttleCalibrationMode";

			public const string PdICSgqFuzplPCgBoflewmjlERGg = "activateActionButtonsOnNegativeValue";

			public const string QVbcaCIvEZwaDfaYLjMaixacpyaQB = "deferControllerConnectedEventsOnStart";

			public const string CFeaCEMAQogclijQVFoldozcWNKcb = "logLevel";

			public const string MUJhDJWijgpAwyKdUNNCUyprUlcN = "disableKeyboard";

			public const string ELaabddQQmIzmaulHOCGjahAaRgxd = "disableMouse";

			public const string JzqpiknogfTpYLIlIlLjWwwjnJxk = "ignoreInputWhenAppNotInFocus";

			public const string nwjsvThleNwQSldaVFMzLsYXYqap = "useEnhancedDeviceSupport";

			public const string RsGKJvwrcYUwkaBNSqxbfwzvxJhA = "useNativeMouse";

			public const string oXPfDrEgXdwFtZWkUgTsQoCkDFkv = "useNativeKeyboard";

			public const string XVKteFTfClBnNJVYkIEIJbmWKcaY = "joystickRefreshRate";

			public const string zJUwonpVsmUAWzQlBfhsAkGRDoIo = "assignJoysticksBySystemId";
		}

		[Serializable]
		public class PlatformVars
		{
			public bool disableKeyboard;

			public bool disableMouse;

			public bool ignoreInputWhenAppNotInFocus = true;
		}

		[Serializable]
		public class PlatformVars_WindowsStandalone : PlatformVars
		{
			public bool useNativeKeyboard = true;

			public int joystickRefreshRate = 240;

			public bool useWindowsGamingInput;

			public List<EnhancedDeviceSupportDeviceType> enhancedDeviceSupportExcludedDeviceTypes;
		}

		[Serializable]
		public class PlatformVars_OSXStandalone : PlatformVars
		{
			public bool useAppleGameController;

			public bool assignJoysticksByUserId;

			public List<EnhancedDeviceSupportDeviceType> enhancedDeviceSupportExcludedDeviceTypes;
		}

		[Serializable]
		public class PlatformVars_LinuxStandalone : PlatformVars
		{
			public bool useEnhancedDeviceSupport = true;

			public List<EnhancedDeviceSupportDeviceType> enhancedDeviceSupportExcludedDeviceTypes;
		}

		[Serializable]
		public class PlatformVars_WindowsUWP : PlatformVars
		{
			public bool useGamepadAPI = true;

			public bool useHIDAPI = true;
		}

		[Serializable]
		public class PlatformVars_GameCoreXboxOne : PlatformVars
		{
			public bool assignJoysticksByUserId;
		}

		[Serializable]
		public class PlatformVars_GameCoreScarlett : PlatformVars
		{
			public bool assignJoysticksByUserId;
		}

		[Serializable]
		public class PlatformVars_PS5 : PlatformVars
		{
			public bool assignJoysticksByPS5JoyId = true;
		}

		[Serializable]
		public sealed class EditorVars
		{
			public bool exportConsts_useParentClass;

			public string exportConsts_parentClassName = "RewiredConsts";

			public bool exportConsts_useNamespace = true;

			public string exportConsts_namespace = "RewiredConsts";

			public bool exportConsts_actions = true;

			public string exportConsts_actionsClassName = "Action";

			public bool exportConsts_actionsIncludeActionCategory;

			public bool exportConsts_actionsCreateClassesForActionCategories;

			public bool exportConsts_mapCategories = true;

			public string exportConsts_mapCategoriesClassName = "Category";

			public bool exportConsts_layouts = true;

			public string exportConsts_layoutsClassName = "Layout";

			public bool exportConsts_players = true;

			public string exportConsts_playersClassName = "Player";

			public bool exportConsts_inputBehaviors;

			public string exportConsts_inputBehaviorsClassName = "InputBehavior";

			public bool exportConsts_customControllers = true;

			public string exportConsts_customControllersClassName = "CustomController";

			public string exportConsts_customControllersAxesClassName = "Axis";

			public string exportConsts_customControllersButtonsClassName = "Button";

			public bool exportConsts_layoutManagerRuleSets = true;

			public string exportConsts_layoutManagerRuleSetsClassName = "LayoutManagerRuleSet";

			public bool exportConsts_mapEnablerRuleSets = true;

			public string exportConsts_mapEnablerRuleSetsClassName = "MapEnablerRuleSet";

			public bool exportConsts_allCapsConstantNames;
		}

		private class omGDHqPqXqumPIEoyLMDjqefblsJA
		{
			public Func<PlatformVars> vbFiPQDgIUyedNSFwbqzDIQBqdpWB;

			public string nRAflKiVDPMIALeKaXTbkDbhIzmY;

			public omGDHqPqXqumPIEoyLMDjqefblsJA(Func<PlatformVars> P_0, string P_1)
			{
				vbFiPQDgIUyedNSFwbqzDIQBqdpWB = P_0;
				nRAflKiVDPMIALeKaXTbkDbhIzmY = P_1;
			}
		}

		private class pLDhrWGsfEcMDkRQoCWmdKkkcoNo
		{
			public Func<Platform, object> LvBpCSLHVOCSjKdFCLthQNmRjWyt;

			public Action<Platform, object> NOzCPrwMJjPRxgxXWwBuikxFuxvC;

			public pLDhrWGsfEcMDkRQoCWmdKkkcoNo(Func<Platform, object> P_0, Action<Platform, object> P_1)
			{
				LvBpCSLHVOCSjKdFCLthQNmRjWyt = P_0;
				NOzCPrwMJjPRxgxXWwBuikxFuxvC = P_1;
			}
		}

		[CustomObfuscation(rename = false)]
		internal enum AllPlatformVar
		{
			[CustomObfuscation(rename = false)]
			DisableKeyboard = 0,
			[CustomObfuscation(rename = false)]
			IgnoreInputWhenAppNotInFocus = 1,
			[CustomObfuscation(rename = false)]
			DisableMouse = 2
		}

		public UpdateMode updateMode;

		public UpdateLoopSetting updateLoop = UpdateLoopSetting.Update;

		public bool alwaysUseUnityInput;

		public WindowsStandalonePrimaryInputSource windowsStandalonePrimaryInputSource;

		public OSXStandalonePrimaryInputSource osx_primaryInputSource;

		public LinuxStandalonePrimaryInputSource linux_primaryInputSource;

		public WindowsUWPPrimaryInputSource windowsUWP_primaryInputSource;

		public XboxOnePrimaryInputSource xboxOne_primaryInputSource;

		public GameCoreXboxOnePrimaryInputSource gameCoreXboxOne_primaryInputSource;

		public GameCoreScarlettPrimaryInputSource gameCoreScarlett_primaryInputSource;

		public PS4PrimaryInputSource ps4_primaryInputSource;

		public PS5PrimaryInputSource ps5_primaryInputSource;

		public WebGLPrimaryInputSource webGL_primaryInputSource;

		public bool useXInput = true;

		public bool useNativeMouse = true;

		public bool useEnhancedDeviceSupport = true;

		public bool osxStandalone_useEnhancedDeviceSupport = true;

		public bool android_supportUnknownGamepads = true;

		public bool ps4_assignJoysticksByPS4JoyId = true;

		public bool useSteamControllerSupport = true;

		public bool logToScreen;

		public bool runInEditMode;

		public bool allowInputInEditorSceneView;

		public bool unityUsePhysicalKeys;

		public KeyCombinationOverrideMode keyCombinationOverrideMode = KeyCombinationOverrideMode.Pause;

		public bool generateKeyEventsOnKeyCombinationOverride;

		public PlatformVars_WindowsStandalone platformVars_windowsStandalone;

		public PlatformVars_LinuxStandalone platformVars_linuxStandalone;

		public PlatformVars_OSXStandalone platformVars_osxStandalone;

		public PlatformVars_WindowsUWP platformVars_windowsUWP;

		public PlatformVars platformVars_iOS;

		public PlatformVars platformVars_tvOS;

		public PlatformVars platformVars_android;

		public PlatformVars platformVars_ps4;

		public PlatformVars_PS5 platformVars_ps5;

		public PlatformVars platformVars_psVita;

		public PlatformVars platformVars_xboxOne;

		public PlatformVars_GameCoreXboxOne platformVars_gameCoreXboxOne;

		public PlatformVars_GameCoreScarlett platformVars_gameCoreScarlett;

		public PlatformVars platformVars_switch;

		public PlatformVars platformVars_switch2;

		public PlatformVars platformVars_webGL;

		[NonSerialized]
		private PlatformVars platformVars_unknown;

		public int maxJoysticksPerPlayer = 1;

		public bool autoAssignJoysticks = true;

		public bool assignJoysticksToPlayingPlayersOnly;

		public bool distributeJoysticksEvenly = true;

		public bool reassignJoystickToPreviousOwnerOnReconnect = true;

		public DeadZone2DType defaultJoystickAxis2DDeadZoneType = DeadZone2DType.Radial;

		public AxisSensitivity2DType defaultJoystickAxis2DSensitivityType;

		public AxisSensitivityType defaultAxisSensitivityType;

		public bool force4WayHats;

		public ThrottleCalibrationMode throttleCalibrationMode;

		public bool activateActionButtonsOnNegativeValue;

		public bool deferControllerConnectedEventsOnStart;

		public LogLevelFlags logLevel = LogLevelFlags.Info | LogLevelFlags.Warning | LogLevelFlags.Error;

		public EditorVars editorSettings;

		private Dictionary<int, omGDHqPqXqumPIEoyLMDjqefblsJA> __platformVarsDict;

		private Dictionary<int, pLDhrWGsfEcMDkRQoCWmdKkkcoNo> __getSetPlatformVariableDict;

		private KeyedGetSetValueStore<string> __configVarsValues;

		private Dictionary<string, object> __valueDelegates;

		private Dictionary<int, omGDHqPqXqumPIEoyLMDjqefblsJA> platformVarsDict
		{
			get
			{
				Dictionary<int, omGDHqPqXqumPIEoyLMDjqefblsJA> dictionary = __platformVarsDict;
				if (dictionary == null)
				{
					Dictionary<int, omGDHqPqXqumPIEoyLMDjqefblsJA> obj = new Dictionary<int, omGDHqPqXqumPIEoyLMDjqefblsJA>
					{
						{
							1,
							new omGDHqPqXqumPIEoyLMDjqefblsJA(() => GetOrCreatePlatformVars(ref platformVars_windowsStandalone), "platformVars_windowsStandalone")
						},
						{
							29,
							new omGDHqPqXqumPIEoyLMDjqefblsJA(() => GetOrCreatePlatformVars(ref platformVars_windowsUWP), "platformVars_windowsUWP")
						},
						{
							6,
							new omGDHqPqXqumPIEoyLMDjqefblsJA(() => GetOrCreatePlatformVars(ref platformVars_linuxStandalone), "platformVars_linuxStandalone")
						},
						{
							4,
							new omGDHqPqXqumPIEoyLMDjqefblsJA(() => GetOrCreatePlatformVars(ref platformVars_osxStandalone), "platformVars_osxStandalone")
						},
						{
							5,
							new omGDHqPqXqumPIEoyLMDjqefblsJA(() => GetOrCreatePlatformVars(ref platformVars_iOS), "platformVars_iOS")
						},
						{
							28,
							new omGDHqPqXqumPIEoyLMDjqefblsJA(() => GetOrCreatePlatformVars(ref platformVars_tvOS), "platformVars_tvOS")
						},
						{
							13,
							new omGDHqPqXqumPIEoyLMDjqefblsJA(() => GetOrCreatePlatformVars(ref platformVars_ps4), "platformVars_ps4")
						},
						{
							106,
							new omGDHqPqXqumPIEoyLMDjqefblsJA(() => GetOrCreatePlatformVars(ref platformVars_ps5), "platformVars_ps5")
						},
						{
							15,
							new omGDHqPqXqumPIEoyLMDjqefblsJA(() => GetOrCreatePlatformVars(ref platformVars_psVita), "platformVars_psVita")
						},
						{
							14,
							new omGDHqPqXqumPIEoyLMDjqefblsJA(() => GetOrCreatePlatformVars(ref platformVars_psVita), "platformVars_psVita")
						},
						{
							32,
							new omGDHqPqXqumPIEoyLMDjqefblsJA(() => GetOrCreatePlatformVars(ref platformVars_switch), "platformVars_switch")
						},
						{
							33,
							new omGDHqPqXqumPIEoyLMDjqefblsJA(() => GetOrCreatePlatformVars(ref platformVars_switch2), "platformVars_switch2")
						},
						{
							11,
							new omGDHqPqXqumPIEoyLMDjqefblsJA(() => GetOrCreatePlatformVars(ref platformVars_xboxOne), "platformVars_xboxOne")
						},
						{
							104,
							new omGDHqPqXqumPIEoyLMDjqefblsJA(() => GetOrCreatePlatformVars(ref platformVars_gameCoreXboxOne), "platformVars_gameCoreXboxOne")
						},
						{
							105,
							new omGDHqPqXqumPIEoyLMDjqefblsJA(() => GetOrCreatePlatformVars(ref platformVars_gameCoreScarlett), "platformVars_gameCoreScarlett")
						},
						{
							19,
							new omGDHqPqXqumPIEoyLMDjqefblsJA(() => GetOrCreatePlatformVars(ref platformVars_webGL), "platformVars_webGL")
						},
						{
							7,
							new omGDHqPqXqumPIEoyLMDjqefblsJA(() => GetOrCreatePlatformVars(ref platformVars_android), "platformVars_android")
						}
					};
					Dictionary<int, omGDHqPqXqumPIEoyLMDjqefblsJA> dictionary2 = obj;
					__platformVarsDict = obj;
					dictionary = dictionary2;
				}
				return dictionary;
			}
		}

		private Dictionary<int, pLDhrWGsfEcMDkRQoCWmdKkkcoNo> getSetPlatformVariableDict
		{
			get
			{
				Dictionary<int, pLDhrWGsfEcMDkRQoCWmdKkkcoNo> dictionary = __getSetPlatformVariableDict;
				if (dictionary == null)
				{
					Dictionary<int, pLDhrWGsfEcMDkRQoCWmdKkkcoNo> obj = new Dictionary<int, pLDhrWGsfEcMDkRQoCWmdKkkcoNo>
					{
						{
							0,
							new pLDhrWGsfEcMDkRQoCWmdKkkcoNo((Platform p) => GetPlatformVars(p).disableKeyboard, delegate(Platform platform, object value)
							{
								GetPlatformVars(platform).disableKeyboard = (bool)value;
							})
						},
						{
							2,
							new pLDhrWGsfEcMDkRQoCWmdKkkcoNo((Platform p) => GetPlatformVars(p).disableMouse, delegate(Platform platform, object value)
							{
								GetPlatformVars(platform).disableMouse = (bool)value;
							})
						},
						{
							1,
							new pLDhrWGsfEcMDkRQoCWmdKkkcoNo((Platform platform) => GetPlatformVars(platform).ignoreInputWhenAppNotInFocus, delegate(Platform platform, object value)
							{
								GetPlatformVars(platform).ignoreInputWhenAppNotInFocus = (bool)value;
							})
						}
					};
					Dictionary<int, pLDhrWGsfEcMDkRQoCWmdKkkcoNo> dictionary2 = obj;
					__getSetPlatformVariableDict = obj;
					dictionary = dictionary2;
				}
				return dictionary;
			}
		}

		KeyedGetSetValueStore<string> IConfigVars_Internal.values
		{
			get
			{
				if (__configVarsValues == null)
				{
					__configVarsValues = new KeyedGetSetValueStore<string>(valueDelegates, true);
				}
				return __configVarsValues;
			}
		}

		private Dictionary<string, object> valueDelegates
		{
			get
			{
				if (__valueDelegates == null)
				{
					__valueDelegates = new Dictionary<string, object>
					{
						{
							"updateLoop",
							new GetSetValue<UpdateLoopSetting>(() => updateLoop, delegate(UpdateLoopSetting x)
							{
								updateLoop = x;
							})
						},
						{
							"alwaysUseUnityInput",
							new GetSetValue<bool>(() => alwaysUseUnityInput, delegate(bool x)
							{
								alwaysUseUnityInput = x;
							})
						},
						{
							"windowsStandalonePrimaryInputSource",
							new GetSetValue<WindowsStandalonePrimaryInputSource>(() => windowsStandalonePrimaryInputSource, delegate(WindowsStandalonePrimaryInputSource x)
							{
								windowsStandalonePrimaryInputSource = x;
							})
						},
						{
							"osx_primaryInputSource",
							new GetSetValue<OSXStandalonePrimaryInputSource>(() => osx_primaryInputSource, delegate(OSXStandalonePrimaryInputSource x)
							{
								osx_primaryInputSource = x;
							})
						},
						{
							"linux_primaryInputSource",
							new GetSetValue<LinuxStandalonePrimaryInputSource>(() => linux_primaryInputSource, delegate(LinuxStandalonePrimaryInputSource x)
							{
								linux_primaryInputSource = x;
							})
						},
						{
							"windowsUWP_primaryInputSource",
							new GetSetValue<WindowsUWPPrimaryInputSource>(() => windowsUWP_primaryInputSource, delegate(WindowsUWPPrimaryInputSource x)
							{
								windowsUWP_primaryInputSource = x;
							})
						},
						{
							"xboxOne_primaryInputSource",
							new GetSetValue<XboxOnePrimaryInputSource>(() => xboxOne_primaryInputSource, delegate(XboxOnePrimaryInputSource x)
							{
								xboxOne_primaryInputSource = x;
							})
						},
						{
							"gameCoreXboxOne_primaryInputSource",
							new GetSetValue<GameCoreXboxOnePrimaryInputSource>(() => gameCoreXboxOne_primaryInputSource, delegate(GameCoreXboxOnePrimaryInputSource x)
							{
								gameCoreXboxOne_primaryInputSource = x;
							})
						},
						{
							"gameCoreScarlett_primaryInputSource",
							new GetSetValue<GameCoreScarlettPrimaryInputSource>(() => gameCoreScarlett_primaryInputSource, delegate(GameCoreScarlettPrimaryInputSource x)
							{
								gameCoreScarlett_primaryInputSource = x;
							})
						},
						{
							"ps4_primaryInputSource",
							new GetSetValue<PS4PrimaryInputSource>(() => ps4_primaryInputSource, delegate(PS4PrimaryInputSource x)
							{
								ps4_primaryInputSource = x;
							})
						},
						{
							"ps5_primaryInputSource",
							new GetSetValue<PS5PrimaryInputSource>(() => ps5_primaryInputSource, delegate(PS5PrimaryInputSource x)
							{
								ps5_primaryInputSource = x;
							})
						},
						{
							"webGL_primaryInputSource",
							new GetSetValue<WebGLPrimaryInputSource>(() => webGL_primaryInputSource, delegate(WebGLPrimaryInputSource x)
							{
								webGL_primaryInputSource = x;
							})
						},
						{
							"useXInput",
							new GetSetValue<bool>(() => useXInput, delegate(bool x)
							{
								useXInput = x;
							})
						},
						{
							"osxStandalone_useEnhancedDeviceSupport",
							new GetSetValue<bool>(() => osxStandalone_useEnhancedDeviceSupport, delegate(bool x)
							{
								osxStandalone_useEnhancedDeviceSupport = x;
							})
						},
						{
							"android_supportUnknownGamepads",
							new GetSetValue<bool>(() => android_supportUnknownGamepads, delegate(bool x)
							{
								android_supportUnknownGamepads = x;
							})
						},
						{
							"ps4_assignJoysticksByPS4JoyId",
							new GetSetValue<bool>(() => ps4_assignJoysticksByPS4JoyId, delegate(bool x)
							{
								ps4_assignJoysticksByPS4JoyId = x;
							})
						},
						{
							"useSteamControllerSupport",
							new GetSetValue<bool>(() => useSteamControllerSupport, delegate(bool x)
							{
								useSteamControllerSupport = x;
							})
						},
						{
							"logToScreen",
							new GetSetValue<bool>(() => logToScreen, delegate(bool x)
							{
								logToScreen = x;
							})
						},
						{
							"runInEditMode",
							new GetSetValue<bool>(() => runInEditMode, delegate(bool x)
							{
								runInEditMode = x;
							})
						},
						{
							"allowInputInEditorSceneView",
							new GetSetValue<bool>(() => allowInputInEditorSceneView, delegate(bool x)
							{
								allowInputInEditorSceneView = x;
							})
						},
						{
							"maxJoysticksPerPlayer",
							new GetSetValue<int>(() => maxJoysticksPerPlayer, delegate(int x)
							{
								maxJoysticksPerPlayer = x;
							})
						},
						{
							"autoAssignJoysticks",
							new GetSetValue<bool>(() => autoAssignJoysticks, delegate(bool x)
							{
								autoAssignJoysticks = x;
							})
						},
						{
							"assignJoysticksToPlayingPlayersOnly",
							new GetSetValue<bool>(() => assignJoysticksToPlayingPlayersOnly, delegate(bool x)
							{
								assignJoysticksToPlayingPlayersOnly = x;
							})
						},
						{
							"distributeJoysticksEvenly",
							new GetSetValue<bool>(() => distributeJoysticksEvenly, delegate(bool x)
							{
								distributeJoysticksEvenly = x;
							})
						},
						{
							"reassignJoystickToPreviousOwnerOnReconnect",
							new GetSetValue<bool>(() => reassignJoystickToPreviousOwnerOnReconnect, delegate(bool x)
							{
								reassignJoystickToPreviousOwnerOnReconnect = x;
							})
						},
						{
							"defaultJoystickAxis2DDeadZoneType",
							new GetSetValue<DeadZone2DType>(() => defaultJoystickAxis2DDeadZoneType, delegate(DeadZone2DType x)
							{
								defaultJoystickAxis2DDeadZoneType = x;
							})
						},
						{
							"defaultJoystickAxis2DSensitivityType",
							new GetSetValue<AxisSensitivity2DType>(() => defaultJoystickAxis2DSensitivityType, delegate(AxisSensitivity2DType x)
							{
								defaultJoystickAxis2DSensitivityType = x;
							})
						},
						{
							"defaultAxisSensitivityType",
							new GetSetValue<AxisSensitivityType>(() => defaultAxisSensitivityType, delegate(AxisSensitivityType x)
							{
								defaultAxisSensitivityType = x;
							})
						},
						{
							"force4WayHats",
							new GetSetValue<bool>(() => force4WayHats, delegate(bool x)
							{
								force4WayHats = x;
							})
						},
						{
							"throttleCalibrationMode",
							new GetSetValue<ThrottleCalibrationMode>(() => throttleCalibrationMode, delegate(ThrottleCalibrationMode x)
							{
								throttleCalibrationMode = x;
							})
						},
						{
							"activateActionButtonsOnNegativeValue",
							new GetSetValue<bool>(() => activateActionButtonsOnNegativeValue, delegate(bool x)
							{
								activateActionButtonsOnNegativeValue = x;
							})
						},
						{
							"deferControllerConnectedEventsOnStart",
							new GetSetValue<bool>(() => deferControllerConnectedEventsOnStart, delegate(bool x)
							{
								deferControllerConnectedEventsOnStart = x;
							})
						},
						{
							"logLevel",
							new GetSetValue<LogLevelFlags>(() => logLevel, delegate(LogLevelFlags x)
							{
								logLevel = x;
							})
						},
						{
							"disableKeyboard",
							new GetSetValue<bool>(() => GetPlatformVar_disableKeyboard(), delegate(bool x)
							{
								SetPlatformVar_disableKeyboard(x);
							})
						},
						{
							"disableMouse",
							new GetSetValue<bool>(() => GetPlatformVar_disableMouse(), delegate(bool x)
							{
								SetPlatformVar_disableMouse(x);
							})
						},
						{
							"ignoreInputWhenAppNotInFocus",
							new GetSetValue<bool>(() => GetPlatformVar_ignoreInputWhenAppNotInFocus(), delegate(bool x)
							{
								SetPlatformVar_ignoreInputWhenAppNotInFocus(x);
							})
						},
						{
							"useEnhancedDeviceSupport",
							new GetSetValue<bool>(() => GetPlatformVar_useEnhancedDeviceSupport(), delegate(bool x)
							{
								SetPlatformVar_useEnhancedDeviceSupport(x);
							})
						},
						{
							"useNativeMouse",
							new GetSetValue<bool>(() => GetPlatformVar_useNativeMouse(), delegate(bool x)
							{
								SetPlatformVar_useNativeMouse(x);
							})
						},
						{
							"useNativeKeyboard",
							new GetSetValue<bool>(() => GetPlatformVar_useNativeKeyboard(), delegate(bool x)
							{
								SetPlatformVar_useNativeKeyboard(x);
							})
						},
						{
							"joystickRefreshRate",
							new GetSetValue<int>(() => GetPlatformVar_joystickRefreshRate(), delegate(int x)
							{
								SetPlatformVar_joystickRefreshRate(x);
							})
						},
						{
							"assignJoysticksBySystemId",
							new GetSetValue<bool>(() => GetPlatformVar_assignJoysticksBySystemId(), delegate(bool x)
							{
								SetPlatformVar_assignJoysticksBySystemId(x);
							})
						}
					};
				}
				return __valueDelegates;
			}
		}

		[Preserve]
		public ConfigVars()
		{
		}

		internal bool DoesPlatformUseFallback(Platform platform, WebplayerPlatform webplayerPlatform, bool isEditor)
		{
			if (alwaysUseUnityInput)
			{
				return true;
			}
			if (!isEditor && webplayerPlatform != WebplayerPlatform.None)
			{
				return true;
			}
			return platform switch
			{
				Platform.Windows => windowsStandalonePrimaryInputSource == WindowsStandalonePrimaryInputSource.Unity, 
				Platform.OSX => osx_primaryInputSource == OSXStandalonePrimaryInputSource.Unity, 
				Platform.Linux => linux_primaryInputSource == LinuxStandalonePrimaryInputSource.Unity, 
				Platform.WindowsUWP => windowsUWP_primaryInputSource == WindowsUWPPrimaryInputSource.Unity, 
				Platform.WebGL => webGL_primaryInputSource == WebGLPrimaryInputSource.Unity, 
				Platform.XboxOne => xboxOne_primaryInputSource == XboxOnePrimaryInputSource.Unity, 
				Platform.PS4 => ps4_primaryInputSource == PS4PrimaryInputSource.Unity, 
				_ => false, 
			};
		}

		internal string GetDebugConfigSettings()
		{
			string text = "";
			switch (UnityTools.platform)
			{
			case Platform.Windows:
				text = text + "Primary input source: " + windowsStandalonePrimaryInputSource.ToString() + "\n";
				text = text + "Use XInput: " + useXInput + "\n";
				break;
			case Platform.OSX:
				text = text + "Primary input source: " + osx_primaryInputSource.ToString() + "\n";
				break;
			case Platform.Linux:
				text = text + "Primary input source: " + linux_primaryInputSource.ToString() + "\n";
				break;
			case Platform.WindowsUWP:
				text = text + "Primary input source: " + windowsUWP_primaryInputSource.ToString() + "\n";
				break;
			case Platform.XboxOne:
				text = text + "Primary input source: " + xboxOne_primaryInputSource.ToString() + "\n";
				break;
			case Platform.GameCoreXboxOne:
				text = text + "Primary input source: " + gameCoreXboxOne_primaryInputSource.ToString() + "\n";
				break;
			case Platform.GameCoreScarlett:
				text = text + "Primary input source: " + gameCoreScarlett_primaryInputSource.ToString() + "\n";
				break;
			case Platform.PS4:
				text = text + "Primary input source: " + ps4_primaryInputSource.ToString() + "\n";
				break;
			case Platform.PS5:
				text = text + "Primary input source: " + ps5_primaryInputSource.ToString() + "\n";
				break;
			case Platform.WebGL:
				text = text + "Primary input source: " + webGL_primaryInputSource.ToString() + "\n";
				break;
			}
			text = text + "Native mouse handling: " + GetPlatformVar_useNativeMouse() + "\n";
			text = text + "Enhanced device support: " + GetPlatformVar_useEnhancedDeviceSupport() + "\n";
			if (UnityTools.isAndroidPlatform)
			{
				text = text + "Android: Support Unknown Gamepads: " + android_supportUnknownGamepads + "\n";
			}
			return text;
		}

		[CustomObfuscation(rename = false)]
		internal string GetPlatformVarsRelPath(Platform platform)
		{
			if (platformVarsDict.ContainsKey((int)platform))
			{
				return platformVarsDict[(int)platform].nRAflKiVDPMIALeKaXTbkDbhIzmY;
			}
			throw new NotImplementedException();
		}

		[CustomObfuscation(rename = false)]
		internal PlatformVars GetPlatformVars(Platform platform)
		{
			PlatformVars platformVars = ((platform == Platform.Custom && pvRPaQsIqigEcORkKjmYXGlVnyZO.GqHFnYqaTUNNdsujzsTuwmmbVupw) ? pvRPaQsIqigEcORkKjmYXGlVnyZO.vPlGBaIZhMFrYUFJcbUjSkWdaiNh() : ((!platformVarsDict.ContainsKey((int)platform)) ? GetOrCreatePlatformVars(ref platformVars_unknown) : platformVarsDict[(int)platform].vbFiPQDgIUyedNSFwbqzDIQBqdpWB()));
			if (platformVars == null)
			{
				platformVars = new PlatformVars();
			}
			return platformVars;
		}

		[CustomObfuscation(rename = false)]
		internal T Editor_GetAllSerializedPlatformVar<T>(AllPlatformVar var)
		{
			if ((object)typeof(T) == typeof(MultiBoolValue))
			{
				return (T)(object)GetAllSerializedPlatformVar_multiBool(var);
			}
			throw new NotImplementedException();
		}

		[CustomObfuscation(rename = false)]
		internal void Editor_SetAllSerializedPlatformVar(AllPlatformVar var, object value)
		{
			foreach (KeyValuePair<int, omGDHqPqXqumPIEoyLMDjqefblsJA> item in platformVarsDict)
			{
				if (getSetPlatformVariableDict.ContainsKey((int)var))
				{
					getSetPlatformVariableDict[(int)var].NOzCPrwMJjPRxgxXWwBuikxFuxvC((Platform)item.Key, value);
				}
			}
		}

		internal bool GetPlatformVar_disableKeyboard()
		{
			return GetPlatformVars().disableKeyboard;
		}

		internal bool SetPlatformVar_disableKeyboard(bool value)
		{
			return GetPlatformVars().disableKeyboard = value;
		}

		internal bool GetPlatformVar_disableMouse()
		{
			return GetPlatformVars().disableMouse;
		}

		internal bool SetPlatformVar_disableMouse(bool value)
		{
			return GetPlatformVars().disableMouse = value;
		}

		internal bool GetPlatformVar_ignoreInputWhenAppNotInFocus()
		{
			return GetPlatformVars().ignoreInputWhenAppNotInFocus;
		}

		internal bool GetPlatformVar_useEnhancedDeviceSupport()
		{
			Platform effectivePlatform = UnityTools.effectivePlatform;
			PlatformVars platformVars = GetPlatformVars(effectivePlatform);
			switch (effectivePlatform)
			{
			case Platform.Windows:
				return useEnhancedDeviceSupport;
			case Platform.OSX:
				return osxStandalone_useEnhancedDeviceSupport;
			case Platform.Linux:
				if (!(platformVars is PlatformVars_LinuxStandalone))
				{
					return false;
				}
				return (platformVars as PlatformVars_LinuxStandalone).useEnhancedDeviceSupport;
			default:
				return false;
			}
		}

		internal bool GetPlatformVar_useNativeMouse()
		{
			Platform effectivePlatform = UnityTools.effectivePlatform;
			PlatformVars platformVars = GetPlatformVars(effectivePlatform);
			if (platformVars == null)
			{
				return false;
			}
			switch (effectivePlatform)
			{
			case Platform.Windows:
				return useNativeMouse;
			case Platform.Custom:
				if (!(platformVars is CustomPlatformConfigVars))
				{
					return true;
				}
				return (platformVars as CustomPlatformConfigVars).useNativeMouse;
			default:
				return false;
			}
		}

		internal bool GetPlatformVar_useNativeKeyboard()
		{
			Platform effectivePlatform = UnityTools.effectivePlatform;
			if (!IsNativeKeyboardAllowed(effectivePlatform, unityUsePhysicalKeys))
			{
				return false;
			}
			PlatformVars platformVars = GetPlatformVars(effectivePlatform);
			if (platformVars == null)
			{
				return false;
			}
			switch (effectivePlatform)
			{
			case Platform.Windows:
				if (!(platformVars is PlatformVars_WindowsStandalone))
				{
					return true;
				}
				return (platformVars as PlatformVars_WindowsStandalone).useNativeKeyboard;
			case Platform.Custom:
				if (!(platformVars is CustomPlatformConfigVars))
				{
					return true;
				}
				return (platformVars as CustomPlatformConfigVars).useNativeKeyboard;
			default:
				return false;
			}
		}

		internal int GetPlatformVar_joystickRefreshRate()
		{
			Platform effectivePlatform = UnityTools.effectivePlatform;
			PlatformVars platformVars = GetPlatformVars(effectivePlatform);
			if (platformVars == null)
			{
				return 240;
			}
			if (effectivePlatform == Platform.Windows)
			{
				if (!(platformVars is PlatformVars_WindowsStandalone))
				{
					return 240;
				}
				return (platformVars as PlatformVars_WindowsStandalone).joystickRefreshRate;
			}
			return 240;
		}

		internal bool GetPlatformVar_assignJoysticksBySystemId()
		{
			Platform effectivePlatform = UnityTools.effectivePlatform;
			PlatformVars platformVars = GetPlatformVars(effectivePlatform);
			if (platformVars == null)
			{
				return false;
			}
			switch (effectivePlatform)
			{
			case Platform.PS4:
				return ps4_assignJoysticksByPS4JoyId;
			case Platform.GameCoreXboxOne:
				if (!(platformVars is PlatformVars_GameCoreXboxOne))
				{
					return false;
				}
				return (platformVars as PlatformVars_GameCoreXboxOne).assignJoysticksByUserId;
			case Platform.GameCoreScarlett:
				if (!(platformVars is PlatformVars_GameCoreScarlett))
				{
					return false;
				}
				return (platformVars as PlatformVars_GameCoreScarlett).assignJoysticksByUserId;
			case Platform.OSX:
				if (!(platformVars is PlatformVars_OSXStandalone))
				{
					return false;
				}
				return (platformVars as PlatformVars_OSXStandalone).assignJoysticksByUserId;
			case Platform.PS5:
				if (!(platformVars is PlatformVars_PS5))
				{
					return false;
				}
				return (platformVars as PlatformVars_PS5).assignJoysticksByPS5JoyId;
			default:
				return false;
			}
		}

		internal bool GetPlatformVar_useAppleGameController()
		{
			Platform effectivePlatform = UnityTools.effectivePlatform;
			PlatformVars platformVars = GetPlatformVars(effectivePlatform);
			if (platformVars == null)
			{
				return false;
			}
			if (effectivePlatform == Platform.OSX)
			{
				if (!(platformVars is PlatformVars_OSXStandalone))
				{
					return false;
				}
				return (platformVars as PlatformVars_OSXStandalone).useAppleGameController;
			}
			return false;
		}

		internal bool GetPlatformVar_useWindowsGamingInput()
		{
			Platform effectivePlatform = UnityTools.effectivePlatform;
			PlatformVars platformVars = GetPlatformVars(effectivePlatform);
			if (platformVars == null)
			{
				return false;
			}
			if (effectivePlatform == Platform.Windows)
			{
				if (!(platformVars is PlatformVars_WindowsStandalone))
				{
					return false;
				}
				return (platformVars as PlatformVars_WindowsStandalone).useWindowsGamingInput;
			}
			return false;
		}

		internal IList<EnhancedDeviceSupportDeviceType> GetPlatformVar_enhancedDeviceSupportExcludedDeviceTypes()
		{
			Platform effectivePlatform = UnityTools.effectivePlatform;
			PlatformVars platformVars = GetPlatformVars(effectivePlatform);
			if (platformVars == null)
			{
				return EmptyObjects<EnhancedDeviceSupportDeviceType>.EmptyReadOnlyIListT;
			}
			List<EnhancedDeviceSupportDeviceType> list = null;
			switch (effectivePlatform)
			{
			case Platform.Windows:
				if (platformVars is PlatformVars_WindowsStandalone)
				{
					list = (platformVars as PlatformVars_WindowsStandalone).enhancedDeviceSupportExcludedDeviceTypes;
				}
				break;
			case Platform.OSX:
				if (platformVars is PlatformVars_OSXStandalone)
				{
					list = (platformVars as PlatformVars_OSXStandalone).enhancedDeviceSupportExcludedDeviceTypes;
				}
				break;
			case Platform.Linux:
				if (platformVars is PlatformVars_LinuxStandalone)
				{
					list = (platformVars as PlatformVars_LinuxStandalone).enhancedDeviceSupportExcludedDeviceTypes;
				}
				break;
			}
			if (list != null)
			{
				return new ReadOnlyCollection<EnhancedDeviceSupportDeviceType>(list);
			}
			return EmptyObjects<EnhancedDeviceSupportDeviceType>.EmptyReadOnlyIListT;
		}

		internal bool SetPlatformVar_ignoreInputWhenAppNotInFocus(bool value)
		{
			if (GetPlatformVars().ignoreInputWhenAppNotInFocus == value)
			{
				return false;
			}
			GetPlatformVars().ignoreInputWhenAppNotInFocus = value;
			return true;
		}

		internal bool SetPlatformVar_useEnhancedDeviceSupport(bool value)
		{
			Platform effectivePlatform = UnityTools.effectivePlatform;
			PlatformVars platformVars = GetPlatformVars(effectivePlatform);
			switch (effectivePlatform)
			{
			case Platform.Windows:
				if (useEnhancedDeviceSupport == value)
				{
					return false;
				}
				useEnhancedDeviceSupport = value;
				return true;
			case Platform.OSX:
				if (osxStandalone_useEnhancedDeviceSupport == value)
				{
					return false;
				}
				osxStandalone_useEnhancedDeviceSupport = value;
				return true;
			case Platform.Linux:
				if (platformVars is PlatformVars_LinuxStandalone)
				{
					(platformVars as PlatformVars_LinuxStandalone).useEnhancedDeviceSupport = value;
				}
				return true;
			default:
				return false;
			}
		}

		internal bool SetPlatformVar_useNativeMouse(bool value)
		{
			switch (UnityTools.effectivePlatform)
			{
			case Platform.Windows:
				if (useNativeMouse == value)
				{
					return false;
				}
				useNativeMouse = value;
				return true;
			case Platform.Custom:
				if (pvRPaQsIqigEcORkKjmYXGlVnyZO.GqHFnYqaTUNNdsujzsTuwmmbVupw)
				{
					pvRPaQsIqigEcORkKjmYXGlVnyZO.vPlGBaIZhMFrYUFJcbUjSkWdaiNh().useNativeMouse = value;
					return true;
				}
				return false;
			default:
				return false;
			}
		}

		internal bool SetPlatformVar_useNativeKeyboard(bool value)
		{
			Platform effectivePlatform = UnityTools.effectivePlatform;
			if (!IsNativeKeyboardAllowed(effectivePlatform, unityUsePhysicalKeys))
			{
				return false;
			}
			PlatformVars platformVars = GetPlatformVars(effectivePlatform);
			if (platformVars == null)
			{
				return false;
			}
			switch (effectivePlatform)
			{
			case Platform.Windows:
				if (platformVars is PlatformVars_WindowsStandalone)
				{
					(platformVars as PlatformVars_WindowsStandalone).useNativeKeyboard = value;
				}
				return true;
			case Platform.Custom:
				if (pvRPaQsIqigEcORkKjmYXGlVnyZO.GqHFnYqaTUNNdsujzsTuwmmbVupw)
				{
					pvRPaQsIqigEcORkKjmYXGlVnyZO.vPlGBaIZhMFrYUFJcbUjSkWdaiNh().useNativeKeyboard = value;
					return true;
				}
				return false;
			default:
				return false;
			}
		}

		internal bool SetPlatformVar_joystickRefreshRate(int value)
		{
			Platform effectivePlatform = UnityTools.effectivePlatform;
			PlatformVars platformVars = GetPlatformVars(effectivePlatform);
			if (platformVars == null)
			{
				return false;
			}
			if (effectivePlatform == Platform.Windows)
			{
				if (platformVars is PlatformVars_WindowsStandalone)
				{
					(platformVars as PlatformVars_WindowsStandalone).joystickRefreshRate = value;
				}
				return true;
			}
			return false;
		}

		internal bool SetPlatformVar_assignJoysticksBySystemId(bool value)
		{
			Platform effectivePlatform = UnityTools.effectivePlatform;
			PlatformVars platformVars = GetPlatformVars(effectivePlatform);
			if (platformVars == null)
			{
				return false;
			}
			switch (effectivePlatform)
			{
			case Platform.PS4:
				ps4_assignJoysticksByPS4JoyId = value;
				return true;
			case Platform.GameCoreXboxOne:
				if (platformVars is PlatformVars_GameCoreXboxOne)
				{
					(platformVars as PlatformVars_GameCoreXboxOne).assignJoysticksByUserId = value;
				}
				return false;
			case Platform.GameCoreScarlett:
				if (platformVars is PlatformVars_GameCoreScarlett)
				{
					(platformVars as PlatformVars_GameCoreScarlett).assignJoysticksByUserId = value;
				}
				return true;
			case Platform.OSX:
				if (platformVars is PlatformVars_OSXStandalone)
				{
					(platformVars as PlatformVars_OSXStandalone).assignJoysticksByUserId = value;
				}
				return true;
			case Platform.PS5:
				if (platformVars is PlatformVars_PS5)
				{
					(platformVars as PlatformVars_PS5).assignJoysticksByPS5JoyId = value;
				}
				return true;
			default:
				return false;
			}
		}

		internal bool SetPlatformVar_useAppleGameController(bool value)
		{
			Platform effectivePlatform = UnityTools.effectivePlatform;
			PlatformVars platformVars = GetPlatformVars(effectivePlatform);
			if (platformVars == null)
			{
				return false;
			}
			if (effectivePlatform == Platform.OSX)
			{
				if (platformVars is PlatformVars_OSXStandalone)
				{
					(platformVars as PlatformVars_OSXStandalone).useAppleGameController = value;
				}
				return true;
			}
			return false;
		}

		internal bool SetPlatformVar_useWindowsGamingInput(bool value)
		{
			Platform effectivePlatform = UnityTools.effectivePlatform;
			PlatformVars platformVars = GetPlatformVars(effectivePlatform);
			if (platformVars == null)
			{
				return false;
			}
			if (effectivePlatform == Platform.Windows)
			{
				if (platformVars is PlatformVars_WindowsStandalone)
				{
					(platformVars as PlatformVars_WindowsStandalone).useWindowsGamingInput = value;
				}
				return true;
			}
			return false;
		}

		internal bool SetPlatformVar_enhancedDeviceSupportExcludedDeviceTypes(IList<EnhancedDeviceSupportDeviceType> value)
		{
			Platform effectivePlatform = UnityTools.effectivePlatform;
			PlatformVars platformVars = GetPlatformVars(effectivePlatform);
			if (platformVars == null)
			{
				return false;
			}
			switch (effectivePlatform)
			{
			case Platform.Windows:
				if (platformVars is PlatformVars_WindowsStandalone)
				{
					(platformVars as PlatformVars_WindowsStandalone).enhancedDeviceSupportExcludedDeviceTypes = ((value != null) ? new List<EnhancedDeviceSupportDeviceType>(value) : new List<EnhancedDeviceSupportDeviceType>());
				}
				return true;
			case Platform.OSX:
				if (platformVars is PlatformVars_OSXStandalone)
				{
					(platformVars as PlatformVars_OSXStandalone).enhancedDeviceSupportExcludedDeviceTypes = ((value != null) ? new List<EnhancedDeviceSupportDeviceType>(value) : new List<EnhancedDeviceSupportDeviceType>());
				}
				return true;
			case Platform.Linux:
				if (platformVars is PlatformVars_LinuxStandalone)
				{
					(platformVars as PlatformVars_LinuxStandalone).enhancedDeviceSupportExcludedDeviceTypes = ((value != null) ? new List<EnhancedDeviceSupportDeviceType>(value) : new List<EnhancedDeviceSupportDeviceType>());
				}
				return true;
			default:
				return false;
			}
		}

		private PlatformVars GetPlatformVars()
		{
			Platform platform = UnityTools.effectivePlatform;
			if (!UnityTools.isEditor && UnityTools.isAndroidPlatform)
			{
				platform = Platform.Android;
			}
			return GetPlatformVars(platform);
		}

		private T GetOrCreatePlatformVars<T>(ref T var) where T : PlatformVars, new()
		{
			if (var == null)
			{
				var = new T();
			}
			return var;
		}

		private MultiBoolValue GetAllSerializedPlatformVar_multiBool(AllPlatformVar var)
		{
			bool flag = false;
			bool flag2 = true;
			foreach (KeyValuePair<int, omGDHqPqXqumPIEoyLMDjqefblsJA> item in platformVarsDict)
			{
				if (!getSetPlatformVariableDict.ContainsKey((int)var))
				{
					continue;
				}
				object obj = getSetPlatformVariableDict[(int)var].LvBpCSLHVOCSjKdFCLthQNmRjWyt((Platform)item.Key);
				if (obj == null)
				{
					continue;
				}
				if ((object)obj.GetType() != typeof(bool))
				{
					Logger.LogWarning("Incorrect type. Expecting bool, got " + obj.GetType().Name);
					continue;
				}
				bool flag3 = (bool)obj;
				if (flag2)
				{
					flag = flag3;
					flag2 = false;
				}
				else if (flag3 != flag)
				{
					return MultiBoolValue.Mixed;
				}
			}
			if (!flag)
			{
				return MultiBoolValue.Off;
			}
			return MultiBoolValue.On;
		}

		internal bool IsEditModeInputSupported(ControllerType controllerType, EditorPlatform editorPlatform)
		{
			if (alwaysUseUnityInput)
			{
				return false;
			}
			switch (controllerType)
			{
			case ControllerType.Keyboard:
			case ControllerType.Mouse:
				switch (editorPlatform)
				{
				case EditorPlatform.OSX:
				case EditorPlatform.Linux:
					return false;
				case EditorPlatform.Windows:
					if (windowsStandalonePrimaryInputSource == WindowsStandalonePrimaryInputSource.RawInput || windowsStandalonePrimaryInputSource == WindowsStandalonePrimaryInputSource.DirectInput || windowsStandalonePrimaryInputSource == WindowsStandalonePrimaryInputSource.XInput)
					{
						if (controllerType != ControllerType.Keyboard)
						{
							return useNativeMouse;
						}
						return platformVars_windowsStandalone.useNativeKeyboard;
					}
					return false;
				default:
					return false;
				}
			case ControllerType.Joystick:
				switch (editorPlatform)
				{
				case EditorPlatform.Linux:
					if (linux_primaryInputSource != LinuxStandalonePrimaryInputSource.Native)
					{
						return linux_primaryInputSource == LinuxStandalonePrimaryInputSource.SDL2;
					}
					return true;
				case EditorPlatform.OSX:
					if (osx_primaryInputSource != OSXStandalonePrimaryInputSource.Native && osx_primaryInputSource != OSXStandalonePrimaryInputSource.GameController)
					{
						return osx_primaryInputSource == OSXStandalonePrimaryInputSource.SDL2;
					}
					return true;
				case EditorPlatform.Windows:
					if (windowsStandalonePrimaryInputSource != WindowsStandalonePrimaryInputSource.RawInput && windowsStandalonePrimaryInputSource != WindowsStandalonePrimaryInputSource.DirectInput && windowsStandalonePrimaryInputSource != WindowsStandalonePrimaryInputSource.XInput)
					{
						return windowsStandalonePrimaryInputSource == WindowsStandalonePrimaryInputSource.SDL2;
					}
					return true;
				default:
					return false;
				}
			default:
				return false;
			}
		}

		private static bool IsNativeKeyboardAllowed(Platform platform, bool unityUsePhysicalKeys)
		{
			if (platform != Platform.Custom && unityUsePhysicalKeys)
			{
				return false;
			}
			return true;
		}
	}
}
