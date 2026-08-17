using System;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.FTools
{
	public abstract class FContainerBase : ScriptableObject
	{
		[Serializable]
		public class AssetReference
		{
			public UnityEngine.Object Reference;

			public string OriginalExtension = "";
		}

		public List<AssetReference> ContainedAssets = new List<AssetReference>();

		public virtual bool Contains(UnityEngine.Object obj)
		{
			for (int num = ContainedAssets.Count - 1; num >= 0; num--)
			{
				if (ContainedAssets[num].Reference == null)
				{
					ContainedAssets.RemoveAt(num);
				}
				else if (ContainedAssets[num].Reference == obj)
				{
					return true;
				}
			}
			return false;
		}

		public virtual void Remove(UnityEngine.Object obj)
		{
			for (int num = ContainedAssets.Count - 1; num >= 0; num--)
			{
				if (ContainedAssets[num].Reference == null)
				{
					ContainedAssets.RemoveAt(num);
				}
				else if (ContainedAssets[num].Reference == obj)
				{
					ContainedAssets.RemoveAt(num);
					break;
				}
			}
		}

		public virtual void RemoveAndDestroy(UnityEngine.Object obj)
		{
			if (!(obj == null))
			{
				Remove(obj);
			}
		}

		public virtual void CopyAsset(UnityEngine.Object obj, string extension = ".asset")
		{
			_ = obj == null;
		}

		public virtual void Add(UnityEngine.Object obj)
		{
			if (!(obj == null))
			{
				AssetReference assetReference = new AssetReference();
				assetReference.Reference = obj;
				ContainedAssets.Add(assetReference);
			}
		}

		public AssetReference GetReferenceTo(UnityEngine.Object asset)
		{
			if (asset == null)
			{
				return null;
			}
			for (int num = ContainedAssets.Count - 1; num >= 0; num--)
			{
				if (ContainedAssets[num].Reference == null)
				{
					ContainedAssets.RemoveAt(num);
				}
				else if (ContainedAssets[num].Reference == asset)
				{
					return ContainedAssets[num];
				}
			}
			return null;
		}

		public virtual void AddAsset(UnityEngine.Object obj)
		{
			if (obj == null)
			{
				return;
			}
			if (Contains(obj))
			{
				UnpackSingleAsset(obj);
				return;
			}
			if (!Contains(obj))
			{
				Add(obj);
			}
			AddAssetTo(this, obj);
		}

		public virtual void UnpackSingleAsset(UnityEngine.Object asset)
		{
			if (!(asset == null))
			{
				UnpackSingleAsset(this, asset);
			}
		}

		public virtual void UnpackAll()
		{
			UnpackAll(this);
		}

		public void _SetDirty()
		{
		}

		public static void AddAssetTo(ScriptableObject container, UnityEngine.Object asset)
		{
		}

		public static void UnpackAll(FContainerBase container)
		{
		}

		public static void UnpackSingleAsset(FContainerBase container, UnityEngine.Object tgt)
		{
		}
	}
}
