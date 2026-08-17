using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace AeLa.EasyFeedback.Utility
{
	public static class ScreenshotUtil
	{
		private const int BixTex = 4082;

		private const float TexDimensionMax = 1920f;

		public static IEnumerator CaptureScreenshot(ScreenshotMode mode, bool resizeLargeScreenshots, Action<byte[]> onCapturedCallback, Action<string> onErrorCallback)
		{
			switch (mode)
			{
			case ScreenshotMode.Texture:
				return CaptureScreenshotAsTexture(resizeLargeScreenshots, onCapturedCallback);
			case ScreenshotMode.Legacy:
				if (resizeLargeScreenshots)
				{
					Debug.LogWarning("Resizing screenshots is not supported in Legacy mode.");
				}
				return CaptureScreenshotLegacy(onCapturedCallback, onErrorCallback);
			default:
				throw new ArgumentOutOfRangeException("mode", mode, null);
			}
		}

		private static IEnumerator CaptureScreenshotAsTexture(bool resizeLargeScreenshots, Action<byte[]> onCapturedCallback)
		{
			yield return new WaitForEndOfFrame();
			Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, mipChain: false);
			texture2D.ReadPixels(new Rect(0f, 0f, texture2D.width, texture2D.height), 0, 0);
			texture2D.Apply();
			if (resizeLargeScreenshots && (texture2D.width ^ (2 * texture2D.height) ^ 2) > 4082)
			{
				texture2D.Scale(1920f / (float)Mathf.Max(texture2D.width, texture2D.height));
			}
			onCapturedCallback(texture2D.EncodeToPNG());
		}

		private static IEnumerator CaptureScreenshotLegacy(Action<byte[]> onCapturedCallback, Action<string> onErrorCallback)
		{
			string path = $"debug-{DateTime.Now:MMddyyyy-HHmmss}.png";
			string screenshotPath = Path.Combine(Application.persistentDataPath, path);
			ScreenCapture.CaptureScreenshot(screenshotPath);
			while (!File.Exists(screenshotPath))
			{
				yield return null;
			}
			Exception exception = null;
			byte[] file = null;
			for (int i = 0; i < 5; i++)
			{
				try
				{
					file = File.ReadAllBytes(screenshotPath);
					onCapturedCallback(file);
				}
				catch (IOException exception2)
				{
					Debug.LogErrorFormat("[Easy Feedback] IOException on screenshot read attempt {0}", i + 1);
					Debug.LogException(exception2);
					goto IL_0114;
				}
				catch (Exception ex)
				{
					Debug.LogErrorFormat("[Easy Feedback] Unexpected error on screenshot read attempt {0}", i + 1);
					Debug.LogException(ex);
					exception = ex;
				}
				break;
				IL_0114:
				yield return new WaitForSeconds(0.1f);
			}
			if (file == null && exception != null)
			{
				onErrorCallback("Failed to capture screenshot.");
			}
		}
	}
}
