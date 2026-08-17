using UnityEngine;

namespace EvilCore.Managers
{
	public class GameManager : MonoBehaviour, IGameManager
	{
		[SerializeField]
		private FpsLimit fpsLimit;

		[SerializeField]
		private ResolutionType resolutionType = ResolutionType.Windowed;

		[SerializeField]
		private ResolutionQuality resolutionQuality;

		private void Awake()
		{
			Object.DontDestroyOnLoad(base.gameObject);
			Application.runInBackground = true;
			Application.targetFrameRate = ResolveFpsLimit(fpsLimit);
			QualitySettings.vSyncCount = 0;
			ApplyDisplayMode(resolutionType, resolutionQuality);
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			Application.runInBackground = true;
			Application.targetFrameRate = ResolveFpsLimit(fpsLimit);
		}

		public void QuitGame()
		{
			Application.Quit();
		}

		public void SetFpsLimit(FpsLimit limit)
		{
			fpsLimit = limit;
			Application.targetFrameRate = ResolveFpsLimit(limit);
		}

		public void SetDisplayMode(ResolutionType type, ResolutionQuality quality)
		{
			resolutionType = type;
			resolutionQuality = quality;
			ApplyDisplayMode(type, quality);
		}

		public void SetResolution(ResolutionType type, int width, int height)
		{
			resolutionType = type;
			FullScreenMode fullscreenMode = ResolveFullScreenMode(type);
			if (type == ResolutionType.Borderless || width <= 0 || height <= 0)
			{
				var (width2, height2) = GetNativeDisplayResolution();
				Screen.SetResolution(width2, height2, fullscreenMode);
			}
			else
			{
				Screen.SetResolution(width, height, fullscreenMode);
			}
		}

		private static void ApplyDisplayMode(ResolutionType type, ResolutionQuality quality)
		{
			FullScreenMode fullscreenMode = ResolveFullScreenMode(type);
			int width;
			int height;
			if (type == ResolutionType.Borderless)
			{
				(width, height) = GetNativeDisplayResolution();
			}
			else
			{
				(width, height) = ResolveResolution(quality);
			}
			Screen.SetResolution(width, height, fullscreenMode);
		}

		private static FullScreenMode ResolveFullScreenMode(ResolutionType type)
		{
			return type switch
			{
				ResolutionType.Fullscreen => FullScreenMode.FullScreenWindow, 
				ResolutionType.Borderless => FullScreenMode.FullScreenWindow, 
				ResolutionType.Windowed => FullScreenMode.Windowed, 
				_ => FullScreenMode.Windowed, 
			};
		}

		private static int ResolveFpsLimit(FpsLimit limit)
		{
			return limit switch
			{
				FpsLimit.Fps60 => 60, 
				FpsLimit.Fps90 => 90, 
				FpsLimit.Fps120 => 120, 
				FpsLimit.Fps144 => 144, 
				FpsLimit.Unlimited => -1, 
				_ => 60, 
			};
		}

		private static (int width, int height) ResolveResolution(ResolutionQuality quality)
		{
			return quality switch
			{
				ResolutionQuality.Resolution1080P => (width: 1920, height: 1080), 
				ResolutionQuality.Resolution1440P => (width: 2560, height: 1440), 
				ResolutionQuality.Resolution4K => (width: 3840, height: 2160), 
				ResolutionQuality.Auto => GetNativeDisplayResolution(), 
				_ => GetNativeDisplayResolution(), 
			};
		}

		private static (int width, int height) GetNativeDisplayResolution()
		{
			int num = Display.main.systemWidth;
			int num2 = Display.main.systemHeight;
			if (num <= 0 || num2 <= 0)
			{
				Resolution currentResolution = Screen.currentResolution;
				num = currentResolution.width;
				num2 = currentResolution.height;
			}
			return (width: num, height: num2);
		}
	}
}
