using System;
using UnityEngine;

namespace TheVisualEngine
{
	[Serializable]
	public class TVEGlobalPaintData
	{
		[Tooltip("Controls the global tinting influence.")]
		[Range(0f, 1f)]
		public float tintingIntensity;

		[Tooltip("Controls the global tinting color.")]
		[ColorUsage(false, true)]
		public Color tintingColor = new Color(0.5f, 0.5f, 0.5f, 0f);

		[Tooltip("Controls the global Cutout intensity.")]
		[Range(0f, 1f)]
		public float cutoutIntensity;
	}
}
