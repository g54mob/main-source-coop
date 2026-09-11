using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkJackass_4 : NetworkDealBase
{
	private float totalDamage;

	public override List<DIFFICULTY> AllowedDifficulties => new List<DIFFICULTY>
	{
		DIFFICULTY.easy,
		DIFFICULTY.medium,
		DIFFICULTY.hard,
		DIFFICULTY.veryHard
	};

	public override string DealName => "Donkey Net";

	public override string EmailTitle => "Energy drink colab!";

	public override string EmailAddress => "Fred@FredGull.nw";

	public override string IconPath => "Artificial-Arm--Streamline-Ultimate";

	public override RARITY Rarity => RARITY.common;

	public override bool UseInGame => true;

	public override List<string> PersistentIdOfItem => new List<string> { "8a4f690b-daa5-424b-ae43-572deb410edb" };

	public float TotalDamage
	{
		get
		{
			return totalDamage;
		}
		set
		{
			if (totalDamage != value)
			{
				totalDamage = value;
				base.ProgressInt = Mathf.RoundToInt(totalDamage);
			}
		}
	}

	public override byte GetIndex()
	{
		return GetIndex<NetworkJackass_4>();
	}

	public override string DealDescription()
	{
		string description_Localized = base.Description_Localized;
		if (string.IsNullOrEmpty(description_Localized))
		{
			return $"Our fans love seeing weirdos like you get hurt. \n\nUpload videos where you and your friends taking a total of <b>{RequiredAmount()} damage</b>";
		}
		return description_Localized.Replace("{amount}", RequiredAmount().ToString()).Replace("\\n", Environment.NewLine);
	}

	public override void OnDestroy()
	{
	}

	public override NetworkDealBase CreateNew()
	{
		return new NetworkJackass_4();
	}

	public override void ReviewUploadContent(ContentBuffer contentBuffer)
	{
		base.ReviewUploadContent(contentBuffer);
		Debug.Log("NetworkJackass_4");
		foreach (ContentBuffer.BufferedContent item in contentBuffer.buffer)
		{
			if (item.frame.contentEvent is PlayerTookDamageContentEvent playerTookDamageContentEvent)
			{
				Debug.LogError($"PlayerTookDamageContentEvent {playerTookDamageContentEvent.recentDamage}");
				TotalDamage += playerTookDamageContentEvent.recentDamage.damage;
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
		return "That looked really painful! Hope the medical bills aren't too high!";
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
			DIFFICULTY.easy => 200, 
			DIFFICULTY.medium => 400, 
			DIFFICULTY.hard => 800, 
			DIFFICULTY.veryHard => 1600, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
	}

	public override void Update()
	{
	}
}
