using UnityEngine;

namespace Mimicraft.UI
{
	public static class PlaceholderIcons
	{
		public static Sprite CreateSquare(Color color, int size = 64)
		{
			Texture2D texture2D = new Texture2D(size, size, TextureFormat.RGBA32, mipChain: false);
			Color[] array = new Color[size * size];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = color;
			}
			texture2D.SetPixels(array);
			texture2D.Apply();
			return Sprite.Create(texture2D, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f));
		}

		public static Sprite CreateCheckerboard(int size = 64, int cellSize = 8)
		{
			Color color = new Color(0.65f, 0.65f, 0.65f, 1f);
			Color color2 = new Color(0.4f, 0.4f, 0.4f, 1f);
			Texture2D texture2D = new Texture2D(size, size, TextureFormat.RGBA32, mipChain: false);
			Color[] array = new Color[size * size];
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					bool flag = (j / cellSize + i / cellSize) % 2 == 0;
					array[i * size + j] = (flag ? color : color2);
				}
			}
			texture2D.SetPixels(array);
			texture2D.Apply();
			return Sprite.Create(texture2D, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f));
		}
	}
}
