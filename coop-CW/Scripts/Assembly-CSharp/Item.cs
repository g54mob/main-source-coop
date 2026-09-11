using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zorro.ControllerSupport;
using Zorro.Core;
using Zorro.Core.CLI;

[CreateAssetMenu(fileName = "Item", menuName = "W/Item", order = -1)]
public class Item : ScriptableObject
{
	public enum ItemType
	{
		Camera = 0,
		Tool = 1,
		Artifact = 2,
		Disc = 3
	}

	[FormerlySerializedAs("shopName")]
	public string displayName;

	public Sprite icon;

	public GameObject itemObject;

	public ItemType itemType = ItemType.Tool;

	[FormerlySerializedAs("toolBudget")]
	public int toolBudgetCost = 1;

	public bool spawnable;

	public RARITY toolSpawnRarity = RARITY.common;

	public int budgetCost;

	public float rarity = 1f;

	public PropContent content;

	public float groundSizeMultiplier = 1f;

	public float groundMassMultiplier = 1f;

	public float mass = 3f;

	public Vector3 holdPos;

	public bool useAlternativeHoldingPos;

	public Vector3 alternativeHoldPos;

	public Vector3 holdRotation;

	public bool useAlternativeHoldingRot;

	public Vector3 alternativeHoldRot;

	public byte id;

	public string persistentID;

	public bool purchasable;

	public ShopItemCategory Category;

	public int price;

	public int quantity;

	public Emote emoteInfo;

	public List<ItemKeyTooltip> Tooltips = new List<ItemKeyTooltip>();

	public float ToolRarity => toolSpawnRarity switch
	{
		RARITY.always => 1000f, 
		RARITY.superCommon => 2f, 
		RARITY.moreCommon => 1.25f, 
		RARITY.common => 1f, 
		RARITY.lessCommon => 0.75f, 
		RARITY.uncommon => 0.5f, 
		RARITY.rare => 0.1f, 
		RARITY.epic => 0.05f, 
		RARITY.legendary => 0.025f, 
		RARITY.mythic => 0.01f, 
		_ => throw new ArgumentOutOfRangeException(), 
	};

	public Guid PersistentID
	{
		get
		{
			if (Guid.TryParse(persistentID, out var result))
			{
				return result;
			}
			return Guid.Empty;
		}
		set
		{
			persistentID = value.ToString();
		}
	}

	private bool ShowEmoteInfo()
	{
		if (Category != ShopItemCategory.Emotes)
		{
			return Category == ShopItemCategory.Emotes2;
		}
		return true;
	}

	[ConsoleCommand]
	public static void SpawnItem(Item item)
	{
		Debug.Log("Spawn item: " + item.name);
		Vector3 debugItemSpawnPos = MainCamera.instance.GetDebugItemSpawnPos();
		Player.localPlayer.RequestCreatePickup(item, new ItemInstanceData(Guid.NewGuid()), debugItemSpawnPos, Quaternion.identity);
	}

	[ConsoleCommand]
	public static void EquipItem(Item item)
	{
		Debug.Log("Equip item: " + item.name);
		Player.localPlayer.TryGetInventory(out var o);
		o.TryAddItem(new ItemDescriptor(item, new ItemInstanceData(Guid.NewGuid())));
	}

	public IEnumerable<IHaveUIData> GetTootipData()
	{
		if (Tooltips.Count > 0)
		{
			string text = base.name.Trim().Replace(" ", "") + "_ToolTips";
			if (Enum.TryParse<LocalizationKeys.Keys>(text, out var result))
			{
				string[] array = LocalizationKeys.GetLocalizedString(result).Split(';');
				Tooltips = new List<ItemKeyTooltip>();
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string input = ParseTip(array2[i], out var provider, out var glpyhType);
					input = StringUtility.EnsureSpaceAfterPhrase(input, "{key}");
					Tooltips.Add(new ItemKeyTooltip(input, provider, glpyhType));
				}
			}
			else
			{
				Debug.LogError("Could not find LOC_Key: " + text);
			}
		}
		return Tooltips;
		static string ParseTip(string t, out IMKbPromptProvider reference, out List<ControllerGlyphs.GlyphType> reference2)
		{
			if (t.Contains("{key_use}"))
			{
				reference = new HardcodedPrompt(ControllerGlyphs.GetSprite(0));
				reference2 = new List<ControllerGlyphs.GlyphType> { ControllerGlyphs.GlyphType.UseItem };
				return t.Replace("{key_use}", "{key}");
			}
			if (t.Contains("{key_use2}"))
			{
				reference = new HardcodedPrompt(ControllerGlyphs.GetSprite(1));
				reference2 = new List<ControllerGlyphs.GlyphType> { ControllerGlyphs.GlyphType.SecondaryUseItem };
				return t.Replace("{key_use2}", "{key}");
			}
			if (t.Contains("{key_selfie}"))
			{
				if (InputHandler.GetCurrentUsedInputScheme() == InputScheme.Gamepad)
				{
					t = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.HoldSelfieMode);
				}
				reference = GameHandler.Instance.SettingsHandler.GetSetting<ToggleSelfieModeKeybindSetting>();
				reference2 = new List<ControllerGlyphs.GlyphType> { ControllerGlyphs.GlyphType.SelfieMode };
				return t.Replace("{key_selfie}", "{key}");
			}
			if (t.Contains("{key_talk}"))
			{
				reference = GameHandler.Instance.SettingsHandler.GetSetting<PushToTalkButtonSetting>();
				reference2 = new List<ControllerGlyphs.GlyphType> { ControllerGlyphs.GlyphType.PushToTalk };
				return t.Replace("{key_talk}", "{key}");
			}
			if (t.Contains("{key_zoom}"))
			{
				reference = new HardcodedPrompt(ControllerGlyphs.GetSprite(2));
				reference2 = new List<ControllerGlyphs.GlyphType>
				{
					ControllerGlyphs.GlyphType.ZoomOut,
					ControllerGlyphs.GlyphType.ZoomIn
				};
				return t.Replace("{key_zoom}", "{key}");
			}
			reference = null;
			reference2 = null;
			return t;
		}
	}

	public string GetLocalizedDisplayName()
	{
		string text = base.name.Trim().Replace(" ", "");
		if (Enum.TryParse<LocalizationKeys.Keys>(text, out var result))
		{
			return LocalizationKeys.GetLocalizedString(result);
		}
		Debug.LogError("Failed to get Localized displayName for: " + text);
		return displayName;
	}
}
