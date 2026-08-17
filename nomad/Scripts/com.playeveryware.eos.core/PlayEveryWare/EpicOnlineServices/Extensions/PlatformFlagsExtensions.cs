using System;
using System.Collections.Generic;
using Epic.OnlineServices.Platform;
using PlayEveryWare.EpicOnlineServices.Utility;

namespace PlayEveryWare.EpicOnlineServices.Extensions
{
	public static class PlatformFlagsExtensions
	{
		public static Dictionary<string, PlatformFlags> CustomMappings { get; } = new Dictionary<string, PlatformFlags>
		{
			{
				"EOS_PF_NONE",
				PlatformFlags.None
			},
			{
				"EOS_PF_LOADING_IN_EDITOR",
				PlatformFlags.LoadingInEditor
			},
			{
				"EOS_PF_DISABLE_OVERLAY",
				PlatformFlags.DisableOverlay
			},
			{
				"EOS_PF_DISABLE_SOCIAL_OVERLAY",
				PlatformFlags.DisableSocialOverlay
			},
			{
				"EOS_PF_WINDOWS_ENABLE_OVERLAY_D3D9",
				PlatformFlags.WindowsEnableOverlayD3D9
			},
			{
				"EOS_PF_WINDOWS_ENABLE_OVERLAY_D3D10",
				PlatformFlags.WindowsEnableOverlayD3D10
			},
			{
				"EOS_PF_WINDOWS_ENABLE_OVERLAY_OPENGL",
				PlatformFlags.WindowsEnableOverlayOpengl
			},
			{
				"EOS_PF_CONSOLE_ENABLE_OVERLAY_AUTOMATIC_UNLOADING",
				PlatformFlags.ConsoleEnableOverlayAutomaticUnloading
			},
			{
				"EOS_PF_RESERVED1",
				PlatformFlags.Reserved1
			}
		};

		public static string GetDescription(this PlatformFlags platformFlags)
		{
			switch (platformFlags)
			{
			case PlatformFlags.None:
			case PlatformFlags.LoadingInEditor:
			case PlatformFlags.DisableOverlay:
			case PlatformFlags.LoadingInEditor | PlatformFlags.DisableOverlay:
			case PlatformFlags.DisableSocialOverlay:
			case PlatformFlags.LoadingInEditor | PlatformFlags.DisableSocialOverlay:
			case PlatformFlags.DisableOverlay | PlatformFlags.DisableSocialOverlay:
			case PlatformFlags.LoadingInEditor | PlatformFlags.DisableOverlay | PlatformFlags.DisableSocialOverlay:
			case PlatformFlags.Reserved1:
			case PlatformFlags.LoadingInEditor | PlatformFlags.Reserved1:
			case PlatformFlags.DisableOverlay | PlatformFlags.Reserved1:
			case PlatformFlags.LoadingInEditor | PlatformFlags.DisableOverlay | PlatformFlags.Reserved1:
			case PlatformFlags.DisableSocialOverlay | PlatformFlags.Reserved1:
			case PlatformFlags.LoadingInEditor | PlatformFlags.DisableSocialOverlay | PlatformFlags.Reserved1:
			case PlatformFlags.DisableOverlay | PlatformFlags.DisableSocialOverlay | PlatformFlags.Reserved1:
			case PlatformFlags.LoadingInEditor | PlatformFlags.DisableOverlay | PlatformFlags.DisableSocialOverlay | PlatformFlags.Reserved1:
			case PlatformFlags.WindowsEnableOverlayD3D9:
				if (platformFlags <= PlatformFlags.Reserved1)
				{
					switch ((uint)platformFlags)
					{
					case 0u:
						return "No flags.";
					case 1u:
						return "A bit that indicates the SDK is being loaded in a game editor, like Unity or UE4 Play-in-Editor";
					case 2u:
						return "A bit that indicates the SDK should skip initialization of the overlay, which is used by the in-app purchase flow and social overlay. This bit is implied by LoadingInEditor.";
					case 4u:
						return "A bit that indicates the SDK should skip initialization of the social overlay, which provides an overlay UI for social features. This bit is implied by LoadingInEditor or DisableOverlay.";
					case 8u:
						return "A reserved bit.";
					case 3u:
					case 5u:
					case 6u:
					case 7u:
						goto end_IL_0004;
					}
				}
				if (platformFlags != PlatformFlags.WindowsEnableOverlayD3D9)
				{
					break;
				}
				return "A bit that indicates your game would like to opt-in to experimental Direct3D 9 support for the overlay. This flag is only relevant on Windows.";
			case PlatformFlags.WindowsEnableOverlayD3D10:
				return "A bit that indicates your game would like to opt-in to experimental Direct3D 10 support for the overlay. This flag is only relevant on Windows.";
			case PlatformFlags.WindowsEnableOverlayOpengl:
				return "A bit that indicates your game would like to opt-in to experimental OpenGL support for the overlay. This flag is only relevant on Windows.";
			case PlatformFlags.ConsoleEnableOverlayAutomaticUnloading:
				{
					return "A bit that indicates your game would like to opt-in to automatic unloading of the overlay module when possible. This flag is only relevant on Consoles.";
				}
				end_IL_0004:
				break;
			}
			throw new ArgumentOutOfRangeException("platformFlags", platformFlags, null);
		}

		public static bool TryParse(IList<string> stringFlags, out PlatformFlags result)
		{
			return EnumUtility<PlatformFlags>.TryParse(stringFlags, CustomMappings, out result, PlatformFlags.None);
		}

		public static WrappedPlatformFlags Wrap(this PlatformFlags internalFlags)
		{
			return (WrappedPlatformFlags)internalFlags;
		}

		public static PlatformFlags Unwrap(this WrappedPlatformFlags wrappedFlags)
		{
			return (PlatformFlags)wrappedFlags;
		}
	}
}
