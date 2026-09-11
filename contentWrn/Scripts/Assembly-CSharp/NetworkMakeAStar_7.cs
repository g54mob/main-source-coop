using System;
using System.Collections.Generic;
using CurvedUI;
using UnityEngine;

public class NetworkMakeAStar_7 : NetworkDealBase
{
	public override List<DIFFICULTY> AllowedDifficulties => new List<DIFFICULTY>
	{
		DIFFICULTY.easy,
		DIFFICULTY.medium,
		DIFFICULTY.hard,
		DIFFICULTY.veryHard
	};

	public override string DealName => "Star Net";

	public override string EmailTitle => "We need a star";

	public override string EmailAddress => "";

	public override List<string> PersistentIdOfItem { get; }

	public override RARITY Rarity => RARITY.common;

	public override string IconPath => "Female-Star--Streamline-Ultimate";

	public override bool UseInGame => false;

	public override byte GetIndex()
	{
		return GetIndex<NetworkMakeAStar_7>();
	}

	public override string DealDescription()
	{
		BigNumbers.ViewsToString(RequiredAmount());
		return "Lots of shots of philip";
	}

	public override void OnDestroy()
	{
	}

	public override NetworkDealBase CreateNew()
	{
		return new NetworkMakeAStar_7();
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

	public override string GetSuccessEmailBody()
	{
		return "";
	}

	public override string GetFailedEmailBody()
	{
		return "";
	}

	public override int RequiredAmount()
	{
		int result = 0;
		switch (difficulty)
		{
		case DIFFICULTY.easy:
			result = GetAmountByRun(0);
			break;
		case DIFFICULTY.medium:
			result = GetAmountByRun(1);
			break;
		case DIFFICULTY.hard:
			result = GetAmountByRun(2);
			break;
		case DIFFICULTY.veryHard:
			result = GetAmountByRun(3);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case DIFFICULTY.veryEasy:
			break;
		}
		return result;
		static int GetAmountByRun(int run)
		{
			return BigNumbers.GetScoreToViewsFromRun(((float)BigNumbers.GetQuota(run) * 1.1f).ToInt(), run);
		}
	}

	public override void Update()
	{
	}
}
