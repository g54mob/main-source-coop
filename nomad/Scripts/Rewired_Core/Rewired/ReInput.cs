using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rewired.Config;
using Rewired.Data;
using Rewired.Data.Mapping;
using Rewired.InputManagers;
using Rewired.Interfaces;
using Rewired.Internal.Glyphs;
using Rewired.Internal.Localization;
using Rewired.Platforms;
using Rewired.Platforms.Custom;
using Rewired.Platforms.XboxOne;
using Rewired.Utils;
using Rewired.Utils.Classes;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Classes.Utility;
using Rewired.Utils.Interfaces;
using UnityEngine;

namespace Rewired
{
	public static class ReInput
	{
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class LocalizationHelper : CodeHelper
		{
			private static LocalizationHelper KodjcnpEZSFJuXEHBzYGeSEFmoIc;

			internal static LocalizationHelper xmDdvaCOnPkVEcAEwKesZEENVnGDA => KodjcnpEZSFJuXEHBzYGeSEFmoIc ?? (KodjcnpEZSFJuXEHBzYGeSEFmoIc = new LocalizationHelper());

			public ILocalizedStringProvider localizedStringProvider
			{
				get
				{
					if (!CheckInitialized())
					{
						return null;
					}
					return LocalizationManager.localizedStringProvider;
				}
				set
				{
					if (CheckInitialized())
					{
						LocalizationManager.localizedStringProvider = value;
					}
				}
			}

			public bool prefetch
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					return LocalizationManager.autoPrefetch;
				}
				set
				{
					if (CheckInitialized())
					{
						LocalizationManager.autoPrefetch = value;
					}
				}
			}

			private LocalizationHelper()
			{
			}

			internal static void XWQSKMHplnEdgkVcBYaGEfyDfntq()
			{
				KodjcnpEZSFJuXEHBzYGeSEFmoIc = null;
			}

			public void Reload()
			{
				if (CheckInitialized())
				{
					LocalizationManager.Reload();
				}
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class GlyphHelper : CodeHelper
		{
			private static GlyphHelper UhuFBxcbOFxOrspQcYxhsdmlRnTO;

			internal static GlyphHelper xiZZVqZMjABxdoOaHlwXNbVTXsBI => UhuFBxcbOFxOrspQcYxhsdmlRnTO ?? (UhuFBxcbOFxOrspQcYxhsdmlRnTO = new GlyphHelper());

			public IGlyphProvider glyphProvider
			{
				get
				{
					if (!CheckInitialized())
					{
						return null;
					}
					return GlyphManager.glyphProvider;
				}
				set
				{
					if (CheckInitialized())
					{
						GlyphManager.glyphProvider = value;
					}
				}
			}

			public bool prefetch
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					return GlyphManager.autoPrefetch;
				}
				set
				{
					if (CheckInitialized())
					{
						GlyphManager.autoPrefetch = value;
					}
				}
			}

			private GlyphHelper()
			{
			}

			internal static void vPzRRQRnMBsTihxHfMmUuPpkKEJP()
			{
				UhuFBxcbOFxOrspQcYxhsdmlRnTO = null;
			}

			public void Reload()
			{
				if (CheckInitialized())
				{
					GlyphManager.Reload();
				}
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class ConfigHelper : CodeHelper
		{
			private static ConfigHelper GEUQoGzNlNHkeBsMGHNnGayvgBYhA;

			private float tEXqAoSGMJEpEBjwADQPHBpJGomn = 0.7f;

			private float hTjkxZoTQhRJQnndfhiqoCSILFhx = 100f;

			internal static ConfigHelper fRYjhvxxJuAvTfSQOyLzPXfQVjxE => GEUQoGzNlNHkeBsMGHNnGayvgBYhA ?? (GEUQoGzNlNHkeBsMGHNnGayvgBYhA = new ConfigHelper());

			public bool useXInput
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					if (UnityTools.effectivePlatform == Platform.Windows && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsStandalonePrimaryInputSource == WindowsStandalonePrimaryInputSource.XInput)
					{
						return true;
					}
					if (UnityTools.effectivePlatform == Platform.WindowsUWP)
					{
						return windowsUWPSupportGamepads;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.useXInput;
				}
				set
				{
					if (!CheckInitialized())
					{
						return;
					}
					if (UnityTools.effectivePlatform == Platform.WindowsUWP)
					{
						windowsUWPSupportGamepads = value;
					}
					else
					{
						if (yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.useXInput == value)
						{
							return;
						}
						if (value)
						{
							if (yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsStandalonePrimaryInputSource == WindowsStandalonePrimaryInputSource.XInput)
							{
								return;
							}
							if (UnityTools.effectivePlatform == Platform.Windows && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsStandalonePrimaryInputSource != WindowsStandalonePrimaryInputSource.RawInput && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsStandalonePrimaryInputSource != WindowsStandalonePrimaryInputSource.DirectInput && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsStandalonePrimaryInputSource != WindowsStandalonePrimaryInputSource.WindowsGamingInput)
							{
								Logger.LogWarning("XInput cannot be enabled with the current primary input source: " + yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsStandalonePrimaryInputSource);
								return;
							}
						}
						else if (UnityTools.effectivePlatform == Platform.Windows && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsStandalonePrimaryInputSource == WindowsStandalonePrimaryInputSource.XInput)
						{
							yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsStandalonePrimaryInputSource = WindowsStandalonePrimaryInputSource.RawInput;
							Logger.LogWarning("XInput cannot be used with the current primary input source. The primary input source has been changed to Raw Input.");
						}
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.useXInput = value;
						if (cDVMhrNPKDHilizCWRqmajBwasQJ != null)
						{
							cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
						}
					}
				}
			}

			public bool useWindowsGamingInput
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					if (UnityTools.effectivePlatform == Platform.Windows && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsStandalonePrimaryInputSource == WindowsStandalonePrimaryInputSource.WindowsGamingInput)
					{
						return true;
					}
					if (UnityTools.effectivePlatform == Platform.WindowsUWP)
					{
						return windowsUWPSupportGamepads;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.GetPlatformVar_useWindowsGamingInput();
				}
				set
				{
					if (!CheckInitialized())
					{
						return;
					}
					if (UnityTools.effectivePlatform == Platform.WindowsUWP)
					{
						windowsUWPSupportGamepads = value;
					}
					else
					{
						if (yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.GetPlatformVar_useWindowsGamingInput() == value)
						{
							return;
						}
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.SetPlatformVar_useWindowsGamingInput(value);
						if (value)
						{
							if (yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsStandalonePrimaryInputSource == WindowsStandalonePrimaryInputSource.WindowsGamingInput)
							{
								return;
							}
							if (UnityTools.effectivePlatform == Platform.Windows && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsStandalonePrimaryInputSource != WindowsStandalonePrimaryInputSource.RawInput && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsStandalonePrimaryInputSource != WindowsStandalonePrimaryInputSource.DirectInput)
							{
								Logger.LogWarning("Windows Gaming Input cannot be enabled with the current primary input source: " + yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsStandalonePrimaryInputSource);
								return;
							}
						}
						else if (UnityTools.effectivePlatform == Platform.Windows && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsStandalonePrimaryInputSource == WindowsStandalonePrimaryInputSource.WindowsGamingInput)
						{
							yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsStandalonePrimaryInputSource = WindowsStandalonePrimaryInputSource.RawInput;
							Logger.LogWarning("Windows Gaming Input cannot be used with the current primary input source. The primary input source has been changed to Raw Input.");
						}
						if (cDVMhrNPKDHilizCWRqmajBwasQJ != null)
						{
							cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
						}
					}
				}
			}

			public UpdateMode updateMode
			{
				get
				{
					if (!CheckInitialized())
					{
						return UpdateMode.Automatic;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.updateMode;
				}
				set
				{
					if (CheckInitialized() && value != yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.updateMode)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.updateMode = value;
						if (cDVMhrNPKDHilizCWRqmajBwasQJ != null)
						{
							cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
						}
					}
				}
			}

			public UpdateLoopSetting updateLoop
			{
				get
				{
					if (!CheckInitialized())
					{
						return UpdateLoopSetting.Update;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.updateLoop;
				}
				set
				{
					if (CheckInitialized() && value != yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.updateLoop)
					{
						if ((value & UpdateLoopSetting.Update) == 0)
						{
							value |= UpdateLoopSetting.Update;
						}
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.updateLoop = value;
						if (cDVMhrNPKDHilizCWRqmajBwasQJ != null)
						{
							cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
						}
					}
				}
			}

			public WindowsStandalonePrimaryInputSource windowsStandalonePrimaryInputSource
			{
				get
				{
					if (!CheckInitialized())
					{
						return WindowsStandalonePrimaryInputSource.RawInput;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsStandalonePrimaryInputSource;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsStandalonePrimaryInputSource != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsStandalonePrimaryInputSource = value;
						if (UnityTools.effectivePlatform == Platform.Windows && value == WindowsStandalonePrimaryInputSource.XInput)
						{
							yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.useXInput = true;
						}
						if (cDVMhrNPKDHilizCWRqmajBwasQJ != null)
						{
							cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
						}
					}
				}
			}

			public OSXStandalonePrimaryInputSource osxStandalonePrimaryInputSource
			{
				get
				{
					if (!CheckInitialized())
					{
						return OSXStandalonePrimaryInputSource.Native;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.osx_primaryInputSource;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.osx_primaryInputSource != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.osx_primaryInputSource = value;
						if (cDVMhrNPKDHilizCWRqmajBwasQJ != null)
						{
							cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
						}
					}
				}
			}

			public LinuxStandalonePrimaryInputSource linuxStandalonePrimaryInputSource
			{
				get
				{
					if (!CheckInitialized())
					{
						return LinuxStandalonePrimaryInputSource.Native;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.linux_primaryInputSource;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.linux_primaryInputSource != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.linux_primaryInputSource = value;
						if (cDVMhrNPKDHilizCWRqmajBwasQJ != null)
						{
							cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
						}
					}
				}
			}

			public WindowsUWPPrimaryInputSource windowsUWPPrimaryInputSource
			{
				get
				{
					if (!CheckInitialized())
					{
						return WindowsUWPPrimaryInputSource.Native;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsUWP_primaryInputSource;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsUWP_primaryInputSource != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.windowsUWP_primaryInputSource = value;
						if (cDVMhrNPKDHilizCWRqmajBwasQJ != null)
						{
							cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
						}
					}
				}
			}

			public bool windowsUWPSupportHIDDevices
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					return (yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.GetPlatformVars(Platform.WindowsUWP) as ConfigVars.PlatformVars_WindowsUWP).useHIDAPI;
				}
				set
				{
					if (!CheckInitialized())
					{
						return;
					}
					ConfigVars.PlatformVars_WindowsUWP platformVars_WindowsUWP = yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.GetPlatformVars(Platform.WindowsUWP) as ConfigVars.PlatformVars_WindowsUWP;
					if (platformVars_WindowsUWP.useHIDAPI != value)
					{
						platformVars_WindowsUWP.useHIDAPI = value;
						if (cDVMhrNPKDHilizCWRqmajBwasQJ != null)
						{
							cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
						}
					}
				}
			}

			public bool windowsUWPSupportGamepads
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					return (yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.GetPlatformVars(Platform.WindowsUWP) as ConfigVars.PlatformVars_WindowsUWP).useGamepadAPI;
				}
				set
				{
					if (!CheckInitialized())
					{
						return;
					}
					ConfigVars.PlatformVars_WindowsUWP platformVars_WindowsUWP = yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.GetPlatformVars(Platform.WindowsUWP) as ConfigVars.PlatformVars_WindowsUWP;
					if (platformVars_WindowsUWP.useGamepadAPI != value)
					{
						platformVars_WindowsUWP.useGamepadAPI = value;
						if (cDVMhrNPKDHilizCWRqmajBwasQJ != null)
						{
							cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
						}
					}
				}
			}

			public bool useAppleGameControllerFramework
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					if (UnityTools.effectivePlatform == Platform.OSX && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.osx_primaryInputSource == OSXStandalonePrimaryInputSource.GameController)
					{
						return true;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.GetPlatformVar_useAppleGameController();
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.GetPlatformVar_useAppleGameController() != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.SetPlatformVar_useAppleGameController(value);
						if (cDVMhrNPKDHilizCWRqmajBwasQJ != null)
						{
							cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
						}
					}
				}
			}

			public XboxOnePrimaryInputSource xboxOnePrimaryInputSource
			{
				get
				{
					if (!CheckInitialized())
					{
						return XboxOnePrimaryInputSource.Native;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.xboxOne_primaryInputSource;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.xboxOne_primaryInputSource != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.xboxOne_primaryInputSource = value;
						if (cDVMhrNPKDHilizCWRqmajBwasQJ != null)
						{
							cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
						}
					}
				}
			}

			public PS4PrimaryInputSource ps4PrimaryInputSource
			{
				get
				{
					if (!CheckInitialized())
					{
						return PS4PrimaryInputSource.PS4Input;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.ps4_primaryInputSource;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.ps4_primaryInputSource != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.ps4_primaryInputSource = value;
						if (cDVMhrNPKDHilizCWRqmajBwasQJ != null)
						{
							cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
						}
					}
				}
			}

			public WebGLPrimaryInputSource webGLPrimaryInputSource
			{
				get
				{
					if (!CheckInitialized())
					{
						return WebGLPrimaryInputSource.Native;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.webGL_primaryInputSource;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.webGL_primaryInputSource != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.webGL_primaryInputSource = value;
						if (cDVMhrNPKDHilizCWRqmajBwasQJ != null)
						{
							cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
						}
					}
				}
			}

			public bool alwaysUseUnityInput
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.alwaysUseUnityInput;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.alwaysUseUnityInput != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.alwaysUseUnityInput = value;
						if (cDVMhrNPKDHilizCWRqmajBwasQJ != null)
						{
							cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
						}
					}
				}
			}

			public bool disableNativeInput
			{
				get
				{
					return alwaysUseUnityInput;
				}
				set
				{
					alwaysUseUnityInput = value;
				}
			}

			public bool nativeMouseSupport
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.GetPlatformVar_useNativeMouse();
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.SetPlatformVar_useNativeMouse(value) && cDVMhrNPKDHilizCWRqmajBwasQJ != null)
					{
						cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
					}
				}
			}

			public bool nativeKeyboardSupport
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.GetPlatformVar_useNativeKeyboard();
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.SetPlatformVar_useNativeKeyboard(value) && cDVMhrNPKDHilizCWRqmajBwasQJ != null)
					{
						cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
					}
				}
			}

			public bool enhancedDeviceSupport
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.GetPlatformVar_useEnhancedDeviceSupport();
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.SetPlatformVar_useEnhancedDeviceSupport(value) && cDVMhrNPKDHilizCWRqmajBwasQJ != null)
					{
						cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
					}
				}
			}

			public int joystickRefreshRate
			{
				get
				{
					if (!CheckInitialized())
					{
						return 0;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.GetPlatformVar_joystickRefreshRate();
				}
				set
				{
					if (CheckInitialized())
					{
						value = MathTools.Clamp(value, 0, 2000);
						if (value == 0)
						{
							value = 240;
						}
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.SetPlatformVar_joystickRefreshRate(value);
					}
				}
			}

			public bool ignoreInputWhenAppNotInFocus
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.GetPlatformVar_ignoreInputWhenAppNotInFocus();
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.SetPlatformVar_ignoreInputWhenAppNotInFocus(value))
					{
						etvVyNKCZehEyMowuRqYpWXYbskH();
					}
				}
			}

			public bool android_supportUnknownGamepads
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.android_supportUnknownGamepads;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.android_supportUnknownGamepads != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.android_supportUnknownGamepads = value;
						if (cDVMhrNPKDHilizCWRqmajBwasQJ != null)
						{
							cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
						}
					}
				}
			}

			public DeadZone2DType defaultJoystickAxis2DDeadZoneType
			{
				get
				{
					if (!CheckInitialized())
					{
						return DeadZone2DType.Radial;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.defaultJoystickAxis2DDeadZoneType;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.defaultJoystickAxis2DDeadZoneType != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.defaultJoystickAxis2DDeadZoneType = value;
					}
				}
			}

			public AxisSensitivity2DType defaultJoystickAxis2DSensitivityType
			{
				get
				{
					if (!CheckInitialized())
					{
						return AxisSensitivity2DType.Radial;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.defaultJoystickAxis2DSensitivityType;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.defaultJoystickAxis2DSensitivityType != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.defaultJoystickAxis2DSensitivityType = value;
					}
				}
			}

			public AxisSensitivityType defaultAxisSensitivityType
			{
				get
				{
					if (!CheckInitialized())
					{
						return AxisSensitivityType.Multiplier;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.defaultAxisSensitivityType;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.defaultAxisSensitivityType != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.defaultAxisSensitivityType = value;
					}
				}
			}

			public bool force4WayHats
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.force4WayHats;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.force4WayHats != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.force4WayHats = value;
					}
				}
			}

			public float defaultAbsoluteAxisPollingDeadZone
			{
				get
				{
					if (!CheckInitialized())
					{
						return 0.7f;
					}
					return tEXqAoSGMJEpEBjwADQPHBpJGomn;
				}
				set
				{
					if (CheckInitialized())
					{
						if (value < 0f)
						{
							value = 0f;
						}
						if (tEXqAoSGMJEpEBjwADQPHBpJGomn != value)
						{
							tEXqAoSGMJEpEBjwADQPHBpJGomn = value;
						}
					}
				}
			}

			public float defaultRelativeAxisPollingDeadZone
			{
				get
				{
					if (!CheckInitialized())
					{
						return 100f;
					}
					return hTjkxZoTQhRJQnndfhiqoCSILFhx;
				}
				set
				{
					if (CheckInitialized())
					{
						if (value < 0f)
						{
							value = 0f;
						}
						if (hTjkxZoTQhRJQnndfhiqoCSILFhx != value)
						{
							hTjkxZoTQhRJQnndfhiqoCSILFhx = value;
						}
					}
				}
			}

			public bool activateActionButtonsOnNegativeValue
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.activateActionButtonsOnNegativeValue;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.activateActionButtonsOnNegativeValue != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.activateActionButtonsOnNegativeValue = value;
					}
				}
			}

			public ThrottleCalibrationMode throttleCalibrationMode
			{
				get
				{
					if (!CheckInitialized())
					{
						return ThrottleCalibrationMode.ZeroToOne;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.throttleCalibrationMode;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.throttleCalibrationMode != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.throttleCalibrationMode = value;
						FoarDfUMCtoVFquEtrllUhEjZUUn.rYKbJGDlkVucJBbAnkMBYeAFjEQmA(value);
					}
				}
			}

			public bool deferControllerConnectedEventsOnStart
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.deferControllerConnectedEventsOnStart;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.deferControllerConnectedEventsOnStart != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.deferControllerConnectedEventsOnStart = value;
					}
				}
			}

			public KeyCombinationOverrideMode keyCombinationOverrideMode
			{
				get
				{
					if (!CheckInitialized())
					{
						return KeyCombinationOverrideMode.Cancel;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.keyCombinationOverrideMode;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.keyCombinationOverrideMode != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.keyCombinationOverrideMode = value;
					}
				}
			}

			public bool generateKeyEventsOnKeyCombinationOverride
			{
				get
				{
					if (!CheckInitialized())
					{
						return true;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.generateKeyEventsOnKeyCombinationOverride;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.generateKeyEventsOnKeyCombinationOverride != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.generateKeyEventsOnKeyCombinationOverride = value;
					}
				}
			}

			public bool autoAssignJoysticks
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.autoAssignJoysticks;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.autoAssignJoysticks != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.autoAssignJoysticks = value;
					}
				}
			}

			public int maxJoysticksPerPlayer
			{
				get
				{
					if (!CheckInitialized())
					{
						return 0;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.maxJoysticksPerPlayer;
				}
				set
				{
					if (CheckInitialized())
					{
						if (value < 1)
						{
							value = 1;
						}
						if (yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.maxJoysticksPerPlayer != value)
						{
							yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.maxJoysticksPerPlayer = value;
						}
					}
				}
			}

			public bool distributeJoysticksEvenly
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.distributeJoysticksEvenly;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.distributeJoysticksEvenly != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.distributeJoysticksEvenly = value;
					}
				}
			}

			public bool assignJoysticksToPlayingPlayersOnly
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.assignJoysticksToPlayingPlayersOnly;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.assignJoysticksToPlayingPlayersOnly != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.assignJoysticksToPlayingPlayersOnly = value;
					}
				}
			}

			public bool reassignJoystickToPreviousOwnerOnReconnect
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.reassignJoystickToPreviousOwnerOnReconnect;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.reassignJoystickToPreviousOwnerOnReconnect != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.reassignJoystickToPreviousOwnerOnReconnect = value;
					}
				}
			}

			public LogLevelFlags logLevel
			{
				get
				{
					if (!CheckInitialized())
					{
						return LogLevelFlags.Off;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.logLevel;
				}
				set
				{
					if (CheckInitialized() && yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.logLevel != value)
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.logLevel = value;
					}
				}
			}

			public List<EnhancedDeviceSupportDeviceType> enhancedDeviceSupportExcludedDeviceTypes
			{
				get
				{
					if (!CheckInitialized())
					{
						return new List<EnhancedDeviceSupportDeviceType>();
					}
					return new List<EnhancedDeviceSupportDeviceType>(yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.GetPlatformVar_enhancedDeviceSupportExcludedDeviceTypes());
				}
				set
				{
					if (CheckInitialized())
					{
						yFcxJYXlykxThQELuKTiwjJTSDUP.ConfigVars.SetPlatformVar_enhancedDeviceSupportExcludedDeviceTypes(value);
						if (cDVMhrNPKDHilizCWRqmajBwasQJ != null)
						{
							cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
						}
					}
				}
			}

			private ConfigHelper()
			{
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class ControllerHelper : CodeHelper
		{
			[Browsable(false)]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public sealed class PollingHelper : CodeHelper
			{
				private sealed class BrjRuFDIHsSGwyHheVYyjjztsUXt : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int wYdnYTSpNibsVTtgAKRUolZFAcfu;

					private ControllerPollingInfo AgBaEOWmTCYSStXGoEcvvMulYTO;

					private int yQGNNifeTyBUDpWkmaHVHsqsVhzS;

					public PollingHelper dhkUfWCoxGSBjRvnLxefcSrnCephA;

					private IEnumerator<ControllerPollingInfo> TtHVhQNUkOaFxWmbEahopnzdfKnAA;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return AgBaEOWmTCYSStXGoEcvvMulYTO;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return AgBaEOWmTCYSStXGoEcvvMulYTO;
						}
					}

					[DebuggerHidden]
					public BrjRuFDIHsSGwyHheVYyjjztsUXt(int P_0)
					{
						wYdnYTSpNibsVTtgAKRUolZFAcfu = P_0;
						yQGNNifeTyBUDpWkmaHVHsqsVhzS = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						switch (wYdnYTSpNibsVTtgAKRUolZFAcfu)
						{
						case -3:
						case 1:
							try
							{
							}
							finally
							{
								OcImFcwPlDhISqUCeTHWRrFpWZGC();
							}
							break;
						case -4:
						case 2:
							try
							{
							}
							finally
							{
								aqoKCIXPCczCHGbUveJVDhPYPCVcA();
							}
							break;
						case -5:
						case 3:
							try
							{
							}
							finally
							{
								rswonZNScpIQiLodSMBgkpLqVbFs();
							}
							break;
						}
						TtHVhQNUkOaFxWmbEahopnzdfKnAA = null;
						wYdnYTSpNibsVTtgAKRUolZFAcfu = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = wYdnYTSpNibsVTtgAKRUolZFAcfu;
							PollingHelper pollingHelper = dhkUfWCoxGSBjRvnLxefcSrnCephA;
							switch (num)
							{
							default:
								return false;
							case 0:
								wYdnYTSpNibsVTtgAKRUolZFAcfu = -1;
								if (!CheckInitialized())
								{
									return false;
								}
								TtHVhQNUkOaFxWmbEahopnzdfKnAA = pollingHelper.ZmMBBgOffpwEyCNffSYAwDliotYE().GetEnumerator();
								wYdnYTSpNibsVTtgAKRUolZFAcfu = -3;
								goto IL_0084;
							case 1:
								wYdnYTSpNibsVTtgAKRUolZFAcfu = -3;
								goto IL_0084;
							case 2:
								wYdnYTSpNibsVTtgAKRUolZFAcfu = -4;
								goto IL_00e4;
							case 3:
								{
									wYdnYTSpNibsVTtgAKRUolZFAcfu = -5;
									break;
								}
								IL_00e4:
								if (TtHVhQNUkOaFxWmbEahopnzdfKnAA.MoveNext())
								{
									ControllerPollingInfo current = TtHVhQNUkOaFxWmbEahopnzdfKnAA.Current;
									AgBaEOWmTCYSStXGoEcvvMulYTO = current;
									wYdnYTSpNibsVTtgAKRUolZFAcfu = 2;
									return true;
								}
								aqoKCIXPCczCHGbUveJVDhPYPCVcA();
								TtHVhQNUkOaFxWmbEahopnzdfKnAA = null;
								TtHVhQNUkOaFxWmbEahopnzdfKnAA = pollingHelper.TUtmdzzCWVVIpbETlFbhTPOZPEPr().GetEnumerator();
								wYdnYTSpNibsVTtgAKRUolZFAcfu = -5;
								break;
								IL_0084:
								if (TtHVhQNUkOaFxWmbEahopnzdfKnAA.MoveNext())
								{
									ControllerPollingInfo current2 = TtHVhQNUkOaFxWmbEahopnzdfKnAA.Current;
									AgBaEOWmTCYSStXGoEcvvMulYTO = current2;
									wYdnYTSpNibsVTtgAKRUolZFAcfu = 1;
									return true;
								}
								OcImFcwPlDhISqUCeTHWRrFpWZGC();
								TtHVhQNUkOaFxWmbEahopnzdfKnAA = null;
								TtHVhQNUkOaFxWmbEahopnzdfKnAA = pollingHelper.JiZdeCxZjDEqKzZUICvpLVpUVVUg().GetEnumerator();
								wYdnYTSpNibsVTtgAKRUolZFAcfu = -4;
								goto IL_00e4;
							}
							if (TtHVhQNUkOaFxWmbEahopnzdfKnAA.MoveNext())
							{
								ControllerPollingInfo current3 = TtHVhQNUkOaFxWmbEahopnzdfKnAA.Current;
								AgBaEOWmTCYSStXGoEcvvMulYTO = current3;
								wYdnYTSpNibsVTtgAKRUolZFAcfu = 3;
								return true;
							}
							rswonZNScpIQiLodSMBgkpLqVbFs();
							TtHVhQNUkOaFxWmbEahopnzdfKnAA = null;
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void OcImFcwPlDhISqUCeTHWRrFpWZGC()
					{
						wYdnYTSpNibsVTtgAKRUolZFAcfu = -1;
						if (TtHVhQNUkOaFxWmbEahopnzdfKnAA != null)
						{
							TtHVhQNUkOaFxWmbEahopnzdfKnAA.Dispose();
						}
					}

					private void aqoKCIXPCczCHGbUveJVDhPYPCVcA()
					{
						wYdnYTSpNibsVTtgAKRUolZFAcfu = -1;
						if (TtHVhQNUkOaFxWmbEahopnzdfKnAA != null)
						{
							TtHVhQNUkOaFxWmbEahopnzdfKnAA.Dispose();
						}
					}

					private void rswonZNScpIQiLodSMBgkpLqVbFs()
					{
						wYdnYTSpNibsVTtgAKRUolZFAcfu = -1;
						if (TtHVhQNUkOaFxWmbEahopnzdfKnAA != null)
						{
							TtHVhQNUkOaFxWmbEahopnzdfKnAA.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						BrjRuFDIHsSGwyHheVYyjjztsUXt brjRuFDIHsSGwyHheVYyjjztsUXt;
						if (wYdnYTSpNibsVTtgAKRUolZFAcfu == -2 && yQGNNifeTyBUDpWkmaHVHsqsVhzS == Environment.CurrentManagedThreadId)
						{
							wYdnYTSpNibsVTtgAKRUolZFAcfu = 0;
							brjRuFDIHsSGwyHheVYyjjztsUXt = this;
						}
						else
						{
							brjRuFDIHsSGwyHheVYyjjztsUXt = new BrjRuFDIHsSGwyHheVYyjjztsUXt(0);
							brjRuFDIHsSGwyHheVYyjjztsUXt.dhkUfWCoxGSBjRvnLxefcSrnCephA = dhkUfWCoxGSBjRvnLxefcSrnCephA;
						}
						return brjRuFDIHsSGwyHheVYyjjztsUXt;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class ttCxvEeWWhhSDWcCreltbGFyBzsW : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int vageDdkLuWxKEFBRvKtNAEIspKzeb;

					private ControllerPollingInfo ytOnerElOVgLgCQzzqGCQXzWSYlr;

					private int jCvURHNiMiEpcEozadndWXytAHI;

					public PollingHelper irYgbNeTzCdHweitiJlaorkwCwKEA;

					private IEnumerator<ControllerPollingInfo> TaOAUeEwPHJfBnPArlhXjylUaKHE;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return ytOnerElOVgLgCQzzqGCQXzWSYlr;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return ytOnerElOVgLgCQzzqGCQXzWSYlr;
						}
					}

					[DebuggerHidden]
					public ttCxvEeWWhhSDWcCreltbGFyBzsW(int P_0)
					{
						vageDdkLuWxKEFBRvKtNAEIspKzeb = P_0;
						jCvURHNiMiEpcEozadndWXytAHI = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						switch (vageDdkLuWxKEFBRvKtNAEIspKzeb)
						{
						case -3:
						case 1:
							try
							{
							}
							finally
							{
								MpkMnqbJLZRtbBkeOcJqIaAjiaQEA();
							}
							break;
						case -4:
						case 2:
							try
							{
							}
							finally
							{
								ernwTTKGkFMKZmxEHHgEUXSKbzvj();
							}
							break;
						case -5:
						case 3:
							try
							{
							}
							finally
							{
								bbGeNsZQgImfjbWJYKXfqGgFZCiJ();
							}
							break;
						case -6:
						case 4:
							try
							{
							}
							finally
							{
								zCzujceCJEqSHDgcvHXSyVfegsai();
							}
							break;
						}
						TaOAUeEwPHJfBnPArlhXjylUaKHE = null;
						vageDdkLuWxKEFBRvKtNAEIspKzeb = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = vageDdkLuWxKEFBRvKtNAEIspKzeb;
							PollingHelper pollingHelper = irYgbNeTzCdHweitiJlaorkwCwKEA;
							switch (num)
							{
							default:
								return false;
							case 0:
								vageDdkLuWxKEFBRvKtNAEIspKzeb = -1;
								if (!CheckInitialized())
								{
									return false;
								}
								TaOAUeEwPHJfBnPArlhXjylUaKHE = pollingHelper.thRiQkrQSyxoOaMVlGZxADGsfZNd().GetEnumerator();
								vageDdkLuWxKEFBRvKtNAEIspKzeb = -3;
								goto IL_0088;
							case 1:
								vageDdkLuWxKEFBRvKtNAEIspKzeb = -3;
								goto IL_0088;
							case 2:
								vageDdkLuWxKEFBRvKtNAEIspKzeb = -4;
								goto IL_00e8;
							case 3:
								vageDdkLuWxKEFBRvKtNAEIspKzeb = -5;
								goto IL_0148;
							case 4:
								{
									vageDdkLuWxKEFBRvKtNAEIspKzeb = -6;
									break;
								}
								IL_00e8:
								if (TaOAUeEwPHJfBnPArlhXjylUaKHE.MoveNext())
								{
									ControllerPollingInfo current = TaOAUeEwPHJfBnPArlhXjylUaKHE.Current;
									ytOnerElOVgLgCQzzqGCQXzWSYlr = current;
									vageDdkLuWxKEFBRvKtNAEIspKzeb = 2;
									return true;
								}
								ernwTTKGkFMKZmxEHHgEUXSKbzvj();
								TaOAUeEwPHJfBnPArlhXjylUaKHE = null;
								TaOAUeEwPHJfBnPArlhXjylUaKHE = pollingHelper.esgGJWDkYFOFcyUmAeBDOuurVDhdA().GetEnumerator();
								vageDdkLuWxKEFBRvKtNAEIspKzeb = -5;
								goto IL_0148;
								IL_0088:
								if (TaOAUeEwPHJfBnPArlhXjylUaKHE.MoveNext())
								{
									ControllerPollingInfo current2 = TaOAUeEwPHJfBnPArlhXjylUaKHE.Current;
									ytOnerElOVgLgCQzzqGCQXzWSYlr = current2;
									vageDdkLuWxKEFBRvKtNAEIspKzeb = 1;
									return true;
								}
								MpkMnqbJLZRtbBkeOcJqIaAjiaQEA();
								TaOAUeEwPHJfBnPArlhXjylUaKHE = null;
								TaOAUeEwPHJfBnPArlhXjylUaKHE = pollingHelper.AralTjnEeTrjVgILEpmyVrrHgnHU().GetEnumerator();
								vageDdkLuWxKEFBRvKtNAEIspKzeb = -4;
								goto IL_00e8;
								IL_0148:
								if (TaOAUeEwPHJfBnPArlhXjylUaKHE.MoveNext())
								{
									ControllerPollingInfo current3 = TaOAUeEwPHJfBnPArlhXjylUaKHE.Current;
									ytOnerElOVgLgCQzzqGCQXzWSYlr = current3;
									vageDdkLuWxKEFBRvKtNAEIspKzeb = 3;
									return true;
								}
								bbGeNsZQgImfjbWJYKXfqGgFZCiJ();
								TaOAUeEwPHJfBnPArlhXjylUaKHE = null;
								TaOAUeEwPHJfBnPArlhXjylUaKHE = pollingHelper.rSsEkOhrFkEUUvEeQkgOIrTpbCVT().GetEnumerator();
								vageDdkLuWxKEFBRvKtNAEIspKzeb = -6;
								break;
							}
							if (TaOAUeEwPHJfBnPArlhXjylUaKHE.MoveNext())
							{
								ControllerPollingInfo current4 = TaOAUeEwPHJfBnPArlhXjylUaKHE.Current;
								ytOnerElOVgLgCQzzqGCQXzWSYlr = current4;
								vageDdkLuWxKEFBRvKtNAEIspKzeb = 4;
								return true;
							}
							zCzujceCJEqSHDgcvHXSyVfegsai();
							TaOAUeEwPHJfBnPArlhXjylUaKHE = null;
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void MpkMnqbJLZRtbBkeOcJqIaAjiaQEA()
					{
						vageDdkLuWxKEFBRvKtNAEIspKzeb = -1;
						if (TaOAUeEwPHJfBnPArlhXjylUaKHE != null)
						{
							TaOAUeEwPHJfBnPArlhXjylUaKHE.Dispose();
						}
					}

					private void ernwTTKGkFMKZmxEHHgEUXSKbzvj()
					{
						vageDdkLuWxKEFBRvKtNAEIspKzeb = -1;
						if (TaOAUeEwPHJfBnPArlhXjylUaKHE != null)
						{
							TaOAUeEwPHJfBnPArlhXjylUaKHE.Dispose();
						}
					}

					private void bbGeNsZQgImfjbWJYKXfqGgFZCiJ()
					{
						vageDdkLuWxKEFBRvKtNAEIspKzeb = -1;
						if (TaOAUeEwPHJfBnPArlhXjylUaKHE != null)
						{
							TaOAUeEwPHJfBnPArlhXjylUaKHE.Dispose();
						}
					}

					private void zCzujceCJEqSHDgcvHXSyVfegsai()
					{
						vageDdkLuWxKEFBRvKtNAEIspKzeb = -1;
						if (TaOAUeEwPHJfBnPArlhXjylUaKHE != null)
						{
							TaOAUeEwPHJfBnPArlhXjylUaKHE.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						ttCxvEeWWhhSDWcCreltbGFyBzsW ttCxvEeWWhhSDWcCreltbGFyBzsW2;
						if (vageDdkLuWxKEFBRvKtNAEIspKzeb == -2 && jCvURHNiMiEpcEozadndWXytAHI == Environment.CurrentManagedThreadId)
						{
							vageDdkLuWxKEFBRvKtNAEIspKzeb = 0;
							ttCxvEeWWhhSDWcCreltbGFyBzsW2 = this;
						}
						else
						{
							ttCxvEeWWhhSDWcCreltbGFyBzsW2 = new ttCxvEeWWhhSDWcCreltbGFyBzsW(0);
							ttCxvEeWWhhSDWcCreltbGFyBzsW2.irYgbNeTzCdHweitiJlaorkwCwKEA = irYgbNeTzCdHweitiJlaorkwCwKEA;
						}
						return ttCxvEeWWhhSDWcCreltbGFyBzsW2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class lOlwLAkLyieOCHXcxLSmHFQdFbzCb : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int rlDvdEwbhudUfATjGoaJHNkIDygb;

					private ControllerPollingInfo sKmKMRnacjdkEckOYQzzcPDoetqgA;

					private int kOBJYbqRdVwtAXoMzxTfiWXgBlIw;

					public PollingHelper IPpbfIhhnMltWrFAXhObySTLUnar;

					private IEnumerator<ControllerPollingInfo> CkIlNIRSxirfpTrctODcZsCkPSWs;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return sKmKMRnacjdkEckOYQzzcPDoetqgA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return sKmKMRnacjdkEckOYQzzcPDoetqgA;
						}
					}

					[DebuggerHidden]
					public lOlwLAkLyieOCHXcxLSmHFQdFbzCb(int P_0)
					{
						rlDvdEwbhudUfATjGoaJHNkIDygb = P_0;
						kOBJYbqRdVwtAXoMzxTfiWXgBlIw = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						switch (rlDvdEwbhudUfATjGoaJHNkIDygb)
						{
						case -3:
						case 1:
							try
							{
							}
							finally
							{
								iGujfMsdXHtfFTKmLaWqcIxuHLtsA();
							}
							break;
						case -4:
						case 2:
							try
							{
							}
							finally
							{
								RhAbFbIkyrQTsMdfWVQivIIOsgFw();
							}
							break;
						case -5:
						case 3:
							try
							{
							}
							finally
							{
								BJjJbvUiYOjUEFwuMQzbJgMalCzG();
							}
							break;
						case -6:
						case 4:
							try
							{
							}
							finally
							{
								ROGaDOCiwzuXkYXDINoNGXNztxkp();
							}
							break;
						}
						CkIlNIRSxirfpTrctODcZsCkPSWs = null;
						rlDvdEwbhudUfATjGoaJHNkIDygb = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = rlDvdEwbhudUfATjGoaJHNkIDygb;
							PollingHelper iPpbfIhhnMltWrFAXhObySTLUnar = IPpbfIhhnMltWrFAXhObySTLUnar;
							switch (num)
							{
							default:
								return false;
							case 0:
								rlDvdEwbhudUfATjGoaJHNkIDygb = -1;
								if (!CheckInitialized())
								{
									return false;
								}
								CkIlNIRSxirfpTrctODcZsCkPSWs = iPpbfIhhnMltWrFAXhObySTLUnar.cazgkEfyQtgpuekViIVRtGHiioIY().GetEnumerator();
								rlDvdEwbhudUfATjGoaJHNkIDygb = -3;
								goto IL_0088;
							case 1:
								rlDvdEwbhudUfATjGoaJHNkIDygb = -3;
								goto IL_0088;
							case 2:
								rlDvdEwbhudUfATjGoaJHNkIDygb = -4;
								goto IL_00e8;
							case 3:
								rlDvdEwbhudUfATjGoaJHNkIDygb = -5;
								goto IL_0148;
							case 4:
								{
									rlDvdEwbhudUfATjGoaJHNkIDygb = -6;
									break;
								}
								IL_00e8:
								if (CkIlNIRSxirfpTrctODcZsCkPSWs.MoveNext())
								{
									ControllerPollingInfo current = CkIlNIRSxirfpTrctODcZsCkPSWs.Current;
									sKmKMRnacjdkEckOYQzzcPDoetqgA = current;
									rlDvdEwbhudUfATjGoaJHNkIDygb = 2;
									return true;
								}
								RhAbFbIkyrQTsMdfWVQivIIOsgFw();
								CkIlNIRSxirfpTrctODcZsCkPSWs = null;
								CkIlNIRSxirfpTrctODcZsCkPSWs = iPpbfIhhnMltWrFAXhObySTLUnar.ChhFaxHlpAHDtYUNYNcrfxZbKFeOA().GetEnumerator();
								rlDvdEwbhudUfATjGoaJHNkIDygb = -5;
								goto IL_0148;
								IL_0088:
								if (CkIlNIRSxirfpTrctODcZsCkPSWs.MoveNext())
								{
									ControllerPollingInfo current2 = CkIlNIRSxirfpTrctODcZsCkPSWs.Current;
									sKmKMRnacjdkEckOYQzzcPDoetqgA = current2;
									rlDvdEwbhudUfATjGoaJHNkIDygb = 1;
									return true;
								}
								iGujfMsdXHtfFTKmLaWqcIxuHLtsA();
								CkIlNIRSxirfpTrctODcZsCkPSWs = null;
								CkIlNIRSxirfpTrctODcZsCkPSWs = iPpbfIhhnMltWrFAXhObySTLUnar.pKRCAfvRKFPFTaZyOsLCeMnboKlF().GetEnumerator();
								rlDvdEwbhudUfATjGoaJHNkIDygb = -4;
								goto IL_00e8;
								IL_0148:
								if (CkIlNIRSxirfpTrctODcZsCkPSWs.MoveNext())
								{
									ControllerPollingInfo current3 = CkIlNIRSxirfpTrctODcZsCkPSWs.Current;
									sKmKMRnacjdkEckOYQzzcPDoetqgA = current3;
									rlDvdEwbhudUfATjGoaJHNkIDygb = 3;
									return true;
								}
								BJjJbvUiYOjUEFwuMQzbJgMalCzG();
								CkIlNIRSxirfpTrctODcZsCkPSWs = null;
								CkIlNIRSxirfpTrctODcZsCkPSWs = iPpbfIhhnMltWrFAXhObySTLUnar.zQIqJPaFGQpllVnHkCSzbRrAsKWUA().GetEnumerator();
								rlDvdEwbhudUfATjGoaJHNkIDygb = -6;
								break;
							}
							if (CkIlNIRSxirfpTrctODcZsCkPSWs.MoveNext())
							{
								ControllerPollingInfo current4 = CkIlNIRSxirfpTrctODcZsCkPSWs.Current;
								sKmKMRnacjdkEckOYQzzcPDoetqgA = current4;
								rlDvdEwbhudUfATjGoaJHNkIDygb = 4;
								return true;
							}
							ROGaDOCiwzuXkYXDINoNGXNztxkp();
							CkIlNIRSxirfpTrctODcZsCkPSWs = null;
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void iGujfMsdXHtfFTKmLaWqcIxuHLtsA()
					{
						rlDvdEwbhudUfATjGoaJHNkIDygb = -1;
						if (CkIlNIRSxirfpTrctODcZsCkPSWs != null)
						{
							CkIlNIRSxirfpTrctODcZsCkPSWs.Dispose();
						}
					}

					private void RhAbFbIkyrQTsMdfWVQivIIOsgFw()
					{
						rlDvdEwbhudUfATjGoaJHNkIDygb = -1;
						if (CkIlNIRSxirfpTrctODcZsCkPSWs != null)
						{
							CkIlNIRSxirfpTrctODcZsCkPSWs.Dispose();
						}
					}

					private void BJjJbvUiYOjUEFwuMQzbJgMalCzG()
					{
						rlDvdEwbhudUfATjGoaJHNkIDygb = -1;
						if (CkIlNIRSxirfpTrctODcZsCkPSWs != null)
						{
							CkIlNIRSxirfpTrctODcZsCkPSWs.Dispose();
						}
					}

					private void ROGaDOCiwzuXkYXDINoNGXNztxkp()
					{
						rlDvdEwbhudUfATjGoaJHNkIDygb = -1;
						if (CkIlNIRSxirfpTrctODcZsCkPSWs != null)
						{
							CkIlNIRSxirfpTrctODcZsCkPSWs.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						lOlwLAkLyieOCHXcxLSmHFQdFbzCb lOlwLAkLyieOCHXcxLSmHFQdFbzCb2;
						if (rlDvdEwbhudUfATjGoaJHNkIDygb == -2 && kOBJYbqRdVwtAXoMzxTfiWXgBlIw == Environment.CurrentManagedThreadId)
						{
							rlDvdEwbhudUfATjGoaJHNkIDygb = 0;
							lOlwLAkLyieOCHXcxLSmHFQdFbzCb2 = this;
						}
						else
						{
							lOlwLAkLyieOCHXcxLSmHFQdFbzCb2 = new lOlwLAkLyieOCHXcxLSmHFQdFbzCb(0);
							lOlwLAkLyieOCHXcxLSmHFQdFbzCb2.IPpbfIhhnMltWrFAXhObySTLUnar = IPpbfIhhnMltWrFAXhObySTLUnar;
						}
						return lOlwLAkLyieOCHXcxLSmHFQdFbzCb2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class guqQPinnHzMHXuJEyaNXLoGjBxkG : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int oVAQSvOtdluZKtZsBtyoKPYYJKLu;

					private ControllerPollingInfo PpFCoEeGYLpXiBIIgCpsMGwXHzrOc;

					private int xhxERzEBhbQUeIvUthcyMfoJyPvNA;

					public PollingHelper ssddRVitMxjvaukdMhlnyucjYAPJA;

					private IEnumerator<ControllerPollingInfo> EdEJhPBDvilYUHipRbcmLUHozOaA;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return PpFCoEeGYLpXiBIIgCpsMGwXHzrOc;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return PpFCoEeGYLpXiBIIgCpsMGwXHzrOc;
						}
					}

					[DebuggerHidden]
					public guqQPinnHzMHXuJEyaNXLoGjBxkG(int P_0)
					{
						oVAQSvOtdluZKtZsBtyoKPYYJKLu = P_0;
						xhxERzEBhbQUeIvUthcyMfoJyPvNA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						switch (oVAQSvOtdluZKtZsBtyoKPYYJKLu)
						{
						case -3:
						case 1:
							try
							{
							}
							finally
							{
								kphsRlMFwzBmLnBAOlygMUDPnkXJ();
							}
							break;
						case -4:
						case 2:
							try
							{
							}
							finally
							{
								xptNrutSRfTuBcKpQmNHioGFdLyp();
							}
							break;
						case -5:
						case 3:
							try
							{
							}
							finally
							{
								szNLvASUfVDeLcwGRSUTNuamVcQy();
							}
							break;
						case -6:
						case 4:
							try
							{
							}
							finally
							{
								AfjOjappHYaJvDduLHQxwIiOAcAhA();
							}
							break;
						}
						EdEJhPBDvilYUHipRbcmLUHozOaA = null;
						oVAQSvOtdluZKtZsBtyoKPYYJKLu = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = oVAQSvOtdluZKtZsBtyoKPYYJKLu;
							PollingHelper pollingHelper = ssddRVitMxjvaukdMhlnyucjYAPJA;
							switch (num)
							{
							default:
								return false;
							case 0:
								oVAQSvOtdluZKtZsBtyoKPYYJKLu = -1;
								if (!CheckInitialized())
								{
									return false;
								}
								EdEJhPBDvilYUHipRbcmLUHozOaA = pollingHelper.wWkhtiljHyhAcETFSqRXNnspaNkeb().GetEnumerator();
								oVAQSvOtdluZKtZsBtyoKPYYJKLu = -3;
								goto IL_0088;
							case 1:
								oVAQSvOtdluZKtZsBtyoKPYYJKLu = -3;
								goto IL_0088;
							case 2:
								oVAQSvOtdluZKtZsBtyoKPYYJKLu = -4;
								goto IL_00e8;
							case 3:
								oVAQSvOtdluZKtZsBtyoKPYYJKLu = -5;
								goto IL_0148;
							case 4:
								{
									oVAQSvOtdluZKtZsBtyoKPYYJKLu = -6;
									break;
								}
								IL_00e8:
								if (EdEJhPBDvilYUHipRbcmLUHozOaA.MoveNext())
								{
									ControllerPollingInfo current = EdEJhPBDvilYUHipRbcmLUHozOaA.Current;
									PpFCoEeGYLpXiBIIgCpsMGwXHzrOc = current;
									oVAQSvOtdluZKtZsBtyoKPYYJKLu = 2;
									return true;
								}
								xptNrutSRfTuBcKpQmNHioGFdLyp();
								EdEJhPBDvilYUHipRbcmLUHozOaA = null;
								EdEJhPBDvilYUHipRbcmLUHozOaA = pollingHelper.CaqPIWVPoFHbENQzcOgZrJmkoodh().GetEnumerator();
								oVAQSvOtdluZKtZsBtyoKPYYJKLu = -5;
								goto IL_0148;
								IL_0088:
								if (EdEJhPBDvilYUHipRbcmLUHozOaA.MoveNext())
								{
									ControllerPollingInfo current2 = EdEJhPBDvilYUHipRbcmLUHozOaA.Current;
									PpFCoEeGYLpXiBIIgCpsMGwXHzrOc = current2;
									oVAQSvOtdluZKtZsBtyoKPYYJKLu = 1;
									return true;
								}
								kphsRlMFwzBmLnBAOlygMUDPnkXJ();
								EdEJhPBDvilYUHipRbcmLUHozOaA = null;
								EdEJhPBDvilYUHipRbcmLUHozOaA = pollingHelper.AralTjnEeTrjVgILEpmyVrrHgnHU().GetEnumerator();
								oVAQSvOtdluZKtZsBtyoKPYYJKLu = -4;
								goto IL_00e8;
								IL_0148:
								if (EdEJhPBDvilYUHipRbcmLUHozOaA.MoveNext())
								{
									ControllerPollingInfo current3 = EdEJhPBDvilYUHipRbcmLUHozOaA.Current;
									PpFCoEeGYLpXiBIIgCpsMGwXHzrOc = current3;
									oVAQSvOtdluZKtZsBtyoKPYYJKLu = 3;
									return true;
								}
								szNLvASUfVDeLcwGRSUTNuamVcQy();
								EdEJhPBDvilYUHipRbcmLUHozOaA = null;
								EdEJhPBDvilYUHipRbcmLUHozOaA = pollingHelper.OKUJnWbReCGIAWmstMwqNjwfDPbJ().GetEnumerator();
								oVAQSvOtdluZKtZsBtyoKPYYJKLu = -6;
								break;
							}
							if (EdEJhPBDvilYUHipRbcmLUHozOaA.MoveNext())
							{
								ControllerPollingInfo current4 = EdEJhPBDvilYUHipRbcmLUHozOaA.Current;
								PpFCoEeGYLpXiBIIgCpsMGwXHzrOc = current4;
								oVAQSvOtdluZKtZsBtyoKPYYJKLu = 4;
								return true;
							}
							AfjOjappHYaJvDduLHQxwIiOAcAhA();
							EdEJhPBDvilYUHipRbcmLUHozOaA = null;
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void kphsRlMFwzBmLnBAOlygMUDPnkXJ()
					{
						oVAQSvOtdluZKtZsBtyoKPYYJKLu = -1;
						if (EdEJhPBDvilYUHipRbcmLUHozOaA != null)
						{
							EdEJhPBDvilYUHipRbcmLUHozOaA.Dispose();
						}
					}

					private void xptNrutSRfTuBcKpQmNHioGFdLyp()
					{
						oVAQSvOtdluZKtZsBtyoKPYYJKLu = -1;
						if (EdEJhPBDvilYUHipRbcmLUHozOaA != null)
						{
							EdEJhPBDvilYUHipRbcmLUHozOaA.Dispose();
						}
					}

					private void szNLvASUfVDeLcwGRSUTNuamVcQy()
					{
						oVAQSvOtdluZKtZsBtyoKPYYJKLu = -1;
						if (EdEJhPBDvilYUHipRbcmLUHozOaA != null)
						{
							EdEJhPBDvilYUHipRbcmLUHozOaA.Dispose();
						}
					}

					private void AfjOjappHYaJvDduLHQxwIiOAcAhA()
					{
						oVAQSvOtdluZKtZsBtyoKPYYJKLu = -1;
						if (EdEJhPBDvilYUHipRbcmLUHozOaA != null)
						{
							EdEJhPBDvilYUHipRbcmLUHozOaA.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						guqQPinnHzMHXuJEyaNXLoGjBxkG guqQPinnHzMHXuJEyaNXLoGjBxkG2;
						if (oVAQSvOtdluZKtZsBtyoKPYYJKLu == -2 && xhxERzEBhbQUeIvUthcyMfoJyPvNA == Environment.CurrentManagedThreadId)
						{
							oVAQSvOtdluZKtZsBtyoKPYYJKLu = 0;
							guqQPinnHzMHXuJEyaNXLoGjBxkG2 = this;
						}
						else
						{
							guqQPinnHzMHXuJEyaNXLoGjBxkG2 = new guqQPinnHzMHXuJEyaNXLoGjBxkG(0);
							guqQPinnHzMHXuJEyaNXLoGjBxkG2.ssddRVitMxjvaukdMhlnyucjYAPJA = ssddRVitMxjvaukdMhlnyucjYAPJA;
						}
						return guqQPinnHzMHXuJEyaNXLoGjBxkG2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class YlxNychZMQJokQYuChURojZbcPtO : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int BnaFFkYTbhMeDjUOLtNFBYMWScBO;

					private ControllerPollingInfo BVTBnMClZHeKxmWakXgeMaFRaZLPA;

					private int fEvqumXHooCTaxeajaiWsPMDtcAG;

					public PollingHelper ZcOVniEhcydAfERuUkKQjxOfUXYuB;

					private IEnumerator<ControllerPollingInfo> wcYASsrMmbAxYkOpRSACLnuAhDvV;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return BVTBnMClZHeKxmWakXgeMaFRaZLPA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return BVTBnMClZHeKxmWakXgeMaFRaZLPA;
						}
					}

					[DebuggerHidden]
					public YlxNychZMQJokQYuChURojZbcPtO(int P_0)
					{
						BnaFFkYTbhMeDjUOLtNFBYMWScBO = P_0;
						fEvqumXHooCTaxeajaiWsPMDtcAG = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						switch (BnaFFkYTbhMeDjUOLtNFBYMWScBO)
						{
						case -3:
						case 1:
							try
							{
							}
							finally
							{
								GihkCgpFdpPsoEHdDIGEjGmelTYA();
							}
							break;
						case -4:
						case 2:
							try
							{
							}
							finally
							{
								VeiWJoHwHEvnpkEtMDLMwjTEUTPF();
							}
							break;
						case -5:
						case 3:
							try
							{
							}
							finally
							{
								SdxSjHcfcUlGLHyNUtQNLLJifvBj();
							}
							break;
						case -6:
						case 4:
							try
							{
							}
							finally
							{
								JEYhiuHgXBTTyatFTlpjuKiIRuZA();
							}
							break;
						}
						wcYASsrMmbAxYkOpRSACLnuAhDvV = null;
						BnaFFkYTbhMeDjUOLtNFBYMWScBO = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int bnaFFkYTbhMeDjUOLtNFBYMWScBO = BnaFFkYTbhMeDjUOLtNFBYMWScBO;
							PollingHelper zcOVniEhcydAfERuUkKQjxOfUXYuB = ZcOVniEhcydAfERuUkKQjxOfUXYuB;
							switch (bnaFFkYTbhMeDjUOLtNFBYMWScBO)
							{
							default:
								return false;
							case 0:
								BnaFFkYTbhMeDjUOLtNFBYMWScBO = -1;
								if (!CheckInitialized())
								{
									return false;
								}
								wcYASsrMmbAxYkOpRSACLnuAhDvV = zcOVniEhcydAfERuUkKQjxOfUXYuB.cJKKupGVtWEfdBakngsxJWVKxwXW().GetEnumerator();
								BnaFFkYTbhMeDjUOLtNFBYMWScBO = -3;
								goto IL_0088;
							case 1:
								BnaFFkYTbhMeDjUOLtNFBYMWScBO = -3;
								goto IL_0088;
							case 2:
								BnaFFkYTbhMeDjUOLtNFBYMWScBO = -4;
								goto IL_00e8;
							case 3:
								BnaFFkYTbhMeDjUOLtNFBYMWScBO = -5;
								goto IL_0148;
							case 4:
								{
									BnaFFkYTbhMeDjUOLtNFBYMWScBO = -6;
									break;
								}
								IL_00e8:
								if (wcYASsrMmbAxYkOpRSACLnuAhDvV.MoveNext())
								{
									ControllerPollingInfo current = wcYASsrMmbAxYkOpRSACLnuAhDvV.Current;
									BVTBnMClZHeKxmWakXgeMaFRaZLPA = current;
									BnaFFkYTbhMeDjUOLtNFBYMWScBO = 2;
									return true;
								}
								VeiWJoHwHEvnpkEtMDLMwjTEUTPF();
								wcYASsrMmbAxYkOpRSACLnuAhDvV = null;
								wcYASsrMmbAxYkOpRSACLnuAhDvV = zcOVniEhcydAfERuUkKQjxOfUXYuB.GwsEnXpiGninpPTBBlWqHVrxbYSn().GetEnumerator();
								BnaFFkYTbhMeDjUOLtNFBYMWScBO = -5;
								goto IL_0148;
								IL_0088:
								if (wcYASsrMmbAxYkOpRSACLnuAhDvV.MoveNext())
								{
									ControllerPollingInfo current2 = wcYASsrMmbAxYkOpRSACLnuAhDvV.Current;
									BVTBnMClZHeKxmWakXgeMaFRaZLPA = current2;
									BnaFFkYTbhMeDjUOLtNFBYMWScBO = 1;
									return true;
								}
								GihkCgpFdpPsoEHdDIGEjGmelTYA();
								wcYASsrMmbAxYkOpRSACLnuAhDvV = null;
								wcYASsrMmbAxYkOpRSACLnuAhDvV = zcOVniEhcydAfERuUkKQjxOfUXYuB.pKRCAfvRKFPFTaZyOsLCeMnboKlF().GetEnumerator();
								BnaFFkYTbhMeDjUOLtNFBYMWScBO = -4;
								goto IL_00e8;
								IL_0148:
								if (wcYASsrMmbAxYkOpRSACLnuAhDvV.MoveNext())
								{
									ControllerPollingInfo current3 = wcYASsrMmbAxYkOpRSACLnuAhDvV.Current;
									BVTBnMClZHeKxmWakXgeMaFRaZLPA = current3;
									BnaFFkYTbhMeDjUOLtNFBYMWScBO = 3;
									return true;
								}
								SdxSjHcfcUlGLHyNUtQNLLJifvBj();
								wcYASsrMmbAxYkOpRSACLnuAhDvV = null;
								wcYASsrMmbAxYkOpRSACLnuAhDvV = zcOVniEhcydAfERuUkKQjxOfUXYuB.YtRgBWTWlRTCXylGhdlBZxlfQXWT().GetEnumerator();
								BnaFFkYTbhMeDjUOLtNFBYMWScBO = -6;
								break;
							}
							if (wcYASsrMmbAxYkOpRSACLnuAhDvV.MoveNext())
							{
								ControllerPollingInfo current4 = wcYASsrMmbAxYkOpRSACLnuAhDvV.Current;
								BVTBnMClZHeKxmWakXgeMaFRaZLPA = current4;
								BnaFFkYTbhMeDjUOLtNFBYMWScBO = 4;
								return true;
							}
							JEYhiuHgXBTTyatFTlpjuKiIRuZA();
							wcYASsrMmbAxYkOpRSACLnuAhDvV = null;
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void GihkCgpFdpPsoEHdDIGEjGmelTYA()
					{
						BnaFFkYTbhMeDjUOLtNFBYMWScBO = -1;
						if (wcYASsrMmbAxYkOpRSACLnuAhDvV != null)
						{
							wcYASsrMmbAxYkOpRSACLnuAhDvV.Dispose();
						}
					}

					private void VeiWJoHwHEvnpkEtMDLMwjTEUTPF()
					{
						BnaFFkYTbhMeDjUOLtNFBYMWScBO = -1;
						if (wcYASsrMmbAxYkOpRSACLnuAhDvV != null)
						{
							wcYASsrMmbAxYkOpRSACLnuAhDvV.Dispose();
						}
					}

					private void SdxSjHcfcUlGLHyNUtQNLLJifvBj()
					{
						BnaFFkYTbhMeDjUOLtNFBYMWScBO = -1;
						if (wcYASsrMmbAxYkOpRSACLnuAhDvV != null)
						{
							wcYASsrMmbAxYkOpRSACLnuAhDvV.Dispose();
						}
					}

					private void JEYhiuHgXBTTyatFTlpjuKiIRuZA()
					{
						BnaFFkYTbhMeDjUOLtNFBYMWScBO = -1;
						if (wcYASsrMmbAxYkOpRSACLnuAhDvV != null)
						{
							wcYASsrMmbAxYkOpRSACLnuAhDvV.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						YlxNychZMQJokQYuChURojZbcPtO ylxNychZMQJokQYuChURojZbcPtO;
						if (BnaFFkYTbhMeDjUOLtNFBYMWScBO == -2 && fEvqumXHooCTaxeajaiWsPMDtcAG == Environment.CurrentManagedThreadId)
						{
							BnaFFkYTbhMeDjUOLtNFBYMWScBO = 0;
							ylxNychZMQJokQYuChURojZbcPtO = this;
						}
						else
						{
							ylxNychZMQJokQYuChURojZbcPtO = new YlxNychZMQJokQYuChURojZbcPtO(0);
							ylxNychZMQJokQYuChURojZbcPtO.ZcOVniEhcydAfERuUkKQjxOfUXYuB = ZcOVniEhcydAfERuUkKQjxOfUXYuB;
						}
						return ylxNychZMQJokQYuChURojZbcPtO;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class QBqbAYVIjITvgYwqMDKanhHdoMEK : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int xtWhbIElAXbsBgDmQKSMIumcqRKyA;

					private ControllerPollingInfo ZsehVkGLhNERXkbOvLoQvVxhxbkr;

					private int TbTteWDoQbYlzcqRZJfCDEDHtteg;

					private IList<CustomController> WYRrmhZhOijZcuIZvxUuYHtMoYUv;

					private int AWsLPNldGaUIvWxmPhmwMpkrPhJG;

					private IEnumerator<ControllerPollingInfo> XhToCKsASvnuacHNNlBZcaFtsYTP;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return ZsehVkGLhNERXkbOvLoQvVxhxbkr;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return ZsehVkGLhNERXkbOvLoQvVxhxbkr;
						}
					}

					[DebuggerHidden]
					public QBqbAYVIjITvgYwqMDKanhHdoMEK(int P_0)
					{
						xtWhbIElAXbsBgDmQKSMIumcqRKyA = P_0;
						TbTteWDoQbYlzcqRZJfCDEDHtteg = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = xtWhbIElAXbsBgDmQKSMIumcqRKyA;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								iIofbMNGAvDNMRBnyQgYQeFsbgveA();
							}
						}
						WYRrmhZhOijZcuIZvxUuYHtMoYUv = null;
						XhToCKsASvnuacHNNlBZcaFtsYTP = null;
						xtWhbIElAXbsBgDmQKSMIumcqRKyA = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = xtWhbIElAXbsBgDmQKSMIumcqRKyA;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								xtWhbIElAXbsBgDmQKSMIumcqRKyA = -3;
								goto IL_0086;
							}
							xtWhbIElAXbsBgDmQKSMIumcqRKyA = -1;
							WYRrmhZhOijZcuIZvxUuYHtMoYUv = FoarDfUMCtoVFquEtrllUhEjZUUn.XyyXBqESkHDJwVnEjfVUWHBlFRAaA;
							AWsLPNldGaUIvWxmPhmwMpkrPhJG = 0;
							goto IL_00b0;
							IL_0086:
							if (XhToCKsASvnuacHNNlBZcaFtsYTP.MoveNext())
							{
								ControllerPollingInfo current = XhToCKsASvnuacHNNlBZcaFtsYTP.Current;
								ZsehVkGLhNERXkbOvLoQvVxhxbkr = current;
								xtWhbIElAXbsBgDmQKSMIumcqRKyA = 1;
								return true;
							}
							iIofbMNGAvDNMRBnyQgYQeFsbgveA();
							XhToCKsASvnuacHNNlBZcaFtsYTP = null;
							AWsLPNldGaUIvWxmPhmwMpkrPhJG++;
							goto IL_00b0;
							IL_00b0:
							if (AWsLPNldGaUIvWxmPhmwMpkrPhJG < WYRrmhZhOijZcuIZvxUuYHtMoYUv.Count)
							{
								XhToCKsASvnuacHNNlBZcaFtsYTP = WYRrmhZhOijZcuIZvxUuYHtMoYUv[AWsLPNldGaUIvWxmPhmwMpkrPhJG].PollForAllAxes().GetEnumerator();
								xtWhbIElAXbsBgDmQKSMIumcqRKyA = -3;
								goto IL_0086;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void iIofbMNGAvDNMRBnyQgYQeFsbgveA()
					{
						xtWhbIElAXbsBgDmQKSMIumcqRKyA = -1;
						if (XhToCKsASvnuacHNNlBZcaFtsYTP != null)
						{
							XhToCKsASvnuacHNNlBZcaFtsYTP.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						if (xtWhbIElAXbsBgDmQKSMIumcqRKyA == -2 && TbTteWDoQbYlzcqRZJfCDEDHtteg == Environment.CurrentManagedThreadId)
						{
							xtWhbIElAXbsBgDmQKSMIumcqRKyA = 0;
							return this;
						}
						return new QBqbAYVIjITvgYwqMDKanhHdoMEK(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class cTzYrXZPJdxjqtMByijBUiNTUAJN : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int ewYdgNfJJpqzftmswGEaaEZIQuttB;

					private ControllerPollingInfo nbTopRISaZLlnMAyahyNDkkjhJYib;

					private int qqYegSHlAMwxKEfgSakKZxyKFWKNA;

					private IList<CustomController> pYMCKSgFsaaHrtpULRfRxryEfIgg;

					private int FffpTygrddUDzyFqzkgPJWiXrWvG;

					private IEnumerator<ControllerPollingInfo> MHrdZfxfNhPMyKSoqEdHSndrHgUq;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return nbTopRISaZLlnMAyahyNDkkjhJYib;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return nbTopRISaZLlnMAyahyNDkkjhJYib;
						}
					}

					[DebuggerHidden]
					public cTzYrXZPJdxjqtMByijBUiNTUAJN(int P_0)
					{
						ewYdgNfJJpqzftmswGEaaEZIQuttB = P_0;
						qqYegSHlAMwxKEfgSakKZxyKFWKNA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = ewYdgNfJJpqzftmswGEaaEZIQuttB;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								WkBApOYSVyxSJhsXbSPIYSTZQlks();
							}
						}
						pYMCKSgFsaaHrtpULRfRxryEfIgg = null;
						MHrdZfxfNhPMyKSoqEdHSndrHgUq = null;
						ewYdgNfJJpqzftmswGEaaEZIQuttB = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = ewYdgNfJJpqzftmswGEaaEZIQuttB;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								ewYdgNfJJpqzftmswGEaaEZIQuttB = -3;
								goto IL_0086;
							}
							ewYdgNfJJpqzftmswGEaaEZIQuttB = -1;
							pYMCKSgFsaaHrtpULRfRxryEfIgg = FoarDfUMCtoVFquEtrllUhEjZUUn.XyyXBqESkHDJwVnEjfVUWHBlFRAaA;
							FffpTygrddUDzyFqzkgPJWiXrWvG = 0;
							goto IL_00b0;
							IL_0086:
							if (MHrdZfxfNhPMyKSoqEdHSndrHgUq.MoveNext())
							{
								ControllerPollingInfo current = MHrdZfxfNhPMyKSoqEdHSndrHgUq.Current;
								nbTopRISaZLlnMAyahyNDkkjhJYib = current;
								ewYdgNfJJpqzftmswGEaaEZIQuttB = 1;
								return true;
							}
							WkBApOYSVyxSJhsXbSPIYSTZQlks();
							MHrdZfxfNhPMyKSoqEdHSndrHgUq = null;
							FffpTygrddUDzyFqzkgPJWiXrWvG++;
							goto IL_00b0;
							IL_00b0:
							if (FffpTygrddUDzyFqzkgPJWiXrWvG < pYMCKSgFsaaHrtpULRfRxryEfIgg.Count)
							{
								MHrdZfxfNhPMyKSoqEdHSndrHgUq = pYMCKSgFsaaHrtpULRfRxryEfIgg[FffpTygrddUDzyFqzkgPJWiXrWvG].PollForAllButtons().GetEnumerator();
								ewYdgNfJJpqzftmswGEaaEZIQuttB = -3;
								goto IL_0086;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void WkBApOYSVyxSJhsXbSPIYSTZQlks()
					{
						ewYdgNfJJpqzftmswGEaaEZIQuttB = -1;
						if (MHrdZfxfNhPMyKSoqEdHSndrHgUq != null)
						{
							MHrdZfxfNhPMyKSoqEdHSndrHgUq.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						if (ewYdgNfJJpqzftmswGEaaEZIQuttB == -2 && qqYegSHlAMwxKEfgSakKZxyKFWKNA == Environment.CurrentManagedThreadId)
						{
							ewYdgNfJJpqzftmswGEaaEZIQuttB = 0;
							return this;
						}
						return new cTzYrXZPJdxjqtMByijBUiNTUAJN(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class vMgDXEtGEVumClOmDpVxsKTrhDKA : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int RKZAnQkIaMNVbcCMWmVGahVfpIVWB;

					private ControllerPollingInfo fOmryFAsuOTxtryHxDXjPCAkKMxk;

					private int ZEOgCySoaQCbiezsXpMjAMCnkzopA;

					private IList<CustomController> PwlNEZirrdSPzyJiUAcGXMBSdECaA;

					private int coerfviIJUVDOKWPVHILCCvDRPbV;

					private IEnumerator<ControllerPollingInfo> zLUxlaGBQrBmmIehoYTuBehNHMSe;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return fOmryFAsuOTxtryHxDXjPCAkKMxk;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return fOmryFAsuOTxtryHxDXjPCAkKMxk;
						}
					}

					[DebuggerHidden]
					public vMgDXEtGEVumClOmDpVxsKTrhDKA(int P_0)
					{
						RKZAnQkIaMNVbcCMWmVGahVfpIVWB = P_0;
						ZEOgCySoaQCbiezsXpMjAMCnkzopA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rKZAnQkIaMNVbcCMWmVGahVfpIVWB = RKZAnQkIaMNVbcCMWmVGahVfpIVWB;
						if (rKZAnQkIaMNVbcCMWmVGahVfpIVWB == -3 || rKZAnQkIaMNVbcCMWmVGahVfpIVWB == 1)
						{
							try
							{
							}
							finally
							{
								fuMbCTxfumzspVyRkChnSuZuhaxW();
							}
						}
						PwlNEZirrdSPzyJiUAcGXMBSdECaA = null;
						zLUxlaGBQrBmmIehoYTuBehNHMSe = null;
						RKZAnQkIaMNVbcCMWmVGahVfpIVWB = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int rKZAnQkIaMNVbcCMWmVGahVfpIVWB = RKZAnQkIaMNVbcCMWmVGahVfpIVWB;
							if (rKZAnQkIaMNVbcCMWmVGahVfpIVWB != 0)
							{
								if (rKZAnQkIaMNVbcCMWmVGahVfpIVWB != 1)
								{
									return false;
								}
								RKZAnQkIaMNVbcCMWmVGahVfpIVWB = -3;
								goto IL_0086;
							}
							RKZAnQkIaMNVbcCMWmVGahVfpIVWB = -1;
							PwlNEZirrdSPzyJiUAcGXMBSdECaA = FoarDfUMCtoVFquEtrllUhEjZUUn.XyyXBqESkHDJwVnEjfVUWHBlFRAaA;
							coerfviIJUVDOKWPVHILCCvDRPbV = 0;
							goto IL_00b0;
							IL_0086:
							if (zLUxlaGBQrBmmIehoYTuBehNHMSe.MoveNext())
							{
								ControllerPollingInfo current = zLUxlaGBQrBmmIehoYTuBehNHMSe.Current;
								fOmryFAsuOTxtryHxDXjPCAkKMxk = current;
								RKZAnQkIaMNVbcCMWmVGahVfpIVWB = 1;
								return true;
							}
							fuMbCTxfumzspVyRkChnSuZuhaxW();
							zLUxlaGBQrBmmIehoYTuBehNHMSe = null;
							coerfviIJUVDOKWPVHILCCvDRPbV++;
							goto IL_00b0;
							IL_00b0:
							if (coerfviIJUVDOKWPVHILCCvDRPbV < PwlNEZirrdSPzyJiUAcGXMBSdECaA.Count)
							{
								zLUxlaGBQrBmmIehoYTuBehNHMSe = PwlNEZirrdSPzyJiUAcGXMBSdECaA[coerfviIJUVDOKWPVHILCCvDRPbV].PollForAllButtonsDown().GetEnumerator();
								RKZAnQkIaMNVbcCMWmVGahVfpIVWB = -3;
								goto IL_0086;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void fuMbCTxfumzspVyRkChnSuZuhaxW()
					{
						RKZAnQkIaMNVbcCMWmVGahVfpIVWB = -1;
						if (zLUxlaGBQrBmmIehoYTuBehNHMSe != null)
						{
							zLUxlaGBQrBmmIehoYTuBehNHMSe.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						if (RKZAnQkIaMNVbcCMWmVGahVfpIVWB == -2 && ZEOgCySoaQCbiezsXpMjAMCnkzopA == Environment.CurrentManagedThreadId)
						{
							RKZAnQkIaMNVbcCMWmVGahVfpIVWB = 0;
							return this;
						}
						return new vMgDXEtGEVumClOmDpVxsKTrhDKA(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class YhZnnAzpWdbprSuUxBdDturgUFzU : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int OFPFYPhwUeYlKRMWQuctDKQRLQfFA;

					private ControllerPollingInfo jAaMFwofazgvjAkPeJmpbjFqtxyj;

					private int jUtFLLdJnKkjBXGREtgCSFeHwxvY;

					private IList<CustomController> eNGUnugCtaDAhhACKzCcQhGcfObY;

					private int XiijMjpDUyAwBjuHTMCztKsXoOeF;

					private IEnumerator<ControllerPollingInfo> NWbArkdRrkijRjbfasmGbkXikIuf;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return jAaMFwofazgvjAkPeJmpbjFqtxyj;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return jAaMFwofazgvjAkPeJmpbjFqtxyj;
						}
					}

					[DebuggerHidden]
					public YhZnnAzpWdbprSuUxBdDturgUFzU(int P_0)
					{
						OFPFYPhwUeYlKRMWQuctDKQRLQfFA = P_0;
						jUtFLLdJnKkjBXGREtgCSFeHwxvY = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int oFPFYPhwUeYlKRMWQuctDKQRLQfFA = OFPFYPhwUeYlKRMWQuctDKQRLQfFA;
						if (oFPFYPhwUeYlKRMWQuctDKQRLQfFA == -3 || oFPFYPhwUeYlKRMWQuctDKQRLQfFA == 1)
						{
							try
							{
							}
							finally
							{
								NUopDNotxOPdNiDtHDITvHBcLRLQ();
							}
						}
						eNGUnugCtaDAhhACKzCcQhGcfObY = null;
						NWbArkdRrkijRjbfasmGbkXikIuf = null;
						OFPFYPhwUeYlKRMWQuctDKQRLQfFA = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int oFPFYPhwUeYlKRMWQuctDKQRLQfFA = OFPFYPhwUeYlKRMWQuctDKQRLQfFA;
							if (oFPFYPhwUeYlKRMWQuctDKQRLQfFA != 0)
							{
								if (oFPFYPhwUeYlKRMWQuctDKQRLQfFA != 1)
								{
									return false;
								}
								OFPFYPhwUeYlKRMWQuctDKQRLQfFA = -3;
								goto IL_0086;
							}
							OFPFYPhwUeYlKRMWQuctDKQRLQfFA = -1;
							eNGUnugCtaDAhhACKzCcQhGcfObY = FoarDfUMCtoVFquEtrllUhEjZUUn.XyyXBqESkHDJwVnEjfVUWHBlFRAaA;
							XiijMjpDUyAwBjuHTMCztKsXoOeF = 0;
							goto IL_00b0;
							IL_0086:
							if (NWbArkdRrkijRjbfasmGbkXikIuf.MoveNext())
							{
								ControllerPollingInfo current = NWbArkdRrkijRjbfasmGbkXikIuf.Current;
								jAaMFwofazgvjAkPeJmpbjFqtxyj = current;
								OFPFYPhwUeYlKRMWQuctDKQRLQfFA = 1;
								return true;
							}
							NUopDNotxOPdNiDtHDITvHBcLRLQ();
							NWbArkdRrkijRjbfasmGbkXikIuf = null;
							XiijMjpDUyAwBjuHTMCztKsXoOeF++;
							goto IL_00b0;
							IL_00b0:
							if (XiijMjpDUyAwBjuHTMCztKsXoOeF < eNGUnugCtaDAhhACKzCcQhGcfObY.Count)
							{
								NWbArkdRrkijRjbfasmGbkXikIuf = eNGUnugCtaDAhhACKzCcQhGcfObY[XiijMjpDUyAwBjuHTMCztKsXoOeF].PollForAllElements().GetEnumerator();
								OFPFYPhwUeYlKRMWQuctDKQRLQfFA = -3;
								goto IL_0086;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void NUopDNotxOPdNiDtHDITvHBcLRLQ()
					{
						OFPFYPhwUeYlKRMWQuctDKQRLQfFA = -1;
						if (NWbArkdRrkijRjbfasmGbkXikIuf != null)
						{
							NWbArkdRrkijRjbfasmGbkXikIuf.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						if (OFPFYPhwUeYlKRMWQuctDKQRLQfFA == -2 && jUtFLLdJnKkjBXGREtgCSFeHwxvY == Environment.CurrentManagedThreadId)
						{
							OFPFYPhwUeYlKRMWQuctDKQRLQfFA = 0;
							return this;
						}
						return new YhZnnAzpWdbprSuUxBdDturgUFzU(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class aMFfEGkaBtvVMzGFzvvQeniWqctX : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int WcCubHJGIsbrpUNasBzufTpfmiNQA;

					private ControllerPollingInfo pVVriwyBxxvEPcshVjKmfVhzcGAG;

					private int czgilkZJyFFDKdyosGcKEsjCnwPJ;

					private IList<CustomController> vmnmsmNvkfitBWAqXmAOsnDVVSBX;

					private int nIADFZCKVOplwxdrvfiHstngMLKs;

					private IEnumerator<ControllerPollingInfo> WSWvPewPItTfMlLXhfqlbblCBNuV;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return pVVriwyBxxvEPcshVjKmfVhzcGAG;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return pVVriwyBxxvEPcshVjKmfVhzcGAG;
						}
					}

					[DebuggerHidden]
					public aMFfEGkaBtvVMzGFzvvQeniWqctX(int P_0)
					{
						WcCubHJGIsbrpUNasBzufTpfmiNQA = P_0;
						czgilkZJyFFDKdyosGcKEsjCnwPJ = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int wcCubHJGIsbrpUNasBzufTpfmiNQA = WcCubHJGIsbrpUNasBzufTpfmiNQA;
						if (wcCubHJGIsbrpUNasBzufTpfmiNQA == -3 || wcCubHJGIsbrpUNasBzufTpfmiNQA == 1)
						{
							try
							{
							}
							finally
							{
								UaAOdmFFDyUJhKoDcoIlmHvuDSNr();
							}
						}
						vmnmsmNvkfitBWAqXmAOsnDVVSBX = null;
						WSWvPewPItTfMlLXhfqlbblCBNuV = null;
						WcCubHJGIsbrpUNasBzufTpfmiNQA = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int wcCubHJGIsbrpUNasBzufTpfmiNQA = WcCubHJGIsbrpUNasBzufTpfmiNQA;
							if (wcCubHJGIsbrpUNasBzufTpfmiNQA != 0)
							{
								if (wcCubHJGIsbrpUNasBzufTpfmiNQA != 1)
								{
									return false;
								}
								WcCubHJGIsbrpUNasBzufTpfmiNQA = -3;
								goto IL_0086;
							}
							WcCubHJGIsbrpUNasBzufTpfmiNQA = -1;
							vmnmsmNvkfitBWAqXmAOsnDVVSBX = FoarDfUMCtoVFquEtrllUhEjZUUn.XyyXBqESkHDJwVnEjfVUWHBlFRAaA;
							nIADFZCKVOplwxdrvfiHstngMLKs = 0;
							goto IL_00b0;
							IL_0086:
							if (WSWvPewPItTfMlLXhfqlbblCBNuV.MoveNext())
							{
								ControllerPollingInfo current = WSWvPewPItTfMlLXhfqlbblCBNuV.Current;
								pVVriwyBxxvEPcshVjKmfVhzcGAG = current;
								WcCubHJGIsbrpUNasBzufTpfmiNQA = 1;
								return true;
							}
							UaAOdmFFDyUJhKoDcoIlmHvuDSNr();
							WSWvPewPItTfMlLXhfqlbblCBNuV = null;
							nIADFZCKVOplwxdrvfiHstngMLKs++;
							goto IL_00b0;
							IL_00b0:
							if (nIADFZCKVOplwxdrvfiHstngMLKs < vmnmsmNvkfitBWAqXmAOsnDVVSBX.Count)
							{
								WSWvPewPItTfMlLXhfqlbblCBNuV = vmnmsmNvkfitBWAqXmAOsnDVVSBX[nIADFZCKVOplwxdrvfiHstngMLKs].PollForAllElementsDown().GetEnumerator();
								WcCubHJGIsbrpUNasBzufTpfmiNQA = -3;
								goto IL_0086;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void UaAOdmFFDyUJhKoDcoIlmHvuDSNr()
					{
						WcCubHJGIsbrpUNasBzufTpfmiNQA = -1;
						if (WSWvPewPItTfMlLXhfqlbblCBNuV != null)
						{
							WSWvPewPItTfMlLXhfqlbblCBNuV.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						if (WcCubHJGIsbrpUNasBzufTpfmiNQA == -2 && czgilkZJyFFDKdyosGcKEsjCnwPJ == Environment.CurrentManagedThreadId)
						{
							WcCubHJGIsbrpUNasBzufTpfmiNQA = 0;
							return this;
						}
						return new aMFfEGkaBtvVMzGFzvvQeniWqctX(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class wGXvEqCJvRCxnOOJPpTiEJWzyhdo : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int BExTVcluPpxaYzbbiijYRUYViqVI;

					private ControllerPollingInfo SjZbjoNiTXHaXvozuaVTCtBtmwfaA;

					private int OheHupVlXoYlwhPgnmQtcBYXjoI;

					private IList<Joystick> iTiFIbgxmECqnrTrNwYdQKGLRUUuA;

					private int LGmJzgqXlUweQkAtqUhfHVgJknsp;

					private IEnumerator<ControllerPollingInfo> KDhFkOcFgwTqYPdLMCzeGGDgHzpc;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return SjZbjoNiTXHaXvozuaVTCtBtmwfaA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return SjZbjoNiTXHaXvozuaVTCtBtmwfaA;
						}
					}

					[DebuggerHidden]
					public wGXvEqCJvRCxnOOJPpTiEJWzyhdo(int P_0)
					{
						BExTVcluPpxaYzbbiijYRUYViqVI = P_0;
						OheHupVlXoYlwhPgnmQtcBYXjoI = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int bExTVcluPpxaYzbbiijYRUYViqVI = BExTVcluPpxaYzbbiijYRUYViqVI;
						if (bExTVcluPpxaYzbbiijYRUYViqVI == -3 || bExTVcluPpxaYzbbiijYRUYViqVI == 1)
						{
							try
							{
							}
							finally
							{
								NEqwFTqWRSaGbLjVFlfSTfXnjHiW();
							}
						}
						iTiFIbgxmECqnrTrNwYdQKGLRUUuA = null;
						KDhFkOcFgwTqYPdLMCzeGGDgHzpc = null;
						BExTVcluPpxaYzbbiijYRUYViqVI = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int bExTVcluPpxaYzbbiijYRUYViqVI = BExTVcluPpxaYzbbiijYRUYViqVI;
							if (bExTVcluPpxaYzbbiijYRUYViqVI != 0)
							{
								if (bExTVcluPpxaYzbbiijYRUYViqVI != 1)
								{
									return false;
								}
								BExTVcluPpxaYzbbiijYRUYViqVI = -3;
								goto IL_0086;
							}
							BExTVcluPpxaYzbbiijYRUYViqVI = -1;
							iTiFIbgxmECqnrTrNwYdQKGLRUUuA = FoarDfUMCtoVFquEtrllUhEjZUUn.vSofOXqlhEhhaEStqlEuVapoCWcU;
							LGmJzgqXlUweQkAtqUhfHVgJknsp = 0;
							goto IL_00b0;
							IL_0086:
							if (KDhFkOcFgwTqYPdLMCzeGGDgHzpc.MoveNext())
							{
								ControllerPollingInfo current = KDhFkOcFgwTqYPdLMCzeGGDgHzpc.Current;
								SjZbjoNiTXHaXvozuaVTCtBtmwfaA = current;
								BExTVcluPpxaYzbbiijYRUYViqVI = 1;
								return true;
							}
							NEqwFTqWRSaGbLjVFlfSTfXnjHiW();
							KDhFkOcFgwTqYPdLMCzeGGDgHzpc = null;
							LGmJzgqXlUweQkAtqUhfHVgJknsp++;
							goto IL_00b0;
							IL_00b0:
							if (LGmJzgqXlUweQkAtqUhfHVgJknsp < iTiFIbgxmECqnrTrNwYdQKGLRUUuA.Count)
							{
								KDhFkOcFgwTqYPdLMCzeGGDgHzpc = iTiFIbgxmECqnrTrNwYdQKGLRUUuA[LGmJzgqXlUweQkAtqUhfHVgJknsp].PollForAllAxes().GetEnumerator();
								BExTVcluPpxaYzbbiijYRUYViqVI = -3;
								goto IL_0086;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void NEqwFTqWRSaGbLjVFlfSTfXnjHiW()
					{
						BExTVcluPpxaYzbbiijYRUYViqVI = -1;
						if (KDhFkOcFgwTqYPdLMCzeGGDgHzpc != null)
						{
							KDhFkOcFgwTqYPdLMCzeGGDgHzpc.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						if (BExTVcluPpxaYzbbiijYRUYViqVI == -2 && OheHupVlXoYlwhPgnmQtcBYXjoI == Environment.CurrentManagedThreadId)
						{
							BExTVcluPpxaYzbbiijYRUYViqVI = 0;
							return this;
						}
						return new wGXvEqCJvRCxnOOJPpTiEJWzyhdo(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class SRvPERmvflyYtDuLXdxAtSCZOJmI : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int nRJbPoKIUfdJHDMxcMxNTJsASqzlA;

					private ControllerPollingInfo nKnJePViRvXgLQqABFHmkhayQPAhA;

					private int syDbgycQgabnrceTHWjNgTbjNedeA;

					private IList<Joystick> CtsayBSyPIOepORiUdFzeLwIxGeK;

					private int ddxAlDiohqweHuGVhspYkjXXaKxhb;

					private IEnumerator<ControllerPollingInfo> psYJZwvBKPwvdGDehxlIMyfytAEV;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return nKnJePViRvXgLQqABFHmkhayQPAhA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return nKnJePViRvXgLQqABFHmkhayQPAhA;
						}
					}

					[DebuggerHidden]
					public SRvPERmvflyYtDuLXdxAtSCZOJmI(int P_0)
					{
						nRJbPoKIUfdJHDMxcMxNTJsASqzlA = P_0;
						syDbgycQgabnrceTHWjNgTbjNedeA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = nRJbPoKIUfdJHDMxcMxNTJsASqzlA;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								adygJQvFStvYnQrmylGfRUvlnWJm();
							}
						}
						CtsayBSyPIOepORiUdFzeLwIxGeK = null;
						psYJZwvBKPwvdGDehxlIMyfytAEV = null;
						nRJbPoKIUfdJHDMxcMxNTJsASqzlA = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = nRJbPoKIUfdJHDMxcMxNTJsASqzlA;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								nRJbPoKIUfdJHDMxcMxNTJsASqzlA = -3;
								goto IL_0086;
							}
							nRJbPoKIUfdJHDMxcMxNTJsASqzlA = -1;
							CtsayBSyPIOepORiUdFzeLwIxGeK = FoarDfUMCtoVFquEtrllUhEjZUUn.vSofOXqlhEhhaEStqlEuVapoCWcU;
							ddxAlDiohqweHuGVhspYkjXXaKxhb = 0;
							goto IL_00b0;
							IL_0086:
							if (psYJZwvBKPwvdGDehxlIMyfytAEV.MoveNext())
							{
								ControllerPollingInfo current = psYJZwvBKPwvdGDehxlIMyfytAEV.Current;
								nKnJePViRvXgLQqABFHmkhayQPAhA = current;
								nRJbPoKIUfdJHDMxcMxNTJsASqzlA = 1;
								return true;
							}
							adygJQvFStvYnQrmylGfRUvlnWJm();
							psYJZwvBKPwvdGDehxlIMyfytAEV = null;
							ddxAlDiohqweHuGVhspYkjXXaKxhb++;
							goto IL_00b0;
							IL_00b0:
							if (ddxAlDiohqweHuGVhspYkjXXaKxhb < CtsayBSyPIOepORiUdFzeLwIxGeK.Count)
							{
								psYJZwvBKPwvdGDehxlIMyfytAEV = CtsayBSyPIOepORiUdFzeLwIxGeK[ddxAlDiohqweHuGVhspYkjXXaKxhb].PollForAllButtons().GetEnumerator();
								nRJbPoKIUfdJHDMxcMxNTJsASqzlA = -3;
								goto IL_0086;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void adygJQvFStvYnQrmylGfRUvlnWJm()
					{
						nRJbPoKIUfdJHDMxcMxNTJsASqzlA = -1;
						if (psYJZwvBKPwvdGDehxlIMyfytAEV != null)
						{
							psYJZwvBKPwvdGDehxlIMyfytAEV.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						if (nRJbPoKIUfdJHDMxcMxNTJsASqzlA == -2 && syDbgycQgabnrceTHWjNgTbjNedeA == Environment.CurrentManagedThreadId)
						{
							nRJbPoKIUfdJHDMxcMxNTJsASqzlA = 0;
							return this;
						}
						return new SRvPERmvflyYtDuLXdxAtSCZOJmI(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class fhjtBpCngjNPGMShYEskSWuDqZjd : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int bfrwUerAioaHwGcVRNzpLEXtfgueA;

					private ControllerPollingInfo fGRazwlawPhXfeMyFnDdhhwuPaFs;

					private int VjbkWHJgINFOhkKfJfAjzoZtYGlWA;

					private IList<Joystick> NvFhzQuIKWGdJYBBaonGgVooQIdx;

					private int fhWneuqHPXWWpRGCZRmVNNYxbmzp;

					private IEnumerator<ControllerPollingInfo> qGKUCrNaDWQZzlwdQNfjcbeueHJP;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return fGRazwlawPhXfeMyFnDdhhwuPaFs;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return fGRazwlawPhXfeMyFnDdhhwuPaFs;
						}
					}

					[DebuggerHidden]
					public fhjtBpCngjNPGMShYEskSWuDqZjd(int P_0)
					{
						bfrwUerAioaHwGcVRNzpLEXtfgueA = P_0;
						VjbkWHJgINFOhkKfJfAjzoZtYGlWA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = bfrwUerAioaHwGcVRNzpLEXtfgueA;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								kMwfvNeTTzLdzVMrLXjDuCuhxFJT();
							}
						}
						NvFhzQuIKWGdJYBBaonGgVooQIdx = null;
						qGKUCrNaDWQZzlwdQNfjcbeueHJP = null;
						bfrwUerAioaHwGcVRNzpLEXtfgueA = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = bfrwUerAioaHwGcVRNzpLEXtfgueA;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								bfrwUerAioaHwGcVRNzpLEXtfgueA = -3;
								goto IL_0086;
							}
							bfrwUerAioaHwGcVRNzpLEXtfgueA = -1;
							NvFhzQuIKWGdJYBBaonGgVooQIdx = FoarDfUMCtoVFquEtrllUhEjZUUn.vSofOXqlhEhhaEStqlEuVapoCWcU;
							fhWneuqHPXWWpRGCZRmVNNYxbmzp = 0;
							goto IL_00b0;
							IL_0086:
							if (qGKUCrNaDWQZzlwdQNfjcbeueHJP.MoveNext())
							{
								ControllerPollingInfo current = qGKUCrNaDWQZzlwdQNfjcbeueHJP.Current;
								fGRazwlawPhXfeMyFnDdhhwuPaFs = current;
								bfrwUerAioaHwGcVRNzpLEXtfgueA = 1;
								return true;
							}
							kMwfvNeTTzLdzVMrLXjDuCuhxFJT();
							qGKUCrNaDWQZzlwdQNfjcbeueHJP = null;
							fhWneuqHPXWWpRGCZRmVNNYxbmzp++;
							goto IL_00b0;
							IL_00b0:
							if (fhWneuqHPXWWpRGCZRmVNNYxbmzp < NvFhzQuIKWGdJYBBaonGgVooQIdx.Count)
							{
								qGKUCrNaDWQZzlwdQNfjcbeueHJP = NvFhzQuIKWGdJYBBaonGgVooQIdx[fhWneuqHPXWWpRGCZRmVNNYxbmzp].PollForAllButtonsDown().GetEnumerator();
								bfrwUerAioaHwGcVRNzpLEXtfgueA = -3;
								goto IL_0086;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void kMwfvNeTTzLdzVMrLXjDuCuhxFJT()
					{
						bfrwUerAioaHwGcVRNzpLEXtfgueA = -1;
						if (qGKUCrNaDWQZzlwdQNfjcbeueHJP != null)
						{
							qGKUCrNaDWQZzlwdQNfjcbeueHJP.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						if (bfrwUerAioaHwGcVRNzpLEXtfgueA == -2 && VjbkWHJgINFOhkKfJfAjzoZtYGlWA == Environment.CurrentManagedThreadId)
						{
							bfrwUerAioaHwGcVRNzpLEXtfgueA = 0;
							return this;
						}
						return new fhjtBpCngjNPGMShYEskSWuDqZjd(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class POpgmYhnwFZbEKQtVJWmrsPsmbXPA : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int CXMEeobOXyhdylixNdUFxpaxfKOJA;

					private ControllerPollingInfo IZUifPAnkQWTYCQkGLAjKJekKqWw;

					private int wjpUEpBViqpiftbsMFIHczdVozOM;

					private IList<Joystick> eXpsghskTPajGaITTkoXDwfEtwkL;

					private int MFqaYTJYxZQkWbjDxbxAHfHEkDNU;

					private IEnumerator<ControllerPollingInfo> yecGZCWtNcYecgyjJTzsFHTkJkmK;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return IZUifPAnkQWTYCQkGLAjKJekKqWw;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return IZUifPAnkQWTYCQkGLAjKJekKqWw;
						}
					}

					[DebuggerHidden]
					public POpgmYhnwFZbEKQtVJWmrsPsmbXPA(int P_0)
					{
						CXMEeobOXyhdylixNdUFxpaxfKOJA = P_0;
						wjpUEpBViqpiftbsMFIHczdVozOM = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int cXMEeobOXyhdylixNdUFxpaxfKOJA = CXMEeobOXyhdylixNdUFxpaxfKOJA;
						if (cXMEeobOXyhdylixNdUFxpaxfKOJA == -3 || cXMEeobOXyhdylixNdUFxpaxfKOJA == 1)
						{
							try
							{
							}
							finally
							{
								wuzMQbCQvlVVGzqKZJqvLChkEFIhA();
							}
						}
						eXpsghskTPajGaITTkoXDwfEtwkL = null;
						yecGZCWtNcYecgyjJTzsFHTkJkmK = null;
						CXMEeobOXyhdylixNdUFxpaxfKOJA = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int cXMEeobOXyhdylixNdUFxpaxfKOJA = CXMEeobOXyhdylixNdUFxpaxfKOJA;
							if (cXMEeobOXyhdylixNdUFxpaxfKOJA != 0)
							{
								if (cXMEeobOXyhdylixNdUFxpaxfKOJA != 1)
								{
									return false;
								}
								CXMEeobOXyhdylixNdUFxpaxfKOJA = -3;
								goto IL_0086;
							}
							CXMEeobOXyhdylixNdUFxpaxfKOJA = -1;
							eXpsghskTPajGaITTkoXDwfEtwkL = FoarDfUMCtoVFquEtrllUhEjZUUn.vSofOXqlhEhhaEStqlEuVapoCWcU;
							MFqaYTJYxZQkWbjDxbxAHfHEkDNU = 0;
							goto IL_00b0;
							IL_0086:
							if (yecGZCWtNcYecgyjJTzsFHTkJkmK.MoveNext())
							{
								ControllerPollingInfo current = yecGZCWtNcYecgyjJTzsFHTkJkmK.Current;
								IZUifPAnkQWTYCQkGLAjKJekKqWw = current;
								CXMEeobOXyhdylixNdUFxpaxfKOJA = 1;
								return true;
							}
							wuzMQbCQvlVVGzqKZJqvLChkEFIhA();
							yecGZCWtNcYecgyjJTzsFHTkJkmK = null;
							MFqaYTJYxZQkWbjDxbxAHfHEkDNU++;
							goto IL_00b0;
							IL_00b0:
							if (MFqaYTJYxZQkWbjDxbxAHfHEkDNU < eXpsghskTPajGaITTkoXDwfEtwkL.Count)
							{
								yecGZCWtNcYecgyjJTzsFHTkJkmK = eXpsghskTPajGaITTkoXDwfEtwkL[MFqaYTJYxZQkWbjDxbxAHfHEkDNU].PollForAllElements().GetEnumerator();
								CXMEeobOXyhdylixNdUFxpaxfKOJA = -3;
								goto IL_0086;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void wuzMQbCQvlVVGzqKZJqvLChkEFIhA()
					{
						CXMEeobOXyhdylixNdUFxpaxfKOJA = -1;
						if (yecGZCWtNcYecgyjJTzsFHTkJkmK != null)
						{
							yecGZCWtNcYecgyjJTzsFHTkJkmK.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						if (CXMEeobOXyhdylixNdUFxpaxfKOJA == -2 && wjpUEpBViqpiftbsMFIHczdVozOM == Environment.CurrentManagedThreadId)
						{
							CXMEeobOXyhdylixNdUFxpaxfKOJA = 0;
							return this;
						}
						return new POpgmYhnwFZbEKQtVJWmrsPsmbXPA(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class ZVoVnITDdrbywRnJuAnHkoehMhEH : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int YrzjGtMEUNdvBXuWMvFAAQgGKZdq;

					private ControllerPollingInfo DQUTlzNnUnqAZwmfbySVMILgNqZG;

					private int UmlvBvumEVhwUhzdqfVaiMIjUbeGA;

					private IList<Joystick> TncoRWylwetykeuZjZdlnXRflAUh;

					private int ZmYWATtNchFswsYuePYXCDtsrCRy;

					private IEnumerator<ControllerPollingInfo> xcyRFqHFqIKCCWCxGoinkUXSCzgn;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return DQUTlzNnUnqAZwmfbySVMILgNqZG;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return DQUTlzNnUnqAZwmfbySVMILgNqZG;
						}
					}

					[DebuggerHidden]
					public ZVoVnITDdrbywRnJuAnHkoehMhEH(int P_0)
					{
						YrzjGtMEUNdvBXuWMvFAAQgGKZdq = P_0;
						UmlvBvumEVhwUhzdqfVaiMIjUbeGA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int yrzjGtMEUNdvBXuWMvFAAQgGKZdq = YrzjGtMEUNdvBXuWMvFAAQgGKZdq;
						if (yrzjGtMEUNdvBXuWMvFAAQgGKZdq == -3 || yrzjGtMEUNdvBXuWMvFAAQgGKZdq == 1)
						{
							try
							{
							}
							finally
							{
								xtidepIPpbLjhFXssMcBaRMDtqrdc();
							}
						}
						TncoRWylwetykeuZjZdlnXRflAUh = null;
						xcyRFqHFqIKCCWCxGoinkUXSCzgn = null;
						YrzjGtMEUNdvBXuWMvFAAQgGKZdq = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int yrzjGtMEUNdvBXuWMvFAAQgGKZdq = YrzjGtMEUNdvBXuWMvFAAQgGKZdq;
							if (yrzjGtMEUNdvBXuWMvFAAQgGKZdq != 0)
							{
								if (yrzjGtMEUNdvBXuWMvFAAQgGKZdq != 1)
								{
									return false;
								}
								YrzjGtMEUNdvBXuWMvFAAQgGKZdq = -3;
								goto IL_0086;
							}
							YrzjGtMEUNdvBXuWMvFAAQgGKZdq = -1;
							TncoRWylwetykeuZjZdlnXRflAUh = FoarDfUMCtoVFquEtrllUhEjZUUn.vSofOXqlhEhhaEStqlEuVapoCWcU;
							ZmYWATtNchFswsYuePYXCDtsrCRy = 0;
							goto IL_00b0;
							IL_0086:
							if (xcyRFqHFqIKCCWCxGoinkUXSCzgn.MoveNext())
							{
								ControllerPollingInfo current = xcyRFqHFqIKCCWCxGoinkUXSCzgn.Current;
								DQUTlzNnUnqAZwmfbySVMILgNqZG = current;
								YrzjGtMEUNdvBXuWMvFAAQgGKZdq = 1;
								return true;
							}
							xtidepIPpbLjhFXssMcBaRMDtqrdc();
							xcyRFqHFqIKCCWCxGoinkUXSCzgn = null;
							ZmYWATtNchFswsYuePYXCDtsrCRy++;
							goto IL_00b0;
							IL_00b0:
							if (ZmYWATtNchFswsYuePYXCDtsrCRy < TncoRWylwetykeuZjZdlnXRflAUh.Count)
							{
								xcyRFqHFqIKCCWCxGoinkUXSCzgn = TncoRWylwetykeuZjZdlnXRflAUh[ZmYWATtNchFswsYuePYXCDtsrCRy].PollForAllElementsDown().GetEnumerator();
								YrzjGtMEUNdvBXuWMvFAAQgGKZdq = -3;
								goto IL_0086;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void xtidepIPpbLjhFXssMcBaRMDtqrdc()
					{
						YrzjGtMEUNdvBXuWMvFAAQgGKZdq = -1;
						if (xcyRFqHFqIKCCWCxGoinkUXSCzgn != null)
						{
							xcyRFqHFqIKCCWCxGoinkUXSCzgn.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						if (YrzjGtMEUNdvBXuWMvFAAQgGKZdq == -2 && UmlvBvumEVhwUhzdqfVaiMIjUbeGA == Environment.CurrentManagedThreadId)
						{
							YrzjGtMEUNdvBXuWMvFAAQgGKZdq = 0;
							return this;
						}
						return new ZVoVnITDdrbywRnJuAnHkoehMhEH(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private static PollingHelper lfBcIHfpRkIuvEiqGPHASvosBOokb;

				internal static PollingHelper gzPDLVqOzmSazIxJQeONMUWYYaY => lfBcIHfpRkIuvEiqGPHASvosBOokb ?? (lfBcIHfpRkIuvEiqGPHASvosBOokb = new PollingHelper());

				private PollingHelper()
				{
				}

				public ControllerPollingInfo PollAllControllersForFirstElement()
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					ControllerPollingInfo result = YVHEdOUzYTHYwULDFhZyfcgJqLNe();
					if (result.success)
					{
						return result;
					}
					result = mFnVuvWFXiGzeAZoFiVHtRumMiQCb();
					if (result.success)
					{
						return result;
					}
					result = ewIFQbeLdBgBdqIzRXjycbzhuwAc();
					if (result.success)
					{
						return result;
					}
					result = pMmeADMQjDjzzZYhkKBhhLKGnTJr();
					if (result.success)
					{
						return result;
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				public ControllerPollingInfo PollAllControllersForFirstElementDown()
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					ControllerPollingInfo result = yTKJQBtAixJtgNFYUfVBPHMySvkU();
					if (result.success)
					{
						return result;
					}
					result = IfaEjXpjxHHixhQNqqVmGGViYGru();
					if (result.success)
					{
						return result;
					}
					result = aZCUOYsUzUMCMMtsvFKKsatJQOuu();
					if (result.success)
					{
						return result;
					}
					result = jjUScddhraGMqcZAgFFJkxfpvikNA();
					if (result.success)
					{
						return result;
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				public ControllerPollingInfo PollAllControllersForFirstButton()
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					ControllerPollingInfo result = LaHlcbMDmHmiXbNywbJGzQgheiRF();
					if (result.success)
					{
						return result;
					}
					result = mFnVuvWFXiGzeAZoFiVHtRumMiQCb();
					if (result.success)
					{
						return result;
					}
					result = PmWfMQAOyJjTrOrAShhheVkAeikp();
					if (result.success)
					{
						return result;
					}
					result = wMuFPScpgMDPJmLMxhdgGNvRJItLA();
					if (result.success)
					{
						return result;
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				public ControllerPollingInfo PollAllControllersForFirstButtonDown()
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					ControllerPollingInfo result = wMKRkucLCFhIUbvEOWgghuuXScQAA();
					if (result.success)
					{
						return result;
					}
					result = IfaEjXpjxHHixhQNqqVmGGViYGru();
					if (result.success)
					{
						return result;
					}
					result = keRKuPhVBCAunKCDzWaqFrTiFGyc();
					if (result.success)
					{
						return result;
					}
					result = OlTwmzLZApOaZLgsandHImpnciAO();
					if (result.success)
					{
						return result;
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				public ControllerPollingInfo PollAllControllersForFirstAxis()
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					ControllerPollingInfo result = tKGvtQOJeDsJTcmgJzWxetlUgqEM();
					if (result.success)
					{
						return result;
					}
					result = ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					if (result.success)
					{
						return result;
					}
					result = ElXgEeCDLTLEOtmUHAcYLvXpIxvrA();
					if (result.success)
					{
						return result;
					}
					result = gnoNiozoZEfWigxSGhcNPwlTkqJRA();
					if (result.success)
					{
						return result;
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				public ControllerPollingInfo PollAllControllersOfTypeForFirstElement(ControllerType controllerType)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Joystick => YVHEdOUzYTHYwULDFhZyfcgJqLNe(), 
						ControllerType.Keyboard => mFnVuvWFXiGzeAZoFiVHtRumMiQCb(), 
						ControllerType.Mouse => ewIFQbeLdBgBdqIzRXjycbzhuwAc(), 
						ControllerType.Custom => pMmeADMQjDjzzZYhkKBhhLKGnTJr(), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollAllControllersOfTypeForFirstElementDown(ControllerType controllerType)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Joystick => yTKJQBtAixJtgNFYUfVBPHMySvkU(), 
						ControllerType.Keyboard => IfaEjXpjxHHixhQNqqVmGGViYGru(), 
						ControllerType.Mouse => aZCUOYsUzUMCMMtsvFKKsatJQOuu(), 
						ControllerType.Custom => jjUScddhraGMqcZAgFFJkxfpvikNA(), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollAllControllersOfTypeForFirstButton(ControllerType controllerType)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Joystick => LaHlcbMDmHmiXbNywbJGzQgheiRF(), 
						ControllerType.Keyboard => mFnVuvWFXiGzeAZoFiVHtRumMiQCb(), 
						ControllerType.Mouse => PmWfMQAOyJjTrOrAShhheVkAeikp(), 
						ControllerType.Custom => wMuFPScpgMDPJmLMxhdgGNvRJItLA(), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollAllControllersOfTypeForFirstButtonDown(ControllerType controllerType)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Joystick => wMKRkucLCFhIUbvEOWgghuuXScQAA(), 
						ControllerType.Keyboard => IfaEjXpjxHHixhQNqqVmGGViYGru(), 
						ControllerType.Mouse => keRKuPhVBCAunKCDzWaqFrTiFGyc(), 
						ControllerType.Custom => OlTwmzLZApOaZLgsandHImpnciAO(), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollAllControllersOfTypeForFirstAxis(ControllerType controllerType)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Joystick => tKGvtQOJeDsJTcmgJzWxetlUgqEM(), 
						ControllerType.Keyboard => ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ(), 
						ControllerType.Mouse => ElXgEeCDLTLEOtmUHAcYLvXpIxvrA(), 
						ControllerType.Custom => gnoNiozoZEfWigxSGhcNPwlTkqJRA(), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollControllerForFirstElement(ControllerType controllerType, int controllerId)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Joystick => SEqmeCVzfLQLmIetXKJTcHoiXpmo(controllerId), 
						ControllerType.Keyboard => mFnVuvWFXiGzeAZoFiVHtRumMiQCb(), 
						ControllerType.Mouse => ewIFQbeLdBgBdqIzRXjycbzhuwAc(), 
						ControllerType.Custom => peopSlkFiifrewMlLSlcTBROOSGF(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollControllerForFirstElementDown(ControllerType controllerType, int controllerId)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Joystick => RxdpEGRfuwqoDGpRixTnnHmNayXcA(controllerId), 
						ControllerType.Keyboard => IfaEjXpjxHHixhQNqqVmGGViYGru(), 
						ControllerType.Mouse => aZCUOYsUzUMCMMtsvFKKsatJQOuu(), 
						ControllerType.Custom => dkEpRTcyVpkZascaIfqyiKDNiHyXA(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollControllerForFirstButton(ControllerType controllerType, int controllerId)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Joystick => mpEJhdDpLdIPGIyHqfNYmiivpEhDb(controllerId), 
						ControllerType.Keyboard => mFnVuvWFXiGzeAZoFiVHtRumMiQCb(), 
						ControllerType.Mouse => PmWfMQAOyJjTrOrAShhheVkAeikp(), 
						ControllerType.Custom => LWjfHmyvuxKBaUqhSPjBlLMyKaoA(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollControllerForFirstButtonDown(ControllerType controllerType, int controllerId)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Joystick => UpEZXMfzbtAGIdQSiAGEfEdIeiaKA(controllerId), 
						ControllerType.Keyboard => IfaEjXpjxHHixhQNqqVmGGViYGru(), 
						ControllerType.Mouse => keRKuPhVBCAunKCDzWaqFrTiFGyc(), 
						ControllerType.Custom => FuBEPWJqQiPalQKoXZOYfXEUCUEQ(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollControllerForFirstAxis(ControllerType controllerType, int controllerId)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Joystick => EAKNDQEJisAbMnAoggdFaTXazmcmA(controllerId), 
						ControllerType.Keyboard => ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ(), 
						ControllerType.Mouse => ElXgEeCDLTLEOtmUHAcYLvXpIxvrA(), 
						ControllerType.Custom => OAfaVlaMmlLQTYnsCyNgTpZAsXEB(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				[IteratorStateMachine(typeof(guqQPinnHzMHXuJEyaNXLoGjBxkG))]
				public IEnumerable<ControllerPollingInfo> PollAllControllersForAllElements()
				{
					return new guqQPinnHzMHXuJEyaNXLoGjBxkG(-2)
					{
						ssddRVitMxjvaukdMhlnyucjYAPJA = this
					};
				}

				[IteratorStateMachine(typeof(YlxNychZMQJokQYuChURojZbcPtO))]
				public IEnumerable<ControllerPollingInfo> PollAllControllersForAllElementsDown()
				{
					return new YlxNychZMQJokQYuChURojZbcPtO(-2)
					{
						ZcOVniEhcydAfERuUkKQjxOfUXYuB = this
					};
				}

				[IteratorStateMachine(typeof(ttCxvEeWWhhSDWcCreltbGFyBzsW))]
				public IEnumerable<ControllerPollingInfo> PollAllControllersForAllButtons()
				{
					return new ttCxvEeWWhhSDWcCreltbGFyBzsW(-2)
					{
						irYgbNeTzCdHweitiJlaorkwCwKEA = this
					};
				}

				[IteratorStateMachine(typeof(lOlwLAkLyieOCHXcxLSmHFQdFbzCb))]
				public IEnumerable<ControllerPollingInfo> PollAllControllersForAllButtonsDown()
				{
					return new lOlwLAkLyieOCHXcxLSmHFQdFbzCb(-2)
					{
						IPpbfIhhnMltWrFAXhObySTLUnar = this
					};
				}

				[IteratorStateMachine(typeof(BrjRuFDIHsSGwyHheVYyjjztsUXt))]
				public IEnumerable<ControllerPollingInfo> PollAllControllersForAllAxes()
				{
					return new BrjRuFDIHsSGwyHheVYyjjztsUXt(-2)
					{
						dhkUfWCoxGSBjRvnLxefcSrnCephA = this
					};
				}

				public IEnumerable<ControllerPollingInfo> PollControllerForAllElements(ControllerType controllerType, int controllerId)
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return controllerType switch
					{
						ControllerType.Joystick => CXdTWGfdNerpKPWmsqdkRtoHZcrB(controllerId), 
						ControllerType.Keyboard => AralTjnEeTrjVgILEpmyVrrHgnHU(), 
						ControllerType.Mouse => CaqPIWVPoFHbENQzcOgZrJmkoodh(), 
						ControllerType.Custom => kUOxEBgrVhfRXaeYDXWSwzhAXRTh(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public IEnumerable<ControllerPollingInfo> PollControllerForAllElementsDown(ControllerType controllerType, int controllerId)
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return controllerType switch
					{
						ControllerType.Joystick => syGxLOrtkIddEgRfylOFxRyBhwZn(controllerId), 
						ControllerType.Keyboard => pKRCAfvRKFPFTaZyOsLCeMnboKlF(), 
						ControllerType.Mouse => GwsEnXpiGninpPTBBlWqHVrxbYSn(), 
						ControllerType.Custom => BwYDpZGWwyTvQJayKzmyOlNQtMNn(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public IEnumerable<ControllerPollingInfo> PollControllerForAllButtons(ControllerType controllerType, int controllerId)
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return controllerType switch
					{
						ControllerType.Joystick => SrxvfzhcyxBsCkEwzZjObTNQYpSw(controllerId), 
						ControllerType.Keyboard => AralTjnEeTrjVgILEpmyVrrHgnHU(), 
						ControllerType.Mouse => esgGJWDkYFOFcyUmAeBDOuurVDhdA(), 
						ControllerType.Custom => UZkFqOAQBPDWwYcsMufDdMFILHoX(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public IEnumerable<ControllerPollingInfo> PollControllerForAllButtonsDown(ControllerType controllerType, int controllerId)
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return controllerType switch
					{
						ControllerType.Joystick => MRgEExIEWoEoyBxtihLRGOIzSSynB(controllerId), 
						ControllerType.Keyboard => pKRCAfvRKFPFTaZyOsLCeMnboKlF(), 
						ControllerType.Mouse => ChhFaxHlpAHDtYUNYNcrfxZbKFeOA(), 
						ControllerType.Custom => EodlNehurcFDofVcIezXgVmWjZcl(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public IEnumerable<ControllerPollingInfo> PollControllerForAllAxes(ControllerType controllerType, int controllerId)
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return controllerType switch
					{
						ControllerType.Joystick => IHyzCPGlcquVBeaWltYskgALKTPm(controllerId), 
						ControllerType.Keyboard => new List<ControllerPollingInfo>(), 
						ControllerType.Mouse => JiZdeCxZjDEqKzZUICvpLVpUVVUg(), 
						ControllerType.Custom => DucHdVqZGGqFmyFckgzURjTVmHqQ(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				private ControllerPollingInfo YVHEdOUzYTHYwULDFhZyfcgJqLNe()
				{
					IList<Joystick> list = FoarDfUMCtoVFquEtrllUhEjZUUn.vSofOXqlhEhhaEStqlEuVapoCWcU;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstElement();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo yTKJQBtAixJtgNFYUfVBPHMySvkU()
				{
					IList<Joystick> list = FoarDfUMCtoVFquEtrllUhEjZUUn.vSofOXqlhEhhaEStqlEuVapoCWcU;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstElementDown();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo LaHlcbMDmHmiXbNywbJGzQgheiRF()
				{
					IList<Joystick> list = FoarDfUMCtoVFquEtrllUhEjZUUn.vSofOXqlhEhhaEStqlEuVapoCWcU;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstButton();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo wMKRkucLCFhIUbvEOWgghuuXScQAA()
				{
					IList<Joystick> list = FoarDfUMCtoVFquEtrllUhEjZUUn.vSofOXqlhEhhaEStqlEuVapoCWcU;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstButtonDown();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo tKGvtQOJeDsJTcmgJzWxetlUgqEM()
				{
					IList<Joystick> list = FoarDfUMCtoVFquEtrllUhEjZUUn.vSofOXqlhEhhaEStqlEuVapoCWcU;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstAxis();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo SEqmeCVzfLQLmIetXKJTcHoiXpmo(int P_0)
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.GetJoystick(P_0)?.PollForFirstElement() ?? ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo RxdpEGRfuwqoDGpRixTnnHmNayXcA(int P_0)
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.GetJoystick(P_0)?.PollForFirstElementDown() ?? ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo mpEJhdDpLdIPGIyHqfNYmiivpEhDb(int P_0)
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.GetJoystick(P_0)?.PollForFirstButton() ?? ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo UpEZXMfzbtAGIdQSiAGEfEdIeiaKA(int P_0)
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.GetJoystick(P_0)?.PollForFirstButtonDown() ?? ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo EAKNDQEJisAbMnAoggdFaTXazmcmA(int P_0)
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.GetJoystick(P_0)?.PollForFirstAxis() ?? ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo mFnVuvWFXiGzeAZoFiVHtRumMiQCb()
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.Keyboard.PollForFirstKey();
				}

				private ControllerPollingInfo IfaEjXpjxHHixhQNqqVmGGViYGru()
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.Keyboard.PollForFirstKeyDown();
				}

				private ControllerPollingInfo ewIFQbeLdBgBdqIzRXjycbzhuwAc()
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.Mouse.PollForFirstElement();
				}

				private ControllerPollingInfo aZCUOYsUzUMCMMtsvFKKsatJQOuu()
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.Mouse.PollForFirstElementDown();
				}

				private ControllerPollingInfo PmWfMQAOyJjTrOrAShhheVkAeikp()
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.Mouse.PollForFirstButton();
				}

				private ControllerPollingInfo keRKuPhVBCAunKCDzWaqFrTiFGyc()
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.Mouse.PollForFirstButtonDown();
				}

				private ControllerPollingInfo ElXgEeCDLTLEOtmUHAcYLvXpIxvrA()
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.Mouse.PollForFirstAxis();
				}

				private ControllerPollingInfo pMmeADMQjDjzzZYhkKBhhLKGnTJr()
				{
					IList<CustomController> list = FoarDfUMCtoVFquEtrllUhEjZUUn.XyyXBqESkHDJwVnEjfVUWHBlFRAaA;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstElement();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo jjUScddhraGMqcZAgFFJkxfpvikNA()
				{
					IList<CustomController> list = FoarDfUMCtoVFquEtrllUhEjZUUn.XyyXBqESkHDJwVnEjfVUWHBlFRAaA;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstElementDown();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo wMuFPScpgMDPJmLMxhdgGNvRJItLA()
				{
					IList<CustomController> list = FoarDfUMCtoVFquEtrllUhEjZUUn.XyyXBqESkHDJwVnEjfVUWHBlFRAaA;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstButton();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo OlTwmzLZApOaZLgsandHImpnciAO()
				{
					IList<CustomController> list = FoarDfUMCtoVFquEtrllUhEjZUUn.XyyXBqESkHDJwVnEjfVUWHBlFRAaA;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstButtonDown();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo gnoNiozoZEfWigxSGhcNPwlTkqJRA()
				{
					IList<CustomController> list = FoarDfUMCtoVFquEtrllUhEjZUUn.XyyXBqESkHDJwVnEjfVUWHBlFRAaA;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstAxis();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo peopSlkFiifrewMlLSlcTBROOSGF(int P_0)
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.GetCustomController(P_0)?.PollForFirstElement() ?? ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo dkEpRTcyVpkZascaIfqyiKDNiHyXA(int P_0)
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.GetCustomController(P_0)?.PollForFirstElementDown() ?? ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo LWjfHmyvuxKBaUqhSPjBlLMyKaoA(int P_0)
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.GetCustomController(P_0)?.PollForFirstButton() ?? ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo FuBEPWJqQiPalQKoXZOYfXEUCUEQ(int P_0)
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.GetCustomController(P_0)?.PollForFirstButtonDown() ?? ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo OAfaVlaMmlLQTYnsCyNgTpZAsXEB(int P_0)
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.GetCustomController(P_0)?.PollForFirstAxis() ?? ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				[IteratorStateMachine(typeof(POpgmYhnwFZbEKQtVJWmrsPsmbXPA))]
				private IEnumerable<ControllerPollingInfo> wWkhtiljHyhAcETFSqRXNnspaNkeb()
				{
					return new POpgmYhnwFZbEKQtVJWmrsPsmbXPA(-2);
				}

				[IteratorStateMachine(typeof(ZVoVnITDdrbywRnJuAnHkoehMhEH))]
				private IEnumerable<ControllerPollingInfo> cJKKupGVtWEfdBakngsxJWVKxwXW()
				{
					return new ZVoVnITDdrbywRnJuAnHkoehMhEH(-2);
				}

				[IteratorStateMachine(typeof(SRvPERmvflyYtDuLXdxAtSCZOJmI))]
				private IEnumerable<ControllerPollingInfo> thRiQkrQSyxoOaMVlGZxADGsfZNd()
				{
					return new SRvPERmvflyYtDuLXdxAtSCZOJmI(-2);
				}

				[IteratorStateMachine(typeof(fhjtBpCngjNPGMShYEskSWuDqZjd))]
				private IEnumerable<ControllerPollingInfo> cazgkEfyQtgpuekViIVRtGHiioIY()
				{
					return new fhjtBpCngjNPGMShYEskSWuDqZjd(-2);
				}

				[IteratorStateMachine(typeof(wGXvEqCJvRCxnOOJPpTiEJWzyhdo))]
				private IEnumerable<ControllerPollingInfo> ZmMBBgOffpwEyCNffSYAwDliotYE()
				{
					return new wGXvEqCJvRCxnOOJPpTiEJWzyhdo(-2);
				}

				private IEnumerable<ControllerPollingInfo> CXdTWGfdNerpKPWmsqdkRtoHZcrB(int P_0)
				{
					Joystick joystick = LpVAXpiVHlQcnvltFwbImSKTLatF.GetJoystick(P_0);
					if (joystick == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return joystick.PollForAllElements();
				}

				private IEnumerable<ControllerPollingInfo> syGxLOrtkIddEgRfylOFxRyBhwZn(int P_0)
				{
					Joystick joystick = LpVAXpiVHlQcnvltFwbImSKTLatF.GetJoystick(P_0);
					if (joystick == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return joystick.PollForAllElementsDown();
				}

				private IEnumerable<ControllerPollingInfo> SrxvfzhcyxBsCkEwzZjObTNQYpSw(int P_0)
				{
					Joystick joystick = LpVAXpiVHlQcnvltFwbImSKTLatF.GetJoystick(P_0);
					if (joystick == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return joystick.PollForAllButtons();
				}

				private IEnumerable<ControllerPollingInfo> MRgEExIEWoEoyBxtihLRGOIzSSynB(int P_0)
				{
					Joystick joystick = LpVAXpiVHlQcnvltFwbImSKTLatF.GetJoystick(P_0);
					if (joystick == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return joystick.PollForAllButtonsDown();
				}

				private IEnumerable<ControllerPollingInfo> IHyzCPGlcquVBeaWltYskgALKTPm(int P_0)
				{
					Joystick joystick = LpVAXpiVHlQcnvltFwbImSKTLatF.GetJoystick(P_0);
					if (joystick == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return joystick.PollForAllAxes();
				}

				private IEnumerable<ControllerPollingInfo> AralTjnEeTrjVgILEpmyVrrHgnHU()
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.Keyboard.PollForAllKeys();
				}

				private IEnumerable<ControllerPollingInfo> pKRCAfvRKFPFTaZyOsLCeMnboKlF()
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.Keyboard.PollForAllKeysDown();
				}

				private IEnumerable<ControllerPollingInfo> CaqPIWVPoFHbENQzcOgZrJmkoodh()
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.Mouse.PollForAllElements();
				}

				private IEnumerable<ControllerPollingInfo> GwsEnXpiGninpPTBBlWqHVrxbYSn()
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.Mouse.PollForAllElementsDown();
				}

				private IEnumerable<ControllerPollingInfo> esgGJWDkYFOFcyUmAeBDOuurVDhdA()
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.Mouse.PollForAllButtons();
				}

				private IEnumerable<ControllerPollingInfo> ChhFaxHlpAHDtYUNYNcrfxZbKFeOA()
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.Mouse.PollForAllButtonsDown();
				}

				private IEnumerable<ControllerPollingInfo> JiZdeCxZjDEqKzZUICvpLVpUVVUg()
				{
					return LpVAXpiVHlQcnvltFwbImSKTLatF.Mouse.PollForAllAxes();
				}

				[IteratorStateMachine(typeof(YhZnnAzpWdbprSuUxBdDturgUFzU))]
				private IEnumerable<ControllerPollingInfo> OKUJnWbReCGIAWmstMwqNjwfDPbJ()
				{
					return new YhZnnAzpWdbprSuUxBdDturgUFzU(-2);
				}

				[IteratorStateMachine(typeof(aMFfEGkaBtvVMzGFzvvQeniWqctX))]
				private IEnumerable<ControllerPollingInfo> YtRgBWTWlRTCXylGhdlBZxlfQXWT()
				{
					return new aMFfEGkaBtvVMzGFzvvQeniWqctX(-2);
				}

				[IteratorStateMachine(typeof(cTzYrXZPJdxjqtMByijBUiNTUAJN))]
				private IEnumerable<ControllerPollingInfo> rSsEkOhrFkEUUvEeQkgOIrTpbCVT()
				{
					return new cTzYrXZPJdxjqtMByijBUiNTUAJN(-2);
				}

				[IteratorStateMachine(typeof(vMgDXEtGEVumClOmDpVxsKTrhDKA))]
				private IEnumerable<ControllerPollingInfo> zQIqJPaFGQpllVnHkCSzbRrAsKWUA()
				{
					return new vMgDXEtGEVumClOmDpVxsKTrhDKA(-2);
				}

				[IteratorStateMachine(typeof(QBqbAYVIjITvgYwqMDKanhHdoMEK))]
				private IEnumerable<ControllerPollingInfo> TUtmdzzCWVVIpbETlFbhTPOZPEPr()
				{
					return new QBqbAYVIjITvgYwqMDKanhHdoMEK(-2);
				}

				private IEnumerable<ControllerPollingInfo> kUOxEBgrVhfRXaeYDXWSwzhAXRTh(int P_0)
				{
					CustomController customController = LpVAXpiVHlQcnvltFwbImSKTLatF.GetCustomController(P_0);
					if (customController == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return customController.PollForAllElements();
				}

				private IEnumerable<ControllerPollingInfo> BwYDpZGWwyTvQJayKzmyOlNQtMNn(int P_0)
				{
					CustomController customController = LpVAXpiVHlQcnvltFwbImSKTLatF.GetCustomController(P_0);
					if (customController == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return customController.PollForAllElementsDown();
				}

				private IEnumerable<ControllerPollingInfo> UZkFqOAQBPDWwYcsMufDdMFILHoX(int P_0)
				{
					CustomController customController = LpVAXpiVHlQcnvltFwbImSKTLatF.GetCustomController(P_0);
					if (customController == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return customController.PollForAllButtons();
				}

				private IEnumerable<ControllerPollingInfo> EodlNehurcFDofVcIezXgVmWjZcl(int P_0)
				{
					CustomController customController = LpVAXpiVHlQcnvltFwbImSKTLatF.GetCustomController(P_0);
					if (customController == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return customController.PollForAllButtonsDown();
				}

				private IEnumerable<ControllerPollingInfo> DucHdVqZGGqFmyFckgzURjTVmHqQ(int P_0)
				{
					CustomController customController = LpVAXpiVHlQcnvltFwbImSKTLatF.GetCustomController(P_0);
					if (customController == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return customController.PollForAllAxes();
				}
			}

			[Browsable(false)]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public sealed class ConflictCheckingHelper : CodeHelper
			{
				private sealed class elcJFlbtidtJPAaetngjzFYnDLFJ : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
				{
					private int cYDzeOTQtqbHeOIlUgHvgRrSxRfx;

					private ElementAssignmentConflictInfo OpUDcTbGeeKomGaHeMPvzpEITxVdA;

					private int ePsNgvJsjzzwLliTnOOURdBWBgsf;

					private int kbewGurqBNBjcAFUyjUAYdZlllLfb;

					public int eUUmcjgyokhGrOcdQSNRfzgwkzos;

					private ActionElementMap teUjYfhOCqaZZaJuWeLDAlhmFixtA;

					public ActionElementMap BirhbQbrBqQjJXPQwXORqIAYwWeN;

					private bool OBOeaVDdVPDfGznrXmeVbBofvwjwb;

					public bool wSMIfwjODbwUFjbqrQOMBOwMotkF;

					private int PiGEnybdgmXNQoQiILiOcaXDxAhm;

					public int VdTUZdTsUAyWzNHaSEnbQIFRmpId;

					private CustomControllerMap MSGfeTYEVxsRukIeFSMtHjAzpqVw;

					public CustomControllerMap halNauhzzdNhniuXuqSxiDiYfQpf;

					private bool bPNKvxOerdOuSspjkQoNTkVekLIM;

					public bool wCJQYaTFkyKfqGcLbbUwIYkdHiBc;

					private bool epKcfrGnIyEVBnAkIbCaLXLNAZBdb;

					public bool FAzTXyErejPwySWbcMRoPeXDDBBT;

					private IList<Player> quClFWzgHltSTLZKDfwZLaNHogSC;

					private int ARXEOVOdevfeLhRqecetytxxEwAhA;

					private IEnumerator<ElementAssignmentConflictInfo> BwqHsxONENrTSBxBsffuuCbKvTOp;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return OpUDcTbGeeKomGaHeMPvzpEITxVdA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return OpUDcTbGeeKomGaHeMPvzpEITxVdA;
						}
					}

					[DebuggerHidden]
					public elcJFlbtidtJPAaetngjzFYnDLFJ(int P_0)
					{
						cYDzeOTQtqbHeOIlUgHvgRrSxRfx = P_0;
						ePsNgvJsjzzwLliTnOOURdBWBgsf = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = cYDzeOTQtqbHeOIlUgHvgRrSxRfx;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								haFbHmxZJqNtEuyoWaosbLqMCVbo();
							}
						}
						quClFWzgHltSTLZKDfwZLaNHogSC = null;
						BwqHsxONENrTSBxBsffuuCbKvTOp = null;
						cYDzeOTQtqbHeOIlUgHvgRrSxRfx = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = cYDzeOTQtqbHeOIlUgHvgRrSxRfx;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								cYDzeOTQtqbHeOIlUgHvgRrSxRfx = -3;
								goto IL_00e2;
							}
							cYDzeOTQtqbHeOIlUgHvgRrSxRfx = -1;
							if (kbewGurqBNBjcAFUyjUAYdZlllLfb < 0 || teUjYfhOCqaZZaJuWeLDAlhmFixtA == null)
							{
								return false;
							}
							quClFWzgHltSTLZKDfwZLaNHogSC = (OBOeaVDdVPDfGznrXmeVbBofvwjwb ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
							ARXEOVOdevfeLhRqecetytxxEwAhA = 0;
							goto IL_010c;
							IL_010c:
							if (ARXEOVOdevfeLhRqecetytxxEwAhA < quClFWzgHltSTLZKDfwZLaNHogSC.Count)
							{
								BwqHsxONENrTSBxBsffuuCbKvTOp = quClFWzgHltSTLZKDfwZLaNHogSC[ARXEOVOdevfeLhRqecetytxxEwAhA].controllers.conflictChecking.ElementAssignmentConflicts(ControllerType.Custom, PiGEnybdgmXNQoQiILiOcaXDxAhm, MSGfeTYEVxsRukIeFSMtHjAzpqVw, teUjYfhOCqaZZaJuWeLDAlhmFixtA, bPNKvxOerdOuSspjkQoNTkVekLIM, epKcfrGnIyEVBnAkIbCaLXLNAZBdb).GetEnumerator();
								cYDzeOTQtqbHeOIlUgHvgRrSxRfx = -3;
								goto IL_00e2;
							}
							return false;
							IL_00e2:
							if (BwqHsxONENrTSBxBsffuuCbKvTOp.MoveNext())
							{
								ElementAssignmentConflictInfo current = BwqHsxONENrTSBxBsffuuCbKvTOp.Current;
								OpUDcTbGeeKomGaHeMPvzpEITxVdA = current;
								cYDzeOTQtqbHeOIlUgHvgRrSxRfx = 1;
								return true;
							}
							haFbHmxZJqNtEuyoWaosbLqMCVbo();
							BwqHsxONENrTSBxBsffuuCbKvTOp = null;
							ARXEOVOdevfeLhRqecetytxxEwAhA++;
							goto IL_010c;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void haFbHmxZJqNtEuyoWaosbLqMCVbo()
					{
						cYDzeOTQtqbHeOIlUgHvgRrSxRfx = -1;
						if (BwqHsxONENrTSBxBsffuuCbKvTOp != null)
						{
							BwqHsxONENrTSBxBsffuuCbKvTOp.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
					{
						elcJFlbtidtJPAaetngjzFYnDLFJ elcJFlbtidtJPAaetngjzFYnDLFJ2;
						if (cYDzeOTQtqbHeOIlUgHvgRrSxRfx == -2 && ePsNgvJsjzzwLliTnOOURdBWBgsf == Environment.CurrentManagedThreadId)
						{
							cYDzeOTQtqbHeOIlUgHvgRrSxRfx = 0;
							elcJFlbtidtJPAaetngjzFYnDLFJ2 = this;
						}
						else
						{
							elcJFlbtidtJPAaetngjzFYnDLFJ2 = new elcJFlbtidtJPAaetngjzFYnDLFJ(0);
						}
						elcJFlbtidtJPAaetngjzFYnDLFJ2.kbewGurqBNBjcAFUyjUAYdZlllLfb = eUUmcjgyokhGrOcdQSNRfzgwkzos;
						elcJFlbtidtJPAaetngjzFYnDLFJ2.PiGEnybdgmXNQoQiILiOcaXDxAhm = VdTUZdTsUAyWzNHaSEnbQIFRmpId;
						elcJFlbtidtJPAaetngjzFYnDLFJ2.MSGfeTYEVxsRukIeFSMtHjAzpqVw = halNauhzzdNhniuXuqSxiDiYfQpf;
						elcJFlbtidtJPAaetngjzFYnDLFJ2.teUjYfhOCqaZZaJuWeLDAlhmFixtA = BirhbQbrBqQjJXPQwXORqIAYwWeN;
						elcJFlbtidtJPAaetngjzFYnDLFJ2.bPNKvxOerdOuSspjkQoNTkVekLIM = wCJQYaTFkyKfqGcLbbUwIYkdHiBc;
						elcJFlbtidtJPAaetngjzFYnDLFJ2.epKcfrGnIyEVBnAkIbCaLXLNAZBdb = FAzTXyErejPwySWbcMRoPeXDDBBT;
						elcJFlbtidtJPAaetngjzFYnDLFJ2.OBOeaVDdVPDfGznrXmeVbBofvwjwb = wSMIfwjODbwUFjbqrQOMBOwMotkF;
						return elcJFlbtidtJPAaetngjzFYnDLFJ2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class BWHtDKxaUGvTHhaJXXmhhiLuVdUA : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
				{
					private int sbmJSdTSxpHpdqhqmWSgGOoJvkvW;

					private ElementAssignmentConflictInfo qYSWdhFLiTXAeSqLsZOlOpDETiDX;

					private int FCkhUFFANSAnTEypzsmfkiePpffm;

					private ElementAssignmentConflictCheck rkyVsDLvrvoDjiXVpEaPckYOpTuk;

					public ElementAssignmentConflictCheck ZIJNcDnmfGDhbAWVHfVCVzocJiBm;

					private bool UJBTPHALCgLSuQnIgTnieKOOHmqY;

					public bool pCtTEYsCpdcdPAQRMRxqjUZTiyFNA;

					private bool wUmzryNtBjcsWYiiavqjFaNcPFmo;

					public bool uJdfzIySjgPIgiNTeHKfZvrSldcx;

					private bool uRPkiunjiTgzjqdyDVdJriNlGTZIA;

					public bool swbeAFRJAHBWIofcurfcJgNEcQhU;

					private IList<Player> TPzbPacafbBmmucSJfLmlFMALyPfb;

					private int CdrsOAzmWJbjNhdwglfoklgKYrdn;

					private IEnumerator<ElementAssignmentConflictInfo> NzqckgjnuAFOabZJTkotQvwEZjEW;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return qYSWdhFLiTXAeSqLsZOlOpDETiDX;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return qYSWdhFLiTXAeSqLsZOlOpDETiDX;
						}
					}

					[DebuggerHidden]
					public BWHtDKxaUGvTHhaJXXmhhiLuVdUA(int P_0)
					{
						sbmJSdTSxpHpdqhqmWSgGOoJvkvW = P_0;
						FCkhUFFANSAnTEypzsmfkiePpffm = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = sbmJSdTSxpHpdqhqmWSgGOoJvkvW;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								ujjFvRVKVEnxgMUvDWlOPnyTaczf();
							}
						}
						TPzbPacafbBmmucSJfLmlFMALyPfb = null;
						NzqckgjnuAFOabZJTkotQvwEZjEW = null;
						sbmJSdTSxpHpdqhqmWSgGOoJvkvW = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = sbmJSdTSxpHpdqhqmWSgGOoJvkvW;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								sbmJSdTSxpHpdqhqmWSgGOoJvkvW = -3;
								goto IL_00df;
							}
							sbmJSdTSxpHpdqhqmWSgGOoJvkvW = -1;
							if (rkyVsDLvrvoDjiXVpEaPckYOpTuk.playerId < 0 || rkyVsDLvrvoDjiXVpEaPckYOpTuk.elementAssignmentType == ElementAssignmentType.KeyboardKey)
							{
								return false;
							}
							TPzbPacafbBmmucSJfLmlFMALyPfb = (UJBTPHALCgLSuQnIgTnieKOOHmqY ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
							CdrsOAzmWJbjNhdwglfoklgKYrdn = 0;
							goto IL_0109;
							IL_0109:
							if (CdrsOAzmWJbjNhdwglfoklgKYrdn < TPzbPacafbBmmucSJfLmlFMALyPfb.Count)
							{
								NzqckgjnuAFOabZJTkotQvwEZjEW = TPzbPacafbBmmucSJfLmlFMALyPfb[CdrsOAzmWJbjNhdwglfoklgKYrdn].controllers.conflictChecking.ElementAssignmentConflicts(rkyVsDLvrvoDjiXVpEaPckYOpTuk, wUmzryNtBjcsWYiiavqjFaNcPFmo, uRPkiunjiTgzjqdyDVdJriNlGTZIA).GetEnumerator();
								sbmJSdTSxpHpdqhqmWSgGOoJvkvW = -3;
								goto IL_00df;
							}
							return false;
							IL_00df:
							if (NzqckgjnuAFOabZJTkotQvwEZjEW.MoveNext())
							{
								ElementAssignmentConflictInfo current = NzqckgjnuAFOabZJTkotQvwEZjEW.Current;
								qYSWdhFLiTXAeSqLsZOlOpDETiDX = current;
								sbmJSdTSxpHpdqhqmWSgGOoJvkvW = 1;
								return true;
							}
							ujjFvRVKVEnxgMUvDWlOPnyTaczf();
							NzqckgjnuAFOabZJTkotQvwEZjEW = null;
							CdrsOAzmWJbjNhdwglfoklgKYrdn++;
							goto IL_0109;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void ujjFvRVKVEnxgMUvDWlOPnyTaczf()
					{
						sbmJSdTSxpHpdqhqmWSgGOoJvkvW = -1;
						if (NzqckgjnuAFOabZJTkotQvwEZjEW != null)
						{
							NzqckgjnuAFOabZJTkotQvwEZjEW.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
					{
						BWHtDKxaUGvTHhaJXXmhhiLuVdUA bWHtDKxaUGvTHhaJXXmhhiLuVdUA;
						if (sbmJSdTSxpHpdqhqmWSgGOoJvkvW == -2 && FCkhUFFANSAnTEypzsmfkiePpffm == Environment.CurrentManagedThreadId)
						{
							sbmJSdTSxpHpdqhqmWSgGOoJvkvW = 0;
							bWHtDKxaUGvTHhaJXXmhhiLuVdUA = this;
						}
						else
						{
							bWHtDKxaUGvTHhaJXXmhhiLuVdUA = new BWHtDKxaUGvTHhaJXXmhhiLuVdUA(0);
						}
						bWHtDKxaUGvTHhaJXXmhhiLuVdUA.rkyVsDLvrvoDjiXVpEaPckYOpTuk = ZIJNcDnmfGDhbAWVHfVCVzocJiBm;
						bWHtDKxaUGvTHhaJXXmhhiLuVdUA.wUmzryNtBjcsWYiiavqjFaNcPFmo = uJdfzIySjgPIgiNTeHKfZvrSldcx;
						bWHtDKxaUGvTHhaJXXmhhiLuVdUA.uRPkiunjiTgzjqdyDVdJriNlGTZIA = swbeAFRJAHBWIofcurfcJgNEcQhU;
						bWHtDKxaUGvTHhaJXXmhhiLuVdUA.UJBTPHALCgLSuQnIgTnieKOOHmqY = pCtTEYsCpdcdPAQRMRxqjUZTiyFNA;
						return bWHtDKxaUGvTHhaJXXmhhiLuVdUA;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class blmVUJkixtaYSECXGEFkRbtzuZMPA : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
				{
					private int KvOMCXoxPQVpZpmnNDToAdvyljYIb;

					private ElementAssignmentConflictInfo iizapkWswmbrvuZBMFNkOfSuEImM;

					private int tJWPltmqQOoYeSYQmxLjYUBmjkzF;

					private int ckwGnFjfVarLnrCQJtOFeivqOqfq;

					public int AQDPNfPwBVLRllayyQHOYEdkBFXL;

					private ActionElementMap hCQBhcSmmuJbcSKDdcsrdYHstOAW;

					public ActionElementMap VJDtxGbWcZUFlQowPtRNqDbPhqPfA;

					private bool aEgYdAFDMGHKtKpDULeMutgFPNRz;

					public bool EgWFgaCHOSZEPrJNDllubxYAcgci;

					private int ChxUvKthaOasXCvFjHHaMNZfZKk;

					public int YNJcVoGashPlKylcHkKdtzCOOJXm;

					private JoystickMap gtDIjvvGPfpVtQCKnurKyHkfeEcbA;

					public JoystickMap ZBuBqcoNAnwcjEBpugSEhvAvgzHc;

					private bool VSTRaXZPpuGMXxTUjBnGaKoLnCazA;

					public bool YGQBQDEdmfenDGccpHupAOVmCwUX;

					private bool UVQjBwmgLmcdjQoGBFhllIHBBpiX;

					public bool TjZCyIEfFyJBDKDIRfcCwHeYJTeA;

					private IList<Player> XfmcKrxeqKTCWjFXVsyECyyfbkFl;

					private int LsQnXDgtHcBRApnvqPakkbljjMWR;

					private IEnumerator<ElementAssignmentConflictInfo> IUDhDMBKjaDypNyXCgGZqsNZyYgjA;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return iizapkWswmbrvuZBMFNkOfSuEImM;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return iizapkWswmbrvuZBMFNkOfSuEImM;
						}
					}

					[DebuggerHidden]
					public blmVUJkixtaYSECXGEFkRbtzuZMPA(int P_0)
					{
						KvOMCXoxPQVpZpmnNDToAdvyljYIb = P_0;
						tJWPltmqQOoYeSYQmxLjYUBmjkzF = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int kvOMCXoxPQVpZpmnNDToAdvyljYIb = KvOMCXoxPQVpZpmnNDToAdvyljYIb;
						if (kvOMCXoxPQVpZpmnNDToAdvyljYIb == -3 || kvOMCXoxPQVpZpmnNDToAdvyljYIb == 1)
						{
							try
							{
							}
							finally
							{
								NBMIDbenOuIRluqOEgPNqkzIkZqZ();
							}
						}
						XfmcKrxeqKTCWjFXVsyECyyfbkFl = null;
						IUDhDMBKjaDypNyXCgGZqsNZyYgjA = null;
						KvOMCXoxPQVpZpmnNDToAdvyljYIb = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int kvOMCXoxPQVpZpmnNDToAdvyljYIb = KvOMCXoxPQVpZpmnNDToAdvyljYIb;
							if (kvOMCXoxPQVpZpmnNDToAdvyljYIb != 0)
							{
								if (kvOMCXoxPQVpZpmnNDToAdvyljYIb != 1)
								{
									return false;
								}
								KvOMCXoxPQVpZpmnNDToAdvyljYIb = -3;
								goto IL_00e1;
							}
							KvOMCXoxPQVpZpmnNDToAdvyljYIb = -1;
							if (ckwGnFjfVarLnrCQJtOFeivqOqfq < 0 || hCQBhcSmmuJbcSKDdcsrdYHstOAW == null)
							{
								return false;
							}
							XfmcKrxeqKTCWjFXVsyECyyfbkFl = (aEgYdAFDMGHKtKpDULeMutgFPNRz ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
							LsQnXDgtHcBRApnvqPakkbljjMWR = 0;
							goto IL_010b;
							IL_010b:
							if (LsQnXDgtHcBRApnvqPakkbljjMWR < XfmcKrxeqKTCWjFXVsyECyyfbkFl.Count)
							{
								IUDhDMBKjaDypNyXCgGZqsNZyYgjA = XfmcKrxeqKTCWjFXVsyECyyfbkFl[LsQnXDgtHcBRApnvqPakkbljjMWR].controllers.conflictChecking.ElementAssignmentConflicts(ControllerType.Joystick, ChxUvKthaOasXCvFjHHaMNZfZKk, gtDIjvvGPfpVtQCKnurKyHkfeEcbA, hCQBhcSmmuJbcSKDdcsrdYHstOAW, VSTRaXZPpuGMXxTUjBnGaKoLnCazA, UVQjBwmgLmcdjQoGBFhllIHBBpiX).GetEnumerator();
								KvOMCXoxPQVpZpmnNDToAdvyljYIb = -3;
								goto IL_00e1;
							}
							return false;
							IL_00e1:
							if (IUDhDMBKjaDypNyXCgGZqsNZyYgjA.MoveNext())
							{
								ElementAssignmentConflictInfo current = IUDhDMBKjaDypNyXCgGZqsNZyYgjA.Current;
								iizapkWswmbrvuZBMFNkOfSuEImM = current;
								KvOMCXoxPQVpZpmnNDToAdvyljYIb = 1;
								return true;
							}
							NBMIDbenOuIRluqOEgPNqkzIkZqZ();
							IUDhDMBKjaDypNyXCgGZqsNZyYgjA = null;
							LsQnXDgtHcBRApnvqPakkbljjMWR++;
							goto IL_010b;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void NBMIDbenOuIRluqOEgPNqkzIkZqZ()
					{
						KvOMCXoxPQVpZpmnNDToAdvyljYIb = -1;
						if (IUDhDMBKjaDypNyXCgGZqsNZyYgjA != null)
						{
							IUDhDMBKjaDypNyXCgGZqsNZyYgjA.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
					{
						blmVUJkixtaYSECXGEFkRbtzuZMPA blmVUJkixtaYSECXGEFkRbtzuZMPA2;
						if (KvOMCXoxPQVpZpmnNDToAdvyljYIb == -2 && tJWPltmqQOoYeSYQmxLjYUBmjkzF == Environment.CurrentManagedThreadId)
						{
							KvOMCXoxPQVpZpmnNDToAdvyljYIb = 0;
							blmVUJkixtaYSECXGEFkRbtzuZMPA2 = this;
						}
						else
						{
							blmVUJkixtaYSECXGEFkRbtzuZMPA2 = new blmVUJkixtaYSECXGEFkRbtzuZMPA(0);
						}
						blmVUJkixtaYSECXGEFkRbtzuZMPA2.ckwGnFjfVarLnrCQJtOFeivqOqfq = AQDPNfPwBVLRllayyQHOYEdkBFXL;
						blmVUJkixtaYSECXGEFkRbtzuZMPA2.ChxUvKthaOasXCvFjHHaMNZfZKk = YNJcVoGashPlKylcHkKdtzCOOJXm;
						blmVUJkixtaYSECXGEFkRbtzuZMPA2.gtDIjvvGPfpVtQCKnurKyHkfeEcbA = ZBuBqcoNAnwcjEBpugSEhvAvgzHc;
						blmVUJkixtaYSECXGEFkRbtzuZMPA2.hCQBhcSmmuJbcSKDdcsrdYHstOAW = VJDtxGbWcZUFlQowPtRNqDbPhqPfA;
						blmVUJkixtaYSECXGEFkRbtzuZMPA2.VSTRaXZPpuGMXxTUjBnGaKoLnCazA = YGQBQDEdmfenDGccpHupAOVmCwUX;
						blmVUJkixtaYSECXGEFkRbtzuZMPA2.UVQjBwmgLmcdjQoGBFhllIHBBpiX = TjZCyIEfFyJBDKDIRfcCwHeYJTeA;
						blmVUJkixtaYSECXGEFkRbtzuZMPA2.aEgYdAFDMGHKtKpDULeMutgFPNRz = EgWFgaCHOSZEPrJNDllubxYAcgci;
						return blmVUJkixtaYSECXGEFkRbtzuZMPA2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class AnAzGkzadSctmQwLhspVzLbReEQeA : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
				{
					private int AbSdGOjYmAGMFZBptzVUTKogCshC;

					private ElementAssignmentConflictInfo MwORCfVKVbmDlmzdbLYqZYQTPraP;

					private int ClhKgBfkffKqodZaDRcuuUqtbFmx;

					private ElementAssignmentConflictCheck aVulrOHHUPxUyFmcbJPqFMVsaPbo;

					public ElementAssignmentConflictCheck EhMwLSzqEMzGsZsCojjaOckrtuTu;

					private bool GibigRcCIDrhgreMYiLbwHefRBFu;

					public bool NplLbgizINhbiorsMJxGKEenVAb;

					private bool HnlJViHzivvzCrdPhxqULVpmhWyI;

					public bool QAIqVNKNlGFjnMgnSitpdRiWLAbzA;

					private bool PNyAzRGZuUurwAbzxFWCVNrvNroKA;

					public bool zWaksVIUKZQKZxNSfENiEcodnQVcb;

					private IList<Player> yqWYPzwVntIjANUeUklSXKkWwOBw;

					private int lBnJrSHiqaSgELkNtuSrgrQucGSR;

					private IEnumerator<ElementAssignmentConflictInfo> gfPFmqIDBkNuqQVSUJkPalPTKpxe;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return MwORCfVKVbmDlmzdbLYqZYQTPraP;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return MwORCfVKVbmDlmzdbLYqZYQTPraP;
						}
					}

					[DebuggerHidden]
					public AnAzGkzadSctmQwLhspVzLbReEQeA(int P_0)
					{
						AbSdGOjYmAGMFZBptzVUTKogCshC = P_0;
						ClhKgBfkffKqodZaDRcuuUqtbFmx = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int abSdGOjYmAGMFZBptzVUTKogCshC = AbSdGOjYmAGMFZBptzVUTKogCshC;
						if (abSdGOjYmAGMFZBptzVUTKogCshC == -3 || abSdGOjYmAGMFZBptzVUTKogCshC == 1)
						{
							try
							{
							}
							finally
							{
								THJKnIiIsQozUicmSLSvBsIbsPaH();
							}
						}
						yqWYPzwVntIjANUeUklSXKkWwOBw = null;
						gfPFmqIDBkNuqQVSUJkPalPTKpxe = null;
						AbSdGOjYmAGMFZBptzVUTKogCshC = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int abSdGOjYmAGMFZBptzVUTKogCshC = AbSdGOjYmAGMFZBptzVUTKogCshC;
							if (abSdGOjYmAGMFZBptzVUTKogCshC != 0)
							{
								if (abSdGOjYmAGMFZBptzVUTKogCshC != 1)
								{
									return false;
								}
								AbSdGOjYmAGMFZBptzVUTKogCshC = -3;
								goto IL_00df;
							}
							AbSdGOjYmAGMFZBptzVUTKogCshC = -1;
							if (aVulrOHHUPxUyFmcbJPqFMVsaPbo.playerId < 0 || aVulrOHHUPxUyFmcbJPqFMVsaPbo.elementAssignmentType == ElementAssignmentType.KeyboardKey)
							{
								return false;
							}
							yqWYPzwVntIjANUeUklSXKkWwOBw = (GibigRcCIDrhgreMYiLbwHefRBFu ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
							lBnJrSHiqaSgELkNtuSrgrQucGSR = 0;
							goto IL_0109;
							IL_0109:
							if (lBnJrSHiqaSgELkNtuSrgrQucGSR < yqWYPzwVntIjANUeUklSXKkWwOBw.Count)
							{
								gfPFmqIDBkNuqQVSUJkPalPTKpxe = yqWYPzwVntIjANUeUklSXKkWwOBw[lBnJrSHiqaSgELkNtuSrgrQucGSR].controllers.conflictChecking.ElementAssignmentConflicts(aVulrOHHUPxUyFmcbJPqFMVsaPbo, HnlJViHzivvzCrdPhxqULVpmhWyI, PNyAzRGZuUurwAbzxFWCVNrvNroKA).GetEnumerator();
								AbSdGOjYmAGMFZBptzVUTKogCshC = -3;
								goto IL_00df;
							}
							return false;
							IL_00df:
							if (gfPFmqIDBkNuqQVSUJkPalPTKpxe.MoveNext())
							{
								ElementAssignmentConflictInfo current = gfPFmqIDBkNuqQVSUJkPalPTKpxe.Current;
								MwORCfVKVbmDlmzdbLYqZYQTPraP = current;
								AbSdGOjYmAGMFZBptzVUTKogCshC = 1;
								return true;
							}
							THJKnIiIsQozUicmSLSvBsIbsPaH();
							gfPFmqIDBkNuqQVSUJkPalPTKpxe = null;
							lBnJrSHiqaSgELkNtuSrgrQucGSR++;
							goto IL_0109;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void THJKnIiIsQozUicmSLSvBsIbsPaH()
					{
						AbSdGOjYmAGMFZBptzVUTKogCshC = -1;
						if (gfPFmqIDBkNuqQVSUJkPalPTKpxe != null)
						{
							gfPFmqIDBkNuqQVSUJkPalPTKpxe.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
					{
						AnAzGkzadSctmQwLhspVzLbReEQeA anAzGkzadSctmQwLhspVzLbReEQeA;
						if (AbSdGOjYmAGMFZBptzVUTKogCshC == -2 && ClhKgBfkffKqodZaDRcuuUqtbFmx == Environment.CurrentManagedThreadId)
						{
							AbSdGOjYmAGMFZBptzVUTKogCshC = 0;
							anAzGkzadSctmQwLhspVzLbReEQeA = this;
						}
						else
						{
							anAzGkzadSctmQwLhspVzLbReEQeA = new AnAzGkzadSctmQwLhspVzLbReEQeA(0);
						}
						anAzGkzadSctmQwLhspVzLbReEQeA.aVulrOHHUPxUyFmcbJPqFMVsaPbo = EhMwLSzqEMzGsZsCojjaOckrtuTu;
						anAzGkzadSctmQwLhspVzLbReEQeA.HnlJViHzivvzCrdPhxqULVpmhWyI = QAIqVNKNlGFjnMgnSitpdRiWLAbzA;
						anAzGkzadSctmQwLhspVzLbReEQeA.PNyAzRGZuUurwAbzxFWCVNrvNroKA = zWaksVIUKZQKZxNSfENiEcodnQVcb;
						anAzGkzadSctmQwLhspVzLbReEQeA.GibigRcCIDrhgreMYiLbwHefRBFu = NplLbgizINhbiorsMJxGKEenVAb;
						return anAzGkzadSctmQwLhspVzLbReEQeA;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class XwYcIBYAJUXSfkaacNnSYtphnNNG : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
				{
					private int ucEZMvUZvBDlztPyhfGyIvlOrgaCA;

					private ElementAssignmentConflictInfo hUpmYiGmroIobYtyrDIREfSTOTlS;

					private int VbUIctdEJLAYDevGpUiNmYFKTsccA;

					private int qyzzFPgCCGBjrIlBgNTVvDEraoxK;

					public int KQRtIqsBlkvNcdWtUrpQHaTPpchy;

					private ActionElementMap coNFGseismvwbbPNOxFazTZQXMSK;

					public ActionElementMap dUpgTLmEeMfhgAyAoLkYSgQGWWJL;

					private bool FNqmritJwHpOrGOxxdAIwypqVGYR;

					public bool lHJUkjEEvVMtOhXURqmKUCqEQHBw;

					private KeyboardMap phhLaaVIKVLZiXkqwngyvBUmKsN;

					public KeyboardMap ZVGxvwsuFTejXikpbJOYAKKSUmGEA;

					private bool IGlWUWbcPaWpELrlAoqSmdKMeDco;

					public bool UvHiyrxSxVnMsGOFIeYvIqyuDLlr;

					private bool prCwMIoUphNKvkYKwmYNtQhkBDAH;

					public bool CSclqCaWYVXmfJIRNiflPlqJgWly;

					private IList<Player> MlrHppKYjtIoGbeFwpksoGUuZrpb;

					private int uYlyaRjDHstnRvVdAxnaONsbwVKX;

					private IEnumerator<ElementAssignmentConflictInfo> LDoriFuoOwsBjLFIrArqAbbbUChw;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return hUpmYiGmroIobYtyrDIREfSTOTlS;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return hUpmYiGmroIobYtyrDIREfSTOTlS;
						}
					}

					[DebuggerHidden]
					public XwYcIBYAJUXSfkaacNnSYtphnNNG(int P_0)
					{
						ucEZMvUZvBDlztPyhfGyIvlOrgaCA = P_0;
						VbUIctdEJLAYDevGpUiNmYFKTsccA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = ucEZMvUZvBDlztPyhfGyIvlOrgaCA;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								pxkfwHzwatzQWogtDpgVkcwHAokGA();
							}
						}
						MlrHppKYjtIoGbeFwpksoGUuZrpb = null;
						LDoriFuoOwsBjLFIrArqAbbbUChw = null;
						ucEZMvUZvBDlztPyhfGyIvlOrgaCA = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = ucEZMvUZvBDlztPyhfGyIvlOrgaCA;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								ucEZMvUZvBDlztPyhfGyIvlOrgaCA = -3;
								goto IL_00dc;
							}
							ucEZMvUZvBDlztPyhfGyIvlOrgaCA = -1;
							if (qyzzFPgCCGBjrIlBgNTVvDEraoxK < 0 || coNFGseismvwbbPNOxFazTZQXMSK == null)
							{
								return false;
							}
							MlrHppKYjtIoGbeFwpksoGUuZrpb = (FNqmritJwHpOrGOxxdAIwypqVGYR ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
							uYlyaRjDHstnRvVdAxnaONsbwVKX = 0;
							goto IL_0106;
							IL_0106:
							if (uYlyaRjDHstnRvVdAxnaONsbwVKX < MlrHppKYjtIoGbeFwpksoGUuZrpb.Count)
							{
								LDoriFuoOwsBjLFIrArqAbbbUChw = MlrHppKYjtIoGbeFwpksoGUuZrpb[uYlyaRjDHstnRvVdAxnaONsbwVKX].controllers.conflictChecking.ElementAssignmentConflicts(ControllerType.Keyboard, 0, phhLaaVIKVLZiXkqwngyvBUmKsN, coNFGseismvwbbPNOxFazTZQXMSK, IGlWUWbcPaWpELrlAoqSmdKMeDco, prCwMIoUphNKvkYKwmYNtQhkBDAH).GetEnumerator();
								ucEZMvUZvBDlztPyhfGyIvlOrgaCA = -3;
								goto IL_00dc;
							}
							return false;
							IL_00dc:
							if (LDoriFuoOwsBjLFIrArqAbbbUChw.MoveNext())
							{
								ElementAssignmentConflictInfo current = LDoriFuoOwsBjLFIrArqAbbbUChw.Current;
								hUpmYiGmroIobYtyrDIREfSTOTlS = current;
								ucEZMvUZvBDlztPyhfGyIvlOrgaCA = 1;
								return true;
							}
							pxkfwHzwatzQWogtDpgVkcwHAokGA();
							LDoriFuoOwsBjLFIrArqAbbbUChw = null;
							uYlyaRjDHstnRvVdAxnaONsbwVKX++;
							goto IL_0106;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void pxkfwHzwatzQWogtDpgVkcwHAokGA()
					{
						ucEZMvUZvBDlztPyhfGyIvlOrgaCA = -1;
						if (LDoriFuoOwsBjLFIrArqAbbbUChw != null)
						{
							LDoriFuoOwsBjLFIrArqAbbbUChw.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
					{
						XwYcIBYAJUXSfkaacNnSYtphnNNG xwYcIBYAJUXSfkaacNnSYtphnNNG;
						if (ucEZMvUZvBDlztPyhfGyIvlOrgaCA == -2 && VbUIctdEJLAYDevGpUiNmYFKTsccA == Environment.CurrentManagedThreadId)
						{
							ucEZMvUZvBDlztPyhfGyIvlOrgaCA = 0;
							xwYcIBYAJUXSfkaacNnSYtphnNNG = this;
						}
						else
						{
							xwYcIBYAJUXSfkaacNnSYtphnNNG = new XwYcIBYAJUXSfkaacNnSYtphnNNG(0);
						}
						xwYcIBYAJUXSfkaacNnSYtphnNNG.qyzzFPgCCGBjrIlBgNTVvDEraoxK = KQRtIqsBlkvNcdWtUrpQHaTPpchy;
						xwYcIBYAJUXSfkaacNnSYtphnNNG.phhLaaVIKVLZiXkqwngyvBUmKsN = ZVGxvwsuFTejXikpbJOYAKKSUmGEA;
						xwYcIBYAJUXSfkaacNnSYtphnNNG.coNFGseismvwbbPNOxFazTZQXMSK = dUpgTLmEeMfhgAyAoLkYSgQGWWJL;
						xwYcIBYAJUXSfkaacNnSYtphnNNG.IGlWUWbcPaWpELrlAoqSmdKMeDco = UvHiyrxSxVnMsGOFIeYvIqyuDLlr;
						xwYcIBYAJUXSfkaacNnSYtphnNNG.prCwMIoUphNKvkYKwmYNtQhkBDAH = CSclqCaWYVXmfJIRNiflPlqJgWly;
						xwYcIBYAJUXSfkaacNnSYtphnNNG.FNqmritJwHpOrGOxxdAIwypqVGYR = lHJUkjEEvVMtOhXURqmKUCqEQHBw;
						return xwYcIBYAJUXSfkaacNnSYtphnNNG;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class crhllplIzjIXNDYZaWKlrZTyKbWg : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
				{
					private int eerKylCDcJHyVVoDgvjVbFpdpksE;

					private ElementAssignmentConflictInfo onPEhsaxtqfmmDXNZieNcrVosaURA;

					private int IfgseiyOfdYRrpMplpKcVhMRWQsH;

					private ElementAssignmentConflictCheck LzjMLPccUTBrvxWjlVSygznEtQAY;

					public ElementAssignmentConflictCheck hOsiBBbBmAeTyCfwymfKihihNxsQB;

					private bool mDjipXDBOZIoIGbDIgJecALALkJOb;

					public bool lepBzpGctuovMroKInIAhbRovCmx;

					private bool tSdvxwHNmQBnFmqZxHRdHSavcprCA;

					public bool poXGrPFrqhWCpWGivLgkjiMQKPpRA;

					private bool wXMBbVgmBhUGoaRxlECBavKXRegfb;

					public bool POCaQGCVzzTgtZJchpTtAXyKShwx;

					private IList<Player> TcwfNAKyfQcPEnstqbsQSGDzBjKc;

					private int uIygtDlbyCrjQDgVVdxjsGEXeHscA;

					private IEnumerator<ElementAssignmentConflictInfo> AWKQfCMTxMDbsRRCBZccNpUsgxDd;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return onPEhsaxtqfmmDXNZieNcrVosaURA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return onPEhsaxtqfmmDXNZieNcrVosaURA;
						}
					}

					[DebuggerHidden]
					public crhllplIzjIXNDYZaWKlrZTyKbWg(int P_0)
					{
						eerKylCDcJHyVVoDgvjVbFpdpksE = P_0;
						IfgseiyOfdYRrpMplpKcVhMRWQsH = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = eerKylCDcJHyVVoDgvjVbFpdpksE;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								rqrJFZWqqyAsBmvzHBvjDlFRUnYJ();
							}
						}
						TcwfNAKyfQcPEnstqbsQSGDzBjKc = null;
						AWKQfCMTxMDbsRRCBZccNpUsgxDd = null;
						eerKylCDcJHyVVoDgvjVbFpdpksE = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = eerKylCDcJHyVVoDgvjVbFpdpksE;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								eerKylCDcJHyVVoDgvjVbFpdpksE = -3;
								goto IL_00df;
							}
							eerKylCDcJHyVVoDgvjVbFpdpksE = -1;
							if (LzjMLPccUTBrvxWjlVSygznEtQAY.playerId < 0 || LzjMLPccUTBrvxWjlVSygznEtQAY.elementAssignmentType != ElementAssignmentType.KeyboardKey)
							{
								return false;
							}
							TcwfNAKyfQcPEnstqbsQSGDzBjKc = (mDjipXDBOZIoIGbDIgJecALALkJOb ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
							uIygtDlbyCrjQDgVVdxjsGEXeHscA = 0;
							goto IL_0109;
							IL_0109:
							if (uIygtDlbyCrjQDgVVdxjsGEXeHscA < TcwfNAKyfQcPEnstqbsQSGDzBjKc.Count)
							{
								AWKQfCMTxMDbsRRCBZccNpUsgxDd = TcwfNAKyfQcPEnstqbsQSGDzBjKc[uIygtDlbyCrjQDgVVdxjsGEXeHscA].controllers.conflictChecking.ElementAssignmentConflicts(LzjMLPccUTBrvxWjlVSygznEtQAY, tSdvxwHNmQBnFmqZxHRdHSavcprCA, wXMBbVgmBhUGoaRxlECBavKXRegfb).GetEnumerator();
								eerKylCDcJHyVVoDgvjVbFpdpksE = -3;
								goto IL_00df;
							}
							return false;
							IL_00df:
							if (AWKQfCMTxMDbsRRCBZccNpUsgxDd.MoveNext())
							{
								ElementAssignmentConflictInfo current = AWKQfCMTxMDbsRRCBZccNpUsgxDd.Current;
								onPEhsaxtqfmmDXNZieNcrVosaURA = current;
								eerKylCDcJHyVVoDgvjVbFpdpksE = 1;
								return true;
							}
							rqrJFZWqqyAsBmvzHBvjDlFRUnYJ();
							AWKQfCMTxMDbsRRCBZccNpUsgxDd = null;
							uIygtDlbyCrjQDgVVdxjsGEXeHscA++;
							goto IL_0109;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void rqrJFZWqqyAsBmvzHBvjDlFRUnYJ()
					{
						eerKylCDcJHyVVoDgvjVbFpdpksE = -1;
						if (AWKQfCMTxMDbsRRCBZccNpUsgxDd != null)
						{
							AWKQfCMTxMDbsRRCBZccNpUsgxDd.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
					{
						crhllplIzjIXNDYZaWKlrZTyKbWg crhllplIzjIXNDYZaWKlrZTyKbWg2;
						if (eerKylCDcJHyVVoDgvjVbFpdpksE == -2 && IfgseiyOfdYRrpMplpKcVhMRWQsH == Environment.CurrentManagedThreadId)
						{
							eerKylCDcJHyVVoDgvjVbFpdpksE = 0;
							crhllplIzjIXNDYZaWKlrZTyKbWg2 = this;
						}
						else
						{
							crhllplIzjIXNDYZaWKlrZTyKbWg2 = new crhllplIzjIXNDYZaWKlrZTyKbWg(0);
						}
						crhllplIzjIXNDYZaWKlrZTyKbWg2.LzjMLPccUTBrvxWjlVSygznEtQAY = hOsiBBbBmAeTyCfwymfKihihNxsQB;
						crhllplIzjIXNDYZaWKlrZTyKbWg2.tSdvxwHNmQBnFmqZxHRdHSavcprCA = poXGrPFrqhWCpWGivLgkjiMQKPpRA;
						crhllplIzjIXNDYZaWKlrZTyKbWg2.wXMBbVgmBhUGoaRxlECBavKXRegfb = POCaQGCVzzTgtZJchpTtAXyKShwx;
						crhllplIzjIXNDYZaWKlrZTyKbWg2.mDjipXDBOZIoIGbDIgJecALALkJOb = lepBzpGctuovMroKInIAhbRovCmx;
						return crhllplIzjIXNDYZaWKlrZTyKbWg2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class xhbuGfOHiGEaJESjhMhJERIITknVb : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
				{
					private int GnPqxwTzaYfCrrMqNJtjVppyjnwBA;

					private ElementAssignmentConflictInfo ljMFBjhoTQWfaNcfzmBOFnoVVxxm;

					private int vwTHysZADPurSTzahLTDrLWFLGtC;

					private int ZMBIavLAijAmvfVlEaKzHjagfeDCA;

					public int fVLdNeFUtmKTyACYOZpunQrkHoIU;

					private ActionElementMap YSaBflMqFsiLMfdBGfsMlvDYgubcb;

					public ActionElementMap KiAcbmltwIfpEUuOCdMIsPSOMwkj;

					private bool usDSPFKihEPoHhcSyUgRhzIVykqd;

					public bool aNqAIpCNeTMjkDvxMyKEPlCZgAIYA;

					private MouseMap veUxMBWkzBqGhNFWSRfiPqLEIpaT;

					public MouseMap MwgdNfFrhJauHMGvnhDDKQLrbPsEb;

					private bool tDkJKfbeGOfvmrgupypQAfOWaigEA;

					public bool drokovmZvqaNnGQMEgWsKFkdfMnp;

					private bool aMbCJkIQPhSpUZKokdoQerNCBWLJ;

					public bool NFOVxmpbHaXnmrpIacnqIyxceFdSA;

					private IList<Player> hsEGLCkCugMshTHkkGaHiJPoTeSU;

					private int HLXqXzdoUJRmWMfBoblLaMTLPsDt;

					private IEnumerator<ElementAssignmentConflictInfo> ArVmjNtnEczRQTQqPbPwLfrffMdb;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return ljMFBjhoTQWfaNcfzmBOFnoVVxxm;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return ljMFBjhoTQWfaNcfzmBOFnoVVxxm;
						}
					}

					[DebuggerHidden]
					public xhbuGfOHiGEaJESjhMhJERIITknVb(int P_0)
					{
						GnPqxwTzaYfCrrMqNJtjVppyjnwBA = P_0;
						vwTHysZADPurSTzahLTDrLWFLGtC = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int gnPqxwTzaYfCrrMqNJtjVppyjnwBA = GnPqxwTzaYfCrrMqNJtjVppyjnwBA;
						if (gnPqxwTzaYfCrrMqNJtjVppyjnwBA == -3 || gnPqxwTzaYfCrrMqNJtjVppyjnwBA == 1)
						{
							try
							{
							}
							finally
							{
								KdMewUGfVJuzceGecxSNBMFWYkEW();
							}
						}
						hsEGLCkCugMshTHkkGaHiJPoTeSU = null;
						ArVmjNtnEczRQTQqPbPwLfrffMdb = null;
						GnPqxwTzaYfCrrMqNJtjVppyjnwBA = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int gnPqxwTzaYfCrrMqNJtjVppyjnwBA = GnPqxwTzaYfCrrMqNJtjVppyjnwBA;
							if (gnPqxwTzaYfCrrMqNJtjVppyjnwBA != 0)
							{
								if (gnPqxwTzaYfCrrMqNJtjVppyjnwBA != 1)
								{
									return false;
								}
								GnPqxwTzaYfCrrMqNJtjVppyjnwBA = -3;
								goto IL_00dc;
							}
							GnPqxwTzaYfCrrMqNJtjVppyjnwBA = -1;
							if (ZMBIavLAijAmvfVlEaKzHjagfeDCA < 0 || YSaBflMqFsiLMfdBGfsMlvDYgubcb == null)
							{
								return false;
							}
							hsEGLCkCugMshTHkkGaHiJPoTeSU = (usDSPFKihEPoHhcSyUgRhzIVykqd ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
							HLXqXzdoUJRmWMfBoblLaMTLPsDt = 0;
							goto IL_0106;
							IL_0106:
							if (HLXqXzdoUJRmWMfBoblLaMTLPsDt < hsEGLCkCugMshTHkkGaHiJPoTeSU.Count)
							{
								ArVmjNtnEczRQTQqPbPwLfrffMdb = hsEGLCkCugMshTHkkGaHiJPoTeSU[HLXqXzdoUJRmWMfBoblLaMTLPsDt].controllers.conflictChecking.ElementAssignmentConflicts(ControllerType.Mouse, 0, veUxMBWkzBqGhNFWSRfiPqLEIpaT, YSaBflMqFsiLMfdBGfsMlvDYgubcb, tDkJKfbeGOfvmrgupypQAfOWaigEA, aMbCJkIQPhSpUZKokdoQerNCBWLJ).GetEnumerator();
								GnPqxwTzaYfCrrMqNJtjVppyjnwBA = -3;
								goto IL_00dc;
							}
							return false;
							IL_00dc:
							if (ArVmjNtnEczRQTQqPbPwLfrffMdb.MoveNext())
							{
								ElementAssignmentConflictInfo current = ArVmjNtnEczRQTQqPbPwLfrffMdb.Current;
								ljMFBjhoTQWfaNcfzmBOFnoVVxxm = current;
								GnPqxwTzaYfCrrMqNJtjVppyjnwBA = 1;
								return true;
							}
							KdMewUGfVJuzceGecxSNBMFWYkEW();
							ArVmjNtnEczRQTQqPbPwLfrffMdb = null;
							HLXqXzdoUJRmWMfBoblLaMTLPsDt++;
							goto IL_0106;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void KdMewUGfVJuzceGecxSNBMFWYkEW()
					{
						GnPqxwTzaYfCrrMqNJtjVppyjnwBA = -1;
						if (ArVmjNtnEczRQTQqPbPwLfrffMdb != null)
						{
							ArVmjNtnEczRQTQqPbPwLfrffMdb.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
					{
						xhbuGfOHiGEaJESjhMhJERIITknVb xhbuGfOHiGEaJESjhMhJERIITknVb2;
						if (GnPqxwTzaYfCrrMqNJtjVppyjnwBA == -2 && vwTHysZADPurSTzahLTDrLWFLGtC == Environment.CurrentManagedThreadId)
						{
							GnPqxwTzaYfCrrMqNJtjVppyjnwBA = 0;
							xhbuGfOHiGEaJESjhMhJERIITknVb2 = this;
						}
						else
						{
							xhbuGfOHiGEaJESjhMhJERIITknVb2 = new xhbuGfOHiGEaJESjhMhJERIITknVb(0);
						}
						xhbuGfOHiGEaJESjhMhJERIITknVb2.ZMBIavLAijAmvfVlEaKzHjagfeDCA = fVLdNeFUtmKTyACYOZpunQrkHoIU;
						xhbuGfOHiGEaJESjhMhJERIITknVb2.veUxMBWkzBqGhNFWSRfiPqLEIpaT = MwgdNfFrhJauHMGvnhDDKQLrbPsEb;
						xhbuGfOHiGEaJESjhMhJERIITknVb2.YSaBflMqFsiLMfdBGfsMlvDYgubcb = KiAcbmltwIfpEUuOCdMIsPSOMwkj;
						xhbuGfOHiGEaJESjhMhJERIITknVb2.tDkJKfbeGOfvmrgupypQAfOWaigEA = drokovmZvqaNnGQMEgWsKFkdfMnp;
						xhbuGfOHiGEaJESjhMhJERIITknVb2.aMbCJkIQPhSpUZKokdoQerNCBWLJ = NFOVxmpbHaXnmrpIacnqIyxceFdSA;
						xhbuGfOHiGEaJESjhMhJERIITknVb2.usDSPFKihEPoHhcSyUgRhzIVykqd = aNqAIpCNeTMjkDvxMyKEPlCZgAIYA;
						return xhbuGfOHiGEaJESjhMhJERIITknVb2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class IwNDXkiwIaJIWjdtroSLgUPoppWjb : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
				{
					private int AdbeQFWRXMrwZdmlQUZPrVYKMIko;

					private ElementAssignmentConflictInfo SxshcMcgHnxeuNIvuGXKDadbiczAb;

					private int EQJBBHUcLCHfeDttbDfFZuDPYXAuA;

					private ElementAssignmentConflictCheck pUFrnmJocWeNmzqJyqflunuenFND;

					public ElementAssignmentConflictCheck osmtTkiXujMIWYOEjLAYsuNAncBt;

					private bool eJrBeqLjAHKWylHsoaHJxYEAfBKn;

					public bool UparmnBcmgiGYapzvqhMAMUmSqrn;

					private bool yNmVxsPCVRigdyghrrOffHfGjRTm;

					public bool axHEiMqartGZijDzKTdgKpCWaVheA;

					private bool GcxeHfkyrGqYCroQzgeQjpLTHlKiA;

					public bool QGLXmIRIGVWHNVamLIFymqFizcsA;

					private IList<Player> RDjWHuaVghqEcGjUyvChNVuIRznS;

					private int mBlngRXDnqkMFQHGheECKmjimDsFA;

					private IEnumerator<ElementAssignmentConflictInfo> RVGFexZNrKZtzkxhBqRjBISsysPd;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return SxshcMcgHnxeuNIvuGXKDadbiczAb;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return SxshcMcgHnxeuNIvuGXKDadbiczAb;
						}
					}

					[DebuggerHidden]
					public IwNDXkiwIaJIWjdtroSLgUPoppWjb(int P_0)
					{
						AdbeQFWRXMrwZdmlQUZPrVYKMIko = P_0;
						EQJBBHUcLCHfeDttbDfFZuDPYXAuA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int adbeQFWRXMrwZdmlQUZPrVYKMIko = AdbeQFWRXMrwZdmlQUZPrVYKMIko;
						if (adbeQFWRXMrwZdmlQUZPrVYKMIko == -3 || adbeQFWRXMrwZdmlQUZPrVYKMIko == 1)
						{
							try
							{
							}
							finally
							{
								VpvlEEyOMiXqCLOPOeIokxTmiTKHA();
							}
						}
						RDjWHuaVghqEcGjUyvChNVuIRznS = null;
						RVGFexZNrKZtzkxhBqRjBISsysPd = null;
						AdbeQFWRXMrwZdmlQUZPrVYKMIko = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int adbeQFWRXMrwZdmlQUZPrVYKMIko = AdbeQFWRXMrwZdmlQUZPrVYKMIko;
							if (adbeQFWRXMrwZdmlQUZPrVYKMIko != 0)
							{
								if (adbeQFWRXMrwZdmlQUZPrVYKMIko != 1)
								{
									return false;
								}
								AdbeQFWRXMrwZdmlQUZPrVYKMIko = -3;
								goto IL_00df;
							}
							AdbeQFWRXMrwZdmlQUZPrVYKMIko = -1;
							if (pUFrnmJocWeNmzqJyqflunuenFND.playerId < 0 || pUFrnmJocWeNmzqJyqflunuenFND.elementAssignmentType == ElementAssignmentType.KeyboardKey)
							{
								return false;
							}
							RDjWHuaVghqEcGjUyvChNVuIRznS = (eJrBeqLjAHKWylHsoaHJxYEAfBKn ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
							mBlngRXDnqkMFQHGheECKmjimDsFA = 0;
							goto IL_0109;
							IL_0109:
							if (mBlngRXDnqkMFQHGheECKmjimDsFA < RDjWHuaVghqEcGjUyvChNVuIRznS.Count)
							{
								RVGFexZNrKZtzkxhBqRjBISsysPd = RDjWHuaVghqEcGjUyvChNVuIRznS[mBlngRXDnqkMFQHGheECKmjimDsFA].controllers.conflictChecking.ElementAssignmentConflicts(pUFrnmJocWeNmzqJyqflunuenFND, yNmVxsPCVRigdyghrrOffHfGjRTm, GcxeHfkyrGqYCroQzgeQjpLTHlKiA).GetEnumerator();
								AdbeQFWRXMrwZdmlQUZPrVYKMIko = -3;
								goto IL_00df;
							}
							return false;
							IL_00df:
							if (RVGFexZNrKZtzkxhBqRjBISsysPd.MoveNext())
							{
								ElementAssignmentConflictInfo current = RVGFexZNrKZtzkxhBqRjBISsysPd.Current;
								SxshcMcgHnxeuNIvuGXKDadbiczAb = current;
								AdbeQFWRXMrwZdmlQUZPrVYKMIko = 1;
								return true;
							}
							VpvlEEyOMiXqCLOPOeIokxTmiTKHA();
							RVGFexZNrKZtzkxhBqRjBISsysPd = null;
							mBlngRXDnqkMFQHGheECKmjimDsFA++;
							goto IL_0109;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void VpvlEEyOMiXqCLOPOeIokxTmiTKHA()
					{
						AdbeQFWRXMrwZdmlQUZPrVYKMIko = -1;
						if (RVGFexZNrKZtzkxhBqRjBISsysPd != null)
						{
							RVGFexZNrKZtzkxhBqRjBISsysPd.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
					{
						IwNDXkiwIaJIWjdtroSLgUPoppWjb iwNDXkiwIaJIWjdtroSLgUPoppWjb;
						if (AdbeQFWRXMrwZdmlQUZPrVYKMIko == -2 && EQJBBHUcLCHfeDttbDfFZuDPYXAuA == Environment.CurrentManagedThreadId)
						{
							AdbeQFWRXMrwZdmlQUZPrVYKMIko = 0;
							iwNDXkiwIaJIWjdtroSLgUPoppWjb = this;
						}
						else
						{
							iwNDXkiwIaJIWjdtroSLgUPoppWjb = new IwNDXkiwIaJIWjdtroSLgUPoppWjb(0);
						}
						iwNDXkiwIaJIWjdtroSLgUPoppWjb.pUFrnmJocWeNmzqJyqflunuenFND = osmtTkiXujMIWYOEjLAYsuNAncBt;
						iwNDXkiwIaJIWjdtroSLgUPoppWjb.yNmVxsPCVRigdyghrrOffHfGjRTm = axHEiMqartGZijDzKTdgKpCWaVheA;
						iwNDXkiwIaJIWjdtroSLgUPoppWjb.GcxeHfkyrGqYCroQzgeQjpLTHlKiA = QGLXmIRIGVWHNVamLIFymqFizcsA;
						iwNDXkiwIaJIWjdtroSLgUPoppWjb.eJrBeqLjAHKWylHsoaHJxYEAfBKn = UparmnBcmgiGYapzvqhMAMUmSqrn;
						return iwNDXkiwIaJIWjdtroSLgUPoppWjb;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private static ConflictCheckingHelper ImGZgBwwfYNBgRcgoMlARdsxzFGt;

				internal static ConflictCheckingHelper AweWlCsAhkEgqcKitSpRSXfMOXSX => ImGZgBwwfYNBgRcgoMlARdsxzFGt ?? (ImGZgBwwfYNBgRcgoMlARdsxzFGt = new ConflictCheckingHelper());

				private ConflictCheckingHelper()
				{
				}

				public bool DoesAnyElementAssignmentConflict()
				{
					return DoesAnyElementAssignmentConflict(skipDisabledMaps: false, forceCheckAllCategories: false, includeSystemPlayer: true);
				}

				public bool DoesAnyElementAssignmentConflict(bool skipDisabledMaps)
				{
					return DoesAnyElementAssignmentConflict(skipDisabledMaps, forceCheckAllCategories: false, includeSystemPlayer: true);
				}

				public bool DoesAnyElementAssignmentConflict(bool skipDisabledMaps, bool forceCheckAllCategories)
				{
					return DoesAnyElementAssignmentConflict(skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer: true);
				}

				public bool DoesAnyElementAssignmentConflict(bool skipDisabledMaps, bool forceCheckAllCategories, bool includeSystemPlayer)
				{
					if (!CheckInitialized())
					{
						return false;
					}
					IList<Player> list = (includeSystemPlayer ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						Player player = list[i];
						int num = (forceCheckAllCategories ? i : 0);
						IList<Joystick> joysticks = player.controllers.Joysticks;
						for (int j = 0; j < joysticks.Count; j++)
						{
							Joystick joystick = joysticks[j];
							IList<JoystickMap> maps = player.controllers.maps.GetMaps<JoystickMap>(joystick.id);
							if (maps == null)
							{
								continue;
							}
							int count2 = maps.Count;
							for (int k = num; k < count; k++)
							{
								Player player2 = list[k];
								for (int l = 0; l < count2; l++)
								{
									if (player2.controllers.conflictChecking.DoesElementAssignmentConflict(ControllerType.Joystick, joystick.id, maps[l], skipDisabledMaps, forceCheckAllCategories))
									{
										return true;
									}
								}
							}
						}
						IList<KeyboardMap> maps2 = player.controllers.maps.GetMaps<KeyboardMap>(0);
						for (int m = 0; m < maps2.Count; m++)
						{
							for (int n = num; n < count; n++)
							{
								if (list[n].controllers.conflictChecking.DoesElementAssignmentConflict(ControllerType.Keyboard, 0, maps2[m], skipDisabledMaps, forceCheckAllCategories))
								{
									return true;
								}
							}
						}
						IList<MouseMap> maps3 = player.controllers.maps.GetMaps<MouseMap>(0);
						for (int num2 = 0; num2 < maps3.Count; num2++)
						{
							for (int num3 = num; num3 < count; num3++)
							{
								if (list[num3].controllers.conflictChecking.DoesElementAssignmentConflict(ControllerType.Mouse, 0, maps3[num2], skipDisabledMaps, forceCheckAllCategories))
								{
									return true;
								}
							}
						}
						IList<CustomController> customControllers = player.controllers.CustomControllers;
						for (int num4 = 0; num4 < customControllers.Count; num4++)
						{
							CustomController customController = customControllers[num4];
							IList<CustomControllerMap> maps4 = player.controllers.maps.GetMaps<CustomControllerMap>(customController.id);
							if (maps4 == null)
							{
								continue;
							}
							int count3 = maps4.Count;
							for (int num5 = num; num5 < count; num5++)
							{
								Player player3 = list[num5];
								for (int num6 = 0; num6 < count3; num6++)
								{
									if (player3.controllers.conflictChecking.DoesElementAssignmentConflict(ControllerType.Custom, customController.id, maps4[num6], skipDisabledMaps, forceCheckAllCategories))
									{
										return true;
									}
								}
							}
						}
					}
					return false;
				}

				public bool DoesElementAssignmentConflict(int playerId, ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap)
				{
					return DoesElementAssignmentConflict(playerId, controllerType, controllerId, controllerMap, elementMap, skipDisabledMaps: false, forceCheckAllCategories: false, includeSystemPlayer: true);
				}

				public bool DoesElementAssignmentConflict(int playerId, ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipDisabledMaps)
				{
					return DoesElementAssignmentConflict(playerId, controllerType, controllerId, controllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories: false, includeSystemPlayer: true);
				}

				public bool DoesElementAssignmentConflict(int playerId, ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipDisabledMaps, bool forceCheckAllCategories)
				{
					return DoesElementAssignmentConflict(playerId, controllerType, controllerId, controllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer: true);
				}

				public bool DoesElementAssignmentConflict(int playerId, ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipDisabledMaps, bool forceCheckAllCategories, bool includeSystemPlayer)
				{
					if (!CheckInitialized())
					{
						return false;
					}
					if (playerId < 0 || elementMap == null)
					{
						return false;
					}
					return controllerType switch
					{
						ControllerType.Joystick => DEJHTMEqrLmQAyPQoCKWHffuBngn(playerId, controllerId, controllerMap as JoystickMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Keyboard => ZUaIYmvRLNkSkHNUNDkiunDezJnb(playerId, controllerMap as KeyboardMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Mouse => CeckLFcjJCfauYsTQCNmFfoZwruj(playerId, controllerMap as MouseMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Custom => wbArZCheNyFDcoXNCuQiLgevuOZ(playerId, controllerId, controllerMap as CustomControllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						_ => throw new NotImplementedException(), 
					};
				}

				public bool DoesElementAssignmentConflict(ElementAssignmentConflictCheck conflictCheck)
				{
					return DoesElementAssignmentConflict(conflictCheck, skipDisabledMaps: false, forceCheckAllCategories: false, includeSystemPlayer: true);
				}

				public bool DoesElementAssignmentConflict(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
				{
					return DoesElementAssignmentConflict(conflictCheck, skipDisabledMaps, forceCheckAllCategories: false, includeSystemPlayer: true);
				}

				public bool DoesElementAssignmentConflict(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps, bool forceCheckAllCategories)
				{
					return DoesElementAssignmentConflict(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer: true);
				}

				public bool DoesElementAssignmentConflict(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps, bool forceCheckAllCategories, bool includeSystemPlayer)
				{
					if (!CheckInitialized())
					{
						return false;
					}
					if (conflictCheck.playerId < 0)
					{
						return false;
					}
					if (conflictCheck.controllerType == ControllerType.Joystick)
					{
						return uxZsTVYLbLnQEKfpVJMGKzUpFqQX(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Keyboard)
					{
						return BNorVNLzCCnKJShbZIfCAUUOQsoU(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Mouse)
					{
						return CuJdgQnWNKQeEJLIcfPwrCFGzlBv(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Custom)
					{
						return AlPXAaqlHAVojImghBPBIIQNppdgA(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					throw new NotImplementedException();
				}

				private bool DEJHTMEqrLmQAyPQoCKWHffuBngn(int P_0, int P_1, JoystickMap P_2, ActionElementMap P_3, bool P_4 = false, bool P_5 = false, bool P_6 = true)
				{
					if (P_0 < 0 || P_3 == null)
					{
						return false;
					}
					IList<Player> list = (P_6 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].controllers.conflictChecking.DoesElementAssignmentConflict(ControllerType.Joystick, P_1, P_2, P_3, P_4, P_5))
						{
							return true;
						}
					}
					return false;
				}

				private bool uxZsTVYLbLnQEKfpVJMGKzUpFqQX(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return false;
					}
					IList<Player> list = (P_3 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].controllers.conflictChecking.DoesElementAssignmentConflict(P_0, P_1, P_2))
						{
							return true;
						}
					}
					return false;
				}

				private bool ZUaIYmvRLNkSkHNUNDkiunDezJnb(int P_0, KeyboardMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false, bool P_5 = true)
				{
					if (P_0 < 0 || P_2 == null)
					{
						return false;
					}
					IList<Player> list = (P_5 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].controllers.conflictChecking.DoesElementAssignmentConflict(ControllerType.Keyboard, 0, P_1, P_2, P_3, P_4))
						{
							return true;
						}
					}
					return false;
				}

				private bool BNorVNLzCCnKJShbZIfCAUUOQsoU(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType != ElementAssignmentType.KeyboardKey)
					{
						return false;
					}
					IList<Player> list = (P_3 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].controllers.conflictChecking.DoesElementAssignmentConflict(P_0, P_1, P_2))
						{
							return true;
						}
					}
					return false;
				}

				private bool CeckLFcjJCfauYsTQCNmFfoZwruj(int P_0, MouseMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false, bool P_5 = true)
				{
					if (P_0 < 0 || P_2 == null)
					{
						return false;
					}
					IList<Player> list = (P_5 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].controllers.conflictChecking.DoesElementAssignmentConflict(ControllerType.Mouse, 0, P_1, P_2, P_3, P_4))
						{
							return true;
						}
					}
					return false;
				}

				private bool CuJdgQnWNKQeEJLIcfPwrCFGzlBv(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return false;
					}
					IList<Player> list = (P_3 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].controllers.conflictChecking.DoesElementAssignmentConflict(P_0, P_1, P_2))
						{
							return true;
						}
					}
					return false;
				}

				private bool wbArZCheNyFDcoXNCuQiLgevuOZ(int P_0, int P_1, CustomControllerMap P_2, ActionElementMap P_3, bool P_4 = false, bool P_5 = false, bool P_6 = true)
				{
					if (P_0 < 0 || P_3 == null)
					{
						return false;
					}
					IList<Player> list = (P_6 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].controllers.conflictChecking.DoesElementAssignmentConflict(ControllerType.Custom, P_1, P_2, P_3, P_4, P_5))
						{
							return true;
						}
					}
					return false;
				}

				private bool AlPXAaqlHAVojImghBPBIIQNppdgA(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return false;
					}
					IList<Player> list = (P_3 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].controllers.conflictChecking.DoesElementAssignmentConflict(P_0, P_1, P_2))
						{
							return true;
						}
					}
					return false;
				}

				public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(int playerId, ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap)
				{
					return ElementAssignmentConflicts(playerId, controllerType, controllerId, controllerMap, elementMap, skipDisabledMaps: false, forceCheckAllCategories: false, includeSystemPlayer: true);
				}

				public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(int playerId, ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipDisabledMaps)
				{
					return ElementAssignmentConflicts(playerId, controllerType, controllerId, controllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories: false, includeSystemPlayer: true);
				}

				public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(int playerId, ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipDisabledMaps, bool forceCheckAllCategories)
				{
					return ElementAssignmentConflicts(playerId, controllerType, controllerId, controllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer: true);
				}

				public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(int playerId, ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipDisabledMaps, bool forceCheckAllCategories, bool includeSystemPlayer)
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<ElementAssignmentConflictInfo>.EmptyReadOnlyIListT;
					}
					if (playerId < 0 || elementMap == null)
					{
						return new List<ElementAssignmentConflictInfo>();
					}
					return controllerType switch
					{
						ControllerType.Joystick => kyNBvKIMKAfdSRATUkDmaCustFkc(playerId, controllerId, controllerMap as JoystickMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Keyboard => SOkBLyJetfawADKjpWQFmzMiVnpG(playerId, controllerMap as KeyboardMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Mouse => iuHgCVaVcFomlVbuKeybItUgHaVPA(playerId, controllerMap as MouseMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Custom => XFjwAEvflZeovaSJbaiwUDNIqdbNA(playerId, controllerId, controllerMap as CustomControllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						_ => throw new NotImplementedException(), 
					};
				}

				public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck)
				{
					return ElementAssignmentConflicts(conflictCheck, skipDisabledMaps: false, forceCheckAllCategories: false, includeSystemPlayer: true);
				}

				public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
				{
					return ElementAssignmentConflicts(conflictCheck, skipDisabledMaps, forceCheckAllCategories: false, includeSystemPlayer: true);
				}

				public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps, bool forceCheckAllCategories)
				{
					return ElementAssignmentConflicts(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer: true);
				}

				public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps, bool forceCheckAllCategories, bool includeSystemPlayer)
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<ElementAssignmentConflictInfo>.EmptyReadOnlyIListT;
					}
					if (conflictCheck.playerId < 0)
					{
						return new List<ElementAssignmentConflictInfo>();
					}
					if (conflictCheck.controllerType == ControllerType.Joystick)
					{
						return tafDVxHXPZotsRlqItbaWnzjDKnM(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Keyboard)
					{
						return JywBkCwtrYWNGgKgqWFpsXgdMWCd(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Mouse)
					{
						return OepZVuulbFZZbKOQYiaoWisYcIfT(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Custom)
					{
						return ExCdJiEcpIHbZEvjwkxmipNVgVBh(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					throw new NotImplementedException();
				}

				[IteratorStateMachine(typeof(blmVUJkixtaYSECXGEFkRbtzuZMPA))]
				private IEnumerable<ElementAssignmentConflictInfo> kyNBvKIMKAfdSRATUkDmaCustFkc(int P_0, int P_1, JoystickMap P_2, ActionElementMap P_3, bool P_4 = false, bool P_5 = false, bool P_6 = true)
				{
					return new blmVUJkixtaYSECXGEFkRbtzuZMPA(-2)
					{
						AQDPNfPwBVLRllayyQHOYEdkBFXL = P_0,
						YNJcVoGashPlKylcHkKdtzCOOJXm = P_1,
						ZBuBqcoNAnwcjEBpugSEhvAvgzHc = P_2,
						VJDtxGbWcZUFlQowPtRNqDbPhqPfA = P_3,
						YGQBQDEdmfenDGccpHupAOVmCwUX = P_4,
						TjZCyIEfFyJBDKDIRfcCwHeYJTeA = P_5,
						EgWFgaCHOSZEPrJNDllubxYAcgci = P_6
					};
				}

				[IteratorStateMachine(typeof(AnAzGkzadSctmQwLhspVzLbReEQeA))]
				private IEnumerable<ElementAssignmentConflictInfo> tafDVxHXPZotsRlqItbaWnzjDKnM(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					return new AnAzGkzadSctmQwLhspVzLbReEQeA(-2)
					{
						EhMwLSzqEMzGsZsCojjaOckrtuTu = P_0,
						QAIqVNKNlGFjnMgnSitpdRiWLAbzA = P_1,
						zWaksVIUKZQKZxNSfENiEcodnQVcb = P_2,
						NplLbgizINhbiorsMJxGKEenVAb = P_3
					};
				}

				[IteratorStateMachine(typeof(XwYcIBYAJUXSfkaacNnSYtphnNNG))]
				private IEnumerable<ElementAssignmentConflictInfo> SOkBLyJetfawADKjpWQFmzMiVnpG(int P_0, KeyboardMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false, bool P_5 = true)
				{
					return new XwYcIBYAJUXSfkaacNnSYtphnNNG(-2)
					{
						KQRtIqsBlkvNcdWtUrpQHaTPpchy = P_0,
						ZVGxvwsuFTejXikpbJOYAKKSUmGEA = P_1,
						dUpgTLmEeMfhgAyAoLkYSgQGWWJL = P_2,
						UvHiyrxSxVnMsGOFIeYvIqyuDLlr = P_3,
						CSclqCaWYVXmfJIRNiflPlqJgWly = P_4,
						lHJUkjEEvVMtOhXURqmKUCqEQHBw = P_5
					};
				}

				[IteratorStateMachine(typeof(crhllplIzjIXNDYZaWKlrZTyKbWg))]
				private IEnumerable<ElementAssignmentConflictInfo> JywBkCwtrYWNGgKgqWFpsXgdMWCd(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					return new crhllplIzjIXNDYZaWKlrZTyKbWg(-2)
					{
						hOsiBBbBmAeTyCfwymfKihihNxsQB = P_0,
						poXGrPFrqhWCpWGivLgkjiMQKPpRA = P_1,
						POCaQGCVzzTgtZJchpTtAXyKShwx = P_2,
						lepBzpGctuovMroKInIAhbRovCmx = P_3
					};
				}

				[IteratorStateMachine(typeof(xhbuGfOHiGEaJESjhMhJERIITknVb))]
				private IEnumerable<ElementAssignmentConflictInfo> iuHgCVaVcFomlVbuKeybItUgHaVPA(int P_0, MouseMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false, bool P_5 = true)
				{
					return new xhbuGfOHiGEaJESjhMhJERIITknVb(-2)
					{
						fVLdNeFUtmKTyACYOZpunQrkHoIU = P_0,
						MwgdNfFrhJauHMGvnhDDKQLrbPsEb = P_1,
						KiAcbmltwIfpEUuOCdMIsPSOMwkj = P_2,
						drokovmZvqaNnGQMEgWsKFkdfMnp = P_3,
						NFOVxmpbHaXnmrpIacnqIyxceFdSA = P_4,
						aNqAIpCNeTMjkDvxMyKEPlCZgAIYA = P_5
					};
				}

				[IteratorStateMachine(typeof(IwNDXkiwIaJIWjdtroSLgUPoppWjb))]
				private IEnumerable<ElementAssignmentConflictInfo> OepZVuulbFZZbKOQYiaoWisYcIfT(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					return new IwNDXkiwIaJIWjdtroSLgUPoppWjb(-2)
					{
						osmtTkiXujMIWYOEjLAYsuNAncBt = P_0,
						axHEiMqartGZijDzKTdgKpCWaVheA = P_1,
						QGLXmIRIGVWHNVamLIFymqFizcsA = P_2,
						UparmnBcmgiGYapzvqhMAMUmSqrn = P_3
					};
				}

				[IteratorStateMachine(typeof(elcJFlbtidtJPAaetngjzFYnDLFJ))]
				private IEnumerable<ElementAssignmentConflictInfo> XFjwAEvflZeovaSJbaiwUDNIqdbNA(int P_0, int P_1, CustomControllerMap P_2, ActionElementMap P_3, bool P_4 = false, bool P_5 = false, bool P_6 = true)
				{
					return new elcJFlbtidtJPAaetngjzFYnDLFJ(-2)
					{
						eUUmcjgyokhGrOcdQSNRfzgwkzos = P_0,
						VdTUZdTsUAyWzNHaSEnbQIFRmpId = P_1,
						halNauhzzdNhniuXuqSxiDiYfQpf = P_2,
						BirhbQbrBqQjJXPQwXORqIAYwWeN = P_3,
						wCJQYaTFkyKfqGcLbbUwIYkdHiBc = P_4,
						FAzTXyErejPwySWbcMRoPeXDDBBT = P_5,
						wSMIfwjODbwUFjbqrQOMBOwMotkF = P_6
					};
				}

				[IteratorStateMachine(typeof(BWHtDKxaUGvTHhaJXXmhhiLuVdUA))]
				private IEnumerable<ElementAssignmentConflictInfo> ExCdJiEcpIHbZEvjwkxmipNVgVBh(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					return new BWHtDKxaUGvTHhaJXXmhhiLuVdUA(-2)
					{
						ZIJNcDnmfGDhbAWVHfVCVzocJiBm = P_0,
						uJdfzIySjgPIgiNTeHKfZvrSldcx = P_1,
						swbeAFRJAHBWIofcurfcJgNEcQhU = P_2,
						pCtTEYsCpdcdPAQRMRxqjUZTiyFNA = P_3
					};
				}

				public int RemoveElementAssignmentConflicts(int playerId, ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap)
				{
					return RemoveElementAssignmentConflicts(playerId, controllerType, controllerId, controllerMap, elementMap, skipDisabledMaps: false, forceCheckAllCategories: false, includeSystemPlayer: true);
				}

				public int RemoveElementAssignmentConflicts(int playerId, ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipDisabledMaps)
				{
					return RemoveElementAssignmentConflicts(playerId, controllerType, controllerId, controllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories: false, includeSystemPlayer: true);
				}

				public int RemoveElementAssignmentConflicts(int playerId, ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipDisabledMaps, bool forceCheckAllCategories)
				{
					return RemoveElementAssignmentConflicts(playerId, controllerType, controllerId, controllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer: true);
				}

				public int RemoveElementAssignmentConflicts(int playerId, ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipDisabledMaps, bool forceCheckAllCategories, bool includeSystemPlayer)
				{
					if (!CheckInitialized())
					{
						return 0;
					}
					if (playerId < 0 || elementMap == null)
					{
						return 0;
					}
					return controllerType switch
					{
						ControllerType.Joystick => rNjsZyAouiDiXtLtmsyyptOtNWbK(playerId, controllerId, controllerMap as JoystickMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Keyboard => vygkcYvUnwOGPdPlGSZgBucPtCOA(playerId, controllerMap as KeyboardMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Mouse => RTLAVitAaPFCEBakYKIZoYYzrZLF(playerId, controllerMap as MouseMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Custom => UZoLuidPBvrqhtIcpjYsuiLRzFjD(playerId, controllerId, controllerMap as CustomControllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						_ => throw new NotImplementedException(), 
					};
				}

				public int RemoveElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck)
				{
					return RemoveElementAssignmentConflicts(conflictCheck, skipDisabledMaps: false, forceCheckAllCategories: false, includeSystemPlayer: true);
				}

				public int RemoveElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
				{
					return RemoveElementAssignmentConflicts(conflictCheck, skipDisabledMaps, forceCheckAllCategories: false, includeSystemPlayer: true);
				}

				public int RemoveElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps, bool forceCheckAllCategories)
				{
					return RemoveElementAssignmentConflicts(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer: true);
				}

				public int RemoveElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps, bool forceCheckAllCategories, bool includeSystemPlayer)
				{
					if (!CheckInitialized())
					{
						return 0;
					}
					if (conflictCheck.playerId < 0)
					{
						return 0;
					}
					if (conflictCheck.controllerType == ControllerType.Joystick)
					{
						return MyPOvbOhUnzVDfdHtwehNcMuRbTe(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Keyboard)
					{
						return iPysdzzqSmpYmBKxuEVSEenSsEPO(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Mouse)
					{
						return jHSibWpajQAlgbvyLuTYsdrIneRkA(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Custom)
					{
						return vFcZpTCtPYWJuHBlJQcPRbBvepp(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					throw new NotImplementedException();
				}

				private int rNjsZyAouiDiXtLtmsyyptOtNWbK(int P_0, int P_1, JoystickMap P_2, ActionElementMap P_3, bool P_4 = false, bool P_5 = false, bool P_6 = true)
				{
					if (P_0 < 0 || P_3 == null)
					{
						return 0;
					}
					IList<Player> list = (P_6 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.RemoveElementAssignmentConflicts(ControllerType.Joystick, P_1, P_2, P_3, P_4, P_5);
					}
					return num;
				}

				private int MyPOvbOhUnzVDfdHtwehNcMuRbTe(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					IList<Player> list = (P_3 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.RemoveElementAssignmentConflicts(P_0, P_1, P_2);
					}
					return num;
				}

				private int vygkcYvUnwOGPdPlGSZgBucPtCOA(int P_0, KeyboardMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false, bool P_5 = true)
				{
					if (P_0 < 0 || P_2 == null)
					{
						return 0;
					}
					IList<Player> list = (P_5 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.RemoveElementAssignmentConflicts(ControllerType.Keyboard, 0, P_1, P_2, P_3, P_4);
					}
					return num;
				}

				private int iPysdzzqSmpYmBKxuEVSEenSsEPO(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType != ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					IList<Player> list = (P_3 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.RemoveElementAssignmentConflicts(P_0, P_1, P_2);
					}
					return num;
				}

				private int RTLAVitAaPFCEBakYKIZoYYzrZLF(int P_0, MouseMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false, bool P_5 = true)
				{
					if (P_0 < 0 || P_2 == null)
					{
						return 0;
					}
					IList<Player> list = (P_5 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.RemoveElementAssignmentConflicts(ControllerType.Mouse, 0, P_1, P_2, P_3, P_4);
					}
					return num;
				}

				private int jHSibWpajQAlgbvyLuTYsdrIneRkA(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					IList<Player> list = (P_3 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.RemoveElementAssignmentConflicts(P_0, P_1, P_2);
					}
					return num;
				}

				private int UZoLuidPBvrqhtIcpjYsuiLRzFjD(int P_0, int P_1, CustomControllerMap P_2, ActionElementMap P_3, bool P_4 = false, bool P_5 = false, bool P_6 = true)
				{
					if (P_0 < 0 || P_3 == null)
					{
						return 0;
					}
					IList<Player> list = (P_6 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.RemoveElementAssignmentConflicts(ControllerType.Custom, P_1, P_2, P_3, P_4, P_5);
					}
					return num;
				}

				private int vFcZpTCtPYWJuHBlJQcPRbBvepp(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					IList<Player> list = (P_3 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.RemoveElementAssignmentConflicts(P_0, P_1, P_2);
					}
					return num;
				}

				public int DisableElementAssignmentConflicts(int playerId, ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap)
				{
					return DisableElementAssignmentConflicts(playerId, controllerType, controllerId, controllerMap, elementMap, skipDisabledMaps: false, forceCheckAllCategories: false, includeSystemPlayer: true);
				}

				public int DisableElementAssignmentConflicts(int playerId, ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipDisabledMaps)
				{
					return DisableElementAssignmentConflicts(playerId, controllerType, controllerId, controllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories: false, includeSystemPlayer: true);
				}

				public int DisableElementAssignmentConflicts(int playerId, ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipDisabledMaps, bool forceCheckAllCategories)
				{
					return DisableElementAssignmentConflicts(playerId, controllerType, controllerId, controllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer: true);
				}

				public int DisableElementAssignmentConflicts(int playerId, ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipDisabledMaps, bool forceCheckAllCategories, bool includeSystemPlayer)
				{
					if (!CheckInitialized())
					{
						return 0;
					}
					if (playerId < 0 || elementMap == null)
					{
						return 0;
					}
					return controllerType switch
					{
						ControllerType.Joystick => gOMNXruBeYwOOciKjGcnhzcgvbUmA(playerId, controllerId, controllerMap as JoystickMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Keyboard => DpJAkuiIzSMeuBSiRJRgXmIfLDIDb(playerId, controllerMap as KeyboardMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Mouse => nVZfVFcDfHBkkbyUEwDdzajslYgxB(playerId, controllerMap as MouseMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Custom => PvcBLCLBkPuENXkUEtSbFQNOIOXT(playerId, controllerId, controllerMap as CustomControllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						_ => throw new NotImplementedException(), 
					};
				}

				public int DisableElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck)
				{
					return DisableElementAssignmentConflicts(conflictCheck, skipDisabledMaps: false, forceCheckAllCategories: false, includeSystemPlayer: true);
				}

				public int DisableElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
				{
					return DisableElementAssignmentConflicts(conflictCheck, skipDisabledMaps, forceCheckAllCategories: false, includeSystemPlayer: true);
				}

				public int DisableElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps, bool forceCheckAllCategories)
				{
					return DisableElementAssignmentConflicts(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer: true);
				}

				public int DisableElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps, bool forceCheckAllCategories, bool includeSystemPlayer)
				{
					if (!CheckInitialized())
					{
						return 0;
					}
					if (conflictCheck.playerId < 0)
					{
						return 0;
					}
					if (conflictCheck.controllerType == ControllerType.Joystick)
					{
						return QStVkfjIjfkQLQYCPgoJpMgwFUVC(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Keyboard)
					{
						return bMbpLeXZgQqlfJJDPeibXesbRmLG(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Mouse)
					{
						return NESIUlyEjPQbKcCKmzNyGvCsgcAb(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Custom)
					{
						return GVgOFeNHQcNQQnczWaOeUEqbgGJG(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					throw new NotImplementedException();
				}

				private int gOMNXruBeYwOOciKjGcnhzcgvbUmA(int P_0, int P_1, JoystickMap P_2, ActionElementMap P_3, bool P_4 = false, bool P_5 = false, bool P_6 = true)
				{
					if (P_0 < 0 || P_3 == null)
					{
						return 0;
					}
					IList<Player> list = (P_6 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.DisableElementAssignmentConflicts(ControllerType.Joystick, P_1, P_2, P_3, P_4, P_5);
					}
					return num;
				}

				private int QStVkfjIjfkQLQYCPgoJpMgwFUVC(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					IList<Player> list = (P_3 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.DisableElementAssignmentConflicts(P_0, P_1, P_2);
					}
					return num;
				}

				private int DpJAkuiIzSMeuBSiRJRgXmIfLDIDb(int P_0, KeyboardMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false, bool P_5 = true)
				{
					if (P_0 < 0 || P_2 == null)
					{
						return 0;
					}
					IList<Player> list = (P_5 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.DisableElementAssignmentConflicts(ControllerType.Keyboard, 0, P_1, P_2, P_3, P_4);
					}
					return num;
				}

				private int bMbpLeXZgQqlfJJDPeibXesbRmLG(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType != ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					IList<Player> list = (P_3 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.DisableElementAssignmentConflicts(P_0, P_1, P_2);
					}
					return num;
				}

				private int nVZfVFcDfHBkkbyUEwDdzajslYgxB(int P_0, MouseMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false, bool P_5 = true)
				{
					if (P_0 < 0 || P_2 == null)
					{
						return 0;
					}
					IList<Player> list = (P_5 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.DisableElementAssignmentConflicts(ControllerType.Mouse, 0, P_1, P_2, P_3, P_4);
					}
					return num;
				}

				private int NESIUlyEjPQbKcCKmzNyGvCsgcAb(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					IList<Player> list = (P_3 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.DisableElementAssignmentConflicts(P_0, P_1, P_2);
					}
					return num;
				}

				private int PvcBLCLBkPuENXkUEtSbFQNOIOXT(int P_0, int P_1, CustomControllerMap P_2, ActionElementMap P_3, bool P_4 = false, bool P_5 = false, bool P_6 = true)
				{
					if (P_0 < 0 || P_3 == null)
					{
						return 0;
					}
					IList<Player> list = (P_6 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.DisableElementAssignmentConflicts(ControllerType.Custom, P_1, P_2, P_3, P_4, P_5);
					}
					return num;
				}

				private int GVgOFeNHQcNQQnczWaOeUEqbgGJG(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					IList<Player> list = (P_3 ? KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh : KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.DisableElementAssignmentConflicts(P_0, P_1, P_2);
					}
					return num;
				}
			}

			private static ControllerHelper dkElXyQnzxrXuvRemqEYcPTuXWsS;

			public readonly PollingHelper polling = PollingHelper.gzPDLVqOzmSazIxJQeONMUWYYaY;

			public readonly ConflictCheckingHelper conflictChecking = ConflictCheckingHelper.AweWlCsAhkEgqcKitSpRSXfMOXSX;

			internal static ControllerHelper LpVAXpiVHlQcnvltFwbImSKTLatF => dkElXyQnzxrXuvRemqEYcPTuXWsS ?? (dkElXyQnzxrXuvRemqEYcPTuXWsS = new ControllerHelper());

			public int controllerCount
			{
				get
				{
					if (!CheckInitialized())
					{
						return 0;
					}
					return FoarDfUMCtoVFquEtrllUhEjZUUn.yLjTfJFmvSvPPOwImgrFOhMEynMR;
				}
			}

			public IList<Controller> Controllers
			{
				get
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<Controller>.EmptyReadOnlyIListT;
					}
					return FoarDfUMCtoVFquEtrllUhEjZUUn.qxItpjGgcTPOulCPqnbsIgGhWdnn;
				}
			}

			public Mouse Mouse
			{
				get
				{
					if (!CheckInitialized())
					{
						return null;
					}
					return FoarDfUMCtoVFquEtrllUhEjZUUn.RVEYLKIoOydxyctXJKWZflgnfQyi;
				}
			}

			public Keyboard Keyboard
			{
				get
				{
					if (!CheckInitialized())
					{
						return null;
					}
					return FoarDfUMCtoVFquEtrllUhEjZUUn.TVvLxBfEgOqnloHdRFcagvmpmnZT;
				}
			}

			[Obsolete("Deprecated: Use Controller.enabled instead. For example, to disable keyboard input: ReInput.controllers.Keyboard.enabled = false.")]
			public bool keyboardEnabled
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					return Keyboard.enabled;
				}
				set
				{
					if (CheckInitialized())
					{
						Keyboard.enabled = value;
					}
				}
			}

			public int joystickCount
			{
				get
				{
					if (!CheckInitialized())
					{
						return 0;
					}
					return FoarDfUMCtoVFquEtrllUhEjZUUn.tMmOaRdQZinbpzIJqOsQpkLeJiKf;
				}
			}

			public IList<Joystick> Joysticks
			{
				get
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<Joystick>.EmptyReadOnlyIListT;
					}
					return FoarDfUMCtoVFquEtrllUhEjZUUn.vSofOXqlhEhhaEStqlEuVapoCWcU;
				}
			}

			public int customControllerCount
			{
				get
				{
					if (!CheckInitialized())
					{
						return 0;
					}
					return FoarDfUMCtoVFquEtrllUhEjZUUn.OaArgfysJnEWrVVscGbEedXuKtYDA;
				}
			}

			public IList<CustomController> CustomControllers
			{
				get
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<CustomController>.EmptyReadOnlyIListT;
					}
					return FoarDfUMCtoVFquEtrllUhEjZUUn.XyyXBqESkHDJwVnEjfVUWHBlFRAaA;
				}
			}

			private ControllerHelper()
			{
			}

			public T GetController<T>(int controllerId) where T : Controller
			{
				if (!CheckInitialized())
				{
					return null;
				}
				if (controllerId < 0)
				{
					return null;
				}
				Type typeFromHandle = typeof(T);
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Joystick)))
				{
					return GetJoystick(controllerId) as T;
				}
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Keyboard)))
				{
					return FoarDfUMCtoVFquEtrllUhEjZUUn.TVvLxBfEgOqnloHdRFcagvmpmnZT as T;
				}
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(CustomController)))
				{
					return GetCustomController(controllerId) as T;
				}
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Mouse)))
				{
					return FoarDfUMCtoVFquEtrllUhEjZUUn.RVEYLKIoOydxyctXJKWZflgnfQyi as T;
				}
				throw new NotImplementedException();
			}

			public int GetControllerCount(ControllerType controllerType)
			{
				if (!CheckInitialized())
				{
					return 0;
				}
				return controllerType switch
				{
					ControllerType.Joystick => joystickCount, 
					ControllerType.Keyboard => 1, 
					ControllerType.Mouse => 1, 
					ControllerType.Custom => customControllerCount, 
					_ => throw new NotImplementedException(), 
				};
			}

			public Controller GetController(ControllerType controllerType, int controllerId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.QaXmawDzDviOFGGVAiudAlfjSkMM(controllerType, controllerId);
			}

			public Controller GetController(ControllerIdentifier controllerIdentifier)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.UpNCLkybyrfyzcgOPVjvhyzXROTtA(controllerIdentifier);
			}

			public Controller[] GetControllers(ControllerType controllerType)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<Controller>.array;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.jskyfxOoiQdVrOHqoqaLZAUxDpKF(controllerType);
			}

			public string[] GetControllerNames(ControllerType controllerType)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<string>.array;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.rsywZcHxTMzhaMfBitMrospUvwJF(controllerType);
			}

			public bool IsControllerAssigned(ControllerType controllerType, Controller controller)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return KTVqyytqGISutLJQbfhSaKddBlfv.aqhIxgnnmPZVlFbGfmDEGfMBtHtE(controller);
			}

			public bool IsControllerAssigned(ControllerType controllerType, int controllerId)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return KTVqyytqGISutLJQbfhSaKddBlfv.NwncrvnyvquUEQJMoESVHNntGqPF(controllerType, controllerId);
			}

			public bool IsControllerAssignedToPlayer(ControllerType controllerType, int controllerId, int playerId)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return KTVqyytqGISutLJQbfhSaKddBlfv.UKErIMlvBybVbWfLuJdJkBpWtcZM(controllerType, controllerId, playerId);
			}

			public void RemoveControllerFromAllPlayers(Controller controller, bool includeSystemPlayer = true)
			{
				if (CheckInitialized())
				{
					KTVqyytqGISutLJQbfhSaKddBlfv.TytuzXrTqkbdWsnyKEArNueZorIJ(controller, includeSystemPlayer);
				}
			}

			public void RemoveControllerFromAllPlayers(ControllerType controllerType, int controllerId, bool includeSystemPlayer = true)
			{
				if (CheckInitialized())
				{
					KTVqyytqGISutLJQbfhSaKddBlfv.RmrBIoFPNnWxKPKpWAhRbDrtIAlPA(controllerType, controllerId, includeSystemPlayer);
				}
			}

			public Joystick GetJoystick(int joystickId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.ypqDskzQOCOIVJAyamsKXzVhMSuK(joystickId);
			}

			public Joystick[] GetJoysticks()
			{
				return FoarDfUMCtoVFquEtrllUhEjZUUn.idLfsOlrRwIOAwtLOCiEiUsbHLTo();
			}

			public string[] GetJoystickNames()
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<string>.array;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.lbnmTCPcLjbLEFbatcILIzDwyaaFA();
			}

			public bool IsJoystickAssigned(Joystick joystick)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return KTVqyytqGISutLJQbfhSaKddBlfv.srxWbDEXENzLHMKSMWHYNlQvNhsc(joystick);
			}

			public bool IsJoystickAssigned(int joystickId)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return KTVqyytqGISutLJQbfhSaKddBlfv.WomCcKkVEQVBDrjsRIxXGYHxVwVN(joystickId);
			}

			public bool IsJoystickAssignedToPlayer(int joystickId, int playerId)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return KTVqyytqGISutLJQbfhSaKddBlfv.TYBTDZeIKOQqBqGWmCVraQPlywlM(joystickId, playerId);
			}

			public void RemoveJoystickFromAllPlayers(Joystick joystick, bool includeSystemPlayer = true)
			{
				if (CheckInitialized())
				{
					KTVqyytqGISutLJQbfhSaKddBlfv.EAGXsKFcPMELlqeigItnhEsvxBZI(joystick, includeSystemPlayer);
				}
			}

			public void RemoveJoystickFromAllPlayers(int joystickId, bool includeSystemPlayer = true)
			{
				if (CheckInitialized())
				{
					KTVqyytqGISutLJQbfhSaKddBlfv.YPRiSbxpfuHpjHJUEtbJoGCbsTxn(joystickId, includeSystemPlayer);
				}
			}

			public int GetUnityJoystickIdFromAnyButtonPress()
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				if (!ATiGsPhUPWKdkmfLpiSUBNiehnaxb)
				{
					Logger.LogWarning("This can only used when Unity Input is handling input. This has no effect on this platform.");
					return -1;
				}
				knWhEoThuJvwMKpsbrLYbADGIRww();
				for (int i = 0; i < 16; i++)
				{
					for (int j = 0; j < 20; j++)
					{
						if (LtLPnFRiUIztAXIRagmfniCjVdpN.eGoPyXCXwZHKhuRJkbyDzEPnOUog(i, j))
						{
							return i + 1;
						}
					}
				}
				return -1;
			}

			public int GetUnityJoystickIdFromAnyButtonOrAxisPress(float axisThreshold, bool positiveAxesOnly)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				if (!ATiGsPhUPWKdkmfLpiSUBNiehnaxb)
				{
					Logger.LogWarning("This can only used when Unity Input is handling input. This has no effect on this platform.");
					return -1;
				}
				knWhEoThuJvwMKpsbrLYbADGIRww();
				for (int i = 0; i < 16; i++)
				{
					for (int j = 0; j < 20; j++)
					{
						if (LtLPnFRiUIztAXIRagmfniCjVdpN.eGoPyXCXwZHKhuRJkbyDzEPnOUog(i, j))
						{
							return i + 1;
						}
					}
					for (int k = 0; k < 29; k++)
					{
						if (LtLPnFRiUIztAXIRagmfniCjVdpN.OhBZiMWrKxEzdKxlOiKmpUxpsenO(i, k, positiveAxesOnly))
						{
							return i + 1;
						}
					}
				}
				return -1;
			}

			public void SetUnityJoystickId(int joystickId, int unityJoystickId)
			{
				if (CheckInitialized())
				{
					if (!ATiGsPhUPWKdkmfLpiSUBNiehnaxb)
					{
						Logger.LogWarning("This can only used when Unity Input is handling input. This has no effect on this platform.");
					}
					else
					{
						zNGjjScruOFRrdIdVfVzDkdgujPhA.SetUnityJoystickId(joystickId, unityJoystickId);
					}
				}
			}

			public bool SetUnityJoystickIdFromAnyButtonPress(int joystickId)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				int unityJoystickIdFromAnyButtonPress = GetUnityJoystickIdFromAnyButtonPress();
				if (unityJoystickIdFromAnyButtonPress < 1)
				{
					return false;
				}
				SetUnityJoystickId(joystickId, unityJoystickIdFromAnyButtonPress);
				return true;
			}

			public bool SetUnityJoystickIdFromAnyButtonOrAxisPress(int joystickId, float axisThreshold, bool positiveAxesOnly)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				int unityJoystickIdFromAnyButtonOrAxisPress = GetUnityJoystickIdFromAnyButtonOrAxisPress(axisThreshold, positiveAxesOnly);
				if (unityJoystickIdFromAnyButtonOrAxisPress < 1)
				{
					return false;
				}
				SetUnityJoystickId(joystickId, unityJoystickIdFromAnyButtonOrAxisPress);
				return true;
			}

			public CustomController GetCustomController(int customControllerId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.ANdTvUmEGdGDfhVQXzoelxqNrFDv(customControllerId);
			}

			public CustomController[] GetCustomControllers()
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<CustomController>.array;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.FxxKDGBXOHFOjsKatpBkZYlwIcqK();
			}

			public string[] GetCustomControllerNames()
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<string>.array;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.yCalGqhSGCRQTFkwwGPZCSKbajIbA();
			}

			public bool IsCustomControllerAssigned(CustomController customController)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return KTVqyytqGISutLJQbfhSaKddBlfv.oiJesQfscgWrOhYjmBeynHCUTSdNA(customController);
			}

			public bool IsCustomControllerAssigned(int customControllerId)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return KTVqyytqGISutLJQbfhSaKddBlfv.yeQFYalmclJKXhkjlMKlOQQfgrSM(customControllerId);
			}

			public bool IsCustomControllerAssignedToPlayer(int customControllerId, int playerId)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return KTVqyytqGISutLJQbfhSaKddBlfv.mvVxELeVNlCkGSnPKMpeKBsEyIkn(customControllerId, playerId);
			}

			public void RemoveCustomControllerFromAllPlayers(CustomController customController, bool includeSystemPlayer = true)
			{
				if (CheckInitialized())
				{
					KTVqyytqGISutLJQbfhSaKddBlfv.zDSWrprkdisFplLdhSLXGgHeEili(customController, includeSystemPlayer);
				}
			}

			public void RemoveCustomControllerFromAllPlayers(int customControllerId, bool includeSystemPlayer = true)
			{
				if (CheckInitialized())
				{
					KTVqyytqGISutLJQbfhSaKddBlfv.fpCJjwOokofCeibJTwPioFDvRNEFb(customControllerId, includeSystemPlayer);
				}
			}

			public CustomController CreateCustomController(int sourceControllerId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.WWEUAIfEGDhZVmEWMiuGLFXZCtBI(sourceControllerId);
			}

			public CustomController CreateCustomController(int sourceControllerId, string tag)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				CustomController customController = FoarDfUMCtoVFquEtrllUhEjZUUn.WWEUAIfEGDhZVmEWMiuGLFXZCtBI(sourceControllerId);
				if (customController == null)
				{
					return null;
				}
				customController.tag = tag;
				return customController;
			}

			public bool DestroyCustomController(CustomController customController)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				if (customController == null)
				{
					return false;
				}
				RemoveCustomControllerFromAllPlayers(customController);
				return FoarDfUMCtoVFquEtrllUhEjZUUn.vfsQiSVhBXImAuqUKHhFsLuzejdB(customController);
			}

			public CustomController GetFirstCustomControllerWithSourceId(int sourceId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.lVeUTSmCPUqbQipxtaNxiuEOuyyH(sourceId);
			}

			public CustomController GetFirstCustomControllerWithTag(string tag)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.CEyjqrKQwjuACHSruGveGpxtwOju(tag);
			}

			public IEnumerable<CustomController> CustomControllersWithSourceId(int sourceId)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<CustomController>.EmptyReadOnlyIListT;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.sifPkQqpzlkQXgCHDCjVKbyzrFGwA(sourceId);
			}

			public IEnumerable<CustomController> CustomControllersWithTag(string tag)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<CustomController>.EmptyReadOnlyIListT;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.txQQUttZxiiPPsPtUvpInElVchaw(tag);
			}

			public IList<TInterface> GetControllerTemplates<TInterface>() where TInterface : IControllerTemplate
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<TInterface>.EmptyReadOnlyIListT;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.YmQipxvVmDsNqUDHtzzfqGORHoZl<TInterface>();
			}

			public Controller GetLastActiveController()
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.OzkDYWaprGZkxpnnegeOlsUIfTGV();
			}

			public Controller GetLastActiveController(ControllerType controllerType)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.kVMKJXGrpERpuLPyWEHnJICCwiPnA(controllerType);
			}

			public T GetLastActiveController<T>() where T : Controller
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.OzkDYWaprGZkxpnnegeOlsUIfTGV<T>();
			}

			public ControllerType GetLastActiveControllerType()
			{
				if (!CheckInitialized())
				{
					return ControllerType.Keyboard;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.uskUJcFSYKuEnjKcOhCfeTmggWLVA();
			}

			public void AddLastActiveControllerChangedDelegate(ActiveControllerChangedDelegate callback)
			{
				if (CheckInitialized())
				{
					FoarDfUMCtoVFquEtrllUhEjZUUn.gTuUmxdySLmMPcmDBQyoKkYuaScT(callback);
				}
			}

			public void AddLastActiveControllerChangedDelegate(ActiveControllerChangedDelegate callback, ControllerType controllerType)
			{
				if (CheckInitialized())
				{
					FoarDfUMCtoVFquEtrllUhEjZUUn.CaFYlnfgOBGxABBsFsaafYjjWmun(callback, controllerType);
				}
			}

			public void RemoveLastActiveControllerChangedDelegate(ActiveControllerChangedDelegate callback)
			{
				if (CheckInitialized())
				{
					FoarDfUMCtoVFquEtrllUhEjZUUn.XjVDXGzMkNqxprATLFFHesSkfjlg(callback);
				}
			}

			public void RemoveLastActiveControllerChangedDelegate(ActiveControllerChangedDelegate callback, ControllerType controllerType)
			{
				if (CheckInitialized())
				{
					FoarDfUMCtoVFquEtrllUhEjZUUn.IfjxMKpFVkAFazHMwlJvNCxlfiZU(callback, controllerType);
				}
			}

			public void ClearLastActiveControllerChangedDelegates()
			{
				if (CheckInitialized())
				{
					FoarDfUMCtoVFquEtrllUhEjZUUn.eEmPpgRThzKuPBYtJSKOZzzLpUpe();
				}
			}

			public bool GetAnyButton()
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.TLjbPoFCLLhzNTmYRGBcjxgqMOhkA();
			}

			public bool GetAnyButton(ControllerType controllerType)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.sVKvRLKlFDzkBvsqLuyhswblQmUy(controllerType);
			}

			public bool GetAnyButtonDown()
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.wXiZvBpmRjyecgotDrIryUpYwQOS();
			}

			public bool GetAnyButtonDown(ControllerType controllerType)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.vyPESacQRtUmMAbFZfKGXdzaquYr(controllerType);
			}

			public bool GetAnyButtonUp()
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.LiEOVesscDjMRDFNZBWJmysCuUXdA();
			}

			public bool GetAnyButtonUp(ControllerType controllerType)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.pDbFNUqNXfoiiwKvhYVNdsICejxd(controllerType);
			}

			public bool GetAnyButtonChanged()
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.QDgFRpBowsehhtyTlZrdUcsuBpfdb();
			}

			public bool GetAnyButtonChanged(ControllerType controllerType)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.NmCDtfhQJzNrgVEGzUvuPHKSwRvP(controllerType);
			}

			public bool GetAnyButtonPrev()
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.tLYadFJuQsHesxdaaUeLWVUAwbKpA();
			}

			public bool GetAnyButtonPrev(ControllerType controllerType)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.eAqZhGncDoHktPpvlezDGaYYEEMq(controllerType);
			}

			public bool AutoAssignJoystick(Joystick joystick)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				if (joystick == null)
				{
					return false;
				}
				if (IsJoystickAssigned(joystick))
				{
					return true;
				}
				KTVqyytqGISutLJQbfhSaKddBlfv.aENDJtIKdDBxcWAwSMmhnOuTbqKl(joystick);
				return IsJoystickAssigned(joystick);
			}

			public void AutoAssignJoysticks()
			{
				if (CheckInitialized())
				{
					int num = joystickCount;
					IList<Joystick> joysticks = Joysticks;
					for (int i = 0; i < num; i++)
					{
						AutoAssignJoystick(joysticks[i]);
					}
				}
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class MappingHelper : CodeHelper
		{
			private static MappingHelper FKMvzMsgCJxVqBHlylDDdLRfUVEB;

			internal static MappingHelper AVvLUjXeLvdrGrSsaHifwWnUOrNk => FKMvzMsgCJxVqBHlylDDdLRfUVEB ?? (FKMvzMsgCJxVqBHlylDDdLRfUVEB = new MappingHelper());

			public IList<InputMapCategory> MapCategories
			{
				get
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<InputMapCategory>.EmptyReadOnlyIListT;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.psnMotVTaWJqzAFWxfjoaAVNVSFMA;
				}
			}

			public IEnumerable<InputMapCategory> UserAssignableMapCategories
			{
				get
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<InputMapCategory>.EmptyReadOnlyIListT;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.rSknYcNUeqOIaSjQkNSDuOVKthIp;
				}
			}

			public IList<InputCategory> ActionCategories
			{
				get
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<InputCategory>.EmptyReadOnlyIListT;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.IMddRaBENAmGfZWTNpvTlDLkNkHU;
				}
			}

			public IEnumerable<InputCategory> UserAssignableActionCategories
			{
				get
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<InputCategory>.EmptyReadOnlyIListT;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.TYBFYKfZaqmdJxUpoyQWiXdebyxY;
				}
			}

			public IList<InputLayout> JoystickLayouts
			{
				get
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<InputLayout>.EmptyReadOnlyIListT;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.jRJbARDRHMLcZQovULcpACLAoSlYA;
				}
			}

			public IList<InputLayout> KeyboardLayouts
			{
				get
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<InputLayout>.EmptyReadOnlyIListT;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.DTuaINDJtEOtldDSZdZpfyTkmIIcA;
				}
			}

			public IList<InputLayout> MouseLayouts
			{
				get
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<InputLayout>.EmptyReadOnlyIListT;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.CYgKlfpQSrdTwHyRItikkHzdxbipA;
				}
			}

			public IList<InputLayout> CustomControllerLayouts
			{
				get
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<InputLayout>.EmptyReadOnlyIListT;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.XzpJaGfpJYXkiIKQPTSLavfAQgiO;
				}
			}

			public IList<InputAction> Actions
			{
				get
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
					}
					return stWBKWMOrAnxQyItwkKzVuulIRgF.oNXBuTCpMYRLGCcxGUYgjnxaGNBWA;
				}
			}

			public IEnumerable<InputAction> UserAssignableActions
			{
				get
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
					}
					return yFcxJYXlykxThQELuKTiwjJTSDUP.FRquySFZeZeRlsVwIqFntUDlCqSQ;
				}
			}

			private MappingHelper()
			{
			}

			public InputMapCategory GetMapCategory(int mapCategoryId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetMapCategoryById(mapCategoryId);
			}

			public InputMapCategory GetMapCategory(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetMapCategory(name);
			}

			public int GetMapCategoryId(string name)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetMapCategoryId(name);
			}

			public IEnumerable<InputMapCategory> MapCategoriesWithTag(string tag)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputMapCategory>.EmptyReadOnlyIListT;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.zTfTxSSvYAQwXhDNxuTubihtRbQV(tag);
			}

			public IEnumerable<InputMapCategory> UserAssignableMapCategoriesWithTag(string tag)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputMapCategory>.EmptyReadOnlyIListT;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.qetRckIAzdanwcZBHJQZsfJLaiEr(tag);
			}

			public bool IsMapCategoryUserAssignable(int mapCategoryId)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return GetMapCategory(mapCategoryId)?.userAssignable ?? false;
			}

			public InputCategory GetActionCategory(int mapCategoryId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetActionCategoryById(mapCategoryId);
			}

			public InputCategory GetActionCategory(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetActionCategory(name);
			}

			public int GetActionCategoryId(string name)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetActionCategoryId(name);
			}

			public IEnumerable<InputCategory> ActionCategoriesWithTag(string tag)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputCategory>.EmptyReadOnlyIListT;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.AmZSnNbIgDdsTBtJwVYxtOhCqjqWA(tag);
			}

			public IEnumerable<InputCategory> UserAssignableActionCategoriesWithTag(string tag)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputCategory>.EmptyReadOnlyIListT;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.mFiumTHjIeYSiWEaMUJPnTMDmsnW(tag);
			}

			public bool IsActionCategoryUserAssignable(int mapCategoryId)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return GetActionCategory(mapCategoryId)?.userAssignable ?? false;
			}

			public InputLayout GetLayout(ControllerType controllerType, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return controllerType switch
				{
					ControllerType.Joystick => yFcxJYXlykxThQELuKTiwjJTSDUP.GetJoystickLayoutById(layoutId), 
					ControllerType.Keyboard => yFcxJYXlykxThQELuKTiwjJTSDUP.GetKeyboardLayoutById(layoutId), 
					ControllerType.Mouse => yFcxJYXlykxThQELuKTiwjJTSDUP.GetMouseLayoutById(layoutId), 
					ControllerType.Custom => yFcxJYXlykxThQELuKTiwjJTSDUP.GetCustomControllerLayoutById(layoutId), 
					_ => throw new NotImplementedException(), 
				};
			}

			public InputLayout GetLayout(ControllerType controllerType, string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return controllerType switch
				{
					ControllerType.Joystick => yFcxJYXlykxThQELuKTiwjJTSDUP.GetJoystickLayout(name), 
					ControllerType.Keyboard => yFcxJYXlykxThQELuKTiwjJTSDUP.GetKeyboardLayout(name), 
					ControllerType.Mouse => yFcxJYXlykxThQELuKTiwjJTSDUP.GetMouseLayout(name), 
					ControllerType.Custom => yFcxJYXlykxThQELuKTiwjJTSDUP.GetCustomControllerLayout(name), 
					_ => throw new NotImplementedException(), 
				};
			}

			public int GetLayoutId(ControllerType controllerType, string name)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				return controllerType switch
				{
					ControllerType.Joystick => yFcxJYXlykxThQELuKTiwjJTSDUP.GetJoystickLayoutId(name), 
					ControllerType.Keyboard => yFcxJYXlykxThQELuKTiwjJTSDUP.GetKeyboardLayoutId(name), 
					ControllerType.Mouse => yFcxJYXlykxThQELuKTiwjJTSDUP.GetMouseLayoutId(name), 
					ControllerType.Custom => yFcxJYXlykxThQELuKTiwjJTSDUP.GetCustomControllerLayoutId(name), 
					_ => throw new NotImplementedException(), 
				};
			}

			public InputLayout GetJoystickLayout(int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetJoystickLayoutById(layoutId);
			}

			public InputLayout GetJoystickLayout(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetJoystickLayout(name);
			}

			public int GetJoystickLayoutId(string name)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetJoystickLayoutId(name);
			}

			public InputLayout GetKeyboardLayout(int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetKeyboardLayoutById(layoutId);
			}

			public InputLayout GetKeyboardLayout(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetKeyboardLayout(name);
			}

			public int GetKeyboardLayoutId(string name)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetKeyboardLayoutId(name);
			}

			public InputLayout GetMouseLayout(int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetMouseLayoutById(layoutId);
			}

			public InputLayout GetMouseLayout(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetMouseLayout(name);
			}

			public int GetMouseLayoutId(string name)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetMouseLayoutId(name);
			}

			public InputLayout GetCustomControllerLayout(int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetCustomControllerLayoutById(layoutId);
			}

			public InputLayout GetCustomControllerLayout(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetCustomControllerLayout(name);
			}

			public int GetCustomControllerLayoutId(string name)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetCustomControllerLayoutId(name);
			}

			public IList<InputLayout> MapLayouts(ControllerType controllerType)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputLayout>.EmptyReadOnlyIListT;
				}
				return controllerType switch
				{
					ControllerType.Joystick => JoystickLayouts, 
					ControllerType.Keyboard => KeyboardLayouts, 
					ControllerType.Mouse => MouseLayouts, 
					ControllerType.Custom => CustomControllerLayouts, 
					_ => throw new NotImplementedException(), 
				};
			}

			public InputAction GetAction(int actionId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetActionById(actionId);
			}

			public InputAction GetAction(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetAction(name);
			}

			public int GetActionId(string name)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetActionId(name);
			}

			public IEnumerable<InputAction> ActionsInCategory(string mapCategoryName)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.CMcipwivHVqDObFtqItXsOoadtMc(mapCategoryName, false);
			}

			public IEnumerable<InputAction> ActionsInCategory(string mapCategoryName, bool sort)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.CMcipwivHVqDObFtqItXsOoadtMc(mapCategoryName, sort);
			}

			public IEnumerable<InputAction> ActionsInCategory(int mapCategoryId)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.sQgquvUbVxkrQbRmtQrCUAPgmcBS(mapCategoryId, false);
			}

			public IEnumerable<InputAction> ActionsInCategory(int mapCategoryId, bool sort)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.sQgquvUbVxkrQbRmtQrCUAPgmcBS(mapCategoryId, sort);
			}

			public IEnumerable<InputAction> ActionsInCategoriesWithTag(string tag)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.zGCzlHzTlymQZPvFVEcaevqYHEjW(tag);
			}

			public IEnumerable<InputAction> UserAssignableActionsInCategory(int mapCategoryId)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.XZATmGlfJcOggZqyTEyTzCpkGCDbA(mapCategoryId, false);
			}

			public IEnumerable<InputAction> UserAssignableActionsInCategory(int mapCategoryId, bool sort)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.XZATmGlfJcOggZqyTEyTzCpkGCDbA(mapCategoryId, sort);
			}

			public IEnumerable<InputAction> UserAssignableActionsInCategory(string mapCategoryName)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.aWEziYZsMPytfEZDKeLsozQMNMUA(mapCategoryName, false);
			}

			public IEnumerable<InputAction> UserAssignableActionsInCategory(string mapCategoryName, bool sort)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.aWEziYZsMPytfEZDKeLsozQMNMUA(mapCategoryName, sort);
			}

			public IList<InputBehavior> GetInputBehaviors(int playerId)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputBehavior>.EmptyReadOnlyIListT;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.UkGZIfhaCReBibQxxKNQcxuisNEb(playerId);
			}

			public IList<InputBehavior> GetSystemPlayerInputBehaviors()
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputBehavior>.EmptyReadOnlyIListT;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.UkGZIfhaCReBibQxxKNQcxuisNEb(9999999);
			}

			public InputBehavior GetInputBehavior(int playerId, int behaviorId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.AUVupqHUEheIGgrZDnacEevMQjxL(playerId, behaviorId);
			}

			public InputBehavior GetInputBehavior(int playerId, string behaviorName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return FoarDfUMCtoVFquEtrllUhEjZUUn.LAjEtOLXxuEGCJYVpCdAEIAbisdo(playerId, behaviorName);
			}

			public InputBehavior GetSystemPlayerInputBehavior(int behaviorId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return GetInputBehavior(9999999, behaviorId);
			}

			public InputBehavior GetSystemPlayerInputBehavior(string behaviorName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return GetInputBehavior(9999999, behaviorName);
			}

			public int GetInputBehaviorId(string behaviorName)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetInputBehaviorId(behaviorName);
			}

			internal InputBehavior QiaJTMgOusFPahTLgdtJdHIxhEGo(int P_0)
			{
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetInputBehaviorById(P_0);
			}

			internal InputBehavior NJrjePNqkDFCpGkllEQBbfEFVZaX(string P_0)
			{
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetInputBehavior(P_0);
			}

			public ControllerMap GetControllerMap(int id)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				IList<Player> allPlayers = players.AllPlayers;
				for (int i = 0; i < allPlayers.Count; i++)
				{
					ControllerMap map = allPlayers[i].controllers.maps.GetMap(id);
					if (map != null)
					{
						return map;
					}
				}
				return null;
			}

			public ActionElementMap GetActionElementMap(int id)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				IList<Player> allPlayers = players.AllPlayers;
				for (int i = 0; i < allPlayers.Count; i++)
				{
					foreach (ControllerMap allMap in allPlayers[i].controllers.maps.GetAllMaps())
					{
						if (allMap != null)
						{
							ActionElementMap elementMap = allMap.GetElementMap(id);
							if (elementMap != null)
							{
								return elementMap;
							}
						}
					}
				}
				return null;
			}

			public ControllerMap GetControllerMapInstance(Controller controller, int mapCategoryId, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				if (controller == null)
				{
					return null;
				}
				return controller.type switch
				{
					ControllerType.Joystick => GetJoystickMapInstance((Joystick)controller, mapCategoryId, layoutId), 
					ControllerType.Keyboard => GetKeyboardMapInstance(mapCategoryId, layoutId), 
					ControllerType.Mouse => GetMouseMapInstance(mapCategoryId, layoutId), 
					ControllerType.Custom => GetCustomControllerMapInstance((CustomController)controller, mapCategoryId, layoutId), 
					_ => throw new NotImplementedException(), 
				};
			}

			public ControllerMap GetControllerMapInstance(Controller controller, string mapCategoryName, string layoutName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				if (controller == null)
				{
					return null;
				}
				int mapCategoryId = GetMapCategoryId(mapCategoryName);
				if (mapCategoryId < 0)
				{
					return null;
				}
				int layoutId = GetLayoutId(controller.type, layoutName);
				if (layoutId < 0)
				{
					return null;
				}
				return GetControllerMapInstance(controller, mapCategoryId, layoutId);
			}

			public ControllerMap GetControllerMapInstance(ControllerIdentifier controllerIdentifier, string mapCategoryName, string layoutName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int mapCategoryId = GetMapCategoryId(mapCategoryName);
				if (mapCategoryId < 0)
				{
					return null;
				}
				int layoutId = GetLayoutId(controllerIdentifier.controllerType, layoutName);
				if (layoutId < 0)
				{
					return null;
				}
				return GetControllerMapInstance(controllerIdentifier, mapCategoryId, layoutId);
			}

			public ControllerMap GetControllerMapInstance(ControllerIdentifier controllerIdentifier, int mapCategoryId, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				Controller controller = FoarDfUMCtoVFquEtrllUhEjZUUn.UpNCLkybyrfyzcgOPVjvhyzXROTtA(controllerIdentifier);
				if (controller != null)
				{
					return GetControllerMapInstance(controller, mapCategoryId, layoutId);
				}
				return controllerIdentifier.controllerType switch
				{
					ControllerType.Joystick => GetJoystickMapInstance(controllerIdentifier, mapCategoryId, layoutId), 
					ControllerType.Custom => GetCustomControllerMapInstance(controllerIdentifier, mapCategoryId, layoutId), 
					ControllerType.Keyboard => GetKeyboardMapInstance(mapCategoryId, layoutId), 
					ControllerType.Mouse => GetMouseMapInstance(mapCategoryId, layoutId), 
					_ => throw new NotImplementedException(), 
				};
			}

			public JoystickMap GetJoystickMapInstance(Joystick joystick, int mapCategoryId, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				if (joystick == null)
				{
					return null;
				}
				JoystickMap joystickMap = yFcxJYXlykxThQELuKTiwjJTSDUP.vAHiNYrtViGADjHOxDfcaDShypcZb(joystick, mapCategoryId, layoutId);
				if (joystickMap != null)
				{
					joystick.FGOWYFxDhAlAiuIOIJEWlURoAViy(joystickMap);
				}
				return joystickMap;
			}

			public JoystickMap GetJoystickMapInstance(Joystick joystick, string mapCategoryName, string layoutName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int mapCategoryId = GetMapCategoryId(mapCategoryName);
				if (mapCategoryId < 0)
				{
					return null;
				}
				int layoutId = GetLayoutId(ControllerType.Joystick, layoutName);
				if (layoutId < 0)
				{
					return null;
				}
				return GetJoystickMapInstance(joystick, mapCategoryId, layoutId);
			}

			public JoystickMap GetJoystickMapInstance(Guid joystickTypeGuid, int mapCategoryId, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				if (joystickTypeGuid == Guid.Empty)
				{
					return null;
				}
				InputSource inputSourceType = zNGjjScruOFRrdIdVfVzDkdgujPhA.inputSourceType;
				HardwareJoystickMap_InputManager hardwareJoystickMap_InputManager = LWRgoiKgeHjHOiswhfZqkdDTTXODA.jwexFdVRDBYWblQfnPvIjvwdoUjW(joystickTypeGuid, inputSourceType);
				if (hardwareJoystickMap_InputManager == null)
				{
					Logger.LogError("No hardware map found.");
					return null;
				}
				JoystickMap joystickMap = yFcxJYXlykxThQELuKTiwjJTSDUP.XmgsLQKXkLUGIsMVxjQlJrvzzSFL(hardwareJoystickMap_InputManager.hardwareMapIdentifier, mapCategoryId, layoutId);
				if (joystickMap != null)
				{
					HardwareControllerMap_Game hardwareControllerMap_Game = hardwareJoystickMap_InputManager.ToGameHardwareControllerMap();
					foreach (ActionElementMap allMap in joystickMap.AllMaps)
					{
						allMap.sahAvUfMOSKesGrXhGKAeDGCZnIJc(joystickMap, hardwareControllerMap_Game);
					}
				}
				return joystickMap;
			}

			public JoystickMap GetJoystickMapInstance(Guid joystickTypeGuid, string mapCategoryName, string layoutName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				if (joystickTypeGuid == Guid.Empty)
				{
					return null;
				}
				int mapCategoryId = GetMapCategoryId(mapCategoryName);
				if (mapCategoryId < 0)
				{
					return null;
				}
				int layoutId = GetLayoutId(ControllerType.Joystick, layoutName);
				if (layoutId < 0)
				{
					return null;
				}
				return GetJoystickMapInstance(joystickTypeGuid, mapCategoryId, layoutId);
			}

			public JoystickMap GetJoystickMapInstance(ControllerIdentifier controllerIdentifier, int mapCategoryId, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				if (controllerIdentifier.controllerType != ControllerType.Joystick)
				{
					return null;
				}
				if (FoarDfUMCtoVFquEtrllUhEjZUUn.UpNCLkybyrfyzcgOPVjvhyzXROTtA(controllerIdentifier) is Joystick joystick)
				{
					return GetJoystickMapInstance(joystick, mapCategoryId, layoutId);
				}
				return GetJoystickMapInstance(controllerIdentifier.hardwareTypeGuid, mapCategoryId, layoutId);
			}

			public JoystickMap GetJoystickMapInstance(ControllerIdentifier controllerIdentifier, string mapCategoryName, string layoutName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int mapCategoryId = GetMapCategoryId(mapCategoryName);
				if (mapCategoryId < 0)
				{
					return null;
				}
				int layoutId = GetLayoutId(ControllerType.Joystick, layoutName);
				if (layoutId < 0)
				{
					return null;
				}
				return GetJoystickMapInstance(controllerIdentifier, mapCategoryId, layoutId);
			}

			public KeyboardMap GetKeyboardMapInstance(int mapCategoryId, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				KeyboardMap keyboardMap = yFcxJYXlykxThQELuKTiwjJTSDUP.FindKeyboardMap_Game(controllers.Keyboard, mapCategoryId, layoutId);
				if (keyboardMap != null)
				{
					controllers.Keyboard.FGOWYFxDhAlAiuIOIJEWlURoAViy(keyboardMap);
				}
				return keyboardMap;
			}

			public KeyboardMap GetKeyboardMapInstance(string mapCategoryName, string layoutName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int mapCategoryId = GetMapCategoryId(mapCategoryName);
				if (mapCategoryId < 0)
				{
					return null;
				}
				int layoutId = GetLayoutId(ControllerType.Keyboard, layoutName);
				if (layoutId < 0)
				{
					return null;
				}
				return GetKeyboardMapInstance(mapCategoryId, layoutId);
			}

			public MouseMap GetMouseMapInstance(int mapCategoryId, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				MouseMap mouseMap = yFcxJYXlykxThQELuKTiwjJTSDUP.FindMouseMap_Game(controllers.Mouse, mapCategoryId, layoutId);
				if (mouseMap != null)
				{
					controllers.Mouse.FGOWYFxDhAlAiuIOIJEWlURoAViy(mouseMap);
				}
				return mouseMap;
			}

			public MouseMap GetMouseMapInstance(string mapCategoryName, string layoutName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int mapCategoryId = GetMapCategoryId(mapCategoryName);
				if (mapCategoryId < 0)
				{
					return null;
				}
				int layoutId = GetLayoutId(ControllerType.Mouse, layoutName);
				if (layoutId < 0)
				{
					return null;
				}
				return GetMouseMapInstance(mapCategoryId, layoutId);
			}

			public CustomControllerMap GetCustomControllerMapInstance(CustomController customController, int mapCategoryId, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				CustomControllerMap customControllerMap = yFcxJYXlykxThQELuKTiwjJTSDUP.MNKkjYicKUjWIvlytoqoHPNWWAfD(customController.sourceControllerId, mapCategoryId, layoutId);
				if (customControllerMap != null)
				{
					customController.FGOWYFxDhAlAiuIOIJEWlURoAViy(customControllerMap);
				}
				return customControllerMap;
			}

			public CustomControllerMap GetCustomControllerMapInstance(CustomController customController, string mapCategoryName, string layoutName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int mapCategoryId = GetMapCategoryId(mapCategoryName);
				if (mapCategoryId < 0)
				{
					return null;
				}
				int layoutId = GetLayoutId(ControllerType.Custom, layoutName);
				if (layoutId < 0)
				{
					return null;
				}
				return GetCustomControllerMapInstance(customController, mapCategoryId, layoutId);
			}

			public CustomControllerMap GetCustomControllerMapInstance(ControllerIdentifier controllerIdentifier, int mapCategoryId, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				if (controllerIdentifier.controllerType != ControllerType.Custom)
				{
					return null;
				}
				if (FoarDfUMCtoVFquEtrllUhEjZUUn.UpNCLkybyrfyzcgOPVjvhyzXROTtA(controllerIdentifier) is CustomController customController)
				{
					return GetCustomControllerMapInstance(customController, mapCategoryId, layoutId);
				}
				if (controllerIdentifier.hardwareTypeGuid == Guid.Empty)
				{
					return null;
				}
				CustomController_Editor customControllerByHardwareTypeGuid = yFcxJYXlykxThQELuKTiwjJTSDUP.GetCustomControllerByHardwareTypeGuid(controllerIdentifier.hardwareTypeGuid);
				if (customControllerByHardwareTypeGuid == null)
				{
					return null;
				}
				CustomControllerMap customControllerMap = yFcxJYXlykxThQELuKTiwjJTSDUP.BrARhyONeKwqXgnVlAvKnHFNFZrI(controllerIdentifier.hardwareTypeGuid, mapCategoryId, layoutId);
				if (customControllerMap != null)
				{
					HardwareControllerMap_Game hardwareControllerMap_Game = customControllerByHardwareTypeGuid.CreateGameHardwareMap();
					if (hardwareControllerMap_Game == null)
					{
						Logger.LogError("No hardware map found.");
						return null;
					}
					foreach (ActionElementMap allMap in customControllerMap.AllMaps)
					{
						allMap.sahAvUfMOSKesGrXhGKAeDGCZnIJc(customControllerMap, hardwareControllerMap_Game);
					}
				}
				return customControllerMap;
			}

			public CustomControllerMap GetCustomControllerMapInstance(ControllerIdentifier controllerIdentifier, string mapCategoryName, string layoutName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int mapCategoryId = GetMapCategoryId(mapCategoryName);
				if (mapCategoryId < 0)
				{
					return null;
				}
				int layoutId = GetLayoutId(ControllerType.Custom, layoutName);
				if (layoutId < 0)
				{
					return null;
				}
				return GetCustomControllerMapInstance(controllerIdentifier, mapCategoryId, layoutId);
			}

			public ControllerMap GetControllerMapInstanceSavedOrDefault(int playerId, Controller controller, int mapCategoryId, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				if (controller == null)
				{
					return null;
				}
				ControllerMap controllerMap = null;
				if (userDataStore is IControllerMapStore controllerMapStore)
				{
					controllerMap = controllerMapStore.LoadControllerMap(playerId, controller.identifier, mapCategoryId, layoutId);
				}
				if (controllerMap == null)
				{
					controllerMap = yFcxJYXlykxThQELuKTiwjJTSDUP.mlHpGZlwlGuuSPQRGehERBHqzRhy(controller, mapCategoryId, layoutId);
				}
				if (controllerMap != null)
				{
					Player player = players.GetPlayer(playerId);
					if (player != null)
					{
						player.controllers.maps.NnhFHdcvDdZZhDqoCXXywvEaJxwS(controller, controllerMap);
					}
					else
					{
						controller.FGOWYFxDhAlAiuIOIJEWlURoAViy(controllerMap);
					}
				}
				return controllerMap;
			}

			public ControllerMap GetControllerMapInstanceSavedOrDefault(int playerId, Controller controller, string mapCategoryName, string layoutName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				if (controller == null)
				{
					return null;
				}
				int mapCategoryId = GetMapCategoryId(mapCategoryName);
				if (mapCategoryId < 0)
				{
					return null;
				}
				int layoutId = GetLayoutId(controller.type, layoutName);
				if (layoutId < 0)
				{
					return null;
				}
				return GetControllerMapInstanceSavedOrDefault(playerId, controller, mapCategoryId, layoutId);
			}

			public ControllerMap GetControllerMapInstanceSavedOrDefault(int playerId, ControllerIdentifier controllerIdentifier, int mapCategoryId, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return controllerIdentifier.controllerType switch
				{
					ControllerType.Joystick => GetJoystickMapInstanceSavedOrDefault(playerId, controllerIdentifier, mapCategoryId, layoutId), 
					ControllerType.Custom => GetCustomControllerMapInstanceSavedOrDefault(playerId, controllerIdentifier, mapCategoryId, layoutId), 
					ControllerType.Keyboard => GetKeyboardMapInstanceSavedOrDefault(playerId, mapCategoryId, layoutId), 
					ControllerType.Mouse => GetMouseMapInstanceSavedOrDefault(playerId, mapCategoryId, layoutId), 
					_ => throw new NotImplementedException(), 
				};
			}

			public ControllerMap GetControllerMapInstanceSavedOrDefault(int playerId, ControllerIdentifier controllerIdentifier, string mapCategoryName, string layoutName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int mapCategoryId = GetMapCategoryId(mapCategoryName);
				if (mapCategoryId < 0)
				{
					return null;
				}
				int layoutId = GetLayoutId(controllerIdentifier.controllerType, layoutName);
				if (layoutId < 0)
				{
					return null;
				}
				return GetControllerMapInstanceSavedOrDefault(playerId, controllerIdentifier, mapCategoryId, layoutId);
			}

			public JoystickMap GetJoystickMapInstanceSavedOrDefault(int playerId, Joystick joystick, int mapCategoryId, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return GetControllerMapInstanceSavedOrDefault(playerId, joystick, mapCategoryId, layoutId) as JoystickMap;
			}

			public JoystickMap GetJoystickMapInstanceSavedOrDefault(int playerId, Joystick joystick, string mapCategoryName, string layoutName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int mapCategoryId = GetMapCategoryId(mapCategoryName);
				if (mapCategoryId < 0)
				{
					return null;
				}
				int layoutId = GetLayoutId(ControllerType.Joystick, layoutName);
				if (layoutId < 0)
				{
					return null;
				}
				return GetJoystickMapInstanceSavedOrDefault(playerId, joystick, mapCategoryId, layoutId);
			}

			public JoystickMap GetJoystickMapInstanceSavedOrDefault(int playerId, ControllerIdentifier controllerIdentifier, int mapCategoryId, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				if (FoarDfUMCtoVFquEtrllUhEjZUUn.UpNCLkybyrfyzcgOPVjvhyzXROTtA(controllerIdentifier) is Joystick joystick)
				{
					return GetJoystickMapInstanceSavedOrDefault(playerId, joystick, mapCategoryId, layoutId);
				}
				InputSource inputSourceType = zNGjjScruOFRrdIdVfVzDkdgujPhA.inputSourceType;
				HardwareJoystickMap_InputManager hardwareJoystickMap_InputManager = LWRgoiKgeHjHOiswhfZqkdDTTXODA.jwexFdVRDBYWblQfnPvIjvwdoUjW(controllerIdentifier.hardwareTypeGuid, inputSourceType);
				if (hardwareJoystickMap_InputManager == null)
				{
					Logger.LogError("No hardware map found.");
					return null;
				}
				JoystickMap joystickMap = null;
				if (userDataStore is IControllerMapStore controllerMapStore)
				{
					joystickMap = controllerMapStore.LoadControllerMap(playerId, controllerIdentifier, mapCategoryId, layoutId) as JoystickMap;
				}
				if (joystickMap == null)
				{
					joystickMap = yFcxJYXlykxThQELuKTiwjJTSDUP.XmgsLQKXkLUGIsMVxjQlJrvzzSFL(hardwareJoystickMap_InputManager.hardwareMapIdentifier, mapCategoryId, layoutId);
				}
				if (joystickMap != null)
				{
					if (players.GetPlayer(playerId) != null)
					{
						joystickMap.playerId = playerId;
					}
					HardwareControllerMap_Game hardwareControllerMap_Game = hardwareJoystickMap_InputManager.ToGameHardwareControllerMap();
					foreach (ActionElementMap allMap in joystickMap.AllMaps)
					{
						allMap.sahAvUfMOSKesGrXhGKAeDGCZnIJc(joystickMap, hardwareControllerMap_Game);
					}
				}
				return joystickMap;
			}

			public JoystickMap GetJoystickMapInstanceSavedOrDefault(int playerId, ControllerIdentifier controllerIdentifier, string mapCategoryName, string layoutName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int mapCategoryId = GetMapCategoryId(mapCategoryName);
				if (mapCategoryId < 0)
				{
					return null;
				}
				int layoutId = GetLayoutId(ControllerType.Joystick, layoutName);
				if (layoutId < 0)
				{
					return null;
				}
				return GetJoystickMapInstanceSavedOrDefault(playerId, controllerIdentifier, mapCategoryId, layoutId);
			}

			public CustomControllerMap GetCustomControllerMapInstanceSavedOrDefault(int playerId, CustomController customController, int mapCategoryId, int layoutId)
			{
				return GetControllerMapInstanceSavedOrDefault(playerId, customController, mapCategoryId, layoutId) as CustomControllerMap;
			}

			public CustomControllerMap GetCustomControllerMapInstanceSavedOrDefault(int playerId, CustomController customController, string mapCategoryName, string layoutName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int mapCategoryId = GetMapCategoryId(mapCategoryName);
				if (mapCategoryId < 0)
				{
					return null;
				}
				int layoutId = GetLayoutId(ControllerType.Custom, layoutName);
				if (layoutId < 0)
				{
					return null;
				}
				return GetCustomControllerMapInstanceSavedOrDefault(playerId, customController, mapCategoryId, layoutId);
			}

			public CustomControllerMap GetCustomControllerMapInstanceSavedOrDefault(int playerId, ControllerIdentifier controllerIdentifier, int mapCategoryId, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				if (FoarDfUMCtoVFquEtrllUhEjZUUn.UpNCLkybyrfyzcgOPVjvhyzXROTtA(controllerIdentifier) is CustomController customController)
				{
					return GetCustomControllerMapInstanceSavedOrDefault(playerId, customController, mapCategoryId, layoutId);
				}
				CustomController_Editor customControllerByHardwareTypeGuid = yFcxJYXlykxThQELuKTiwjJTSDUP.GetCustomControllerByHardwareTypeGuid(controllerIdentifier.hardwareTypeGuid);
				if (customControllerByHardwareTypeGuid == null)
				{
					return null;
				}
				CustomControllerMap customControllerMap = null;
				if (userDataStore is IControllerMapStore controllerMapStore)
				{
					customControllerMap = controllerMapStore.LoadControllerMap(playerId, controllerIdentifier, mapCategoryId, layoutId) as CustomControllerMap;
				}
				if (customControllerMap == null)
				{
					customControllerMap = yFcxJYXlykxThQELuKTiwjJTSDUP.BrARhyONeKwqXgnVlAvKnHFNFZrI(controllerIdentifier.hardwareTypeGuid, mapCategoryId, layoutId);
				}
				if (customControllerMap != null)
				{
					HardwareControllerMap_Game hardwareControllerMap_Game = customControllerByHardwareTypeGuid.CreateGameHardwareMap();
					if (hardwareControllerMap_Game == null)
					{
						Logger.LogError("No hardware map found.");
						return null;
					}
					if (players.GetPlayer(playerId) != null)
					{
						customControllerMap.playerId = playerId;
					}
					foreach (ActionElementMap allMap in customControllerMap.AllMaps)
					{
						allMap.sahAvUfMOSKesGrXhGKAeDGCZnIJc(customControllerMap, hardwareControllerMap_Game);
					}
				}
				return customControllerMap;
			}

			public CustomControllerMap GetCustomControllerMapInstanceSavedOrDefault(int playerId, ControllerIdentifier controllerIdentifier, string mapCategoryName, string layoutName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int mapCategoryId = GetMapCategoryId(mapCategoryName);
				if (mapCategoryId < 0)
				{
					return null;
				}
				int layoutId = GetLayoutId(ControllerType.Custom, layoutName);
				if (layoutId < 0)
				{
					return null;
				}
				return GetCustomControllerMapInstanceSavedOrDefault(playerId, controllerIdentifier, mapCategoryId, layoutId);
			}

			public KeyboardMap GetKeyboardMapInstanceSavedOrDefault(int playerId, int mapCategoryId, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				Controller keyboard = controllers.Keyboard;
				KeyboardMap keyboardMap = null;
				if (userDataStore is IControllerMapStore controllerMapStore)
				{
					keyboardMap = controllerMapStore.LoadControllerMap(playerId, keyboard.identifier, mapCategoryId, layoutId) as KeyboardMap;
				}
				if (keyboardMap == null)
				{
					keyboardMap = yFcxJYXlykxThQELuKTiwjJTSDUP.FindKeyboardMap_Game(controllers.Keyboard, mapCategoryId, layoutId);
				}
				if (keyboardMap != null)
				{
					Player player = players.GetPlayer(playerId);
					if (player != null)
					{
						player.controllers.maps.NnhFHdcvDdZZhDqoCXXywvEaJxwS(keyboard, keyboardMap);
					}
					else
					{
						keyboard.FGOWYFxDhAlAiuIOIJEWlURoAViy(keyboardMap);
					}
				}
				return keyboardMap;
			}

			public KeyboardMap GetKeyboardMapInstanceSavedOrDefault(int playerId, string mapCategoryName, string layoutName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int mapCategoryId = GetMapCategoryId(mapCategoryName);
				if (mapCategoryId < 0)
				{
					return null;
				}
				int layoutId = GetLayoutId(ControllerType.Keyboard, layoutName);
				if (layoutId < 0)
				{
					return null;
				}
				return GetKeyboardMapInstanceSavedOrDefault(playerId, mapCategoryId, layoutId);
			}

			public MouseMap GetMouseMapInstanceSavedOrDefault(int playerId, int mapCategoryId, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				Controller mouse = controllers.Mouse;
				MouseMap mouseMap = null;
				if (userDataStore is IControllerMapStore controllerMapStore)
				{
					mouseMap = controllerMapStore.LoadControllerMap(playerId, mouse.identifier, mapCategoryId, layoutId) as MouseMap;
				}
				if (mouseMap == null)
				{
					mouseMap = yFcxJYXlykxThQELuKTiwjJTSDUP.FindMouseMap_Game(controllers.Mouse, mapCategoryId, layoutId);
				}
				if (mouseMap != null)
				{
					Player player = players.GetPlayer(playerId);
					if (player != null)
					{
						player.controllers.maps.NnhFHdcvDdZZhDqoCXXywvEaJxwS(mouse, mouseMap);
					}
					else
					{
						mouse.FGOWYFxDhAlAiuIOIJEWlURoAViy(mouseMap);
					}
				}
				return mouseMap;
			}

			public MouseMap GetMouseMapInstanceSavedOrDefault(int playerId, string mapCategoryName, string layoutName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int mapCategoryId = GetMapCategoryId(mapCategoryName);
				if (mapCategoryId < 0)
				{
					return null;
				}
				int layoutId = GetLayoutId(ControllerType.Mouse, layoutName);
				if (layoutId < 0)
				{
					return null;
				}
				return GetMouseMapInstanceSavedOrDefault(playerId, mapCategoryId, layoutId);
			}

			[Obsolete("This method has been deprecated. Use the Controller Template system instead.", false)]
			public ControllerElementIdentifier GetFirstJoystickTemplateElementIdentifier(Joystick joystick, int joystickElementIdentifierId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				if (joystick == null)
				{
					return null;
				}
				return wdGietbvPphnKyRuTHGFGcDBWvgOA(joystick.hardwareTypeGuid, joystickElementIdentifierId);
			}

			private ControllerElementIdentifier wdGietbvPphnKyRuTHGFGcDBWvgOA(Guid P_0, int P_1)
			{
				HardwareJoystickMap hardwareControllerMap;
				return LWRgoiKgeHjHOiswhfZqkdDTTXODA.fBUqoADVkvleUsHccPxptkUXrJul(P_0, P_1, out hardwareControllerMap)?.ToControllerElementIdentifier(hardwareControllerMap);
			}

			internal int YpODnAqLZPqAbLeGSTAMeqcCIwgC(Guid P_0, Guid P_1, int P_2, List<HardwareControllerTemplateMap.jiuvLTxcDzWSddWpdRJbLPDWISiIA> P_3)
			{
				return LWRgoiKgeHjHOiswhfZqkdDTTXODA.xjEHAWWCpwfeVfmHzFfAOtRyVEbaA(P_0, P_1, P_2, P_3);
			}

			public ControllerTemplateMap GetControllerTemplateMapInstance(Guid templateTypeGuid, int mapCategoryId, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.FDriMYNjLvRFqOeANQkVBNghaPBCA(templateTypeGuid, mapCategoryId, layoutId);
			}

			public ControllerTemplateMap GetControllerTemplateMapInstance(Guid templateTypeGuid, string mapCategoryName, string layoutName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int mapCategoryId = GetMapCategoryId(mapCategoryName);
				if (mapCategoryId < 0)
				{
					return null;
				}
				int layoutId = GetLayoutId(ControllerType.Custom, layoutName);
				if (layoutId < 0)
				{
					return null;
				}
				return GetControllerTemplateMapInstance(templateTypeGuid, mapCategoryId, layoutId);
			}

			public ControllerMapLayoutManager.RuleSet GetControllerMapLayoutManagerRuleSetInstance(int id)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetControllerMapLayoutManagerRuleSetById(id)?.ToRuntime();
			}

			public ControllerMapLayoutManager.RuleSet GetControllerMapLayoutManagerRuleSetInstance(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int controllerMapLayoutManagerRuleSetId = yFcxJYXlykxThQELuKTiwjJTSDUP.GetControllerMapLayoutManagerRuleSetId(name);
				if (controllerMapLayoutManagerRuleSetId < 0)
				{
					return null;
				}
				return GetControllerMapLayoutManagerRuleSetInstance(controllerMapLayoutManagerRuleSetId);
			}

			public ControllerMapEnabler.RuleSet GetControllerMapEnablerRuleSetInstance(int id)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return yFcxJYXlykxThQELuKTiwjJTSDUP.GetControllerMapEnablerRuleSetById(id)?.ToRuntime();
			}

			public ControllerMapEnabler.RuleSet GetControllerMapEnablerRuleSetInstance(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int controllerMapEnablerRuleSetId = yFcxJYXlykxThQELuKTiwjJTSDUP.GetControllerMapEnablerRuleSetId(name);
				if (controllerMapEnablerRuleSetId < 0)
				{
					return null;
				}
				return GetControllerMapEnablerRuleSetInstance(controllerMapEnablerRuleSetId);
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class PlayerHelper : CodeHelper
		{
			private static PlayerHelper jyWOlbMYKLzfSPLIqlWEWElHRSJh;

			internal static PlayerHelper fqxEmVsThfKpPgrmumetLSEjqcFq => jyWOlbMYKLzfSPLIqlWEWElHRSJh ?? (jyWOlbMYKLzfSPLIqlWEWElHRSJh = new PlayerHelper());

			public int playerCount
			{
				get
				{
					if (!CheckInitialized())
					{
						return 0;
					}
					return KTVqyytqGISutLJQbfhSaKddBlfv.VeldubbvOvGpfoCqfSOeOBOPVmbpA;
				}
			}

			public int allPlayerCount
			{
				get
				{
					if (!CheckInitialized())
					{
						return 0;
					}
					return KTVqyytqGISutLJQbfhSaKddBlfv.dTPqHqcLdarzLjRMtjGyxsDcgqoq;
				}
			}

			public IList<Player> Players
			{
				get
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<Player>.EmptyReadOnlyIListT;
					}
					return KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA;
				}
			}

			public IList<Player> AllPlayers
			{
				get
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<Player>.EmptyReadOnlyIListT;
					}
					return KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh;
				}
			}

			public Player SystemPlayer
			{
				get
				{
					if (!CheckInitialized())
					{
						return null;
					}
					return KTVqyytqGISutLJQbfhSaKddBlfv.KSOsWtiVPgllQavrnvGgcPSdahjn();
				}
			}

			private PlayerHelper()
			{
			}

			public IList<Player> GetPlayers(bool includeSystemPlayer = false)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<Player>.EmptyReadOnlyIListT;
				}
				if (!includeSystemPlayer)
				{
					return KTVqyytqGISutLJQbfhSaKddBlfv.rBBrAfxShCBQlDmrrfnkdCxzHJalA;
				}
				return KTVqyytqGISutLJQbfhSaKddBlfv.QxMAcoPmpKUqxnkzdWuGJnRqUkqh;
			}

			public Player GetPlayer(int playerId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return KTVqyytqGISutLJQbfhSaKddBlfv.LcqJOYavcMfniFdJGbmbfGcBCdPHA(playerId);
			}

			public Player GetPlayer(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return KTVqyytqGISutLJQbfhSaKddBlfv.vVgeiNMSEqqjIpQPOUSSbXqKMJKi(name);
			}

			public Player GetSystemPlayer()
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return KTVqyytqGISutLJQbfhSaKddBlfv.KSOsWtiVPgllQavrnvGgcPSdahjn();
			}

			public int GetPlayerId(string playerName)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				return KTVqyytqGISutLJQbfhSaKddBlfv.ETcCPPDiAgogRIOlbLeCrfltHOeic(playerName);
			}

			public string[] GetPlayerNames(bool includeSystemPlayer = false)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<string>.array;
				}
				return KTVqyytqGISutLJQbfhSaKddBlfv.AiwSECrvmluRBIukiFAqNNkGiatcA(includeSystemPlayer);
			}

			public string[] GetPlayerDescriptiveNames(bool includeSystemPlayer = false)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<string>.array;
				}
				return KTVqyytqGISutLJQbfhSaKddBlfv.lzghKbuCDVimnIqvQdasXbPaLcryA(includeSystemPlayer);
			}

			public int[] GetPlayerIds(bool includeSystemPlayer = false)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<int>.array;
				}
				return KTVqyytqGISutLJQbfhSaKddBlfv.VSZNYLBMdwJKnwACpAGwapcLMJUpA(includeSystemPlayer);
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class TimeHelper : CodeHelper
		{
			private static TimeHelper IKNEShBwdnNJMNgFsHjzCHIjRMxgB;

			internal static TimeHelper kBdUKIBPLXUuMWRaCNEeIsUeGcYgA => IKNEShBwdnNJMNgFsHjzCHIjRMxgB ?? (IKNEShBwdnNJMNgFsHjzCHIjRMxgB = new TimeHelper());

			public float unscaledDeltaTime
			{
				get
				{
					if (!CheckInitialized())
					{
						return 0f;
					}
					return (float)hSvDRPkauaeZKJDlzLyieMTBASFlB.SbfTzKDPhUTEhwGbCicbDvkluizAA;
				}
			}

			public double unscaledTime
			{
				get
				{
					if (!CheckInitialized())
					{
						return 0.0;
					}
					return hSvDRPkauaeZKJDlzLyieMTBASFlB.HPcRKhKOhMjfqdvqHOdDbiVZdMenA;
				}
			}

			public uint currentFrame
			{
				get
				{
					if (!CheckInitialized())
					{
						return 0u;
					}
					return hSvDRPkauaeZKJDlzLyieMTBASFlB.AIdOHeFZjBFhHWGDroEBzshSgDWFA;
				}
			}

			private TimeHelper()
			{
			}
		}

		private class LDrchKIStuCJQDHfATgwEZaUZcaSB
		{
			private class IyqTTFLMGvqgrEvkFClQzqdEeGoF
			{
				public readonly UpdateLoopType DFDJuhCnjIIWHhfaPwzkLWvhddac;

				private double AvQOkiAJTfclNDObAcdyoWxJJpTOA;

				private double UwKBHmZgcxacTKurCNmnIwFYlJMsA;

				private double PniWcrKfHMcoNIUGFHTYBuHkSeyj;

				private double EjBcgFBUzdTkbipXlYRaULzwIPPLA;

				private uint UPVOVPHEFlCszuOAqAPpIkOtEmah;

				private uint bUepFPrsfVjdtFdciImUwWxKjTiJ;

				private float uacVVheppvbauKfDUfkaXxLVnMfK;

				private float pPVyYClwlJdFhDfvRFNVlzJsGEYT;

				public double EMJBONudmzmoemRknHndwTJqAsEK => AvQOkiAJTfclNDObAcdyoWxJJpTOA;

				public double EUzsMjiZiiqWmKsDgGFMnsSTffhq => UwKBHmZgcxacTKurCNmnIwFYlJMsA;

				public double xqsJoqPzbPPePZhFMIorBbyjYUPK => PniWcrKfHMcoNIUGFHTYBuHkSeyj;

				public uint OfYKpXoFxkgBnKpUaRPIrGbeiJxkA => UPVOVPHEFlCszuOAqAPpIkOtEmah;

				public uint aBBebyQeKICedhDlYSoxcNHTRLHRA => bUepFPrsfVjdtFdciImUwWxKjTiJ;

				public float kYUIUdJxofdThshVbgJhcyESirAK => uacVVheppvbauKfDUfkaXxLVnMfK;

				public float YMTnfjcLNUhlAGksApaMAWprltYY => pPVyYClwlJdFhDfvRFNVlzJsGEYT;

				public IyqTTFLMGvqgrEvkFClQzqdEeGoF(UpdateLoopType P_0)
				{
					DFDJuhCnjIIWHhfaPwzkLWvhddac = P_0;
					EjBcgFBUzdTkbipXlYRaULzwIPPLA = Time.realtimeSinceStartup;
					UPVOVPHEFlCszuOAqAPpIkOtEmah = 0u;
				}

				public void qJYgSjGpeBOqkyKzyJkJBzvoetUc()
				{
					UwKBHmZgcxacTKurCNmnIwFYlJMsA = AvQOkiAJTfclNDObAcdyoWxJJpTOA;
					AvQOkiAJTfclNDObAcdyoWxJJpTOA = realTime;
					if (EjBcgFBUzdTkbipXlYRaULzwIPPLA > AvQOkiAJTfclNDObAcdyoWxJJpTOA)
					{
						EjBcgFBUzdTkbipXlYRaULzwIPPLA = 0.0;
					}
					PniWcrKfHMcoNIUGFHTYBuHkSeyj = AvQOkiAJTfclNDObAcdyoWxJJpTOA - EjBcgFBUzdTkbipXlYRaULzwIPPLA;
					EjBcgFBUzdTkbipXlYRaULzwIPPLA = AvQOkiAJTfclNDObAcdyoWxJJpTOA;
					bUepFPrsfVjdtFdciImUwWxKjTiJ = UPVOVPHEFlCszuOAqAPpIkOtEmah;
					UPVOVPHEFlCszuOAqAPpIkOtEmah = MiscTools.Tick(UPVOVPHEFlCszuOAqAPpIkOtEmah);
					pPVyYClwlJdFhDfvRFNVlzJsGEYT = uacVVheppvbauKfDUfkaXxLVnMfK;
					uacVVheppvbauKfDUfkaXxLVnMfK = kddaDXMucKDMHHoHoGfEpEaRsVfwA();
					previousFrame = bUepFPrsfVjdtFdciImUwWxKjTiJ;
					currentFrame = UPVOVPHEFlCszuOAqAPpIkOtEmah;
					unscaledTime = AvQOkiAJTfclNDObAcdyoWxJJpTOA;
					unscaledTimePrev = UwKBHmZgcxacTKurCNmnIwFYlJMsA;
					unscaledDeltaTime = PniWcrKfHMcoNIUGFHTYBuHkSeyj;
				}
			}

			private static class zAfsgWCZMvUEmUXPLsZOuiiLqrJw
			{
				public static StopwatchBase bYzXryDPCfXQXclyGJOLNpvslzKG
				{
					get
					{
						if (!UnityTools.isEditor && UnityTools.platform == Platform.XboxOne)
						{
							return UnityStopwatch.Global;
						}
						return Rewired.Utils.Classes.Utility.Stopwatch.Global;
					}
				}

				public static StopwatchBase ieOWXhfJcLXJVuBxfwRGwePbeaZi()
				{
					if (!UnityTools.isEditor && UnityTools.platform == Platform.XboxOne)
					{
						return new UnityStopwatch();
					}
					return new Rewired.Utils.Classes.Utility.Stopwatch();
				}

				public static StopwatchBase jQnSbrksuFBdelUpVppqYLqlAIlIA()
				{
					if (!UnityTools.isEditor && UnityTools.platform == Platform.XboxOne)
					{
						return UnityStopwatch.StartNew();
					}
					return Rewired.Utils.Classes.Utility.Stopwatch.StartNew();
				}
			}

			private StopwatchBase JZpEPMjMAnyKoHgGPeEfjnaFrBOIA;

			private double niFeLFcnOsXVyaUdBVApUORImRLsA;

			private IyqTTFLMGvqgrEvkFClQzqdEeGoF TXeavlekKaqaXQqPwDVGPuVHNDEE;

			private ADictionary<int, IyqTTFLMGvqgrEvkFClQzqdEeGoF> sydZElMxQvoIutzpeFfGuHTpPSdE;

			private uint QnwOBecywMkffXyUpftEBSAmUNUy;

			public double HPcRKhKOhMjfqdvqHOdDbiVZdMenA => TXeavlekKaqaXQqPwDVGPuVHNDEE.EMJBONudmzmoemRknHndwTJqAsEK;

			public double DCUfAHpqeckjRhIMTxcYJKADDodK => TXeavlekKaqaXQqPwDVGPuVHNDEE.EUzsMjiZiiqWmKsDgGFMnsSTffhq;

			public double SbfTzKDPhUTEhwGbCicbDvkluizAA => TXeavlekKaqaXQqPwDVGPuVHNDEE.xqsJoqPzbPPePZhFMIorBbyjYUPK;

			public float PrWDZEHUEoMqLdERYmzILRHaMwLU => TXeavlekKaqaXQqPwDVGPuVHNDEE.kYUIUdJxofdThshVbgJhcyESirAK;

			public float CgcFLERxRGxNrzukyLYoijQYCoko => TXeavlekKaqaXQqPwDVGPuVHNDEE.YMTnfjcLNUhlAGksApaMAWprltYY;

			internal double HOPdnIsQmFkcPHZTOCRDaIhTAaihA => JZpEPMjMAnyKoHgGPeEfjnaFrBOIA.elapsedSeconds + niFeLFcnOsXVyaUdBVApUORImRLsA;

			public uint AIdOHeFZjBFhHWGDroEBzshSgDWFA => TXeavlekKaqaXQqPwDVGPuVHNDEE.OfYKpXoFxkgBnKpUaRPIrGbeiJxkA;

			public uint QoMjhMULVTunMfUQORBHnaLRSUlE => TXeavlekKaqaXQqPwDVGPuVHNDEE.aBBebyQeKICedhDlYSoxcNHTRLHRA;

			public uint urPiAkWoSEqVKPwaOSsDBhJMCggz => QnwOBecywMkffXyUpftEBSAmUNUy;

			public LDrchKIStuCJQDHfATgwEZaUZcaSB()
			{
				JZpEPMjMAnyKoHgGPeEfjnaFrBOIA = zAfsgWCZMvUEmUXPLsZOuiiLqrJw.bYzXryDPCfXQXclyGJOLNpvslzKG;
				hNiPXTnheEgAYOtBwAVJXCzLCwNr();
			}

			public void mFgzgVCFqSMBYBpaBAjoCKdMnQHG()
			{
				niFeLFcnOsXVyaUdBVApUORImRLsA = Time.realtimeSinceStartup;
			}

			public void hNiPXTnheEgAYOtBwAVJXCzLCwNr()
			{
				TXeavlekKaqaXQqPwDVGPuVHNDEE = null;
				sydZElMxQvoIutzpeFfGuHTpPSdE = new ADictionary<int, IyqTTFLMGvqgrEvkFClQzqdEeGoF>();
				using TempListPool.TList<UpdateLoopType> tList = TempListPool.GetTList<UpdateLoopType>(3);
				List<UpdateLoopType> list = tList.list;
				EnumConverter.ToUpdateLoopTypes((UpdateLoopSetting)(-1), list);
				for (int i = 0; i < list.Count; i++)
				{
					IyqTTFLMGvqgrEvkFClQzqdEeGoF iyqTTFLMGvqgrEvkFClQzqdEeGoF = new IyqTTFLMGvqgrEvkFClQzqdEeGoF(list[i]);
					sydZElMxQvoIutzpeFfGuHTpPSdE.Add((int)list[i], iyqTTFLMGvqgrEvkFClQzqdEeGoF);
					if (TXeavlekKaqaXQqPwDVGPuVHNDEE == null)
					{
						TXeavlekKaqaXQqPwDVGPuVHNDEE = iyqTTFLMGvqgrEvkFClQzqdEeGoF;
					}
				}
			}

			public void lwDBSDbpgYGxBgtAiyDGAzaaSLoob(UpdateLoopType P_0)
			{
				if (TXeavlekKaqaXQqPwDVGPuVHNDEE.DFDJuhCnjIIWHhfaPwzkLWvhddac != P_0)
				{
					TXeavlekKaqaXQqPwDVGPuVHNDEE = sydZElMxQvoIutzpeFfGuHTpPSdE[(int)P_0];
				}
				if (P_0 != UpdateLoopType.OnGUI || Event.current.rawType == EventType.Layout)
				{
					TXeavlekKaqaXQqPwDVGPuVHNDEE.qJYgSjGpeBOqkyKzyJkJBzvoetUc();
					QnwOBecywMkffXyUpftEBSAmUNUy = MiscTools.Tick(QnwOBecywMkffXyUpftEBSAmUNUy);
					absFrame = QnwOBecywMkffXyUpftEBSAmUNUy;
				}
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class UnityTouch : CodeHelper
		{
			private static UnityTouch vpefQgtMjLejxrPMOiseBipeLJYf;

			internal static UnityTouch POXovXrWIAjzlpoPPidLwlWoIIyu => vpefQgtMjLejxrPMOiseBipeLJYf ?? (vpefQgtMjLejxrPMOiseBipeLJYf = new UnityTouch());

			public int touchCount => Input.touchCount;

			public Touch[] touches => Input.touches;

			public bool simulateMouseWithTouches
			{
				get
				{
					return Input.simulateMouseWithTouches;
				}
				set
				{
					Input.simulateMouseWithTouches = value;
				}
			}

			public bool multiTouchEnabled
			{
				get
				{
					return Input.multiTouchEnabled;
				}
				set
				{
					Input.multiTouchEnabled = value;
				}
			}

			private UnityTouch()
			{
			}

			public Touch GetTouch(int index)
			{
				return Input.GetTouch(index);
			}
		}

		internal class sBcqfTPTCyxCMIMZgNUXcgRSkEPB
		{
			[Serializable]
			private sealed class nsaSjnhvYPgJBJQkTHFbEOFxkkkFb
			{
				public static readonly nsaSjnhvYPgJBJQkTHFbEOFxkkkFb _003C_003E9 = new nsaSjnhvYPgJBJQkTHFbEOFxkkkFb();

				public static Func<bool> _003C_003E9__12_1;

				public static Func<bool> _003C_003E9__12_2;

				public static Func<int> _003C_003E9__12_3;

				public static Func<float> _003C_003E9__12_4;

				public static Func<bool> _003C_003E9__12_5;

				public static Func<string> _003C_003E9__12_0;

				internal bool rVjsVitrcfYeaOXynkmRdjqKAkEB()
				{
					return Screen.fullScreen;
				}

				internal bool OfYlDOOIurKuZDJxiGlxZVYGcuaI()
				{
					return Application.runInBackground;
				}

				internal int nNEuHvvqRQTWUXglSHrzkRRNcQkI()
				{
					return (int)Screen.fullScreenMode;
				}

				internal float lJtcbexwBfeDfBaFasfFcUtlFsbgb()
				{
					return Time.unscaledDeltaTime;
				}

				internal bool ujiCLpISeqjASDJfjePlJTcsoqfPb()
				{
					return MathTools.ApproximatelyZero(Time.timeScale);
				}

				internal string PRiESPKAvXOxuNFfoMAbdRjjitdw()
				{
					return UnityTools.externalTools.GetFocusedEditorWindowTitle();
				}
			}

			public readonly ValueWatcher<bool> rCTftDWWynDPIdXOxJzufkZyAppS;

			public readonly ValueWatcher<bool> fprvKTDGpmgCSdvSGFkvXVgHfahr;

			public readonly ValueWatcher<bool> DorhhtzMOCBEoCAFbgkcdflbyOgxb;

			public readonly ValueWatcher<bool> zPoAsRMylaokWCNvjIrKsKUbDctX;

			public readonly ValueWatcher<int> bZykPhWdHEGTeESTOVyggnACZscX;

			public readonly ValueWatcher<float> UTXFtWfATWRgjRymvWXWRkQeHmPo;

			public readonly ValueWatcher<string> QzIqsXbpOGekmiBGLFqApRXgsaqAA;

			public readonly ValueWatcher<bool> FatGjtZSkCjvOinZoTxDBsswvbWi;

			private int qQpyHawUFQGFKzVEmlyuZTVyqZWN;

			private readonly ValueWatcher[] HheEceOeRgELVvWhzrrYxxaRriGi;

			public int DYyBxiFMnRWAAvUeXkkKVejAbVtRA => qQpyHawUFQGFKzVEmlyuZTVyqZWN;

			public sBcqfTPTCyxCMIMZgNUXcgRSkEPB()
			{
				bool flag = UnityTools.isEditor || Application.isFocused;
				List<ValueWatcher> list = new List<ValueWatcher>
				{
					(rCTftDWWynDPIdXOxJzufkZyAppS = new ValueWatcher<bool>(flag, false)),
					(fprvKTDGpmgCSdvSGFkvXVgHfahr = new ValueWatcher<bool>(false, false)),
					(DorhhtzMOCBEoCAFbgkcdflbyOgxb = new ValueWatcher<bool>(Screen.fullScreen, nsaSjnhvYPgJBJQkTHFbEOFxkkkFb._003C_003E9.rVjsVitrcfYeaOXynkmRdjqKAkEB, false)),
					(zPoAsRMylaokWCNvjIrKsKUbDctX = new ValueWatcher<bool>(Application.runInBackground, nsaSjnhvYPgJBJQkTHFbEOFxkkkFb._003C_003E9.OfYlDOOIurKuZDJxiGlxZVYGcuaI, false)),
					(bZykPhWdHEGTeESTOVyggnACZscX = new ValueWatcher<int>((int)Screen.fullScreenMode, nsaSjnhvYPgJBJQkTHFbEOFxkkkFb._003C_003E9.nNEuHvvqRQTWUXglSHrzkRRNcQkI, false)),
					(UTXFtWfATWRgjRymvWXWRkQeHmPo = new ValueWatcher<float>(Time.unscaledDeltaTime, nsaSjnhvYPgJBJQkTHFbEOFxkkkFb._003C_003E9.lJtcbexwBfeDfBaFasfFcUtlFsbgb, false)),
					(FatGjtZSkCjvOinZoTxDBsswvbWi = new ValueWatcher<bool>(MathTools.ApproximatelyZero(Time.timeScale), nsaSjnhvYPgJBJQkTHFbEOFxkkkFb._003C_003E9.ujiCLpISeqjASDJfjePlJTcsoqfPb, MathTools.ApproximatelyZero(Time.timeScale)))
				};
				if (editorPlatform != EditorPlatform.None)
				{
					list.Add(QzIqsXbpOGekmiBGLFqApRXgsaqAA = new ValueWatcher<string>(UnityTools.externalTools.GetFocusedEditorWindowTitle(), nsaSjnhvYPgJBJQkTHFbEOFxkkkFb._003C_003E9.PRiESPKAvXOxuNFfoMAbdRjjitdw, false));
				}
				HheEceOeRgELVvWhzrrYxxaRriGi = list.ToArray();
				tftYhzpkMyTnrDGRtAbEjjCNPGMV();
			}

			public void tftYhzpkMyTnrDGRtAbEjjCNPGMV()
			{
				for (int i = 0; i < HheEceOeRgELVvWhzrrYxxaRriGi.Length; i++)
				{
					HheEceOeRgELVvWhzrrYxxaRriGi[i].Update();
				}
				qQpyHawUFQGFKzVEmlyuZTVyqZWN = Time.frameCount;
			}

			public void AmoYkDYJkUSqFWmfZXmupuyepfQl()
			{
				for (int i = 0; i < HheEceOeRgELVvWhzrrYxxaRriGi.Length; i++)
				{
					HheEceOeRgELVvWhzrrYxxaRriGi[i].TriggerEvent();
				}
			}
		}

		[Serializable]
		private sealed class MGkUfnooiWCkAERkxKAwMbUXPeWm
		{
			public static readonly MGkUfnooiWCkAERkxKAwMbUXPeWm _003C_003E9 = new MGkUfnooiWCkAERkxKAwMbUXPeWm();

			public static Func<bool> _003C_003E9__240_0;

			internal void yZkUsIPcMKGucqgQRYVqmkksTVdl(Exception P_0)
			{
				HandleCallbackException("", P_0);
			}

			internal void yTzGWIYrTTWlkYFRmBgdVLdqQPL(Exception P_0)
			{
				HandleCallbackException("Rewired.ReInput.ControllerConnectedEvent", P_0);
			}

			internal void FYfjQYwIWRWhlQnVlpokDJFefvXG(Exception P_0)
			{
				HandleCallbackException("Rewired.ReInput.ControllerPreDisconnectEvent", P_0);
			}

			internal void jaTRXVgHxPzKxHaJadDrSnRBXDKL(Exception P_0)
			{
				HandleCallbackException("Rewired.ReInput.ControllerDisconnectedEvent", P_0);
			}

			internal void MShiKTiPphCxxopmCMrZKGmjxadE(Exception P_0)
			{
				HandleCallbackException("Rewired.ReInput.InputSourceUpdateEvent", P_0);
			}

			internal void ehThqCwOVqkgZUWwxjkYCsvhiMFp(Exception P_0)
			{
				HandleCallbackException("Rewired.ReInput.EditorRecompileEvent", P_0);
			}

			internal void TLgziGXaMyTaxhJsHgKTxwkldGxu(Exception P_0)
			{
				HandleCallbackException("Rewired.ReInput.PreShutDownEvent", P_0);
			}

			internal void TurShGzCEFcRPsLMIBJMQXhTVVCQ(Exception P_0)
			{
				HandleCallbackException("Rewired.ReInput.ShutDownEvent", P_0);
			}

			internal void rGZfywEJhljeizCoMHmaluSbtaPAA(Exception P_0)
			{
				HandleCallbackException("Rewired.ReInput.InitializedEvent", P_0);
			}

			internal bool xifMKVdulvHAabWHVNApApFyGwcJ()
			{
				if (isUnityEditorFocused)
				{
					return isAllowedEditorWindowFocused;
				}
				return false;
			}
		}

		[CustomObfuscation(rename = false)]
		internal const int programVersion1 = 1;

		[CustomObfuscation(rename = false)]
		internal const int programVersion2 = 1;

		[CustomObfuscation(rename = false)]
		internal const int programVersion3 = 59;

		[CustomObfuscation(rename = false)]
		internal const int programVersion4 = 1;

		[CustomObfuscation(rename = false)]
		internal const int dataVersion = 1;

		[CustomObfuscation(rename = false)]
		internal const bool isTrial = false;

		[CustomObfuscation(rename = false)]
		internal const string majorBranch = "U6000";

		private static InputManager_Base cDVMhrNPKDHilizCWRqmajBwasQJ;

		private static PlatformInputManager zNGjjScruOFRrdIdVfVzDkdgujPhA;

		internal static DckEcfkkHGpczSsaRQGOdUFFpVWD stWBKWMOrAnxQyItwkKzVuulIRgF;

		internal static NXTDUDBeDljECgTdktAoWNvFngijA FoarDfUMCtoVFquEtrllUhEjZUUn;

		internal static vVGxMaRQSgzzFjtXaZTAmFOGsEmm KTVqyytqGISutLJQbfhSaKddBlfv;

		private static ControllerDataFiles LWRgoiKgeHjHOiswhfZqkdDTTXODA;

		private static UserData yFcxJYXlykxThQELuKTiwjJTSDUP;

		private static bool inovUwbXLgwsjextWnpkNhsbRCFj;

		private static ConfigVars GUjvmeafVeXwsTBJxzvZxvAaPGij;

		private static UpdateLoopType hDzkDqfsQiHkoOZgEmvpkDoKtuOE;

		private static bool ATiGsPhUPWKdkmfLpiSUBNiehnaxb;

		private static Platform YiqdXGalePxrTOWNrrBdjDnMPESK;

		private static WebplayerPlatform nSCJWWZEcfAdabVylbtGymPFXRrd;

		private static EditorPlatform LEjGRGHnhSxXKfKZJqMYCDGjUVBKA;

		private static bool ZoDfMkswRdFTiHHAMvPMWBnixlkoA;

		private static TimerAbs mEEbMtrbvWLYXANsTfSEAKpKWRkL;

		private static LDrchKIStuCJQDHfATgwEZaUZcaSB hSvDRPkauaeZKJDlzLyieMTBASFlB;

		private static string IeahfihEauzEOJPDSsYYdCknikKq;

		private static bool AVPemOcKfkWYczVANhlzVYKIhviJ;

		private static bool vxPlmHYxBOeFdFDgWaImlOtSGVBn;

		private static bool xYguSnTeeRsxZfJtIDfsqfBUgSzd;

		private static int qFYFOBhjpaStBAWdPBCGiiHUXUDgA;

		[CustomObfuscation(rename = false)]
		internal static int _id;

		private static int UQUwmSnRViTXgqTjVgTNoVztbpKU;

		private static int hwRjxtaUUIaMTBTKOHKSbdMABPKU;

		private static bool uQKkFSljhjsuWoLrgihbcGjKiqbn;

		private static readonly UnityTouch ITEEzogpTfMjgWuCCuiaTsfSyktkA;

		private static readonly PlayerHelper TFvdYkeuJStNMhJTScxjAkJIcMtoB;

		private static readonly ControllerHelper iTEgIKcePpHegEDJDmTWWxYahmBjB;

		private static readonly MappingHelper RCoFaRBGqTInoehLkkOwAQIYwniIB;

		private static readonly TimeHelper BfHJDqCQrExzSksjffvEsOdRpxhJ;

		private static readonly ConfigHelper BQZjQSjxHhKjimPocQtmfqaCbVUF;

		private static readonly LocalizationHelper xIWiHikPQAJCJgCctkYksyBLDhAn;

		private static readonly GlyphHelper qjCxTdTPgqOuCjcZHMBztoEpbjGn;

		private static aMedMVNTIqnhicrKJWiBDJyWkGfg uLrgmyotPfzwaIGcSpFKaxGDEfjG;

		private static UserDataStore UbBcKMOxPIbZfdPfMoDLitEDJCuR;

		private static IControllerAssigner HulHinCwhKVHDMDuzqbdhtxOhMPn;

		private static sBcqfTPTCyxCMIMZgNUXcgRSkEPB YNsKnkTRZYIJBWGPKzEDlaqoIbrA;

		private static int skUBPwcChrpPuHkaWDgJGsEadpYAc;

		private static SafeAction<ControllerStatusChangedEventArgs> IlvmHErtqEihwCPcEtbQxzIUXLaj;

		private static SafeAction<ControllerStatusChangedEventArgs> jePogismdTiNgTPlensACEVgcmch;

		private static SafeAction<ControllerStatusChangedEventArgs> WVHJeNwaDrLTFYHhCXGUxVtDniBG;

		private static SafeAction VZDEIGoOcFwVZDpHkUFCQkHjSCZD;

		private static SafeAction xMhDzdAKXZkrIbLofJLoshCuWICk;

		private static SafeAction jIFLtMmDEFvMixTWBElgjonMgZfL;

		private static SafeAction lRPURjiPheavHaCbvmRtntVYDzNHA;

		private static SafeAction fpFUkdkyTaZDEsPxRFhghAPHRWc;

		[CustomObfuscation(rename = false)]
		private static Action<bool> _ApplicationFocusChangedEvent;

		[CustomObfuscation(rename = false)]
		private static Action<bool> _ApplicationPauseChangedEvent;

		private static Action jRqCytUNSMkiQOIhzHQbFdtopNjo;

		private static Action<UpdateLoopType> lmbcFEIsrzugMwBZMmMGGhscrXsO;

		private static Action<UpdateLoopType> DAaZPmEAiBMuILaIxfRQKEijKkLhA;

		private static Action<UpdateLoopType> gxzkypAlObmSKYYoMQPprxFkRSsx;

		private static Action TnFxAihEUcgcJheqhbGLENJPmjXOA;

		private static Action<bool> TkzwfvwrPakTRTjueGeVVUIGSSsv;

		private static Action<bool> myGkoLzPwyUJzUsHwPeXByMeeAUF;

		private static Action<bool> ElcQAmgaFoFfUjJcwMkZwClCdpqiA;

		private static Action<FullScreenMode> OjKFfdgrEOqUiTMwAwpqFvbEcakEb;

		private static Action LFMjvcXYTCppPfcDTKpKsXtfiybP;

		private static Action<bool> vfwBoiEWnkPcDieMiMihGvIMYMghA;

		[CustomObfuscation(rename = false)]
		internal static double unscaledDeltaTime;

		[CustomObfuscation(rename = false)]
		internal static double unscaledTime;

		[CustomObfuscation(rename = false)]
		internal static double unscaledTimePrev;

		[CustomObfuscation(rename = false)]
		internal static uint currentFrame;

		[CustomObfuscation(rename = false)]
		internal static uint previousFrame;

		[CustomObfuscation(rename = false)]
		internal static uint absFrame;

		private static aMedMVNTIqnhicrKJWiBDJyWkGfg LtLPnFRiUIztAXIRagmfniCjVdpN => uLrgmyotPfzwaIGcSpFKaxGDEfjG ?? (uLrgmyotPfzwaIGcSpFKaxGDEfjG = new aMedMVNTIqnhicrKJWiBDJyWkGfg(GUjvmeafVeXwsTBJxzvZxvAaPGij.updateLoop));

		private static bool eCKAQaAGGyaipXtiYemwxFTbZfxd => skUBPwcChrpPuHkaWDgJGsEadpYAc > 0;

		public static PlayerHelper players
		{
			get
			{
				if (!inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					axSwwsMdJdcTYbypVNNVuuAtaKOxA();
					return null;
				}
				return TFvdYkeuJStNMhJTScxjAkJIcMtoB;
			}
		}

		public static ControllerHelper controllers
		{
			get
			{
				if (!inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					axSwwsMdJdcTYbypVNNVuuAtaKOxA();
					return null;
				}
				return iTEgIKcePpHegEDJDmTWWxYahmBjB;
			}
		}

		public static MappingHelper mapping
		{
			get
			{
				if (!inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					axSwwsMdJdcTYbypVNNVuuAtaKOxA();
					return null;
				}
				return RCoFaRBGqTInoehLkkOwAQIYwniIB;
			}
		}

		public static UnityTouch touch
		{
			get
			{
				if (!inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					axSwwsMdJdcTYbypVNNVuuAtaKOxA();
					return null;
				}
				return ITEEzogpTfMjgWuCCuiaTsfSyktkA;
			}
		}

		public static TimeHelper time
		{
			get
			{
				if (!inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					axSwwsMdJdcTYbypVNNVuuAtaKOxA();
					return null;
				}
				return BfHJDqCQrExzSksjffvEsOdRpxhJ;
			}
		}

		public static IUserDataStore userDataStore
		{
			get
			{
				if (!inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					axSwwsMdJdcTYbypVNNVuuAtaKOxA();
					return null;
				}
				return UbBcKMOxPIbZfdPfMoDLitEDJCuR;
			}
		}

		public static ConfigHelper configuration
		{
			get
			{
				if (!inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					axSwwsMdJdcTYbypVNNVuuAtaKOxA();
					return null;
				}
				return BQZjQSjxHhKjimPocQtmfqaCbVUF;
			}
		}

		public static LocalizationHelper localization
		{
			get
			{
				if (!inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					axSwwsMdJdcTYbypVNNVuuAtaKOxA();
					return null;
				}
				return xIWiHikPQAJCJgCctkYksyBLDhAn;
			}
		}

		public static GlyphHelper glyphs
		{
			get
			{
				if (!inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					axSwwsMdJdcTYbypVNNVuuAtaKOxA();
					return null;
				}
				return qjCxTdTPgqOuCjcZHMBztoEpbjGn;
			}
		}

		public static string programVersion => 1 + "." + 1 + "." + 59 + "." + 1 + ".U6000";

		public static bool usingUnityInput => ATiGsPhUPWKdkmfLpiSUBNiehnaxb;

		public static bool unityJoystickIdentificationRequired
		{
			get
			{
				if (!inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					return false;
				}
				if (isWindowsStandaloneWebplayerOrEditorPlatform && !UnityTools.windowsJoystickNamesReturnsEmptyStringsIfJoystickNull)
				{
					return true;
				}
				return false;
			}
		}

		public static bool isReady => inovUwbXLgwsjextWnpkNhsbRCFj;

		[CustomObfuscation(rename = false)]
		internal static int id => _id;

		[CustomObfuscation(rename = false)]
		internal static bool initialized => inovUwbXLgwsjextWnpkNhsbRCFj;

		[CustomObfuscation(rename = false)]
		internal static UpdateLoopType currentUpdateLoop => hDzkDqfsQiHkoOZgEmvpkDoKtuOE;

		[CustomObfuscation(rename = false)]
		internal static ConfigVars configVars => GUjvmeafVeXwsTBJxzvZxvAaPGij;

		[CustomObfuscation(rename = false)]
		internal static IConfigVars_Internal pluginConfigVars => GUjvmeafVeXwsTBJxzvZxvAaPGij;

		[CustomObfuscation(rename = false)]
		internal static UserData UserData => yFcxJYXlykxThQELuKTiwjJTSDUP;

		[CustomObfuscation(rename = false)]
		internal static Platform currentPlatform => YiqdXGalePxrTOWNrrBdjDnMPESK;

		[CustomObfuscation(rename = false)]
		internal static WebplayerPlatform webplayerPlatform => nSCJWWZEcfAdabVylbtGymPFXRrd;

		[CustomObfuscation(rename = false)]
		internal static EditorPlatform editorPlatform => LEjGRGHnhSxXKfKZJqMYCDGjUVBKA;

		[CustomObfuscation(rename = false)]
		internal static bool checkNeverPressed
		{
			get
			{
				if (YiqdXGalePxrTOWNrrBdjDnMPESK == Platform.Linux && ATiGsPhUPWKdkmfLpiSUBNiehnaxb)
				{
					return true;
				}
				if (YiqdXGalePxrTOWNrrBdjDnMPESK == Platform.OSX && (ATiGsPhUPWKdkmfLpiSUBNiehnaxb || primaryInputManager.inputSourceType == InputSource.OSX))
				{
					return true;
				}
				if (UnityTools.isAndroidPlatform && ATiGsPhUPWKdkmfLpiSUBNiehnaxb)
				{
					return true;
				}
				if (YiqdXGalePxrTOWNrrBdjDnMPESK == Platform.Webplayer && nSCJWWZEcfAdabVylbtGymPFXRrd == WebplayerPlatform.OSX)
				{
					return true;
				}
				if (YiqdXGalePxrTOWNrrBdjDnMPESK == Platform.WebGL)
				{
					return true;
				}
				return false;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool isEditor => LEjGRGHnhSxXKfKZJqMYCDGjUVBKA != EditorPlatform.None;

		[CustomObfuscation(rename = false)]
		internal static Guid defaultHardwareJoystickMapGuid
		{
			get
			{
				if (!inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					return Guid.Empty;
				}
				return LWRgoiKgeHjHOiswhfZqkdDTTXODA.defaultHardwareJoystickMapGuid;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool isRunningInEditMode => vxPlmHYxBOeFdFDgWaImlOtSGVBn;

		[CustomObfuscation(rename = false)]
		internal static bool isEditorPaused => UnityTools.externalTools.isEditorPaused;

		[CustomObfuscation(rename = false)]
		internal static float unityUnscaledDeltaTime => hSvDRPkauaeZKJDlzLyieMTBASFlB.PrWDZEHUEoMqLdERYmzILRHaMwLU;

		[CustomObfuscation(rename = false)]
		internal static float unityUnscaledDeltaTimePrev => hSvDRPkauaeZKJDlzLyieMTBASFlB.CgcFLERxRGxNrzukyLYoijQYCoko;

		[CustomObfuscation(rename = false)]
		internal static double realTime
		{
			get
			{
				if (!inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					return 0.0;
				}
				return hSvDRPkauaeZKJDlzLyieMTBASFlB.HOPdnIsQmFkcPHZTOCRDaIhTAaihA;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static int currentUnityFrame
		{
			get
			{
				if (!inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					return 0;
				}
				return YNsKnkTRZYIJBWGPKzEDlaqoIbrA.DYyBxiFMnRWAAvUeXkkKVejAbVtRA;
			}
		}

		private static bool WxgEQiEbaaFUqHMPIEcnGasJQOYhA
		{
			get
			{
				if (UnityTools.unityVersion >= UnityTools.UnityVersion.UNITY_5_1)
				{
					return IeahfihEauzEOJPDSsYYdCknikKq == "Game";
				}
				return IeahfihEauzEOJPDSsYYdCknikKq == "UnityEditor.GameView";
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool isAllowedEditorWindowFocused
		{
			get
			{
				if (GUjvmeafVeXwsTBJxzvZxvAaPGij.allowInputInEditorSceneView && UnityTools.externalTools.IsEditorSceneViewFocused())
				{
					return true;
				}
				if (!xYguSnTeeRsxZfJtIDfsqfBUgSzd)
				{
					return WxgEQiEbaaFUqHMPIEcnGasJQOYhA;
				}
				return true;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool isUnityEditorFocused
		{
			get
			{
				if (zNGjjScruOFRrdIdVfVzDkdgujPhA is INativePlatformHelper nativePlatformHelper)
				{
					return nativePlatformHelper.isApplicationFocused;
				}
				return xYguSnTeeRsxZfJtIDfsqfBUgSzd;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool isWindowsStandaloneWebplayerOrEditorPlatform
		{
			get
			{
				if (!inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					return false;
				}
				if (!ATiGsPhUPWKdkmfLpiSUBNiehnaxb)
				{
					return false;
				}
				if (YiqdXGalePxrTOWNrrBdjDnMPESK != Platform.Windows && (YiqdXGalePxrTOWNrrBdjDnMPESK != Platform.Webplayer || nSCJWWZEcfAdabVylbtGymPFXRrd != WebplayerPlatform.Windows))
				{
					return LEjGRGHnhSxXKfKZJqMYCDGjUVBKA == EditorPlatform.Windows;
				}
				return true;
			}
		}

		private static bool gxociZaYeXBFCmTVyFasELybZqhUA
		{
			get
			{
				if (!inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					return false;
				}
				if (!YNsKnkTRZYIJBWGPKzEDlaqoIbrA.rCTftDWWynDPIdXOxJzufkZyAppS.value)
				{
					if (uQKkFSljhjsuWoLrgihbcGjKiqbn)
					{
						return false;
					}
					if ((!isEditor || !isUnityEditorFocused) && !YNsKnkTRZYIJBWGPKzEDlaqoIbrA.zPoAsRMylaokWCNvjIrKsKUbDctX.value)
					{
						return false;
					}
				}
				return true;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool applicationIsFocused
		{
			get
			{
				if (inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					return YNsKnkTRZYIJBWGPKzEDlaqoIbrA.rCTftDWWynDPIdXOxJzufkZyAppS.value;
				}
				return false;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool applicationIsPaused
		{
			get
			{
				if (inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					return YNsKnkTRZYIJBWGPKzEDlaqoIbrA.fprvKTDGpmgCSdvSGFkvXVgHfahr.value;
				}
				return false;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool applicationIsFullScreen
		{
			get
			{
				if (inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					return YNsKnkTRZYIJBWGPKzEDlaqoIbrA.DorhhtzMOCBEoCAFbgkcdflbyOgxb.value;
				}
				return false;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool applicationRunInBackground
		{
			get
			{
				if (inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					return YNsKnkTRZYIJBWGPKzEDlaqoIbrA.zPoAsRMylaokWCNvjIrKsKUbDctX.value;
				}
				return false;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool timeScaleIsPaused
		{
			get
			{
				if (inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					return YNsKnkTRZYIJBWGPKzEDlaqoIbrA.FatGjtZSkCjvOinZoTxDBsswvbWi.value;
				}
				return false;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static InputManager_Base rewiredInputManager => cDVMhrNPKDHilizCWRqmajBwasQJ;

		[CustomObfuscation(rename = false)]
		internal static PlatformInputManager primaryInputManager
		{
			get
			{
				if (!inovUwbXLgwsjextWnpkNhsbRCFj)
				{
					axSwwsMdJdcTYbypVNNVuuAtaKOxA();
					return null;
				}
				return zNGjjScruOFRrdIdVfVzDkdgujPhA.primaryInputManager;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static IControllerAssigner controllerAssigner
		{
			get
			{
				return HulHinCwhKVHDMDuzqbdhtxOhMPn;
			}
			set
			{
				HulHinCwhKVHDMDuzqbdhtxOhMPn = value;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static RewiredVersion rewiredVersion => new RewiredVersion(programVersion);

		[CustomObfuscation(rename = false)]
		internal static int timeScalePauseChangedCount => hwRjxtaUUIaMTBTKOHKSbdMABPKU;

		public static event Action<ControllerStatusChangedEventArgs> ControllerConnectedEvent
		{
			add
			{
				IlvmHErtqEihwCPcEtbQxzIUXLaj += value;
			}
			remove
			{
				IlvmHErtqEihwCPcEtbQxzIUXLaj -= value;
			}
		}

		public static event Action<ControllerStatusChangedEventArgs> ControllerPreDisconnectEvent
		{
			add
			{
				jePogismdTiNgTPlensACEVgcmch += value;
			}
			remove
			{
				jePogismdTiNgTPlensACEVgcmch -= value;
			}
		}

		public static event Action<ControllerStatusChangedEventArgs> ControllerDisconnectedEvent
		{
			add
			{
				WVHJeNwaDrLTFYHhCXGUxVtDniBG += value;
			}
			remove
			{
				WVHJeNwaDrLTFYHhCXGUxVtDniBG -= value;
			}
		}

		public static event Action InputSourceUpdateEvent
		{
			add
			{
				VZDEIGoOcFwVZDpHkUFCQkHjSCZD += value;
			}
			remove
			{
				VZDEIGoOcFwVZDpHkUFCQkHjSCZD -= value;
			}
		}

		public static event Action EditorRecompileEvent
		{
			add
			{
				xMhDzdAKXZkrIbLofJLoshCuWICk += value;
			}
			remove
			{
				xMhDzdAKXZkrIbLofJLoshCuWICk -= value;
			}
		}

		public static event Action PreShutDownEvent
		{
			add
			{
				jIFLtMmDEFvMixTWBElgjonMgZfL += value;
			}
			remove
			{
				jIFLtMmDEFvMixTWBElgjonMgZfL -= value;
			}
		}

		public static event Action ShutDownEvent
		{
			add
			{
				lRPURjiPheavHaCbvmRtntVYDzNHA += value;
			}
			remove
			{
				lRPURjiPheavHaCbvmRtntVYDzNHA -= value;
			}
		}

		public static event Action InitializedEvent
		{
			add
			{
				fpFUkdkyTaZDEsPxRFhghAPHRWc += value;
			}
			remove
			{
				fpFUkdkyTaZDEsPxRFhghAPHRWc -= value;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action<bool> ApplicationFocusChangedEvent
		{
			add
			{
				_ApplicationFocusChangedEvent = (Action<bool>)Delegate.Combine(_ApplicationFocusChangedEvent, value);
			}
			remove
			{
				_ApplicationFocusChangedEvent = (Action<bool>)Delegate.Remove(_ApplicationFocusChangedEvent, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action<bool> ApplicationPauseChangedEvent
		{
			add
			{
				_ApplicationPauseChangedEvent = (Action<bool>)Delegate.Combine(_ApplicationPauseChangedEvent, value);
			}
			remove
			{
				_ApplicationPauseChangedEvent = (Action<bool>)Delegate.Remove(_ApplicationPauseChangedEvent, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action EarlyUpdateEvent
		{
			add
			{
				jRqCytUNSMkiQOIhzHQbFdtopNjo = (Action)Delegate.Combine(jRqCytUNSMkiQOIhzHQbFdtopNjo, value);
			}
			remove
			{
				jRqCytUNSMkiQOIhzHQbFdtopNjo = (Action)Delegate.Remove(jRqCytUNSMkiQOIhzHQbFdtopNjo, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action<UpdateLoopType> BeforeTimeManagerUpdateEvent
		{
			add
			{
				lmbcFEIsrzugMwBZMmMGGhscrXsO = (Action<UpdateLoopType>)Delegate.Combine(lmbcFEIsrzugMwBZMmMGGhscrXsO, value);
			}
			remove
			{
				lmbcFEIsrzugMwBZMmMGGhscrXsO = (Action<UpdateLoopType>)Delegate.Remove(lmbcFEIsrzugMwBZMmMGGhscrXsO, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action<UpdateLoopType> UpdateStartedEvent
		{
			add
			{
				DAaZPmEAiBMuILaIxfRQKEijKkLhA = (Action<UpdateLoopType>)Delegate.Combine(DAaZPmEAiBMuILaIxfRQKEijKkLhA, value);
			}
			remove
			{
				DAaZPmEAiBMuILaIxfRQKEijKkLhA = (Action<UpdateLoopType>)Delegate.Remove(DAaZPmEAiBMuILaIxfRQKEijKkLhA, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action<UpdateLoopType> UpdateEndedEvent
		{
			add
			{
				gxzkypAlObmSKYYoMQPprxFkRSsx = (Action<UpdateLoopType>)Delegate.Combine(gxzkypAlObmSKYYoMQPprxFkRSsx, value);
			}
			remove
			{
				gxzkypAlObmSKYYoMQPprxFkRSsx = (Action<UpdateLoopType>)Delegate.Remove(gxzkypAlObmSKYYoMQPprxFkRSsx, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action LateUpdateEvent
		{
			add
			{
				TnFxAihEUcgcJheqhbGLENJPmjXOA = (Action)Delegate.Combine(TnFxAihEUcgcJheqhbGLENJPmjXOA, value);
			}
			remove
			{
				TnFxAihEUcgcJheqhbGLENJPmjXOA = (Action)Delegate.Remove(TnFxAihEUcgcJheqhbGLENJPmjXOA, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action<bool> ApplicationIsFullScreenChangedEvent
		{
			add
			{
				TkzwfvwrPakTRTjueGeVVUIGSSsv = (Action<bool>)Delegate.Combine(TkzwfvwrPakTRTjueGeVVUIGSSsv, value);
			}
			remove
			{
				TkzwfvwrPakTRTjueGeVVUIGSSsv = (Action<bool>)Delegate.Remove(TkzwfvwrPakTRTjueGeVVUIGSSsv, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action<bool> ApplicationRunInBackgroundChangedEvent
		{
			add
			{
				myGkoLzPwyUJzUsHwPeXByMeeAUF = (Action<bool>)Delegate.Combine(myGkoLzPwyUJzUsHwPeXByMeeAUF, value);
			}
			remove
			{
				myGkoLzPwyUJzUsHwPeXByMeeAUF = (Action<bool>)Delegate.Remove(myGkoLzPwyUJzUsHwPeXByMeeAUF, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action<bool> TimeScalePauseChangedEvent
		{
			add
			{
				ElcQAmgaFoFfUjJcwMkZwClCdpqiA = (Action<bool>)Delegate.Combine(ElcQAmgaFoFfUjJcwMkZwClCdpqiA, value);
			}
			remove
			{
				ElcQAmgaFoFfUjJcwMkZwClCdpqiA = (Action<bool>)Delegate.Remove(ElcQAmgaFoFfUjJcwMkZwClCdpqiA, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action<FullScreenMode> ApplicationFullScreenModeChangedEvent
		{
			add
			{
				OjKFfdgrEOqUiTMwAwpqFvbEcakEb = (Action<FullScreenMode>)Delegate.Combine(OjKFfdgrEOqUiTMwAwpqFvbEcakEb, value);
			}
			remove
			{
				OjKFfdgrEOqUiTMwAwpqFvbEcakEb = (Action<FullScreenMode>)Delegate.Remove(OjKFfdgrEOqUiTMwAwpqFvbEcakEb, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action SceneLoadedEvent
		{
			add
			{
				LFMjvcXYTCppPfcDTKpKsXtfiybP = (Action)Delegate.Combine(LFMjvcXYTCppPfcDTKpKsXtfiybP, value);
			}
			remove
			{
				LFMjvcXYTCppPfcDTKpKsXtfiybP = (Action)Delegate.Remove(LFMjvcXYTCppPfcDTKpKsXtfiybP, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action<bool> EditorPauseChangedEvent
		{
			add
			{
				vfwBoiEWnkPcDieMiMihGvIMYMghA = (Action<bool>)Delegate.Combine(vfwBoiEWnkPcDieMiMihGvIMYMghA, value);
			}
			remove
			{
				vfwBoiEWnkPcDieMiMihGvIMYMghA = (Action<bool>)Delegate.Remove(vfwBoiEWnkPcDieMiMihGvIMYMghA, value);
			}
		}

		static ReInput()
		{
			xYguSnTeeRsxZfJtIDfsqfBUgSzd = true;
			qFYFOBhjpaStBAWdPBCGiiHUXUDgA = -1;
			_id = -1;
			UQUwmSnRViTXgqTjVgTNoVztbpKU = 0;
			ITEEzogpTfMjgWuCCuiaTsfSyktkA = UnityTouch.POXovXrWIAjzlpoPPidLwlWoIIyu;
			TFvdYkeuJStNMhJTScxjAkJIcMtoB = PlayerHelper.fqxEmVsThfKpPgrmumetLSEjqcFq;
			iTEgIKcePpHegEDJDmTWWxYahmBjB = ControllerHelper.LpVAXpiVHlQcnvltFwbImSKTLatF;
			RCoFaRBGqTInoehLkkOwAQIYwniIB = MappingHelper.AVvLUjXeLvdrGrSsaHifwWnUOrNk;
			BfHJDqCQrExzSksjffvEsOdRpxhJ = TimeHelper.kBdUKIBPLXUuMWRaCNEeIsUeGcYgA;
			BQZjQSjxHhKjimPocQtmfqaCbVUF = ConfigHelper.fRYjhvxxJuAvTfSQOyLzPXfQVjxE;
			xIWiHikPQAJCJgCctkYksyBLDhAn = LocalizationHelper.xmDdvaCOnPkVEcAEwKesZEENVnGDA;
			qjCxTdTPgqOuCjcZHMBztoEpbjGn = GlyphHelper.xiZZVqZMjABxdoOaHlwXNbVTXsBI;
			IlvmHErtqEihwCPcEtbQxzIUXLaj = new SafeAction<ControllerStatusChangedEventArgs>(MGkUfnooiWCkAERkxKAwMbUXPeWm._003C_003E9.yTzGWIYrTTWlkYFRmBgdVLdqQPL);
			jePogismdTiNgTPlensACEVgcmch = new SafeAction<ControllerStatusChangedEventArgs>(MGkUfnooiWCkAERkxKAwMbUXPeWm._003C_003E9.FYfjQYwIWRWhlQnVlpokDJFefvXG);
			WVHJeNwaDrLTFYHhCXGUxVtDniBG = new SafeAction<ControllerStatusChangedEventArgs>(MGkUfnooiWCkAERkxKAwMbUXPeWm._003C_003E9.jaTRXVgHxPzKxHaJadDrSnRBXDKL);
			VZDEIGoOcFwVZDpHkUFCQkHjSCZD = new SafeAction(MGkUfnooiWCkAERkxKAwMbUXPeWm._003C_003E9.MShiKTiPphCxxopmCMrZKGmjxadE);
			xMhDzdAKXZkrIbLofJLoshCuWICk = new SafeAction(MGkUfnooiWCkAERkxKAwMbUXPeWm._003C_003E9.ehThqCwOVqkgZUWwxjkYCsvhiMFp);
			jIFLtMmDEFvMixTWBElgjonMgZfL = new SafeAction(MGkUfnooiWCkAERkxKAwMbUXPeWm._003C_003E9.TLgziGXaMyTaxhJsHgKTxwkldGxu);
			lRPURjiPheavHaCbvmRtntVYDzNHA = new SafeAction(MGkUfnooiWCkAERkxKAwMbUXPeWm._003C_003E9.TurShGzCEFcRPsLMIBJMQXhTVVCQ);
			fpFUkdkyTaZDEsPxRFhghAPHRWc = new SafeAction(MGkUfnooiWCkAERkxKAwMbUXPeWm._003C_003E9.rGZfywEJhljeizCoMHmaluSbtaPAA);
			SafeDelegate.S_ExceptionHandler = MGkUfnooiWCkAERkxKAwMbUXPeWm._003C_003E9.yZkUsIPcMKGucqgQRYVqmkksTVdl;
		}

		private static void rUoCEWEVeRGNxHPCVtEemjIeWEhvA()
		{
			skUBPwcChrpPuHkaWDgJGsEadpYAc++;
		}

		private static void JIPDJGBKlIrJuAgvqhFDHFZCBNwHB()
		{
			skUBPwcChrpPuHkaWDgJGsEadpYAc--;
			if (skUBPwcChrpPuHkaWDgJGsEadpYAc < 0)
			{
				skUBPwcChrpPuHkaWDgJGsEadpYAc = 0;
			}
		}

		public static void Update()
		{
			if (inovUwbXLgwsjextWnpkNhsbRCFj)
			{
				if (GUjvmeafVeXwsTBJxzvZxvAaPGij.updateMode != UpdateMode.Manual)
				{
					Logger.LogError("Rewired cannot be updated manually unless Update Mode is set to Manual.");
				}
				else
				{
					cDVMhrNPKDHilizCWRqmajBwasQJ.DoUpdate(UpdateLoopType.Update, UpdateLoopSetting.Update);
				}
			}
		}

		public static void Reset()
		{
			if (inovUwbXLgwsjextWnpkNhsbRCFj && !(cDVMhrNPKDHilizCWRqmajBwasQJ == null))
			{
				if (eCKAQaAGGyaipXtiYemwxFTbZfxd)
				{
					Logger.LogError("You are attempting to reset Rewired in the middle of its update routine, probably in an event callback. This is inherently unsafe and would lead to undefined behavior. Rewired will not be reset.");
				}
				else
				{
					cDVMhrNPKDHilizCWRqmajBwasQJ.ResetAll();
				}
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool IsInputAllowed(ControllerType controllerType)
		{
			if (!gxociZaYeXBFCmTVyFasELybZqhUA)
			{
				return false;
			}
			if (LEjGRGHnhSxXKfKZJqMYCDGjUVBKA != EditorPlatform.None && (controllerType == ControllerType.Keyboard || controllerType == ControllerType.Mouse))
			{
				if (uQKkFSljhjsuWoLrgihbcGjKiqbn)
				{
					if (!YNsKnkTRZYIJBWGPKzEDlaqoIbrA.rCTftDWWynDPIdXOxJzufkZyAppS.value)
					{
						return false;
					}
				}
				else if (!isAllowedEditorWindowFocused)
				{
					return false;
				}
			}
			return true;
		}

		private static void DFrVuEnOkCrEtWOSASAkrDgCBzse()
		{
			YiqdXGalePxrTOWNrrBdjDnMPESK = UnityTools.platform;
			nSCJWWZEcfAdabVylbtGymPFXRrd = UnityTools.webplayerPlatform;
			LEjGRGHnhSxXKfKZJqMYCDGjUVBKA = UnityTools.editorPlatform;
		}

		internal static void DWxsqOSIxChqCEhqSvvHsJewazPfb(InputManager_Base P_0, Func<ConfigVars, object> P_1, ConfigVars P_2, ControllerDataFiles P_3, UserData P_4, Func<UnityTools.StIgCRgsklQdBuXGCpKvcsOPHcHhb> P_5, Action<Platform> P_6, Action<InputManager_Base.mDgyADDglGQPlMONtRWxEsDTqPKX> P_7)
		{
			try
			{
				rUoCEWEVeRGNxHPCVtEemjIeWEhvA();
				_id = UQUwmSnRViTXgqTjVgTNoVztbpKU;
				UQUwmSnRViTXgqTjVgTNoVztbpKU++;
				inovUwbXLgwsjextWnpkNhsbRCFj = true;
				AVPemOcKfkWYczVANhlzVYKIhviJ = true;
				vxPlmHYxBOeFdFDgWaImlOtSGVBn = UnityTools.isEditor && !Application.isPlaying;
				if (UnityTools.isEditor)
				{
					CheckRewiredVersionCompatibility();
				}
				cDVMhrNPKDHilizCWRqmajBwasQJ = P_0;
				GUjvmeafVeXwsTBJxzvZxvAaPGij = P_2;
				DFrVuEnOkCrEtWOSASAkrDgCBzse();
				if (P_2.logToScreen)
				{
					Logger.logToScreen = true;
				}
				UnityTools.externalTools.EditorPausedStateChangedEvent += DXMWgXfJeGywdOsJyaBcUQDYpPvo;
				LWRgoiKgeHjHOiswhfZqkdDTTXODA = P_3;
				yFcxJYXlykxThQELuKTiwjJTSDUP = P_4;
				mEEbMtrbvWLYXANsTfSEAKpKWRkL = new TimerAbs(1.0);
				hSvDRPkauaeZKJDlzLyieMTBASFlB = new LDrchKIStuCJQDHfATgwEZaUZcaSB();
				LocalizationManager.Initialize();
				GlyphManager.Initialize();
				P_4.sYenrtjyzFlZbHGsMCbWQdyazWCe();
				ThreadSafeUnityInput.Initialize();
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA = new sBcqfTPTCyxCMIMZgNUXcgRSkEPB();
				if (!UnityTools.isEditor)
				{
					xYguSnTeeRsxZfJtIDfsqfBUgSzd = Application.isFocused;
				}
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.rCTftDWWynDPIdXOxJzufkZyAppS.Set(xYguSnTeeRsxZfJtIDfsqfBUgSzd);
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.rCTftDWWynDPIdXOxJzufkZyAppS.Use();
				if (LEjGRGHnhSxXKfKZJqMYCDGjUVBKA != EditorPlatform.None)
				{
					YNsKnkTRZYIJBWGPKzEDlaqoIbrA.rCTftDWWynDPIdXOxJzufkZyAppS.getValueDelegate = MGkUfnooiWCkAERkxKAwMbUXPeWm._003C_003E9.xifMKVdulvHAabWHVNApApFyGwcJ;
					if (vxPlmHYxBOeFdFDgWaImlOtSGVBn)
					{
						xYguSnTeeRsxZfJtIDfsqfBUgSzd = WxgEQiEbaaFUqHMPIEcnGasJQOYhA;
					}
					YNsKnkTRZYIJBWGPKzEDlaqoIbrA.rCTftDWWynDPIdXOxJzufkZyAppS.Set(isUnityEditorFocused && isAllowedEditorWindowFocused);
				}
				TteyEJVBkWuPjNrTNtPZotBTLjRO();
				List<ICustomPlatformInitializer> componentsInSelfAndChildren = UnityTools.GetComponentsInSelfAndChildren<ICustomPlatformInitializer>(P_0.gameObject);
				if (componentsInSelfAndChildren != null)
				{
					for (int i = 0; i < componentsInSelfAndChildren.Count; i++)
					{
						Behaviour behaviour = componentsInSelfAndChildren[i] as Behaviour;
						if (behaviour == null || !behaviour.enabled || !behaviour.gameObject.activeInHierarchy)
						{
							continue;
						}
						CustomPlatformInitOptions customPlatformInitOptions = componentsInSelfAndChildren[i].GetCustomPlatformInitOptions();
						if (customPlatformInitOptions != null)
						{
							pvRPaQsIqigEcORkKjmYXGlVnyZO.cBXKtwJGtBRNVxTEewuaskEervpF(customPlatformInitOptions);
							bool num = LEjGRGHnhSxXKfKZJqMYCDGjUVBKA != EditorPlatform.None;
							P_7(new InputManager_Base.mDgyADDglGQPlMONtRWxEsDTqPKX
							{
								qayVzFPDDLOOAOgQagwfvlVMFWYs = Platform.Custom,
								nFxmqAPigUxiLZgrJgegfsxqiNjbb = EditorPlatform.None,
								neNcrnNGkiIgWcVlKqVWPtBprEIWA = WebplayerPlatform.None
							});
							DFrVuEnOkCrEtWOSASAkrDgCBzse();
							hSvDRPkauaeZKJDlzLyieMTBASFlB = new LDrchKIStuCJQDHfATgwEZaUZcaSB();
							if (num)
							{
								Logger.LogWarning("A custom platform is in use. All input will be managed by user-defined custom platform handling.");
							}
							break;
						}
					}
				}
				AroNjGKbBsWJpndNHeRFYEesRbok(P_1, P_5(), P_6);
				stWBKWMOrAnxQyItwkKzVuulIRgF = new DckEcfkkHGpczSsaRQGOdUFFpVWD(P_4.GetActions_Copy());
				FoarDfUMCtoVFquEtrllUhEjZUUn = new NXTDUDBeDljECgTdktAoWNvFngijA(P_2, zNGjjScruOFRrdIdVfVzDkdgujPhA);
				KTVqyytqGISutLJQbfhSaKddBlfv = new vVGxMaRQSgzzFjtXaZTAmFOGsEmm(P_2);
				zNGjjScruOFRrdIdVfVzDkdgujPhA.DeviceConnectedEvent += VhvkucmArxchvYrYSDDacGEoUENe;
				zNGjjScruOFRrdIdVfVzDkdgujPhA.DeviceDisconnectedEvent += WJDRDixzatdhyFxNbJcpKGWeTpnH;
				zNGjjScruOFRrdIdVfVzDkdgujPhA.UpdateControllerInfoEvent += YfYbJcOQAuhiWGyMHQLBvOqrwbbqA;
				FoarDfUMCtoVFquEtrllUhEjZUUn.lCbtGzMeNwCOvlLkHCUAZALCxwY += CRoJMAeytQLXPMFaQEXNDhkrJLQO;
				FoarDfUMCtoVFquEtrllUhEjZUUn.HBrXrEzBGOMoCLqyxxqezaDoNCZO += KTVqyytqGISutLJQbfhSaKddBlfv.DtcMqkBDUFDNlsQzRBvpRrePYJJT;
				ThreadSafeUnityInput.PostInitialize();
				YWbdYPhUkNsJhuzHqthGhhFSSQAJA();
				ThreadSafeUnityInput.PostInitialize2();
				UbBcKMOxPIbZfdPfMoDLitEDJCuR = UnityTools.GetComponent<UserDataStore>(cDVMhrNPKDHilizCWRqmajBwasQJ);
				if (UbBcKMOxPIbZfdPfMoDLitEDJCuR != null)
				{
					UbBcKMOxPIbZfdPfMoDLitEDJCuR.Initialize();
				}
				YgRkLubWsHosJOiGQLnxxUczEFKR();
				AVPemOcKfkWYczVANhlzVYKIhviJ = false;
				if (vxPlmHYxBOeFdFDgWaImlOtSGVBn)
				{
					Logger.Log("Rewired is running in Edit mode.");
				}
				if (fpFUkdkyTaZDEsPxRFhghAPHRWc != null)
				{
					fpFUkdkyTaZDEsPxRFhghAPHRWc.Invoke();
				}
			}
			catch (Exception)
			{
				inovUwbXLgwsjextWnpkNhsbRCFj = false;
				AVPemOcKfkWYczVANhlzVYKIhviJ = false;
				throw;
			}
			finally
			{
				JIPDJGBKlIrJuAgvqhFDHFZCBNwHB();
			}
		}

		internal static void BiGoqugNtSNAzAgIwoSbZBKRsIfB()
		{
			try
			{
				rUoCEWEVeRGNxHPCVtEemjIeWEhvA();
				if (hSvDRPkauaeZKJDlzLyieMTBASFlB != null)
				{
					hSvDRPkauaeZKJDlzLyieMTBASFlB.mFgzgVCFqSMBYBpaBAjoCKdMnQHG();
				}
				if (configVars.deferControllerConnectedEventsOnStart)
				{
					for (int i = 0; i < FoarDfUMCtoVFquEtrllUhEjZUUn.tMmOaRdQZinbpzIJqOsQpkLeJiKf; i++)
					{
						Joystick joystick = FoarDfUMCtoVFquEtrllUhEjZUUn.vSofOXqlhEhhaEStqlEuVapoCWcU[i];
						GVDsRgaSfUSWjGRpOJtoJIfcCUKX(new ControllerStatusChangedEventArgs(joystick.name, joystick.id, joystick.type));
					}
				}
			}
			finally
			{
				JIPDJGBKlIrJuAgvqhFDHFZCBNwHB();
			}
		}

		internal static void rZuDseFyvaBcTaqsesQNbHrcbudGd(UpdateLoopType P_0)
		{
			if (!inovUwbXLgwsjextWnpkNhsbRCFj)
			{
				return;
			}
			try
			{
				rUoCEWEVeRGNxHPCVtEemjIeWEhvA();
				LrqlfIPpNBQlxQgOXRlAJpZVCAvZ(P_0);
				if ((uint)P_0 <= 1u)
				{
					ydCAHBijPoDCJcHDtRQtahvetMEAc();
				}
			}
			finally
			{
				JIPDJGBKlIrJuAgvqhFDHFZCBNwHB();
			}
		}

		private static void LrqlfIPpNBQlxQgOXRlAJpZVCAvZ(UpdateLoopType P_0)
		{
			if (YNsKnkTRZYIJBWGPKzEDlaqoIbrA != null)
			{
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.tftYhzpkMyTnrDGRtAbEjjCNPGMV();
			}
			Action<UpdateLoopType> action = lmbcFEIsrzugMwBZMmMGGhscrXsO;
			if (action != null)
			{
				try
				{
					action(P_0);
				}
				catch (Exception exception)
				{
					HandleCallbackException("ReInput.BeforeTimeManagerUpdateEvent", exception);
				}
			}
			hSvDRPkauaeZKJDlzLyieMTBASFlB.lwDBSDbpgYGxBgtAiyDGAzaaSLoob(P_0);
		}

		private static void ydCAHBijPoDCJcHDtRQtahvetMEAc()
		{
			int frameCount = Time.frameCount;
			if (qFYFOBhjpaStBAWdPBCGiiHUXUDgA == frameCount)
			{
				return;
			}
			qFYFOBhjpaStBAWdPBCGiiHUXUDgA = frameCount;
			ThreadSafeUnityInput.Update();
			Action action = jRqCytUNSMkiQOIhzHQbFdtopNjo;
			if (action == null)
			{
				return;
			}
			try
			{
				action();
			}
			catch (Exception exception)
			{
				HandleCallbackException("ReInput.EarlyUpdateEvent", exception);
			}
		}

		internal static void pyVUBgGsuenpnCfjOvuGKVUkDznu(UpdateLoopType P_0)
		{
			if (!inovUwbXLgwsjextWnpkNhsbRCFj)
			{
				return;
			}
			try
			{
				rUoCEWEVeRGNxHPCVtEemjIeWEhvA();
				if (hDzkDqfsQiHkoOZgEmvpkDoKtuOE != P_0)
				{
					hDzkDqfsQiHkoOZgEmvpkDoKtuOE = P_0;
				}
				if (editorPlatform != EditorPlatform.None)
				{
					IeahfihEauzEOJPDSsYYdCknikKq = YNsKnkTRZYIJBWGPKzEDlaqoIbrA.QzIqsXbpOGekmiBGLFqApRXgsaqAA.value;
				}
				if (ZoDfMkswRdFTiHHAMvPMWBnixlkoA)
				{
					if (mEEbMtrbvWLYXANsTfSEAKpKWRkL.Update())
					{
						ZoDfMkswRdFTiHHAMvPMWBnixlkoA = false;
						mEEbMtrbvWLYXANsTfSEAKpKWRkL.Clear();
					}
					else
					{
						LtLPnFRiUIztAXIRagmfniCjVdpN.jLowpfofZEkWfXCClnvBCCrJlgHL(P_0);
					}
				}
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.AmoYkDYJkUSqFWmfZXmupuyepfQl();
				Action<UpdateLoopType> dAaZPmEAiBMuILaIxfRQKEijKkLhA = DAaZPmEAiBMuILaIxfRQKEijKkLhA;
				if (dAaZPmEAiBMuILaIxfRQKEijKkLhA != null)
				{
					try
					{
						dAaZPmEAiBMuILaIxfRQKEijKkLhA(P_0);
					}
					catch (Exception exception)
					{
						HandleCallbackException("ReInput.UpdateStartedEvent", exception);
					}
				}
				zNGjjScruOFRrdIdVfVzDkdgujPhA.Update(P_0);
				if (VZDEIGoOcFwVZDpHkUFCQkHjSCZD != null)
				{
					VZDEIGoOcFwVZDpHkUFCQkHjSCZD.Invoke();
				}
				FoarDfUMCtoVFquEtrllUhEjZUUn.GpGfvrbUiJpEQCprYpRlROLBPWqD(P_0);
				Action<UpdateLoopType> action = gxzkypAlObmSKYYoMQPprxFkRSsx;
				if (action != null)
				{
					try
					{
						action(P_0);
						return;
					}
					catch (Exception exception2)
					{
						HandleCallbackException("ReInput.UpdateEndedEvent", exception2);
						return;
					}
				}
			}
			finally
			{
				JIPDJGBKlIrJuAgvqhFDHFZCBNwHB();
			}
		}

		internal static void VdSVLOdsuIIINIqAieoudpWwoKAo()
		{
			Action tnFxAihEUcgcJheqhbGLENJPmjXOA = TnFxAihEUcgcJheqhbGLENJPmjXOA;
			if (tnFxAihEUcgcJheqhbGLENJPmjXOA == null)
			{
				return;
			}
			try
			{
				rUoCEWEVeRGNxHPCVtEemjIeWEhvA();
				tnFxAihEUcgcJheqhbGLENJPmjXOA();
			}
			catch (Exception exception)
			{
				HandleCallbackException("ReInput.LateUpdateEvent", exception);
			}
			finally
			{
				JIPDJGBKlIrJuAgvqhFDHFZCBNwHB();
			}
		}

		[CustomObfuscation(rename = false)]
		internal static void EditorUpdate()
		{
			if (inovUwbXLgwsjextWnpkNhsbRCFj && vxPlmHYxBOeFdFDgWaImlOtSGVBn)
			{
				rZuDseFyvaBcTaqsesQNbHrcbudGd(UpdateLoopType.Update);
				pyVUBgGsuenpnCfjOvuGKVUkDznu(UpdateLoopType.Update);
				VdSVLOdsuIIINIqAieoudpWwoKAo();
			}
		}

		internal static void nEfDdbNdFfuayIferPrVrvGJgHAi()
		{
			if (eCKAQaAGGyaipXtiYemwxFTbZfxd)
			{
				Logger.LogError("You are destroying or disabling the Rewired Input Manager while Rewired is in the middle of its update routine, probably in an event callback. This is inherently unsafe and will result in undefined behavior. You should never do this.");
			}
			if (jIFLtMmDEFvMixTWBElgjonMgZfL != null)
			{
				jIFLtMmDEFvMixTWBElgjonMgZfL.Invoke();
			}
			if (zNGjjScruOFRrdIdVfVzDkdgujPhA != null)
			{
				zNGjjScruOFRrdIdVfVzDkdgujPhA.OnDestroy();
			}
			FOFeLakCFHxrCjFDEgpTwWRMCwae();
			if (lRPURjiPheavHaCbvmRtntVYDzNHA != null)
			{
				lRPURjiPheavHaCbvmRtntVYDzNHA.Invoke();
				lRPURjiPheavHaCbvmRtntVYDzNHA = null;
			}
		}

		internal static void VhMBVNfZYaefnruNYVRBKbWACPdF()
		{
			if (xMhDzdAKXZkrIbLofJLoshCuWICk == null)
			{
				return;
			}
			try
			{
				rUoCEWEVeRGNxHPCVtEemjIeWEhvA();
				xMhDzdAKXZkrIbLofJLoshCuWICk.Invoke();
			}
			finally
			{
				JIPDJGBKlIrJuAgvqhFDHFZCBNwHB();
			}
		}

		internal static void swlDsSHuUJKUqsBFfOdaMtEeYtOP(bool P_0)
		{
			xYguSnTeeRsxZfJtIDfsqfBUgSzd = P_0;
			if (LEjGRGHnhSxXKfKZJqMYCDGjUVBKA == EditorPlatform.None && inovUwbXLgwsjextWnpkNhsbRCFj)
			{
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.rCTftDWWynDPIdXOxJzufkZyAppS.Set(P_0);
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.rCTftDWWynDPIdXOxJzufkZyAppS.TriggerEvent();
			}
		}

		internal static void OYeTRtdljSHbmgluViBLkcUaPDrw(bool P_0)
		{
			if (inovUwbXLgwsjextWnpkNhsbRCFj)
			{
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.fprvKTDGpmgCSdvSGFkvXVgHfahr.Set(P_0);
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.fprvKTDGpmgCSdvSGFkvXVgHfahr.TriggerEvent();
			}
		}

		internal static void JaMoNsWpzEptBLjpjYuwNwuXEaOv()
		{
			if (!inovUwbXLgwsjextWnpkNhsbRCFj)
			{
				return;
			}
			Action lFMjvcXYTCppPfcDTKpKsXtfiybP = LFMjvcXYTCppPfcDTKpKsXtfiybP;
			if (lFMjvcXYTCppPfcDTKpKsXtfiybP == null)
			{
				return;
			}
			try
			{
				lFMjvcXYTCppPfcDTKpKsXtfiybP();
			}
			catch (Exception exception)
			{
				HandleCallbackException("ReInput.SceneLoadedEvent", exception);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static HardwareJoystickMap_InputManager GetHardwareJoystickMap_InputManager(BridgedControllerHWInfo bridgedController)
		{
			return LWRgoiKgeHjHOiswhfZqkdDTTXODA.txSjzVtgvxFSxJFfZsbmLnJmTopH(bridgedController);
		}

		internal static HardwareJoystickMap mPDhHJaciMKPHWhrdsNxKsGzQEcJ(Guid P_0)
		{
			return LWRgoiKgeHjHOiswhfZqkdDTTXODA.GetHardwareJoystickMap(P_0);
		}

		internal static HardwareJoystickTemplateMap aJxRUejDypCmjlkaIdpomVPvZxgi(Guid P_0)
		{
			return LWRgoiKgeHjHOiswhfZqkdDTTXODA.GetJoystickTemplate(P_0);
		}

		internal static KvKIjjJtUTuaYVUulSaPgImHJaaT lsQXmwJMQCkfHSQCVwAraUenyajb(Guid P_0)
		{
			return LWRgoiKgeHjHOiswhfZqkdDTTXODA.dqMGxuKtuLbjVCnceAWmghHJelaNb(P_0);
		}

		internal static IHardwareControllerTemplateMap ReAeRpCbIgpUzZwREYMGbLPpmjRDb(Guid P_0)
		{
			return LWRgoiKgeHjHOiswhfZqkdDTTXODA.GetControllerTemplate(P_0);
		}

		internal static IHardwareControllerTemplateMap IfizLymKkTakdnRWaNLVnCtshNql(Guid P_0)
		{
			return LWRgoiKgeHjHOiswhfZqkdDTTXODA.kEqZRSzZuaBLQpXnjiItHqvyCeFdA(P_0);
		}

		internal static IList<KvKIjjJtUTuaYVUulSaPgImHJaaT> ZIgVwzVQWHAcYVhSlyysyIYalOxM(Guid P_0)
		{
			HardwareJoystickMap hardwareJoystickMap = LWRgoiKgeHjHOiswhfZqkdDTTXODA.GetHardwareJoystickMap(P_0);
			if (hardwareJoystickMap == null)
			{
				return EmptyObjects<KvKIjjJtUTuaYVUulSaPgImHJaaT>.EmptyReadOnlyIListT;
			}
			string[] templateGuidsOrig = hardwareJoystickMap.GetTemplateGuidsOrig();
			if (templateGuidsOrig == null || templateGuidsOrig.Length == 0)
			{
				return EmptyObjects<KvKIjjJtUTuaYVUulSaPgImHJaaT>.EmptyReadOnlyIListT;
			}
			List<KvKIjjJtUTuaYVUulSaPgImHJaaT> list = null;
			for (int i = 0; i < templateGuidsOrig.Length; i++)
			{
				Guid guid;
				try
				{
					guid = new Guid(templateGuidsOrig[i]);
				}
				catch
				{
					Logger.LogWarning("Controller Template GUID is invalid: " + templateGuidsOrig[i]);
					continue;
				}
				KvKIjjJtUTuaYVUulSaPgImHJaaT kvKIjjJtUTuaYVUulSaPgImHJaaT = lsQXmwJMQCkfHSQCVwAraUenyajb(guid);
				if (kvKIjjJtUTuaYVUulSaPgImHJaaT == null)
				{
					Logger.LogWarning("Controller Template was not found for GUID " + guid.ToString());
					continue;
				}
				if (list == null)
				{
					list = new List<KvKIjjJtUTuaYVUulSaPgImHJaaT>();
				}
				ListTools.AddIfUnique(list, kvKIjjJtUTuaYVUulSaPgImHJaaT);
			}
			if (list == null)
			{
				return EmptyObjects<KvKIjjJtUTuaYVUulSaPgImHJaaT>.EmptyReadOnlyIListT;
			}
			return list;
		}

		[CustomObfuscation(rename = false)]
		internal static int GetNewJoystickId()
		{
			return FoarDfUMCtoVFquEtrllUhEjZUUn.OJXbLxHHcgVefAVbbgBrlhzSUErv();
		}

		[CustomObfuscation(rename = false)]
		internal static void HandleCallbackException(string source, Exception exception)
		{
			string msg = "An exception occurred inside an event handler or callback.\nSource: " + source + "\n\nThis happens if your event handler/callback code throws an exception. This means the error is in your code, not Rewired. Read the exception message and the stack trace carefully to find the source of the exception being thrown by your code.\n\nThis can also happen if you forget to unsubscribe to an event in a MonoBehaviour class and that object gets destroyed. Make sure you unsubscribe to events in OnDisable or OnDestroy. Rewired will attempt to continue running.\n\nException:\n" + ((exception.InnerException != null) ? exception.InnerException : exception);
			Logger.LogException((exception.InnerException != null) ? exception.InnerException : exception, msg, requiredThreadSafety: true);
		}

		[CustomObfuscation(rename = false)]
		internal static void HandleExternException(string source, Exception exception)
		{
			Logger.LogException((exception.InnerException != null) ? exception.InnerException : exception, string.Empty, requiredThreadSafety: true);
		}

		[CustomObfuscation(rename = false)]
		internal static void HandleExternalInterfaceException(string source, Exception exception)
		{
			string msg = "An exception occurred inside an external function call.\nSource: " + source + "\n\nThis happens if the external function throws an exception. This could indicate the error is in your code if Rewired is calling a function in an interface implementation you created. Read the exception message and the stack trace carefully to find the source of the exception being thrown.\n\nThis can also happen if you forget to unsubscribe to an event in a MonoBehaviour class and that object gets destroyed.\n\nException:\n" + ((exception.InnerException != null) ? exception.InnerException : exception);
			Logger.LogException((exception.InnerException != null) ? exception.InnerException : exception, msg, requiredThreadSafety: true);
		}

		internal static void etvVyNKCZehEyMowuRqYpWXYbskH()
		{
			if (inovUwbXLgwsjextWnpkNhsbRCFj)
			{
				YgRkLubWsHosJOiGQLnxxUczEFKR();
			}
		}

		[CustomObfuscation(rename = false)]
		internal static void CheckRewiredVersionCompatibility()
		{
			if (UnityTools.unityVersionObj != null && 6000 != UnityTools.unityVersionObj.major)
			{
				QIfsjpnPJaEbRaLDhRNWmQJItkaV();
			}
		}

		internal static float kddaDXMucKDMHHoHoGfEpEaRsVfwA()
		{
			return YNsKnkTRZYIJBWGPKzEDlaqoIbrA.UTXFtWfATWRgjRymvWXWRkQeHmPo.value;
		}

		[CustomObfuscation(rename = false)]
		internal static bool CheckInitialized()
		{
			if (!inovUwbXLgwsjextWnpkNhsbRCFj)
			{
				Logger.LogError("Rewired is not initialized. You must have an active and enabled Rewired Input Manager in the scene before calling any part of the Rewired API.");
				return false;
			}
			return true;
		}

		[CustomObfuscation(rename = false)]
		internal static bool CheckInitialized(int reInputId)
		{
			if (!CheckInitialized())
			{
				return false;
			}
			if (_id != reInputId)
			{
				Logger.LogError("You are attemping to access an object that was created by a previous session or different instance of Rewired and is no longer valid. When Rewired is reset or the Rewired Input Manager is disabled or destroyed, all old object references become invalid and can no longer be used. If you deinitialize Rewired, you cannot use locally stored Rewired objects obtained prior to deinitialization and you must get new objects from the Rewired API.");
				return false;
			}
			return true;
		}

		private static void YWbdYPhUkNsJhuzHqthGhhFSSQAJA()
		{
			KTVqyytqGISutLJQbfhSaKddBlfv.viMGrxkPlOmAmdnLgTNfCNtNoCBKc();
			FoarDfUMCtoVFquEtrllUhEjZUUn.oFhsHQXkmNwYyEyVythlXdYyuaJh(zNGjjScruOFRrdIdVfVzDkdgujPhA.GetInputDataUpdateDelegate(), yFcxJYXlykxThQELuKTiwjJTSDUP.GetInputBehaviors_Copy());
			zNGjjScruOFRrdIdVfVzDkdgujPhA.Initialize();
		}

		private static void FOFeLakCFHxrCjFDEgpTwWRMCwae()
		{
			if (cDVMhrNPKDHilizCWRqmajBwasQJ != null)
			{
				List<IExternalInputManager> componentsInSelfAndChildren = UnityTools.GetComponentsInSelfAndChildren<IExternalInputManager>(cDVMhrNPKDHilizCWRqmajBwasQJ);
				for (int i = 0; i < componentsInSelfAndChildren.Count; i++)
				{
					componentsInSelfAndChildren[i].Deinitialize();
				}
			}
			cDVMhrNPKDHilizCWRqmajBwasQJ = null;
			zNGjjScruOFRrdIdVfVzDkdgujPhA = null;
			stWBKWMOrAnxQyItwkKzVuulIRgF = null;
			if (FoarDfUMCtoVFquEtrllUhEjZUUn != null)
			{
				FoarDfUMCtoVFquEtrllUhEjZUUn.Dispose();
			}
			FoarDfUMCtoVFquEtrllUhEjZUUn = null;
			KTVqyytqGISutLJQbfhSaKddBlfv = null;
			LWRgoiKgeHjHOiswhfZqkdDTTXODA = null;
			if (yFcxJYXlykxThQELuKTiwjJTSDUP != null)
			{
				yFcxJYXlykxThQELuKTiwjJTSDUP.DGtuTHACBgsoyBTMvnQjtQIYnBlt();
			}
			yFcxJYXlykxThQELuKTiwjJTSDUP = null;
			LocalizationHelper.XWQSKMHplnEdgkVcBYaGEfyDfntq();
			GlyphHelper.vPzRRQRnMBsTihxHfMmUuPpkKEJP();
			LocalizationManager.Deinitialize();
			GlyphManager.Deinitialize();
			HulHinCwhKVHDMDuzqbdhtxOhMPn = null;
			inovUwbXLgwsjextWnpkNhsbRCFj = false;
			GUjvmeafVeXwsTBJxzvZxvAaPGij = null;
			hDzkDqfsQiHkoOZgEmvpkDoKtuOE = UpdateLoopType.Update;
			ATiGsPhUPWKdkmfLpiSUBNiehnaxb = false;
			YiqdXGalePxrTOWNrrBdjDnMPESK = Platform.Windows;
			nSCJWWZEcfAdabVylbtGymPFXRrd = WebplayerPlatform.None;
			LEjGRGHnhSxXKfKZJqMYCDGjUVBKA = EditorPlatform.None;
			ZoDfMkswRdFTiHHAMvPMWBnixlkoA = false;
			mEEbMtrbvWLYXANsTfSEAKpKWRkL = null;
			hSvDRPkauaeZKJDlzLyieMTBASFlB = null;
			IeahfihEauzEOJPDSsYYdCknikKq = null;
			uQKkFSljhjsuWoLrgihbcGjKiqbn = false;
			vxPlmHYxBOeFdFDgWaImlOtSGVBn = false;
			xYguSnTeeRsxZfJtIDfsqfBUgSzd = true;
			qFYFOBhjpaStBAWdPBCGiiHUXUDgA = -1;
			_id = -1;
			hwRjxtaUUIaMTBTKOHKSbdMABPKU = 0;
			skUBPwcChrpPuHkaWDgJGsEadpYAc = 0;
			unscaledDeltaTime = 0.0;
			unscaledTime = 0.0;
			unscaledTimePrev = 0.0;
			currentFrame = 0u;
			previousFrame = 0u;
			absFrame = 0u;
			IlvmHErtqEihwCPcEtbQxzIUXLaj.Clear();
			jePogismdTiNgTPlensACEVgcmch.Clear();
			WVHJeNwaDrLTFYHhCXGUxVtDniBG.Clear();
			VZDEIGoOcFwVZDpHkUFCQkHjSCZD.Clear();
			xMhDzdAKXZkrIbLofJLoshCuWICk.Clear();
			_ApplicationFocusChangedEvent = null;
			_ApplicationPauseChangedEvent = null;
			TkzwfvwrPakTRTjueGeVVUIGSSsv = null;
			myGkoLzPwyUJzUsHwPeXByMeeAUF = null;
			OjKFfdgrEOqUiTMwAwpqFvbEcakEb = null;
			ElcQAmgaFoFfUjJcwMkZwClCdpqiA = null;
			jRqCytUNSMkiQOIhzHQbFdtopNjo = null;
			DAaZPmEAiBMuILaIxfRQKEijKkLhA = null;
			gxzkypAlObmSKYYoMQPprxFkRSsx = null;
			TnFxAihEUcgcJheqhbGLENJPmjXOA = null;
			jIFLtMmDEFvMixTWBElgjonMgZfL = null;
			LFMjvcXYTCppPfcDTKpKsXtfiybP = null;
			vfwBoiEWnkPcDieMiMihGvIMYMghA = null;
			KYHSYfxQABnMYMlXIdiNehuunjAMA();
			YNsKnkTRZYIJBWGPKzEDlaqoIbrA = null;
			ThreadSafeUnityInput.Deinitialize();
			if (UnityTools.externalTools != null)
			{
				UnityTools.externalTools.EditorPausedStateChangedEvent -= DXMWgXfJeGywdOsJyaBcUQDYpPvo;
			}
			pvRPaQsIqigEcORkKjmYXGlVnyZO.jGeriDoeSwnLLfJSCNjdJDAAbmJY();
		}

		private static void JZVjFnUOlspogfNSjbOVjWbYyfAfA(string P_0 = null)
		{
			string text = ((P_0 == null) ? "This function" : P_0);
			Logger.LogError(text + " can only be called in Play mode!");
		}

		private static void knWhEoThuJvwMKpsbrLYbADGIRww()
		{
			if (!ZoDfMkswRdFTiHHAMvPMWBnixlkoA)
			{
				ZoDfMkswRdFTiHHAMvPMWBnixlkoA = true;
				LtLPnFRiUIztAXIRagmfniCjVdpN.nDsldkGDaQnVqjfUfNpLkedFdsCV();
				LtLPnFRiUIztAXIRagmfniCjVdpN.PiRrpNiZJeRXJSxwNECWWCmjLjLU();
			}
			mEEbMtrbvWLYXANsTfSEAKpKWRkL.Start();
		}

		private static void axSwwsMdJdcTYbypVNNVuuAtaKOxA()
		{
			Logger.LogError("Rewired is not initialized. Do you have a Rewired Input Manager in the scene and enabled?");
		}

		private static void VhvkucmArxchvYrYSDDacGEoUENe(BridgedController P_0)
		{
			if (P_0.sourceJoystick == null)
			{
				return;
			}
			FoarDfUMCtoVFquEtrllUhEjZUUn.sASGoPhpiZXntIFnupiKVlrNeegWA(P_0);
			Joystick joystick = FoarDfUMCtoVFquEtrllUhEjZUUn.ypqDskzQOCOIVJAyamsKXzVhMSuK(P_0.sourceJoystick.rewiredId);
			if (joystick != null)
			{
				KTVqyytqGISutLJQbfhSaKddBlfv.XYpxgsTENovtMXHsttRwttzGtfJB(joystick);
				if (!configVars.deferControllerConnectedEventsOnStart || !AVPemOcKfkWYczVANhlzVYKIhviJ)
				{
					GVDsRgaSfUSWjGRpOJtoJIfcCUKX(new ControllerStatusChangedEventArgs(joystick.name, joystick.id, joystick.type));
				}
			}
		}

		private static void WJDRDixzatdhyFxNbJcpKGWeTpnH(ControllerDisconnectedEventArgs P_0)
		{
			if (P_0 != null)
			{
				Joystick joystick = FoarDfUMCtoVFquEtrllUhEjZUUn.ypqDskzQOCOIVJAyamsKXzVhMSuK(P_0.rewiredId);
				if (joystick != null)
				{
					FoarDfUMCtoVFquEtrllUhEjZUUn.hNqZsrGJVouRHrpaNkjKKedTLbTi(P_0.rewiredId);
					daxFlYbFnsalbyfJPpPahJKgeYOoA(new ControllerStatusChangedEventArgs(joystick.name, joystick.id, joystick.type));
				}
			}
		}

		private static void GVDsRgaSfUSWjGRpOJtoJIfcCUKX(ControllerStatusChangedEventArgs P_0)
		{
			if (IlvmHErtqEihwCPcEtbQxzIUXLaj != null)
			{
				IlvmHErtqEihwCPcEtbQxzIUXLaj.Invoke(P_0);
			}
		}

		private static void CRoJMAeytQLXPMFaQEXNDhkrJLQO(ControllerStatusChangedEventArgs P_0)
		{
			if (jePogismdTiNgTPlensACEVgcmch != null)
			{
				jePogismdTiNgTPlensACEVgcmch.Invoke(P_0);
			}
		}

		private static void daxFlYbFnsalbyfJPpPahJKgeYOoA(ControllerStatusChangedEventArgs P_0)
		{
			if (WVHJeNwaDrLTFYHhCXGUxVtDniBG != null)
			{
				WVHJeNwaDrLTFYHhCXGUxVtDniBG.Invoke(P_0);
			}
		}

		private static void YfYbJcOQAuhiWGyMHQLBvOqrwbbqA(UpdateControllerInfoEventArgs P_0)
		{
			FoarDfUMCtoVFquEtrllUhEjZUUn.WBFGWdYfgxHoAguKSomSYudqfBve(P_0);
		}

		private static void JXAyaoOMcICTCiMwUamDWWbaMEiOA(bool P_0)
		{
			if (!inovUwbXLgwsjextWnpkNhsbRCFj)
			{
				return;
			}
			Action<bool> applicationFocusChangedEvent = _ApplicationFocusChangedEvent;
			if (applicationFocusChangedEvent == null)
			{
				return;
			}
			try
			{
				applicationFocusChangedEvent(P_0);
			}
			catch (Exception exception)
			{
				HandleCallbackException("ReInput.ApplicationFocusChangedEvent", exception);
			}
		}

		private static void ZoArIFmDNpvsVxJZdRNQzNOWRdEs(bool P_0)
		{
			if (!inovUwbXLgwsjextWnpkNhsbRCFj)
			{
				return;
			}
			Action<bool> applicationPauseChangedEvent = _ApplicationPauseChangedEvent;
			if (applicationPauseChangedEvent == null)
			{
				return;
			}
			try
			{
				applicationPauseChangedEvent(P_0);
			}
			catch (Exception exception)
			{
				HandleCallbackException("ReInput.ApplicationPauseChangedEvent", exception);
			}
		}

		private static void JiidxiWEykzbSlpSgXwYSnFGCMuc(bool P_0)
		{
			Action<bool> tkzwfvwrPakTRTjueGeVVUIGSSsv = TkzwfvwrPakTRTjueGeVVUIGSSsv;
			if (tkzwfvwrPakTRTjueGeVVUIGSSsv != null)
			{
				try
				{
					tkzwfvwrPakTRTjueGeVVUIGSSsv(P_0);
				}
				catch (Exception exception)
				{
					HandleCallbackException("ReInput.ApplicationIsFullScreenChangedEvent", exception);
				}
			}
		}

		private static void OeVFBQIVOXluTskCrszPBEIfHPWD(int P_0)
		{
			if (OjKFfdgrEOqUiTMwAwpqFvbEcakEb != null)
			{
				try
				{
					OjKFfdgrEOqUiTMwAwpqFvbEcakEb((FullScreenMode)P_0);
				}
				catch (Exception exception)
				{
					HandleCallbackException("ReInput.ApplicationFullScreenModeChangedEvent", exception);
				}
			}
		}

		private static void lUiCBeUfRZobpGUMojzCaZcexQqd(bool P_0)
		{
			Action<bool> action = myGkoLzPwyUJzUsHwPeXByMeeAUF;
			if (action != null)
			{
				try
				{
					action(P_0);
				}
				catch (Exception exception)
				{
					HandleCallbackException("ReInput.ApplicationRunInBackgroundChangedEvent", exception);
				}
			}
		}

		private static void hfUYBFQlwNzIivUbqTztYUrGtIuK(bool P_0)
		{
			hwRjxtaUUIaMTBTKOHKSbdMABPKU++;
			Action<bool> elcQAmgaFoFfUjJcwMkZwClCdpqiA = ElcQAmgaFoFfUjJcwMkZwClCdpqiA;
			if (elcQAmgaFoFfUjJcwMkZwClCdpqiA != null)
			{
				try
				{
					elcQAmgaFoFfUjJcwMkZwClCdpqiA(P_0);
				}
				catch (Exception exception)
				{
					HandleCallbackException("ReInput.TimeScalePauseChangedEvent", exception);
				}
			}
		}

		private static void TteyEJVBkWuPjNrTNtPZotBTLjRO()
		{
			if (YNsKnkTRZYIJBWGPKzEDlaqoIbrA != null)
			{
				KYHSYfxQABnMYMlXIdiNehuunjAMA();
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.rCTftDWWynDPIdXOxJzufkZyAppS.ChangedEvent += JXAyaoOMcICTCiMwUamDWWbaMEiOA;
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.fprvKTDGpmgCSdvSGFkvXVgHfahr.ChangedEvent += ZoArIFmDNpvsVxJZdRNQzNOWRdEs;
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.DorhhtzMOCBEoCAFbgkcdflbyOgxb.ChangedEvent += JiidxiWEykzbSlpSgXwYSnFGCMuc;
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.zPoAsRMylaokWCNvjIrKsKUbDctX.ChangedEvent += lUiCBeUfRZobpGUMojzCaZcexQqd;
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.bZykPhWdHEGTeESTOVyggnACZscX.ChangedEvent += OeVFBQIVOXluTskCrszPBEIfHPWD;
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.FatGjtZSkCjvOinZoTxDBsswvbWi.ChangedEvent += hfUYBFQlwNzIivUbqTztYUrGtIuK;
			}
		}

		private static void KYHSYfxQABnMYMlXIdiNehuunjAMA()
		{
			if (YNsKnkTRZYIJBWGPKzEDlaqoIbrA != null)
			{
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.rCTftDWWynDPIdXOxJzufkZyAppS.ChangedEvent -= JXAyaoOMcICTCiMwUamDWWbaMEiOA;
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.fprvKTDGpmgCSdvSGFkvXVgHfahr.ChangedEvent -= ZoArIFmDNpvsVxJZdRNQzNOWRdEs;
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.DorhhtzMOCBEoCAFbgkcdflbyOgxb.ChangedEvent -= JiidxiWEykzbSlpSgXwYSnFGCMuc;
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.zPoAsRMylaokWCNvjIrKsKUbDctX.ChangedEvent -= lUiCBeUfRZobpGUMojzCaZcexQqd;
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.bZykPhWdHEGTeESTOVyggnACZscX.ChangedEvent -= OeVFBQIVOXluTskCrszPBEIfHPWD;
				YNsKnkTRZYIJBWGPKzEDlaqoIbrA.FatGjtZSkCjvOinZoTxDBsswvbWi.ChangedEvent -= hfUYBFQlwNzIivUbqTztYUrGtIuK;
			}
		}

		private static void DXMWgXfJeGywdOsJyaBcUQDYpPvo(bool P_0)
		{
			Action<bool> action = vfwBoiEWnkPcDieMiMihGvIMYMghA;
			if (action != null)
			{
				try
				{
					action(P_0);
				}
				catch (Exception exception)
				{
					HandleCallbackException("ReInput.EditorPauseChangedEvent", exception);
				}
			}
		}

		private static void AroNjGKbBsWJpndNHeRFYEesRbok(Func<ConfigVars, object> P_0, UnityTools.StIgCRgsklQdBuXGCpKvcsOPHcHhb P_1, Action<Platform> P_2)
		{
			bool flag = false;
			if (P_1.XpzZdnZcPjZUByzkmiNKqvhIqRCl != P_1.xGmlutjllQnviLVMIarCiAObUTWp)
			{
				UnityTools.StIgCRgsklQdBuXGCpKvcsOPHcHhb stIgCRgsklQdBuXGCpKvcsOPHcHhb = P_1;
				stIgCRgsklQdBuXGCpKvcsOPHcHhb.XpzZdnZcPjZUByzkmiNKqvhIqRCl = P_1.xGmlutjllQnviLVMIarCiAObUTWp;
				UnityTools.dSQRBxIlyXdsgFrhJxqIxgzHKZgB(stIgCRgsklQdBuXGCpKvcsOPHcHhb);
				P_2(stIgCRgsklQdBuXGCpKvcsOPHcHhb.xGmlutjllQnviLVMIarCiAObUTWp);
				DFrVuEnOkCrEtWOSASAkrDgCBzse();
				flag = true;
			}
			if (!configVars.DoesPlatformUseFallback(P_1.xGmlutjllQnviLVMIarCiAObUTWp, P_1.arcPZqDekeaEgvAgMCjrBSNoKzKh, isEditor) && !configVars.DoesPlatformUseFallback(P_1.XpzZdnZcPjZUByzkmiNKqvhIqRCl, P_1.arcPZqDekeaEgvAgMCjrBSNoKzKh, isEditor))
			{
				List<IExternalInputManager> componentsInSelfAndChildren = UnityTools.GetComponentsInSelfAndChildren<IExternalInputManager>(cDVMhrNPKDHilizCWRqmajBwasQJ);
				for (int i = 0; i < componentsInSelfAndChildren.Count; i++)
				{
					if (componentsInSelfAndChildren[i].Initialize(P_1.xGmlutjllQnviLVMIarCiAObUTWp, GUjvmeafVeXwsTBJxzvZxvAaPGij) is PlatformInputManager platformInputManager)
					{
						zNGjjScruOFRrdIdVfVzDkdgujPhA = platformInputManager;
						return;
					}
				}
			}
			if (flag)
			{
				UnityTools.dSQRBxIlyXdsgFrhJxqIxgzHKZgB(P_1);
				P_2(P_1.xGmlutjllQnviLVMIarCiAObUTWp);
				DFrVuEnOkCrEtWOSASAkrDgCBzse();
				flag = false;
			}
			if (configVars.DoesPlatformUseFallback(YiqdXGalePxrTOWNrrBdjDnMPESK, nSCJWWZEcfAdabVylbtGymPFXRrd, isEditor))
			{
				ATiGsPhUPWKdkmfLpiSUBNiehnaxb = true;
				zNGjjScruOFRrdIdVfVzDkdgujPhA = new pWRDtMxMZRbTxFlbqpruZytnvSMMA(GUjvmeafVeXwsTBJxzvZxvAaPGij.updateLoop);
			}
			else if (YiqdXGalePxrTOWNrrBdjDnMPESK == Platform.Windows || YiqdXGalePxrTOWNrrBdjDnMPESK == Platform.WindowsAppStore || YiqdXGalePxrTOWNrrBdjDnMPESK == Platform.WindowsUWP || YiqdXGalePxrTOWNrrBdjDnMPESK == Platform.OSX || YiqdXGalePxrTOWNrrBdjDnMPESK == Platform.Linux)
			{
				zNGjjScruOFRrdIdVfVzDkdgujPhA = P_0(GUjvmeafVeXwsTBJxzvZxvAaPGij) as PlatformInputManager;
			}
			else if (YiqdXGalePxrTOWNrrBdjDnMPESK == Platform.WebGL && !isEditor)
			{
				try
				{
					zNGjjScruOFRrdIdVfVzDkdgujPhA = P_0(GUjvmeafVeXwsTBJxzvZxvAaPGij) as PlatformInputManager;
					if (zNGjjScruOFRrdIdVfVzDkdgujPhA == null)
					{
						throw new Exception();
					}
				}
				catch
				{
					Logger.LogError("WebGL platform could not be initialized! Is the Rewired WebGL library installed? See the documentation for more information.");
					zNGjjScruOFRrdIdVfVzDkdgujPhA = null;
				}
			}
			else if (YiqdXGalePxrTOWNrrBdjDnMPESK == Platform.XboxOne && !isEditor)
			{
				try
				{
					zNGjjScruOFRrdIdVfVzDkdgujPhA = new CustomInputManager(new XboxOneInputSource(), GUjvmeafVeXwsTBJxzvZxvAaPGij.updateLoop, GetHardwareJoystickMap_InputManager, GetNewJoystickId);
					if (zNGjjScruOFRrdIdVfVzDkdgujPhA == null)
					{
						throw new Exception();
					}
				}
				catch
				{
					Logger.LogError("Xbox One platform could not be initialized! Is the Rewired Xbox One library installed? See the documentation for more information.");
					zNGjjScruOFRrdIdVfVzDkdgujPhA = null;
				}
			}
			else if (YiqdXGalePxrTOWNrrBdjDnMPESK == Platform.PS4 && !isEditor)
			{
				try
				{
					zNGjjScruOFRrdIdVfVzDkdgujPhA = P_0(GUjvmeafVeXwsTBJxzvZxvAaPGij) as PlatformInputManager;
					if (zNGjjScruOFRrdIdVfVzDkdgujPhA == null)
					{
						throw new Exception("Input Manager was null.");
					}
				}
				catch (Exception msg)
				{
					Logger.LogError("PS4 platform could not be initialized! Is the Rewired PS4 plugin installed? See the documentation for more information.");
					Logger.LogError(msg);
					zNGjjScruOFRrdIdVfVzDkdgujPhA = null;
				}
			}
			else if (YiqdXGalePxrTOWNrrBdjDnMPESK == Platform.PS5 && !isEditor)
			{
				try
				{
					zNGjjScruOFRrdIdVfVzDkdgujPhA = P_0(GUjvmeafVeXwsTBJxzvZxvAaPGij) as PlatformInputManager;
					if (zNGjjScruOFRrdIdVfVzDkdgujPhA == null)
					{
						throw new Exception("Input Manager was null.");
					}
				}
				catch (Exception msg2)
				{
					Logger.LogError("PS5 platform could not be initialized! Is the Rewired PS5 plugin installed? See the documentation for more information.");
					Logger.LogError(msg2);
					zNGjjScruOFRrdIdVfVzDkdgujPhA = null;
				}
			}
			else if ((YiqdXGalePxrTOWNrrBdjDnMPESK == Platform.GameCoreXboxOne || YiqdXGalePxrTOWNrrBdjDnMPESK == Platform.GameCoreScarlett) && !isEditor)
			{
				try
				{
					zNGjjScruOFRrdIdVfVzDkdgujPhA = P_0(GUjvmeafVeXwsTBJxzvZxvAaPGij) as PlatformInputManager;
					if (zNGjjScruOFRrdIdVfVzDkdgujPhA == null)
					{
						throw new Exception("Input Manager was null.");
					}
				}
				catch (Exception msg3)
				{
					string text = ((YiqdXGalePxrTOWNrrBdjDnMPESK == Platform.GameCoreXboxOne) ? "Xbox One" : "Xbox Series X");
					Logger.LogError(text + " platform could not be initialized! Is the Rewired " + text + " library installed? See the documentation for more information.");
					Logger.LogError(msg3);
					zNGjjScruOFRrdIdVfVzDkdgujPhA = null;
				}
			}
			else if (YiqdXGalePxrTOWNrrBdjDnMPESK == Platform.Ouya && !isEditor)
			{
				Logger.LogError("Ouya is no longer supported.");
				zNGjjScruOFRrdIdVfVzDkdgujPhA = null;
			}
			else if (UnityTools.isAndroidPlatform && !isEditor)
			{
				try
				{
					UnityTools.xtkrcfYiRlMAbbhtyriicjfJbkdk = P_0(GUjvmeafVeXwsTBJxzvZxvAaPGij) as IAndroidFallbackPlatformHelper;
				}
				catch (Exception msg4)
				{
					Logger.LogError(msg4);
				}
			}
			else if (YiqdXGalePxrTOWNrrBdjDnMPESK == Platform.Custom)
			{
				try
				{
					zNGjjScruOFRrdIdVfVzDkdgujPhA = new CustomInputManager(pvRPaQsIqigEcORkKjmYXGlVnyZO.WnzJlECbJGuczDNuGNIGRlfINCnN(), GUjvmeafVeXwsTBJxzvZxvAaPGij.updateLoop, GetHardwareJoystickMap_InputManager, GetNewJoystickId);
					if (zNGjjScruOFRrdIdVfVzDkdgujPhA == null)
					{
						throw new Exception();
					}
				}
				catch
				{
					Logger.LogError("Custom platform could not be initialized due to an exception!");
					zNGjjScruOFRrdIdVfVzDkdgujPhA = null;
					throw;
				}
			}
			if (zNGjjScruOFRrdIdVfVzDkdgujPhA == null)
			{
				ATiGsPhUPWKdkmfLpiSUBNiehnaxb = true;
				zNGjjScruOFRrdIdVfVzDkdgujPhA = new pWRDtMxMZRbTxFlbqpruZytnvSMMA(GUjvmeafVeXwsTBJxzvZxvAaPGij.updateLoop);
			}
		}

		private static void YgRkLubWsHosJOiGQLnxxUczEFKR()
		{
			if (uQKkFSljhjsuWoLrgihbcGjKiqbn != GUjvmeafVeXwsTBJxzvZxvAaPGij.GetPlatformVar_ignoreInputWhenAppNotInFocus())
			{
				uQKkFSljhjsuWoLrgihbcGjKiqbn = !uQKkFSljhjsuWoLrgihbcGjKiqbn;
			}
		}

		private static void QIfsjpnPJaEbRaLDhRNWmQJItkaV()
		{
			if (!(UnityTools.unityVersionObj == null))
			{
				string[] obj = new string[7] { "The version of Rewired installed (", programVersion, ") was not designed for Unity ", null, null, null, null };
				int major = UnityTools.unityVersionObj.major;
				obj[3] = major.ToString();
				obj[4] = ". Please install Rewired for Unity ";
				major = UnityTools.unityVersionObj.major;
				obj[5] = major.ToString();
				obj[6] = ".\n\nThis warning does not mean that Rewired will not function, but it may not function optimally.\n\nSome different major versions of Unity download Asset Store assets to the same folder location on disk, so if you download an asset in one version of the Unity editor, then open another version of the Unity editor and install the asset without re-downloading it, the wrong asset version will be installed. To fix this, manually re-download Rewired in the Unity Asset Store panel in this version of the Unity Editor, then install it.\n\nIf you are using a beta version of a new major version of Unity, you will have to wait until the release of the final version before a compatible version of Rewired can be uploaded to the Asset Store. When the new version is ready, it will be available through the Unity Asset Store for download as usual.";
				Logger.LogWarning(string.Concat(obj));
			}
		}
	}
}
