using System;
using UnityEngine;

namespace HeathenEngineering.Serializable
{
	[Serializable]
	[Obsolete("Use Unity's Color")]
	public class SerializableColor : SerializableVector4
	{
		public float r
		{
			get
			{
				return x;
			}
			set
			{
				x = value;
			}
		}

		public float g
		{
			get
			{
				return y;
			}
			set
			{
				y = value;
			}
		}

		public float b
		{
			get
			{
				return z;
			}
			set
			{
				z = value;
			}
		}

		public float a
		{
			get
			{
				return w;
			}
			set
			{
				w = value;
			}
		}

		public SerializableColor()
		{
			r = 1f;
			g = 1f;
			b = 1f;
			a = 1f;
		}

		public SerializableColor(Color color)
		{
			x = color.r;
			y = color.g;
			z = color.b;
			w = color.a;
		}

		public SerializableVector4 ToHSVA()
		{
			RGBToHSV(this, out var h, out var s, out var v);
			return new SerializableVector4
			{
				x = h,
				y = s,
				z = v,
				w = a
			};
		}

		public SerializableVector3 ToHSV()
		{
			RGBToHSV(this, out var h, out var s, out var v);
			return new SerializableVector3
			{
				x = h,
				y = s,
				z = v
			};
		}

		public static implicit operator Color(SerializableColor value)
		{
			return new Color(value.x, value.y, value.z, value.w);
		}

		public static implicit operator SerializableColor(Color value)
		{
			return new SerializableColor
			{
				x = value.r,
				y = value.g,
				z = value.b,
				w = value.a
			};
		}

		public static void RGBToHSV(Color color, out float h, out float s, out float v)
		{
			Color.RGBToHSV(color, out h, out s, out v);
		}

		public static SerializableColor HSVtoRGB(float h, float s, float v, float a = 1f, bool hdr = false)
		{
			SerializableColor serializableColor = Color.HSVToRGB(h, s, v, hdr);
			if (hdr)
			{
				return serializableColor;
			}
			serializableColor.w = a;
			return serializableColor;
		}
	}
}
