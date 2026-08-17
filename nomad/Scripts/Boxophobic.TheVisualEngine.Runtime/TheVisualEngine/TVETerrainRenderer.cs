using System;
using Boxophobic.StyledGUI;
using UnityEngine;

namespace TheVisualEngine
{
	[Serializable]
	public class TVETerrainRenderer
	{
		[StyledDisplay("Proxy Mode")]
		public TVETerrainBaking bakeMode = TVETerrainBaking.RuntimeRenderTexture;

		[StyledDisplay("Proxy Texture")]
		public TVETextureSize bakeTexture = TVETextureSize._256;

		[StyledDisplay("Proxy Material")]
		public Material bakeMaterial;
	}
}
