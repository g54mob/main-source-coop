using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class AssetSelectorPopupAttribute : PropertyAttribute
{
	public readonly Type AssetType;

	public readonly string AssetTypeName;

	public readonly bool StorePath;

	public AssetSelectorPopupAttribute(Type assetType, bool storePath = false)
	{
		AssetType = assetType;
		AssetTypeName = ((assetType != null) ? assetType.Name : "");
		StorePath = storePath;
	}

	public AssetSelectorPopupAttribute(string assetTypeName, bool storePath = false)
	{
		AssetType = null;
		AssetTypeName = assetTypeName ?? "";
		StorePath = storePath;
	}
}
