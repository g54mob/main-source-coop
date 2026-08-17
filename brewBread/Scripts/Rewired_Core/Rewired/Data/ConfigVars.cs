using System;
using System.Collections.Generic;
using Rewired.Config;
using Rewired.Platforms;
using Rewired.Utils;
using Rewired.Utils.Attributes;
using Rewired.Utils.Classes.Data;

namespace Rewired.Data
{
	[Serializable]
	[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
	public sealed class ConfigVars : IConfigVars_Internal
	{
		private static class EBlSfRTrzyYCdaAPAJGIjfotIJFAA
		{
			public const string PiEdbgjMHSsksYjRKSGckYZEsfAE = "updateLoop";

			public const string SNxhUjtGlDdXLIJJfQqQnbqjAFbI = "alwaysUseUnityInput";

			public const string fYowBgGxDyoBWQnVUWbBlEgeamNH = "windowsStandalonePrimaryInputSource";

			public const string wqgpuKOIkcioMaXOUGQgCnSLZugJA = "osx_primaryInputSource";

			public const string rziHucliiCtZAqtwTPDsMyMCFBvB = "linux_primaryInputSource";

			public const string GDghyEedRSzAmUkooYerHxbKzBMi = "windowsUWP_primaryInputSource";

			public const string FWnIjeiAifdGHdNWXLLUeegoQSlEb = "xboxOne_primaryInputSource";

			public const string wtDuQaDjjycjOgawQJaCeHsgXtLoA = "gameCoreXboxOne_primaryInputSource";

			public const string ImuHmdSIILEGWHFJvdIAsfGJszov = "gameCoreScarlett_primaryInputSource";

			public const string lWwJoLSjbITrazfoVXKoPMssLFT = "ps4_primaryInputSource";

			public const string pKWFegGXMoPpsWujaOVkIfFOswcRA = "ps5_primaryInputSource";

			public const string qrpjjAeXKxJtcdtbqvZPULEYtTBX = "webGL_primaryInputSource";

			public const string ynlqmEmJgAFquGeTwkxUfbpGgUJhB = "stadia_primaryInputSource";

			public const string WKREfOKpaAITrzKYZnMcVIGWXMww = "useXInput";

			public const string pDbWQUzglewmbMmLWZpPsXBEPNPp = "windowsStandalone_useSteamRawInputControllerWorkaround";

			public const string KVtYrNmqXLLekdGYRcujzuHIoQCd = "osxStandalone_useEnhancedDeviceSupport";

			public const string yNcGkUnJJWGzXNaNJhbxxcuiLxHQ = "android_supportUnknownGamepads";

			public const string UtGFaAHRmLfAJqMhzhroHgpbOdcCb = "ps4_assignJoysticksByPS4JoyId";

			public const string mmkkqkPlWdgfFdXuHQpTUyveLvRbA = "useSteamControllerSupport";

			public const string UEFaNAjaItAsHBXGkfnFFvLxNNGR = "logToScreen";

			public const string dgCpaYiOikWlajPjwggbGQEQvGPc = "runInEditMode";

			public const string yyoBqoaAhGHQjuTwAlQetJgFOHwkA = "allowInputInEditorSceneView";

			public const string XOmvorxjbSwGIrWYZcIFsnEFwTnB = "maxJoysticksPerPlayer";

			public const string NhjDFeQkPUVDzTdOwRnLpawvWuKl = "autoAssignJoysticks";

			public const string VhomlfQmzTCfqXNvNkQpAoTJefVbA = "assignJoysticksToPlayingPlayersOnly";

			public const string cJIrhMPGonirzOJzVBPNaNthaURu = "distributeJoysticksEvenly";

			public const string pFkyyLXyGHItJKLhVvGemBVQfdSn = "reassignJoystickToPreviousOwnerOnReconnect";

			public const string DgLfkwjhuYCruVwWExwhQWOfuXeX = "defaultJoystickAxis2DDeadZoneType";

			public const string AwePHDXELXkMecTBoQRjCpWTPpwh = "defaultJoystickAxis2DSensitivityType";

			public const string guMJaSBozlzYMVzcETbGknxQnSPs = "defaultAxisSensitivityType";

			public const string NfTfDGvWvdyMYtOgBzpFljiOrhjq = "force4WayHats";

			public const string XGkFoEObVIRcbtzQyGzUJMRqLIoT = "throttleCalibrationMode";

			public const string JZHzvwwfpgxTUBIvwsqLXSdOovgT = "activateActionButtonsOnNegativeValue";

			public const string ReEqtrjnQasdqWouBScEvvZpkRmh = "deferControllerConnectedEventsOnStart";

			public const string pprvDWnGcNnqmvvOCYqulBAxEXqS = "logLevel";

			public const string qMOnxWbbDMukmgNUmDKSAliAwDBg = "disableKeyboard";

			public const string uwsEbWrMHicTKbdRCuhrnIrqCccJ = "disableMouse";

			public const string ypnfyRbutvVNcPSWjdUplQLSadZCb = "ignoreInputWhenAppNotInFocus";

			public const string bNGGXlpNazHqDoipghvTrFVegePI = "useEnhancedDeviceSupport";

			public const string jSVpqccvUEjfEvUOCGXUQgglGCxQ = "useNativeMouse";

			public const string OojdUcAIqVIOZQBUZZlAcoYmAGQQA = "useNativeKeyboard";

			public const string WQZCRvfhvZTzGakKmFtOgZkFcDPLB = "joystickRefreshRate";

			public const string gbjRVRnPyrgeVogwJGChImrQHseBA = "assignJoysticksBySystemId";
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
		}

		[Serializable]
		public class PlatformVars_OSXStandalone : PlatformVars
		{
			public bool useAppleGameController;

			public bool assignJoysticksByUserId;
		}

		[Serializable]
		public class PlatformVars_WindowsUWP : PlatformVars
		{
			public bool useGamepadAPI = true;

			public bool useHIDAPI = true;
		}

		[Serializable]
		public class PlatformVars_Stadia : PlatformVars
		{
			public bool useNativeKeyboard = true;

			public bool useNativeMouse = true;
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

		private class OpskQejDgIZSWovHGUlHGilBlTnF
		{
			public Func<PlatformVars> KwWcmpxWpdaEZpKWAerfiotksPQf;

			public string eHMcnYradBtmTJXOxXSYtHEVcnBD;

			public OpskQejDgIZSWovHGUlHGilBlTnF(Func<PlatformVars> P_0, string P_1)
			{
				KwWcmpxWpdaEZpKWAerfiotksPQf = P_0;
				eHMcnYradBtmTJXOxXSYtHEVcnBD = P_1;
			}
		}

		private class RitCkGqDKsfnMMztQPMiYMzIeGYJ
		{
			public Func<Platform, object> KwWcmpxWpdaEZpKWAerfiotksPQf;

			public Action<Platform, object> GUhNJxVCnnOncLXQZyIcvFXecYAl;

			public RitCkGqDKsfnMMztQPMiYMzIeGYJ(Func<Platform, object> P_0, Action<Platform, object> P_1)
			{
				KwWcmpxWpdaEZpKWAerfiotksPQf = P_0;
				GUhNJxVCnnOncLXQZyIcvFXecYAl = P_1;
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

		public StadiaPrimaryInputSource stadia_primaryInputSource;

		public bool useXInput = true;

		public bool useNativeMouse = true;

		public bool useEnhancedDeviceSupport = true;

		public bool windowsStandalone_useSteamRawInputControllerWorkaround;

		public bool osxStandalone_useEnhancedDeviceSupport = true;

		public bool android_supportUnknownGamepads = true;

		public bool ps4_assignJoysticksByPS4JoyId = true;

		public bool useSteamControllerSupport = true;

		public bool logToScreen;

		public bool runInEditMode;

		public bool allowInputInEditorSceneView;

		public PlatformVars_WindowsStandalone platformVars_windowsStandalone;

		public PlatformVars platformVars_linuxStandalone;

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

		public PlatformVars platformVars_webGL;

		public PlatformVars_Stadia platformVars_stadia;

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

		private Dictionary<int, OpskQejDgIZSWovHGUlHGilBlTnF> __platformVarsDict;

		private Dictionary<int, RitCkGqDKsfnMMztQPMiYMzIeGYJ> __getSetPlatformVariableDict;

		private KeyedGetSetValueStore<string> __configVarsValues;

		private Dictionary<string, object> __valueDelegates;

		private Dictionary<int, OpskQejDgIZSWovHGUlHGilBlTnF> platformVarsDict
		{
			get
			{
				Dictionary<int, OpskQejDgIZSWovHGUlHGilBlTnF> dictionary = __platformVarsDict;
				if (dictionary == null)
				{
					Dictionary<int, OpskQejDgIZSWovHGUlHGilBlTnF> obj = new Dictionary<int, OpskQejDgIZSWovHGUlHGilBlTnF>
					{
						{
							1,
							new OpskQejDgIZSWovHGUlHGilBlTnF(() => GetOrCreatePlatformVars(ref platformVars_windowsStandalone), "platformVars_windowsStandalone")
						},
						{
							29,
							new OpskQejDgIZSWovHGUlHGilBlTnF(() => GetOrCreatePlatformVars(ref platformVars_windowsUWP), "platformVars_windowsUWP")
						},
						{
							6,
							new OpskQejDgIZSWovHGUlHGilBlTnF(() => GetOrCreatePlatformVars(ref platformVars_linuxStandalone), "platformVars_linuxStandalone")
						},
						{
							4,
							new OpskQejDgIZSWovHGUlHGilBlTnF(() => GetOrCreatePlatformVars(ref platformVars_osxStandalone), "platformVars_osxStandalone")
						},
						{
							5,
							new OpskQejDgIZSWovHGUlHGilBlTnF(() => GetOrCreatePlatformVars(ref platformVars_iOS), "platformVars_iOS")
						},
						{
							28,
							new OpskQejDgIZSWovHGUlHGilBlTnF(() => GetOrCreatePlatformVars(ref platformVars_tvOS), "platformVars_tvOS")
						},
						{
							13,
							new OpskQejDgIZSWovHGUlHGilBlTnF(() => GetOrCreatePlatformVars(ref platformVars_ps4), "platformVars_ps4")
						},
						{
							106,
							new OpskQejDgIZSWovHGUlHGilBlTnF(() => GetOrCreatePlatformVars(ref platformVars_ps5), "platformVars_ps5")
						},
						{
							15,
							new OpskQejDgIZSWovHGUlHGilBlTnF(() => GetOrCreatePlatformVars(ref platformVars_psVita), "platformVars_psVita")
						},
						{
							14,
							new OpskQejDgIZSWovHGUlHGilBlTnF(() => GetOrCreatePlatformVars(ref platformVars_psVita), "platformVars_psVita")
						},
						{
							32,
							new OpskQejDgIZSWovHGUlHGilBlTnF(() => GetOrCreatePlatformVars(ref platformVars_switch), "platformVars_switch")
						},
						{
							11,
							new OpskQejDgIZSWovHGUlHGilBlTnF(() => GetOrCreatePlatformVars(ref platformVars_xboxOne), "platformVars_xboxOne")
						},
						{
							104,
							new OpskQejDgIZSWovHGUlHGilBlTnF(() => GetOrCreatePlatformVars(ref platformVars_gameCoreXboxOne), "platformVars_gameCoreXboxOne")
						},
						{
							105,
							new OpskQejDgIZSWovHGUlHGilBlTnF(() => GetOrCreatePlatformVars(ref platformVars_gameCoreScarlett), "platformVars_gameCoreScarlett")
						},
						{
							19,
							new OpskQejDgIZSWovHGUlHGilBlTnF(() => GetOrCreatePlatformVars(ref platformVars_webGL), "platformVars_webGL")
						},
						{
							103,
							new OpskQejDgIZSWovHGUlHGilBlTnF(() => GetOrCreatePlatformVars(ref platformVars_stadia), "platformVars_stadia")
						},
						{
							7,
							new OpskQejDgIZSWovHGUlHGilBlTnF(() => GetOrCreatePlatformVars(ref platformVars_android), "platformVars_android")
						}
					};
					Dictionary<int, OpskQejDgIZSWovHGUlHGilBlTnF> dictionary2 = obj;
					__platformVarsDict = obj;
					dictionary = dictionary2;
				}
				return dictionary;
			}
		}

		private Dictionary<int, RitCkGqDKsfnMMztQPMiYMzIeGYJ> getSetPlatformVariableDict
		{
			get
			{
				Dictionary<int, RitCkGqDKsfnMMztQPMiYMzIeGYJ> dictionary = __getSetPlatformVariableDict;
				if (dictionary == null)
				{
					Dictionary<int, RitCkGqDKsfnMMztQPMiYMzIeGYJ> obj = new Dictionary<int, RitCkGqDKsfnMMztQPMiYMzIeGYJ>
					{
						{
							0,
							new RitCkGqDKsfnMMztQPMiYMzIeGYJ((Platform p) => GetPlatformVars(p).disableKeyboard, delegate(Platform platform, object value)
							{
								GetPlatformVars(platform).disableKeyboard = (bool)value;
							})
						},
						{
							2,
							new RitCkGqDKsfnMMztQPMiYMzIeGYJ((Platform p) => GetPlatformVars(p).disableMouse, delegate(Platform platform, object value)
							{
								GetPlatformVars(platform).disableMouse = (bool)value;
							})
						},
						{
							1,
							new RitCkGqDKsfnMMztQPMiYMzIeGYJ((Platform platform) => GetPlatformVars(platform).ignoreInputWhenAppNotInFocus, delegate(Platform platform, object value)
							{
								GetPlatformVars(platform).ignoreInputWhenAppNotInFocus = (bool)value;
							})
						}
					};
					Dictionary<int, RitCkGqDKsfnMMztQPMiYMzIeGYJ> dictionary2 = obj;
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
							"stadia_primaryInputSource",
							new GetSetValue<StadiaPrimaryInputSource>(() => stadia_primaryInputSource, delegate(StadiaPrimaryInputSource x)
							{
								stadia_primaryInputSource = x;
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
							"windowsStandalone_useSteamRawInputControllerWorkaround",
							new GetSetValue<bool>(() => windowsStandalone_useSteamRawInputControllerWorkaround, delegate(bool x)
							{
								windowsStandalone_useSteamRawInputControllerWorkaround = x;
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

		internal bool DoesPlatformUseSDL2(Platform platform, WebplayerPlatform webplayerPlatform, bool isEditor)
		{
			if (alwaysUseUnityInput)
			{
				return false;
			}
			if (!isEditor && webplayerPlatform != WebplayerPlatform.None)
			{
				return false;
			}
			return platform switch
			{
				Platform.Windows => windowsStandalonePrimaryInputSource == WindowsStandalonePrimaryInputSource.SDL2, 
				Platform.OSX => osx_primaryInputSource == OSXStandalonePrimaryInputSource.SDL2, 
				Platform.Linux => linux_primaryInputSource == LinuxStandalonePrimaryInputSource.SDL2, 
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
			case Platform.Stadia:
				text = text + "Primary input source: " + stadia_primaryInputSource.ToString() + "\n";
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
				return platformVarsDict[(int)platform].eHMcnYradBtmTJXOxXSYtHEVcnBD;
			}
			throw new NotImplementedException();
		}

		[CustomObfuscation(rename = false)]
		internal PlatformVars GetPlatformVars(Platform platform)
		{
			PlatformVars platformVars = ((!platformVarsDict.ContainsKey((int)platform)) ? GetOrCreatePlatformVars(ref platformVars_unknown) : platformVarsDict[(int)platform].KwWcmpxWpdaEZpKWAerfiotksPQf());
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
			foreach (KeyValuePair<int, OpskQejDgIZSWovHGUlHGilBlTnF> item in platformVarsDict)
			{
				if (getSetPlatformVariableDict.ContainsKey((int)var))
				{
					getSetPlatformVariableDict[(int)var].GUhNJxVCnnOncLXQZyIcvFXecYAl((Platform)item.Key, value);
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
			return UnityTools.effectivePlatform switch
			{
				Platform.Windows => useEnhancedDeviceSupport, 
				Platform.OSX => osxStandalone_useEnhancedDeviceSupport, 
				_ => false, 
			};
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
			case Platform.Stadia:
				if (!(platformVars is PlatformVars_Stadia))
				{
					return true;
				}
				return (platformVars as PlatformVars_Stadia).useNativeMouse;
			default:
				return false;
			}
		}

		internal bool GetPlatformVar_useNativeKeyboard()
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
				if (!(platformVars is PlatformVars_WindowsStandalone))
				{
					return true;
				}
				return (platformVars as PlatformVars_WindowsStandalone).useNativeKeyboard;
			case Platform.Stadia:
				if (!(platformVars is PlatformVars_Stadia))
				{
					return true;
				}
				return (platformVars as PlatformVars_Stadia).useNativeKeyboard;
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
			switch (UnityTools.effectivePlatform)
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
			default:
				return false;
			}
		}

		internal bool SetPlatformVar_useNativeMouse(bool value)
		{
			if (UnityTools.effectivePlatform == Platform.Windows)
			{
				if (useNativeMouse == value)
				{
					return false;
				}
				useNativeMouse = value;
				return true;
			}
			return false;
		}

		internal bool SetPlatformVar_useNativeKeyboard(bool value)
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
					(platformVars as PlatformVars_WindowsStandalone).useNativeKeyboard = value;
				}
				return true;
			case Platform.Stadia:
				if (platformVars is PlatformVars_Stadia)
				{
					(platformVars as PlatformVars_Stadia).useNativeKeyboard = value;
				}
				return true;
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
			foreach (KeyValuePair<int, OpskQejDgIZSWovHGUlHGilBlTnF> item in platformVarsDict)
			{
				if (!getSetPlatformVariableDict.ContainsKey((int)var))
				{
					continue;
				}
				object obj = getSetPlatformVariableDict[(int)var].KwWcmpxWpdaEZpKWAerfiotksPQf((Platform)item.Key);
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
	}
}
