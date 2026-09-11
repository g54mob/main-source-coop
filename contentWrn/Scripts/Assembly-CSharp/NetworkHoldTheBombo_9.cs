using System.Collections.Generic;
using UnityEngine;

public class NetworkHoldTheBombo_9 : NetworkDealBase
{
	public override List<DIFFICULTY> AllowedDifficulties => new List<DIFFICULTY> { DIFFICULTY.hard };

	public override string DealName => "HoldTheBombo";

	public override string EmailTitle => "IMPORTANT! Try our award winning helmet!";

	public override string EmailAddress => "Anna@HeadSafety.nw";

	public override string IconPath => "Catch-Bug--Streamline-Ultimate";

	public override RARITY Rarity => RARITY.rare;

	public override bool UseInGame => true;

	public override List<string> PersistentIdOfItem => new List<string> { "43d8e587-54e2-48ad-8256-62f45611cea4" };

	public override int RequiredAmount()
	{
		return 1;
	}

	public override byte GetIndex()
	{
		return GetIndex<NetworkHoldTheBombo_9>();
	}

	public override string DealDescription()
	{
		string description_Localized = base.Description_Localized;
		if (string.IsNullOrEmpty(description_Localized))
		{
			return "Upload a video of someone holding a bomb to prove how safe our helmet is!";
		}
		return description_Localized;
	}

	public override void OnDestroy()
	{
	}

	public override NetworkDealBase CreateNew()
	{
		return new NetworkHoldTheBombo_9();
	}

	public override void ReviewUploadContent(ContentBuffer contentBuffer)
	{
		base.ReviewUploadContent(contentBuffer);
		Debug.Log("ReviewUploadContent By NetworkHoldTheBombo_9");
		foreach (ContentBuffer.BufferedContent item in contentBuffer.buffer)
		{
			if (item.frame.contentEvent is BombContentEvent bombContentEvent)
			{
				Debug.Log($"bombContentEvent {item.frame.contentEvent.GetName()} IsHeld {bombContentEvent.isHeld}");
				if (bombContentEvent.isHeld)
				{
					Debug.Log($"Success! IsHeld {bombContentEvent.isHeld}");
					base.ProgressInt = 1;
				}
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
		return "You showed them the power of the bomb!";
	}

	public override string GetFailedEmailBody()
	{
		return "";
	}

	public override void Update()
	{
	}
}
