using System;
using UnityEngine;

namespace TheVisualEngine
{
	[Serializable]
	public class TVEGlobalCoatData
	{
		[Tooltip("Controls the global Layer intensity.")]
		[Range(0f, 1f)]
		public float layerIntensity = 1f;

		[Tooltip("Controls the global Detail intensity.")]
		[Range(0f, 1f)]
		public float detailIntensity = 1f;
	}
}
