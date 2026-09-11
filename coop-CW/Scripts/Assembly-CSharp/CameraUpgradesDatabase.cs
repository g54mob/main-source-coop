using System.Collections.Generic;
using UnityEngine;
using Zorro.Core;

[CreateAssetMenu(menuName = "Database/CameraUpgradesDatabase", order = 9999, fileName = "CameraUpgradesDatabase")]
public class CameraUpgradesDatabase : ObjectDatabaseAsset<CameraUpgradesDatabase, CameraUpgradeItem>
{
	public Dictionary<CameraUpgradeItem.CameraUpgradeType, List<CameraUpgradeItem>> UpgradeTypeToItemsDic = new Dictionary<CameraUpgradeItem.CameraUpgradeType, List<CameraUpgradeItem>>();

	public static bool TryGetItemFromUpgradeID(byte id, out CameraUpgradeItem item)
	{
		foreach (CameraUpgradeItem @object in SingletonAsset<CameraUpgradesDatabase>.Instance.Objects)
		{
			if (@object.upgradeId == id)
			{
				item = @object;
				return true;
			}
		}
		item = null;
		return false;
	}
}
