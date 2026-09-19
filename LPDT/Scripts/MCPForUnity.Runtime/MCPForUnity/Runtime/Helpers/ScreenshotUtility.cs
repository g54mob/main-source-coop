using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MCPForUnity.Runtime.Helpers
{
	public static class ScreenshotUtility
	{
		public const string DefaultFolder = "Assets/Screenshots";

		private static bool s_loggedLegacyScreenCaptureFallback;

		private static bool? s_screenCaptureModuleAvailable;

		private static MethodInfo s_captureScreenshotMethod;

		public const string ScreenCaptureModuleNotAvailableError = "The Screen Capture module (com.unity.modules.screencapture) is not enabled. To use screenshot capture with ScreenCapture API, please enable it in Unity: Window > Package Manager > Built-in > Screen Capture > Enable. Alternatively, MCP for Unity will use camera-based capture as a fallback if a Camera exists in the scene.";

		public static bool IsScreenCaptureModuleAvailable
		{
			get
			{
				if (!s_screenCaptureModuleAvailable.HasValue)
				{
					Type type = Type.GetType("UnityEngine.ScreenCapture, UnityEngine.ScreenCaptureModule") ?? Type.GetType("UnityEngine.ScreenCapture, UnityEngine.CoreModule");
					s_screenCaptureModuleAvailable = type != null;
					if (type != null)
					{
						s_captureScreenshotMethod = type.GetMethod("CaptureScreenshot", new Type[2]
						{
							typeof(string),
							typeof(int)
						});
					}
				}
				return s_screenCaptureModuleAvailable.Value;
			}
		}

		private static Camera FindAvailableCamera()
		{
			Camera main = Camera.main;
			if (main != null)
			{
				return main;
			}
			try
			{
				return UnityFindObjectsCompat.FindAll<Camera>().FirstOrDefault();
			}
			catch
			{
				return null;
			}
		}

		public static ScreenshotCaptureResult CaptureToProjectFolder(string fileName = null, int superSize = 1, bool ensureUniqueFileName = true, string folderOverride = null)
		{
			if (IsScreenCaptureModuleAvailable && s_captureScreenshotMethod != null)
			{
				ScreenshotCaptureResult result = PrepareCaptureResult(fileName, superSize, ensureUniqueFileName, folderOverride, isAsync: true);
				s_captureScreenshotMethod.Invoke(null, new object[2] { result.ProjectRelativePath, result.SuperSize });
				return result;
			}
			Debug.LogWarning("[MCP for Unity] The Screen Capture module (com.unity.modules.screencapture) is not enabled. To use screenshot capture with ScreenCapture API, please enable it in Unity: Window > Package Manager > Built-in > Screen Capture > Enable. Alternatively, MCP for Unity will use camera-based capture as a fallback if a Camera exists in the scene.");
			return CaptureWithCameraFallback(fileName, superSize, ensureUniqueFileName, folderOverride);
		}

		private static ScreenshotCaptureResult CaptureWithCameraFallback(string fileName, int superSize, bool ensureUniqueFileName, string folderOverride)
		{
			if (!s_loggedLegacyScreenCaptureFallback)
			{
				Debug.Log("[MCP for Unity] Using camera-based screenshot capture. This requires a Camera in the scene. For best results on Unity 2022.1+, ensure the Screen Capture module is enabled: Window > Package Manager > Built-in > Screen Capture > Enable.");
				s_loggedLegacyScreenCaptureFallback = true;
			}
			Camera camera = FindAvailableCamera();
			if (camera == null)
			{
				throw new InvalidOperationException("No camera found to capture screenshot. Camera-based capture requires a Camera in the scene. Either add a Camera to your scene, or enable the Screen Capture module: Window > Package Manager > Built-in > Screen Capture > Enable.");
			}
			return CaptureFromCameraToProjectFolder(camera, fileName, superSize, ensureUniqueFileName, includeImage: false, 0, folderOverride);
		}

		public static ScreenshotCaptureResult CaptureFromCameraToProjectFolder(Camera camera, string fileName = null, int superSize = 1, bool ensureUniqueFileName = true, bool includeImage = false, int maxResolution = 0, string folderOverride = null)
		{
			if (camera == null)
			{
				throw new ArgumentNullException("camera");
			}
			ScreenshotCaptureResult result = PrepareCaptureResult(fileName, superSize, ensureUniqueFileName, folderOverride, isAsync: false);
			int superSize2 = result.SuperSize;
			int num = Mathf.Max(1, (camera.pixelWidth > 0) ? camera.pixelWidth : Screen.width);
			int num2 = Mathf.Max(1, (camera.pixelHeight > 0) ? camera.pixelHeight : Screen.height);
			num *= superSize2;
			num2 *= superSize2;
			RenderTexture targetTexture = camera.targetTexture;
			RenderTexture active = RenderTexture.active;
			RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 24, RenderTextureFormat.ARGB32);
			Texture2D texture2D = null;
			Texture2D texture2D2 = null;
			string text = null;
			int imageWidth = 0;
			int imageHeight = 0;
			try
			{
				camera.targetTexture = temporary;
				camera.Render();
				RenderTexture.active = temporary;
				texture2D = new Texture2D(num, num2, TextureFormat.RGBA32, mipChain: false);
				texture2D.ReadPixels(new Rect(0f, 0f, num, num2), 0, 0);
				texture2D.Apply();
				byte[] array = texture2D.EncodeToPNG();
				File.WriteAllBytes(result.FullPath, array);
				if (includeImage)
				{
					int num3 = ((maxResolution > 0) ? maxResolution : 640);
					if (num > num3 || num2 > num3)
					{
						texture2D2 = DownscaleTexture(texture2D, num3);
						text = Convert.ToBase64String(texture2D2.EncodeToPNG());
						imageWidth = texture2D2.width;
						imageHeight = texture2D2.height;
					}
					else
					{
						text = Convert.ToBase64String(array);
						imageWidth = num;
						imageHeight = num2;
					}
				}
			}
			finally
			{
				camera.targetTexture = targetTexture;
				RenderTexture.active = active;
				RenderTexture.ReleaseTemporary(temporary);
				DestroyTexture(texture2D);
				DestroyTexture(texture2D2);
			}
			if (includeImage && text != null)
			{
				return new ScreenshotCaptureResult(result.FullPath, result.ProjectRelativePath, result.SuperSize, isAsync: false, text, imageWidth, imageHeight);
			}
			return result;
		}

		public static ScreenshotCaptureResult CaptureComposited(string fileName = null, int superSize = 1, bool ensureUniqueFileName = true, bool includeImage = false, int maxResolution = 0, string folderOverride = null)
		{
			if (!IsScreenCaptureModuleAvailable)
			{
				Camera camera = FindAvailableCamera();
				if (camera != null)
				{
					return CaptureFromCameraToProjectFolder(camera, fileName, superSize, ensureUniqueFileName, includeImage, maxResolution, folderOverride);
				}
				throw new InvalidOperationException("ScreenCapture module is unavailable and no fallback camera found.");
			}
			ScreenshotCaptureResult result = PrepareCaptureResult(fileName, superSize, ensureUniqueFileName, folderOverride, isAsync: false);
			Texture2D texture2D = null;
			Texture2D texture2D2 = null;
			string text = null;
			int imageWidth = 0;
			int imageHeight = 0;
			try
			{
				texture2D = ScreenCapture.CaptureScreenshotAsTexture(result.SuperSize);
				if (texture2D == null)
				{
					Camera camera2 = FindAvailableCamera();
					if (camera2 != null)
					{
						return CaptureFromCameraToProjectFolder(camera2, fileName, superSize, ensureUniqueFileName, includeImage, maxResolution, folderOverride);
					}
					throw new InvalidOperationException("ScreenCapture.CaptureScreenshotAsTexture returned null and no fallback camera available.");
				}
				int width = texture2D.width;
				int height = texture2D.height;
				byte[] array = texture2D.EncodeToPNG();
				File.WriteAllBytes(result.FullPath, array);
				if (includeImage)
				{
					int num = ((maxResolution > 0) ? maxResolution : 640);
					if (width > num || height > num)
					{
						texture2D2 = DownscaleTexture(texture2D, num);
						text = Convert.ToBase64String(texture2D2.EncodeToPNG());
						imageWidth = texture2D2.width;
						imageHeight = texture2D2.height;
					}
					else
					{
						text = Convert.ToBase64String(array);
						imageWidth = width;
						imageHeight = height;
					}
				}
			}
			finally
			{
				DestroyTexture(texture2D);
				DestroyTexture(texture2D2);
			}
			if (includeImage && text != null)
			{
				return new ScreenshotCaptureResult(result.FullPath, result.ProjectRelativePath, result.SuperSize, isAsync: false, text, imageWidth, imageHeight);
			}
			return result;
		}

		public static (string base64, int width, int height) RenderCameraToBase64(Camera camera, int maxResolution = 640)
		{
			if (camera == null)
			{
				throw new ArgumentNullException("camera");
			}
			int num = Mathf.Max(1, (camera.pixelWidth > 0) ? camera.pixelWidth : Screen.width);
			int num2 = Mathf.Max(1, (camera.pixelHeight > 0) ? camera.pixelHeight : Screen.height);
			RenderTexture targetTexture = camera.targetTexture;
			RenderTexture active = RenderTexture.active;
			RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 24, RenderTextureFormat.ARGB32);
			Texture2D texture2D = null;
			Texture2D texture2D2 = null;
			try
			{
				camera.targetTexture = temporary;
				camera.Render();
				RenderTexture.active = temporary;
				texture2D = new Texture2D(num, num2, TextureFormat.RGBA32, mipChain: false);
				texture2D.ReadPixels(new Rect(0f, 0f, num, num2), 0, 0);
				texture2D.Apply();
				int num3 = ((maxResolution > 0) ? maxResolution : 640);
				if (num > num3 || num2 > num3)
				{
					texture2D2 = DownscaleTexture(texture2D, num3);
					return (base64: Convert.ToBase64String(texture2D2.EncodeToPNG()), width: texture2D2.width, height: texture2D2.height);
				}
				return (base64: Convert.ToBase64String(texture2D.EncodeToPNG()), width: num, height: num2);
			}
			finally
			{
				camera.targetTexture = targetTexture;
				RenderTexture.active = active;
				RenderTexture.ReleaseTemporary(temporary);
				DestroyTexture(texture2D);
				DestroyTexture(texture2D2);
			}
		}

		public static Texture2D RenderCameraToTexture(Camera camera, int maxResolution = 640)
		{
			if (camera == null)
			{
				throw new ArgumentNullException("camera");
			}
			int num = Mathf.Max(1, (camera.pixelWidth > 0) ? camera.pixelWidth : Screen.width);
			int num2 = Mathf.Max(1, (camera.pixelHeight > 0) ? camera.pixelHeight : Screen.height);
			RenderTexture targetTexture = camera.targetTexture;
			RenderTexture active = RenderTexture.active;
			RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 24, RenderTextureFormat.ARGB32);
			Texture2D texture2D = null;
			try
			{
				camera.targetTexture = temporary;
				camera.Render();
				RenderTexture.active = temporary;
				texture2D = new Texture2D(num, num2, TextureFormat.RGBA32, mipChain: false);
				texture2D.ReadPixels(new Rect(0f, 0f, num, num2), 0, 0);
				texture2D.Apply();
				int num3 = ((maxResolution > 0) ? maxResolution : 640);
				if (num > num3 || num2 > num3)
				{
					Texture2D result = DownscaleTexture(texture2D, num3);
					DestroyTexture(texture2D);
					texture2D = null;
					return result;
				}
				Texture2D result2 = texture2D;
				texture2D = null;
				return result2;
			}
			finally
			{
				camera.targetTexture = targetTexture;
				RenderTexture.active = active;
				RenderTexture.ReleaseTemporary(temporary);
				DestroyTexture(texture2D);
			}
		}

		public static (string base64, int width, int height) ComposeContactSheet(List<Texture2D> tiles, List<string> labels, int padding = 4)
		{
			if (tiles == null || tiles.Count == 0)
			{
				throw new ArgumentException("No tiles to compose.", "tiles");
			}
			int width = tiles[0].width;
			int height = tiles[0].height;
			int count = tiles.Count;
			int num = Mathf.CeilToInt(Mathf.Sqrt(count));
			int num2 = Mathf.CeilToInt((float)count / (float)num);
			int num3 = Mathf.Max(14, height / 12);
			int num4 = width + padding;
			int num5 = height + num3 + padding;
			int num6 = num * num4 + padding;
			int num7 = num2 * num5 + padding;
			Texture2D texture2D = null;
			try
			{
				texture2D = new Texture2D(num6, num7, TextureFormat.RGBA32, mipChain: false);
				Color32 color = new Color32(30, 30, 30, byte.MaxValue);
				Color32[] array = new Color32[num6 * num7];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = color;
				}
				List<(string, int, int, int)> list = new List<(string, int, int, int)>();
				for (int j = 0; j < count; j++)
				{
					int num8 = j % num;
					int num9 = j / num;
					int num10 = padding + num8 * num4;
					int num11 = num7 - padding - (num9 + 1) * num5 + padding;
					Color32[] pixels = tiles[j].GetPixels32();
					for (int k = 0; k < height; k++)
					{
						int sourceIndex = k * width;
						int destinationIndex = (num11 + num3 + k) * num6 + num10;
						Array.Copy(pixels, sourceIndex, array, destinationIndex, width);
					}
					Color32 color2 = new Color32(20, 20, 20, 220);
					for (int l = 0; l < num3; l++)
					{
						int num12 = (num11 + l) * num6 + num10;
						for (int m = 0; m < width; m++)
						{
							array[num12 + m] = color2;
						}
					}
					if (labels != null && j < labels.Count && !string.IsNullOrEmpty(labels[j]))
					{
						list.Add((labels[j], num10 + 3, num11 + 2, num3 - 4));
					}
				}
				texture2D.SetPixels32(array);
				foreach (var (text, startX, startY, charHeight) in list)
				{
					DrawText(texture2D, text, startX, startY, charHeight, Color.white);
				}
				texture2D.Apply();
				return (base64: Convert.ToBase64String(texture2D.EncodeToPNG()), width: num6, height: num7);
			}
			finally
			{
				foreach (Texture2D tile in tiles)
				{
					DestroyTexture(tile);
				}
				DestroyTexture(texture2D);
			}
		}

		private static void DrawText(Texture2D tex, string text, int startX, int startY, int charHeight, Color color)
		{
			int num = Mathf.Max(4, charHeight * 5 / 7);
			int num2 = Mathf.Max(1, num / 5);
			int num3 = startX;
			foreach (char c in text)
			{
				if (num3 + num > tex.width)
				{
					break;
				}
				ulong glyph = GetGlyph(c);
				if (glyph != 0L)
				{
					for (int j = 0; j < 7; j++)
					{
						for (int k = 0; k < 5; k++)
						{
							if (((glyph >> (6 - j) * 5 + (4 - k)) & 1) != 1)
							{
								continue;
							}
							int num4 = num3 + k * num / 5;
							int num5 = num3 + (k + 1) * num / 5;
							int num6 = startY + (6 - j) * charHeight / 7;
							int num7 = startY + (7 - j) * charHeight / 7;
							for (int l = num6; l < num7 && l < tex.height; l++)
							{
								for (int m = num4; m < num5 && m < tex.width; m++)
								{
									tex.SetPixel(m, l, color);
								}
							}
						}
					}
				}
				num3 += num + num2;
			}
		}

		private static ulong GetGlyph(char c)
		{
			return char.ToUpperInvariant(c) switch
			{
				'A' => 15621670449uL, 
				'B' => 32801506878uL, 
				'C' => 15620129326uL, 
				'D' => 30687151708uL, 
				'E' => 33840644639uL, 
				'F' => 33840644624uL, 
				'G' => 15620359726uL, 
				'H' => 18842895921uL, 
				'I' => 15170932878uL, 
				'K' => 18879369809uL, 
				'L' => 17734058527uL, 
				'M' => 19182306865uL, 
				'N' => 19115132465uL, 
				'O' => 15621211694uL, 
				'R' => 32801509969uL, 
				'S' => 15620048430uL, 
				'T' => 33424543876uL, 
				'U' => 18842437166uL, 
				'V' => 18842429764uL, 
				'W' => 18842572657uL, 
				'Y' => 18834657412uL, 
				'0' => 15692650286uL, 
				'1' => 4701950094uL, 
				'2' => 15603929375uL, 
				'3' => 15604057646uL, 
				'4' => 2359917634uL, 
				'5' => 33854359086uL, 
				'6' => 15620589102uL, 
				'7' => 33321787656uL, 
				'8' => 15621113390uL, 
				'9' => 15621129774uL, 
				'J' => 7585466956uL, 
				'P' => 32801505808uL, 
				'Q' => 15621215821uL, 
				'X' => 18593485137uL, 
				'Z' => 33321787935uL, 
				'-' => 1015808uL, 
				'_' => 31uL, 
				' ' => 0uL, 
				'+' => 139432064uL, 
				_ => 0uL, 
			};
		}

		public static Texture2D DownscaleTexture(Texture2D source, int maxEdge)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (maxEdge <= 0)
			{
				throw new ArgumentOutOfRangeException("maxEdge", maxEdge, "maxEdge must be > 0.");
			}
			int width = source.width;
			int height = source.height;
			float a = Mathf.Min((float)maxEdge / (float)width, (float)maxEdge / (float)height);
			a = Mathf.Min(a, 1f);
			int num = Mathf.Max(1, Mathf.RoundToInt((float)width * a));
			int num2 = Mathf.Max(1, Mathf.RoundToInt((float)height * a));
			RenderTexture active = RenderTexture.active;
			RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 0, RenderTextureFormat.ARGB32);
			temporary.filterMode = FilterMode.Bilinear;
			try
			{
				Graphics.Blit(source, temporary);
				RenderTexture.active = temporary;
				Texture2D texture2D = new Texture2D(num, num2, TextureFormat.RGBA32, mipChain: false);
				texture2D.ReadPixels(new Rect(0f, 0f, num, num2), 0, 0);
				texture2D.Apply();
				return texture2D;
			}
			finally
			{
				RenderTexture.active = active;
				RenderTexture.ReleaseTemporary(temporary);
			}
		}

		private static void DestroyTexture(Texture2D tex)
		{
			if (!(tex == null))
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(tex);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(tex);
				}
			}
		}

		private static ScreenshotCaptureResult PrepareCaptureResult(string fileName, int superSize, bool ensureUniqueFileName, string folderOverride, bool isAsync)
		{
			int superSize2 = Mathf.Max(1, superSize);
			string path = BuildFileName(fileName);
			string text = ResolveFolderAbsolute(folderOverride);
			Directory.CreateDirectory(text);
			string text2 = Path.Combine(text, path);
			if (ensureUniqueFileName)
			{
				text2 = EnsureUnique(text2);
			}
			string text3 = text2.Replace('\\', '/');
			string projectRelativePath = ToProjectRelativePath(text3);
			return new ScreenshotCaptureResult(text3, projectRelativePath, superSize2, isAsync);
		}

		public static string ResolveFolderAbsolute(string folderOverride)
		{
			string text = GetProjectRootPath().TrimEnd('/');
			string text2 = (string.IsNullOrWhiteSpace(folderOverride) ? "Assets/Screenshots" : folderOverride.Trim());
			text2 = text2.Replace('\\', '/').TrimEnd('/');
			string text3 = Path.GetFullPath(Path.IsPathRooted(text2) ? text2 : Path.Combine(text, text2)).Replace('\\', '/').TrimEnd('/');
			string text4 = text;
			StringComparison comparisonType = ((Application.platform == RuntimePlatform.WindowsEditor) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
			if (!text3.Equals(text4, comparisonType) && !text3.StartsWith(text4 + "/", comparisonType))
			{
				throw new InvalidOperationException("Screenshot folder '" + folderOverride + "' resolves outside the Unity project root ('" + text3 + "'). Use a project-relative path (e.g. 'Assets/Screenshots' or 'Captures').");
			}
			return text3;
		}

		public static string ToProjectRelativePath(string normalizedFullPath)
		{
			if (string.IsNullOrEmpty(normalizedFullPath))
			{
				return normalizedFullPath;
			}
			string projectRootPath = GetProjectRootPath();
			string text = normalizedFullPath.Replace('\\', '/');
			if (text.StartsWith(projectRootPath, StringComparison.OrdinalIgnoreCase))
			{
				return text.Substring(projectRootPath.Length).TrimStart('/');
			}
			return text;
		}

		public static bool IsUnderAssets(string projectRelativePath)
		{
			if (string.IsNullOrEmpty(projectRelativePath))
			{
				return false;
			}
			string text = projectRelativePath.Replace('\\', '/').TrimStart('/');
			if (!text.Equals("Assets", StringComparison.OrdinalIgnoreCase))
			{
				return text.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase);
			}
			return true;
		}

		private static string BuildFileName(string fileName)
		{
			string fileName2 = (string.IsNullOrWhiteSpace(fileName) ? $"screenshot-{DateTime.Now:yyyyMMdd-HHmmss}" : fileName.Trim());
			fileName2 = SanitizeFileName(fileName2);
			if (!fileName2.EndsWith(".png", StringComparison.OrdinalIgnoreCase) && !fileName2.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) && !fileName2.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
			{
				fileName2 += ".png";
			}
			return fileName2;
		}

		private static string SanitizeFileName(string fileName)
		{
			char[] invalidChars = Path.GetInvalidFileNameChars();
			string text = new string(fileName.Select((char ch) => (!invalidChars.Contains(ch) && ch != '/' && ch != '\\') ? ch : '_').ToArray());
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
			return "screenshot";
		}

		private static string EnsureUnique(string path)
		{
			if (!File.Exists(path))
			{
				return path;
			}
			string path2 = Path.GetDirectoryName(path) ?? string.Empty;
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
			string extension = Path.GetExtension(path);
			int num = 1;
			string text;
			do
			{
				text = Path.Combine(path2, $"{fileNameWithoutExtension}-{num}{extension}");
				num++;
			}
			while (File.Exists(text));
			return text;
		}

		private static string GetProjectRootPath()
		{
			string fullPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
			fullPath = fullPath.Replace('\\', '/');
			if (!fullPath.EndsWith("/", StringComparison.Ordinal))
			{
				fullPath += "/";
			}
			return fullPath;
		}
	}
}
