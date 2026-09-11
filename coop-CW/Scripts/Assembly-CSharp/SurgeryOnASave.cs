using System;
using System.IO;
using System.Linq;
using UnityEngine;

public static class SurgeryOnASave
{
	public static void PerformSurgery(Save save)
	{
		int saveIndex = save.SerializedSave.SaveIndex;
		FileInfo fileInfo = SaveIO.GetSaveFiles().FirstOrDefault((FileInfo f) => f.Name.Contains(saveIndex.ToString()));
		if (fileInfo != null)
		{
			Save save2 = SaveIO.LoadSave(fileInfo, saveIndex);
			save.SerializedSave = save2.SerializedSave;
			save.SerializedNetworkDeals = Array.Empty<NetworkDealBase.SerializedNetworkDeal>();
			SaveIO.SaveTo(fileInfo.FullName, save);
			Debug.Log("Surgery on Save Complete!");
		}
		else
		{
			Debug.LogError("Save File not found!");
		}
	}
}
