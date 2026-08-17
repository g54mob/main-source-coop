using System;
using UnityEngine;

namespace TheVisualEngine
{
	[Serializable]
	public class TVEElementRendererData
	{
		public TVETextureSize baseTexture = TVETextureSize._512;

		public Transform baseCenter;

		public float baseRadius = 400f;

		[Space(10f)]
		public TVETextureSize nearTexture = TVETextureSize._512;

		public Transform nearCenter;

		public float nearRadius = 40f;

		[Space(10f)]
		[Range(0f, 1f)]
		public float baseToNearBlend = 0.5f;
	}
}
