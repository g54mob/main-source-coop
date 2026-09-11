using System;
using System.Collections.Generic;
using CurvedUI;
using UnityEngine;

public class NetworkBingoBongo_0 : NetworkDealBase
{
	public override List<DIFFICULTY> AllowedDifficulties => new List<DIFFICULTY>
	{
		DIFFICULTY.easy,
		DIFFICULTY.medium,
		DIFFICULTY.hard,
		DIFFICULTY.veryHard
	};

	public override string DealName => "Norf gun";

	public override string EmailTitle => "Norf sponsorship opportunity!";

	public override string EmailAddress => "Anita@BigNorf.nw";

	public override string IconPath => "Family-Mother--Streamline-Ultimate";

	public override bool UseInGame => true;

	public override RARITY Rarity => RARITY.common;

	public override List<string> PersistentIdOfItem => new List<string> { "0476de79-9616-41a9-8365-355aa0813f4a" };

	public override byte GetIndex()
	{
		return GetIndex<NetworkBingoBongo_0>();
	}

	public override string DealDescription()
	{
		string text = BigNumbers.ViewsToString(RequiredAmount());
		string description_Localized = base.Description_Localized;
		if (string.IsNullOrEmpty(description_Localized))
		{
			return "Make us an add for our brand new NORF GUN 2000!\n\nGet a total of <b>" + text + " views</b> without uploading any <b>dead bodies!</b> Kids don't like those.";
		}
		return description_Localized.Replace("{Views}", text).Replace("\\n", Environment.NewLine);
	}

	public override void OnDestroy()
	{
	}

	public override NetworkDealBase CreateNew()
	{
		return new NetworkBingoBongo_0();
	}

	public override void OnAddQuota(int quotaToAdd)
	{
		base.OnAddQuota(quotaToAdd);
		base.ProgressInt += BigNumbers.GetScoreToViews(quotaToAdd, GameAPI.CurrentDay);
	}

	public override string GetSuccessEmailBody()
	{
		string successEmailBody_Localized = base.SuccessEmailBody_Localized;
		if (string.IsNullOrEmpty(successEmailBody_Localized))
		{
			return $"You didn't upload any <b>dead bodies</b> and <b>got {RequiredAmount()} views!</b>";
		}
		return successEmailBody_Localized.Replace("{Views}", RequiredAmount().ToString());
	}

	public override void ReviewUploadContent(ContentBuffer contentBuffer)
	{
		base.ReviewUploadContent(contentBuffer);
		Debug.Log("Running By Bingo Bongo");
		foreach (ContentBuffer.BufferedContent item in contentBuffer.buffer)
		{
			if (item.frame.contentEvent is PlayerDeadContentEvent)
			{
				Debug.Log($"failed at {item.frame.contentEvent.GetName()} ID: {item.frame.contentEvent.GetID()}");
				base.State = DEAL_STATE.failed;
			}
		}
	}

	public override int RequiredAmount()
	{
		int num = 0;
		return difficulty switch
		{
			DIFFICULTY.easy => GetAmountByRun(1), 
			DIFFICULTY.medium => GetAmountByRun(2), 
			DIFFICULTY.hard => GetAmountByRun(3), 
			DIFFICULTY.veryHard => GetAmountByRun(4), 
			_ => throw new ArgumentOutOfRangeException(), 
		};
		static int GetAmountByRun(int run)
		{
			return BigNumbers.GetScoreToViewsFromRun(((float)BigNumbers.GetQuota(run) * 1.1f).ToInt(), run);
		}
	}

	public override void Update()
	{
	}

	public override string GetProgressText()
	{
		return BigNumbers.ViewsToString(base.ProgressInt) + "/" + BigNumbers.ViewsToString(RequiredAmount());
	}

	public override string GetFailedEmailBody()
	{
		string failedEmailBody_Localized = base.FailedEmailBody_Localized;
		if (string.IsNullOrEmpty(failedEmailBody_Localized))
		{
			return "Can you not read??! You uploaded a video with  a dead body!?\n\nWe told you <b>NOT to film any dead bodies</b> and you do just that?\n\nThe kids will never be the same, you know?";
		}
		return failedEmailBody_Localized.Replace("\\n", Environment.NewLine);
	}
}
