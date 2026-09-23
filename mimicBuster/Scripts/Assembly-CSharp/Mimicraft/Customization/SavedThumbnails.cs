using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mimicraft.Customization
{
	public static class SavedThumbnails
	{
		private const string Extension = ".png";

		private static readonly Dictionary<string, Sprite> cache = new Dictionary<string, Sprite>();

		public static string PathFor(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				return "";
			}
			string directoryName = Path.GetDirectoryName(filePath);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
			if (!string.IsNullOrEmpty(directoryName))
			{
				return Path.Combine(directoryName, fileNameWithoutExtension + ".png");
			}
			return fileNameWithoutExtension + ".png";
		}

		public static bool Exists(string filePath)
		{
			string text = PathFor(filePath);
			if (text.Length > 0)
			{
				return File.Exists(text);
			}
			return false;
		}

		public static Sprite Load(string filePath)
		{
			string text = PathFor(filePath);
			if (text.Length == 0)
			{
				return null;
			}
			if (cache.TryGetValue(text, out var value) && value != null)
			{
				return value;
			}
			if (!File.Exists(text))
			{
				return null;
			}
			byte[] data;
			try
			{
				data = File.ReadAllBytes(text);
			}
			catch (IOException ex)
			{
				Debug.LogWarning("[SavedThumbnails] '" + text + "' okunamadi: " + ex.Message);
				return null;
			}
			Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, mipChain: false);
			if (!texture2D.LoadImage(data))
			{
				Object.Destroy(texture2D);
				Debug.LogWarning("[SavedThumbnails] '" + text + "' bir PNG gibi gorunmuyor.");
				return null;
			}
			texture2D.wrapMode = TextureWrapMode.Clamp;
			Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
			cache[text] = sprite;
			return sprite;
		}

		public static void Write(string filePath, Texture2D texture)
		{
			string text = PathFor(filePath);
			if (text.Length == 0 || texture == null)
			{
				return;
			}
			try
			{
				string directoryName = Path.GetDirectoryName(text);
				if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.WriteAllBytes(text, texture.EncodeToPNG());
			}
			catch (IOException ex)
			{
				Debug.LogWarning("[SavedThumbnails] '" + text + "' yazilamadi: " + ex.Message);
				return;
			}
			Forget(text);
		}

		public static void Delete(string filePath)
		{
			string text = PathFor(filePath);
			if (text.Length == 0)
			{
				return;
			}
			Forget(text);
			try
			{
				if (File.Exists(text))
				{
					File.Delete(text);
				}
			}
			catch (IOException ex)
			{
				Debug.LogWarning("[SavedThumbnails] '" + text + "' silinemedi: " + ex.Message);
			}
		}

		private static void Forget(string path)
		{
			if (!cache.TryGetValue(path, out var value))
			{
				return;
			}
			cache.Remove(path);
			if (!(value == null))
			{
				Texture2D texture = value.texture;
				Object.Destroy(value);
				if (texture != null)
				{
					Object.Destroy(texture);
				}
			}
		}
	}
}
