using System;
using System.IO;
using UnityEngine;

namespace NomadDrive.Features.SaveSystem
{
	public static class SaveThumbnailCapture
	{
		public static bool TryCaptureToFile(Camera camera, string filePath, int width, int height, int jpgQuality)
		{
			if (camera == null || string.IsNullOrEmpty(filePath))
			{
				return false;
			}
			RenderTexture renderTexture = null;
			Texture2D texture2D = null;
			RenderTexture targetTexture = camera.targetTexture;
			RenderTexture active = RenderTexture.active;
			try
			{
				renderTexture = (camera.targetTexture = RenderTexture.GetTemporary(width, height, 24));
				camera.Render();
				RenderTexture.active = renderTexture;
				texture2D = new Texture2D(width, height, TextureFormat.RGB24, mipChain: false);
				texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
				texture2D.Apply();
				byte[] bytes = texture2D.EncodeToJPG(jpgQuality);
				string directoryName = Path.GetDirectoryName(filePath);
				if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.WriteAllBytes(filePath, bytes);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
			finally
			{
				camera.targetTexture = targetTexture;
				RenderTexture.active = active;
				if (renderTexture != null)
				{
					RenderTexture.ReleaseTemporary(renderTexture);
				}
				if (texture2D != null)
				{
					UnityEngine.Object.Destroy(texture2D);
				}
			}
		}
	}
}
