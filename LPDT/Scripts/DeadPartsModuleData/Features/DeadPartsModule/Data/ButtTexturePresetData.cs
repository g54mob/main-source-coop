using System;
using UnityEngine;

namespace Features.DeadPartsModule.Data
{
	[Serializable]
	public class ButtTexturePresetData
	{
		public Texture BaseTexture;

		public Texture NormalMap;

		public Texture Mask;

		[Range(0f, 1f)]
		public float Metallic;
	}
}
