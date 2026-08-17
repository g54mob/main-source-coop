using System.Collections.Generic;
using UnityEngine;

namespace QFSW.QC.Extras
{
	public static class ScreenCommands
	{
		[Command("fullscreen", "fullscreen state of the application.", Platform.AllPlatforms, MonoTargetType.Single)]
		private static bool Fullscreen
		{
			get
			{
				return Screen.fullScreen;
			}
			set
			{
				Screen.fullScreen = value;
			}
		}

		[Command("screen-dpi", "dpi of the current device's screen.", Platform.AllPlatforms, MonoTargetType.Single)]
		private static float DPI => Screen.dpi;

		[Command("screen-orientation", "the orientation of the screen.", Platform.AllPlatforms, MonoTargetType.Single)]
		[CommandPlatform(Platform.MobilePlatforms)]
		private static ScreenOrientation Orientation
		{
			get
			{
				return Screen.orientation;
			}
			set
			{
				Screen.orientation = value;
			}
		}

		[Command("current-resolution", "current resolution of the application or window.", Platform.AllPlatforms, MonoTargetType.Single)]
		private static Resolution GetCurrentResolution()
		{
			return new Resolution
			{
				width = Screen.width,
				height = Screen.height,
				refreshRateRatio = Screen.currentResolution.refreshRateRatio
			};
		}

		[Command("supported-resolutions", "all resolutions supported by this device in fullscreen mode.", Platform.AllPlatforms, MonoTargetType.Single)]
		[CommandPlatform(~Platform.WebGLPlayer)]
		private static IEnumerable<Resolution> GetSupportedResolutions()
		{
			Resolution[] resolutions = Screen.resolutions;
			for (int i = 0; i < resolutions.Length; i++)
			{
				yield return resolutions[i];
			}
		}

		[Command("set-resolution", Platform.AllPlatforms, MonoTargetType.Single)]
		private static void SetResolution(int x, int y)
		{
			SetResolution(x, y, Screen.fullScreen);
		}

		[Command("set-resolution", "sets the resolution of the current application, optionally setting the fullscreen state too.", Platform.AllPlatforms, MonoTargetType.Single)]
		private static void SetResolution(int x, int y, bool fullscreen)
		{
			Screen.SetResolution(x, y, fullscreen);
		}

		[Command("capture-screenshot", Platform.AllPlatforms, MonoTargetType.Single)]
		[CommandDescription("Captures a screenshot and saves it to the supplied file path as a PNG.\nIf superSize is supplied the screenshot will be captured at a higher than native resolution.")]
		private static void CaptureScreenshot([CommandParameterDescription("The name of the file to save the screenshot in")] string filename, [CommandParameterDescription("Factor by which to increase resolution")] int superSize = 1)
		{
			ScreenCapture.CaptureScreenshot(filename, superSize);
		}
	}
}
