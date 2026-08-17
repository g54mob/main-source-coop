using System;
using UnityEngine;

namespace PaintCore
{
	[Serializable]
	public struct CwSlot
	{
		public int Index;

		public string Name;

		public CwSlot(int newIndex, string newName)
		{
			Index = newIndex;
			Name = newName;
		}

		public Texture FindTexture(GameObject gameObject)
		{
			if (gameObject != null)
			{
				CwModel componentInParent = gameObject.GetComponentInParent<CwModel>();
				if (componentInParent != null)
				{
					Material material = CwCommon.GetMaterial(componentInParent.CachedRenderer, Index);
					if (material != null && material.HasProperty(Name))
					{
						return material.GetTexture(Name);
					}
				}
			}
			return null;
		}

		public bool IsTransformed(GameObject gameObject)
		{
			if (gameObject != null)
			{
				CwModel componentInParent = gameObject.GetComponentInParent<CwModel>();
				if (componentInParent != null)
				{
					Material material = CwCommon.GetMaterial(componentInParent.CachedRenderer, Index);
					if (material != null && (material.GetTextureScale(Name) != Vector2.one || material.GetTextureOffset(Name) != Vector2.zero))
					{
						return true;
					}
				}
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return Index.GetHashCode() ^ Name.GetHashCode();
		}

		public static bool operator ==(CwSlot a, CwSlot b)
		{
			if (a.Index == b.Index)
			{
				return a.Name == b.Name;
			}
			return false;
		}

		public static bool operator !=(CwSlot a, CwSlot b)
		{
			if (a.Index == b.Index)
			{
				return a.Name != b.Name;
			}
			return true;
		}
	}
}
