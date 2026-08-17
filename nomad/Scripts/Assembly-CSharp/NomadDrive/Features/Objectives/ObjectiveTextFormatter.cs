using TMPro;
using UnityEngine;

namespace NomadDrive.Features.Objectives
{
	public static class ObjectiveTextFormatter
	{
		public static string Format(string raw, ObjectiveTextStyle style)
		{
			if (string.IsNullOrEmpty(raw))
			{
				return raw;
			}
			string text = raw;
			if (style.Bold)
			{
				text = "<b>" + text + "</b>";
			}
			if (style.Italic)
			{
				text = "<i>" + text + "</i>";
			}
			if (style.UseCustomColor)
			{
				text = "<color=#" + ColorUtility.ToHtmlStringRGB(style.Color) + ">" + text + "</color>";
			}
			return text;
		}

		public static void Apply(TMP_Text field, string raw, ObjectiveTextStyle style)
		{
			if (!(field == null))
			{
				field.text = Format(raw, style);
				if (style.FontSizeOverride > 0)
				{
					field.fontSize = style.FontSizeOverride;
				}
			}
		}
	}
}
