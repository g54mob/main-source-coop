using System;
using UnityEngine;
using WebSocketSharp;

public class Hat : MonoBehaviour
{
	public string displayName;

	public RARITY rarity;

	public int priceToday;

	public int runtimeHatIndex;

	public bool giveContentEvent;

	public string[] comments = new string[1] { "" };

	public int contentValue = 1;

	private string m_LocalizedName;

	private void Awake()
	{
		GenerateLocalizedName();
	}

	public void GenerateLocalizedName()
	{
		_ = string.Empty;
		string empty = string.Empty;
		string text = base.gameObject.name.Trim().Replace(" ", "").Replace("(Clone)", "");
		empty = text.Substring(text.IndexOf("_") + 1) + "Hat";
		if (Enum.TryParse<LocalizationKeys.Keys>(empty, out var result))
		{
			m_LocalizedName = LocalizationKeys.GetLocalizedString(result);
		}
		else
		{
			Debug.LogWarning("Cant find locKey for: " + empty);
		}
	}

	public string GetName()
	{
		string text = ((!m_LocalizedName.IsNullOrEmpty()) ? m_LocalizedName : displayName);
		if (!(text != ""))
		{
			return base.name;
		}
		return text;
	}

	public int GetBasePrice()
	{
		return rarity switch
		{
			RARITY.common => 300, 
			RARITY.uncommon => 400, 
			RARITY.rare => 500, 
			RARITY.epic => 750, 
			RARITY.legendary => 1500, 
			RARITY.mythic => 3000, 
			_ => throw new ArgumentOutOfRangeException("rarity", rarity, null), 
		};
	}
}
