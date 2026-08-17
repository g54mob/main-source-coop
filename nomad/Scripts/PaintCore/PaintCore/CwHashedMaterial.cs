using System;
using UnityEngine;

namespace PaintCore
{
	[Serializable]
	public struct CwHashedMaterial
	{
		[NonSerialized]
		private Material instance;

		[SerializeField]
		private int hash;

		public CwHashedMaterial(Material newInstance, int newHash)
		{
			instance = newInstance;
			hash = newHash;
		}

		public bool TryGetInstance(out Material model)
		{
			if (instance != null)
			{
				model = instance;
				return true;
			}
			return CwSerialization.HashToMaterial.TryGetValue(hash, out model);
		}
	}
}
