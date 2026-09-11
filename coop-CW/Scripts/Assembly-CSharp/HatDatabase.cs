using System;
using UnityEngine;

public class HatDatabase : MonoBehaviour
{
	public static HatDatabase instance;

	[TextArea(6, 6)]
	public string WARNING;

	public Hat[] hats;

	public void Awake()
	{
		instance = this;
		MakeLocalizedHatNames();
	}

	private void MakeLocalizedHatNames()
	{
		Hat[] array = hats;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GenerateLocalizedName();
		}
	}

	public static void Init()
	{
		UnityEngine.Object.FindObjectOfType<HatDatabase>().Awake();
	}

	public Hat InstantiateHat(GameObject hatPrefab)
	{
		int indexOfHat = GetIndexOfHat(hatPrefab.GetComponent<Hat>());
		Hat component = UnityEngine.Object.Instantiate(hatPrefab).GetComponent<Hat>();
		component.runtimeHatIndex = indexOfHat;
		return component;
	}

	public Hat GetHatFromIndex(int index)
	{
		return hats[index];
	}

	public int GetIndexOfHat(Hat hatBuySlotHatPrefab)
	{
		for (int i = 0; i < hats.Length; i++)
		{
			if (hats[i] == hatBuySlotHatPrefab)
			{
				return i;
			}
		}
		throw new Exception("hat database is wrong");
	}
}
