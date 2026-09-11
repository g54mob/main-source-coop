using System.Collections.Generic;
using UnityEngine;

public class CameraUpgradeTable
{
	private List<byte> m_Upgrades;

	public CameraUpgradeTable()
	{
		m_Upgrades = new List<byte>();
	}

	public CameraUpgradeTable(byte[] upgradeData)
	{
		m_Upgrades = new List<byte>(upgradeData);
		CameraUpgradeItem item = null;
		foreach (byte upgrade in m_Upgrades)
		{
			if (CameraUpgradesDatabase.TryGetItemFromUpgradeID(upgrade, out item))
			{
				Debug.Log("Loaded Camera, Has Upgrade: " + item.UpgradeType.ToString() + " : " + item.name);
			}
		}
	}

	public void AddUpgrade(byte upgradeID)
	{
		Debug.Log("Adding upgrade: " + upgradeID);
		m_Upgrades.Add(upgradeID);
	}

	public byte[] GetUpgradeData()
	{
		return m_Upgrades.ToArray();
	}

	public void ClearUpgrades()
	{
		Debug.Log("Clearing Camera Upgrades: ");
		m_Upgrades = new List<byte>();
	}

	public bool HaveUpgrade(byte cameraUpgradeID)
	{
		return m_Upgrades.Contains(cameraUpgradeID);
	}
}
