using System;
using UnityEngine;

namespace NomadDrive.Features.LiquidTransferSystem
{
	[Serializable]
	public class LiquidVisualPreset
	{
		public Color shallowColor = Color.white;

		public Color deepColor = Color.gray;

		[Tooltip("Base opacity at zero depth. Clear liquids (water) low, opaque liquids (milk, coffee) high.")]
		[Range(0f, 1f)]
		public float opacity = 0.5f;

		[Tooltip("Beer-Lambert absorption coefficient in 1/m: how fast the color shifts to deepColor with liquid depth.")]
		[Min(0f)]
		public float absorption = 2f;

		[Range(0f, 1f)]
		public float smoothness = 0.85f;

		[Range(0f, 4f)]
		public float specIntensity = 1f;

		[Header("Surface foam / crema / bubbles")]
		[Tooltip("Color of the surface foam, crema and bubbles for this liquid (coffee crema tan, milk/water white).")]
		public Color foamColor = Color.white;

		[Tooltip("Fraction of the top surface covered by matte foam/crema. Coffee high, water 0.")]
		[Range(0f, 1f)]
		public float foamCoverage;

		[Tooltip("Density of drifting, popping surface bubbles.")]
		[Range(0f, 1f)]
		public float bubbleAmount = 0.12f;
	}
}
