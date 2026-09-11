using System;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Zorro.Core;

[Serializable]
public abstract class NetworkDealBase
{
	public enum DEAL_STATE
	{
		unInited = 0,
		progressing = 1,
		failed = 2,
		success = 3
	}

	[Serializable]
	public class SerializedNetworkDeal
	{
		public int progress;

		public DEAL_STATE state;

		public int dealIndex;

		public int rewardIndex;

		public DIFFICULTY difficulty;

		public NetworkDealBase UnSerialize()
		{
			NetworkDealBase dealFromIndex = SingletonAsset<NetworkDealDataBase>.Instance.GetDealFromIndex(dealIndex);
			DealRewardBase rewardFromIndex = SingletonAsset<DealRewardDatabase>.Instance.GetRewardFromIndex(rewardIndex);
			dealFromIndex.Init(rewardFromIndex, difficulty);
			dealFromIndex.ProgressInt = progress;
			dealFromIndex.State = state;
			return dealFromIndex;
		}
	}

	[HideInInspector]
	public DIFFICULTY difficulty;

	private Sprite iconSprite;

	private int progressInt;

	[HideInInspector]
	public DealRewardBase reward;

	private DEAL_STATE state;

	public const string SuccessEmailTitle = "Great Job!";

	public abstract List<DIFFICULTY> AllowedDifficulties { get; }

	public abstract string DealName { get; }

	public abstract string EmailTitle { get; }

	public string Description_Localized { get; private set; }

	public string EmailTitle_Localized { get; private set; }

	public string DealName_Localized { get; private set; }

	public string SuccessEmailBody_Localized { get; private set; }

	public string FailedEmailBody_Localized { get; private set; }

	public string Difficulty_Very_Easy_Localized { get; private set; }

	public string Difficulty_Easy_Localized { get; private set; }

	public string Difficulty_Medium_Localized { get; private set; }

	public string Difficulty_Hard_Localized { get; private set; }

	public string Difficulty_Very_Hard_Localized { get; private set; }

	public abstract string EmailAddress { get; }

	public abstract List<string> PersistentIdOfItem { get; }

	public bool IsDirty { get; private set; }

	public abstract bool UseInGame { get; }

	public abstract RARITY Rarity { get; }

	public bool Inited => State != DEAL_STATE.unInited;

	public bool IsDone
	{
		get
		{
			DEAL_STATE dEAL_STATE = State;
			return dEAL_STATE == DEAL_STATE.failed || dEAL_STATE == DEAL_STATE.success;
		}
	}

	public bool IsFailed => State == DEAL_STATE.failed;

	public bool IsSuccess => State == DEAL_STATE.success;

	public int ProgressInt
	{
		get
		{
			return progressInt;
		}
		set
		{
			if (progressInt != value)
			{
				progressInt = value;
				SetDirty();
				if (GetProgress() >= 1f)
				{
					State = DEAL_STATE.success;
				}
			}
		}
	}

	public DEAL_STATE State
	{
		get
		{
			return state;
		}
		set
		{
			if (state != value)
			{
				state = value;
				SetDirty();
			}
		}
	}

	public abstract string IconPath { get; }

	private string LocalizedEmailTitle()
	{
		string text = GetType().Name + "_EmailTitle";
		if (!Enum.TryParse<LocalizationKeys.Keys>(text, out var result))
		{
			Debug.LogError("Cant find localized key: " + text);
			return string.Empty;
		}
		return LocalizationKeys.GetLocalizedString(result);
	}

	private string LocalizedDealName()
	{
		string text = GetType().Name + "_DealName";
		if (!Enum.TryParse<LocalizationKeys.Keys>(text, out var result))
		{
			Debug.LogError("Cant find localized key: " + text);
			return string.Empty;
		}
		return LocalizationKeys.GetLocalizedString(result);
	}

	private string LocalizedDescription()
	{
		string text = GetType().Name + "_Description";
		if (!Enum.TryParse<LocalizationKeys.Keys>(text, out var result))
		{
			Debug.LogError("Cant find localized key: " + text);
			return string.Empty;
		}
		return LocalizationKeys.GetLocalizedString(result);
	}

	private string LocalizedSuccessEmailBody()
	{
		string text = GetType().Name + "_SuccessEmailBody";
		if (!Enum.TryParse<LocalizationKeys.Keys>(text, out var result))
		{
			Debug.LogError("Cant find localized key: " + text);
			return string.Empty;
		}
		return LocalizationKeys.GetLocalizedString(result);
	}

	private string LocalizedFailedEmailBody()
	{
		string text = GetType().Name + "_FailedEmailBody";
		if (!Enum.TryParse<LocalizationKeys.Keys>(text, out var result))
		{
			Debug.LogError("Cant find localized key: " + text);
			return string.Empty;
		}
		return LocalizationKeys.GetLocalizedString(result);
	}

	public virtual float GetProgress()
	{
		return (float)ProgressInt / (float)RequiredAmount();
	}

	public abstract int RequiredAmount();

	public void SetDirty()
	{
		IsDirty = true;
	}

	public void ClearDirty()
	{
		IsDirty = false;
	}

	public abstract NetworkDealBase CreateNew();

	public virtual void Init(DealRewardBase reward, DIFFICULTY dif)
	{
		Debug.Log($"Initing Deal! Reward: {reward.GetType().Name}, Difficulty: {dif}");
		this.reward = reward;
		difficulty = dif;
		reward.Init(difficulty);
		State = DEAL_STATE.progressing;
		iconSprite = Resources.Load<Sprite>(IconPath);
		if (!AllowedDifficulties.Contains(dif))
		{
			Debug.LogError($"{this} has the wrong difficulty: {dif}");
		}
		if (iconSprite == null)
		{
			Debug.LogError("Icon not loaded");
		}
		MakeLocalizedStrings();
		Refresh();
	}

	public void MakeLocalizedStrings()
	{
		string text = LocalizedEmailTitle();
		if (!string.IsNullOrEmpty(text))
		{
			EmailTitle_Localized = text;
		}
		else
		{
			EmailTitle_Localized = EmailTitle;
		}
		text = LocalizedDealName();
		if (!string.IsNullOrEmpty(text))
		{
			DealName_Localized = text;
		}
		else
		{
			DealName_Localized = DealName;
		}
		text = LocalizedDescription();
		if (!string.IsNullOrEmpty(text))
		{
			Description_Localized = text;
		}
		text = LocalizedSuccessEmailBody();
		if (!string.IsNullOrEmpty(text))
		{
			SuccessEmailBody_Localized = text;
		}
		text = LocalizedFailedEmailBody();
		if (!string.IsNullOrEmpty(text))
		{
			FailedEmailBody_Localized = text;
		}
		Difficulty_Very_Easy_Localized = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Difficulty_VeryEasy);
		Difficulty_Easy_Localized = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Difficulty_Easy);
		Difficulty_Medium_Localized = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Difficulty_Medium);
		Difficulty_Hard_Localized = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Difficulty_Hard);
		Difficulty_Very_Hard_Localized = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Difficulty_VeryHard);
		reward.MakeLocalized();
	}

	public void SpawnSponsorItem()
	{
		ShoppingCart shoppingCart = new ShoppingCart();
		if (PersistentIdOfItem != null)
		{
			foreach (string item2 in PersistentIdOfItem)
			{
				if (item2.IsNullOrEmpty())
				{
					return;
				}
				Debug.Log("ItemId " + item2);
				if (!ItemDatabase.TryGetItemFromPersistentID(new Guid(item2), out var item))
				{
					Debug.LogError("Couldn't load item from id: " + item2);
					return;
				}
				shoppingCart.Cart.Add(new ShopItem
				{
					Item = item
				});
			}
		}
		if (shoppingCart.Cart != null && shoppingCart.Cart.Count > 0)
		{
			ShopHandler.Instance.BuyItem(shoppingCart);
		}
	}

	public virtual Sprite GetIcon()
	{
		if (iconSprite != null)
		{
			return iconSprite;
		}
		Debug.LogError("Icon not loaded");
		return null;
	}

	public abstract string DealDescription();

	public abstract void Update();

	public void ClaimReward()
	{
		if (State == DEAL_STATE.success)
		{
			reward.ClaimReward();
		}
	}

	public virtual string GetDifficultyText()
	{
		return difficulty switch
		{
			DIFFICULTY.veryEasy => Difficulty_Very_Easy_Localized, 
			DIFFICULTY.easy => Difficulty_Easy_Localized, 
			DIFFICULTY.medium => Difficulty_Medium_Localized, 
			DIFFICULTY.hard => Difficulty_Hard_Localized, 
			DIFFICULTY.veryHard => Difficulty_Very_Hard_Localized, 
			_ => throw new ArgumentOutOfRangeException("difficulty", difficulty, null), 
		};
	}

	public virtual SerializedNetworkDeal GetSerialized()
	{
		SerializedNetworkDeal serializedNetworkDeal = new SerializedNetworkDeal();
		FillSerializedData(serializedNetworkDeal);
		return serializedNetworkDeal;
	}

	protected void FillSerializedData(SerializedNetworkDeal deal)
	{
		deal.progress = progressInt;
		deal.state = State;
		deal.dealIndex = GetIndex();
		deal.rewardIndex = reward.GetIndex();
		deal.difficulty = difficulty;
	}

	public abstract byte GetIndex();

	protected byte GetIndex<T>()
	{
		string[] array = typeof(T).Name.Split('_');
		try
		{
			return byte.Parse(array[1]);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			return byte.MaxValue;
		}
	}

	public float GetWeight()
	{
		return Rarity switch
		{
			RARITY.always => 100f, 
			RARITY.superCommon => 2f, 
			RARITY.moreCommon => 1.5f, 
			RARITY.common => 1f, 
			RARITY.lessCommon => 0.5f, 
			RARITY.uncommon => 0.25f, 
			RARITY.rare => 0.1f, 
			RARITY.epic => 0.01f, 
			RARITY.legendary => 0.001f, 
			RARITY.mythic => 0.001f, 
			_ => throw new ArgumentOutOfRangeException("Rarity", Rarity, null), 
		};
	}

	public abstract void OnDestroy();

	public virtual void ReviewUploadContent(ContentBuffer contentBuffer)
	{
	}

	public virtual void OnStatusUpdated()
	{
	}

	public virtual void OnAddQuota(int quotaToAdd)
	{
	}

	public virtual void OnRemovedMoney(int removedAmount)
	{
	}

	public abstract string GetSuccessEmailBody();

	public void Refresh()
	{
		OnStatusUpdated();
		OnAddQuota(0);
	}

	public virtual string GetProgressText()
	{
		return ProgressInt + "/" + RequiredAmount();
	}

	public abstract string GetFailedEmailBody();
}
