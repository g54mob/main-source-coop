using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace PaintCore
{
	[Serializable]
	[MovedFrom(true, "PaintIn3D", "PaintIn3D", "P3dModifyTextureRandom")]
	public class CwModifyTextureRandom : CwModifier
	{
		public static string Group = "Texture";

		public static string Title = "Random";

		[SerializeField]
		private List<Texture> textures;

		public List<Texture> Textures
		{
			get
			{
				if (textures == null)
				{
					textures = new List<Texture>();
				}
				return textures;
			}
		}

		protected override void OnModifyTexture(ref Texture texture, float pressure)
		{
			if (textures != null && textures.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, textures.Count);
				texture = textures[index];
			}
		}
	}
}
