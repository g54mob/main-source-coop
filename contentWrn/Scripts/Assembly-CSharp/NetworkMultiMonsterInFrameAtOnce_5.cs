using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkMultiMonsterInFrameAtOnce_5 : NetworkDealBase
{
	public override List<DIFFICULTY> AllowedDifficulties => new List<DIFFICULTY>
	{
		DIFFICULTY.easy,
		DIFFICULTY.hard
	};

	public override string DealName => "MultiMonster Net";

	public override string EmailTitle => "Sending this email to 1.000.000 spööktubers!";

	public override string EmailAddress => "Timmy@SirMonsterBurger.nw";

	public override RARITY Rarity => RARITY.common;

	public override bool UseInGame => true;

	public override string IconPath => "Crime-Wanted--Streamline-Ultimate";

	public override List<string> PersistentIdOfItem => new List<string> { "e01a0559-0532-46b2-9df0-5903dbe4a19e" };

	public override byte GetIndex()
	{
		return GetIndex<NetworkMultiMonsterInFrameAtOnce_5>();
	}

	public override string DealDescription()
	{
		string text = BigNumbers.ViewsToString(RequiredAmount());
		string description_Localized = base.Description_Localized;
		if (string.IsNullOrEmpty(description_Localized))
		{
			return "upload video of " + text + " different monsters in the same shot to show how irresistible our burgers are!";
		}
		return description_Localized.Replace("{Views}", text);
	}

	public override void OnDestroy()
	{
	}

	public override NetworkDealBase CreateNew()
	{
		return new NetworkMultiMonsterInFrameAtOnce_5();
	}

	public override void ReviewUploadContent(ContentBuffer contentBuffer)
	{
		base.ReviewUploadContent(contentBuffer);
		int requiredAmount = RequiredAmount();
		List<MultiMonsterEvent> list = (from MultiMonsterEvent e in from content in contentBuffer.buffer
				select content.frame.contentEvent into e
				where e is MultiMonsterEvent
				select e
			where e.monsterCount >= requiredAmount
			select e).ToList();
		if (list.Count > 0)
		{
			base.ProgressInt += requiredAmount;
		}
		foreach (MultiMonsterEvent item in list)
		{
			Debug.Log(item.GetName());
		}
	}

	public override string GetSuccessEmailBody()
	{
		string successEmailBody_Localized = base.SuccessEmailBody_Localized;
		if (!string.IsNullOrEmpty(successEmailBody_Localized))
		{
			return successEmailBody_Localized;
		}
		return "Thank you for capturing the monsters in the same shot!";
	}

	public override string GetFailedEmailBody()
	{
		return "";
	}

	public override int RequiredAmount()
	{
		int num = 0;
		return difficulty switch
		{
			DIFFICULTY.easy => 2, 
			DIFFICULTY.hard => 3, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
	}

	public override void Update()
	{
	}
}
