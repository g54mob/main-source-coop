using System;
using UnityEngine;

namespace TheVisualEngine
{
	[Serializable]
	public class TVEGlobalGlowData
	{
		[Tooltip("Controls the global emissive intensity.")]
		[Range(0f, 1f)]
		public float emissiveIntensity = 1f;

		[Tooltip("Controls the global emissive color.")]
		[ColorUsage(false, true)]
		public Color emissiveColor = new Color(1f, 1f, 1f, 0f);

		[Tooltip("Controls the global subsurface intensity.")]
		[Range(0f, 1f)]
		public float subsurfaceIntensity = 1f;
	}
}
