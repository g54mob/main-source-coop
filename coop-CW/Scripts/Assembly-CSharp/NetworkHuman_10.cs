using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkHuman_10 : NetworkDealBase
{
	public override List<DIFFICULTY> AllowedDifficulties => new List<DIFFICULTY> { DIFFICULTY.medium };

	public override string DealName => "Human Net";

	public override string EmailTitle => "Login help needed?";

	public override string EmailAddress => "TotallyHuman@bmail.ow";

	public override List<string> PersistentIdOfItem { get; }

	public override string IconPath => "Human-Resources-Network--Streamline-Ultimate";

	public override RARITY Rarity => RARITY.rare;

	public override bool UseInGame => false;

	public override byte GetIndex()
	{
		return GetIndex<NetworkHuman_10>();
	}

	public override string DealDescription()
	{
		return "Help a nice robot get into Spööktube";
	}

	public override void OnDestroy()
	{
	}

	public override NetworkDealBase CreateNew()
	{
		return new NetworkHuman_10();
	}

	public override void ReviewUploadContent(ContentBuffer contentBuffer)
	{
		base.ReviewUploadContent(contentBuffer);
		Debug.Log("ReviewUploadContent By NetworkHuman_10");
		foreach (ContentBuffer.BufferedContent item in contentBuffer.buffer)
		{
			if (item.frame.contentEvent is WeepingContentEventSuccess)
			{
				base.ProgressInt++;
			}
		}
	}

	public override string GetSuccessEmailBody()
	{
		string successEmailBody_Localized = base.SuccessEmailBody_Localized;
		if (!string.IsNullOrEmpty(successEmailBody_Localized))
		{
			return successEmailBody_Localized;
		}
		return "Thank you for helping our friend! login to Spööktube!";
	}

	public override string GetFailedEmailBody()
	{
		return "";
	}

	public override int RequiredAmount()
	{
		int num = 0;
		if (difficulty == DIFFICULTY.medium)
		{
			return 1;
		}
		throw new ArgumentOutOfRangeException();
	}

	public override void Update()
	{
	}
}
