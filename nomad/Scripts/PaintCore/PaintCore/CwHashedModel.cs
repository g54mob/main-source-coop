using System;
using UnityEngine;

namespace PaintCore
{
	[Serializable]
	public struct CwHashedModel
	{
		[NonSerialized]
		private CwModel instance;

		[SerializeField]
		private CwHash hash;

		public static implicit operator CwHashedModel(CwModel newInstance)
		{
			CwHashedModel result = default(CwHashedModel);
			result.instance = newInstance;
			CwSerialization.ModelToHash.TryGetValue(newInstance, out result.hash);
			return result;
		}

		public bool TryGetInstance(out CwModel model)
		{
			if (instance != null)
			{
				model = instance;
				return true;
			}
			if (CwSerialization.HashToModel.TryGetValue(hash, out model) && model != null)
			{
				instance = model;
				return true;
			}
			return false;
		}
	}
}
