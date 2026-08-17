using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheVisualEngine
{
	[Serializable]
	public class TVEGlobalFormData
	{
		[Tooltip("Controls the global height for Conforming.")]
		public float confromHeight;

		[FormerlySerializedAs("sizeFadeIntensity")]
		[Tooltip("Controls the global Size Fade scale.")]
		[Range(0f, 1f)]
		public float sizeFadeValue = 1f;
	}
}
