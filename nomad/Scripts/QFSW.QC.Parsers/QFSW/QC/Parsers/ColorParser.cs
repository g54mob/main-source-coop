using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;

namespace QFSW.QC.Parsers
{
	public class ColorParser : BasicCachedQcParser<Color>
	{
		private readonly Dictionary<string, Color> _colorLookup;

		public ColorParser()
		{
			_colorLookup = new Dictionary<string, Color>();
			PropertyInfo[] properties = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.CanRead && !propertyInfo.CanWrite)
				{
					MethodInfo getMethod = propertyInfo.GetMethod;
					if (getMethod.ReturnType == typeof(Color))
					{
						_colorLookup.Add(propertyInfo.Name, (Color)getMethod.Invoke(null, Array.Empty<object>()));
					}
				}
			}
		}

		public override Color Parse(string value)
		{
			if (_colorLookup.ContainsKey(value.ToLower()))
			{
				return _colorLookup[value.ToLower()];
			}
			try
			{
				if (value.StartsWith("0x"))
				{
					return ParseHexColor(value);
				}
				return ParseRGBAColor(value);
			}
			catch (FormatException ex)
			{
				throw new ParserInputException(ex.Message + "\nThe format must be either of:\n   - R,G,B\n   - R,G,B,A\n   - 0xRRGGBB\n   - 0xRRGGBBAA\n   - A preset color such as 'red'", ex);
			}
		}

		private Color ParseRGBAColor(string value)
		{
			string[] array = value.Split(',');
			Color white = Color.white;
			int i = 0;
			if (array.Length < 3 || array.Length > 4)
			{
				throw new FormatException("Cannot parse '" + value + "' as a Color.");
			}
			try
			{
				for (; i < array.Length; i++)
				{
					white[i] = ParsePart(array[i]);
				}
				return white;
			}
			catch (FormatException)
			{
				throw new FormatException("Cannot parse '" + array[i] + "' as part of a Color, it must be numerical and in the valid range [0,1].");
			}
			static float ParsePart(string part)
			{
				float num = float.Parse(part);
				if (num < 0f || num > 1f)
				{
					throw new FormatException($"{num} falls outside of the valid [0,1] range for a component of a Color.");
				}
				return num;
			}
		}

		private Color ParseHexColor(string value)
		{
			int num = value.Length - 2;
			if (num != 6 && num != 8)
			{
				throw new FormatException("Hex colors must contain either 6 or 8 hex digits.");
			}
			Color white = Color.white;
			int num2 = num / 2;
			int i = 0;
			try
			{
				for (; i < num2; i++)
				{
					white[i] = (float)int.Parse(value.Substring(2 * (1 + i), 2), NumberStyles.HexNumber) / 255f;
				}
				return white;
			}
			catch (FormatException)
			{
				throw new FormatException("Cannot parse '" + value.Substring(2 * (1 + i), 2) + "' as part of a Color as it was invalid hex.");
			}
		}
	}
}
