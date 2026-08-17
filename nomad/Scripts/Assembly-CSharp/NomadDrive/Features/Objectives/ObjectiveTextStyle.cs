using System;
using UnityEngine;

namespace NomadDrive.Features.Objectives
{
	[Serializable]
	public struct ObjectiveTextStyle
	{
		public bool Bold;

		public bool Italic;

		public bool UseCustomColor;

		public Color Color;

		[Tooltip("0 = leave TMP default. Otherwise overrides TMP_Text.fontSize for this label.")]
		[Range(0f, 96f)]
		public int FontSizeOverride;
	}
}
