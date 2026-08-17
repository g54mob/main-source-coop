using System;
using UnityEngine;

namespace PaintCore
{
	[Serializable]
	public struct CwHashedTexture
	{
		[NonSerialized]
		private Texture instance;

		[SerializeField]
		private CwHash hash;

		public static implicit operator CwHashedTexture(Texture newInstance)
		{
			CwHashedTexture result = default(CwHashedTexture);
			result.instance = newInstance;
			if (newInstance != null)
			{
				CwSerialization.TextureToHash.TryGetValue(newInstance, out result.hash);
			}
			else
			{
				result.hash = 0;
			}
			return result;
		}

		public static implicit operator Texture(CwHashedTexture hashed)
		{
			hashed.TryGetInstance(out var texture);
			return texture;
		}

		public bool TryGetInstance(out Texture texture)
		{
			if (instance != null)
			{
				texture = instance;
				return true;
			}
			return CwSerialization.HashToTexture.TryGetValue(hash, out texture);
		}
	}
}
