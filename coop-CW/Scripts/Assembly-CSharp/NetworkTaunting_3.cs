using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTaunting_3 : NetworkDealBase
{
	public override List<DIFFICULTY> AllowedDifficulties => new List<DIFFICULTY>
	{
		DIFFICULTY.easy,
		DIFFICULTY.medium,
		DIFFICULTY.hard,
		DIFFICULTY.veryHard
	};

	public override string DealName => "Brave Net";

	public override string EmailTitle => "DANCE VIDEO SPONSORSHIP?!";

	public override string EmailAddress => "Bob@Spöttiphy.nw";

	public override RARITY Rarity => RARITY.common;

	public override bool UseInGame => true;

	public override List<string> PersistentIdOfItem => new List<string> { "c9dea120-aae9-4cb7-ba8b-4ac9e6ae775c", "fd14e6aa-1e64-49f4-ad0d-e04dda6b8b9f" };

	public override string IconPath => "Party-Music-Dance-Woman--Streamline-Ultimate";

	public override byte GetIndex()
	{
		return GetIndex<NetworkTaunting_3>();
	}

	public override string DealDescription()
	{
		string description_Localized = base.Description_Localized;
		if (string.IsNullOrEmpty(description_Localized))
		{
			return $"perform our new dance in front of {RequiredAmount()} monsters with our hit song playing!";
		}
		return description_Localized.Replace("{Amount}", RequiredAmount().ToString());
	}

	public override void OnDestroy()
	{
	}

	public override NetworkDealBase CreateNew()
	{
		return new NetworkTaunting_3();
	}

	public override void ReviewUploadContent(ContentBuffer contentBuffer)
	{
		base.ReviewUploadContent(contentBuffer);
		Debug.Log("ReviewUploadContent By NetworkTaunting_3");
		foreach (ContentBuffer.BufferedContent item in contentBuffer.buffer)
		{
			if (item.frame.contentEvent is TauntEvent)
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
		return "Thank you for showing your bravery!";
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
			DIFFICULTY.medium => 4, 
			DIFFICULTY.hard => 6, 
			DIFFICULTY.veryHard => 8, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
	}

	public override void Update()
	{
	}
}
