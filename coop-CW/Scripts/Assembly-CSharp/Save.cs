using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zorro.Core.Serizalization;

[Serializable]
public class Save
{
	public SerializedSave SerializedSave = new SerializedSave();

	public NetworkDealBase.SerializedNetworkDeal[] SerializedNetworkDeals = Array.Empty<NetworkDealBase.SerializedNetworkDeal>();

	public bool Valid => GetSaveID() != Guid.Empty;

	public DateTime Date { get; set; }

	public Guid GetSaveID()
	{
		if (!Guid.TryParse(SerializedSave.SaveID, out var result))
		{
			return Guid.Empty;
		}
		return result;
	}

	public Save(int saveIndex)
	{
		Debug.Log($"Creating new save with index: {saveIndex}");
		SerializedSave = new SerializedSave();
		SerializedSave.SaveIndex = saveIndex;
		SerializedSave.SaveID = Guid.NewGuid().ToString();
		SerializedSave.InventoryItems = Array.Empty<SavedInventoryItem>();
	}

	public string Serialize()
	{
		List<object> list = new List<object>();
		list.Add(SerializedSave);
		NetworkDealBase.SerializedNetworkDeal[] serializedNetworkDeals = SerializedNetworkDeals;
		foreach (NetworkDealBase.SerializedNetworkDeal item in serializedNetworkDeals)
		{
			list.Add(item);
		}
		return FlatJsonSerializer.Serialize(list.ToArray());
	}

	public void Deserialize(byte[] bytes)
	{
		string data = Encoding.UTF8.GetString(bytes);
		Deserialize(data);
	}

	public void Deserialize(string data)
	{
		FlatJsonSerializer.DeserializeResult deserializeResult = FlatJsonSerializer.Deserialize(data);
		if (!deserializeResult.TryGetObject<SerializedSave>(out SerializedSave))
		{
			Debug.LogError("Failed to deserialize save data!");
		}
		else if (string.IsNullOrEmpty(SerializedSave.SaveID))
		{
			SerializedSave.SaveID = Guid.NewGuid().ToString();
			Debug.Log("Save ID was null, generated new ID: " + SerializedSave.SaveID);
		}
		if (deserializeResult.TryGetObjects(out IEnumerable<NetworkDealBase.SerializedNetworkDeal> objects))
		{
			SerializedNetworkDeals = objects.ToArray();
			NetworkDealBase.SerializedNetworkDeal[] serializedNetworkDeals = SerializedNetworkDeals;
			foreach (NetworkDealBase.SerializedNetworkDeal serializedNetworkDeal in serializedNetworkDeals)
			{
				Debug.Log("Loaded Network Deal: " + serializedNetworkDeal.GetType());
			}
		}
	}
}
