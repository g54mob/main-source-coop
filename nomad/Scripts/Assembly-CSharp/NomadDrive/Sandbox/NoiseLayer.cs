using System;
using UnityEngine;

namespace NomadDrive.Sandbox
{
	[Serializable]
	public class NoiseLayer
	{
		[Header("Layer Settings")]
		public bool enabled = true;

		public string layerName = "Noise Layer";

		[Header("Noise Parameters")]
		[Range(0f, 1f)]
		public float amplitude = 0.5f;

		[Range(0.001f, 0.1f)]
		public float frequency = 0.01f;

		[Range(1f, 8f)]
		public int octaves = 4;

		[Range(0f, 1f)]
		public float persistence = 0.5f;

		[Range(1f, 4f)]
		public float lacunarity = 2f;

		[Header("Position")]
		public Vector2 offset = Vector2.zero;

		[Header("Blending")]
		public BlendMode blendMode;
	}
}
