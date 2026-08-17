using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Rewired.Config;
using Rewired.Data;
using Rewired.Data.Mapping;
using Rewired.InputManagers;
using Rewired.Interfaces;
using Rewired.Platforms;
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
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public sealed class ConfigHelper : CodeHelper
		{
			private static ConfigHelper LDnMtBpByMhNcmNpPzdzLRPijZRi;

			private float rcFjLisvbvYtJGWpnKEYgFUHqZfd = 0.7f;

			private float UJqtCyevQhANodQxANgKNPSQEzWI = 100f;

			internal static ConfigHelper izfChqvjKyhCcypSioMPzwuCmhEV => LDnMtBpByMhNcmNpPzdzLRPijZRi ?? (LDnMtBpByMhNcmNpPzdzLRPijZRi = new ConfigHelper());

			public bool useXInput
			{
				get
				{
					if (!CheckInitialized())
					{
						return false;
					}
					if (UnityTools.platform == Platform.Windows && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.windowsStandalonePrimaryInputSource == WindowsStandalonePrimaryInputSource.XInput)
					{
						return true;
					}
					if (UnityTools.platform == Platform.WindowsUWP)
					{
						return (afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.GetPlatformVars(Platform.WindowsUWP) as ConfigVars.PlatformVars_WindowsUWP).useGamepadAPI;
					}
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.useXInput;
				}
				set
				{
					if (!CheckInitialized())
					{
						return;
					}
					if (UnityTools.platform == Platform.WindowsUWP)
					{
						ConfigVars.PlatformVars_WindowsUWP platformVars_WindowsUWP = afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.GetPlatformVars(Platform.WindowsUWP) as ConfigVars.PlatformVars_WindowsUWP;
						if (platformVars_WindowsUWP.useGamepadAPI != value)
						{
							platformVars_WindowsUWP.useGamepadAPI = value;
							if (HvlowarpEJjTwmVIKndWValQpypI != null)
							{
								HvlowarpEJjTwmVIKndWValQpypI.ResetAll();
							}
						}
					}
					else if (afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.useXInput != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.useXInput = value;
						if (!value && UnityTools.platform == Platform.Windows && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.windowsStandalonePrimaryInputSource == WindowsStandalonePrimaryInputSource.XInput)
						{
							windowsStandalonePrimaryInputSource = WindowsStandalonePrimaryInputSource.RawInput;
							Logger.Log("The primary input source has been changed to Raw Input.");
						}
						else if (HvlowarpEJjTwmVIKndWValQpypI != null)
						{
							HvlowarpEJjTwmVIKndWValQpypI.ResetAll();
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.updateLoop;
				}
				set
				{
					if (CheckInitialized() && value != afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.updateLoop)
					{
						if ((value & UpdateLoopSetting.Update) == 0)
						{
							value |= UpdateLoopSetting.Update;
						}
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.updateLoop = value;
						if (HvlowarpEJjTwmVIKndWValQpypI != null)
						{
							HvlowarpEJjTwmVIKndWValQpypI.ResetAll();
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.windowsStandalonePrimaryInputSource;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.windowsStandalonePrimaryInputSource != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.windowsStandalonePrimaryInputSource = value;
						if (UnityTools.platform == Platform.Windows && value == WindowsStandalonePrimaryInputSource.XInput)
						{
							afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.useXInput = true;
						}
						if (HvlowarpEJjTwmVIKndWValQpypI != null)
						{
							HvlowarpEJjTwmVIKndWValQpypI.ResetAll();
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.osx_primaryInputSource;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.osx_primaryInputSource != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.osx_primaryInputSource = value;
						if (HvlowarpEJjTwmVIKndWValQpypI != null)
						{
							HvlowarpEJjTwmVIKndWValQpypI.ResetAll();
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.linux_primaryInputSource;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.linux_primaryInputSource != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.linux_primaryInputSource = value;
						if (HvlowarpEJjTwmVIKndWValQpypI != null)
						{
							HvlowarpEJjTwmVIKndWValQpypI.ResetAll();
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.windowsUWP_primaryInputSource;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.windowsUWP_primaryInputSource != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.windowsUWP_primaryInputSource = value;
						if (HvlowarpEJjTwmVIKndWValQpypI != null)
						{
							HvlowarpEJjTwmVIKndWValQpypI.ResetAll();
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
					return (afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.GetPlatformVars(Platform.WindowsUWP) as ConfigVars.PlatformVars_WindowsUWP).useHIDAPI;
				}
				set
				{
					if (!CheckInitialized())
					{
						return;
					}
					ConfigVars.PlatformVars_WindowsUWP platformVars_WindowsUWP = afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.GetPlatformVars(Platform.WindowsUWP) as ConfigVars.PlatformVars_WindowsUWP;
					if (platformVars_WindowsUWP.useHIDAPI != value)
					{
						platformVars_WindowsUWP.useHIDAPI = value;
						if (HvlowarpEJjTwmVIKndWValQpypI != null)
						{
							HvlowarpEJjTwmVIKndWValQpypI.ResetAll();
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.xboxOne_primaryInputSource;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.xboxOne_primaryInputSource != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.xboxOne_primaryInputSource = value;
						if (HvlowarpEJjTwmVIKndWValQpypI != null)
						{
							HvlowarpEJjTwmVIKndWValQpypI.ResetAll();
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.ps4_primaryInputSource;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.ps4_primaryInputSource != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.ps4_primaryInputSource = value;
						if (HvlowarpEJjTwmVIKndWValQpypI != null)
						{
							HvlowarpEJjTwmVIKndWValQpypI.ResetAll();
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.webGL_primaryInputSource;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.webGL_primaryInputSource != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.webGL_primaryInputSource = value;
						if (HvlowarpEJjTwmVIKndWValQpypI != null)
						{
							HvlowarpEJjTwmVIKndWValQpypI.ResetAll();
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.alwaysUseUnityInput;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.alwaysUseUnityInput != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.alwaysUseUnityInput = value;
						if (HvlowarpEJjTwmVIKndWValQpypI != null)
						{
							HvlowarpEJjTwmVIKndWValQpypI.ResetAll();
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.GetPlatformVar_useNativeMouse();
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.SetPlatformVar_useNativeMouse(value) && HvlowarpEJjTwmVIKndWValQpypI != null)
					{
						HvlowarpEJjTwmVIKndWValQpypI.ResetAll();
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.GetPlatformVar_useNativeKeyboard();
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.SetPlatformVar_useNativeKeyboard(value) && HvlowarpEJjTwmVIKndWValQpypI != null)
					{
						HvlowarpEJjTwmVIKndWValQpypI.ResetAll();
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.GetPlatformVar_useEnhancedDeviceSupport();
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.SetPlatformVar_useEnhancedDeviceSupport(value) && HvlowarpEJjTwmVIKndWValQpypI != null)
					{
						HvlowarpEJjTwmVIKndWValQpypI.ResetAll();
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.GetPlatformVar_joystickRefreshRate();
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
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.SetPlatformVar_joystickRefreshRate(value);
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.GetPlatformVar_ignoreInputWhenAppNotInFocus();
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.SetPlatformVar_ignoreInputWhenAppNotInFocus(value))
					{
						iaFqwxjsRihFAZTTzjugnSXKeOgL();
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.android_supportUnknownGamepads;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.android_supportUnknownGamepads != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.android_supportUnknownGamepads = value;
						if (HvlowarpEJjTwmVIKndWValQpypI != null)
						{
							HvlowarpEJjTwmVIKndWValQpypI.ResetAll();
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.defaultJoystickAxis2DDeadZoneType;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.defaultJoystickAxis2DDeadZoneType != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.defaultJoystickAxis2DDeadZoneType = value;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.defaultJoystickAxis2DSensitivityType;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.defaultJoystickAxis2DSensitivityType != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.defaultJoystickAxis2DSensitivityType = value;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.defaultAxisSensitivityType;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.defaultAxisSensitivityType != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.defaultAxisSensitivityType = value;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.force4WayHats;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.force4WayHats != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.force4WayHats = value;
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
					return rcFjLisvbvYtJGWpnKEYgFUHqZfd;
				}
				set
				{
					if (CheckInitialized())
					{
						if (value < 0f)
						{
							value = 0f;
						}
						if (rcFjLisvbvYtJGWpnKEYgFUHqZfd != value)
						{
							rcFjLisvbvYtJGWpnKEYgFUHqZfd = value;
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
					return UJqtCyevQhANodQxANgKNPSQEzWI;
				}
				set
				{
					if (CheckInitialized())
					{
						if (value < 0f)
						{
							value = 0f;
						}
						if (UJqtCyevQhANodQxANgKNPSQEzWI != value)
						{
							UJqtCyevQhANodQxANgKNPSQEzWI = value;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.activateActionButtonsOnNegativeValue;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.activateActionButtonsOnNegativeValue != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.activateActionButtonsOnNegativeValue = value;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.throttleCalibrationMode;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.throttleCalibrationMode != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.throttleCalibrationMode = value;
						VmqcbbbvPImXBEBcMHWjUAXVfrUSA.bScBilCiPympXpnYaJzxzFWvePuLA(value);
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.deferControllerConnectedEventsOnStart;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.deferControllerConnectedEventsOnStart != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.deferControllerConnectedEventsOnStart = value;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.autoAssignJoysticks;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.autoAssignJoysticks != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.autoAssignJoysticks = value;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.maxJoysticksPerPlayer;
				}
				set
				{
					if (CheckInitialized())
					{
						if (value < 1)
						{
							value = 1;
						}
						if (afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.maxJoysticksPerPlayer != value)
						{
							afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.maxJoysticksPerPlayer = value;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.distributeJoysticksEvenly;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.distributeJoysticksEvenly != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.distributeJoysticksEvenly = value;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.assignJoysticksToPlayingPlayersOnly;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.assignJoysticksToPlayingPlayersOnly != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.assignJoysticksToPlayingPlayersOnly = value;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.reassignJoystickToPreviousOwnerOnReconnect;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.reassignJoystickToPreviousOwnerOnReconnect != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.reassignJoystickToPreviousOwnerOnReconnect = value;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.logLevel;
				}
				set
				{
					if (CheckInitialized() && afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.logLevel != value)
					{
						afarNIRJlvcfXIrnUwLxEgdcbDEjA.ConfigVars.logLevel = value;
					}
				}
			}

			private ConfigHelper()
			{
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public sealed class ControllerHelper : CodeHelper
		{
			[EditorBrowsable(EditorBrowsableState.Never)]
			[Browsable(false)]
			public sealed class PollingHelper : CodeHelper
			{
				private sealed class tDTFBDdEcQFfhUOSSEGcGWgTBMKNA : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					public PollingHelper TtytLoUfsgUyhsklaKccrnoMiiek;

					private IEnumerator<ControllerPollingInfo> hSeONskmQJhVsjVUHGUASYbDGJrU;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public tDTFBDdEcQFfhUOSSEGcGWgTBMKNA(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						switch (RxAoyfYzYDsYonLGXsvUgwChukLk)
						{
						case -3:
						case 1:
							try
							{
								break;
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						case -4:
						case 2:
							try
							{
								break;
							}
							finally
							{
								tMeeFNDmLhzrFkrObkICOsBGeJux();
							}
						case -5:
						case 3:
							try
							{
								break;
							}
							finally
							{
								bKYtFpdOyyopwWDgXevZsgrxWQOs();
							}
						case -2:
						case -1:
						case 0:
							break;
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							PollingHelper ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
							switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
							{
							default:
								return false;
							case 0:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
								if (!CheckInitialized())
								{
									return false;
								}
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.zwNhHIcIgtilZkHFNoLdYkBpudmR().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0084;
							case 1:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0084;
							case 2:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -4;
								goto IL_00e4;
							case 3:
								{
									RxAoyfYzYDsYonLGXsvUgwChukLk = -5;
									break;
								}
								IL_00e4:
								if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
								{
									ControllerPollingInfo current = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
									VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
									RxAoyfYzYDsYonLGXsvUgwChukLk = 2;
									return true;
								}
								tMeeFNDmLhzrFkrObkICOsBGeJux();
								hSeONskmQJhVsjVUHGUASYbDGJrU = null;
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.VJeweIaiXWbvCcIFICQocFzFqnbpB().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -5;
								break;
								IL_0084:
								if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
								{
									ControllerPollingInfo current2 = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
									VqEePGSMyrGKIqkWibsjeHcWPSIx = current2;
									RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
									return true;
								}
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
								hSeONskmQJhVsjVUHGUASYbDGJrU = null;
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.xLpJNhMMqKQpDjLnVPCaMhTCFhvDA().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -4;
								goto IL_00e4;
							}
							if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
							{
								ControllerPollingInfo current3 = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current3;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 3;
								return true;
							}
							bKYtFpdOyyopwWDgXevZsgrxWQOs();
							hSeONskmQJhVsjVUHGUASYbDGJrU = null;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
						}
					}

					private void tMeeFNDmLhzrFkrObkICOsBGeJux()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
						}
					}

					private void bKYtFpdOyyopwWDgXevZsgrxWQOs()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
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
						tDTFBDdEcQFfhUOSSEGcGWgTBMKNA tDTFBDdEcQFfhUOSSEGcGWgTBMKNA2;
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							tDTFBDdEcQFfhUOSSEGcGWgTBMKNA2 = this;
						}
						else
						{
							tDTFBDdEcQFfhUOSSEGcGWgTBMKNA2 = new tDTFBDdEcQFfhUOSSEGcGWgTBMKNA(0);
							tDTFBDdEcQFfhUOSSEGcGWgTBMKNA2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
						}
						return tDTFBDdEcQFfhUOSSEGcGWgTBMKNA2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class VTmCTECfrLOpWyDjTbthIzGUmvdv : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					public PollingHelper TtytLoUfsgUyhsklaKccrnoMiiek;

					private IEnumerator<ControllerPollingInfo> hSeONskmQJhVsjVUHGUASYbDGJrU;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public VTmCTECfrLOpWyDjTbthIzGUmvdv(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						switch (RxAoyfYzYDsYonLGXsvUgwChukLk)
						{
						case -3:
						case 1:
							try
							{
								break;
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						case -4:
						case 2:
							try
							{
								break;
							}
							finally
							{
								tMeeFNDmLhzrFkrObkICOsBGeJux();
							}
						case -5:
						case 3:
							try
							{
								break;
							}
							finally
							{
								bKYtFpdOyyopwWDgXevZsgrxWQOs();
							}
						case -6:
						case 4:
							try
							{
								break;
							}
							finally
							{
								voQyPKUnPshXZdFCyWVzLjeZHBAs();
							}
						case -2:
						case -1:
						case 0:
							break;
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							PollingHelper ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
							switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
							{
							default:
								return false;
							case 0:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
								if (!CheckInitialized())
								{
									return false;
								}
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.sszMazZiRlLXifusuhSvrXruVsvS().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0088;
							case 1:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0088;
							case 2:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -4;
								goto IL_00e8;
							case 3:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -5;
								goto IL_0148;
							case 4:
								{
									RxAoyfYzYDsYonLGXsvUgwChukLk = -6;
									break;
								}
								IL_00e8:
								if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
								{
									ControllerPollingInfo current = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
									VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
									RxAoyfYzYDsYonLGXsvUgwChukLk = 2;
									return true;
								}
								tMeeFNDmLhzrFkrObkICOsBGeJux();
								hSeONskmQJhVsjVUHGUASYbDGJrU = null;
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.HKfvIjVyGzhVyNzzmHuXrPXdNtwB().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -5;
								goto IL_0148;
								IL_0088:
								if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
								{
									ControllerPollingInfo current2 = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
									VqEePGSMyrGKIqkWibsjeHcWPSIx = current2;
									RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
									return true;
								}
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
								hSeONskmQJhVsjVUHGUASYbDGJrU = null;
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.HOEJAMRWlNjoQHyywIgOAMoSqKTE().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -4;
								goto IL_00e8;
								IL_0148:
								if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
								{
									ControllerPollingInfo current3 = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
									VqEePGSMyrGKIqkWibsjeHcWPSIx = current3;
									RxAoyfYzYDsYonLGXsvUgwChukLk = 3;
									return true;
								}
								bKYtFpdOyyopwWDgXevZsgrxWQOs();
								hSeONskmQJhVsjVUHGUASYbDGJrU = null;
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.hTjfFKtRwBiwDyrjjHQnZaVkwhIc().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -6;
								break;
							}
							if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
							{
								ControllerPollingInfo current4 = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current4;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 4;
								return true;
							}
							voQyPKUnPshXZdFCyWVzLjeZHBAs();
							hSeONskmQJhVsjVUHGUASYbDGJrU = null;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
						}
					}

					private void tMeeFNDmLhzrFkrObkICOsBGeJux()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
						}
					}

					private void bKYtFpdOyyopwWDgXevZsgrxWQOs()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
						}
					}

					private void voQyPKUnPshXZdFCyWVzLjeZHBAs()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
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
						VTmCTECfrLOpWyDjTbthIzGUmvdv vTmCTECfrLOpWyDjTbthIzGUmvdv;
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							vTmCTECfrLOpWyDjTbthIzGUmvdv = this;
						}
						else
						{
							vTmCTECfrLOpWyDjTbthIzGUmvdv = new VTmCTECfrLOpWyDjTbthIzGUmvdv(0);
							vTmCTECfrLOpWyDjTbthIzGUmvdv.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
						}
						return vTmCTECfrLOpWyDjTbthIzGUmvdv;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class DHJqrAIJpUnVHhVnHAUqkBCLrIqg : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					public PollingHelper TtytLoUfsgUyhsklaKccrnoMiiek;

					private IEnumerator<ControllerPollingInfo> hSeONskmQJhVsjVUHGUASYbDGJrU;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public DHJqrAIJpUnVHhVnHAUqkBCLrIqg(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						switch (RxAoyfYzYDsYonLGXsvUgwChukLk)
						{
						case -3:
						case 1:
							try
							{
								break;
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						case -4:
						case 2:
							try
							{
								break;
							}
							finally
							{
								tMeeFNDmLhzrFkrObkICOsBGeJux();
							}
						case -5:
						case 3:
							try
							{
								break;
							}
							finally
							{
								bKYtFpdOyyopwWDgXevZsgrxWQOs();
							}
						case -6:
						case 4:
							try
							{
								break;
							}
							finally
							{
								voQyPKUnPshXZdFCyWVzLjeZHBAs();
							}
						case -2:
						case -1:
						case 0:
							break;
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							PollingHelper ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
							switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
							{
							default:
								return false;
							case 0:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
								if (!CheckInitialized())
								{
									return false;
								}
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.ZqxTJDIDYhzvQgREIWmdhRCmoMej().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0088;
							case 1:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0088;
							case 2:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -4;
								goto IL_00e8;
							case 3:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -5;
								goto IL_0148;
							case 4:
								{
									RxAoyfYzYDsYonLGXsvUgwChukLk = -6;
									break;
								}
								IL_00e8:
								if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
								{
									ControllerPollingInfo current = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
									VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
									RxAoyfYzYDsYonLGXsvUgwChukLk = 2;
									return true;
								}
								tMeeFNDmLhzrFkrObkICOsBGeJux();
								hSeONskmQJhVsjVUHGUASYbDGJrU = null;
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.vjoctRbePlNRDLebnEgMQAlXsvCEA().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -5;
								goto IL_0148;
								IL_0088:
								if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
								{
									ControllerPollingInfo current2 = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
									VqEePGSMyrGKIqkWibsjeHcWPSIx = current2;
									RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
									return true;
								}
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
								hSeONskmQJhVsjVUHGUASYbDGJrU = null;
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.nTOaOzAizIVxHePeOnxkRHlAxAWS().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -4;
								goto IL_00e8;
								IL_0148:
								if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
								{
									ControllerPollingInfo current3 = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
									VqEePGSMyrGKIqkWibsjeHcWPSIx = current3;
									RxAoyfYzYDsYonLGXsvUgwChukLk = 3;
									return true;
								}
								bKYtFpdOyyopwWDgXevZsgrxWQOs();
								hSeONskmQJhVsjVUHGUASYbDGJrU = null;
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.DvrOAgWzTHcCWGLyldMWiMsAROaM().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -6;
								break;
							}
							if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
							{
								ControllerPollingInfo current4 = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current4;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 4;
								return true;
							}
							voQyPKUnPshXZdFCyWVzLjeZHBAs();
							hSeONskmQJhVsjVUHGUASYbDGJrU = null;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
						}
					}

					private void tMeeFNDmLhzrFkrObkICOsBGeJux()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
						}
					}

					private void bKYtFpdOyyopwWDgXevZsgrxWQOs()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
						}
					}

					private void voQyPKUnPshXZdFCyWVzLjeZHBAs()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
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
						DHJqrAIJpUnVHhVnHAUqkBCLrIqg dHJqrAIJpUnVHhVnHAUqkBCLrIqg;
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							dHJqrAIJpUnVHhVnHAUqkBCLrIqg = this;
						}
						else
						{
							dHJqrAIJpUnVHhVnHAUqkBCLrIqg = new DHJqrAIJpUnVHhVnHAUqkBCLrIqg(0);
							dHJqrAIJpUnVHhVnHAUqkBCLrIqg.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
						}
						return dHJqrAIJpUnVHhVnHAUqkBCLrIqg;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class QAMubuZiiPudSCqhOPyFeyHLNNhg : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					public PollingHelper TtytLoUfsgUyhsklaKccrnoMiiek;

					private IEnumerator<ControllerPollingInfo> hSeONskmQJhVsjVUHGUASYbDGJrU;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public QAMubuZiiPudSCqhOPyFeyHLNNhg(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						switch (RxAoyfYzYDsYonLGXsvUgwChukLk)
						{
						case -3:
						case 1:
							try
							{
								break;
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						case -4:
						case 2:
							try
							{
								break;
							}
							finally
							{
								tMeeFNDmLhzrFkrObkICOsBGeJux();
							}
						case -5:
						case 3:
							try
							{
								break;
							}
							finally
							{
								bKYtFpdOyyopwWDgXevZsgrxWQOs();
							}
						case -6:
						case 4:
							try
							{
								break;
							}
							finally
							{
								voQyPKUnPshXZdFCyWVzLjeZHBAs();
							}
						case -2:
						case -1:
						case 0:
							break;
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							PollingHelper ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
							switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
							{
							default:
								return false;
							case 0:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
								if (!CheckInitialized())
								{
									return false;
								}
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.dYUufOAgiYxVCulHuDweDbmlvBuAA().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0088;
							case 1:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0088;
							case 2:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -4;
								goto IL_00e8;
							case 3:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -5;
								goto IL_0148;
							case 4:
								{
									RxAoyfYzYDsYonLGXsvUgwChukLk = -6;
									break;
								}
								IL_00e8:
								if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
								{
									ControllerPollingInfo current = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
									VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
									RxAoyfYzYDsYonLGXsvUgwChukLk = 2;
									return true;
								}
								tMeeFNDmLhzrFkrObkICOsBGeJux();
								hSeONskmQJhVsjVUHGUASYbDGJrU = null;
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.yaXGMnGRHzLAsdmWClzYAeCKjTIlb().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -5;
								goto IL_0148;
								IL_0088:
								if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
								{
									ControllerPollingInfo current2 = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
									VqEePGSMyrGKIqkWibsjeHcWPSIx = current2;
									RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
									return true;
								}
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
								hSeONskmQJhVsjVUHGUASYbDGJrU = null;
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.HOEJAMRWlNjoQHyywIgOAMoSqKTE().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -4;
								goto IL_00e8;
								IL_0148:
								if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
								{
									ControllerPollingInfo current3 = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
									VqEePGSMyrGKIqkWibsjeHcWPSIx = current3;
									RxAoyfYzYDsYonLGXsvUgwChukLk = 3;
									return true;
								}
								bKYtFpdOyyopwWDgXevZsgrxWQOs();
								hSeONskmQJhVsjVUHGUASYbDGJrU = null;
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.YreEBReqxCdIJgxQdzUpAFWJOKmwA().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -6;
								break;
							}
							if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
							{
								ControllerPollingInfo current4 = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current4;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 4;
								return true;
							}
							voQyPKUnPshXZdFCyWVzLjeZHBAs();
							hSeONskmQJhVsjVUHGUASYbDGJrU = null;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
						}
					}

					private void tMeeFNDmLhzrFkrObkICOsBGeJux()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
						}
					}

					private void bKYtFpdOyyopwWDgXevZsgrxWQOs()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
						}
					}

					private void voQyPKUnPshXZdFCyWVzLjeZHBAs()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
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
						QAMubuZiiPudSCqhOPyFeyHLNNhg qAMubuZiiPudSCqhOPyFeyHLNNhg;
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							qAMubuZiiPudSCqhOPyFeyHLNNhg = this;
						}
						else
						{
							qAMubuZiiPudSCqhOPyFeyHLNNhg = new QAMubuZiiPudSCqhOPyFeyHLNNhg(0);
							qAMubuZiiPudSCqhOPyFeyHLNNhg.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
						}
						return qAMubuZiiPudSCqhOPyFeyHLNNhg;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class sFJFwaZdTkBclwhRyQeZXBYRFsqL : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					public PollingHelper TtytLoUfsgUyhsklaKccrnoMiiek;

					private IEnumerator<ControllerPollingInfo> hSeONskmQJhVsjVUHGUASYbDGJrU;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public sFJFwaZdTkBclwhRyQeZXBYRFsqL(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						switch (RxAoyfYzYDsYonLGXsvUgwChukLk)
						{
						case -3:
						case 1:
							try
							{
								break;
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						case -4:
						case 2:
							try
							{
								break;
							}
							finally
							{
								tMeeFNDmLhzrFkrObkICOsBGeJux();
							}
						case -5:
						case 3:
							try
							{
								break;
							}
							finally
							{
								bKYtFpdOyyopwWDgXevZsgrxWQOs();
							}
						case -6:
						case 4:
							try
							{
								break;
							}
							finally
							{
								voQyPKUnPshXZdFCyWVzLjeZHBAs();
							}
						case -2:
						case -1:
						case 0:
							break;
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							PollingHelper ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
							switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
							{
							default:
								return false;
							case 0:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
								if (!CheckInitialized())
								{
									return false;
								}
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.SktOozRnmJREqRNSUWbYArRRBgoK().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0088;
							case 1:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0088;
							case 2:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -4;
								goto IL_00e8;
							case 3:
								RxAoyfYzYDsYonLGXsvUgwChukLk = -5;
								goto IL_0148;
							case 4:
								{
									RxAoyfYzYDsYonLGXsvUgwChukLk = -6;
									break;
								}
								IL_00e8:
								if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
								{
									ControllerPollingInfo current = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
									VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
									RxAoyfYzYDsYonLGXsvUgwChukLk = 2;
									return true;
								}
								tMeeFNDmLhzrFkrObkICOsBGeJux();
								hSeONskmQJhVsjVUHGUASYbDGJrU = null;
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.cLPDCRhyqgMmkTmpLiLNnnreryrD().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -5;
								goto IL_0148;
								IL_0088:
								if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
								{
									ControllerPollingInfo current2 = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
									VqEePGSMyrGKIqkWibsjeHcWPSIx = current2;
									RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
									return true;
								}
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
								hSeONskmQJhVsjVUHGUASYbDGJrU = null;
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.nTOaOzAizIVxHePeOnxkRHlAxAWS().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -4;
								goto IL_00e8;
								IL_0148:
								if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
								{
									ControllerPollingInfo current3 = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
									VqEePGSMyrGKIqkWibsjeHcWPSIx = current3;
									RxAoyfYzYDsYonLGXsvUgwChukLk = 3;
									return true;
								}
								bKYtFpdOyyopwWDgXevZsgrxWQOs();
								hSeONskmQJhVsjVUHGUASYbDGJrU = null;
								hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.gwHUtKVRaufrytBZKyNeqEdqvaFV().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -6;
								break;
							}
							if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
							{
								ControllerPollingInfo current4 = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current4;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 4;
								return true;
							}
							voQyPKUnPshXZdFCyWVzLjeZHBAs();
							hSeONskmQJhVsjVUHGUASYbDGJrU = null;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
						}
					}

					private void tMeeFNDmLhzrFkrObkICOsBGeJux()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
						}
					}

					private void bKYtFpdOyyopwWDgXevZsgrxWQOs()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
						}
					}

					private void voQyPKUnPshXZdFCyWVzLjeZHBAs()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
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
						sFJFwaZdTkBclwhRyQeZXBYRFsqL sFJFwaZdTkBclwhRyQeZXBYRFsqL2;
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							sFJFwaZdTkBclwhRyQeZXBYRFsqL2 = this;
						}
						else
						{
							sFJFwaZdTkBclwhRyQeZXBYRFsqL2 = new sFJFwaZdTkBclwhRyQeZXBYRFsqL(0);
							sFJFwaZdTkBclwhRyQeZXBYRFsqL2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
						}
						return sFJFwaZdTkBclwhRyQeZXBYRFsqL2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class aIEOUAdnYePshwGHiyHoEEGBUoBv : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					private IList<CustomController> rSWjbVXZAEjqBDDAVYoCDnxtWdpE;

					private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

					private IEnumerator<ControllerPollingInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public aIEOUAdnYePshwGHiyHoEEGBUoBv(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
						if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
						{
							try
							{
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
							{
								if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
								{
									return false;
								}
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0086;
							}
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							rSWjbVXZAEjqBDDAVYoCDnxtWdpE = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.XoVatgitzcrXSMTZlyGxtxxDoZSr;
							fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
							goto IL_00b0;
							IL_0086:
							if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
							{
								ControllerPollingInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
							uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
							fIMVaffCgsuIJcnrkMmGGKfPwwel++;
							goto IL_00b0;
							IL_00b0:
							if (fIMVaffCgsuIJcnrkMmGGKfPwwel < rSWjbVXZAEjqBDDAVYoCDnxtWdpE.Count)
							{
								ffMVLALuuURkVdxHSgrNcNQkUFZS = rSWjbVXZAEjqBDDAVYoCDnxtWdpE[fIMVaffCgsuIJcnrkMmGGKfPwwel].PollForAllAxes().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
						{
							ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							return this;
						}
						return new aIEOUAdnYePshwGHiyHoEEGBUoBv(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class MgVUQNxiuTMlhVigUdHTvZAvSYEi : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					private IList<CustomController> rSWjbVXZAEjqBDDAVYoCDnxtWdpE;

					private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

					private IEnumerator<ControllerPollingInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public MgVUQNxiuTMlhVigUdHTvZAvSYEi(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
						if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
						{
							try
							{
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
							{
								if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
								{
									return false;
								}
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0086;
							}
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							rSWjbVXZAEjqBDDAVYoCDnxtWdpE = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.XoVatgitzcrXSMTZlyGxtxxDoZSr;
							fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
							goto IL_00b0;
							IL_0086:
							if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
							{
								ControllerPollingInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
							uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
							fIMVaffCgsuIJcnrkMmGGKfPwwel++;
							goto IL_00b0;
							IL_00b0:
							if (fIMVaffCgsuIJcnrkMmGGKfPwwel < rSWjbVXZAEjqBDDAVYoCDnxtWdpE.Count)
							{
								ffMVLALuuURkVdxHSgrNcNQkUFZS = rSWjbVXZAEjqBDDAVYoCDnxtWdpE[fIMVaffCgsuIJcnrkMmGGKfPwwel].PollForAllButtons().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
						{
							ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							return this;
						}
						return new MgVUQNxiuTMlhVigUdHTvZAvSYEi(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class VAwDyFDQrkXRrHxdEwzRDaRvetEM : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					private IList<CustomController> rSWjbVXZAEjqBDDAVYoCDnxtWdpE;

					private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

					private IEnumerator<ControllerPollingInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public VAwDyFDQrkXRrHxdEwzRDaRvetEM(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
						if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
						{
							try
							{
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
							{
								if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
								{
									return false;
								}
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0086;
							}
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							rSWjbVXZAEjqBDDAVYoCDnxtWdpE = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.XoVatgitzcrXSMTZlyGxtxxDoZSr;
							fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
							goto IL_00b0;
							IL_0086:
							if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
							{
								ControllerPollingInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
							uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
							fIMVaffCgsuIJcnrkMmGGKfPwwel++;
							goto IL_00b0;
							IL_00b0:
							if (fIMVaffCgsuIJcnrkMmGGKfPwwel < rSWjbVXZAEjqBDDAVYoCDnxtWdpE.Count)
							{
								ffMVLALuuURkVdxHSgrNcNQkUFZS = rSWjbVXZAEjqBDDAVYoCDnxtWdpE[fIMVaffCgsuIJcnrkMmGGKfPwwel].PollForAllButtonsDown().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
						{
							ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							return this;
						}
						return new VAwDyFDQrkXRrHxdEwzRDaRvetEM(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class ycbUWOVarDEjyaGrPbtDIduGrXup : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					private IList<CustomController> rSWjbVXZAEjqBDDAVYoCDnxtWdpE;

					private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

					private IEnumerator<ControllerPollingInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public ycbUWOVarDEjyaGrPbtDIduGrXup(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
						if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
						{
							try
							{
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
							{
								if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
								{
									return false;
								}
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0086;
							}
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							rSWjbVXZAEjqBDDAVYoCDnxtWdpE = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.XoVatgitzcrXSMTZlyGxtxxDoZSr;
							fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
							goto IL_00b0;
							IL_0086:
							if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
							{
								ControllerPollingInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
							uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
							fIMVaffCgsuIJcnrkMmGGKfPwwel++;
							goto IL_00b0;
							IL_00b0:
							if (fIMVaffCgsuIJcnrkMmGGKfPwwel < rSWjbVXZAEjqBDDAVYoCDnxtWdpE.Count)
							{
								ffMVLALuuURkVdxHSgrNcNQkUFZS = rSWjbVXZAEjqBDDAVYoCDnxtWdpE[fIMVaffCgsuIJcnrkMmGGKfPwwel].PollForAllElements().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
						{
							ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							return this;
						}
						return new ycbUWOVarDEjyaGrPbtDIduGrXup(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class CpraxQOhkTAwVPrcBInKZBtqfgmZ : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					private IList<CustomController> rSWjbVXZAEjqBDDAVYoCDnxtWdpE;

					private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

					private IEnumerator<ControllerPollingInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public CpraxQOhkTAwVPrcBInKZBtqfgmZ(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
						if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
						{
							try
							{
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
							{
								if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
								{
									return false;
								}
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0086;
							}
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							rSWjbVXZAEjqBDDAVYoCDnxtWdpE = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.XoVatgitzcrXSMTZlyGxtxxDoZSr;
							fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
							goto IL_00b0;
							IL_0086:
							if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
							{
								ControllerPollingInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
							uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
							fIMVaffCgsuIJcnrkMmGGKfPwwel++;
							goto IL_00b0;
							IL_00b0:
							if (fIMVaffCgsuIJcnrkMmGGKfPwwel < rSWjbVXZAEjqBDDAVYoCDnxtWdpE.Count)
							{
								ffMVLALuuURkVdxHSgrNcNQkUFZS = rSWjbVXZAEjqBDDAVYoCDnxtWdpE[fIMVaffCgsuIJcnrkMmGGKfPwwel].PollForAllElementsDown().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
						{
							ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							return this;
						}
						return new CpraxQOhkTAwVPrcBInKZBtqfgmZ(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class MulfOamGrrYNgwuunFFmlDKPfKcD : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					private IList<Joystick> DCHsYsHgaGZanwaPfowyYcSnBov;

					private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

					private IEnumerator<ControllerPollingInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public MulfOamGrrYNgwuunFFmlDKPfKcD(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
						if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
						{
							try
							{
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
							{
								if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
								{
									return false;
								}
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0086;
							}
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							DCHsYsHgaGZanwaPfowyYcSnBov = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.FMULSNPMOjtlSugUpKDpvqOlAjyl;
							fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
							goto IL_00b0;
							IL_0086:
							if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
							{
								ControllerPollingInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
							uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
							fIMVaffCgsuIJcnrkMmGGKfPwwel++;
							goto IL_00b0;
							IL_00b0:
							if (fIMVaffCgsuIJcnrkMmGGKfPwwel < DCHsYsHgaGZanwaPfowyYcSnBov.Count)
							{
								ffMVLALuuURkVdxHSgrNcNQkUFZS = DCHsYsHgaGZanwaPfowyYcSnBov[fIMVaffCgsuIJcnrkMmGGKfPwwel].PollForAllAxes().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
						{
							ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							return this;
						}
						return new MulfOamGrrYNgwuunFFmlDKPfKcD(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class ueTpyPCBSBBBmDjixqnMAYJfkUpBA : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					private IList<Joystick> DCHsYsHgaGZanwaPfowyYcSnBov;

					private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

					private IEnumerator<ControllerPollingInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public ueTpyPCBSBBBmDjixqnMAYJfkUpBA(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
						if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
						{
							try
							{
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
							{
								if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
								{
									return false;
								}
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0086;
							}
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							DCHsYsHgaGZanwaPfowyYcSnBov = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.FMULSNPMOjtlSugUpKDpvqOlAjyl;
							fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
							goto IL_00b0;
							IL_0086:
							if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
							{
								ControllerPollingInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
							uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
							fIMVaffCgsuIJcnrkMmGGKfPwwel++;
							goto IL_00b0;
							IL_00b0:
							if (fIMVaffCgsuIJcnrkMmGGKfPwwel < DCHsYsHgaGZanwaPfowyYcSnBov.Count)
							{
								ffMVLALuuURkVdxHSgrNcNQkUFZS = DCHsYsHgaGZanwaPfowyYcSnBov[fIMVaffCgsuIJcnrkMmGGKfPwwel].PollForAllButtons().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
						{
							ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							return this;
						}
						return new ueTpyPCBSBBBmDjixqnMAYJfkUpBA(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class LMHOYxqZNZkTXmOSyOpwlBDngIkl : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					private IList<Joystick> DCHsYsHgaGZanwaPfowyYcSnBov;

					private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

					private IEnumerator<ControllerPollingInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public LMHOYxqZNZkTXmOSyOpwlBDngIkl(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
						if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
						{
							try
							{
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
							{
								if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
								{
									return false;
								}
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0086;
							}
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							DCHsYsHgaGZanwaPfowyYcSnBov = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.FMULSNPMOjtlSugUpKDpvqOlAjyl;
							fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
							goto IL_00b0;
							IL_0086:
							if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
							{
								ControllerPollingInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
							uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
							fIMVaffCgsuIJcnrkMmGGKfPwwel++;
							goto IL_00b0;
							IL_00b0:
							if (fIMVaffCgsuIJcnrkMmGGKfPwwel < DCHsYsHgaGZanwaPfowyYcSnBov.Count)
							{
								ffMVLALuuURkVdxHSgrNcNQkUFZS = DCHsYsHgaGZanwaPfowyYcSnBov[fIMVaffCgsuIJcnrkMmGGKfPwwel].PollForAllButtonsDown().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
						{
							ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							return this;
						}
						return new LMHOYxqZNZkTXmOSyOpwlBDngIkl(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class lYPJKDFXDlOcBmKKrQJiJMmKvgUG : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					private IList<Joystick> DCHsYsHgaGZanwaPfowyYcSnBov;

					private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

					private IEnumerator<ControllerPollingInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public lYPJKDFXDlOcBmKKrQJiJMmKvgUG(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
						if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
						{
							try
							{
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
							{
								if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
								{
									return false;
								}
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0086;
							}
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							DCHsYsHgaGZanwaPfowyYcSnBov = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.FMULSNPMOjtlSugUpKDpvqOlAjyl;
							fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
							goto IL_00b0;
							IL_0086:
							if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
							{
								ControllerPollingInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
							uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
							fIMVaffCgsuIJcnrkMmGGKfPwwel++;
							goto IL_00b0;
							IL_00b0:
							if (fIMVaffCgsuIJcnrkMmGGKfPwwel < DCHsYsHgaGZanwaPfowyYcSnBov.Count)
							{
								ffMVLALuuURkVdxHSgrNcNQkUFZS = DCHsYsHgaGZanwaPfowyYcSnBov[fIMVaffCgsuIJcnrkMmGGKfPwwel].PollForAllElements().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
						{
							ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							return this;
						}
						return new lYPJKDFXDlOcBmKKrQJiJMmKvgUG(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class bWUVgMzqQNbYjirwSwYDGRzHUYFdA : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					private IList<Joystick> DCHsYsHgaGZanwaPfowyYcSnBov;

					private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

					private IEnumerator<ControllerPollingInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public bWUVgMzqQNbYjirwSwYDGRzHUYFdA(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
						if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
						{
							try
							{
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
							{
								if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
								{
									return false;
								}
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_0086;
							}
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							DCHsYsHgaGZanwaPfowyYcSnBov = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.FMULSNPMOjtlSugUpKDpvqOlAjyl;
							fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
							goto IL_00b0;
							IL_0086:
							if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
							{
								ControllerPollingInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
							uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
							fIMVaffCgsuIJcnrkMmGGKfPwwel++;
							goto IL_00b0;
							IL_00b0:
							if (fIMVaffCgsuIJcnrkMmGGKfPwwel < DCHsYsHgaGZanwaPfowyYcSnBov.Count)
							{
								ffMVLALuuURkVdxHSgrNcNQkUFZS = DCHsYsHgaGZanwaPfowyYcSnBov[fIMVaffCgsuIJcnrkMmGGKfPwwel].PollForAllElementsDown().GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
						{
							ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							return this;
						}
						return new bWUVgMzqQNbYjirwSwYDGRzHUYFdA(0);
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private static PollingHelper LDnMtBpByMhNcmNpPzdzLRPijZRi;

				internal static PollingHelper izfChqvjKyhCcypSioMPzwuCmhEV => LDnMtBpByMhNcmNpPzdzLRPijZRi ?? (LDnMtBpByMhNcmNpPzdzLRPijZRi = new PollingHelper());

				private PollingHelper()
				{
				}

				public ControllerPollingInfo PollAllControllersForFirstElement()
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
					}
					ControllerPollingInfo result = GKrJpTrxcPpJqSaqxyfZNueemgaT();
					if (result.success)
					{
						return result;
					}
					result = wvVVQJyWhuYyNhrFzHpkHbOGEXyf();
					if (result.success)
					{
						return result;
					}
					result = SOeaMXqsnlIBHgFVxBnLzeJoANyy();
					if (result.success)
					{
						return result;
					}
					result = ZRcqlVLesegvydCSfddDexARVYfVA();
					if (result.success)
					{
						return result;
					}
					return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				public ControllerPollingInfo PollAllControllersForFirstElementDown()
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
					}
					ControllerPollingInfo result = ElbWqmjNpxhNlJVcrHeCzNuWKEbY();
					if (result.success)
					{
						return result;
					}
					result = HZlEHhKOolOVlKfGkQcNwFLnLVQU();
					if (result.success)
					{
						return result;
					}
					result = TFbBeRdvKlLoObdKBtkdOvGafEzSb();
					if (result.success)
					{
						return result;
					}
					result = osVgAhkVAskWFpdJbCiDyOlJTuvAA();
					if (result.success)
					{
						return result;
					}
					return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				public ControllerPollingInfo PollAllControllersForFirstButton()
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
					}
					ControllerPollingInfo result = wMHnzEswEIgswbnzZQJsMJSnaxVA();
					if (result.success)
					{
						return result;
					}
					result = wvVVQJyWhuYyNhrFzHpkHbOGEXyf();
					if (result.success)
					{
						return result;
					}
					result = JjTqruNoTDGoOZqLkDfaFEwClmUjb();
					if (result.success)
					{
						return result;
					}
					result = EgUZfjOUYamlwVqiVtmewVMCRfKG();
					if (result.success)
					{
						return result;
					}
					return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				public ControllerPollingInfo PollAllControllersForFirstButtonDown()
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
					}
					ControllerPollingInfo result = aNTAMQEsRrjlqIVjGRQcbzVlrpjUA();
					if (result.success)
					{
						return result;
					}
					result = HZlEHhKOolOVlKfGkQcNwFLnLVQU();
					if (result.success)
					{
						return result;
					}
					result = vZDUVGmVjOMElqUPkPkAxxqoHRmc();
					if (result.success)
					{
						return result;
					}
					result = mQYimJQCZdvnMaDTdYgpznaCDrop();
					if (result.success)
					{
						return result;
					}
					return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				public ControllerPollingInfo PollAllControllersForFirstAxis()
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
					}
					ControllerPollingInfo result = jwoNChRojuQIQfgwOQDzRBiszxqS();
					if (result.success)
					{
						return result;
					}
					result = ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
					if (result.success)
					{
						return result;
					}
					result = DvrGZhBBEOyjqnASGsQiERANMtrbb();
					if (result.success)
					{
						return result;
					}
					result = LJQtpXRHBuUXPRTwVODChNsxAblx();
					if (result.success)
					{
						return result;
					}
					return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				public ControllerPollingInfo PollAllControllersOfTypeForFirstElement(ControllerType controllerType)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
					}
					return controllerType switch
					{
						ControllerType.Joystick => GKrJpTrxcPpJqSaqxyfZNueemgaT(), 
						ControllerType.Keyboard => wvVVQJyWhuYyNhrFzHpkHbOGEXyf(), 
						ControllerType.Mouse => SOeaMXqsnlIBHgFVxBnLzeJoANyy(), 
						ControllerType.Custom => ZRcqlVLesegvydCSfddDexARVYfVA(), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollAllControllersOfTypeForFirstElementDown(ControllerType controllerType)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
					}
					return controllerType switch
					{
						ControllerType.Joystick => ElbWqmjNpxhNlJVcrHeCzNuWKEbY(), 
						ControllerType.Keyboard => HZlEHhKOolOVlKfGkQcNwFLnLVQU(), 
						ControllerType.Mouse => TFbBeRdvKlLoObdKBtkdOvGafEzSb(), 
						ControllerType.Custom => osVgAhkVAskWFpdJbCiDyOlJTuvAA(), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollAllControllersOfTypeForFirstButton(ControllerType controllerType)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
					}
					return controllerType switch
					{
						ControllerType.Joystick => wMHnzEswEIgswbnzZQJsMJSnaxVA(), 
						ControllerType.Keyboard => wvVVQJyWhuYyNhrFzHpkHbOGEXyf(), 
						ControllerType.Mouse => JjTqruNoTDGoOZqLkDfaFEwClmUjb(), 
						ControllerType.Custom => EgUZfjOUYamlwVqiVtmewVMCRfKG(), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollAllControllersOfTypeForFirstButtonDown(ControllerType controllerType)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
					}
					return controllerType switch
					{
						ControllerType.Joystick => aNTAMQEsRrjlqIVjGRQcbzVlrpjUA(), 
						ControllerType.Keyboard => HZlEHhKOolOVlKfGkQcNwFLnLVQU(), 
						ControllerType.Mouse => vZDUVGmVjOMElqUPkPkAxxqoHRmc(), 
						ControllerType.Custom => mQYimJQCZdvnMaDTdYgpznaCDrop(), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollAllControllersOfTypeForFirstAxis(ControllerType controllerType)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
					}
					return controllerType switch
					{
						ControllerType.Joystick => jwoNChRojuQIQfgwOQDzRBiszxqS(), 
						ControllerType.Keyboard => ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA(), 
						ControllerType.Mouse => DvrGZhBBEOyjqnASGsQiERANMtrbb(), 
						ControllerType.Custom => LJQtpXRHBuUXPRTwVODChNsxAblx(), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollControllerForFirstElement(ControllerType controllerType, int controllerId)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
					}
					return controllerType switch
					{
						ControllerType.Joystick => CAdAKNfAFajMLqVTAIWcHUyKPLFcA(controllerId), 
						ControllerType.Keyboard => wvVVQJyWhuYyNhrFzHpkHbOGEXyf(), 
						ControllerType.Mouse => SOeaMXqsnlIBHgFVxBnLzeJoANyy(), 
						ControllerType.Custom => TabxPWDsPBgMMUmZCwDDEBBYvvJT(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollControllerForFirstElementDown(ControllerType controllerType, int controllerId)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
					}
					return controllerType switch
					{
						ControllerType.Joystick => MQNHnJKyMyOUsvFtzBxhHeCkebicb(controllerId), 
						ControllerType.Keyboard => HZlEHhKOolOVlKfGkQcNwFLnLVQU(), 
						ControllerType.Mouse => TFbBeRdvKlLoObdKBtkdOvGafEzSb(), 
						ControllerType.Custom => lOBtbLHhkelQpoMMXEOfmtKjRAQs(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollControllerForFirstButton(ControllerType controllerType, int controllerId)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
					}
					return controllerType switch
					{
						ControllerType.Joystick => oSCkEAQLxZCBYiLhqNuRPbcSaAIRA(controllerId), 
						ControllerType.Keyboard => wvVVQJyWhuYyNhrFzHpkHbOGEXyf(), 
						ControllerType.Mouse => JjTqruNoTDGoOZqLkDfaFEwClmUjb(), 
						ControllerType.Custom => mRixVDsFNSoDIIHrgxwdIdjvXMrn(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollControllerForFirstButtonDown(ControllerType controllerType, int controllerId)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
					}
					return controllerType switch
					{
						ControllerType.Joystick => mDMdaSIOCjnWkjmpwteYZjEarfvs(controllerId), 
						ControllerType.Keyboard => HZlEHhKOolOVlKfGkQcNwFLnLVQU(), 
						ControllerType.Mouse => vZDUVGmVjOMElqUPkPkAxxqoHRmc(), 
						ControllerType.Custom => rfHaRyvuRaqHKnglRdqvSNnphxiHA(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollControllerForFirstAxis(ControllerType controllerType, int controllerId)
				{
					if (!CheckInitialized())
					{
						return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
					}
					return controllerType switch
					{
						ControllerType.Joystick => XvxceAafWNSISoBwWMSOMtOyJYpL(controllerId), 
						ControllerType.Keyboard => ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA(), 
						ControllerType.Mouse => DvrGZhBBEOyjqnASGsQiERANMtrbb(), 
						ControllerType.Custom => PftdYFBEmQjrNwCTsqAwsRSQOjbbA(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public IEnumerable<ControllerPollingInfo> PollAllControllersForAllElements()
				{
					return new QAMubuZiiPudSCqhOPyFeyHLNNhg(-2)
					{
						TtytLoUfsgUyhsklaKccrnoMiiek = this
					};
				}

				public IEnumerable<ControllerPollingInfo> PollAllControllersForAllElementsDown()
				{
					return new sFJFwaZdTkBclwhRyQeZXBYRFsqL(-2)
					{
						TtytLoUfsgUyhsklaKccrnoMiiek = this
					};
				}

				public IEnumerable<ControllerPollingInfo> PollAllControllersForAllButtons()
				{
					return new VTmCTECfrLOpWyDjTbthIzGUmvdv(-2)
					{
						TtytLoUfsgUyhsklaKccrnoMiiek = this
					};
				}

				public IEnumerable<ControllerPollingInfo> PollAllControllersForAllButtonsDown()
				{
					return new DHJqrAIJpUnVHhVnHAUqkBCLrIqg(-2)
					{
						TtytLoUfsgUyhsklaKccrnoMiiek = this
					};
				}

				public IEnumerable<ControllerPollingInfo> PollAllControllersForAllAxes()
				{
					return new tDTFBDdEcQFfhUOSSEGcGWgTBMKNA(-2)
					{
						TtytLoUfsgUyhsklaKccrnoMiiek = this
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
						ControllerType.Joystick => oNHzsXEUVLwrwfDlrjFvikEAiyRz(controllerId), 
						ControllerType.Keyboard => HOEJAMRWlNjoQHyywIgOAMoSqKTE(), 
						ControllerType.Mouse => yaXGMnGRHzLAsdmWClzYAeCKjTIlb(), 
						ControllerType.Custom => LlJumptLayGtzjEBFQRJFZTPmUvX(controllerId), 
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
						ControllerType.Joystick => uASjJkZZigXWClPGrxYXTWuYytCB(controllerId), 
						ControllerType.Keyboard => nTOaOzAizIVxHePeOnxkRHlAxAWS(), 
						ControllerType.Mouse => cLPDCRhyqgMmkTmpLiLNnnreryrD(), 
						ControllerType.Custom => uStEnMWAjtgNoCXbEUVvdQyxXPXSA(controllerId), 
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
						ControllerType.Joystick => InchXcmEQdFAaFwgkGWaJwSgblHbA(controllerId), 
						ControllerType.Keyboard => HOEJAMRWlNjoQHyywIgOAMoSqKTE(), 
						ControllerType.Mouse => HKfvIjVyGzhVyNzzmHuXrPXdNtwB(), 
						ControllerType.Custom => jZKhYzuFNMAcZCWigHRDHQhabfYp(controllerId), 
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
						ControllerType.Joystick => vrICuQWYoOrlhALPFYnIOpJlEyPk(controllerId), 
						ControllerType.Keyboard => nTOaOzAizIVxHePeOnxkRHlAxAWS(), 
						ControllerType.Mouse => vjoctRbePlNRDLebnEgMQAlXsvCEA(), 
						ControllerType.Custom => vdQAqndUFXikgRdontniOExbHDlBA(controllerId), 
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
						ControllerType.Joystick => poJGVDxjXbhXycFeIsdyHsoedsCYA(controllerId), 
						ControllerType.Keyboard => new List<ControllerPollingInfo>(), 
						ControllerType.Mouse => xLpJNhMMqKQpDjLnVPCaMhTCFhvDA(), 
						ControllerType.Custom => utngrPJaCAzrAjFBBjQiFNhEtvAib(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				private ControllerPollingInfo GKrJpTrxcPpJqSaqxyfZNueemgaT()
				{
					IList<Joystick> list = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.FMULSNPMOjtlSugUpKDpvqOlAjyl;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstElement();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo ElbWqmjNpxhNlJVcrHeCzNuWKEbY()
				{
					IList<Joystick> list = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.FMULSNPMOjtlSugUpKDpvqOlAjyl;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstElementDown();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo wMHnzEswEIgswbnzZQJsMJSnaxVA()
				{
					IList<Joystick> list = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.FMULSNPMOjtlSugUpKDpvqOlAjyl;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstButton();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo aNTAMQEsRrjlqIVjGRQcbzVlrpjUA()
				{
					IList<Joystick> list = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.FMULSNPMOjtlSugUpKDpvqOlAjyl;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstButtonDown();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo jwoNChRojuQIQfgwOQDzRBiszxqS()
				{
					IList<Joystick> list = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.FMULSNPMOjtlSugUpKDpvqOlAjyl;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstAxis();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo CAdAKNfAFajMLqVTAIWcHUyKPLFcA(int P_0)
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetJoystick(P_0)?.PollForFirstElement() ?? ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo MQNHnJKyMyOUsvFtzBxhHeCkebicb(int P_0)
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetJoystick(P_0)?.PollForFirstElementDown() ?? ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo oSCkEAQLxZCBYiLhqNuRPbcSaAIRA(int P_0)
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetJoystick(P_0)?.PollForFirstButton() ?? ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo mDMdaSIOCjnWkjmpwteYZjEarfvs(int P_0)
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetJoystick(P_0)?.PollForFirstButtonDown() ?? ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo XvxceAafWNSISoBwWMSOMtOyJYpL(int P_0)
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetJoystick(P_0)?.PollForFirstAxis() ?? ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo wvVVQJyWhuYyNhrFzHpkHbOGEXyf()
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.Keyboard.PollForFirstKey();
				}

				private ControllerPollingInfo HZlEHhKOolOVlKfGkQcNwFLnLVQU()
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.Keyboard.PollForFirstKeyDown();
				}

				private ControllerPollingInfo SOeaMXqsnlIBHgFVxBnLzeJoANyy()
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.Mouse.PollForFirstElement();
				}

				private ControllerPollingInfo TFbBeRdvKlLoObdKBtkdOvGafEzSb()
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.Mouse.PollForFirstElementDown();
				}

				private ControllerPollingInfo JjTqruNoTDGoOZqLkDfaFEwClmUjb()
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.Mouse.PollForFirstButton();
				}

				private ControllerPollingInfo vZDUVGmVjOMElqUPkPkAxxqoHRmc()
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.Mouse.PollForFirstButtonDown();
				}

				private ControllerPollingInfo DvrGZhBBEOyjqnASGsQiERANMtrbb()
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.Mouse.PollForFirstAxis();
				}

				private ControllerPollingInfo ZRcqlVLesegvydCSfddDexARVYfVA()
				{
					IList<CustomController> list = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.XoVatgitzcrXSMTZlyGxtxxDoZSr;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstElement();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo osVgAhkVAskWFpdJbCiDyOlJTuvAA()
				{
					IList<CustomController> list = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.XoVatgitzcrXSMTZlyGxtxxDoZSr;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstElementDown();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo EgUZfjOUYamlwVqiVtmewVMCRfKG()
				{
					IList<CustomController> list = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.XoVatgitzcrXSMTZlyGxtxxDoZSr;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstButton();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo mQYimJQCZdvnMaDTdYgpznaCDrop()
				{
					IList<CustomController> list = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.XoVatgitzcrXSMTZlyGxtxxDoZSr;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstButtonDown();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo LJQtpXRHBuUXPRTwVODChNsxAblx()
				{
					IList<CustomController> list = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.XoVatgitzcrXSMTZlyGxtxxDoZSr;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstAxis();
						if (result.success)
						{
							return result;
						}
					}
					return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo TabxPWDsPBgMMUmZCwDDEBBYvvJT(int P_0)
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetCustomController(P_0)?.PollForFirstElement() ?? ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo lOBtbLHhkelQpoMMXEOfmtKjRAQs(int P_0)
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetCustomController(P_0)?.PollForFirstElementDown() ?? ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo mRixVDsFNSoDIIHrgxwdIdjvXMrn(int P_0)
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetCustomController(P_0)?.PollForFirstButton() ?? ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo rfHaRyvuRaqHKnglRdqvSNnphxiHA(int P_0)
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetCustomController(P_0)?.PollForFirstButtonDown() ?? ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private ControllerPollingInfo PftdYFBEmQjrNwCTsqAwsRSQOjbbA(int P_0)
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetCustomController(P_0)?.PollForFirstAxis() ?? ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
				}

				private IEnumerable<ControllerPollingInfo> dYUufOAgiYxVCulHuDweDbmlvBuAA()
				{
					return new lYPJKDFXDlOcBmKKrQJiJMmKvgUG(-2);
				}

				private IEnumerable<ControllerPollingInfo> SktOozRnmJREqRNSUWbYArRRBgoK()
				{
					return new bWUVgMzqQNbYjirwSwYDGRzHUYFdA(-2);
				}

				private IEnumerable<ControllerPollingInfo> sszMazZiRlLXifusuhSvrXruVsvS()
				{
					return new ueTpyPCBSBBBmDjixqnMAYJfkUpBA(-2);
				}

				private IEnumerable<ControllerPollingInfo> ZqxTJDIDYhzvQgREIWmdhRCmoMej()
				{
					return new LMHOYxqZNZkTXmOSyOpwlBDngIkl(-2);
				}

				private IEnumerable<ControllerPollingInfo> zwNhHIcIgtilZkHFNoLdYkBpudmR()
				{
					return new MulfOamGrrYNgwuunFFmlDKPfKcD(-2);
				}

				private IEnumerable<ControllerPollingInfo> oNHzsXEUVLwrwfDlrjFvikEAiyRz(int P_0)
				{
					Joystick joystick = ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetJoystick(P_0);
					if (joystick == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return joystick.PollForAllElements();
				}

				private IEnumerable<ControllerPollingInfo> uASjJkZZigXWClPGrxYXTWuYytCB(int P_0)
				{
					Joystick joystick = ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetJoystick(P_0);
					if (joystick == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return joystick.PollForAllElementsDown();
				}

				private IEnumerable<ControllerPollingInfo> InchXcmEQdFAaFwgkGWaJwSgblHbA(int P_0)
				{
					Joystick joystick = ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetJoystick(P_0);
					if (joystick == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return joystick.PollForAllButtons();
				}

				private IEnumerable<ControllerPollingInfo> vrICuQWYoOrlhALPFYnIOpJlEyPk(int P_0)
				{
					Joystick joystick = ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetJoystick(P_0);
					if (joystick == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return joystick.PollForAllButtonsDown();
				}

				private IEnumerable<ControllerPollingInfo> poJGVDxjXbhXycFeIsdyHsoedsCYA(int P_0)
				{
					Joystick joystick = ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetJoystick(P_0);
					if (joystick == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return joystick.PollForAllAxes();
				}

				private IEnumerable<ControllerPollingInfo> HOEJAMRWlNjoQHyywIgOAMoSqKTE()
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.Keyboard.PollForAllKeys();
				}

				private IEnumerable<ControllerPollingInfo> nTOaOzAizIVxHePeOnxkRHlAxAWS()
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.Keyboard.PollForAllKeysDown();
				}

				private IEnumerable<ControllerPollingInfo> yaXGMnGRHzLAsdmWClzYAeCKjTIlb()
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.Mouse.PollForAllElements();
				}

				private IEnumerable<ControllerPollingInfo> cLPDCRhyqgMmkTmpLiLNnnreryrD()
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.Mouse.PollForAllElementsDown();
				}

				private IEnumerable<ControllerPollingInfo> HKfvIjVyGzhVyNzzmHuXrPXdNtwB()
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.Mouse.PollForAllButtons();
				}

				private IEnumerable<ControllerPollingInfo> vjoctRbePlNRDLebnEgMQAlXsvCEA()
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.Mouse.PollForAllButtonsDown();
				}

				private IEnumerable<ControllerPollingInfo> xLpJNhMMqKQpDjLnVPCaMhTCFhvDA()
				{
					return ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.Mouse.PollForAllAxes();
				}

				private IEnumerable<ControllerPollingInfo> YreEBReqxCdIJgxQdzUpAFWJOKmwA()
				{
					return new ycbUWOVarDEjyaGrPbtDIduGrXup(-2);
				}

				private IEnumerable<ControllerPollingInfo> gwHUtKVRaufrytBZKyNeqEdqvaFV()
				{
					return new CpraxQOhkTAwVPrcBInKZBtqfgmZ(-2);
				}

				private IEnumerable<ControllerPollingInfo> hTjfFKtRwBiwDyrjjHQnZaVkwhIc()
				{
					return new MgVUQNxiuTMlhVigUdHTvZAvSYEi(-2);
				}

				private IEnumerable<ControllerPollingInfo> DvrOAgWzTHcCWGLyldMWiMsAROaM()
				{
					return new VAwDyFDQrkXRrHxdEwzRDaRvetEM(-2);
				}

				private IEnumerable<ControllerPollingInfo> VJeweIaiXWbvCcIFICQocFzFqnbpB()
				{
					return new aIEOUAdnYePshwGHiyHoEEGBUoBv(-2);
				}

				private IEnumerable<ControllerPollingInfo> LlJumptLayGtzjEBFQRJFZTPmUvX(int P_0)
				{
					CustomController customController = ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetCustomController(P_0);
					if (customController == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return customController.PollForAllElements();
				}

				private IEnumerable<ControllerPollingInfo> uStEnMWAjtgNoCXbEUVvdQyxXPXSA(int P_0)
				{
					CustomController customController = ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetCustomController(P_0);
					if (customController == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return customController.PollForAllElementsDown();
				}

				private IEnumerable<ControllerPollingInfo> jZKhYzuFNMAcZCWigHRDHQhabfYp(int P_0)
				{
					CustomController customController = ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetCustomController(P_0);
					if (customController == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return customController.PollForAllButtons();
				}

				private IEnumerable<ControllerPollingInfo> vdQAqndUFXikgRdontniOExbHDlBA(int P_0)
				{
					CustomController customController = ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetCustomController(P_0);
					if (customController == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return customController.PollForAllButtonsDown();
				}

				private IEnumerable<ControllerPollingInfo> utngrPJaCAzrAjFBBjQiFNhEtvAib(int P_0)
				{
					CustomController customController = ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV.GetCustomController(P_0);
					if (customController == null)
					{
						return new List<ControllerPollingInfo>();
					}
					return customController.PollForAllAxes();
				}
			}

			[EditorBrowsable(EditorBrowsableState.Never)]
			[Browsable(false)]
			public sealed class ConflictCheckingHelper : CodeHelper
			{
				private sealed class WNGBiFaXYHJWEqVXHglvFOFXoBWEb : IDisposable, IEnumerable, IEnumerator, IEnumerable<ElementAssignmentConflictInfo>, IEnumerator<ElementAssignmentConflictInfo>
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ElementAssignmentConflictInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					private int DljjufeDxHKtQhGDvoMcJRNEJSNE;

					public int LPJODyrZpNxPzXAMrfCKobQaJWhD;

					private ActionElementMap IZGLeBjXaoohNXDnyqGgBnrFgcfw;

					public ActionElementMap ElKoGEhTJYZThMFOOdRumLXCMPDE;

					private bool LsArpUAaycAWpQMssIwwpGxYhCYd;

					public bool EqRTwgfjsYypsrTblHXtvnwSZMnf;

					private int dxNWoKAWRnEjMzagpBqSNtmzbbJFA;

					public int wYvEyNiUnzAaMlalJwSaPpoqwWXHA;

					private CustomControllerMap OZiEbTfyFdeNlbEgiDNQmLdZJIxpA;

					public CustomControllerMap fsLFWnOwyMFecKtbquAxUpiyvKDPA;

					private bool HIuFjgoQMMeZHtUafJbERKutTjQt;

					public bool MlWZQPQtLyJnETcXcYBEkbNZbonN;

					private bool fxcwWCJuWyMdSnVzKKdavSrfSkCc;

					public bool AIUEvmkpzzZEGHiPnuHpyDYEjNVE;

					private IList<Player> BKKFaUdfuegBblGWCXFcTsceicYAb;

					private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

					private IEnumerator<ElementAssignmentConflictInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public WNGBiFaXYHJWEqVXHglvFOFXoBWEb(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
						if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
						{
							try
							{
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
							{
								if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
								{
									return false;
								}
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_00e2;
							}
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							if (DljjufeDxHKtQhGDvoMcJRNEJSNE < 0 || IZGLeBjXaoohNXDnyqGgBnrFgcfw == null)
							{
								return false;
							}
							BKKFaUdfuegBblGWCXFcTsceicYAb = (LsArpUAaycAWpQMssIwwpGxYhCYd ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
							fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
							goto IL_010c;
							IL_010c:
							if (fIMVaffCgsuIJcnrkMmGGKfPwwel < BKKFaUdfuegBblGWCXFcTsceicYAb.Count)
							{
								ffMVLALuuURkVdxHSgrNcNQkUFZS = BKKFaUdfuegBblGWCXFcTsceicYAb[fIMVaffCgsuIJcnrkMmGGKfPwwel].controllers.conflictChecking.ElementAssignmentConflicts(ControllerType.Custom, dxNWoKAWRnEjMzagpBqSNtmzbbJFA, OZiEbTfyFdeNlbEgiDNQmLdZJIxpA, IZGLeBjXaoohNXDnyqGgBnrFgcfw, HIuFjgoQMMeZHtUafJbERKutTjQt, fxcwWCJuWyMdSnVzKKdavSrfSkCc).GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_00e2;
							}
							return false;
							IL_00e2:
							if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
							{
								ElementAssignmentConflictInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
							uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
							fIMVaffCgsuIJcnrkMmGGKfPwwel++;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
						{
							ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
						WNGBiFaXYHJWEqVXHglvFOFXoBWEb wNGBiFaXYHJWEqVXHglvFOFXoBWEb;
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							wNGBiFaXYHJWEqVXHglvFOFXoBWEb = this;
						}
						else
						{
							wNGBiFaXYHJWEqVXHglvFOFXoBWEb = new WNGBiFaXYHJWEqVXHglvFOFXoBWEb(0);
						}
						wNGBiFaXYHJWEqVXHglvFOFXoBWEb.DljjufeDxHKtQhGDvoMcJRNEJSNE = LPJODyrZpNxPzXAMrfCKobQaJWhD;
						wNGBiFaXYHJWEqVXHglvFOFXoBWEb.dxNWoKAWRnEjMzagpBqSNtmzbbJFA = wYvEyNiUnzAaMlalJwSaPpoqwWXHA;
						wNGBiFaXYHJWEqVXHglvFOFXoBWEb.OZiEbTfyFdeNlbEgiDNQmLdZJIxpA = fsLFWnOwyMFecKtbquAxUpiyvKDPA;
						wNGBiFaXYHJWEqVXHglvFOFXoBWEb.IZGLeBjXaoohNXDnyqGgBnrFgcfw = ElKoGEhTJYZThMFOOdRumLXCMPDE;
						wNGBiFaXYHJWEqVXHglvFOFXoBWEb.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
						wNGBiFaXYHJWEqVXHglvFOFXoBWEb.fxcwWCJuWyMdSnVzKKdavSrfSkCc = AIUEvmkpzzZEGHiPnuHpyDYEjNVE;
						wNGBiFaXYHJWEqVXHglvFOFXoBWEb.LsArpUAaycAWpQMssIwwpGxYhCYd = EqRTwgfjsYypsrTblHXtvnwSZMnf;
						return wNGBiFaXYHJWEqVXHglvFOFXoBWEb;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class vasCQHBiPilUGNJZhGPwjCjftBeIB : IDisposable, IEnumerable, IEnumerator, IEnumerable<ElementAssignmentConflictInfo>, IEnumerator<ElementAssignmentConflictInfo>
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ElementAssignmentConflictInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					private ElementAssignmentConflictCheck HHyftJlGnirHuGQIQnIQkaXvgYegA;

					public ElementAssignmentConflictCheck MelcAnrraPFJzeIaADSAOKRmqdwDb;

					private bool LsArpUAaycAWpQMssIwwpGxYhCYd;

					public bool EqRTwgfjsYypsrTblHXtvnwSZMnf;

					private bool HIuFjgoQMMeZHtUafJbERKutTjQt;

					public bool MlWZQPQtLyJnETcXcYBEkbNZbonN;

					private bool fxcwWCJuWyMdSnVzKKdavSrfSkCc;

					public bool AIUEvmkpzzZEGHiPnuHpyDYEjNVE;

					private IList<Player> BKKFaUdfuegBblGWCXFcTsceicYAb;

					private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

					private IEnumerator<ElementAssignmentConflictInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public vasCQHBiPilUGNJZhGPwjCjftBeIB(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
						if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
						{
							try
							{
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
							{
								if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
								{
									return false;
								}
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_00df;
							}
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							if (HHyftJlGnirHuGQIQnIQkaXvgYegA.playerId < 0 || HHyftJlGnirHuGQIQnIQkaXvgYegA.elementAssignmentType == ElementAssignmentType.KeyboardKey)
							{
								return false;
							}
							BKKFaUdfuegBblGWCXFcTsceicYAb = (LsArpUAaycAWpQMssIwwpGxYhCYd ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
							fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
							goto IL_0109;
							IL_0109:
							if (fIMVaffCgsuIJcnrkMmGGKfPwwel < BKKFaUdfuegBblGWCXFcTsceicYAb.Count)
							{
								ffMVLALuuURkVdxHSgrNcNQkUFZS = BKKFaUdfuegBblGWCXFcTsceicYAb[fIMVaffCgsuIJcnrkMmGGKfPwwel].controllers.conflictChecking.ElementAssignmentConflicts(HHyftJlGnirHuGQIQnIQkaXvgYegA, HIuFjgoQMMeZHtUafJbERKutTjQt, fxcwWCJuWyMdSnVzKKdavSrfSkCc).GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_00df;
							}
							return false;
							IL_00df:
							if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
							{
								ElementAssignmentConflictInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
							uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
							fIMVaffCgsuIJcnrkMmGGKfPwwel++;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
						{
							ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
						vasCQHBiPilUGNJZhGPwjCjftBeIB vasCQHBiPilUGNJZhGPwjCjftBeIB2;
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							vasCQHBiPilUGNJZhGPwjCjftBeIB2 = this;
						}
						else
						{
							vasCQHBiPilUGNJZhGPwjCjftBeIB2 = new vasCQHBiPilUGNJZhGPwjCjftBeIB(0);
						}
						vasCQHBiPilUGNJZhGPwjCjftBeIB2.HHyftJlGnirHuGQIQnIQkaXvgYegA = MelcAnrraPFJzeIaADSAOKRmqdwDb;
						vasCQHBiPilUGNJZhGPwjCjftBeIB2.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
						vasCQHBiPilUGNJZhGPwjCjftBeIB2.fxcwWCJuWyMdSnVzKKdavSrfSkCc = AIUEvmkpzzZEGHiPnuHpyDYEjNVE;
						vasCQHBiPilUGNJZhGPwjCjftBeIB2.LsArpUAaycAWpQMssIwwpGxYhCYd = EqRTwgfjsYypsrTblHXtvnwSZMnf;
						return vasCQHBiPilUGNJZhGPwjCjftBeIB2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class TDAlrBUSJPvPNosCaDVkScZBJRXC : IDisposable, IEnumerable, IEnumerator, IEnumerable<ElementAssignmentConflictInfo>, IEnumerator<ElementAssignmentConflictInfo>
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ElementAssignmentConflictInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					private int DljjufeDxHKtQhGDvoMcJRNEJSNE;

					public int LPJODyrZpNxPzXAMrfCKobQaJWhD;

					private ActionElementMap IZGLeBjXaoohNXDnyqGgBnrFgcfw;

					public ActionElementMap ElKoGEhTJYZThMFOOdRumLXCMPDE;

					private bool LsArpUAaycAWpQMssIwwpGxYhCYd;

					public bool EqRTwgfjsYypsrTblHXtvnwSZMnf;

					private int dxNWoKAWRnEjMzagpBqSNtmzbbJFA;

					public int wYvEyNiUnzAaMlalJwSaPpoqwWXHA;

					private JoystickMap QNctLPxloztIePsRkfyJcgjGIKffA;

					public JoystickMap FUzhWsmbUqzGnKLlnGCfVluuTcJU;

					private bool HIuFjgoQMMeZHtUafJbERKutTjQt;

					public bool MlWZQPQtLyJnETcXcYBEkbNZbonN;

					private bool fxcwWCJuWyMdSnVzKKdavSrfSkCc;

					public bool AIUEvmkpzzZEGHiPnuHpyDYEjNVE;

					private IList<Player> BKKFaUdfuegBblGWCXFcTsceicYAb;

					private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

					private IEnumerator<ElementAssignmentConflictInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public TDAlrBUSJPvPNosCaDVkScZBJRXC(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
						if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
						{
							try
							{
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
							{
								if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
								{
									return false;
								}
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_00e1;
							}
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							if (DljjufeDxHKtQhGDvoMcJRNEJSNE < 0 || IZGLeBjXaoohNXDnyqGgBnrFgcfw == null)
							{
								return false;
							}
							BKKFaUdfuegBblGWCXFcTsceicYAb = (LsArpUAaycAWpQMssIwwpGxYhCYd ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
							fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
							goto IL_010b;
							IL_010b:
							if (fIMVaffCgsuIJcnrkMmGGKfPwwel < BKKFaUdfuegBblGWCXFcTsceicYAb.Count)
							{
								ffMVLALuuURkVdxHSgrNcNQkUFZS = BKKFaUdfuegBblGWCXFcTsceicYAb[fIMVaffCgsuIJcnrkMmGGKfPwwel].controllers.conflictChecking.ElementAssignmentConflicts(ControllerType.Joystick, dxNWoKAWRnEjMzagpBqSNtmzbbJFA, QNctLPxloztIePsRkfyJcgjGIKffA, IZGLeBjXaoohNXDnyqGgBnrFgcfw, HIuFjgoQMMeZHtUafJbERKutTjQt, fxcwWCJuWyMdSnVzKKdavSrfSkCc).GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_00e1;
							}
							return false;
							IL_00e1:
							if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
							{
								ElementAssignmentConflictInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
							uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
							fIMVaffCgsuIJcnrkMmGGKfPwwel++;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
						{
							ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
						TDAlrBUSJPvPNosCaDVkScZBJRXC tDAlrBUSJPvPNosCaDVkScZBJRXC;
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							tDAlrBUSJPvPNosCaDVkScZBJRXC = this;
						}
						else
						{
							tDAlrBUSJPvPNosCaDVkScZBJRXC = new TDAlrBUSJPvPNosCaDVkScZBJRXC(0);
						}
						tDAlrBUSJPvPNosCaDVkScZBJRXC.DljjufeDxHKtQhGDvoMcJRNEJSNE = LPJODyrZpNxPzXAMrfCKobQaJWhD;
						tDAlrBUSJPvPNosCaDVkScZBJRXC.dxNWoKAWRnEjMzagpBqSNtmzbbJFA = wYvEyNiUnzAaMlalJwSaPpoqwWXHA;
						tDAlrBUSJPvPNosCaDVkScZBJRXC.QNctLPxloztIePsRkfyJcgjGIKffA = FUzhWsmbUqzGnKLlnGCfVluuTcJU;
						tDAlrBUSJPvPNosCaDVkScZBJRXC.IZGLeBjXaoohNXDnyqGgBnrFgcfw = ElKoGEhTJYZThMFOOdRumLXCMPDE;
						tDAlrBUSJPvPNosCaDVkScZBJRXC.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
						tDAlrBUSJPvPNosCaDVkScZBJRXC.fxcwWCJuWyMdSnVzKKdavSrfSkCc = AIUEvmkpzzZEGHiPnuHpyDYEjNVE;
						tDAlrBUSJPvPNosCaDVkScZBJRXC.LsArpUAaycAWpQMssIwwpGxYhCYd = EqRTwgfjsYypsrTblHXtvnwSZMnf;
						return tDAlrBUSJPvPNosCaDVkScZBJRXC;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class mxceWoHUIqeUxieeTTfBQoojYKHv : IDisposable, IEnumerable, IEnumerator, IEnumerable<ElementAssignmentConflictInfo>, IEnumerator<ElementAssignmentConflictInfo>
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ElementAssignmentConflictInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					private ElementAssignmentConflictCheck HHyftJlGnirHuGQIQnIQkaXvgYegA;

					public ElementAssignmentConflictCheck MelcAnrraPFJzeIaADSAOKRmqdwDb;

					private bool LsArpUAaycAWpQMssIwwpGxYhCYd;

					public bool EqRTwgfjsYypsrTblHXtvnwSZMnf;

					private bool HIuFjgoQMMeZHtUafJbERKutTjQt;

					public bool MlWZQPQtLyJnETcXcYBEkbNZbonN;

					private bool fxcwWCJuWyMdSnVzKKdavSrfSkCc;

					public bool AIUEvmkpzzZEGHiPnuHpyDYEjNVE;

					private IList<Player> BKKFaUdfuegBblGWCXFcTsceicYAb;

					private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

					private IEnumerator<ElementAssignmentConflictInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public mxceWoHUIqeUxieeTTfBQoojYKHv(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
						if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
						{
							try
							{
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
							{
								if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
								{
									return false;
								}
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_00df;
							}
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							if (HHyftJlGnirHuGQIQnIQkaXvgYegA.playerId < 0 || HHyftJlGnirHuGQIQnIQkaXvgYegA.elementAssignmentType == ElementAssignmentType.KeyboardKey)
							{
								return false;
							}
							BKKFaUdfuegBblGWCXFcTsceicYAb = (LsArpUAaycAWpQMssIwwpGxYhCYd ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
							fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
							goto IL_0109;
							IL_0109:
							if (fIMVaffCgsuIJcnrkMmGGKfPwwel < BKKFaUdfuegBblGWCXFcTsceicYAb.Count)
							{
								ffMVLALuuURkVdxHSgrNcNQkUFZS = BKKFaUdfuegBblGWCXFcTsceicYAb[fIMVaffCgsuIJcnrkMmGGKfPwwel].controllers.conflictChecking.ElementAssignmentConflicts(HHyftJlGnirHuGQIQnIQkaXvgYegA, HIuFjgoQMMeZHtUafJbERKutTjQt, fxcwWCJuWyMdSnVzKKdavSrfSkCc).GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_00df;
							}
							return false;
							IL_00df:
							if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
							{
								ElementAssignmentConflictInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
							uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
							fIMVaffCgsuIJcnrkMmGGKfPwwel++;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
						{
							ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
						mxceWoHUIqeUxieeTTfBQoojYKHv mxceWoHUIqeUxieeTTfBQoojYKHv2;
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							mxceWoHUIqeUxieeTTfBQoojYKHv2 = this;
						}
						else
						{
							mxceWoHUIqeUxieeTTfBQoojYKHv2 = new mxceWoHUIqeUxieeTTfBQoojYKHv(0);
						}
						mxceWoHUIqeUxieeTTfBQoojYKHv2.HHyftJlGnirHuGQIQnIQkaXvgYegA = MelcAnrraPFJzeIaADSAOKRmqdwDb;
						mxceWoHUIqeUxieeTTfBQoojYKHv2.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
						mxceWoHUIqeUxieeTTfBQoojYKHv2.fxcwWCJuWyMdSnVzKKdavSrfSkCc = AIUEvmkpzzZEGHiPnuHpyDYEjNVE;
						mxceWoHUIqeUxieeTTfBQoojYKHv2.LsArpUAaycAWpQMssIwwpGxYhCYd = EqRTwgfjsYypsrTblHXtvnwSZMnf;
						return mxceWoHUIqeUxieeTTfBQoojYKHv2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class nVoULXghumEcyfAHUNBSCfsXKdArA : IDisposable, IEnumerable, IEnumerator, IEnumerable<ElementAssignmentConflictInfo>, IEnumerator<ElementAssignmentConflictInfo>
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ElementAssignmentConflictInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					private int DljjufeDxHKtQhGDvoMcJRNEJSNE;

					public int LPJODyrZpNxPzXAMrfCKobQaJWhD;

					private ActionElementMap IZGLeBjXaoohNXDnyqGgBnrFgcfw;

					public ActionElementMap ElKoGEhTJYZThMFOOdRumLXCMPDE;

					private bool LsArpUAaycAWpQMssIwwpGxYhCYd;

					public bool EqRTwgfjsYypsrTblHXtvnwSZMnf;

					private KeyboardMap kGvpgbedpfSteCoDmBIhADOcBQyHA;

					public KeyboardMap pRKRGLTTzhAvrzgKqdgcDhrkAYOX;

					private bool HIuFjgoQMMeZHtUafJbERKutTjQt;

					public bool MlWZQPQtLyJnETcXcYBEkbNZbonN;

					private bool fxcwWCJuWyMdSnVzKKdavSrfSkCc;

					public bool AIUEvmkpzzZEGHiPnuHpyDYEjNVE;

					private IList<Player> BKKFaUdfuegBblGWCXFcTsceicYAb;

					private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

					private IEnumerator<ElementAssignmentConflictInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public nVoULXghumEcyfAHUNBSCfsXKdArA(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
						if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
						{
							try
							{
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
							{
								if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
								{
									return false;
								}
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_00dc;
							}
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							if (DljjufeDxHKtQhGDvoMcJRNEJSNE < 0 || IZGLeBjXaoohNXDnyqGgBnrFgcfw == null)
							{
								return false;
							}
							BKKFaUdfuegBblGWCXFcTsceicYAb = (LsArpUAaycAWpQMssIwwpGxYhCYd ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
							fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
							goto IL_0106;
							IL_0106:
							if (fIMVaffCgsuIJcnrkMmGGKfPwwel < BKKFaUdfuegBblGWCXFcTsceicYAb.Count)
							{
								ffMVLALuuURkVdxHSgrNcNQkUFZS = BKKFaUdfuegBblGWCXFcTsceicYAb[fIMVaffCgsuIJcnrkMmGGKfPwwel].controllers.conflictChecking.ElementAssignmentConflicts(ControllerType.Keyboard, 0, kGvpgbedpfSteCoDmBIhADOcBQyHA, IZGLeBjXaoohNXDnyqGgBnrFgcfw, HIuFjgoQMMeZHtUafJbERKutTjQt, fxcwWCJuWyMdSnVzKKdavSrfSkCc).GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_00dc;
							}
							return false;
							IL_00dc:
							if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
							{
								ElementAssignmentConflictInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
							uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
							fIMVaffCgsuIJcnrkMmGGKfPwwel++;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
						{
							ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
						nVoULXghumEcyfAHUNBSCfsXKdArA nVoULXghumEcyfAHUNBSCfsXKdArA2;
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							nVoULXghumEcyfAHUNBSCfsXKdArA2 = this;
						}
						else
						{
							nVoULXghumEcyfAHUNBSCfsXKdArA2 = new nVoULXghumEcyfAHUNBSCfsXKdArA(0);
						}
						nVoULXghumEcyfAHUNBSCfsXKdArA2.DljjufeDxHKtQhGDvoMcJRNEJSNE = LPJODyrZpNxPzXAMrfCKobQaJWhD;
						nVoULXghumEcyfAHUNBSCfsXKdArA2.kGvpgbedpfSteCoDmBIhADOcBQyHA = pRKRGLTTzhAvrzgKqdgcDhrkAYOX;
						nVoULXghumEcyfAHUNBSCfsXKdArA2.IZGLeBjXaoohNXDnyqGgBnrFgcfw = ElKoGEhTJYZThMFOOdRumLXCMPDE;
						nVoULXghumEcyfAHUNBSCfsXKdArA2.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
						nVoULXghumEcyfAHUNBSCfsXKdArA2.fxcwWCJuWyMdSnVzKKdavSrfSkCc = AIUEvmkpzzZEGHiPnuHpyDYEjNVE;
						nVoULXghumEcyfAHUNBSCfsXKdArA2.LsArpUAaycAWpQMssIwwpGxYhCYd = EqRTwgfjsYypsrTblHXtvnwSZMnf;
						return nVoULXghumEcyfAHUNBSCfsXKdArA2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class IAHhOpDAMTIfSFtwWHMbhGSUtKXBA : IDisposable, IEnumerable, IEnumerator, IEnumerable<ElementAssignmentConflictInfo>, IEnumerator<ElementAssignmentConflictInfo>
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ElementAssignmentConflictInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					private ElementAssignmentConflictCheck HHyftJlGnirHuGQIQnIQkaXvgYegA;

					public ElementAssignmentConflictCheck MelcAnrraPFJzeIaADSAOKRmqdwDb;

					private bool LsArpUAaycAWpQMssIwwpGxYhCYd;

					public bool EqRTwgfjsYypsrTblHXtvnwSZMnf;

					private bool HIuFjgoQMMeZHtUafJbERKutTjQt;

					public bool MlWZQPQtLyJnETcXcYBEkbNZbonN;

					private bool fxcwWCJuWyMdSnVzKKdavSrfSkCc;

					public bool AIUEvmkpzzZEGHiPnuHpyDYEjNVE;

					private IList<Player> BKKFaUdfuegBblGWCXFcTsceicYAb;

					private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

					private IEnumerator<ElementAssignmentConflictInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public IAHhOpDAMTIfSFtwWHMbhGSUtKXBA(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
						if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
						{
							try
							{
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
							{
								if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
								{
									return false;
								}
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_00df;
							}
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							if (HHyftJlGnirHuGQIQnIQkaXvgYegA.playerId < 0 || HHyftJlGnirHuGQIQnIQkaXvgYegA.elementAssignmentType != ElementAssignmentType.KeyboardKey)
							{
								return false;
							}
							BKKFaUdfuegBblGWCXFcTsceicYAb = (LsArpUAaycAWpQMssIwwpGxYhCYd ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
							fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
							goto IL_0109;
							IL_0109:
							if (fIMVaffCgsuIJcnrkMmGGKfPwwel < BKKFaUdfuegBblGWCXFcTsceicYAb.Count)
							{
								ffMVLALuuURkVdxHSgrNcNQkUFZS = BKKFaUdfuegBblGWCXFcTsceicYAb[fIMVaffCgsuIJcnrkMmGGKfPwwel].controllers.conflictChecking.ElementAssignmentConflicts(HHyftJlGnirHuGQIQnIQkaXvgYegA, HIuFjgoQMMeZHtUafJbERKutTjQt, fxcwWCJuWyMdSnVzKKdavSrfSkCc).GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_00df;
							}
							return false;
							IL_00df:
							if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
							{
								ElementAssignmentConflictInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
							uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
							fIMVaffCgsuIJcnrkMmGGKfPwwel++;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
						{
							ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
						IAHhOpDAMTIfSFtwWHMbhGSUtKXBA iAHhOpDAMTIfSFtwWHMbhGSUtKXBA;
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							iAHhOpDAMTIfSFtwWHMbhGSUtKXBA = this;
						}
						else
						{
							iAHhOpDAMTIfSFtwWHMbhGSUtKXBA = new IAHhOpDAMTIfSFtwWHMbhGSUtKXBA(0);
						}
						iAHhOpDAMTIfSFtwWHMbhGSUtKXBA.HHyftJlGnirHuGQIQnIQkaXvgYegA = MelcAnrraPFJzeIaADSAOKRmqdwDb;
						iAHhOpDAMTIfSFtwWHMbhGSUtKXBA.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
						iAHhOpDAMTIfSFtwWHMbhGSUtKXBA.fxcwWCJuWyMdSnVzKKdavSrfSkCc = AIUEvmkpzzZEGHiPnuHpyDYEjNVE;
						iAHhOpDAMTIfSFtwWHMbhGSUtKXBA.LsArpUAaycAWpQMssIwwpGxYhCYd = EqRTwgfjsYypsrTblHXtvnwSZMnf;
						return iAHhOpDAMTIfSFtwWHMbhGSUtKXBA;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class BHVdDdqfJoFRCitCHvVZmPFjqYuf : IDisposable, IEnumerable, IEnumerator, IEnumerable<ElementAssignmentConflictInfo>, IEnumerator<ElementAssignmentConflictInfo>
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ElementAssignmentConflictInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					private int DljjufeDxHKtQhGDvoMcJRNEJSNE;

					public int LPJODyrZpNxPzXAMrfCKobQaJWhD;

					private ActionElementMap IZGLeBjXaoohNXDnyqGgBnrFgcfw;

					public ActionElementMap ElKoGEhTJYZThMFOOdRumLXCMPDE;

					private bool LsArpUAaycAWpQMssIwwpGxYhCYd;

					public bool EqRTwgfjsYypsrTblHXtvnwSZMnf;

					private MouseMap kChLcMuJtEcaCxjQmCepJSRrFQHc;

					public MouseMap MkNFnAVvjNWGUMeukDpFFHBtGBOGA;

					private bool HIuFjgoQMMeZHtUafJbERKutTjQt;

					public bool MlWZQPQtLyJnETcXcYBEkbNZbonN;

					private bool fxcwWCJuWyMdSnVzKKdavSrfSkCc;

					public bool AIUEvmkpzzZEGHiPnuHpyDYEjNVE;

					private IList<Player> BKKFaUdfuegBblGWCXFcTsceicYAb;

					private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

					private IEnumerator<ElementAssignmentConflictInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public BHVdDdqfJoFRCitCHvVZmPFjqYuf(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
						if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
						{
							try
							{
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
							{
								if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
								{
									return false;
								}
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_00dc;
							}
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							if (DljjufeDxHKtQhGDvoMcJRNEJSNE < 0 || IZGLeBjXaoohNXDnyqGgBnrFgcfw == null)
							{
								return false;
							}
							BKKFaUdfuegBblGWCXFcTsceicYAb = (LsArpUAaycAWpQMssIwwpGxYhCYd ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
							fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
							goto IL_0106;
							IL_0106:
							if (fIMVaffCgsuIJcnrkMmGGKfPwwel < BKKFaUdfuegBblGWCXFcTsceicYAb.Count)
							{
								ffMVLALuuURkVdxHSgrNcNQkUFZS = BKKFaUdfuegBblGWCXFcTsceicYAb[fIMVaffCgsuIJcnrkMmGGKfPwwel].controllers.conflictChecking.ElementAssignmentConflicts(ControllerType.Mouse, 0, kChLcMuJtEcaCxjQmCepJSRrFQHc, IZGLeBjXaoohNXDnyqGgBnrFgcfw, HIuFjgoQMMeZHtUafJbERKutTjQt, fxcwWCJuWyMdSnVzKKdavSrfSkCc).GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_00dc;
							}
							return false;
							IL_00dc:
							if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
							{
								ElementAssignmentConflictInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
							uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
							fIMVaffCgsuIJcnrkMmGGKfPwwel++;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
						{
							ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
						BHVdDdqfJoFRCitCHvVZmPFjqYuf bHVdDdqfJoFRCitCHvVZmPFjqYuf;
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							bHVdDdqfJoFRCitCHvVZmPFjqYuf = this;
						}
						else
						{
							bHVdDdqfJoFRCitCHvVZmPFjqYuf = new BHVdDdqfJoFRCitCHvVZmPFjqYuf(0);
						}
						bHVdDdqfJoFRCitCHvVZmPFjqYuf.DljjufeDxHKtQhGDvoMcJRNEJSNE = LPJODyrZpNxPzXAMrfCKobQaJWhD;
						bHVdDdqfJoFRCitCHvVZmPFjqYuf.kChLcMuJtEcaCxjQmCepJSRrFQHc = MkNFnAVvjNWGUMeukDpFFHBtGBOGA;
						bHVdDdqfJoFRCitCHvVZmPFjqYuf.IZGLeBjXaoohNXDnyqGgBnrFgcfw = ElKoGEhTJYZThMFOOdRumLXCMPDE;
						bHVdDdqfJoFRCitCHvVZmPFjqYuf.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
						bHVdDdqfJoFRCitCHvVZmPFjqYuf.fxcwWCJuWyMdSnVzKKdavSrfSkCc = AIUEvmkpzzZEGHiPnuHpyDYEjNVE;
						bHVdDdqfJoFRCitCHvVZmPFjqYuf.LsArpUAaycAWpQMssIwwpGxYhCYd = EqRTwgfjsYypsrTblHXtvnwSZMnf;
						return bHVdDdqfJoFRCitCHvVZmPFjqYuf;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class ytjiGsKxOQtVUZYCNKiVfAhUxIXC : IDisposable, IEnumerable, IEnumerator, IEnumerable<ElementAssignmentConflictInfo>, IEnumerator<ElementAssignmentConflictInfo>
				{
					private int RxAoyfYzYDsYonLGXsvUgwChukLk;

					private ElementAssignmentConflictInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

					private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

					private ElementAssignmentConflictCheck HHyftJlGnirHuGQIQnIQkaXvgYegA;

					public ElementAssignmentConflictCheck MelcAnrraPFJzeIaADSAOKRmqdwDb;

					private bool LsArpUAaycAWpQMssIwwpGxYhCYd;

					public bool EqRTwgfjsYypsrTblHXtvnwSZMnf;

					private bool HIuFjgoQMMeZHtUafJbERKutTjQt;

					public bool MlWZQPQtLyJnETcXcYBEkbNZbonN;

					private bool fxcwWCJuWyMdSnVzKKdavSrfSkCc;

					public bool AIUEvmkpzzZEGHiPnuHpyDYEjNVE;

					private IList<Player> BKKFaUdfuegBblGWCXFcTsceicYAb;

					private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

					private IEnumerator<ElementAssignmentConflictInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VqEePGSMyrGKIqkWibsjeHcWPSIx;
						}
					}

					[DebuggerHidden]
					public ytjiGsKxOQtVUZYCNKiVfAhUxIXC(int P_0)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
						wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
						if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
						{
							try
							{
							}
							finally
							{
								uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							}
						}
					}

					private bool MoveNext()
					{
						try
						{
							int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
							if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
							{
								if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
								{
									return false;
								}
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_00df;
							}
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							if (HHyftJlGnirHuGQIQnIQkaXvgYegA.playerId < 0 || HHyftJlGnirHuGQIQnIQkaXvgYegA.elementAssignmentType == ElementAssignmentType.KeyboardKey)
							{
								return false;
							}
							BKKFaUdfuegBblGWCXFcTsceicYAb = (LsArpUAaycAWpQMssIwwpGxYhCYd ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
							fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
							goto IL_0109;
							IL_0109:
							if (fIMVaffCgsuIJcnrkMmGGKfPwwel < BKKFaUdfuegBblGWCXFcTsceicYAb.Count)
							{
								ffMVLALuuURkVdxHSgrNcNQkUFZS = BKKFaUdfuegBblGWCXFcTsceicYAb[fIMVaffCgsuIJcnrkMmGGKfPwwel].controllers.conflictChecking.ElementAssignmentConflicts(HHyftJlGnirHuGQIQnIQkaXvgYegA, HIuFjgoQMMeZHtUafJbERKutTjQt, fxcwWCJuWyMdSnVzKKdavSrfSkCc).GetEnumerator();
								RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
								goto IL_00df;
							}
							return false;
							IL_00df:
							if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
							{
								ElementAssignmentConflictInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
								VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
							uvbwjgAeXsWySvtbvsLpuFVuAgJB();
							ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
							fIMVaffCgsuIJcnrkMmGGKfPwwel++;
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

					private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
						{
							ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
						ytjiGsKxOQtVUZYCNKiVfAhUxIXC ytjiGsKxOQtVUZYCNKiVfAhUxIXC2;
						if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
							ytjiGsKxOQtVUZYCNKiVfAhUxIXC2 = this;
						}
						else
						{
							ytjiGsKxOQtVUZYCNKiVfAhUxIXC2 = new ytjiGsKxOQtVUZYCNKiVfAhUxIXC(0);
						}
						ytjiGsKxOQtVUZYCNKiVfAhUxIXC2.HHyftJlGnirHuGQIQnIQkaXvgYegA = MelcAnrraPFJzeIaADSAOKRmqdwDb;
						ytjiGsKxOQtVUZYCNKiVfAhUxIXC2.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
						ytjiGsKxOQtVUZYCNKiVfAhUxIXC2.fxcwWCJuWyMdSnVzKKdavSrfSkCc = AIUEvmkpzzZEGHiPnuHpyDYEjNVE;
						ytjiGsKxOQtVUZYCNKiVfAhUxIXC2.LsArpUAaycAWpQMssIwwpGxYhCYd = EqRTwgfjsYypsrTblHXtvnwSZMnf;
						return ytjiGsKxOQtVUZYCNKiVfAhUxIXC2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private static ConflictCheckingHelper LDnMtBpByMhNcmNpPzdzLRPijZRi;

				internal static ConflictCheckingHelper izfChqvjKyhCcypSioMPzwuCmhEV => LDnMtBpByMhNcmNpPzdzLRPijZRi ?? (LDnMtBpByMhNcmNpPzdzLRPijZRi = new ConflictCheckingHelper());

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
					IList<Player> list = (includeSystemPlayer ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
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
						ControllerType.Joystick => ymnLFQhXWNLmBMBQVJkYrHmiVwIN(playerId, controllerId, controllerMap as JoystickMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Keyboard => RMJsdMpFNwNTgKwfoCyFUFCCewYX(playerId, controllerMap as KeyboardMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Mouse => NgeGWAepIMRdAGGrkCkwhHxaQozvA(playerId, controllerMap as MouseMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Custom => cROBuCiptkEbkPmUNDwokTzcqynw(playerId, controllerId, controllerMap as CustomControllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
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
						return ymnLFQhXWNLmBMBQVJkYrHmiVwIN(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Keyboard)
					{
						return RMJsdMpFNwNTgKwfoCyFUFCCewYX(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Mouse)
					{
						return NgeGWAepIMRdAGGrkCkwhHxaQozvA(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Custom)
					{
						return cROBuCiptkEbkPmUNDwokTzcqynw(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					throw new NotImplementedException();
				}

				private bool ymnLFQhXWNLmBMBQVJkYrHmiVwIN(int P_0, int P_1, JoystickMap P_2, ActionElementMap P_3, bool P_4 = false, bool P_5 = false, bool P_6 = true)
				{
					if (P_0 < 0 || P_3 == null)
					{
						return false;
					}
					IList<Player> list = (P_6 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].controllers.conflictChecking.DoesElementAssignmentConflict(ControllerType.Joystick, P_1, P_2, P_3, P_4, P_5))
						{
							return true;
						}
					}
					return false;
				}

				private bool ymnLFQhXWNLmBMBQVJkYrHmiVwIN(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return false;
					}
					IList<Player> list = (P_3 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].controllers.conflictChecking.DoesElementAssignmentConflict(P_0, P_1, P_2))
						{
							return true;
						}
					}
					return false;
				}

				private bool RMJsdMpFNwNTgKwfoCyFUFCCewYX(int P_0, KeyboardMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false, bool P_5 = true)
				{
					if (P_0 < 0 || P_2 == null)
					{
						return false;
					}
					IList<Player> list = (P_5 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].controllers.conflictChecking.DoesElementAssignmentConflict(ControllerType.Keyboard, 0, P_1, P_2, P_3, P_4))
						{
							return true;
						}
					}
					return false;
				}

				private bool RMJsdMpFNwNTgKwfoCyFUFCCewYX(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType != ElementAssignmentType.KeyboardKey)
					{
						return false;
					}
					IList<Player> list = (P_3 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].controllers.conflictChecking.DoesElementAssignmentConflict(P_0, P_1, P_2))
						{
							return true;
						}
					}
					return false;
				}

				private bool NgeGWAepIMRdAGGrkCkwhHxaQozvA(int P_0, MouseMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false, bool P_5 = true)
				{
					if (P_0 < 0 || P_2 == null)
					{
						return false;
					}
					IList<Player> list = (P_5 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].controllers.conflictChecking.DoesElementAssignmentConflict(ControllerType.Mouse, 0, P_1, P_2, P_3, P_4))
						{
							return true;
						}
					}
					return false;
				}

				private bool NgeGWAepIMRdAGGrkCkwhHxaQozvA(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return false;
					}
					IList<Player> list = (P_3 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].controllers.conflictChecking.DoesElementAssignmentConflict(P_0, P_1, P_2))
						{
							return true;
						}
					}
					return false;
				}

				private bool cROBuCiptkEbkPmUNDwokTzcqynw(int P_0, int P_1, CustomControllerMap P_2, ActionElementMap P_3, bool P_4 = false, bool P_5 = false, bool P_6 = true)
				{
					if (P_0 < 0 || P_3 == null)
					{
						return false;
					}
					IList<Player> list = (P_6 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].controllers.conflictChecking.DoesElementAssignmentConflict(ControllerType.Custom, P_1, P_2, P_3, P_4, P_5))
						{
							return true;
						}
					}
					return false;
				}

				private bool cROBuCiptkEbkPmUNDwokTzcqynw(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return false;
					}
					IList<Player> list = (P_3 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
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
						ControllerType.Joystick => znBuFfizqrLFBugWnfWLWnuWexbK(playerId, controllerId, controllerMap as JoystickMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Keyboard => AIiZaJtFBRDFazbZijQVwstGunqJ(playerId, controllerMap as KeyboardMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Mouse => phEmVSGjUGgXUoBFqwOulGhXfBwgA(playerId, controllerMap as MouseMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Custom => WIOsiAmzlYryvOWygGdlIaNvTGIr(playerId, controllerId, controllerMap as CustomControllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
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
						return znBuFfizqrLFBugWnfWLWnuWexbK(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Keyboard)
					{
						return AIiZaJtFBRDFazbZijQVwstGunqJ(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Mouse)
					{
						return phEmVSGjUGgXUoBFqwOulGhXfBwgA(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Custom)
					{
						return WIOsiAmzlYryvOWygGdlIaNvTGIr(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					throw new NotImplementedException();
				}

				private IEnumerable<ElementAssignmentConflictInfo> znBuFfizqrLFBugWnfWLWnuWexbK(int P_0, int P_1, JoystickMap P_2, ActionElementMap P_3, bool P_4 = false, bool P_5 = false, bool P_6 = true)
				{
					return new TDAlrBUSJPvPNosCaDVkScZBJRXC(-2)
					{
						LPJODyrZpNxPzXAMrfCKobQaJWhD = P_0,
						wYvEyNiUnzAaMlalJwSaPpoqwWXHA = P_1,
						FUzhWsmbUqzGnKLlnGCfVluuTcJU = P_2,
						ElKoGEhTJYZThMFOOdRumLXCMPDE = P_3,
						MlWZQPQtLyJnETcXcYBEkbNZbonN = P_4,
						AIUEvmkpzzZEGHiPnuHpyDYEjNVE = P_5,
						EqRTwgfjsYypsrTblHXtvnwSZMnf = P_6
					};
				}

				private IEnumerable<ElementAssignmentConflictInfo> znBuFfizqrLFBugWnfWLWnuWexbK(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					return new mxceWoHUIqeUxieeTTfBQoojYKHv(-2)
					{
						MelcAnrraPFJzeIaADSAOKRmqdwDb = P_0,
						MlWZQPQtLyJnETcXcYBEkbNZbonN = P_1,
						AIUEvmkpzzZEGHiPnuHpyDYEjNVE = P_2,
						EqRTwgfjsYypsrTblHXtvnwSZMnf = P_3
					};
				}

				private IEnumerable<ElementAssignmentConflictInfo> AIiZaJtFBRDFazbZijQVwstGunqJ(int P_0, KeyboardMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false, bool P_5 = true)
				{
					return new nVoULXghumEcyfAHUNBSCfsXKdArA(-2)
					{
						LPJODyrZpNxPzXAMrfCKobQaJWhD = P_0,
						pRKRGLTTzhAvrzgKqdgcDhrkAYOX = P_1,
						ElKoGEhTJYZThMFOOdRumLXCMPDE = P_2,
						MlWZQPQtLyJnETcXcYBEkbNZbonN = P_3,
						AIUEvmkpzzZEGHiPnuHpyDYEjNVE = P_4,
						EqRTwgfjsYypsrTblHXtvnwSZMnf = P_5
					};
				}

				private IEnumerable<ElementAssignmentConflictInfo> AIiZaJtFBRDFazbZijQVwstGunqJ(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					return new IAHhOpDAMTIfSFtwWHMbhGSUtKXBA(-2)
					{
						MelcAnrraPFJzeIaADSAOKRmqdwDb = P_0,
						MlWZQPQtLyJnETcXcYBEkbNZbonN = P_1,
						AIUEvmkpzzZEGHiPnuHpyDYEjNVE = P_2,
						EqRTwgfjsYypsrTblHXtvnwSZMnf = P_3
					};
				}

				private IEnumerable<ElementAssignmentConflictInfo> phEmVSGjUGgXUoBFqwOulGhXfBwgA(int P_0, MouseMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false, bool P_5 = true)
				{
					return new BHVdDdqfJoFRCitCHvVZmPFjqYuf(-2)
					{
						LPJODyrZpNxPzXAMrfCKobQaJWhD = P_0,
						MkNFnAVvjNWGUMeukDpFFHBtGBOGA = P_1,
						ElKoGEhTJYZThMFOOdRumLXCMPDE = P_2,
						MlWZQPQtLyJnETcXcYBEkbNZbonN = P_3,
						AIUEvmkpzzZEGHiPnuHpyDYEjNVE = P_4,
						EqRTwgfjsYypsrTblHXtvnwSZMnf = P_5
					};
				}

				private IEnumerable<ElementAssignmentConflictInfo> phEmVSGjUGgXUoBFqwOulGhXfBwgA(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					return new ytjiGsKxOQtVUZYCNKiVfAhUxIXC(-2)
					{
						MelcAnrraPFJzeIaADSAOKRmqdwDb = P_0,
						MlWZQPQtLyJnETcXcYBEkbNZbonN = P_1,
						AIUEvmkpzzZEGHiPnuHpyDYEjNVE = P_2,
						EqRTwgfjsYypsrTblHXtvnwSZMnf = P_3
					};
				}

				private IEnumerable<ElementAssignmentConflictInfo> WIOsiAmzlYryvOWygGdlIaNvTGIr(int P_0, int P_1, CustomControllerMap P_2, ActionElementMap P_3, bool P_4 = false, bool P_5 = false, bool P_6 = true)
				{
					return new WNGBiFaXYHJWEqVXHglvFOFXoBWEb(-2)
					{
						LPJODyrZpNxPzXAMrfCKobQaJWhD = P_0,
						wYvEyNiUnzAaMlalJwSaPpoqwWXHA = P_1,
						fsLFWnOwyMFecKtbquAxUpiyvKDPA = P_2,
						ElKoGEhTJYZThMFOOdRumLXCMPDE = P_3,
						MlWZQPQtLyJnETcXcYBEkbNZbonN = P_4,
						AIUEvmkpzzZEGHiPnuHpyDYEjNVE = P_5,
						EqRTwgfjsYypsrTblHXtvnwSZMnf = P_6
					};
				}

				private IEnumerable<ElementAssignmentConflictInfo> WIOsiAmzlYryvOWygGdlIaNvTGIr(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					return new vasCQHBiPilUGNJZhGPwjCjftBeIB(-2)
					{
						MelcAnrraPFJzeIaADSAOKRmqdwDb = P_0,
						MlWZQPQtLyJnETcXcYBEkbNZbonN = P_1,
						AIUEvmkpzzZEGHiPnuHpyDYEjNVE = P_2,
						EqRTwgfjsYypsrTblHXtvnwSZMnf = P_3
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
						ControllerType.Joystick => CGFdGObbsyFxlykUDZWsmkteJIcc(playerId, controllerId, controllerMap as JoystickMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Keyboard => ncAmdpgnlUsLKPNiZuGfcRVfSqNb(playerId, controllerMap as KeyboardMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Mouse => NQkMArDaKiGPhTLFFrabAwrAKPGe(playerId, controllerMap as MouseMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Custom => TVZvfLolErGOlzstQxEbtdiWeGeH(playerId, controllerId, controllerMap as CustomControllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
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
						return CGFdGObbsyFxlykUDZWsmkteJIcc(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Keyboard)
					{
						return ncAmdpgnlUsLKPNiZuGfcRVfSqNb(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Mouse)
					{
						return NQkMArDaKiGPhTLFFrabAwrAKPGe(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Custom)
					{
						return TVZvfLolErGOlzstQxEbtdiWeGeH(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					throw new NotImplementedException();
				}

				private int CGFdGObbsyFxlykUDZWsmkteJIcc(int P_0, int P_1, JoystickMap P_2, ActionElementMap P_3, bool P_4 = false, bool P_5 = false, bool P_6 = true)
				{
					if (P_0 < 0 || P_3 == null)
					{
						return 0;
					}
					IList<Player> list = (P_6 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.RemoveElementAssignmentConflicts(ControllerType.Joystick, P_1, P_2, P_3, P_4, P_5);
					}
					return num;
				}

				private int CGFdGObbsyFxlykUDZWsmkteJIcc(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					IList<Player> list = (P_3 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.RemoveElementAssignmentConflicts(P_0, P_1, P_2);
					}
					return num;
				}

				private int ncAmdpgnlUsLKPNiZuGfcRVfSqNb(int P_0, KeyboardMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false, bool P_5 = true)
				{
					if (P_0 < 0 || P_2 == null)
					{
						return 0;
					}
					IList<Player> list = (P_5 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.RemoveElementAssignmentConflicts(ControllerType.Keyboard, 0, P_1, P_2, P_3, P_4);
					}
					return num;
				}

				private int ncAmdpgnlUsLKPNiZuGfcRVfSqNb(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType != ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					IList<Player> list = (P_3 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.RemoveElementAssignmentConflicts(P_0, P_1, P_2);
					}
					return num;
				}

				private int NQkMArDaKiGPhTLFFrabAwrAKPGe(int P_0, MouseMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false, bool P_5 = true)
				{
					if (P_0 < 0 || P_2 == null)
					{
						return 0;
					}
					IList<Player> list = (P_5 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.RemoveElementAssignmentConflicts(ControllerType.Mouse, 0, P_1, P_2, P_3, P_4);
					}
					return num;
				}

				private int NQkMArDaKiGPhTLFFrabAwrAKPGe(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					IList<Player> list = (P_3 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.RemoveElementAssignmentConflicts(P_0, P_1, P_2);
					}
					return num;
				}

				private int TVZvfLolErGOlzstQxEbtdiWeGeH(int P_0, int P_1, CustomControllerMap P_2, ActionElementMap P_3, bool P_4 = false, bool P_5 = false, bool P_6 = true)
				{
					if (P_0 < 0 || P_3 == null)
					{
						return 0;
					}
					IList<Player> list = (P_6 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.RemoveElementAssignmentConflicts(ControllerType.Custom, P_1, P_2, P_3, P_4, P_5);
					}
					return num;
				}

				private int TVZvfLolErGOlzstQxEbtdiWeGeH(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					IList<Player> list = (P_3 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
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
						ControllerType.Joystick => qCUfdLEmBburXJkElGEdPXHYHzARA(playerId, controllerId, controllerMap as JoystickMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Keyboard => dQUgbuFpmKHOeSPKNCxLWQpEsQpf(playerId, controllerMap as KeyboardMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Mouse => GPGSYUklIBOtdgkFIuelUtTlLUYs(playerId, controllerMap as MouseMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
						ControllerType.Custom => PfkrvNRyjUqYoeHZeFiWwxVomJvE(playerId, controllerId, controllerMap as CustomControllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer), 
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
						return qCUfdLEmBburXJkElGEdPXHYHzARA(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Keyboard)
					{
						return dQUgbuFpmKHOeSPKNCxLWQpEsQpf(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Mouse)
					{
						return GPGSYUklIBOtdgkFIuelUtTlLUYs(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					if (conflictCheck.controllerType == ControllerType.Custom)
					{
						return PfkrvNRyjUqYoeHZeFiWwxVomJvE(conflictCheck, skipDisabledMaps, forceCheckAllCategories, includeSystemPlayer);
					}
					throw new NotImplementedException();
				}

				private int qCUfdLEmBburXJkElGEdPXHYHzARA(int P_0, int P_1, JoystickMap P_2, ActionElementMap P_3, bool P_4 = false, bool P_5 = false, bool P_6 = true)
				{
					if (P_0 < 0 || P_3 == null)
					{
						return 0;
					}
					IList<Player> list = (P_6 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.DisableElementAssignmentConflicts(ControllerType.Joystick, P_1, P_2, P_3, P_4, P_5);
					}
					return num;
				}

				private int qCUfdLEmBburXJkElGEdPXHYHzARA(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					IList<Player> list = (P_3 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.DisableElementAssignmentConflicts(P_0, P_1, P_2);
					}
					return num;
				}

				private int dQUgbuFpmKHOeSPKNCxLWQpEsQpf(int P_0, KeyboardMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false, bool P_5 = true)
				{
					if (P_0 < 0 || P_2 == null)
					{
						return 0;
					}
					IList<Player> list = (P_5 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.DisableElementAssignmentConflicts(ControllerType.Keyboard, 0, P_1, P_2, P_3, P_4);
					}
					return num;
				}

				private int dQUgbuFpmKHOeSPKNCxLWQpEsQpf(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType != ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					IList<Player> list = (P_3 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.DisableElementAssignmentConflicts(P_0, P_1, P_2);
					}
					return num;
				}

				private int GPGSYUklIBOtdgkFIuelUtTlLUYs(int P_0, MouseMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false, bool P_5 = true)
				{
					if (P_0 < 0 || P_2 == null)
					{
						return 0;
					}
					IList<Player> list = (P_5 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.DisableElementAssignmentConflicts(ControllerType.Mouse, 0, P_1, P_2, P_3, P_4);
					}
					return num;
				}

				private int GPGSYUklIBOtdgkFIuelUtTlLUYs(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					IList<Player> list = (P_3 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.DisableElementAssignmentConflicts(P_0, P_1, P_2);
					}
					return num;
				}

				private int PfkrvNRyjUqYoeHZeFiWwxVomJvE(int P_0, int P_1, CustomControllerMap P_2, ActionElementMap P_3, bool P_4 = false, bool P_5 = false, bool P_6 = true)
				{
					if (P_0 < 0 || P_3 == null)
					{
						return 0;
					}
					IList<Player> list = (P_6 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.DisableElementAssignmentConflicts(ControllerType.Custom, P_1, P_2, P_3, P_4, P_5);
					}
					return num;
				}

				private int PfkrvNRyjUqYoeHZeFiWwxVomJvE(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, bool P_3 = true)
				{
					if (P_0.playerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					IList<Player> list = (P_3 ? hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA : hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ);
					int num = 0;
					for (int i = 0; i < list.Count; i++)
					{
						num += list[i].controllers.conflictChecking.DisableElementAssignmentConflicts(P_0, P_1, P_2);
					}
					return num;
				}
			}

			private static ControllerHelper LDnMtBpByMhNcmNpPzdzLRPijZRi;

			public readonly PollingHelper polling = PollingHelper.izfChqvjKyhCcypSioMPzwuCmhEV;

			public readonly ConflictCheckingHelper conflictChecking = ConflictCheckingHelper.izfChqvjKyhCcypSioMPzwuCmhEV;

			internal static ControllerHelper izfChqvjKyhCcypSioMPzwuCmhEV => LDnMtBpByMhNcmNpPzdzLRPijZRi ?? (LDnMtBpByMhNcmNpPzdzLRPijZRi = new ControllerHelper());

			public int controllerCount
			{
				get
				{
					if (!CheckInitialized())
					{
						return 0;
					}
					return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.XAxgQwEvifPgbHUznCFCtyEbWwOtA;
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
					return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.FXqycrxiIdSgUWNefithenzRnAiDb;
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
					return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.rgqCDZHatmAjYfIausJQUnRSqVpAA;
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
					return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.OurrBprwSKVEMTXHNSeMGLuBHall;
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
					return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.OeeHBecrguVtJOJkdEjZKpQgYwls;
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
					return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.FMULSNPMOjtlSugUpKDpvqOlAjyl;
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
					return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.GAMwoDaktPkGWjOteLxMlJnoastv;
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
					return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.XoVatgitzcrXSMTZlyGxtxxDoZSr;
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
					return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.OurrBprwSKVEMTXHNSeMGLuBHall as T;
				}
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(CustomController)))
				{
					return GetCustomController(controllerId) as T;
				}
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Mouse)))
				{
					return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.rgqCDZHatmAjYfIausJQUnRSqVpAA as T;
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
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.IzoAZhLSJPdQcJxvWLgBrvgaJNMKA(controllerType, controllerId);
			}

			public Controller GetController(ControllerIdentifier controllerIdentifier)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.IzoAZhLSJPdQcJxvWLgBrvgaJNMKA(controllerIdentifier);
			}

			public Controller[] GetControllers(ControllerType controllerType)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<Controller>.array;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.nzQQovtPuMkcgOGwQubivSJhAFlx(controllerType);
			}

			public string[] GetControllerNames(ControllerType controllerType)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<string>.array;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.IyVdxTVDGtYxFqHRAhFWyOfLJNWm(controllerType);
			}

			public bool IsControllerAssigned(ControllerType controllerType, Controller controller)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.RaqNthvphttAgGIIabSXRCuGWBIl(controller);
			}

			public bool IsControllerAssigned(ControllerType controllerType, int controllerId)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.RaqNthvphttAgGIIabSXRCuGWBIl(controllerType, controllerId);
			}

			public bool IsControllerAssignedToPlayer(ControllerType controllerType, int controllerId, int playerId)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.gdVKWEYmfTArKpxaWVepHQidLbuF(controllerType, controllerId, playerId);
			}

			public void RemoveControllerFromAllPlayers(Controller controller, bool includeSystemPlayer = true)
			{
				if (CheckInitialized())
				{
					hqGXmYxZUrSrCxDobJTGEPCWvHAsA.LWrxtiSdTIxHkxgXOOIRRipxOuaE(controller, includeSystemPlayer);
				}
			}

			public void RemoveControllerFromAllPlayers(ControllerType controllerType, int controllerId, bool includeSystemPlayer = true)
			{
				if (CheckInitialized())
				{
					hqGXmYxZUrSrCxDobJTGEPCWvHAsA.LWrxtiSdTIxHkxgXOOIRRipxOuaE(controllerType, controllerId, includeSystemPlayer);
				}
			}

			public Joystick GetJoystick(int joystickId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.ojzheXTjdCUaEzozjVQUFinqCht(joystickId);
			}

			public Joystick[] GetJoysticks()
			{
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.FlyseoUFWWdQIBiWngLFaaOSOfwHb();
			}

			public string[] GetJoystickNames()
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<string>.array;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.gtXizTltiHBZLUKHFdFDkemIjcZY();
			}

			public bool IsJoystickAssigned(Joystick joystick)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.voAbaiSenJzcoJXjSwTrqVlucavU(joystick);
			}

			public bool IsJoystickAssigned(int joystickId)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.voAbaiSenJzcoJXjSwTrqVlucavU(joystickId);
			}

			public bool IsJoystickAssignedToPlayer(int joystickId, int playerId)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.LmjBCLHxwMetbFeBHpVClYJbrGjJc(joystickId, playerId);
			}

			public void RemoveJoystickFromAllPlayers(Joystick joystick, bool includeSystemPlayer = true)
			{
				if (CheckInitialized())
				{
					hqGXmYxZUrSrCxDobJTGEPCWvHAsA.LBrJBEsxmdmIQJAcusUxcQSRcmIG(joystick, includeSystemPlayer);
				}
			}

			public void RemoveJoystickFromAllPlayers(int joystickId, bool includeSystemPlayer = true)
			{
				if (CheckInitialized())
				{
					hqGXmYxZUrSrCxDobJTGEPCWvHAsA.LBrJBEsxmdmIQJAcusUxcQSRcmIG(joystickId, includeSystemPlayer);
				}
			}

			public int GetUnityJoystickIdFromAnyButtonPress()
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				if (!ccufkjAWAhTbJaDuyGebaJsLxHKUA)
				{
					Logger.LogWarning("This can only used when Unity Input is handling input. This has no effect on this platform.");
					return -1;
				}
				vvnGZHfUStdhjUsYlsLiYRHwhKXg();
				for (int i = 0; i < 16; i++)
				{
					for (int j = 0; j < 20; j++)
					{
						if (QgbIinFdaCcKNDxZdcNgHuxnvnzHb.bmcGOvypZBGMiaYHnsomqOQcWvEoA(i, j))
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
				if (!ccufkjAWAhTbJaDuyGebaJsLxHKUA)
				{
					Logger.LogWarning("This can only used when Unity Input is handling input. This has no effect on this platform.");
					return -1;
				}
				vvnGZHfUStdhjUsYlsLiYRHwhKXg();
				for (int i = 0; i < 16; i++)
				{
					for (int j = 0; j < 20; j++)
					{
						if (QgbIinFdaCcKNDxZdcNgHuxnvnzHb.bmcGOvypZBGMiaYHnsomqOQcWvEoA(i, j))
						{
							return i + 1;
						}
					}
					for (int k = 0; k < 29; k++)
					{
						if (QgbIinFdaCcKNDxZdcNgHuxnvnzHb.HLQdiZXhKRobIEOHRYhcAiSwwYrn(i, k, positiveAxesOnly))
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
					if (!ccufkjAWAhTbJaDuyGebaJsLxHKUA)
					{
						Logger.LogWarning("This can only used when Unity Input is handling input. This has no effect on this platform.");
					}
					else
					{
						CTpVLYlUovEEvUpcNlivRncrrQbW.SetUnityJoystickId(joystickId, unityJoystickId);
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
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.UhFBEoKYbICWKGrfPbNBAMYDdcqzA(customControllerId);
			}

			public CustomController[] GetCustomControllers()
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<CustomController>.array;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.xRsdIcKLBoMJoRCkGuUDaFVkcLvs();
			}

			public string[] GetCustomControllerNames()
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<string>.array;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.zDkghtyeRYxdcAfWYOgOCoOMPalb();
			}

			public bool IsCustomControllerAssigned(CustomController customController)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.kziJJZFacpjCcYYBwzoCbEKEKuzq(customController);
			}

			public bool IsCustomControllerAssigned(int customControllerId)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.kziJJZFacpjCcYYBwzoCbEKEKuzq(customControllerId);
			}

			public bool IsCustomControllerAssignedToPlayer(int customControllerId, int playerId)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.zvWNUbsyYlpZeNcbcfzBpxHFvdSJ(customControllerId, playerId);
			}

			public void RemoveCustomControllerFromAllPlayers(CustomController customController, bool includeSystemPlayer = true)
			{
				if (CheckInitialized())
				{
					hqGXmYxZUrSrCxDobJTGEPCWvHAsA.YyoVnjzGVxqGIqdlPQgQqQnVhvGu(customController, includeSystemPlayer);
				}
			}

			public void RemoveCustomControllerFromAllPlayers(int customControllerId, bool includeSystemPlayer = true)
			{
				if (CheckInitialized())
				{
					hqGXmYxZUrSrCxDobJTGEPCWvHAsA.YyoVnjzGVxqGIqdlPQgQqQnVhvGu(customControllerId, includeSystemPlayer);
				}
			}

			public CustomController CreateCustomController(int sourceControllerId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.labQcDriOISpTvNogGqpTBqIeJDA(sourceControllerId);
			}

			public CustomController CreateCustomController(int sourceControllerId, string tag)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				CustomController customController = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.labQcDriOISpTvNogGqpTBqIeJDA(sourceControllerId);
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
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.hgJXJmdxNMqcjJmsiLmfzAnuBfpbA(customController);
			}

			public CustomController GetFirstCustomControllerWithSourceId(int sourceId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.weNaberKoEhMzGELeiozzgiHRPtHA(sourceId);
			}

			public CustomController GetFirstCustomControllerWithTag(string tag)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.pCJeiYSRNApAXrfmZBHqQVEAIQzg(tag);
			}

			public IEnumerable<CustomController> CustomControllersWithSourceId(int sourceId)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<CustomController>.EmptyReadOnlyIListT;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.whXOtSlXGtPSNIikcNUrRlaeaFKIA(sourceId);
			}

			public IEnumerable<CustomController> CustomControllersWithTag(string tag)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<CustomController>.EmptyReadOnlyIListT;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.UZaCrMKFZHYrqgcALBkFZJwZoOsi(tag);
			}

			public IList<TInterface> GetControllerTemplates<TInterface>() where TInterface : IControllerTemplate
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<TInterface>.EmptyReadOnlyIListT;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.yiiCShGioGtoKQqhkzTTNUynLASJ<TInterface>();
			}

			public Controller GetLastActiveController()
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.hVkRNMWgAYioJtcYTeDknaJRusNi();
			}

			public Controller GetLastActiveController(ControllerType controllerType)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.hVkRNMWgAYioJtcYTeDknaJRusNi(controllerType);
			}

			public T GetLastActiveController<T>() where T : Controller
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.hVkRNMWgAYioJtcYTeDknaJRusNi<T>();
			}

			public ControllerType GetLastActiveControllerType()
			{
				if (!CheckInitialized())
				{
					return ControllerType.Keyboard;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.xqJfJvckYUGwxCjPVXdZBHEjcqvrA();
			}

			public void AddLastActiveControllerChangedDelegate(ActiveControllerChangedDelegate callback)
			{
				if (CheckInitialized())
				{
					VmqcbbbvPImXBEBcMHWjUAXVfrUSA.ffjZabrkykFoYthIJjudFzxTDcXdA(callback);
				}
			}

			public void AddLastActiveControllerChangedDelegate(ActiveControllerChangedDelegate callback, ControllerType controllerType)
			{
				if (CheckInitialized())
				{
					VmqcbbbvPImXBEBcMHWjUAXVfrUSA.ffjZabrkykFoYthIJjudFzxTDcXdA(callback, controllerType);
				}
			}

			public void RemoveLastActiveControllerChangedDelegate(ActiveControllerChangedDelegate callback)
			{
				if (CheckInitialized())
				{
					VmqcbbbvPImXBEBcMHWjUAXVfrUSA.vGCWQiJKsuezzBAZLoskYhZekgebb(callback);
				}
			}

			public void RemoveLastActiveControllerChangedDelegate(ActiveControllerChangedDelegate callback, ControllerType controllerType)
			{
				if (CheckInitialized())
				{
					VmqcbbbvPImXBEBcMHWjUAXVfrUSA.AKcoZthCUaHCaaCsWVsXriIBiHuY(callback, controllerType);
				}
			}

			public void ClearLastActiveControllerChangedDelegates()
			{
				if (CheckInitialized())
				{
					VmqcbbbvPImXBEBcMHWjUAXVfrUSA.QNWANzBRZPmxOCLzNtEOxRAVONPaA();
				}
			}

			public bool GetAnyButton()
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.BFKZawnovHBiifPPnuuSRDawTwze();
			}

			public bool GetAnyButton(ControllerType controllerType)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.BFKZawnovHBiifPPnuuSRDawTwze(controllerType);
			}

			public bool GetAnyButtonDown()
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.VSTjcPRPJuIMVohKVeAMxLRVDISe();
			}

			public bool GetAnyButtonDown(ControllerType controllerType)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.VSTjcPRPJuIMVohKVeAMxLRVDISe(controllerType);
			}

			public bool GetAnyButtonUp()
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.huUGSZvDeDGtAwKwPyflEhkiDTSU();
			}

			public bool GetAnyButtonUp(ControllerType controllerType)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.huUGSZvDeDGtAwKwPyflEhkiDTSU(controllerType);
			}

			public bool GetAnyButtonChanged()
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.cwGisSwHogQrGVvEJHgYIEyuPaOv();
			}

			public bool GetAnyButtonChanged(ControllerType controllerType)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.cwGisSwHogQrGVvEJHgYIEyuPaOv(controllerType);
			}

			public bool GetAnyButtonPrev()
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.tCLNIawgRirkkeTAwgbgLCflRZH();
			}

			public bool GetAnyButtonPrev(ControllerType controllerType)
			{
				if (!CheckInitialized())
				{
					return false;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.tCLNIawgRirkkeTAwgbgLCflRZH(controllerType);
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
				hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OjIFqmaNarRMdzDSkOpVffgHGfVxA(joystick);
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

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public sealed class MappingHelper : CodeHelper
		{
			private static MappingHelper LDnMtBpByMhNcmNpPzdzLRPijZRi;

			internal static MappingHelper izfChqvjKyhCcypSioMPzwuCmhEV => LDnMtBpByMhNcmNpPzdzLRPijZRi ?? (LDnMtBpByMhNcmNpPzdzLRPijZRi = new MappingHelper());

			public IList<InputMapCategory> MapCategories
			{
				get
				{
					if (!CheckInitialized())
					{
						return EmptyObjects<InputMapCategory>.EmptyReadOnlyIListT;
					}
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.RSXfoCPlVhAqzWjiPGWRnMihRjmd;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.pLKbazjnyHBagGHAVFxumHJDFxgQ;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.OCPXiYawvnhuDUIpVLMqzhQlYhSf;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.eeieCjFPvCkMvhgikkHPegWBZQERe;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.vyeJQWQbdYUeKBxeVafPMcqiYtME;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.KxMHtHeQmhHNKLltHJceXACHrZpV;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.ZyzyetomWfCQRSNHBxYMQdjdhWAs;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.lkOlJCNOJlpoOygUHGghIOtFqrdrA;
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
					return GAwkqrvWlvpikRXvCLOCwZfKeMsC.pKMZFxQiAHeZkCfHZqCmwzClFZlQ;
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
					return afarNIRJlvcfXIrnUwLxEgdcbDEjA.uokisPQPtevGpzdvwIlJxaBPFzbB;
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
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetMapCategoryById(mapCategoryId);
			}

			public InputMapCategory GetMapCategory(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetMapCategory(name);
			}

			public int GetMapCategoryId(string name)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetMapCategoryId(name);
			}

			public IEnumerable<InputMapCategory> MapCategoriesWithTag(string tag)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputMapCategory>.EmptyReadOnlyIListT;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.tNIcUmxLngoAzMHyIqmvCwvRNGJc(tag);
			}

			public IEnumerable<InputMapCategory> UserAssignableMapCategoriesWithTag(string tag)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputMapCategory>.EmptyReadOnlyIListT;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.LLEhlxTnovXrgJamrJPyyoxmBYYx(tag);
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
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetActionCategoryById(mapCategoryId);
			}

			public InputCategory GetActionCategory(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetActionCategory(name);
			}

			public int GetActionCategoryId(string name)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetActionCategoryId(name);
			}

			public IEnumerable<InputCategory> ActionCategoriesWithTag(string tag)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputCategory>.EmptyReadOnlyIListT;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.FtbyKBcUriukmsLuMgkIjnVAwSxfA(tag);
			}

			public IEnumerable<InputCategory> UserAssignableActionCategoriesWithTag(string tag)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputCategory>.EmptyReadOnlyIListT;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.nvCnollClWWQVpygolJAIGrAvCqc(tag);
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
					ControllerType.Joystick => afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetJoystickLayoutById(layoutId), 
					ControllerType.Keyboard => afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetKeyboardLayoutById(layoutId), 
					ControllerType.Mouse => afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetMouseLayoutById(layoutId), 
					ControllerType.Custom => afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetCustomControllerLayoutById(layoutId), 
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
					ControllerType.Joystick => afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetJoystickLayout(name), 
					ControllerType.Keyboard => afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetKeyboardLayout(name), 
					ControllerType.Mouse => afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetMouseLayout(name), 
					ControllerType.Custom => afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetCustomControllerLayout(name), 
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
					ControllerType.Joystick => afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetJoystickLayoutId(name), 
					ControllerType.Keyboard => afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetKeyboardLayoutId(name), 
					ControllerType.Mouse => afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetMouseLayoutId(name), 
					ControllerType.Custom => afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetCustomControllerLayoutId(name), 
					_ => throw new NotImplementedException(), 
				};
			}

			public InputLayout GetJoystickLayout(int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetJoystickLayoutById(layoutId);
			}

			public InputLayout GetJoystickLayout(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetJoystickLayout(name);
			}

			public int GetJoystickLayoutId(string name)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetJoystickLayoutId(name);
			}

			public InputLayout GetKeyboardLayout(int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetKeyboardLayoutById(layoutId);
			}

			public InputLayout GetKeyboardLayout(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetKeyboardLayout(name);
			}

			public int GetKeyboardLayoutId(string name)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetKeyboardLayoutId(name);
			}

			public InputLayout GetMouseLayout(int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetMouseLayoutById(layoutId);
			}

			public InputLayout GetMouseLayout(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetMouseLayout(name);
			}

			public int GetMouseLayoutId(string name)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetMouseLayoutId(name);
			}

			public InputLayout GetCustomControllerLayout(int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetCustomControllerLayoutById(layoutId);
			}

			public InputLayout GetCustomControllerLayout(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetCustomControllerLayout(name);
			}

			public int GetCustomControllerLayoutId(string name)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetCustomControllerLayoutId(name);
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
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetActionById(actionId);
			}

			public InputAction GetAction(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetAction(name);
			}

			public int GetActionId(string name)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetActionId(name);
			}

			public IEnumerable<InputAction> ActionsInCategory(string mapCategoryName)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.YFWdcTWVATPjnOxbSRpOHEWQCkKEA(mapCategoryName, false);
			}

			public IEnumerable<InputAction> ActionsInCategory(string mapCategoryName, bool sort)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.YFWdcTWVATPjnOxbSRpOHEWQCkKEA(mapCategoryName, sort);
			}

			public IEnumerable<InputAction> ActionsInCategory(int mapCategoryId)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.YFWdcTWVATPjnOxbSRpOHEWQCkKEA(mapCategoryId, false);
			}

			public IEnumerable<InputAction> ActionsInCategory(int mapCategoryId, bool sort)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.YFWdcTWVATPjnOxbSRpOHEWQCkKEA(mapCategoryId, sort);
			}

			public IEnumerable<InputAction> ActionsInCategoriesWithTag(string tag)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.HDlCkRPcvrfiGagWJebaOEdJgObk(tag);
			}

			public IEnumerable<InputAction> UserAssignableActionsInCategory(int mapCategoryId)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.pYcfKZCKtilaPxDoEnkWooQHxgInA(mapCategoryId, false);
			}

			public IEnumerable<InputAction> UserAssignableActionsInCategory(int mapCategoryId, bool sort)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.pYcfKZCKtilaPxDoEnkWooQHxgInA(mapCategoryId, sort);
			}

			public IEnumerable<InputAction> UserAssignableActionsInCategory(string mapCategoryName)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.pYcfKZCKtilaPxDoEnkWooQHxgInA(mapCategoryName, false);
			}

			public IEnumerable<InputAction> UserAssignableActionsInCategory(string mapCategoryName, bool sort)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputAction>.EmptyReadOnlyIListT;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.pYcfKZCKtilaPxDoEnkWooQHxgInA(mapCategoryName, sort);
			}

			public IList<InputBehavior> GetInputBehaviors(int playerId)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputBehavior>.EmptyReadOnlyIListT;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.hrFktKFZmNkMtXheIgPWJTjSvNng(playerId);
			}

			public IList<InputBehavior> GetSystemPlayerInputBehaviors()
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<InputBehavior>.EmptyReadOnlyIListT;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.hrFktKFZmNkMtXheIgPWJTjSvNng(9999999);
			}

			public InputBehavior GetInputBehavior(int playerId, int behaviorId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.uwwouvQdjxpfTIRrJNyUxsHLWDAC(playerId, behaviorId);
			}

			public InputBehavior GetInputBehavior(int playerId, string behaviorName)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.uwwouvQdjxpfTIRrJNyUxsHLWDAC(playerId, behaviorName);
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
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetInputBehaviorId(behaviorName);
			}

			internal InputBehavior kRhNFszrrvRsbYXXXIXgWSMseAZb(int P_0)
			{
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetInputBehaviorById(P_0);
			}

			internal InputBehavior kRhNFszrrvRsbYXXXIXgWSMseAZb(string P_0)
			{
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetInputBehavior(P_0);
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
				Controller controller = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.IzoAZhLSJPdQcJxvWLgBrvgaJNMKA(controllerIdentifier);
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
				JoystickMap joystickMap = afarNIRJlvcfXIrnUwLxEgdcbDEjA.WHOFmQewBIbguHOagLmODlVoDgCP(joystick, mapCategoryId, layoutId);
				if (joystickMap != null)
				{
					joystick.dSUizcQaOGnPQOOBZVgCNLRSewAe(joystickMap);
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
				InputSource inputSourceType = CTpVLYlUovEEvUpcNlivRncrrQbW.inputSourceType;
				HardwareJoystickMap_InputManager hardwareJoystickMap_InputManager = hvtSbKLcAoLwFocNZGaXyDHVIyhp.igyTzPwKAtUKQePinKyePYFeMWpS(joystickTypeGuid, inputSourceType);
				if (hardwareJoystickMap_InputManager == null)
				{
					Logger.LogError("No hardware map found.");
					return null;
				}
				JoystickMap joystickMap = afarNIRJlvcfXIrnUwLxEgdcbDEjA.rSuwQVxddIUsjiemBrsQiuZXYnhL(hardwareJoystickMap_InputManager.hardwareMapIdentifier, mapCategoryId, layoutId);
				if (joystickMap != null)
				{
					HardwareControllerMap_Game hardwareControllerMap_Game = hardwareJoystickMap_InputManager.ToGameHardwareControllerMap();
					foreach (ActionElementMap allMap in joystickMap.AllMaps)
					{
						allMap.RPQWwIDvDftjNUjQfuErQNYMCCXf(joystickMap, hardwareControllerMap_Game);
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
				if (VmqcbbbvPImXBEBcMHWjUAXVfrUSA.IzoAZhLSJPdQcJxvWLgBrvgaJNMKA(controllerIdentifier) is Joystick joystick)
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
				KeyboardMap keyboardMap = afarNIRJlvcfXIrnUwLxEgdcbDEjA.FindKeyboardMap_Game(controllers.Keyboard, mapCategoryId, layoutId);
				if (keyboardMap != null)
				{
					controllers.Keyboard.dSUizcQaOGnPQOOBZVgCNLRSewAe(keyboardMap);
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
				MouseMap mouseMap = afarNIRJlvcfXIrnUwLxEgdcbDEjA.FindMouseMap_Game(controllers.Mouse, mapCategoryId, layoutId);
				if (mouseMap != null)
				{
					controllers.Mouse.dSUizcQaOGnPQOOBZVgCNLRSewAe(mouseMap);
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
				CustomControllerMap customControllerMap = afarNIRJlvcfXIrnUwLxEgdcbDEjA.vrRCeAkEWTOCEPPnLUEetfzUZqDnA(customController.sourceControllerId, mapCategoryId, layoutId);
				if (customControllerMap != null)
				{
					customController.dSUizcQaOGnPQOOBZVgCNLRSewAe(customControllerMap);
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
				if (VmqcbbbvPImXBEBcMHWjUAXVfrUSA.IzoAZhLSJPdQcJxvWLgBrvgaJNMKA(controllerIdentifier) is CustomController customController)
				{
					return GetCustomControllerMapInstance(customController, mapCategoryId, layoutId);
				}
				if (controllerIdentifier.hardwareTypeGuid == Guid.Empty)
				{
					return null;
				}
				CustomController_Editor customControllerByHardwareTypeGuid = afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetCustomControllerByHardwareTypeGuid(controllerIdentifier.hardwareTypeGuid);
				if (customControllerByHardwareTypeGuid == null)
				{
					return null;
				}
				CustomControllerMap customControllerMap = afarNIRJlvcfXIrnUwLxEgdcbDEjA.vrRCeAkEWTOCEPPnLUEetfzUZqDnA(controllerIdentifier.hardwareTypeGuid, mapCategoryId, layoutId);
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
						allMap.RPQWwIDvDftjNUjQfuErQNYMCCXf(customControllerMap, hardwareControllerMap_Game);
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
					controllerMap = afarNIRJlvcfXIrnUwLxEgdcbDEjA.joPhuTnAZZbFFbZAKsHmYkOQMBQJ(controller, mapCategoryId, layoutId);
				}
				if (controllerMap != null)
				{
					Player player = players.GetPlayer(playerId);
					if (player != null)
					{
						player.controllers.maps.dSUizcQaOGnPQOOBZVgCNLRSewAe(controller, controllerMap);
					}
					else
					{
						controller.dSUizcQaOGnPQOOBZVgCNLRSewAe(controllerMap);
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
				if (VmqcbbbvPImXBEBcMHWjUAXVfrUSA.IzoAZhLSJPdQcJxvWLgBrvgaJNMKA(controllerIdentifier) is Joystick joystick)
				{
					return GetJoystickMapInstanceSavedOrDefault(playerId, joystick, mapCategoryId, layoutId);
				}
				InputSource inputSourceType = CTpVLYlUovEEvUpcNlivRncrrQbW.inputSourceType;
				HardwareJoystickMap_InputManager hardwareJoystickMap_InputManager = hvtSbKLcAoLwFocNZGaXyDHVIyhp.igyTzPwKAtUKQePinKyePYFeMWpS(controllerIdentifier.hardwareTypeGuid, inputSourceType);
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
					joystickMap = afarNIRJlvcfXIrnUwLxEgdcbDEjA.rSuwQVxddIUsjiemBrsQiuZXYnhL(hardwareJoystickMap_InputManager.hardwareMapIdentifier, mapCategoryId, layoutId);
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
						allMap.RPQWwIDvDftjNUjQfuErQNYMCCXf(joystickMap, hardwareControllerMap_Game);
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
				if (VmqcbbbvPImXBEBcMHWjUAXVfrUSA.IzoAZhLSJPdQcJxvWLgBrvgaJNMKA(controllerIdentifier) is CustomController customController)
				{
					return GetCustomControllerMapInstanceSavedOrDefault(playerId, customController, mapCategoryId, layoutId);
				}
				CustomController_Editor customControllerByHardwareTypeGuid = afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetCustomControllerByHardwareTypeGuid(controllerIdentifier.hardwareTypeGuid);
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
					customControllerMap = afarNIRJlvcfXIrnUwLxEgdcbDEjA.vrRCeAkEWTOCEPPnLUEetfzUZqDnA(controllerIdentifier.hardwareTypeGuid, mapCategoryId, layoutId);
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
						allMap.RPQWwIDvDftjNUjQfuErQNYMCCXf(customControllerMap, hardwareControllerMap_Game);
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
					keyboardMap = afarNIRJlvcfXIrnUwLxEgdcbDEjA.FindKeyboardMap_Game(controllers.Keyboard, mapCategoryId, layoutId);
				}
				if (keyboardMap != null)
				{
					Player player = players.GetPlayer(playerId);
					if (player != null)
					{
						player.controllers.maps.dSUizcQaOGnPQOOBZVgCNLRSewAe(keyboard, keyboardMap);
					}
					else
					{
						keyboard.dSUizcQaOGnPQOOBZVgCNLRSewAe(keyboardMap);
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
					mouseMap = afarNIRJlvcfXIrnUwLxEgdcbDEjA.FindMouseMap_Game(controllers.Mouse, mapCategoryId, layoutId);
				}
				if (mouseMap != null)
				{
					Player player = players.GetPlayer(playerId);
					if (player != null)
					{
						player.controllers.maps.dSUizcQaOGnPQOOBZVgCNLRSewAe(mouse, mouseMap);
					}
					else
					{
						mouse.dSUizcQaOGnPQOOBZVgCNLRSewAe(mouseMap);
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
				return EUZRwgQIgYtQoFlxWBOXLFnVfPwl(joystick.hardwareTypeGuid, joystickElementIdentifierId);
			}

			private ControllerElementIdentifier EUZRwgQIgYtQoFlxWBOXLFnVfPwl(Guid P_0, int P_1)
			{
				return hvtSbKLcAoLwFocNZGaXyDHVIyhp.EUZRwgQIgYtQoFlxWBOXLFnVfPwl(P_0, P_1)?.ToControllerElementIdentifier();
			}

			public ControllerTemplateMap GetControllerTemplateMapInstance(Guid templateTypeGuid, int mapCategoryId, int layoutId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.NkbAmvqRrkSDcURXfqJuMaxmUEaE(templateTypeGuid, mapCategoryId, layoutId);
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
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetControllerMapLayoutManagerRuleSetById(id)?.ToRuntime();
			}

			public ControllerMapLayoutManager.RuleSet GetControllerMapLayoutManagerRuleSetInstance(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int controllerMapLayoutManagerRuleSetId = afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetControllerMapLayoutManagerRuleSetId(name);
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
				return afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetControllerMapEnablerRuleSetById(id)?.ToRuntime();
			}

			public ControllerMapEnabler.RuleSet GetControllerMapEnablerRuleSetInstance(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				int controllerMapEnablerRuleSetId = afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetControllerMapEnablerRuleSetId(name);
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
			private static PlayerHelper LDnMtBpByMhNcmNpPzdzLRPijZRi;

			internal static PlayerHelper izfChqvjKyhCcypSioMPzwuCmhEV => LDnMtBpByMhNcmNpPzdzLRPijZRi ?? (LDnMtBpByMhNcmNpPzdzLRPijZRi = new PlayerHelper());

			public int playerCount
			{
				get
				{
					if (!CheckInitialized())
					{
						return 0;
					}
					return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.CQIAQFpzVmISknegYFOIxRunxPpK;
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
					return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.jXskRzMcqiBuuZuSFMpqenJeANVp;
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
					return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ;
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
					return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA;
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
					return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.blBVoZZhgxiIOOqkchCQmhLeDxsEA();
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
					return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WEZyqeTcxBEnozDVUGnNzVZBqwqJ;
				}
				return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.OLLVcigGyhAbFaNDXiyTXurgpLPOA;
			}

			public Player GetPlayer(int playerId)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.ynWFEmrsktqGecVuJbofDaNeQFxn(playerId);
			}

			public Player GetPlayer(string name)
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.ynWFEmrsktqGecVuJbofDaNeQFxn(name);
			}

			public Player GetSystemPlayer()
			{
				if (!CheckInitialized())
				{
					return null;
				}
				return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.blBVoZZhgxiIOOqkchCQmhLeDxsEA();
			}

			public int GetPlayerId(string playerName)
			{
				if (!CheckInitialized())
				{
					return -1;
				}
				return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.ZCqHACovSplPzgbpXgZTFMQjMLOFb(playerName);
			}

			public string[] GetPlayerNames(bool includeSystemPlayer = false)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<string>.array;
				}
				return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.CbNUOSNNDybeWCluNpDcdFlpPdlc(includeSystemPlayer);
			}

			public string[] GetPlayerDescriptiveNames(bool includeSystemPlayer = false)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<string>.array;
				}
				return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.CIYITztLXRrClpnNvmgkHPLIqRAt(includeSystemPlayer);
			}

			public int[] GetPlayerIds(bool includeSystemPlayer = false)
			{
				if (!CheckInitialized())
				{
					return EmptyObjects<int>.array;
				}
				return hqGXmYxZUrSrCxDobJTGEPCWvHAsA.WVREjiFkAWoWTPKBmLDbFFyfZMij(includeSystemPlayer);
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class TimeHelper : CodeHelper
		{
			private static TimeHelper LDnMtBpByMhNcmNpPzdzLRPijZRi;

			internal static TimeHelper izfChqvjKyhCcypSioMPzwuCmhEV => LDnMtBpByMhNcmNpPzdzLRPijZRi ?? (LDnMtBpByMhNcmNpPzdzLRPijZRi = new TimeHelper());

			public float unscaledDeltaTime
			{
				get
				{
					if (!CheckInitialized())
					{
						return 0f;
					}
					return (float)EgFqVkhnQuwqWzKlxbTTqBIkwAdn.qDnfJdfXrGdKIjhGHpBGIytaTokcE;
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
					return EgFqVkhnQuwqWzKlxbTTqBIkwAdn.BHafZFWbiZCkNYpfoeSagMhWvnNK;
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
					return EgFqVkhnQuwqWzKlxbTTqBIkwAdn.XhKxIzDXGwEJghbHIgWHJwLzJsmD;
				}
			}

			private TimeHelper()
			{
			}
		}

		private class hZoSIeiGQlsNpxKnhkskatxNwdZA
		{
			private class kDKEfdfOLDciMiJECUQyUkOtQTwpB
			{
				public readonly UpdateLoopType PiEdbgjMHSsksYjRKSGckYZEsfAE;

				private double FcWClUEYqCkONndVVYEPBiKEVBNB;

				private double XZOBJpCdOwBGkZIyBYDcVtUfFFWmA;

				private double wZXFAliRaxWJujhJCKLDQTOTYZoc;

				private double NOBBHpOgmrCDwBpMuddlsRhuNmMRA;

				private uint NCglwbjCeCDmrtawbrrpCSJyjfSA;

				private uint UivYnXQnUJuOgczaZBaIEfHvkPAJ;

				private float dJqdUrXrkApZXanEYjNoMExGczlc;

				private float YoGxJgYUpoJzBxiNAHVJzSahzkLj;

				public double BHafZFWbiZCkNYpfoeSagMhWvnNK => FcWClUEYqCkONndVVYEPBiKEVBNB;

				public double fmqigLdmtKVJaOnXYivoVmzqcnJX => XZOBJpCdOwBGkZIyBYDcVtUfFFWmA;

				public double qDnfJdfXrGdKIjhGHpBGIytaTokcE => wZXFAliRaxWJujhJCKLDQTOTYZoc;

				public uint XhKxIzDXGwEJghbHIgWHJwLzJsmD => NCglwbjCeCDmrtawbrrpCSJyjfSA;

				public uint tbbIMaZcjtdSHBFrVZSJigTUSUABA => UivYnXQnUJuOgczaZBaIEfHvkPAJ;

				public float WAIiMBZlcYQBQMoohQvPidrhDnOZ => dJqdUrXrkApZXanEYjNoMExGczlc;

				public float kBbCjoDeqMFCajtnaIxyRQUFTynfA => YoGxJgYUpoJzBxiNAHVJzSahzkLj;

				public kDKEfdfOLDciMiJECUQyUkOtQTwpB(UpdateLoopType P_0)
				{
					PiEdbgjMHSsksYjRKSGckYZEsfAE = P_0;
					NOBBHpOgmrCDwBpMuddlsRhuNmMRA = Time.realtimeSinceStartup;
					NCglwbjCeCDmrtawbrrpCSJyjfSA = 0u;
				}

				public void jRaYtHNVcykNMAbqOnSGaKIIGSEaA()
				{
					XZOBJpCdOwBGkZIyBYDcVtUfFFWmA = FcWClUEYqCkONndVVYEPBiKEVBNB;
					FcWClUEYqCkONndVVYEPBiKEVBNB = realTime;
					if (NOBBHpOgmrCDwBpMuddlsRhuNmMRA > FcWClUEYqCkONndVVYEPBiKEVBNB)
					{
						NOBBHpOgmrCDwBpMuddlsRhuNmMRA = 0.0;
					}
					wZXFAliRaxWJujhJCKLDQTOTYZoc = FcWClUEYqCkONndVVYEPBiKEVBNB - NOBBHpOgmrCDwBpMuddlsRhuNmMRA;
					NOBBHpOgmrCDwBpMuddlsRhuNmMRA = FcWClUEYqCkONndVVYEPBiKEVBNB;
					UivYnXQnUJuOgczaZBaIEfHvkPAJ = NCglwbjCeCDmrtawbrrpCSJyjfSA;
					NCglwbjCeCDmrtawbrrpCSJyjfSA = MiscTools.Tick(NCglwbjCeCDmrtawbrrpCSJyjfSA);
					YoGxJgYUpoJzBxiNAHVJzSahzkLj = dJqdUrXrkApZXanEYjNoMExGczlc;
					dJqdUrXrkApZXanEYjNoMExGczlc = TpRDVeKUrDzPBCwrBJsvhbGodtGT();
					previousFrame = UivYnXQnUJuOgczaZBaIEfHvkPAJ;
					currentFrame = NCglwbjCeCDmrtawbrrpCSJyjfSA;
					unscaledTime = FcWClUEYqCkONndVVYEPBiKEVBNB;
					unscaledTimePrev = XZOBJpCdOwBGkZIyBYDcVtUfFFWmA;
					unscaledDeltaTime = wZXFAliRaxWJujhJCKLDQTOTYZoc;
				}
			}

			private static class TbDNSGwIxLAfdqScpHJWJUvftbIQ
			{
				public static StopwatchBase xalFtCqNqSQGgJNyNzskdnWqkjIL
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

				public static StopwatchBase lOzmglNwrCkddSvkHTjSvLufiURJ()
				{
					if (!UnityTools.isEditor && UnityTools.platform == Platform.XboxOne)
					{
						return new UnityStopwatch();
					}
					return new Rewired.Utils.Classes.Utility.Stopwatch();
				}

				public static StopwatchBase JgrmECpCbiQpRfGjRhlsjvGBETAn()
				{
					if (!UnityTools.isEditor && UnityTools.platform == Platform.XboxOne)
					{
						return UnityStopwatch.StartNew();
					}
					return Rewired.Utils.Classes.Utility.Stopwatch.StartNew();
				}
			}

			private StopwatchBase hArdDerumcBRzshORbjwoKPaIhvm;

			private double aEQkUBDfXMggCpBhlrKNfYCKOQFK;

			private kDKEfdfOLDciMiJECUQyUkOtQTwpB GIGZxQvhKXDENIiOdEmKJcLFFHANA;

			private ADictionary<int, kDKEfdfOLDciMiJECUQyUkOtQTwpB> fsZiqIxBodMcuysspsefSOuHjyKt;

			private uint AWyHCOmvqqiSaSAbNuHstXbiGTmf;

			public double BHafZFWbiZCkNYpfoeSagMhWvnNK => GIGZxQvhKXDENIiOdEmKJcLFFHANA.BHafZFWbiZCkNYpfoeSagMhWvnNK;

			public double fmqigLdmtKVJaOnXYivoVmzqcnJX => GIGZxQvhKXDENIiOdEmKJcLFFHANA.fmqigLdmtKVJaOnXYivoVmzqcnJX;

			public double qDnfJdfXrGdKIjhGHpBGIytaTokcE => GIGZxQvhKXDENIiOdEmKJcLFFHANA.qDnfJdfXrGdKIjhGHpBGIytaTokcE;

			public float WAIiMBZlcYQBQMoohQvPidrhDnOZ => GIGZxQvhKXDENIiOdEmKJcLFFHANA.WAIiMBZlcYQBQMoohQvPidrhDnOZ;

			public float kBbCjoDeqMFCajtnaIxyRQUFTynfA => GIGZxQvhKXDENIiOdEmKJcLFFHANA.kBbCjoDeqMFCajtnaIxyRQUFTynfA;

			internal double wEeKniBOCFcwArpfmZhOqVuhQdhP => hArdDerumcBRzshORbjwoKPaIhvm.elapsedSeconds + aEQkUBDfXMggCpBhlrKNfYCKOQFK;

			public uint XhKxIzDXGwEJghbHIgWHJwLzJsmD => GIGZxQvhKXDENIiOdEmKJcLFFHANA.XhKxIzDXGwEJghbHIgWHJwLzJsmD;

			public uint tbbIMaZcjtdSHBFrVZSJigTUSUABA => GIGZxQvhKXDENIiOdEmKJcLFFHANA.tbbIMaZcjtdSHBFrVZSJigTUSUABA;

			public uint ZvkPgetdtkFtGzpLfTpaUOOEAcNO => AWyHCOmvqqiSaSAbNuHstXbiGTmf;

			public hZoSIeiGQlsNpxKnhkskatxNwdZA()
			{
				hArdDerumcBRzshORbjwoKPaIhvm = TbDNSGwIxLAfdqScpHJWJUvftbIQ.xalFtCqNqSQGgJNyNzskdnWqkjIL;
				jpwugzufXqktYbXkMYboQpqCbQgL();
			}

			public void rZuOrpWYhhCfBPmUsQdcUinbQbVD()
			{
				aEQkUBDfXMggCpBhlrKNfYCKOQFK = Time.realtimeSinceStartup;
			}

			public void jpwugzufXqktYbXkMYboQpqCbQgL()
			{
				GIGZxQvhKXDENIiOdEmKJcLFFHANA = null;
				fsZiqIxBodMcuysspsefSOuHjyKt = new ADictionary<int, kDKEfdfOLDciMiJECUQyUkOtQTwpB>();
				using TempListPool.TList<UpdateLoopType> tList = TempListPool.GetTList<UpdateLoopType>(3);
				List<UpdateLoopType> list = tList.list;
				EnumConverter.ToUpdateLoopTypes((UpdateLoopSetting)2147483647, list);
				for (int i = 0; i < list.Count; i++)
				{
					kDKEfdfOLDciMiJECUQyUkOtQTwpB kDKEfdfOLDciMiJECUQyUkOtQTwpB2 = new kDKEfdfOLDciMiJECUQyUkOtQTwpB(list[i]);
					fsZiqIxBodMcuysspsefSOuHjyKt.Add((int)list[i], kDKEfdfOLDciMiJECUQyUkOtQTwpB2);
					if (GIGZxQvhKXDENIiOdEmKJcLFFHANA == null)
					{
						GIGZxQvhKXDENIiOdEmKJcLFFHANA = kDKEfdfOLDciMiJECUQyUkOtQTwpB2;
					}
				}
			}

			public void jRaYtHNVcykNMAbqOnSGaKIIGSEaA(UpdateLoopType P_0)
			{
				if (GIGZxQvhKXDENIiOdEmKJcLFFHANA.PiEdbgjMHSsksYjRKSGckYZEsfAE != P_0)
				{
					GIGZxQvhKXDENIiOdEmKJcLFFHANA = fsZiqIxBodMcuysspsefSOuHjyKt[(int)P_0];
				}
				if (P_0 != UpdateLoopType.OnGUI || Event.current.rawType == EventType.Layout)
				{
					GIGZxQvhKXDENIiOdEmKJcLFFHANA.jRaYtHNVcykNMAbqOnSGaKIIGSEaA();
					AWyHCOmvqqiSaSAbNuHstXbiGTmf = MiscTools.Tick(AWyHCOmvqqiSaSAbNuHstXbiGTmf);
					absFrame = AWyHCOmvqqiSaSAbNuHstXbiGTmf;
				}
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class UnityTouch : CodeHelper
		{
			private static UnityTouch LDnMtBpByMhNcmNpPzdzLRPijZRi;

			internal static UnityTouch izfChqvjKyhCcypSioMPzwuCmhEV => LDnMtBpByMhNcmNpPzdzLRPijZRi ?? (LDnMtBpByMhNcmNpPzdzLRPijZRi = new UnityTouch());

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

		internal class MqUEZjlwiIeKTuXxUzFWBUliKyLL
		{
			[Serializable]
			private sealed class yNqIGGPBgoDiQbHyTmBpJcgluJfsA
			{
				public static readonly yNqIGGPBgoDiQbHyTmBpJcgluJfsA _003C_003E9 = new yNqIGGPBgoDiQbHyTmBpJcgluJfsA();

				public static Func<bool> _003C_003E9__11_1;

				public static Func<bool> _003C_003E9__11_2;

				public static Func<int> _003C_003E9__11_3;

				public static Func<float> _003C_003E9__11_4;

				public static Func<bool> _003C_003E9__11_5;

				public static Func<string> _003C_003E9__11_0;

				internal bool tWDswhYAvBqHImkNJdYGpAwokPVM()
				{
					return Screen.fullScreen;
				}

				internal bool LBgFBZQrZLQqglpncuFkjnZgRpSO()
				{
					return Application.runInBackground;
				}

				internal int pNTdosrvgmmgqqQIYuBZbWKKoCpj()
				{
					return (int)Screen.fullScreenMode;
				}

				internal float AElOJWFyoEskqxqdgUiyBMRpImTB()
				{
					return Time.unscaledDeltaTime;
				}

				internal bool IBnfeLEHNkAMvGXwOccFPpbKfgwiA()
				{
					return MathTools.ApproximatelyZero(Time.timeScale);
				}

				internal string fqfnoFdSaoYewLmeTldQXFozeXiq()
				{
					return UnityTools.externalTools.GetFocusedEditorWindowTitle();
				}
			}

			public readonly ValueWatcher<bool> UZYSHsTJwyaEEFondgqwlzyMavdk;

			public readonly ValueWatcher<bool> KYiOUlwMqmWUeZyXbFwxCXGMMnDJ;

			public readonly ValueWatcher<bool> vbOhiocIcDPNMFSYkWDgYncMpduHA;

			public readonly ValueWatcher<int> bYcrVqtLPhbPOoKrkaPlbHEWjLygb;

			public readonly ValueWatcher<float> qDnfJdfXrGdKIjhGHpBGIytaTokcE;

			public readonly ValueWatcher<string> CRuRampAWykoWdPZeqkFGVaRdTjs;

			public readonly ValueWatcher<bool> mYuWjVGPwctxkGRqeqjMVZiXqUQ;

			private int LqefkSWyanwezQfUHigehUsVcaio;

			private readonly ValueWatcher[] YCSshVVGDwHfgBfrWXhtGsZExlIu;

			public int gOuDkZUQNKJvrYshgNhmDOkegsQi => LqefkSWyanwezQfUHigehUsVcaio;

			public MqUEZjlwiIeKTuXxUzFWBUliKyLL()
			{
				List<ValueWatcher> list = new List<ValueWatcher>
				{
					(UZYSHsTJwyaEEFondgqwlzyMavdk = new ValueWatcher<bool>(true, false)),
					(KYiOUlwMqmWUeZyXbFwxCXGMMnDJ = new ValueWatcher<bool>(Screen.fullScreen, yNqIGGPBgoDiQbHyTmBpJcgluJfsA._003C_003E9.tWDswhYAvBqHImkNJdYGpAwokPVM, false)),
					(vbOhiocIcDPNMFSYkWDgYncMpduHA = new ValueWatcher<bool>(Application.runInBackground, yNqIGGPBgoDiQbHyTmBpJcgluJfsA._003C_003E9.LBgFBZQrZLQqglpncuFkjnZgRpSO, false)),
					(bYcrVqtLPhbPOoKrkaPlbHEWjLygb = new ValueWatcher<int>((int)Screen.fullScreenMode, yNqIGGPBgoDiQbHyTmBpJcgluJfsA._003C_003E9.pNTdosrvgmmgqqQIYuBZbWKKoCpj, false)),
					(qDnfJdfXrGdKIjhGHpBGIytaTokcE = new ValueWatcher<float>(Time.unscaledDeltaTime, yNqIGGPBgoDiQbHyTmBpJcgluJfsA._003C_003E9.AElOJWFyoEskqxqdgUiyBMRpImTB, false)),
					(mYuWjVGPwctxkGRqeqjMVZiXqUQ = new ValueWatcher<bool>(MathTools.ApproximatelyZero(Time.timeScale), yNqIGGPBgoDiQbHyTmBpJcgluJfsA._003C_003E9.IBnfeLEHNkAMvGXwOccFPpbKfgwiA, MathTools.ApproximatelyZero(Time.timeScale)))
				};
				if (editorPlatform != EditorPlatform.None)
				{
					list.Add(CRuRampAWykoWdPZeqkFGVaRdTjs = new ValueWatcher<string>(UnityTools.externalTools.GetFocusedEditorWindowTitle(), yNqIGGPBgoDiQbHyTmBpJcgluJfsA._003C_003E9.fqfnoFdSaoYewLmeTldQXFozeXiq, false));
				}
				YCSshVVGDwHfgBfrWXhtGsZExlIu = list.ToArray();
				jRaYtHNVcykNMAbqOnSGaKIIGSEaA();
			}

			public void jRaYtHNVcykNMAbqOnSGaKIIGSEaA()
			{
				for (int i = 0; i < YCSshVVGDwHfgBfrWXhtGsZExlIu.Length; i++)
				{
					YCSshVVGDwHfgBfrWXhtGsZExlIu[i].Update();
				}
				LqefkSWyanwezQfUHigehUsVcaio = Time.frameCount;
			}

			public void uNorXdRhAZJVgdORHzhewkLNilUr()
			{
				for (int i = 0; i < YCSshVVGDwHfgBfrWXhtGsZExlIu.Length; i++)
				{
					YCSshVVGDwHfgBfrWXhtGsZExlIu[i].TriggerEvent();
				}
			}
		}

		[Serializable]
		private sealed class cgEOizKXucFXVuDYJObcnLIdaeTG
		{
			public static readonly cgEOizKXucFXVuDYJObcnLIdaeTG _003C_003E9 = new cgEOizKXucFXVuDYJObcnLIdaeTG();

			public static Func<bool> _003C_003E9__221_0;

			internal void TDDiqmPleXAatJzZudtmfHVfcaTEA(Exception P_0)
			{
				HandleCallbackException("", P_0);
			}

			internal void ZfmGIXNjbXILurFfUjnNBqwPWzqR(Exception P_0)
			{
				HandleCallbackException("Rewired.ReInput.ControllerConnectedEvent", P_0);
			}

			internal void FCEhqTiwEbxeTVPvQspMepWjMzyAb(Exception P_0)
			{
				HandleCallbackException("Rewired.ReInput.ControllerPreDisconnectEvent", P_0);
			}

			internal void MfetaeDhesggpwtBdkNDnrxNUPKs(Exception P_0)
			{
				HandleCallbackException("Rewired.ReInput.ControllerDisconnectedEvent", P_0);
			}

			internal void ygyKQijsPTyjSmaEXkDpwzcfstbN(Exception P_0)
			{
				HandleCallbackException("Rewired.ReInput.InputSourceUpdateEvent", P_0);
			}

			internal void sKWexrMIZTGMdilvVozOUbVLJxcT(Exception P_0)
			{
				HandleCallbackException("Rewired.ReInput.EditorRecompileEvent", P_0);
			}

			internal void GFmRdWcpdvgqTwWvxiPXChNqNoBk(Exception P_0)
			{
				HandleCallbackException("Rewired.ReInput.PreShutDownEvent", P_0);
			}

			internal void bqWVBlqBuflPyHklEcVCgPsKGyEd(Exception P_0)
			{
				HandleCallbackException("Rewired.ReInput.ShutDownEvent", P_0);
			}

			internal void AArQVFcZfagUZcVGJKtdEEjyGOtjA(Exception P_0)
			{
				HandleCallbackException("Rewired.ReInput.InitializedEvent", P_0);
			}

			internal bool bKlGBRKPxYxDUcMjyzVSkpkneezIb()
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
		internal const int programVersion3 = 43;

		[CustomObfuscation(rename = false)]
		internal const int programVersion4 = 0;

		[CustomObfuscation(rename = false)]
		internal const int dataVersion = 1;

		[CustomObfuscation(rename = false)]
		internal const bool isTrial = false;

		[CustomObfuscation(rename = false)]
		internal const string majorBranch = "U2020";

		private static InputManager_Base HvlowarpEJjTwmVIKndWValQpypI;

		private static PlatformInputManager CTpVLYlUovEEvUpcNlivRncrrQbW;

		internal static nWObnmDQDyPAcKuVnyCIqGPjixDdb GAwkqrvWlvpikRXvCLOCwZfKeMsC;

		internal static pmhdIRhCoLlQZffSYgOmsLkpxsjN VmqcbbbvPImXBEBcMHWjUAXVfrUSA;

		internal static BBctKivJxEjGKRzyEkHSRjJoJqnW hqGXmYxZUrSrCxDobJTGEPCWvHAsA;

		private static ControllerDataFiles hvtSbKLcAoLwFocNZGaXyDHVIyhp;

		private static UserData afarNIRJlvcfXIrnUwLxEgdcbDEjA;

		private static bool obpzIVquVRQseulcTcTZaHvizTjRA;

		private static ConfigVars MzljsQKNnzXzAuzZSJhUefpPCcEy;

		private static UpdateLoopType JZKJGBsuSaOLhJiTvsRyNFkLxwMp;

		private static bool ccufkjAWAhTbJaDuyGebaJsLxHKUA;

		private static Platform uDnCOOlZZpUIrgXGeSbcMIctXMiG;

		private static WebplayerPlatform EWHwzexfoiyRFhLiBgPzrmmYiUKt;

		private static EditorPlatform unICjthLTqHPNnLSrXcmbvFaXzQQ;

		private static bool uncSVBAxHAmqUjuITaNtgwfiLSRd;

		private static TimerAbs HmHsEaZxDIDUXLKxzIztJkUaaoZm;

		private static hZoSIeiGQlsNpxKnhkskatxNwdZA EgFqVkhnQuwqWzKlxbTTqBIkwAdn;

		private static string qvEajoyMuISFgeEqGQeJiQKMKgmK;

		private static bool hCmEAcPmvppxEjhawxLgIZABpLik;

		private static bool GkmiXdOXsbIXKjeFpIDPABcVkAEp;

		private static bool ciaKHlkdfnxSFTEXSBgJHScCNEMbA;

		private static int UELUFrjaUxsAhaTllRzYIkUctOmj;

		[CustomObfuscation(rename = false)]
		internal static int _id;

		private static int eiTDtHWtAjacrCGIktMvceGodkmNA;

		private static int CDFHTYcVYyNjfwcbyMnOntgUhkKJ;

		private static bool DxzQhCtDRoyInHoxWcnNRxVqcFzB;

		private static readonly UnityTouch akZVbtgXAOvjVgWMOQHFJjdJMvoD;

		private static readonly PlayerHelper GdwWjDCOcxwJUizjrHagUJgJeCNJ;

		private static readonly ControllerHelper QpbBNmWrfInkcoGyYFaZIQVXTXrNA;

		private static readonly MappingHelper qIzETGiaCujiFKelzjMBwftDgerG;

		private static readonly TimeHelper ecdLtUBiFgzGXZaYvntixqlbMFjm;

		private static readonly ConfigHelper zFposRXLCpdyIpBgRutQBeayGyFI;

		private static WiGEESztaQcOlAVSzDYLfidomemMA apYTMDWGCXhutvjiTTLcQkjwiobJ;

		private static UserDataStore LPxDLELdQfiHxinMUfSElrZHFOEd;

		private static IControllerAssigner CDsiOyqanEANRJmTqOQLnofCbVXm;

		private static MqUEZjlwiIeKTuXxUzFWBUliKyLL KCnPsFgFLyiYyEYWEjTridcaKheJA;

		private static SafeAction<ControllerStatusChangedEventArgs> uFMSNayMaqnyuBvdoywREwUAsimj;

		private static SafeAction<ControllerStatusChangedEventArgs> RSMIOQyasZdEIGhSzpIEFbajfJYPc;

		private static SafeAction<ControllerStatusChangedEventArgs> JYXlYmJfMTgurfCCixbvqZlIozKP;

		private static SafeAction BbIMbQeLvHjivRxkUFEuuHyecJUe;

		private static SafeAction DoMcMTFZOcROcVWyENyvXEFgDWYm;

		private static SafeAction DxDDJgdGopEpAltRNiEBalwJEIfob;

		private static SafeAction BDmisuCgVCeghMSBpbXRvlOriVYBb;

		private static SafeAction vMWkXfFtLyWssWlmvPyaljgWTHhk;

		[CustomObfuscation(rename = false)]
		private static Action<bool> _ApplicationFocusChangedEvent;

		private static Action IzBYZMAEXTGrtOeLAlYNcYpctpQK;

		private static Action<UpdateLoopType> TrQuHFJWSbBEQkUuadDRDhOfcwRn;

		private static Action<UpdateLoopType> DIGTJvqzbyagBhANOgLCGihAWkgZ;

		private static Action<UpdateLoopType> RwMSqmrPEIcUWPMqqCGgofpACAIz;

		private static Action nEoJdFSxGZTTQgqVzwyiDJlZpkoT;

		private static Action<bool> yNbnOmzfriGRWQMmXbBnZbjOIcYA;

		private static Action<bool> TcVDZtTBkbCPvFcbwUNzUxWXdNzKA;

		private static Action<bool> EPCOWdjrRVZgEJREalGNkcBsOdV;

		private static Action<FullScreenMode> IQDcUMOkJbbJpvsxoxIaATuqmVFJ;

		private static Action atJFAUEGsWvXzxPSVIFgFPyLZccI;

		private static Action<bool> BHfOhXExzTwULJhJBFXiuCiXbVPK;

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

		private static WiGEESztaQcOlAVSzDYLfidomemMA QgbIinFdaCcKNDxZdcNgHuxnvnzHb => apYTMDWGCXhutvjiTTLcQkjwiobJ ?? (apYTMDWGCXhutvjiTTLcQkjwiobJ = new WiGEESztaQcOlAVSzDYLfidomemMA(MzljsQKNnzXzAuzZSJhUefpPCcEy.updateLoop));

		public static PlayerHelper players
		{
			get
			{
				if (!obpzIVquVRQseulcTcTZaHvizTjRA)
				{
					LJNkuIumYUrwPEaXMrTJGeZvMNxe();
					return null;
				}
				return GdwWjDCOcxwJUizjrHagUJgJeCNJ;
			}
		}

		public static ControllerHelper controllers
		{
			get
			{
				if (!obpzIVquVRQseulcTcTZaHvizTjRA)
				{
					LJNkuIumYUrwPEaXMrTJGeZvMNxe();
					return null;
				}
				return QpbBNmWrfInkcoGyYFaZIQVXTXrNA;
			}
		}

		public static MappingHelper mapping
		{
			get
			{
				if (!obpzIVquVRQseulcTcTZaHvizTjRA)
				{
					LJNkuIumYUrwPEaXMrTJGeZvMNxe();
					return null;
				}
				return qIzETGiaCujiFKelzjMBwftDgerG;
			}
		}

		public static UnityTouch touch
		{
			get
			{
				if (!obpzIVquVRQseulcTcTZaHvizTjRA)
				{
					LJNkuIumYUrwPEaXMrTJGeZvMNxe();
					return null;
				}
				return akZVbtgXAOvjVgWMOQHFJjdJMvoD;
			}
		}

		public static TimeHelper time
		{
			get
			{
				if (!obpzIVquVRQseulcTcTZaHvizTjRA)
				{
					LJNkuIumYUrwPEaXMrTJGeZvMNxe();
					return null;
				}
				return ecdLtUBiFgzGXZaYvntixqlbMFjm;
			}
		}

		public static IUserDataStore userDataStore
		{
			get
			{
				if (!obpzIVquVRQseulcTcTZaHvizTjRA)
				{
					LJNkuIumYUrwPEaXMrTJGeZvMNxe();
					return null;
				}
				return LPxDLELdQfiHxinMUfSElrZHFOEd;
			}
		}

		public static ConfigHelper configuration
		{
			get
			{
				if (!obpzIVquVRQseulcTcTZaHvizTjRA)
				{
					LJNkuIumYUrwPEaXMrTJGeZvMNxe();
					return null;
				}
				return zFposRXLCpdyIpBgRutQBeayGyFI;
			}
		}

		public static string programVersion => 1 + "." + 1 + "." + 43 + "." + 0 + ".U2020";

		public static bool usingUnityInput => ccufkjAWAhTbJaDuyGebaJsLxHKUA;

		public static bool unityJoystickIdentificationRequired
		{
			get
			{
				if (!obpzIVquVRQseulcTcTZaHvizTjRA)
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

		public static bool isReady => obpzIVquVRQseulcTcTZaHvizTjRA;

		[CustomObfuscation(rename = false)]
		internal static int id => _id;

		[CustomObfuscation(rename = false)]
		internal static bool initialized => obpzIVquVRQseulcTcTZaHvizTjRA;

		[CustomObfuscation(rename = false)]
		internal static UpdateLoopType currentUpdateLoop => JZKJGBsuSaOLhJiTvsRyNFkLxwMp;

		[CustomObfuscation(rename = false)]
		internal static ConfigVars configVars => MzljsQKNnzXzAuzZSJhUefpPCcEy;

		[CustomObfuscation(rename = false)]
		internal static IConfigVars_Internal pluginConfigVars => MzljsQKNnzXzAuzZSJhUefpPCcEy;

		[CustomObfuscation(rename = false)]
		internal static UserData UserData => afarNIRJlvcfXIrnUwLxEgdcbDEjA;

		[CustomObfuscation(rename = false)]
		internal static Platform currentPlatform => uDnCOOlZZpUIrgXGeSbcMIctXMiG;

		[CustomObfuscation(rename = false)]
		internal static WebplayerPlatform webplayerPlatform => EWHwzexfoiyRFhLiBgPzrmmYiUKt;

		[CustomObfuscation(rename = false)]
		internal static EditorPlatform editorPlatform => unICjthLTqHPNnLSrXcmbvFaXzQQ;

		[CustomObfuscation(rename = false)]
		internal static bool checkNeverPressed
		{
			get
			{
				if (uDnCOOlZZpUIrgXGeSbcMIctXMiG == Platform.Linux && ccufkjAWAhTbJaDuyGebaJsLxHKUA)
				{
					return true;
				}
				if (uDnCOOlZZpUIrgXGeSbcMIctXMiG == Platform.OSX && (ccufkjAWAhTbJaDuyGebaJsLxHKUA || primaryInputManager.inputSourceType == InputSource.OSX))
				{
					return true;
				}
				if (UnityTools.isAndroidPlatform && ccufkjAWAhTbJaDuyGebaJsLxHKUA)
				{
					return true;
				}
				if (uDnCOOlZZpUIrgXGeSbcMIctXMiG == Platform.Webplayer && EWHwzexfoiyRFhLiBgPzrmmYiUKt == WebplayerPlatform.OSX)
				{
					return true;
				}
				if (uDnCOOlZZpUIrgXGeSbcMIctXMiG == Platform.WebGL)
				{
					return true;
				}
				return false;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool isEditor => unICjthLTqHPNnLSrXcmbvFaXzQQ != EditorPlatform.None;

		[CustomObfuscation(rename = false)]
		internal static Guid defaultHardwareJoystickMapGuid
		{
			get
			{
				if (!obpzIVquVRQseulcTcTZaHvizTjRA)
				{
					return Guid.Empty;
				}
				return hvtSbKLcAoLwFocNZGaXyDHVIyhp.defaultHardwareJoystickMapGuid;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool isRunningInEditMode => GkmiXdOXsbIXKjeFpIDPABcVkAEp;

		[CustomObfuscation(rename = false)]
		internal static bool isEditorPaused => UnityTools.externalTools.isEditorPaused;

		[CustomObfuscation(rename = false)]
		internal static float unityUnscaledDeltaTime => EgFqVkhnQuwqWzKlxbTTqBIkwAdn.WAIiMBZlcYQBQMoohQvPidrhDnOZ;

		[CustomObfuscation(rename = false)]
		internal static float unityUnscaledDeltaTimePrev => EgFqVkhnQuwqWzKlxbTTqBIkwAdn.kBbCjoDeqMFCajtnaIxyRQUFTynfA;

		[CustomObfuscation(rename = false)]
		internal static double realTime
		{
			get
			{
				if (!obpzIVquVRQseulcTcTZaHvizTjRA)
				{
					return 0.0;
				}
				return EgFqVkhnQuwqWzKlxbTTqBIkwAdn.wEeKniBOCFcwArpfmZhOqVuhQdhP;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static int currentUnityFrame
		{
			get
			{
				if (!obpzIVquVRQseulcTcTZaHvizTjRA)
				{
					return 0;
				}
				return KCnPsFgFLyiYyEYWEjTridcaKheJA.gOuDkZUQNKJvrYshgNhmDOkegsQi;
			}
		}

		private static bool zNyGeVXSIgnSvIDeeAEQjOxSNRYfb
		{
			get
			{
				if (UnityTools.unityVersion >= UnityTools.UnityVersion.UNITY_5_1)
				{
					return qvEajoyMuISFgeEqGQeJiQKMKgmK == "Game";
				}
				return qvEajoyMuISFgeEqGQeJiQKMKgmK == "UnityEditor.GameView";
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool isAllowedEditorWindowFocused
		{
			get
			{
				if (MzljsQKNnzXzAuzZSJhUefpPCcEy.allowInputInEditorSceneView && UnityTools.externalTools.IsEditorSceneViewFocused())
				{
					return true;
				}
				if (!ciaKHlkdfnxSFTEXSBgJHScCNEMbA)
				{
					return zNyGeVXSIgnSvIDeeAEQjOxSNRYfb;
				}
				return true;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool isUnityEditorFocused
		{
			get
			{
				if (CTpVLYlUovEEvUpcNlivRncrrQbW is INativePlatformHelper nativePlatformHelper)
				{
					return nativePlatformHelper.isApplicationFocused;
				}
				return ciaKHlkdfnxSFTEXSBgJHScCNEMbA;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool isWindowsStandaloneWebplayerOrEditorPlatform
		{
			get
			{
				if (!obpzIVquVRQseulcTcTZaHvizTjRA)
				{
					return false;
				}
				if (!ccufkjAWAhTbJaDuyGebaJsLxHKUA)
				{
					return false;
				}
				if (uDnCOOlZZpUIrgXGeSbcMIctXMiG != Platform.Windows && (uDnCOOlZZpUIrgXGeSbcMIctXMiG != Platform.Webplayer || EWHwzexfoiyRFhLiBgPzrmmYiUKt != WebplayerPlatform.Windows))
				{
					return unICjthLTqHPNnLSrXcmbvFaXzQQ == EditorPlatform.Windows;
				}
				return true;
			}
		}

		private static bool epCjQafeDYRojVpaRIeiCxKFjwof
		{
			get
			{
				if (!obpzIVquVRQseulcTcTZaHvizTjRA)
				{
					return false;
				}
				if (!KCnPsFgFLyiYyEYWEjTridcaKheJA.UZYSHsTJwyaEEFondgqwlzyMavdk.value)
				{
					if (DxzQhCtDRoyInHoxWcnNRxVqcFzB)
					{
						return false;
					}
					if (!isEditor && !KCnPsFgFLyiYyEYWEjTridcaKheJA.vbOhiocIcDPNMFSYkWDgYncMpduHA.value)
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
				if (obpzIVquVRQseulcTcTZaHvizTjRA)
				{
					return KCnPsFgFLyiYyEYWEjTridcaKheJA.UZYSHsTJwyaEEFondgqwlzyMavdk.value;
				}
				return false;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool applicationIsFullScreen
		{
			get
			{
				if (obpzIVquVRQseulcTcTZaHvizTjRA)
				{
					return KCnPsFgFLyiYyEYWEjTridcaKheJA.KYiOUlwMqmWUeZyXbFwxCXGMMnDJ.value;
				}
				return false;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool applicationRunInBackground
		{
			get
			{
				if (obpzIVquVRQseulcTcTZaHvizTjRA)
				{
					return KCnPsFgFLyiYyEYWEjTridcaKheJA.vbOhiocIcDPNMFSYkWDgYncMpduHA.value;
				}
				return false;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool timeScaleIsPaused
		{
			get
			{
				if (obpzIVquVRQseulcTcTZaHvizTjRA)
				{
					return KCnPsFgFLyiYyEYWEjTridcaKheJA.mYuWjVGPwctxkGRqeqjMVZiXqUQ.value;
				}
				return false;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static InputManager_Base rewiredInputManager => HvlowarpEJjTwmVIKndWValQpypI;

		[CustomObfuscation(rename = false)]
		internal static PlatformInputManager primaryInputManager
		{
			get
			{
				if (!obpzIVquVRQseulcTcTZaHvizTjRA)
				{
					LJNkuIumYUrwPEaXMrTJGeZvMNxe();
					return null;
				}
				return CTpVLYlUovEEvUpcNlivRncrrQbW.primaryInputManager;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static IControllerAssigner controllerAssigner
		{
			get
			{
				return CDsiOyqanEANRJmTqOQLnofCbVXm;
			}
			set
			{
				CDsiOyqanEANRJmTqOQLnofCbVXm = value;
			}
		}

		[CustomObfuscation(rename = false)]
		internal static RewiredVersion rewiredVersion => new RewiredVersion(programVersion);

		[CustomObfuscation(rename = false)]
		internal static int timeScalePauseChangedCount => CDFHTYcVYyNjfwcbyMnOntgUhkKJ;

		public static event Action<ControllerStatusChangedEventArgs> ControllerConnectedEvent
		{
			add
			{
				uFMSNayMaqnyuBvdoywREwUAsimj += value;
			}
			remove
			{
				uFMSNayMaqnyuBvdoywREwUAsimj -= value;
			}
		}

		public static event Action<ControllerStatusChangedEventArgs> ControllerPreDisconnectEvent
		{
			add
			{
				RSMIOQyasZdEIGhSzpIEFbajfJYPc += value;
			}
			remove
			{
				RSMIOQyasZdEIGhSzpIEFbajfJYPc -= value;
			}
		}

		public static event Action<ControllerStatusChangedEventArgs> ControllerDisconnectedEvent
		{
			add
			{
				JYXlYmJfMTgurfCCixbvqZlIozKP += value;
			}
			remove
			{
				JYXlYmJfMTgurfCCixbvqZlIozKP -= value;
			}
		}

		public static event Action InputSourceUpdateEvent
		{
			add
			{
				BbIMbQeLvHjivRxkUFEuuHyecJUe += value;
			}
			remove
			{
				BbIMbQeLvHjivRxkUFEuuHyecJUe -= value;
			}
		}

		public static event Action EditorRecompileEvent
		{
			add
			{
				DoMcMTFZOcROcVWyENyvXEFgDWYm += value;
			}
			remove
			{
				DoMcMTFZOcROcVWyENyvXEFgDWYm -= value;
			}
		}

		public static event Action PreShutDownEvent
		{
			add
			{
				DxDDJgdGopEpAltRNiEBalwJEIfob += value;
			}
			remove
			{
				DxDDJgdGopEpAltRNiEBalwJEIfob -= value;
			}
		}

		public static event Action ShutDownEvent
		{
			add
			{
				BDmisuCgVCeghMSBpbXRvlOriVYBb += value;
			}
			remove
			{
				BDmisuCgVCeghMSBpbXRvlOriVYBb -= value;
			}
		}

		public static event Action InitializedEvent
		{
			add
			{
				vMWkXfFtLyWssWlmvPyaljgWTHhk += value;
			}
			remove
			{
				vMWkXfFtLyWssWlmvPyaljgWTHhk -= value;
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
		internal static event Action EarlyUpdateEvent
		{
			add
			{
				IzBYZMAEXTGrtOeLAlYNcYpctpQK = (Action)Delegate.Combine(IzBYZMAEXTGrtOeLAlYNcYpctpQK, value);
			}
			remove
			{
				IzBYZMAEXTGrtOeLAlYNcYpctpQK = (Action)Delegate.Remove(IzBYZMAEXTGrtOeLAlYNcYpctpQK, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action<UpdateLoopType> BeforeTimeManagerUpdateEvent
		{
			add
			{
				TrQuHFJWSbBEQkUuadDRDhOfcwRn = (Action<UpdateLoopType>)Delegate.Combine(TrQuHFJWSbBEQkUuadDRDhOfcwRn, value);
			}
			remove
			{
				TrQuHFJWSbBEQkUuadDRDhOfcwRn = (Action<UpdateLoopType>)Delegate.Remove(TrQuHFJWSbBEQkUuadDRDhOfcwRn, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action<UpdateLoopType> UpdateStartedEvent
		{
			add
			{
				DIGTJvqzbyagBhANOgLCGihAWkgZ = (Action<UpdateLoopType>)Delegate.Combine(DIGTJvqzbyagBhANOgLCGihAWkgZ, value);
			}
			remove
			{
				DIGTJvqzbyagBhANOgLCGihAWkgZ = (Action<UpdateLoopType>)Delegate.Remove(DIGTJvqzbyagBhANOgLCGihAWkgZ, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action<UpdateLoopType> UpdateEndedEvent
		{
			add
			{
				RwMSqmrPEIcUWPMqqCGgofpACAIz = (Action<UpdateLoopType>)Delegate.Combine(RwMSqmrPEIcUWPMqqCGgofpACAIz, value);
			}
			remove
			{
				RwMSqmrPEIcUWPMqqCGgofpACAIz = (Action<UpdateLoopType>)Delegate.Remove(RwMSqmrPEIcUWPMqqCGgofpACAIz, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action LateUpdateEvent
		{
			add
			{
				nEoJdFSxGZTTQgqVzwyiDJlZpkoT = (Action)Delegate.Combine(nEoJdFSxGZTTQgqVzwyiDJlZpkoT, value);
			}
			remove
			{
				nEoJdFSxGZTTQgqVzwyiDJlZpkoT = (Action)Delegate.Remove(nEoJdFSxGZTTQgqVzwyiDJlZpkoT, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action<bool> ApplicationIsFullScreenChangedEvent
		{
			add
			{
				yNbnOmzfriGRWQMmXbBnZbjOIcYA = (Action<bool>)Delegate.Combine(yNbnOmzfriGRWQMmXbBnZbjOIcYA, value);
			}
			remove
			{
				yNbnOmzfriGRWQMmXbBnZbjOIcYA = (Action<bool>)Delegate.Remove(yNbnOmzfriGRWQMmXbBnZbjOIcYA, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action<bool> ApplicationRunInBackgroundChangedEvent
		{
			add
			{
				TcVDZtTBkbCPvFcbwUNzUxWXdNzKA = (Action<bool>)Delegate.Combine(TcVDZtTBkbCPvFcbwUNzUxWXdNzKA, value);
			}
			remove
			{
				TcVDZtTBkbCPvFcbwUNzUxWXdNzKA = (Action<bool>)Delegate.Remove(TcVDZtTBkbCPvFcbwUNzUxWXdNzKA, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action<bool> TimeScalePauseChangedEvent
		{
			add
			{
				EPCOWdjrRVZgEJREalGNkcBsOdV = (Action<bool>)Delegate.Combine(EPCOWdjrRVZgEJREalGNkcBsOdV, value);
			}
			remove
			{
				EPCOWdjrRVZgEJREalGNkcBsOdV = (Action<bool>)Delegate.Remove(EPCOWdjrRVZgEJREalGNkcBsOdV, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action<FullScreenMode> ApplicationFullScreenModeChangedEvent
		{
			add
			{
				IQDcUMOkJbbJpvsxoxIaATuqmVFJ = (Action<FullScreenMode>)Delegate.Combine(IQDcUMOkJbbJpvsxoxIaATuqmVFJ, value);
			}
			remove
			{
				IQDcUMOkJbbJpvsxoxIaATuqmVFJ = (Action<FullScreenMode>)Delegate.Remove(IQDcUMOkJbbJpvsxoxIaATuqmVFJ, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action SceneLoadedEvent
		{
			add
			{
				atJFAUEGsWvXzxPSVIFgFPyLZccI = (Action)Delegate.Combine(atJFAUEGsWvXzxPSVIFgFPyLZccI, value);
			}
			remove
			{
				atJFAUEGsWvXzxPSVIFgFPyLZccI = (Action)Delegate.Remove(atJFAUEGsWvXzxPSVIFgFPyLZccI, value);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static event Action<bool> EditorPauseChangedEvent
		{
			add
			{
				BHfOhXExzTwULJhJBFXiuCiXbVPK = (Action<bool>)Delegate.Combine(BHfOhXExzTwULJhJBFXiuCiXbVPK, value);
			}
			remove
			{
				BHfOhXExzTwULJhJBFXiuCiXbVPK = (Action<bool>)Delegate.Remove(BHfOhXExzTwULJhJBFXiuCiXbVPK, value);
			}
		}

		static ReInput()
		{
			ciaKHlkdfnxSFTEXSBgJHScCNEMbA = true;
			UELUFrjaUxsAhaTllRzYIkUctOmj = -1;
			_id = -1;
			eiTDtHWtAjacrCGIktMvceGodkmNA = 0;
			akZVbtgXAOvjVgWMOQHFJjdJMvoD = UnityTouch.izfChqvjKyhCcypSioMPzwuCmhEV;
			GdwWjDCOcxwJUizjrHagUJgJeCNJ = PlayerHelper.izfChqvjKyhCcypSioMPzwuCmhEV;
			QpbBNmWrfInkcoGyYFaZIQVXTXrNA = ControllerHelper.izfChqvjKyhCcypSioMPzwuCmhEV;
			qIzETGiaCujiFKelzjMBwftDgerG = MappingHelper.izfChqvjKyhCcypSioMPzwuCmhEV;
			ecdLtUBiFgzGXZaYvntixqlbMFjm = TimeHelper.izfChqvjKyhCcypSioMPzwuCmhEV;
			zFposRXLCpdyIpBgRutQBeayGyFI = ConfigHelper.izfChqvjKyhCcypSioMPzwuCmhEV;
			uFMSNayMaqnyuBvdoywREwUAsimj = new SafeAction<ControllerStatusChangedEventArgs>(cgEOizKXucFXVuDYJObcnLIdaeTG._003C_003E9.ZfmGIXNjbXILurFfUjnNBqwPWzqR);
			RSMIOQyasZdEIGhSzpIEFbajfJYPc = new SafeAction<ControllerStatusChangedEventArgs>(cgEOizKXucFXVuDYJObcnLIdaeTG._003C_003E9.FCEhqTiwEbxeTVPvQspMepWjMzyAb);
			JYXlYmJfMTgurfCCixbvqZlIozKP = new SafeAction<ControllerStatusChangedEventArgs>(cgEOizKXucFXVuDYJObcnLIdaeTG._003C_003E9.MfetaeDhesggpwtBdkNDnrxNUPKs);
			BbIMbQeLvHjivRxkUFEuuHyecJUe = new SafeAction(cgEOizKXucFXVuDYJObcnLIdaeTG._003C_003E9.ygyKQijsPTyjSmaEXkDpwzcfstbN);
			DoMcMTFZOcROcVWyENyvXEFgDWYm = new SafeAction(cgEOizKXucFXVuDYJObcnLIdaeTG._003C_003E9.sKWexrMIZTGMdilvVozOUbVLJxcT);
			DxDDJgdGopEpAltRNiEBalwJEIfob = new SafeAction(cgEOizKXucFXVuDYJObcnLIdaeTG._003C_003E9.GFmRdWcpdvgqTwWvxiPXChNqNoBk);
			BDmisuCgVCeghMSBpbXRvlOriVYBb = new SafeAction(cgEOizKXucFXVuDYJObcnLIdaeTG._003C_003E9.bqWVBlqBuflPyHklEcVCgPsKGyEd);
			vMWkXfFtLyWssWlmvPyaljgWTHhk = new SafeAction(cgEOizKXucFXVuDYJObcnLIdaeTG._003C_003E9.AArQVFcZfagUZcVGJKtdEEjyGOtjA);
			SafeDelegate.S_ExceptionHandler = cgEOizKXucFXVuDYJObcnLIdaeTG._003C_003E9.TDDiqmPleXAatJzZudtmfHVfcaTEA;
		}

		public static void Reset()
		{
			if (obpzIVquVRQseulcTcTZaHvizTjRA && !(HvlowarpEJjTwmVIKndWValQpypI == null))
			{
				HvlowarpEJjTwmVIKndWValQpypI.ResetAll();
			}
		}

		[CustomObfuscation(rename = false)]
		internal static bool IsInputAllowed(ControllerType controllerType)
		{
			if (!epCjQafeDYRojVpaRIeiCxKFjwof)
			{
				return false;
			}
			if (unICjthLTqHPNnLSrXcmbvFaXzQQ != EditorPlatform.None && (controllerType == ControllerType.Keyboard || controllerType == ControllerType.Mouse))
			{
				if (DxzQhCtDRoyInHoxWcnNRxVqcFzB)
				{
					if (!KCnPsFgFLyiYyEYWEjTridcaKheJA.UZYSHsTJwyaEEFondgqwlzyMavdk.value)
					{
						return false;
					}
				}
				else
				{
					if (!isAllowedEditorWindowFocused)
					{
						return false;
					}
					if (controllerType == ControllerType.Mouse && !isUnityEditorFocused)
					{
						return false;
					}
				}
			}
			return true;
		}

		internal static void zQQfvDZMmpVqPPLYlLuSJXXpwJcI(InputManager_Base P_0, Func<ConfigVars, object> P_1, ConfigVars P_2, ControllerDataFiles P_3, UserData P_4)
		{
			try
			{
				_id = eiTDtHWtAjacrCGIktMvceGodkmNA;
				eiTDtHWtAjacrCGIktMvceGodkmNA++;
				obpzIVquVRQseulcTcTZaHvizTjRA = true;
				hCmEAcPmvppxEjhawxLgIZABpLik = true;
				GkmiXdOXsbIXKjeFpIDPABcVkAEp = UnityTools.isEditor && !Application.isPlaying;
				if (UnityTools.isEditor)
				{
					CheckRewiredVersionCompatibility();
				}
				HvlowarpEJjTwmVIKndWValQpypI = P_0;
				MzljsQKNnzXzAuzZSJhUefpPCcEy = P_2;
				uDnCOOlZZpUIrgXGeSbcMIctXMiG = UnityTools.platform;
				EWHwzexfoiyRFhLiBgPzrmmYiUKt = UnityTools.webplayerPlatform;
				unICjthLTqHPNnLSrXcmbvFaXzQQ = UnityTools.editorPlatform;
				if (P_2.logToScreen)
				{
					Logger.logToScreen = true;
				}
				UnityTools.externalTools.EditorPausedStateChangedEvent += eluifZHoypzzwuCRdKpaEltadRFGA;
				hvtSbKLcAoLwFocNZGaXyDHVIyhp = P_3;
				afarNIRJlvcfXIrnUwLxEgdcbDEjA = P_4;
				P_4.zQQfvDZMmpVqPPLYlLuSJXXpwJcI();
				ThreadSafeUnityInput.Initialize();
				KCnPsFgFLyiYyEYWEjTridcaKheJA = new MqUEZjlwiIeKTuXxUzFWBUliKyLL();
				KCnPsFgFLyiYyEYWEjTridcaKheJA.UZYSHsTJwyaEEFondgqwlzyMavdk.Set(ciaKHlkdfnxSFTEXSBgJHScCNEMbA);
				KCnPsFgFLyiYyEYWEjTridcaKheJA.UZYSHsTJwyaEEFondgqwlzyMavdk.Use();
				if (unICjthLTqHPNnLSrXcmbvFaXzQQ != EditorPlatform.None)
				{
					KCnPsFgFLyiYyEYWEjTridcaKheJA.UZYSHsTJwyaEEFondgqwlzyMavdk.getValueDelegate = cgEOizKXucFXVuDYJObcnLIdaeTG._003C_003E9.bKlGBRKPxYxDUcMjyzVSkpkneezIb;
					if (GkmiXdOXsbIXKjeFpIDPABcVkAEp)
					{
						ciaKHlkdfnxSFTEXSBgJHScCNEMbA = zNyGeVXSIgnSvIDeeAEQjOxSNRYfb;
					}
					KCnPsFgFLyiYyEYWEjTridcaKheJA.UZYSHsTJwyaEEFondgqwlzyMavdk.Set(isUnityEditorFocused && isAllowedEditorWindowFocused);
				}
				JaxBYPFZsFwQSYeYUhbJgQnBWTaY();
				HmHsEaZxDIDUXLKxzIztJkUaaoZm = new TimerAbs(1.0);
				EgFqVkhnQuwqWzKlxbTTqBIkwAdn = new hZoSIeiGQlsNpxKnhkskatxNwdZA();
				UoqRqIFlbmbMteHEhPCXEDTNMiuB(P_1);
				GAwkqrvWlvpikRXvCLOCwZfKeMsC = new nWObnmDQDyPAcKuVnyCIqGPjixDdb(P_4.GetActions_Copy());
				VmqcbbbvPImXBEBcMHWjUAXVfrUSA = new pmhdIRhCoLlQZffSYgOmsLkpxsjN(P_2, CTpVLYlUovEEvUpcNlivRncrrQbW);
				hqGXmYxZUrSrCxDobJTGEPCWvHAsA = new BBctKivJxEjGKRzyEkHSRjJoJqnW(P_2);
				CTpVLYlUovEEvUpcNlivRncrrQbW.DeviceConnectedEvent += gvyVbcfgxJoHCgHCMpkGqQFLWVmJ;
				CTpVLYlUovEEvUpcNlivRncrrQbW.DeviceDisconnectedEvent += XbzGxHmPnLSRwWFaMhRHIylwgclg;
				CTpVLYlUovEEvUpcNlivRncrrQbW.UpdateControllerInfoEvent += JpkuUtwWxYVeZSTbpXuvptwLbCTc;
				VmqcbbbvPImXBEBcMHWjUAXVfrUSA.iQyOLDUnjPHgzcBvmppBkkalWfrl += iMqpfKAKSOAZpIOaLIqnRLvZlxzP;
				VmqcbbbvPImXBEBcMHWjUAXVfrUSA.qGgZpjGtlrLnQGOPypQsiBrlekEz += hqGXmYxZUrSrCxDobJTGEPCWvHAsA.elDPsiONERnRCGVdgPltkMSHilkF;
				ThreadSafeUnityInput.PostInitialize();
				vySTJyCQQbGmMkAKrmQlPwABCQhE();
				ThreadSafeUnityInput.PostInitialize2();
				LPxDLELdQfiHxinMUfSElrZHFOEd = UnityTools.GetComponent<UserDataStore>(HvlowarpEJjTwmVIKndWValQpypI);
				if (LPxDLELdQfiHxinMUfSElrZHFOEd != null)
				{
					LPxDLELdQfiHxinMUfSElrZHFOEd.Initialize();
				}
				gNOuFVusZxBVSuCaAILcKBpFgAtbA();
				hCmEAcPmvppxEjhawxLgIZABpLik = false;
				if (GkmiXdOXsbIXKjeFpIDPABcVkAEp)
				{
					Logger.Log("Rewired is running in Edit mode.");
				}
				if (vMWkXfFtLyWssWlmvPyaljgWTHhk != null)
				{
					vMWkXfFtLyWssWlmvPyaljgWTHhk.Invoke();
				}
			}
			catch (Exception)
			{
				obpzIVquVRQseulcTcTZaHvizTjRA = false;
				hCmEAcPmvppxEjhawxLgIZABpLik = false;
				throw;
			}
		}

		internal static void edGQWybkvEgdpdpFVqgmUdPGZTqV()
		{
			if (EgFqVkhnQuwqWzKlxbTTqBIkwAdn != null)
			{
				EgFqVkhnQuwqWzKlxbTTqBIkwAdn.rZuOrpWYhhCfBPmUsQdcUinbQbVD();
			}
			if (configVars.deferControllerConnectedEventsOnStart)
			{
				for (int i = 0; i < VmqcbbbvPImXBEBcMHWjUAXVfrUSA.OeeHBecrguVtJOJkdEjZKpQgYwls; i++)
				{
					Joystick joystick = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.FMULSNPMOjtlSugUpKDpvqOlAjyl[i];
					PUyKKfOBaawGLxPIxGJwGRhAoMls(new ControllerStatusChangedEventArgs(joystick.name, joystick.id, joystick.type));
				}
			}
		}

		internal static void FddrIOuEFxAEchqWvFaTErixupUu(UpdateLoopType P_0)
		{
			if (obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				noKmfvdPLaGovcnpLUDWmQJqtyuAA(P_0);
				if ((uint)P_0 <= 1u)
				{
					VxQffUITEqMNYFntBlCQCUoWpOodB();
				}
			}
		}

		private static void noKmfvdPLaGovcnpLUDWmQJqtyuAA(UpdateLoopType P_0)
		{
			if (KCnPsFgFLyiYyEYWEjTridcaKheJA != null)
			{
				KCnPsFgFLyiYyEYWEjTridcaKheJA.jRaYtHNVcykNMAbqOnSGaKIIGSEaA();
			}
			Action<UpdateLoopType> trQuHFJWSbBEQkUuadDRDhOfcwRn = TrQuHFJWSbBEQkUuadDRDhOfcwRn;
			if (trQuHFJWSbBEQkUuadDRDhOfcwRn != null)
			{
				try
				{
					trQuHFJWSbBEQkUuadDRDhOfcwRn(P_0);
				}
				catch (Exception exception)
				{
					HandleCallbackException("ReInput.BeforeTimeManagerUpdateEvent", exception);
				}
			}
			EgFqVkhnQuwqWzKlxbTTqBIkwAdn.jRaYtHNVcykNMAbqOnSGaKIIGSEaA(P_0);
		}

		private static void VxQffUITEqMNYFntBlCQCUoWpOodB()
		{
			int frameCount = Time.frameCount;
			if (UELUFrjaUxsAhaTllRzYIkUctOmj == frameCount)
			{
				return;
			}
			UELUFrjaUxsAhaTllRzYIkUctOmj = frameCount;
			ThreadSafeUnityInput.Update();
			Action izBYZMAEXTGrtOeLAlYNcYpctpQK = IzBYZMAEXTGrtOeLAlYNcYpctpQK;
			if (izBYZMAEXTGrtOeLAlYNcYpctpQK == null)
			{
				return;
			}
			try
			{
				izBYZMAEXTGrtOeLAlYNcYpctpQK();
			}
			catch (Exception exception)
			{
				HandleCallbackException("ReInput.EarlyUpdateEvent", exception);
			}
		}

		internal static void jRaYtHNVcykNMAbqOnSGaKIIGSEaA(UpdateLoopType P_0)
		{
			if (!obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				return;
			}
			if (JZKJGBsuSaOLhJiTvsRyNFkLxwMp != P_0)
			{
				JZKJGBsuSaOLhJiTvsRyNFkLxwMp = P_0;
			}
			if (editorPlatform != EditorPlatform.None)
			{
				qvEajoyMuISFgeEqGQeJiQKMKgmK = KCnPsFgFLyiYyEYWEjTridcaKheJA.CRuRampAWykoWdPZeqkFGVaRdTjs.value;
			}
			if (uncSVBAxHAmqUjuITaNtgwfiLSRd)
			{
				if (HmHsEaZxDIDUXLKxzIztJkUaaoZm.Update())
				{
					uncSVBAxHAmqUjuITaNtgwfiLSRd = false;
					HmHsEaZxDIDUXLKxzIztJkUaaoZm.Clear();
				}
				else
				{
					QgbIinFdaCcKNDxZdcNgHuxnvnzHb.jRaYtHNVcykNMAbqOnSGaKIIGSEaA(P_0);
				}
			}
			KCnPsFgFLyiYyEYWEjTridcaKheJA.uNorXdRhAZJVgdORHzhewkLNilUr();
			Action<UpdateLoopType> dIGTJvqzbyagBhANOgLCGihAWkgZ = DIGTJvqzbyagBhANOgLCGihAWkgZ;
			if (dIGTJvqzbyagBhANOgLCGihAWkgZ != null)
			{
				try
				{
					dIGTJvqzbyagBhANOgLCGihAWkgZ(P_0);
				}
				catch (Exception exception)
				{
					HandleCallbackException("ReInput.UpdateStartedEvent", exception);
				}
			}
			CTpVLYlUovEEvUpcNlivRncrrQbW.Update(P_0);
			if (BbIMbQeLvHjivRxkUFEuuHyecJUe != null)
			{
				BbIMbQeLvHjivRxkUFEuuHyecJUe.Invoke();
			}
			VmqcbbbvPImXBEBcMHWjUAXVfrUSA.jRaYtHNVcykNMAbqOnSGaKIIGSEaA(P_0);
			Action<UpdateLoopType> rwMSqmrPEIcUWPMqqCGgofpACAIz = RwMSqmrPEIcUWPMqqCGgofpACAIz;
			if (rwMSqmrPEIcUWPMqqCGgofpACAIz == null)
			{
				return;
			}
			try
			{
				rwMSqmrPEIcUWPMqqCGgofpACAIz(P_0);
			}
			catch (Exception exception2)
			{
				HandleCallbackException("ReInput.UpdateEndedEvent", exception2);
			}
		}

		internal static void xKyppkUUtEcaFoPeiMUWhYAtRasV()
		{
			Action action = nEoJdFSxGZTTQgqVzwyiDJlZpkoT;
			if (action != null)
			{
				try
				{
					action();
				}
				catch (Exception exception)
				{
					HandleCallbackException("ReInput.LateUpdateEvent", exception);
				}
			}
		}

		[CustomObfuscation(rename = false)]
		internal static void EditorUpdate()
		{
			if (obpzIVquVRQseulcTcTZaHvizTjRA && GkmiXdOXsbIXKjeFpIDPABcVkAEp)
			{
				FddrIOuEFxAEchqWvFaTErixupUu(UpdateLoopType.Update);
				jRaYtHNVcykNMAbqOnSGaKIIGSEaA(UpdateLoopType.Update);
				xKyppkUUtEcaFoPeiMUWhYAtRasV();
			}
		}

		internal static void LWWrvKsndLGerwSeVdnkTVqAzMnP()
		{
			if (DxDDJgdGopEpAltRNiEBalwJEIfob != null)
			{
				DxDDJgdGopEpAltRNiEBalwJEIfob.Invoke();
			}
			if (CTpVLYlUovEEvUpcNlivRncrrQbW != null)
			{
				CTpVLYlUovEEvUpcNlivRncrrQbW.OnDestroy();
			}
			YhaddXEWASnLYtmXhkQurcwjDWkPA();
			if (BDmisuCgVCeghMSBpbXRvlOriVYBb != null)
			{
				BDmisuCgVCeghMSBpbXRvlOriVYBb.Invoke();
				BDmisuCgVCeghMSBpbXRvlOriVYBb = null;
			}
		}

		internal static void bzOGfyCVRfBbKzomQOgXceRcFkIjb()
		{
			if (DoMcMTFZOcROcVWyENyvXEFgDWYm != null)
			{
				DoMcMTFZOcROcVWyENyvXEFgDWYm.Invoke();
			}
		}

		internal static void bkbtHSeXqnEvLDwYOkKMebMJpxpf(bool P_0)
		{
			ciaKHlkdfnxSFTEXSBgJHScCNEMbA = P_0;
			if (unICjthLTqHPNnLSrXcmbvFaXzQQ == EditorPlatform.None && obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				KCnPsFgFLyiYyEYWEjTridcaKheJA.UZYSHsTJwyaEEFondgqwlzyMavdk.Set(P_0);
				KCnPsFgFLyiYyEYWEjTridcaKheJA.UZYSHsTJwyaEEFondgqwlzyMavdk.TriggerEvent();
			}
		}

		internal static void froUXlIdrkajRjAcMiLmmOCCQMmm()
		{
			if (!obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				return;
			}
			Action action = atJFAUEGsWvXzxPSVIFgFPyLZccI;
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
				HandleCallbackException("ReInput.SceneLoadedEvent", exception);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static HardwareJoystickMap_InputManager GetHardwareJoystickMap_InputManager(BridgedControllerHWInfo bridgedController)
		{
			return hvtSbKLcAoLwFocNZGaXyDHVIyhp.QybEKscbrtSTeOWvpcJxVwbuuRGnA(bridgedController);
		}

		internal static HardwareJoystickMap fdqXOouyCFlzjkPTPiIErxwlIHye(Guid P_0)
		{
			return hvtSbKLcAoLwFocNZGaXyDHVIyhp.GetHardwareJoystickMap(P_0);
		}

		internal static HardwareJoystickTemplateMap rfsEypEpDSCojIvJWQqUMoXAhWHO(Guid P_0)
		{
			return hvtSbKLcAoLwFocNZGaXyDHVIyhp.GetJoystickTemplate(P_0);
		}

		internal static IHardwareControllerTemplateMap YbndFNkGSRPhLsXywDJcJwBCUGCxA(Guid P_0)
		{
			return hvtSbKLcAoLwFocNZGaXyDHVIyhp.GetControllerTemplate(P_0);
		}

		internal static IList<HardwareJoystickTemplateMap> NyMhJvALUKLeLLDFuwVjXLerIxgdA(Guid P_0)
		{
			HardwareJoystickMap hardwareJoystickMap = hvtSbKLcAoLwFocNZGaXyDHVIyhp.GetHardwareJoystickMap(P_0);
			if (hardwareJoystickMap == null)
			{
				return EmptyObjects<HardwareJoystickTemplateMap>.EmptyReadOnlyIListT;
			}
			string[] templateGuidsOrig = hardwareJoystickMap.GetTemplateGuidsOrig();
			if (templateGuidsOrig == null || templateGuidsOrig.Length == 0)
			{
				return EmptyObjects<HardwareJoystickTemplateMap>.EmptyReadOnlyIListT;
			}
			List<HardwareJoystickTemplateMap> list = null;
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
				HardwareJoystickTemplateMap hardwareJoystickTemplateMap = rfsEypEpDSCojIvJWQqUMoXAhWHO(guid);
				if (hardwareJoystickTemplateMap == null)
				{
					Logger.LogWarning("Controller Template was not found for GUID " + guid.ToString());
					continue;
				}
				if (list == null)
				{
					list = new List<HardwareJoystickTemplateMap>();
				}
				ListTools.AddIfUnique(list, hardwareJoystickTemplateMap);
			}
			if (list == null)
			{
				return EmptyObjects<HardwareJoystickTemplateMap>.EmptyReadOnlyIListT;
			}
			return list;
		}

		[CustomObfuscation(rename = false)]
		internal static int GetNewJoystickId()
		{
			return VmqcbbbvPImXBEBcMHWjUAXVfrUSA.vLiLmJTosNNvyFgxkOlaPhnFocg();
		}

		[CustomObfuscation(rename = false)]
		internal static void HandleCallbackException(string source, Exception exception)
		{
			Logger.LogError("An exception occurred inside an event handler or callback.\nSource: " + source + "\n\nThis happens if your event handler/callback code throws an exception. This means the error is in your code, not Rewired. Read the exception message and the stack trace carefully to find the source of the exception being thrown by your code.\n\nThis can also happen if you forget to unsubscribe to an event in a MonoBehaviour class and that object gets destroyed. Make sure you unsubscribe to events in OnDisable or OnDestroy. Rewired will attempt to continue running.\n\nException:\n" + ((exception.InnerException != null) ? exception.InnerException : exception), requiredThreadSafety: true);
		}

		[CustomObfuscation(rename = false)]
		internal static void HandleExternException(string source, Exception exception)
		{
		}

		[CustomObfuscation(rename = false)]
		internal static void HandleExternalInterfaceException(string source, Exception exception)
		{
			Logger.LogError("An exception occurred inside an external function call.\nSource: " + source + "\n\nThis happens if the external function throws an exception. This could indicate the error is in your code if Rewired is calling a function in an interface implementation you created. Read the exception message and the stack trace carefully to find the source of the exception being thrown.\n\nThis can also happen if you forget to unsubscribe to an event in a MonoBehaviour class and that object gets destroyed.\n\nException:\n" + ((exception.InnerException != null) ? exception.InnerException : exception), requiredThreadSafety: true);
		}

		internal static void iaFqwxjsRihFAZTTzjugnSXKeOgL()
		{
			if (obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				gNOuFVusZxBVSuCaAILcKBpFgAtbA();
			}
		}

		[CustomObfuscation(rename = false)]
		internal static void CheckRewiredVersionCompatibility()
		{
			if (UnityTools.unityVersionObj != null && 2020 != UnityTools.unityVersionObj.major)
			{
				nRaBRAYQPLInWdZbPkcAoOMGefydA();
			}
		}

		internal static float TpRDVeKUrDzPBCwrBJsvhbGodtGT()
		{
			return KCnPsFgFLyiYyEYWEjTridcaKheJA.qDnfJdfXrGdKIjhGHpBGIytaTokcE.value;
		}

		[CustomObfuscation(rename = false)]
		internal static bool CheckInitialized()
		{
			if (!obpzIVquVRQseulcTcTZaHvizTjRA)
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

		private static void vySTJyCQQbGmMkAKrmQlPwABCQhE()
		{
			hqGXmYxZUrSrCxDobJTGEPCWvHAsA.zQQfvDZMmpVqPPLYlLuSJXXpwJcI();
			VmqcbbbvPImXBEBcMHWjUAXVfrUSA.zQQfvDZMmpVqPPLYlLuSJXXpwJcI(CTpVLYlUovEEvUpcNlivRncrrQbW.GetInputDataUpdateDelegate(), afarNIRJlvcfXIrnUwLxEgdcbDEjA.GetInputBehaviors_Copy());
			CTpVLYlUovEEvUpcNlivRncrrQbW.Initialize();
		}

		private static void YhaddXEWASnLYtmXhkQurcwjDWkPA()
		{
			if (HvlowarpEJjTwmVIKndWValQpypI != null)
			{
				List<IExternalInputManager> componentsInSelfAndChildren = UnityTools.GetComponentsInSelfAndChildren<IExternalInputManager>(HvlowarpEJjTwmVIKndWValQpypI);
				for (int i = 0; i < componentsInSelfAndChildren.Count; i++)
				{
					componentsInSelfAndChildren[i].Deinitialize();
				}
			}
			HvlowarpEJjTwmVIKndWValQpypI = null;
			CTpVLYlUovEEvUpcNlivRncrrQbW = null;
			GAwkqrvWlvpikRXvCLOCwZfKeMsC = null;
			if (VmqcbbbvPImXBEBcMHWjUAXVfrUSA != null)
			{
				VmqcbbbvPImXBEBcMHWjUAXVfrUSA.Dispose();
			}
			VmqcbbbvPImXBEBcMHWjUAXVfrUSA = null;
			hqGXmYxZUrSrCxDobJTGEPCWvHAsA = null;
			hvtSbKLcAoLwFocNZGaXyDHVIyhp = null;
			afarNIRJlvcfXIrnUwLxEgdcbDEjA = null;
			CDsiOyqanEANRJmTqOQLnofCbVXm = null;
			obpzIVquVRQseulcTcTZaHvizTjRA = false;
			MzljsQKNnzXzAuzZSJhUefpPCcEy = null;
			JZKJGBsuSaOLhJiTvsRyNFkLxwMp = UpdateLoopType.Update;
			ccufkjAWAhTbJaDuyGebaJsLxHKUA = false;
			uDnCOOlZZpUIrgXGeSbcMIctXMiG = Platform.Windows;
			EWHwzexfoiyRFhLiBgPzrmmYiUKt = WebplayerPlatform.None;
			unICjthLTqHPNnLSrXcmbvFaXzQQ = EditorPlatform.None;
			uncSVBAxHAmqUjuITaNtgwfiLSRd = false;
			HmHsEaZxDIDUXLKxzIztJkUaaoZm = null;
			EgFqVkhnQuwqWzKlxbTTqBIkwAdn = null;
			qvEajoyMuISFgeEqGQeJiQKMKgmK = null;
			DxzQhCtDRoyInHoxWcnNRxVqcFzB = false;
			GkmiXdOXsbIXKjeFpIDPABcVkAEp = false;
			ciaKHlkdfnxSFTEXSBgJHScCNEMbA = true;
			UELUFrjaUxsAhaTllRzYIkUctOmj = -1;
			_id = -1;
			CDFHTYcVYyNjfwcbyMnOntgUhkKJ = 0;
			uFMSNayMaqnyuBvdoywREwUAsimj.Clear();
			RSMIOQyasZdEIGhSzpIEFbajfJYPc.Clear();
			JYXlYmJfMTgurfCCixbvqZlIozKP.Clear();
			BbIMbQeLvHjivRxkUFEuuHyecJUe.Clear();
			DoMcMTFZOcROcVWyENyvXEFgDWYm.Clear();
			_ApplicationFocusChangedEvent = null;
			yNbnOmzfriGRWQMmXbBnZbjOIcYA = null;
			TcVDZtTBkbCPvFcbwUNzUxWXdNzKA = null;
			IQDcUMOkJbbJpvsxoxIaATuqmVFJ = null;
			EPCOWdjrRVZgEJREalGNkcBsOdV = null;
			IzBYZMAEXTGrtOeLAlYNcYpctpQK = null;
			DIGTJvqzbyagBhANOgLCGihAWkgZ = null;
			RwMSqmrPEIcUWPMqqCGgofpACAIz = null;
			nEoJdFSxGZTTQgqVzwyiDJlZpkoT = null;
			DxDDJgdGopEpAltRNiEBalwJEIfob = null;
			atJFAUEGsWvXzxPSVIFgFPyLZccI = null;
			BHfOhXExzTwULJhJBFXiuCiXbVPK = null;
			ExTSxDbLJXjqNMzpYuBREBUEugJb();
			KCnPsFgFLyiYyEYWEjTridcaKheJA = null;
			ThreadSafeUnityInput.Deinitialize();
			if (UnityTools.externalTools != null)
			{
				UnityTools.externalTools.EditorPausedStateChangedEvent -= eluifZHoypzzwuCRdKpaEltadRFGA;
			}
		}

		private static void SjpASscjvlkmyVQrQykyLRaJComhb(string P_0 = null)
		{
			string text = ((P_0 == null) ? "This function" : P_0);
			Logger.LogError(text + " can only be called in Play mode!");
		}

		private static void vvnGZHfUStdhjUsYlsLiYRHwhKXg()
		{
			if (!uncSVBAxHAmqUjuITaNtgwfiLSRd)
			{
				uncSVBAxHAmqUjuITaNtgwfiLSRd = true;
				QgbIinFdaCcKNDxZdcNgHuxnvnzHb.SPGTRPyvIslcMdbPTItsewSLRPxx();
				QgbIinFdaCcKNDxZdcNgHuxnvnzHb.iSvbUvOqNFSvgKLMMuWonBnKnWnE();
			}
			HmHsEaZxDIDUXLKxzIztJkUaaoZm.Start();
		}

		private static void LJNkuIumYUrwPEaXMrTJGeZvMNxe()
		{
			Logger.LogError("Rewired is not initialized. Do you have a Rewired Input Manager in the scene and enabled?");
		}

		private static void gvyVbcfgxJoHCgHCMpkGqQFLWVmJ(BridgedController P_0)
		{
			if (P_0.sourceJoystick == null)
			{
				return;
			}
			VmqcbbbvPImXBEBcMHWjUAXVfrUSA.bHsYHPUicNEFXDZZhoWGRxKnRVDu(P_0);
			Joystick joystick = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.ojzheXTjdCUaEzozjVQUFinqCht(P_0.sourceJoystick.rewiredId);
			if (joystick != null)
			{
				hqGXmYxZUrSrCxDobJTGEPCWvHAsA.nhAQUIndynxeHenHwIxuJxrjAQNp(joystick);
				if (!configVars.deferControllerConnectedEventsOnStart || !hCmEAcPmvppxEjhawxLgIZABpLik)
				{
					PUyKKfOBaawGLxPIxGJwGRhAoMls(new ControllerStatusChangedEventArgs(joystick.name, joystick.id, joystick.type));
				}
			}
		}

		private static void XbzGxHmPnLSRwWFaMhRHIylwgclg(ControllerDisconnectedEventArgs P_0)
		{
			if (P_0 != null)
			{
				Joystick joystick = VmqcbbbvPImXBEBcMHWjUAXVfrUSA.ojzheXTjdCUaEzozjVQUFinqCht(P_0.rewiredId);
				if (joystick != null)
				{
					VmqcbbbvPImXBEBcMHWjUAXVfrUSA.PqJEIUwQDbRjQsunXAtcvOSSVLxX(P_0.rewiredId);
					tJDcUjiPVLjHhRpCEnddSqykgjZY(new ControllerStatusChangedEventArgs(joystick.name, joystick.id, joystick.type));
				}
			}
		}

		private static void PUyKKfOBaawGLxPIxGJwGRhAoMls(ControllerStatusChangedEventArgs P_0)
		{
			if (uFMSNayMaqnyuBvdoywREwUAsimj != null)
			{
				uFMSNayMaqnyuBvdoywREwUAsimj.Invoke(P_0);
			}
		}

		private static void iMqpfKAKSOAZpIOaLIqnRLvZlxzP(ControllerStatusChangedEventArgs P_0)
		{
			if (RSMIOQyasZdEIGhSzpIEFbajfJYPc != null)
			{
				RSMIOQyasZdEIGhSzpIEFbajfJYPc.Invoke(P_0);
			}
		}

		private static void tJDcUjiPVLjHhRpCEnddSqykgjZY(ControllerStatusChangedEventArgs P_0)
		{
			if (JYXlYmJfMTgurfCCixbvqZlIozKP != null)
			{
				JYXlYmJfMTgurfCCixbvqZlIozKP.Invoke(P_0);
			}
		}

		private static void JpkuUtwWxYVeZSTbpXuvptwLbCTc(UpdateControllerInfoEventArgs P_0)
		{
			VmqcbbbvPImXBEBcMHWjUAXVfrUSA.bGjYPyDQFpioBExhyERNSZtsiMQQA(P_0);
		}

		private static void xKXEQmiiNXKorUsTsPlIOtwcwnfr(bool P_0)
		{
			if (!obpzIVquVRQseulcTcTZaHvizTjRA)
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

		private static void knnWPfeazRExMNUGyhISzowVnouk(bool P_0)
		{
			Action<bool> action = yNbnOmzfriGRWQMmXbBnZbjOIcYA;
			if (action != null)
			{
				try
				{
					action(P_0);
				}
				catch (Exception exception)
				{
					HandleCallbackException("ReInput.ApplicationIsFullScreenChangedEvent", exception);
				}
			}
		}

		private static void qCViIgANYdkSaCjgCKjdsocAngtDA(int P_0)
		{
			if (IQDcUMOkJbbJpvsxoxIaATuqmVFJ != null)
			{
				try
				{
					IQDcUMOkJbbJpvsxoxIaATuqmVFJ((FullScreenMode)P_0);
				}
				catch (Exception exception)
				{
					HandleCallbackException("ReInput.ApplicationFullScreenModeChangedEvent", exception);
				}
			}
		}

		private static void yYkWdJJVWdAmxbPIBmMJTDVEgMPm(bool P_0)
		{
			Action<bool> tcVDZtTBkbCPvFcbwUNzUxWXdNzKA = TcVDZtTBkbCPvFcbwUNzUxWXdNzKA;
			if (tcVDZtTBkbCPvFcbwUNzUxWXdNzKA != null)
			{
				try
				{
					tcVDZtTBkbCPvFcbwUNzUxWXdNzKA(P_0);
				}
				catch (Exception exception)
				{
					HandleCallbackException("ReInput.ApplicationRunInBackgroundChangedEvent", exception);
				}
			}
		}

		private static void STlfpSSClhBfHJInYFgTePujYnqwB(bool P_0)
		{
			CDFHTYcVYyNjfwcbyMnOntgUhkKJ++;
			Action<bool> ePCOWdjrRVZgEJREalGNkcBsOdV = EPCOWdjrRVZgEJREalGNkcBsOdV;
			if (ePCOWdjrRVZgEJREalGNkcBsOdV != null)
			{
				try
				{
					ePCOWdjrRVZgEJREalGNkcBsOdV(P_0);
				}
				catch (Exception exception)
				{
					HandleCallbackException("ReInput.TimeScalePauseChangedEvent", exception);
				}
			}
		}

		private static void JaxBYPFZsFwQSYeYUhbJgQnBWTaY()
		{
			if (KCnPsFgFLyiYyEYWEjTridcaKheJA != null)
			{
				ExTSxDbLJXjqNMzpYuBREBUEugJb();
				KCnPsFgFLyiYyEYWEjTridcaKheJA.UZYSHsTJwyaEEFondgqwlzyMavdk.ChangedEvent += xKXEQmiiNXKorUsTsPlIOtwcwnfr;
				KCnPsFgFLyiYyEYWEjTridcaKheJA.KYiOUlwMqmWUeZyXbFwxCXGMMnDJ.ChangedEvent += knnWPfeazRExMNUGyhISzowVnouk;
				KCnPsFgFLyiYyEYWEjTridcaKheJA.vbOhiocIcDPNMFSYkWDgYncMpduHA.ChangedEvent += yYkWdJJVWdAmxbPIBmMJTDVEgMPm;
				KCnPsFgFLyiYyEYWEjTridcaKheJA.bYcrVqtLPhbPOoKrkaPlbHEWjLygb.ChangedEvent += qCViIgANYdkSaCjgCKjdsocAngtDA;
				KCnPsFgFLyiYyEYWEjTridcaKheJA.mYuWjVGPwctxkGRqeqjMVZiXqUQ.ChangedEvent += STlfpSSClhBfHJInYFgTePujYnqwB;
			}
		}

		private static void ExTSxDbLJXjqNMzpYuBREBUEugJb()
		{
			if (KCnPsFgFLyiYyEYWEjTridcaKheJA != null)
			{
				KCnPsFgFLyiYyEYWEjTridcaKheJA.UZYSHsTJwyaEEFondgqwlzyMavdk.ChangedEvent -= xKXEQmiiNXKorUsTsPlIOtwcwnfr;
				KCnPsFgFLyiYyEYWEjTridcaKheJA.KYiOUlwMqmWUeZyXbFwxCXGMMnDJ.ChangedEvent -= knnWPfeazRExMNUGyhISzowVnouk;
				KCnPsFgFLyiYyEYWEjTridcaKheJA.vbOhiocIcDPNMFSYkWDgYncMpduHA.ChangedEvent -= yYkWdJJVWdAmxbPIBmMJTDVEgMPm;
				KCnPsFgFLyiYyEYWEjTridcaKheJA.bYcrVqtLPhbPOoKrkaPlbHEWjLygb.ChangedEvent -= qCViIgANYdkSaCjgCKjdsocAngtDA;
				KCnPsFgFLyiYyEYWEjTridcaKheJA.mYuWjVGPwctxkGRqeqjMVZiXqUQ.ChangedEvent -= STlfpSSClhBfHJInYFgTePujYnqwB;
			}
		}

		private static void eluifZHoypzzwuCRdKpaEltadRFGA(bool P_0)
		{
			Action<bool> bHfOhXExzTwULJhJBFXiuCiXbVPK = BHfOhXExzTwULJhJBFXiuCiXbVPK;
			if (bHfOhXExzTwULJhJBFXiuCiXbVPK != null)
			{
				try
				{
					bHfOhXExzTwULJhJBFXiuCiXbVPK(P_0);
				}
				catch (Exception exception)
				{
					HandleCallbackException("ReInput.EditorPauseChangedEvent", exception);
				}
			}
		}

		private static void UoqRqIFlbmbMteHEhPCXEDTNMiuB(Func<ConfigVars, object> P_0)
		{
			bool flag = configVars.DoesPlatformUseFallback(UnityTools.platform, UnityTools.webplayerPlatform, isEditor);
			if (!flag)
			{
				List<IExternalInputManager> componentsInSelfAndChildren = UnityTools.GetComponentsInSelfAndChildren<IExternalInputManager>(HvlowarpEJjTwmVIKndWValQpypI);
				for (int i = 0; i < componentsInSelfAndChildren.Count; i++)
				{
					if (componentsInSelfAndChildren[i].Initialize(UnityTools.platform, MzljsQKNnzXzAuzZSJhUefpPCcEy) is PlatformInputManager cTpVLYlUovEEvUpcNlivRncrrQbW)
					{
						CTpVLYlUovEEvUpcNlivRncrrQbW = cTpVLYlUovEEvUpcNlivRncrrQbW;
						return;
					}
				}
			}
			if (flag)
			{
				ccufkjAWAhTbJaDuyGebaJsLxHKUA = true;
				CTpVLYlUovEEvUpcNlivRncrrQbW = new JnpGFOTHilhwgNkQYIjkXacPlURx(MzljsQKNnzXzAuzZSJhUefpPCcEy.updateLoop);
			}
			else if (configVars.DoesPlatformUseSDL2(UnityTools.platform, UnityTools.webplayerPlatform, isEditor))
			{
				try
				{
					CTpVLYlUovEEvUpcNlivRncrrQbW = new wMWDgGeOIAaKcfOESivauebILOjQA(MzljsQKNnzXzAuzZSJhUefpPCcEy, GetHardwareJoystickMap_InputManager, GetNewJoystickId, true, false, false);
					if (CTpVLYlUovEEvUpcNlivRncrrQbW == null)
					{
						throw new Exception();
					}
				}
				catch
				{
					Logger.LogError("SDL2 could not be initialized! Make sure you have the SDL2 library installed. Please see the documentation for more information. Rewired will fall back to Unity input. Certain features may not be available.");
					CTpVLYlUovEEvUpcNlivRncrrQbW = null;
				}
			}
			else if (UnityTools.platform == Platform.Windows || UnityTools.platform == Platform.WindowsAppStore || UnityTools.platform == Platform.WindowsUWP || UnityTools.platform == Platform.OSX || UnityTools.platform == Platform.Linux)
			{
				CTpVLYlUovEEvUpcNlivRncrrQbW = P_0(MzljsQKNnzXzAuzZSJhUefpPCcEy) as PlatformInputManager;
			}
			else if (UnityTools.platform == Platform.WebGL && !isEditor)
			{
				try
				{
					CTpVLYlUovEEvUpcNlivRncrrQbW = P_0(MzljsQKNnzXzAuzZSJhUefpPCcEy) as PlatformInputManager;
					if (CTpVLYlUovEEvUpcNlivRncrrQbW == null)
					{
						throw new Exception();
					}
				}
				catch
				{
					Logger.LogError("WebGL platform could not be initialized! Is the Rewired WebGL library installed? See the documentation for more information.");
					CTpVLYlUovEEvUpcNlivRncrrQbW = null;
				}
			}
			else if (UnityTools.platform == Platform.XboxOne && !isEditor)
			{
				try
				{
					CTpVLYlUovEEvUpcNlivRncrrQbW = new CustomInputManager(new XboxOneInputSource(), MzljsQKNnzXzAuzZSJhUefpPCcEy.updateLoop, GetHardwareJoystickMap_InputManager, GetNewJoystickId);
					if (CTpVLYlUovEEvUpcNlivRncrrQbW == null)
					{
						throw new Exception();
					}
				}
				catch
				{
					Logger.LogError("Xbox One platform could not be initialized! Is the Rewired Xbox One library installed? See the documentation for more information.");
					CTpVLYlUovEEvUpcNlivRncrrQbW = null;
				}
			}
			else if (UnityTools.platform == Platform.PS4 && !isEditor)
			{
				try
				{
					CTpVLYlUovEEvUpcNlivRncrrQbW = P_0(MzljsQKNnzXzAuzZSJhUefpPCcEy) as PlatformInputManager;
					if (CTpVLYlUovEEvUpcNlivRncrrQbW == null)
					{
						throw new Exception("Input Manager was null.");
					}
				}
				catch (Exception msg)
				{
					Logger.LogError("PS4 platform could not be initialized! Is the Rewired PS4 plugin installed? See the documentation for more information.");
					Logger.LogError(msg);
					CTpVLYlUovEEvUpcNlivRncrrQbW = null;
				}
			}
			else if (UnityTools.platform == Platform.PS5 && !isEditor)
			{
				try
				{
					CTpVLYlUovEEvUpcNlivRncrrQbW = P_0(MzljsQKNnzXzAuzZSJhUefpPCcEy) as PlatformInputManager;
					if (CTpVLYlUovEEvUpcNlivRncrrQbW == null)
					{
						throw new Exception("Input Manager was null.");
					}
				}
				catch (Exception msg2)
				{
					Logger.LogError("PS5 platform could not be initialized! Is the Rewired PS5 plugin installed? See the documentation for more information.");
					Logger.LogError(msg2);
					CTpVLYlUovEEvUpcNlivRncrrQbW = null;
				}
			}
			else if (UnityTools.platform == Platform.Stadia && !isEditor)
			{
				try
				{
					CTpVLYlUovEEvUpcNlivRncrrQbW = P_0(MzljsQKNnzXzAuzZSJhUefpPCcEy) as PlatformInputManager;
					if (CTpVLYlUovEEvUpcNlivRncrrQbW == null)
					{
						throw new Exception("Input Manager was null.");
					}
				}
				catch (Exception msg3)
				{
					Logger.LogError("Stadia platform could not be initialized! Is the Rewired Stadia library installed? See the documentation for more information.");
					Logger.LogError(msg3);
					CTpVLYlUovEEvUpcNlivRncrrQbW = null;
				}
			}
			else if ((UnityTools.platform == Platform.GameCoreXboxOne || UnityTools.platform == Platform.GameCoreScarlett) && !isEditor)
			{
				try
				{
					CTpVLYlUovEEvUpcNlivRncrrQbW = P_0(MzljsQKNnzXzAuzZSJhUefpPCcEy) as PlatformInputManager;
					if (CTpVLYlUovEEvUpcNlivRncrrQbW == null)
					{
						throw new Exception("Input Manager was null.");
					}
				}
				catch (Exception msg4)
				{
					string text = ((UnityTools.platform == Platform.GameCoreXboxOne) ? "Xbox One" : "Xbox Series X");
					Logger.LogError(text + " platform could not be initialized! Is the Rewired " + text + " library installed? See the documentation for more information.");
					Logger.LogError(msg4);
					CTpVLYlUovEEvUpcNlivRncrrQbW = null;
				}
			}
			else if (UnityTools.platform == Platform.Ouya && !isEditor)
			{
				Logger.LogError("Ouya is no longer supported.");
				CTpVLYlUovEEvUpcNlivRncrrQbW = null;
			}
			else if (UnityTools.isAndroidPlatform && !isEditor)
			{
				try
				{
					UnityTools.YamuuACgfaSgRUmyfSScpMCIdkMJ = P_0(MzljsQKNnzXzAuzZSJhUefpPCcEy) as IAndroidFallbackPlatformHelper;
				}
				catch (Exception msg5)
				{
					Logger.LogError(msg5);
				}
			}
			if (CTpVLYlUovEEvUpcNlivRncrrQbW == null)
			{
				ccufkjAWAhTbJaDuyGebaJsLxHKUA = true;
				CTpVLYlUovEEvUpcNlivRncrrQbW = new JnpGFOTHilhwgNkQYIjkXacPlURx(MzljsQKNnzXzAuzZSJhUefpPCcEy.updateLoop);
			}
		}

		private static void gNOuFVusZxBVSuCaAILcKBpFgAtbA()
		{
			if (DxzQhCtDRoyInHoxWcnNRxVqcFzB != MzljsQKNnzXzAuzZSJhUefpPCcEy.GetPlatformVar_ignoreInputWhenAppNotInFocus())
			{
				DxzQhCtDRoyInHoxWcnNRxVqcFzB = !DxzQhCtDRoyInHoxWcnNRxVqcFzB;
			}
		}

		private static void nRaBRAYQPLInWdZbPkcAoOMGefydA()
		{
			if (!(UnityTools.unityVersionObj == null))
			{
				Logger.LogWarning("The version of Rewired installed (" + programVersion + ") was not designed for Unity " + UnityTools.unityVersionObj.major + ". Please install Rewired for Unity " + UnityTools.unityVersionObj.major + ".\n\nThis warning does not mean that Rewired will not function, but it may not function optimally.\n\nSome different major versions of Unity download Asset Store assets to the same folder location on disk, so if you download an asset in one version of the Unity editor, then open another version of the Unity editor and install the asset without re-downloading it, the wrong asset version will be installed. To fix this, manually re-download Rewired in the Unity Asset Store panel in this version of the Unity Editor, then install it.\n\nIf you are using a beta version of a new major version of Unity, you will have to wait until the release of the final version before a compatible version of Rewired can be uploaded to the Asset Store. When the new version is ready, it will be available through the Unity Asset Store for download as usual.");
			}
		}
	}
}
