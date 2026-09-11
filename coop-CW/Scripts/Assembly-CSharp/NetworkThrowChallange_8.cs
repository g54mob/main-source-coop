using System;
using System.Collections.Generic;
using System.Linq;
using CurvedUI;
using UnityEngine;

public class NetworkThrowChallange_8 : NetworkDealBase
{
	public override List<DIFFICULTY> AllowedDifficulties => new List<DIFFICULTY>
	{
		DIFFICULTY.easy,
		DIFFICULTY.medium,
		DIFFICULTY.hard,
		DIFFICULTY.veryHard
	};

	public override string DealName => "Thrower Net";

	public override string EmailTitle => "We need a thrower";

	public override string EmailAddress { get; }

	public override List<string> PersistentIdOfItem { get; }

	public override bool UseInGame => false;

	public override RARITY Rarity => RARITY.common;

	public override string IconPath => "Athletics-Javelin-Throwing--Streamline-Ultimate";

	public override byte GetIndex()
	{
		return GetIndex<NetworkThrowChallange_8>();
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
		return new NetworkThrowChallange_8();
	}

	public override void ReviewUploadContent(ContentBuffer contentBuffer)
	{
		base.ReviewUploadContent(contentBuffer);
		Debug.Log("Running By Bingo Bongo");
		foreach (ContentEvent item in contentBuffer.buffer.Select((ContentBuffer.BufferedContent content) => content.frame.contentEvent).ToList())
		{
			Debug.Log(item.GetName());
		}
		foreach (ContentBuffer.BufferedContent item2 in contentBuffer.buffer)
		{
			if (item2.frame.contentEvent is PlayerDeadContentEvent)
			{
				Debug.Log($"failed at {item2.frame.contentEvent.GetName()} ID: {item2.frame.contentEvent.GetID()}");
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
		int num = 0;
		return difficulty switch
		{
			DIFFICULTY.easy => GetAmountByRun(0), 
			DIFFICULTY.medium => GetAmountByRun(1), 
			DIFFICULTY.hard => GetAmountByRun(2), 
			DIFFICULTY.veryHard => GetAmountByRun(3), 
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
}
