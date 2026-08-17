using System;
using CW.Common;
using UnityEngine;

namespace PaintCore
{
	[Serializable]
	public struct CwBlendMode
	{
		public const int ALPHA_BLEND = 0;

		public const int ALPHA_BLEND_INVERSE = 1;

		public const int PREMULTIPLIED = 2;

		public const int ADDITIVE = 3;

		public const int ADDITIVE_SOFT = 4;

		public const int SUBTRACTIVE = 5;

		public const int SUBTRACTIVE_SOFT = 6;

		public const int REPLACE = 7;

		public const int REPLACE_ORIGINAL = 8;

		public const int REPLACE_CUSTOM = 9;

		public const int MULTIPLY_INVERSE_RGB = 10;

		public const int BLUR = 11;

		public const int NORMAL_BLEND = 12;

		public const int NORMAL_REPLACE = 13;

		public const int FLOW = 14;

		public const int NORMAL_REPLACE_ORIGINAL = 15;

		public const int NORMAL_REPLACE_CUSTOM = 16;

		public const int MIN = 17;

		public const int MAX = 18;

		public const int COUNT = 19;

		public static readonly string[] NAMES = new string[19]
		{
			"Alpha Blend", "Alpha Blend Inverse", "Premultiplied", "Additive", "Additive Soft", "Subtractive", "Subtractive Soft", "Replace", "Replace Original", "Replace Custom",
			"Multiply RGB Inverse", "Blur", "Normal Blend", "Normal Replace", "Flow", "Normal Replace Original", "Normal Replace Custom", "Min", "Max"
		};

		public int Index;

		public Color Color;

		public Texture Texture;

		public float Kernel;

		public Vector4 Channels;

		private static int _Channels = Shader.PropertyToID("_Channels");

		private static int _ReplaceColor = Shader.PropertyToID("_ReplaceColor");

		private static int _ReplaceTexture = Shader.PropertyToID("_ReplaceTexture");

		private static int _ReplaceTextureSize = Shader.PropertyToID("_ReplaceTextureSize");

		private static int _Kernel = Shader.PropertyToID("_Kernel");

		public static CwBlendMode AlphaBlend(Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 0,
				Color = Color.white,
				Kernel = 1f,
				Channels = channels
			};
		}

		public static CwBlendMode AlphaBlendInverse(Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 1,
				Color = Color.white,
				Kernel = 1f,
				Channels = channels
			};
		}

		public static CwBlendMode Premultiplied(Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 2,
				Color = Color.white,
				Kernel = 1f,
				Channels = channels
			};
		}

		public static CwBlendMode Additive(Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 3,
				Color = Color.white,
				Kernel = 1f,
				Channels = channels
			};
		}

		public static CwBlendMode AdditiveSoft(Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 4,
				Color = Color.white,
				Kernel = 1f,
				Channels = channels
			};
		}

		public static CwBlendMode Subtractive(Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 5,
				Color = Color.white,
				Kernel = 1f,
				Channels = channels
			};
		}

		public static CwBlendMode SubtractiveSoft(Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 6,
				Color = Color.white,
				Kernel = 1f,
				Channels = channels
			};
		}

		public static CwBlendMode Replace(Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 7,
				Color = Color.white,
				Kernel = 1f,
				Channels = channels
			};
		}

		public static CwBlendMode ReplaceOriginal(Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 8,
				Color = Color.white,
				Kernel = 1f,
				Channels = channels
			};
		}

		public static CwBlendMode ReplaceCustom(Color color, Texture texture, Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 9,
				Color = color,
				Texture = texture,
				Kernel = 1f,
				Channels = channels
			};
		}

		public static CwBlendMode MultiplyInverseRGB(Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 10,
				Color = Color.white,
				Kernel = 1f,
				Channels = channels
			};
		}

		public static CwBlendMode Blur(float kernel, Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 11,
				Color = Color.white,
				Kernel = kernel,
				Channels = channels
			};
		}

		public static CwBlendMode NormalBlend(Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 12,
				Color = Color.white,
				Kernel = 1f,
				Channels = channels
			};
		}

		public static CwBlendMode NormalReplace(Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 13,
				Color = Color.white,
				Kernel = 1f,
				Channels = channels
			};
		}

		public static CwBlendMode Flow(float kernel, Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 14,
				Color = Color.gray,
				Kernel = kernel,
				Channels = channels
			};
		}

		public static CwBlendMode NormalReplaceOriginal(Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 15,
				Color = Color.white,
				Kernel = 1f,
				Channels = channels
			};
		}

		public static CwBlendMode NormalReplaceCustom(Color color, Texture texture, Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 16,
				Color = color,
				Texture = texture,
				Kernel = 1f,
				Channels = channels
			};
		}

		public static CwBlendMode Min(Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 17,
				Color = Color.white,
				Kernel = 1f,
				Channels = channels
			};
		}

		public static CwBlendMode Max(Vector4 channels)
		{
			return new CwBlendMode
			{
				Index = 18,
				Color = Color.white,
				Kernel = 1f,
				Channels = channels
			};
		}

		public void Apply(Material material)
		{
			material.SetVector(_Channels, Channels);
			if (Index == 8 || Index == 9 || Index == 15 || Index == 16)
			{
				material.SetColor(_ReplaceColor, CwHelper.ToLinear(Color));
				material.SetTexture(_ReplaceTexture, Texture);
				if (Texture != null)
				{
					material.SetVector(_ReplaceTextureSize, new Vector2(Texture.width, Texture.height));
				}
			}
			else if (Index == 11 || Index == 14)
			{
				material.SetFloat(_Kernel, Kernel);
			}
		}

		public static string GetName(int index)
		{
			if (index >= 0 && index < 19)
			{
				return NAMES[index];
			}
			return null;
		}

		public static bool operator ==(CwBlendMode a, int b)
		{
			return a.Index == b;
		}

		public static bool operator !=(CwBlendMode a, int b)
		{
			return a.Index != b;
		}

		public static implicit operator int(CwBlendMode a)
		{
			return a.Index;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
	}
}
