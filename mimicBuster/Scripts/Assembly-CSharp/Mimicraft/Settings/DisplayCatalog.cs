using System.Collections.Generic;
using Mimicraft.Localization;
using UnityEngine;

namespace Mimicraft.Settings
{
	public static class DisplayCatalog
	{
		public const string AutoAspectId = "";

		private const float AspectTolerance = 0.02f;

		private static readonly (string Id, float Ratio)[] KnownAspects = new(string, float)[7]
		{
			("5:4", 1.25f),
			("4:3", 1.3333334f),
			("3:2", 1.5f),
			("16:10", 1.6f),
			("16:9", 1.7777778f),
			("21:9", 2.3703704f),
			("32:9", 3.5555556f)
		};

		private static readonly List<DisplayInfo> displayBuffer = new List<DisplayInfo>();

		public static IReadOnlyList<DisplayInfo> Displays()
		{
			displayBuffer.Clear();
			Screen.GetDisplayLayout(displayBuffer);
			return displayBuffer;
		}

		public static string DisplayLabel(int index, DisplayInfo info)
		{
			string arg = (string.IsNullOrWhiteSpace(info.name) ? Loc.Format("Video.DisplayNumbered", index + 1) : info.name);
			return $"{arg}  ({info.width}x{info.height})";
		}

		public static List<string> AspectIds()
		{
			List<string> list = new List<string> { "" };
			Resolution[] resolutions = Screen.resolutions;
			for (int i = 0; i < resolutions.Length; i++)
			{
				Resolution resolution = resolutions[i];
				string item = AspectIdOf(resolution.width, resolution.height);
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		public static string AspectLabel(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				return id;
			}
			return Loc.Get("Video.AspectRatio.Auto");
		}

		public static string AspectIdOf(int width, int height)
		{
			if (height <= 0)
			{
				return "?";
			}
			float num = (float)width / (float)height;
			(string, float)[] knownAspects = KnownAspects;
			for (int i = 0; i < knownAspects.Length; i++)
			{
				var (result, num2) = knownAspects[i];
				if (Mathf.Abs(num - num2) <= 0.02f)
				{
					return result;
				}
			}
			int num3 = GreatestCommonDivisor(width, height);
			return $"{width / num3}:{height / num3}";
		}

		public static List<Resolution> ResolutionsFor(string aspectId)
		{
			List<Resolution> list = new List<Resolution>();
			Resolution[] resolutions = Screen.resolutions;
			for (int i = 0; i < resolutions.Length; i++)
			{
				Resolution item = resolutions[i];
				if ((string.IsNullOrEmpty(aspectId) || !(AspectIdOf(item.width, item.height) != aspectId)) && !ContainsSize(list, item.width, item.height))
				{
					list.Add(item);
				}
			}
			list.Reverse();
			return list;
		}

		public static string ResolutionLabel(Resolution resolution)
		{
			return $"{resolution.width} x {resolution.height}";
		}

		public static List<FullScreenMode> WindowModes()
		{
			List<FullScreenMode> list = new List<FullScreenMode>
			{
				FullScreenMode.FullScreenWindow,
				FullScreenMode.Windowed
			};
			if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
			{
				list.Add(FullScreenMode.MaximizedWindow);
			}
			return list;
		}

		public static string WindowModeLabel(FullScreenMode mode)
		{
			return mode switch
			{
				FullScreenMode.ExclusiveFullScreen => Loc.Get("Video.WindowMode.Fullscreen"), 
				FullScreenMode.FullScreenWindow => Loc.Get("Video.WindowMode.Fullscreen"), 
				FullScreenMode.MaximizedWindow => Loc.Get("Video.WindowMode.Maximized"), 
				_ => Loc.Get("Video.WindowMode.Windowed"), 
			};
		}

		private static bool ContainsSize(List<Resolution> list, int width, int height)
		{
			foreach (Resolution item in list)
			{
				if (item.width == width && item.height == height)
				{
					return true;
				}
			}
			return false;
		}

		private static int GreatestCommonDivisor(int a, int b)
		{
			while (b != 0)
			{
				int num = b;
				int num2 = a % b;
				a = num;
				b = num2;
			}
			return Mathf.Max(1, a);
		}
	}
}
