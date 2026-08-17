using System;
using Boxophobic.StyledGUI;
using UnityEngine;

namespace TheVisualEngine
{
	[Serializable]
	public class TVEGlobalAtmoData
	{
		[Tooltip("Controls the global dryness intensity.")]
		[Range(0f, 1f)]
		public float drynessIntensity;

		[Tooltip("Controls the global overlay intensity.")]
		[Range(0f, 1f)]
		public float overlayIntensity;

		[Tooltip("Controls the global wetness intensity.")]
		[Range(0f, 1f)]
		public float wetnessIntensity;

		[Tooltip("Controls the global rainfall intensity.")]
		[StyledDisplay("Rainfall Intensity")]
		[Range(0f, 1f)]
		public float raindropsIntensity;
	}
}
