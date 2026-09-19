using System;
using System.Collections.Generic;
using UnityEngine;

namespace INab.BetterFog.Runtime
{
	[CreateAssetMenu(fileName = "NewGradientName", menuName = "BetterFog/Gradient")]
	public class GradientTexture : ScriptableObject, IEquatable<Texture2D>, ISerializationCallbackReceiver, IGradientTextureForEditor
	{
		[SerializeField]
		private Vector2Int _resolution = new Vector2Int(256, 1);

		[SerializeField]
		[GradientUsage(false)]
		private Gradient _gradient = GetDefaultGradient();

		[SerializeField]
		[HideInInspector]
		private Texture2D _texture;

		[SerializeField]
		private List<string> _hexadecimals;

		private int _width => _resolution.x;

		private int _height => _resolution.y;

		public Texture2D GetTexture()
		{
			return _texture;
		}

		public static implicit operator Texture2D(GradientTexture asset)
		{
			return asset.GetTexture();
		}

		private static Gradient GetDefaultGradient()
		{
			Gradient gradient = new Gradient();
			gradient.alphaKeys = new GradientAlphaKey[1]
			{
				new GradientAlphaKey(1f, 1f)
			};
			gradient.colorKeys = new GradientColorKey[2]
			{
				new GradientColorKey(Color.black, 0f),
				new GradientColorKey(Color.white, 1f)
			};
			return gradient;
		}

		public void FillGradientWithHexadecimals()
		{
			if (_hexadecimals.Count < 1)
			{
				return;
			}
			int num = 0;
			float num2 = 0f;
			float num3 = 1f / (float)(_hexadecimals.Count - 1);
			GradientColorKey[] array = new GradientColorKey[_hexadecimals.Count];
			GradientAlphaKey[] array2 = new GradientAlphaKey[_hexadecimals.Count];
			foreach (string hexadecimal in _hexadecimals)
			{
				string text = hexadecimal;
				if (text[0] != '#')
				{
					text = "#" + text;
				}
				ColorUtility.TryParseHtmlString(text, out var color);
				array[num] = new GradientColorKey(color, num2);
				array2[num] = new GradientAlphaKey(1f, num2);
				num2 += num3;
				num++;
			}
			_gradient.SetKeys(array, array2);
			ValidateTextureValues();
		}

		public void InvertGradient()
		{
			GradientColorKey[] colorKeys = _gradient.colorKeys;
			GradientColorKey[] array = new GradientColorKey[colorKeys.Length];
			for (int i = 0; i < colorKeys.Length; i++)
			{
				array[i].color = colorKeys[colorKeys.Length - 1 - i].color;
				array[i].time = 1f - colorKeys[colorKeys.Length - 1 - i].time;
			}
			GradientAlphaKey[] alphaKeys = _gradient.alphaKeys;
			GradientAlphaKey[] array2 = new GradientAlphaKey[alphaKeys.Length];
			for (int j = 0; j < alphaKeys.Length; j++)
			{
				array2[j].alpha = alphaKeys[alphaKeys.Length - 1 - j].alpha;
				array2[j].time = 1f - alphaKeys[alphaKeys.Length - 1 - j].time;
			}
			_gradient.SetKeys(array, array2);
			ValidateTextureValues();
		}

		public void FillColors()
		{
			_texture.wrapMode = TextureWrapMode.Clamp;
			for (int i = 0; i < _height; i++)
			{
				for (int j = 0; j < _width; j++)
				{
					float time = (float)j / (float)_width;
					Color color = _gradient.Evaluate(time);
					_texture.SetPixel(j, i, color);
				}
			}
			_texture.Apply();
		}

		public bool Equals(Texture2D other)
		{
			return _texture.Equals(other);
		}

		private void OnValidate()
		{
			ValidateTextureValues();
		}

		void IGradientTextureForEditor.LoadExisitingTexture()
		{
		}

		private string GetName()
		{
			return "Preview_" + base.name;
		}

		void IGradientTextureForEditor.CreateTexture()
		{
		}

		private void ValidateTextureValues()
		{
			if (!_texture)
			{
				return;
			}
			if (_texture.name != GetName())
			{
				_texture.name = GetName();
				return;
			}
			if (_texture.width != _resolution.x || _texture.height != _resolution.y)
			{
				_texture.Reinitialize(_resolution.x, _resolution.y);
			}
			FillColors();
			SetDirtyTexture();
		}

		private void SetDirtyTexture()
		{
		}

		public void OnAfterDeserialize()
		{
		}

		public void OnBeforeSerialize()
		{
		}
	}
}
