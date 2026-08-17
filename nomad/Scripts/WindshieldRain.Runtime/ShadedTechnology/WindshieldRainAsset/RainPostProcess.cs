using System;
using UnityEngine;

namespace ShadedTechnology.WindshieldRainAsset
{
	[Serializable]
	public class RainPostProcess
	{
		[SerializeField]
		public Material material;

		[SerializeField]
		[HideInInspector]
		public RenderTexture renderTexture;

		[SerializeField]
		public MaterialTexture[] texturesToSet;
	}
}
